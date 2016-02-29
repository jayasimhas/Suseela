namespace Informa.Library.User.ResetPassword.Web
{
	public class WebGenerateUserResetPasswordResult : IWebGenerateUserResetPasswordResult
	{
		public WebGenerateUserResetPasswordStatus Status { get; set; }
		public string Token { get; set; }
	}
}
