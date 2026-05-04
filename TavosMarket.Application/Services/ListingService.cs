using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using TavosMarket.Application.Interfaces;
using TavosMarket.Application.Auth;
using TavosMarket.Domain.Entities;
using TavosMarket.Domain.Enums;
using TavosMarket.Shared.Listings.DTOs;
using TavosMarket.Shared.Categories.DTOs;
using TavosMarket.Shared.Enums;

namespace TavosMarket.Application.Services;

public class ListingService(
    ITavosMarketDbContext dbContext, 
    ICurrentUserService currentUserService,
    IWebHostEnvironment environment)
{
    public async Task<List<ListingDto>> GetListingsAsync(ListingFilterDto filter, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Listings
            .Include(l => l.Category)
            .Include(l => l.Images)
            .AsQueryable();

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(l => l.CategoryId == filter.CategoryId.Value);
        }

        if (filter.MinPrice.HasValue)
        {
            query = query.Where(l => l.Price >= filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            query = query.Where(l => l.Price <= filter.MaxPrice.Value);
        }

        if (filter.FieldFilters != null && filter.FieldFilters.Any())
        {
            var fieldIds = filter.FieldFilters.Keys.ToList();
            var fieldDefinitions = await dbContext.CategoryFieldDefinitions
                .Where(fd => fieldIds.Contains(fd.Id))
                .ToListAsync(cancellationToken);

            foreach (var fieldFilter in filter.FieldFilters)
            {
                var fieldId = fieldFilter.Key;
                var value = fieldFilter.Value;
                if (string.IsNullOrWhiteSpace(value)) continue;

                var fd = fieldDefinitions.FirstOrDefault(f => f.Id == fieldId);
                if (fd == null) continue;

                switch (fd.DataType)
                {
                    case FieldDataType.Select:
                    case FieldDataType.MultiSelect:
                        if (Guid.TryParse(value, out var guidVal))
                            query = query.Where(l => l.FieldValues.Any(fv => fv.FieldDefinitionId == fieldId && (fv.OptionId == guidVal || fv.SelectedOptions.Any(so => so.Id == guidVal))));
                        break;
                    case FieldDataType.Integer:
                        if (int.TryParse(value, out var intVal))
                            query = query.Where(l => l.FieldValues.Any(fv => fv.FieldDefinitionId == fieldId && fv.IntValue == intVal));
                        break;
                    case FieldDataType.Decimal:
                        if (decimal.TryParse(value, out var decVal))
                            query = query.Where(l => l.FieldValues.Any(fv => fv.FieldDefinitionId == fieldId && fv.DecimalValue == decVal));
                        break;
                    case FieldDataType.Boolean:
                        if (bool.TryParse(value, out var boolVal))
                            query = query.Where(l => l.FieldValues.Any(fv => fv.FieldDefinitionId == fieldId && fv.BoolValue == boolVal));
                        break;
                    default:
                        query = query.Where(l => l.FieldValues.Any(fv => fv.FieldDefinitionId == fieldId && fv.StringValue != null && fv.StringValue.Contains(value)));
                        break;
                }
            }
        }

        var listings = await query
            .OrderByDescending(l => l.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return listings.Select(MapToDto).ToList();
    }

    public async Task<List<ListingDto>> GetCurrentUserListingsAsync(CancellationToken cancellationToken = default)
    {
        var userId = currentUserService.UserId;
        if (userId == null) return [];

        var listings = await dbContext.Listings
            .Include(l => l.Category)
            .Include(l => l.Images)
            .Where(l => l.SellerId == userId.Value)
            .OrderByDescending(l => l.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return listings.Select(MapToDto).ToList();
    }

    public async Task<ListingDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var listing = await dbContext.Listings
            .Include(l => l.Category)
            .Include(l => l.Images)
            .Include(l => l.FieldValues)
                .ThenInclude(fv => fv.FieldDefinition)
                    .ThenInclude(fd => fd.Options)
            .Include(l => l.FieldValues)
                .ThenInclude(fv => fv.SelectedOptions)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

        return listing != null ? MapToDto(listing) : null;
    }

    public async Task<ListingDto> CreateAsync(ListingDto dto, CancellationToken cancellationToken = default)
    {
        var listing = new Listing
        {
            Id = Guid.NewGuid(),
            SellerId = currentUserService.UserId ?? Guid.Empty,
            CategoryId = dto.CategoryId,
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            IsNegotiable = dto.IsNegotiable,
            City = dto.City,
            Region = dto.Region,
            Status = ListingStatus.Published // Defaulting to published for now
        };

        foreach (var imgDto in dto.Images)
        {
            var url = imgDto.Url;
            if (imgDto.Data != null && !string.IsNullOrEmpty(imgDto.FileName))
            {
                url = await SaveImageAsync(imgDto.Data, imgDto.FileName);
            }

            listing.Images.Add(new ListingImage
            {
                Url = url,
                ThumbnailUrl = imgDto.ThumbnailUrl,
                SortOrder = imgDto.SortOrder,
                IsMain = imgDto.IsMain
            });
        }

        var allOptionIds = dto.FieldValues.SelectMany(fv => fv.OptionIds).Distinct().ToList();
        var allOptions = await dbContext.CategoryFieldOptions
            .Where(o => allOptionIds.Contains(o.Id))
            .ToListAsync(cancellationToken);

        foreach (var fvDto in dto.FieldValues)
        {
            var fv = new ListingFieldValue
            {
                FieldDefinitionId = fvDto.FieldDefinitionId,
                StringValue = fvDto.StringValue,
                IntValue = fvDto.IntValue,
                DecimalValue = fvDto.DecimalValue,
                BoolValue = fvDto.BoolValue,
                DateValue = fvDto.DateValue,
                OptionId = fvDto.OptionId
            };

            if (fvDto.OptionIds.Any())
            {
                fv.SelectedOptions = allOptions.Where(o => fvDto.OptionIds.Contains(o.Id)).ToList();
            }

            listing.FieldValues.Add(fv);
        }

        dbContext.Listings.Add(listing);
        await dbContext.SaveChangesAsync(cancellationToken);

        var createdListing = await dbContext.Listings
	        .Include(l => l.Category)
	        .Include(l => l.Images)
	        .Include(l => l.FieldValues)
	        .ThenInclude(fv => fv.FieldDefinition)
	        .ThenInclude(fd => fd.Options)
	        .Include(l => l.FieldValues)
	        .ThenInclude(fv => fv.SelectedOptions)
	        .FirstAsync(l => l.Id == listing.Id, cancellationToken);

        return MapToDto(createdListing);
    }

    public async Task UpdateAsync(Guid id, ListingDto dto, CancellationToken cancellationToken = default)
    {
	    var listing = await dbContext.Listings
		    .Include(l => l.Images)
		    .Include(l => l.FieldValues)
		    .ThenInclude(fv => fv.SelectedOptions)
		    .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

        if (listing == null) return;

        // Check ownership
        if (currentUserService.UserId != listing.SellerId)
        {
            throw new UnauthorizedAccessException("You can only update your own listings.");
        }

        listing.CategoryId = dto.CategoryId;
        listing.Title = dto.Title;
        listing.Description = dto.Description;
        listing.Price = dto.Price;
        listing.IsNegotiable = dto.IsNegotiable;
        listing.City = dto.City;
        listing.Region = dto.Region;

        // Update Images
        dbContext.ListingImages.RemoveRange(listing.Images);

        foreach (var imgDto in dto.Images)
        {
            var url = imgDto.Url;
            if (imgDto.Data != null && !string.IsNullOrEmpty(imgDto.FileName))
            {
                url = await SaveImageAsync(imgDto.Data, imgDto.FileName);
            }

            listing.Images.Add(new ListingImage
            {
                Url = url,
                ThumbnailUrl = imgDto.ThumbnailUrl,
                SortOrder = imgDto.SortOrder,
                IsMain = imgDto.IsMain
            });
        }

        // Update Field Values
        foreach (var fieldValue in listing.FieldValues)
        {
	        fieldValue.SelectedOptions.Clear();
        }
        dbContext.ListingFieldValues.RemoveRange(listing.FieldValues);

        var allOptionIds = dto.FieldValues.SelectMany(fv => fv.OptionIds).Distinct().ToList();
        var allOptions = await dbContext.CategoryFieldOptions
            .Where(o => allOptionIds.Contains(o.Id))
            .ToListAsync(cancellationToken);

        foreach (var fvDto in dto.FieldValues)
        {
            var fv = new ListingFieldValue
            {
                FieldDefinitionId = fvDto.FieldDefinitionId,
                StringValue = fvDto.StringValue,
                IntValue = fvDto.IntValue,
                DecimalValue = fvDto.DecimalValue,
                BoolValue = fvDto.BoolValue,
                DateValue = fvDto.DateValue,
                OptionId = fvDto.OptionId
            };

            if (fvDto.OptionIds.Any())
            {
                fv.SelectedOptions = allOptions.Where(o => fvDto.OptionIds.Contains(o.Id)).ToList();
            }

            listing.FieldValues.Add(fv);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
	    await dbContext.ListingFieldValues
		    .Where(x => x.ListingId == id)
		    .ExecuteDeleteAsync(cancellationToken);
	    
        var listing = await dbContext.Listings.FindAsync([id], cancellationToken);
        if (listing == null) return;

        if (currentUserService.UserId != listing.SellerId)
        {
            throw new UnauthorizedAccessException("You can only delete your own listings.");
        }

        dbContext.Listings.Remove(listing);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<string> SaveImageAsync(byte[] data, string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        var uploadsFolder = Path.Combine(environment.WebRootPath, "uploads", "listings");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var newFileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsFolder, newFileName);

        await File.WriteAllBytesAsync(filePath, data);

        return $"/uploads/listings/{newFileName}";
    }

    private static ListingDto MapToDto(Listing listing)
    {
        return new ListingDto
        {
            Id = listing.Id,
            SellerId = listing.SellerId,
            CategoryId = listing.CategoryId,
            Title = listing.Title,
            Description = listing.Description,
            Price = listing.Price,
            IsNegotiable = listing.IsNegotiable,
            City = listing.City,
            Region = listing.Region,
            Status = (ListingStatusDto)listing.Status,
            CreatedAtUtc = listing.CreatedAtUtc,
            Category = new CategoryDto { Id = listing.Category.Id, Name = listing.Category.Name },
            Images = listing.Images.Select(i => new ListingImageDto
            {
                Id = i.Id,
                Url = i.Url,
                ThumbnailUrl = i.ThumbnailUrl,
                SortOrder = i.SortOrder,
                IsMain = i.IsMain
            }).OrderBy(i => i.SortOrder).ToList(),
            FieldValues = listing.FieldValues.Select(fv => new ListingFieldValueDto
            {
                Id = fv.Id,
                FieldDefinitionId = fv.FieldDefinitionId,
                StringValue = fv.StringValue,
                IntValue = fv.IntValue,
                DecimalValue = fv.DecimalValue,
                BoolValue = fv.BoolValue,
                DateValue = fv.DateValue,
                OptionId = fv.OptionId,
                OptionIds = fv.SelectedOptions.Select(so => so.Id).ToList(),
                FieldDefinition = new CategoryFieldDefinitionDto
                {
	                Id = fv.FieldDefinition.Id,
	                Name = fv.FieldDefinition.Name,
	                DataTypeDto = (FieldDataTypeDto)fv.FieldDefinition.DataType,
	                IsRequired = fv.FieldDefinition.IsRequired,
	                Unit = fv.FieldDefinition.Unit,
	                Placeholder = fv.FieldDefinition.Placeholder,
	                Options = fv.FieldDefinition.Options.Select(o => new CategoryFieldOptionDto
	                {
		                Id = o.Id,
		                Value = o.Value,
		                SortOrder = o.SortOrder
	                }).ToList()
                }
            }).ToList()
        };
    }
}
