using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Models;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Newsletter
{
	[AutowireService(LifetimeScope.Default)]
	public class SitesNewsletterTypeContext : ISitesNewsletterTypeContext
	{
		protected readonly ISitecoreService SitecoreService;

		public SitesNewsletterTypeContext(
			ISitecoreService sitecoreService)
		{
			SitecoreService = sitecoreService;
		}

		public IEnumerable<string> Types
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
