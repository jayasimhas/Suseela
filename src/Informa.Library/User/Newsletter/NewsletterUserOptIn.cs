using Informa.Library.Newsletter;

namespace Informa.Library.User.Newsletter
{
	public class NewsletterUserOptIn : INewsletterUserOptIn
	{
		public bool OptIn { get; set; }
		public NewsletterType NewsletterType { get; set; }
	}
}
