using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Mail
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class EmailFactory : IEmailFactory
	{
		public IEmail Create()
		{
			return new Email();
		}
	}
}
