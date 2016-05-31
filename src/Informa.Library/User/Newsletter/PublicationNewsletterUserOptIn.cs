using Informa.Library.Publication;

namespace Informa.Library.User.Newsletter
{
	public class PublicationNewsletterUserOptIn : IPublicationNewsletterUserOptIn
	{
		public bool OptIn { get; set; }
		public ISitePublication Publication { get; set; }
	}
}
