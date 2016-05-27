using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Publication
{
	[AutowireService]
	public class SitePublicationCodeFactory : ISitePublicationCodeFactory
	{
		public string Create(ISite_Root siteRoot)
		{
			return siteRoot?.Publication_Code ?? string.Empty;
		}
	}
}
