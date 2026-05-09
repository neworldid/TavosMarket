namespace TavosMarket.Shared;

public static class ApiRoutes
{
	public static class Auth
	{
		public const string Login = "/api/auth/login";
		
		public const string Register = "/api/auth/register";
		public const string CurrentUser = "/api/auth/current-user";
		public const string Logout = "/api/auth/logout";
		public const string ChangePassword = "/api/auth/change-password";
		public const string ForgotPassword = "/api/auth/forgot-password";
		public const string ResetPassword = "/api/auth/reset-password";
		public const string GoogleLogin = "/api/auth/google-login";
	}

	public static class UserProfile
	{
		public const string Base = "/api/user-profile";
		public const string Update = "/api/user-profile";
	}

	public static class Categories
	{
		public const string Base = "/api/categories";
		public const string GetById = "/api/categories/{id}";
		public const string Admin = "/api/categories/admin";
		public const string AdminById = "/api/categories/admin/{id}";
		public const string AdminReorder = "/api/categories/admin/{id}/reorder";
	}

	public static class Listings
	{
		public const string Base = "/api/listings";
		public const string GetById = "/api/listings/{id}";
		public const string Create = "/api/listings";
		public const string Update = "/api/listings/{id}";
		public const string Delete = "/api/listings/{id}";
		public const string MyListings = "/api/listings/my";
	}

	public static class Stats
	{
		public const string Public = "/api/stats/public";
		public const string User = "/api/stats/user";
		public const string Admin = "/api/stats/admin";
	}
}