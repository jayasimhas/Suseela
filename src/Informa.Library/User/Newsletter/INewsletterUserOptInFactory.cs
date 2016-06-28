namespace Informa.Library.User.Newsletter
{
	public interface INewsletterUserOptInFactory
	{
		INewsletterUserOptIn Create(string newsletterOptInType, bool optIn);
	}
}
