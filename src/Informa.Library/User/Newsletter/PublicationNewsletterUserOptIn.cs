namespace Informa.Library.User.Newsletter
{
	public class PublicationNewsletterUserOptIn : IPublicationNewsletterUserOptIn
	{
		public bool OptIn { get; set; }
		public string Publication { get; set; }
	}
}
