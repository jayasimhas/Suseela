using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.ResetPassword.Web
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class WebGenerateUserResetPassword : IWebGenerateUserResetPassword
	{
		protected readonly IFindUserByEmail FindUser;
		protected readonly IGenerateUserResetPassword GenerateUserResetPassword;
		protected readonly IWebGenerateUserResetPasswordActions Actions;

		public WebGenerateUserResetPassword(
			IFindUserByEmail findUser,
			IGenerateUserResetPassword generateUserResetPassword,
			IWebGenerateUserResetPasswordActions actions)
		{
			FindUser = findUser;
			GenerateUserResetPassword = generateUserResetPassword;
			Actions = actions;
		}

		public IWebGenerateUserResetPasswordResult Generate(string email)
		{
			var user = FindUser.Find(email);

			if (user == null)
			{
				return CreateResult(WebGenerateUserResetPasswordStatus.UserNotFound);
			}

			var userResetPassword = GenerateUserResetPassword.Generate(user);

			if (userResetPassword == null)
			{
				return CreateResult(WebGenerateUserResetPasswordStatus.Failure);
			}

			Actions.Process(userResetPassword);

			var result = CreateResult(WebGenerateUserResetPasswordStatus.Success);

			result.Token = userResetPassword.Token;

			return result;
		}

		public WebGenerateUserResetPasswordResult CreateResult(WebGenerateUserResetPasswordStatus status)
		{
			return new WebGenerateUserResetPasswordResult
			{
				Status = status
			};
		}
	}
}
