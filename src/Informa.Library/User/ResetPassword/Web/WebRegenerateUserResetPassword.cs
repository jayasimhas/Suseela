using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.ResetPassword.Web
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class WebRegenerateUserResetPassword : IWebRegenerateUserResetPassword
	{
		protected readonly IFindUserResetPassword FindUserResetPassword;
		protected readonly IWebGenerateUserResetPassword GenerateUserResetPassword;

		public WebRegenerateUserResetPassword(
			IFindUserResetPassword findUserResetPassword,
			IWebGenerateUserResetPassword generateUserResetPassword)
		{
			FindUserResetPassword = findUserResetPassword;
			GenerateUserResetPassword = generateUserResetPassword;
		}

		public IWebGenerateUserResetPasswordResult Regenerate(string token)
		{
			var userResetPassword = FindUserResetPassword.Find(token);

			return GenerateUserResetPassword.Generate(userResetPassword?.Username ?? string.Empty);
		}
	}
}
