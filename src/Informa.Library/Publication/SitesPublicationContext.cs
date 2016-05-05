using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Models;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Publication
{
	[AutowireService(LifetimeScope.Default)]
	public class SitesPublicationContext : ISitesPublicationContext
	{
		protected readonly ISitecoreService SitecoreService;

		public SitesPublicationContext(
			ISitecoreService sitecoreService)
		{
			SitecoreService = sitecoreService;
		}

		public IEnumerable<string> Names
		{
			get
			{
				var contentItem = SitecoreService.GetItem<IGlassBase>("/sitecore/content");
				var publicationNames = contentItem._ChildrenWithInferType.Where(sr => sr is ISite_Root).Cast<ISite_Root>().Select(sr => sr.Publication_Name).ToList();

				return publicationNames;
			}
		}
	}
}
