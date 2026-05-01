using Microsoft.EntityFrameworkCore;
using TavosMarket.Application.Interfaces;
using TavosMarket.Application.Auth;
using TavosMarket.Domain.Entities;
using TavosMarket.Shared.Listings.DTOs;
using TavosMarket.Shared.Categories.DTOs;
using TavosMarket.Shared.Enums;

namespace TavosMarket.Application.Services;

public class ListingService(ITavosMarketDbContext dbContext, ICurrentUserService currentUserService)
{
    public async Task<List<ListingDto>> GetListingsAsync(Guid? categoryId = null, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Listings
            .Include(l => l.Category)
            .Include(l => l.Images)
            .AsQueryable();

        if (categoryId.HasValue)
        {
            query = query.Where(l => l.CategoryId == categoryId.Value);
        }

        var listings = await query
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
            listing.Images.Add(new ListingImage
            {
                Url = imgDto.Url,
                ThumbnailUrl = imgDto.ThumbnailUrl,
                SortOrder = imgDto.SortOrder,
                IsMain = imgDto.IsMain
            });
        }

        foreach (var fvDto in dto.FieldValues)
        {
            listing.FieldValues.Add(new ListingFieldValue
            {
                FieldDefinitionId = fvDto.FieldDefinitionId,
                StringValue = fvDto.StringValue,
                IntValue = fvDto.IntValue,
                DecimalValue = fvDto.DecimalValue,
                BoolValue = fvDto.BoolValue,
                DateValue = fvDto.DateValue,
                OptionId = fvDto.OptionId
            });
        }

        dbContext.Listings.Add(listing);
        await dbContext.SaveChangesAsync(cancellationToken);

        var createdListing = await dbContext.Listings
	        .Include(l => l.Category)
	        .Include(l => l.Images)
	        .Include(l => l.FieldValues)
	        .ThenInclude(fv => fv.FieldDefinition)
	        .ThenInclude(fd => fd.Options)
	        .FirstAsync(l => l.Id == listing.Id, cancellationToken);

        return MapToDto(createdListing);
    }

    private ListingDto MapToDto(Listing listing)
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
