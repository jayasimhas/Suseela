using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.ResetPassword
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ProcessUserResetPassword : IProcessUserResetPassword
	{
		protected readonly IFindValidUserResetPassword FindUserResetPassword;
		protected readonly IFindUserByUsername FindUserByUsername;
		protected readonly IUpdateUserPassword UpdateUserPassword;

		public ProcessUserResetPassword(
			IFindValidUserResetPassword findUserResetPassword,
			IFindUserByUsername findUserByUsername,
			IUpdateUserPassword updateUserPassword)
		{
			FindUserResetPassword = findUserResetPassword;
			FindUserByUsername = findUserByUsername;
			UpdateUserPassword = updateUserPassword;
		}

		public IProcessUserResetPasswordResult Process(string token, string newPassword)
		{
			IUserResetPassword userResetPassword;

			if (string.IsNullOrWhiteSpace(token) || (userResetPassword = FindUserResetPassword.Find(token)) == null)
			{
				return CreateResult(ProcessUserResetPasswordStatus.InvalidToken);
			}

			var user = FindUserByUsername.Find(userResetPassword.Username);

			if (user == null)
			{
				return CreateResult(ProcessUserResetPasswordStatus.Failure);
			}

			var success = UpdateUserPassword.Update(user, newPassword);

			return CreateResult(success ? ProcessUserResetPasswordStatus.Success : ProcessUserResetPasswordStatus.Failure);
		}

		public ProcessUserResetPasswordResult CreateResult(ProcessUserResetPasswordStatus status)
		{
			return new ProcessUserResetPasswordResult
			{
				Status = status
			};
		}
	}
}
