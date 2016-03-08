using Informa.Library.Newsletter;

namespace Informa.Library.User.Profile
{
	public interface INewsletterUserOptIn
	{
		bool OptIn { get; set; }
		NewsletterType NewsletterType { get; }
	}
}
