using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TavosMarket.Api;
using TavosMarket.Application.Auth;
using TavosMarket.Infrastructure.Auth;
using TavosMarket.Infrastructure.Database;
using TavosMarket.Application.Interfaces;
using TavosMarket.Application.Services;
using TavosMarket.Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<TavosMarketDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
	.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
	.AddEntityFrameworkStores<TavosMarketDbContext>()
	.AddDefaultTokenProviders();

builder.Services.AddScoped<ITavosMarketDbContext>(provider =>
	provider.GetRequiredService<TavosMarketDbContext>());

builder.Services.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(options =>
	{
		var jwtSection = builder.Configuration.GetSection("Jwt");
		var key = jwtSection["Key"]!;
		var issuer = jwtSection["Issuer"]!;
		var audience = jwtSection["Audience"]!;

		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateIssuerSigningKey = true,
			ValidateLifetime = true,
			ValidIssuer = issuer,
			ValidAudience = audience,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
			ClockSkew = TimeSpan.FromSeconds(30)
		};
	});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ListingService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddCors(options =>
{
	options.AddPolicy("Client", policy =>
	{
		policy.WithOrigins("https://localhost:7137")
			.AllowAnyHeader()
			.AllowAnyMethod();
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("Client");
app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();

await EnsureRolesAsync(app);

app.Run();

static async Task EnsureRolesAsync(WebApplication app)
{
	using var scope = app.Services.CreateScope();

	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
	var roles = new[] { "User", "Admin" };

	foreach (var role in roles)
	{
		if (!await roleManager.RoleExistsAsync(role))
			await roleManager.CreateAsync(new IdentityRole<Guid>(role));
	}
}