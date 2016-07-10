namespace Informa.Library.User.Newsletter
{
	public class NewsletterUserOptIn : INewsletterUserOptIn
	{
		public bool OptIn { get; set; }
		public string NewsletterType { get; set; }
	}
}
