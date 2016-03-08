using Informa.Library.Newsletter;

namespace Informa.Library.User.Profile
{
	public class NewsletterUserOptIn : INewsletterUserOptIn
	{
		public bool OptIn { get; set; }
		public NewsletterType NewsletterType { get; set; }
	}
}
