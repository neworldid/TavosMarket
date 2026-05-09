using Microsoft.AspNetCore.Mvc;
using TavosMarket.Application.Services;
using TavosMarket.Shared;
using TavosMarket.Shared.Categories.DTOs;

namespace TavosMarket.Api.Endpoints;

public class CategoryEndpoint : IEndpoint
{
	public static void MapEndpoint(IEndpointRouteBuilder builder)
	{
		var group = builder.MapGroup("");
		
		group.MapGet(ApiRoutes.Categories.Base, GetTree);
		group.MapGet(ApiRoutes.Categories.GetById, GetById);
		
		var adminGroup = builder.MapGroup("").RequireAuthorization(policy => policy.RequireRole("Admin"));
		
		adminGroup.MapPost(ApiRoutes.Categories.Admin, Create);
		adminGroup.MapPut(ApiRoutes.Categories.AdminById, Update);
		adminGroup.MapDelete(ApiRoutes.Categories.AdminById, Delete);
		adminGroup.MapPost(ApiRoutes.Categories.AdminReorder, Reorder);
	}

	private static async Task<IResult> GetTree(CategoryService categoryService, CancellationToken ct)
	{
		return TypedResults.Ok(await categoryService.GetCategoriesTreeAsync(ct));
	}

	private static async Task<IResult> GetById(Guid id, CategoryService categoryService, CancellationToken ct)
	{
		var category = await categoryService.GetByIdAsync(id, ct);
		return category is not null ? TypedResults.Ok(category) : TypedResults.NotFound();
	}

	private static async Task<IResult> Create([FromBody] CategoryDto request, CategoryService categoryService, CancellationToken ct)
	{
		var result = await categoryService.CreateAsync(request, ct);
		return TypedResults.Created($"{ApiRoutes.Categories.Base}/{result.Id}", result);
	}

	private static async Task<IResult> Update(Guid id, [FromBody] CategoryDto request, CategoryService categoryService, CancellationToken ct)
	{
		if (id != request.Id) return TypedResults.BadRequest("ID mismatch");
		var result = await categoryService.UpdateAsync(request, ct);
		return TypedResults.Ok(result);
	}

	private static async Task<IResult> Delete(Guid id, CategoryService categoryService, CancellationToken ct)
	{
		await categoryService.DeleteAsync(id, ct);
		return TypedResults.NoContent();
	}

	private static async Task<IResult> Reorder(Guid id, [FromQuery] bool moveUp, CategoryService categoryService, CancellationToken ct)
	{
		await categoryService.ReorderAsync(id, moveUp, ct);
		return TypedResults.Ok();
	}
}
