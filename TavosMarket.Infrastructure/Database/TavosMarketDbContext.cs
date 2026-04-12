using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TavosMarket.Infrastructure.Identity;

namespace TavosMarket.Infrastructure.Database;

public class TavosMarketDbContext(DbContextOptions<TavosMarketDbContext> options) 
	: IdentityDbContext<ApplicationUser, Microsoft.AspNetCore.Identity.IdentityRole<Guid>, Guid>(options)
{
	
}