using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Publication
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SitePublicationNameFactory : ISitePublicationNameFactory
	{
		public string Create(ISite_Root siteRoot)
		{
			return siteRoot?.Publication_Name;
		}
	}
}
