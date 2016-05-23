using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Publication
{
	[AutowireService]
	public class SitePublicationNameFactory : ISitePublicationNameFactory
	{
		public string Create(ISite_Root siteRoot)
		{
			return siteRoot?.Publication_Name ?? string.Empty;
		}
	}
}
