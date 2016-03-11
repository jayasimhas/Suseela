namespace Informa.Library.User.ResetPassword.Entity
{
	public class EntityUserResetPasswordContextFactory : IEntityUserResetPasswordContextFactory
	{
		public EntityUserResetPasswordContext Create()
		{
			return new EntityUserResetPasswordContext();
		}
	}
}
