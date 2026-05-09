using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TavosMarket.Application.Auth;
using TavosMarket.Application.Interfaces;
using TavosMarket.Domain.Entities;
using TavosMarket.Infrastructure.Identity;

namespace TavosMarket.Infrastructure.Database;

public class TavosMarketDbContext(DbContextOptions<TavosMarketDbContext> options, ICurrentUserService currentUserService) 
	: IdentityDbContext<ApplicationUser, Microsoft.AspNetCore.Identity.IdentityRole<Guid>, Guid>(options), ITavosMarketDbContext
{
	public DbSet<Listing> Listings { get; set; }
	public DbSet<Category> Categories { get; set; }
	public DbSet<City> Cities { get; set; }
	public DbSet<CategoryFieldDefinition> CategoryFieldDefinitions { get; set; }
	public DbSet<CategoryFieldOption> CategoryFieldOptions { get; set; }
	public DbSet<ListingFieldValue> ListingFieldValues { get; set; }
	public DbSet<ListingImage> ListingImages { get; set; }
	public DbSet<ListingFavorite> ListingFavorites { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<ListingFieldValue>()
			.HasMany(x => x.SelectedOptions)
			.WithMany()
			.UsingEntity<Dictionary<string, object>>(
				"ListingFieldValueSelectedOption",
				j => j.HasOne<CategoryFieldOption>().WithMany().OnDelete(DeleteBehavior.NoAction),
				j => j.HasOne<ListingFieldValue>().WithMany().OnDelete(DeleteBehavior.NoAction));

		SeedCities(modelBuilder);
		modelBuilder.Entity<City>(entity =>
		{
			entity.Property(x => x.Name)
				.HasMaxLength(128)
				.UseCollation("Latvian_CI_AI");
		});
	}

	private void SeedCities(ModelBuilder modelBuilder)
	{
		var cities = new[]
		{
			"Rīga", "Daugavpils", "Liepāja", "Jelgava", "Jūrmala", "Ventspils", "Rēzekne", "Valmiera", "Jēkabpils", "Ogre",
			"Salaspils", "Mārupe", "Tukums", "Cēsis", "Sigulda", "Olaine", "Kuldīga", "Bauska", "Salacgrīva", "Limbaži",
			"Talsi", "Dobele", "Ludza", "Madona", "Gulbene", "Alūksne", "Krāslava", "Aizkraukle", "Preiļi", "Balvi"
		};

		var cityEntities = cities.Select(name => new City
		{
			Id = Guid.Parse(GenerateGuidFromName(name)),
			Name = name
		}).ToArray();

		modelBuilder.Entity<City>().HasData(cityEntities);
	}

	private string GenerateGuidFromName(string name)
	{
		using var md5 = System.Security.Cryptography.MD5.Create();
		byte[] hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(name));
		return new Guid(hash).ToString();
	}
	
	public override int SaveChanges()
	{
		ApplyAuditInfo();
		return base.SaveChanges();
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		ApplyAuditInfo();
		return base.SaveChangesAsync(cancellationToken);
	}

	private void ApplyAuditInfo()
	{
		var now = DateTime.UtcNow;
		var userId = currentUserService.UserId;

		foreach (var entry in ChangeTracker.Entries<AuditableEntityBase>())
		{
			if (entry.State == EntityState.Added)
			{
				entry.Entity.CreatedAtUtc = now;
				entry.Entity.CreatedById = userId;
			}

			if (entry.State == EntityState.Modified)
			{
				entry.Entity.UpdatedAtUtc = now;
				entry.Entity.UpdatedById = userId;
			}
		}
	}
}