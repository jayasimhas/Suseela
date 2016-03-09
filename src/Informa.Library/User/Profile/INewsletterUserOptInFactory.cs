using Informa.Library.Newsletter;

namespace Informa.Library.User.Profile
{
	public interface INewsletterUserOptInFactory
	{
		INewsletterUserOptIn Create(NewsletterType newsletterOptInType, bool optIn);
	}
}
