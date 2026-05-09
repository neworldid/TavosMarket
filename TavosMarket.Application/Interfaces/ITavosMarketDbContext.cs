using Microsoft.EntityFrameworkCore;
using TavosMarket.Domain.Entities;

namespace TavosMarket.Application.Interfaces;

public interface ITavosMarketDbContext
{
	public DbSet<Listing> Listings { get; }
	public DbSet<Category> Categories { get; }
	public DbSet<City> Cities { get; }
	public DbSet<CategoryFieldDefinition> CategoryFieldDefinitions { get; }
	public DbSet<CategoryFieldOption> CategoryFieldOptions { get; }
	public DbSet<ListingFieldValue> ListingFieldValues { get; }
	public DbSet<ListingImage> ListingImages { get; }
	public DbSet<ListingFavorite> ListingFavorites { get; }
	

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}