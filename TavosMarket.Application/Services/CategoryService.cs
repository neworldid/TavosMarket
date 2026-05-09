using Microsoft.EntityFrameworkCore;
using TavosMarket.Application.Interfaces;
using TavosMarket.Domain.Entities;
using TavosMarket.Domain.Enums;
using TavosMarket.Shared.Categories.DTOs;
using TavosMarket.Shared.Enums;

namespace TavosMarket.Application.Services;

public class CategoryService(ITavosMarketDbContext dbContext)
{
	public async Task<List<CategoryDto>> GetCategoriesTreeAsync(CancellationToken cancellationToken = default)
	{
		var allCategories = await dbContext.Categories
			.Include(c => c.Parent)
			.Include(c => c.FieldDefinitions)
				.ThenInclude(fd => fd.Options)
			.OrderBy(c => c.SortOrder)
			.ToListAsync(cancellationToken);

		var rootCategories = allCategories
			.Where(c => c.ParentId == null)
			.Select(c => MapToDto(c, allCategories))
			.ToList();

		return rootCategories;
	}

	public async Task<CategoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var category = await dbContext.Categories
			.Include(c => c.Parent)
			.Include(c => c.FieldDefinitions)
				.ThenInclude(fd => fd.Options)
			.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

		if (category == null) return null;

		var fullName = category.Name;
		if (category.ParentId != null)
		{
			var names = await GetFullNamesAsync(cancellationToken);
			names.TryGetValue(category.Id, out fullName);
		}

		var dto = MapToDto(category, null);
		dto.FullName = fullName ?? category.Name;
		return dto;
	}

	public async Task<Dictionary<Guid, string>> GetFullNamesAsync(CancellationToken cancellationToken = default)
	{
		var allCategories = await dbContext.Categories
			.OrderBy(c => c.SortOrder)
			.ToListAsync(cancellationToken);

		var result = new Dictionary<Guid, string>();
		var roots = allCategories.Where(c => c.ParentId == null).ToList();

		foreach (var root in roots)
		{
			FillFullNames(root, allCategories, "", result);
		}

		return result;
	}

	private void FillFullNames(Category category, List<Category> all, string parentPath, Dictionary<Guid, string> result)
	{
		var currentPath = string.IsNullOrEmpty(parentPath) ? category.Name : $"{parentPath} > {category.Name}";
		result[category.Id] = currentPath;

		var children = all.Where(c => c.ParentId == category.Id).ToList();
		foreach (var child in children)
		{
			FillFullNames(child, all, currentPath, result);
		}
	}

	public async Task<CategoryDto> CreateAsync(CategoryDto categoryDto, CancellationToken cancellationToken = default)
	{
		var category = new Category
		{
			Name = categoryDto.Name,
			Description = categoryDto.Description,
			ParentId = categoryDto.ParentId,
			SortOrder = categoryDto.SortOrder,
			IsActive = categoryDto.IsActive,
			IsDirectUseForListings = categoryDto.IsDirectUseForListings
		};

		foreach (var fdDto in categoryDto.FieldDefinitions)
		{
			var fd = MapToEntity(fdDto);
			category.FieldDefinitions.Add(fd);
		}

		dbContext.Categories.Add(category);
		await dbContext.SaveChangesAsync(cancellationToken);

		return MapToDto(category, null);
	}

	public async Task<CategoryDto> UpdateAsync(CategoryDto categoryDto, CancellationToken cancellationToken = default)
	{
		var category = await dbContext.Categories
			.Include(c => c.FieldDefinitions)
				.ThenInclude(fd => fd.Options)
			.FirstOrDefaultAsync(c => c.Id == categoryDto.Id, cancellationToken);

		if (category == null) throw new InvalidOperationException("Category not found");

		category.Name = categoryDto.Name;
		category.Description = categoryDto.Description;
		category.ParentId = categoryDto.ParentId;
		category.SortOrder = categoryDto.SortOrder;
		category.IsActive = categoryDto.IsActive;
		category.IsDirectUseForListings = categoryDto.IsDirectUseForListings;

		// Sync FieldDefinitions
		var existingFds = category.FieldDefinitions.ToList();
		var dtoFds = categoryDto.FieldDefinitions;

		foreach (var existingFd in existingFds)
		{
			if (dtoFds.All(d => d.Id != existingFd.Id))
			{
				dbContext.CategoryFieldDefinitions.Remove(existingFd);
			}
		}

		foreach (var fdDto in dtoFds)
		{
			var existingFd = existingFds.FirstOrDefault(f => f.Id == fdDto.Id);
			if (existingFd != null)
			{
				existingFd.Name = fdDto.Name;
				existingFd.Description = fdDto.Description;
				existingFd.DataType = (FieldDataType)fdDto.DataTypeDto;
				existingFd.IsRequired = fdDto.IsRequired;
				existingFd.IsFilterable = fdDto.IsFilterable;
				existingFd.SortOrder = fdDto.SortOrder;
				existingFd.Unit = fdDto.Unit;
				existingFd.Placeholder = fdDto.Placeholder;
				existingFd.MinValue = fdDto.MinValue;
				existingFd.MaxValue = fdDto.MaxValue;

				SyncOptions(existingFd, fdDto.Options);
			}
			else
			{
				category.FieldDefinitions.Add(MapToEntity(fdDto));
			}
		}

		await dbContext.SaveChangesAsync(cancellationToken);
		return MapToDto(category, null);
	}

	private void SyncOptions(CategoryFieldDefinition fd, List<CategoryFieldOptionDto> dtoOptions)
	{
		var existingOptions = fd.Options.ToList();
		
		foreach (var existingOpt in existingOptions)
		{
			if (dtoOptions.All(o => o.Id != existingOpt.Id))
			{
				dbContext.CategoryFieldOptions.Remove(existingOpt);
			}
		}

		foreach (var optDto in dtoOptions)
		{
			var existingOpt = existingOptions.FirstOrDefault(o => o.Id == optDto.Id);
			if (existingOpt != null)
			{
				existingOpt.Value = optDto.Value;
				existingOpt.Label = optDto.Value;
				existingOpt.SortOrder = optDto.SortOrder;
			}
			else
			{
				fd.Options.Add(new CategoryFieldOption
				{
					Id = Guid.NewGuid(),
					Value = optDto.Value,
					Label = optDto.Value,
					SortOrder = optDto.SortOrder
				});
			}
		}
	}

	public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var category = await dbContext.Categories.FindAsync([id], cancellationToken);
		if (category != null)
		{
			dbContext.Categories.Remove(category);
			await dbContext.SaveChangesAsync(cancellationToken);
		}
	}

	public async Task<List<Guid>> GetCategoryIdsRecursiveAsync(Guid categoryId, CancellationToken cancellationToken = default)
	{
		var allCategories = await dbContext.Categories
			.Select(c => new { c.Id, c.ParentId })
			.ToListAsync(cancellationToken);

		var result = new List<Guid>();
		FillIdsRecursive(categoryId, allCategories.Select(c => (c.Id, c.ParentId)).ToList(), result);
		return result;
	}

	private void FillIdsRecursive(Guid parentId, List<(Guid Id, Guid? ParentId)> all, List<Guid> result)
	{
		result.Add(parentId);
		var children = all.Where(c => c.ParentId == parentId).Select(c => c.Id).ToList();
		foreach (var childId in children)
		{
			FillIdsRecursive(childId, all, result);
		}
	}

	private static CategoryDto MapToDto(Category category, List<Category>? allCategories, string parentFullName = "")
	{
		var fullName = string.IsNullOrEmpty(parentFullName) ? category.Name : $"{parentFullName} > {category.Name}";
		var dto = new CategoryDto
		{
			Id = category.Id,
			Name = category.Name,
			FullName = fullName,
			Description = category.Description,
			ParentId = category.ParentId,
			ParentName = category.Parent?.Name,
			SortOrder = category.SortOrder,
			IsActive = category.IsActive,
			IsDirectUseForListings = category.IsDirectUseForListings,
			FieldDefinitions = category.FieldDefinitions
				.OrderBy(fd => fd.SortOrder)
				.Select(fd => new CategoryFieldDefinitionDto
				{
					Id = fd.Id,
					Name = fd.Name,
					Description = fd.Description,
					DataTypeDto = (FieldDataTypeDto)fd.DataType,
					IsRequired = fd.IsRequired,
					IsFilterable = fd.IsFilterable,
					SortOrder = fd.SortOrder,
					Unit = fd.Unit,
					Placeholder = fd.Placeholder,
					MinValue = fd.MinValue,
					MaxValue = fd.MaxValue,
					Options = fd.Options
						.OrderBy(o => o.SortOrder)
						.Select(o => new CategoryFieldOptionDto
						{
							Id = o.Id,
							Value = o.Value,
							SortOrder = o.SortOrder
						}).ToList()
				}).ToList()
		};

		if (allCategories != null)
		{
			dto.Children = allCategories
				.Where(c => c.ParentId == category.Id)
				.Select(c => MapToDto(c, allCategories, fullName))
				.ToList();
		}

		return dto;
	}

	private static CategoryFieldDefinition MapToEntity(CategoryFieldDefinitionDto dto)
	{
		var fd = new CategoryFieldDefinition
		{
			Name = dto.Name,
			Description = dto.Description,
			DataType = (FieldDataType)dto.DataTypeDto,
			IsRequired = dto.IsRequired,
			IsFilterable = dto.IsFilterable,
			SortOrder = dto.SortOrder,
			Unit = dto.Unit,
			Placeholder = dto.Placeholder,
			MinValue = dto.MinValue,
			MaxValue = dto.MaxValue
		};

		foreach (var optDto in dto.Options)
		{
			fd.Options.Add(new CategoryFieldOption
			{
				Value = optDto.Value,
				Label = optDto.Value,
				SortOrder = optDto.SortOrder
			});
		}

		return fd;
	}
}
