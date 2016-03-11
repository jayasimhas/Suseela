using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class FindUserByUsername : IFindUserByUsername
	{
		protected readonly IFindUserByEmail FindUserByEmail;

		public FindUserByUsername(
			IFindUserByEmail findUserByEmail)
		{
			FindUserByEmail = findUserByEmail;
		}

		public IUser Find(string username)
		{
			return FindUserByEmail.Find(username);
		}
	}
}
