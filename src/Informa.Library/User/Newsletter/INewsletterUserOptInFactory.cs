using Informa.Library.Newsletter;

namespace Informa.Library.User.Newsletter
{
	public interface INewsletterUserOptInFactory
	{
		INewsletterUserOptIn Create(NewsletterType newsletterOptInType, bool optIn);
	}
}
