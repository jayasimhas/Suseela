using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Registration
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class NewUserFactory : INewUserFactory
	{
		public INewUser Create()
		{
			return new NewUser();
		}
	}
}
