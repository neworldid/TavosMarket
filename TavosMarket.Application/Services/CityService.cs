using Microsoft.EntityFrameworkCore;
using TavosMarket.Application.Interfaces;
using TavosMarket.Shared.Cities.DTOs;

namespace TavosMarket.Application.Services;

public class CityService(ITavosMarketDbContext dbContext)
{
	public async Task<List<CityDto>> GetCitiesAsync(string? search = null, CancellationToken ct = default)
	{
		var query = dbContext.Cities.AsNoTracking();

		if (!string.IsNullOrWhiteSpace(search))
		{
			query = query.Where(c => c.Name.Contains(search));
		}

		return await query
			.OrderBy(c => c.Name)
			.Select(c => new CityDto
			{
				Id = c.Id,
				Name = c.Name
			})
			.ToListAsync(ct);
	}
}
