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
}