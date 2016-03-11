using Informa.Library.Data.Entity;
using System.Data.Entity;

namespace Informa.Library.User.ResetPassword.Entity
{
	public class EntityUserResetPasswordContext : CustomDbContext
	{
		public DbSet<UserResetPassword> UserResetPasswords { get; set; }
	}
}
