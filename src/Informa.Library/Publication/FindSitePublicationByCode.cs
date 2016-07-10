using Jabberwocky.Autofac.Attributes;
using System;
using System.Linq;

namespace Informa.Library.Publication
{
	[AutowireService]
	public class FindSitePublicationByCode : IFindSitePublicationByCode
	{
		protected readonly ISitePublicationsContext PublicationsContext;

		public FindSitePublicationByCode(
			ISitePublicationsContext publicationsContext)
		{
			PublicationsContext = publicationsContext;
		}

		public ISitePublication Find(string publicationCode)
		{
			return PublicationsContext.Publications.FirstOrDefault(p => string.Equals(p.Name, publicationCode, StringComparison.InvariantCultureIgnoreCase));
		}
	}
}
