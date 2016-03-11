using System.Linq;

namespace Informa.Library.User.ResetPassword.Entity
{
	public class EntityFindUserResetPassword : IFindUserResetPassword
	{
		protected readonly IEntityUserResetPasswordContextFactory EntityContextFactory;

		public EntityFindUserResetPassword(
			IEntityUserResetPasswordContextFactory entityContextFactory)
		{
			EntityContextFactory = entityContextFactory;
		}

		public IUserResetPassword Find(string token)
		{
			using (var context = EntityContextFactory.Create())
			{
				return context.UserResetPasswords.FirstOrDefault(urp => string.Equals(urp.Token, token));
			}
		}
	}
}
