using Informa.Library.Publication;

namespace Informa.Library.User.Newsletter
{
	public class SiteNewsletterTypes : ISiteNewsletterTypes
	{
		public string Breaking { get; set; }
		public string Daily { get; set; }
		public string Weekly { get; set; }
		public ISitePublication Publication { get; set; }
	}
}
