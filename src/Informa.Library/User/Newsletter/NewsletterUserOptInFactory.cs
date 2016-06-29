using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Newsletter
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class NewsletterUserOptInFactory : INewsletterUserOptInFactory
	{
		public INewsletterUserOptIn Create(string newsletterOptInType, bool optIn)
		{
			return new NewsletterUserOptIn
			{
				OptIn = optIn,
				NewsletterType = newsletterOptInType
			};
		}
	}
}
