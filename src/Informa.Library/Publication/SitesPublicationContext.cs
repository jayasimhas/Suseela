﻿using Informa.Library.Site;
using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Publication
{
	[AutowireService]
	public class SitesPublicationContext : ISitesPublicationContext
	{
		protected readonly ISiteRootsContext SiteRootsContext;
		protected readonly ISitePublicationCodeFactory SitePublicatioCodeFactory;

		public SitesPublicationContext(
			ISiteRootsContext siteRootsContext,
			ISitePublicationCodeFactory sitePublicationCodeFactory)
		{
			SiteRootsContext = siteRootsContext;
			SitePublicatioCodeFactory = sitePublicationCodeFactory;
		}

		public IEnumerable<string> Names => SiteRootsContext.SiteRoots.Select(sr => SitePublicatioCodeFactory.Create(sr)).ToList();
	}
}
