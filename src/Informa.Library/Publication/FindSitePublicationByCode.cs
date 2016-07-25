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
		// TODO : 2016/07/11 - We really should make one to find by name and find by code.  This is used in 2 different ways and the variable naming is confusing. Results are inconsistent.
		public ISitePublication Find(string publicationCode)
		{
			return PublicationsContext.Publications.FirstOrDefault(p => string.Equals(p.Name, publicationCode, StringComparison.InvariantCultureIgnoreCase));
		}
	}
}
