using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Informa.Library.Globalization;
using Informa.Library.Services.Sitemap;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using WebApi.OutputCache.Core.Cache;

namespace Informa.Web.ViewModels
{
    public class SitemapViewModel : GlassViewModel<ISitemap_Page>
    {
        protected readonly ISitemapService SitemapService;
        
        public SitemapViewModel(ISitemapService sitemapService)
        {
            SitemapService = sitemapService;
        }

        public string GetXML()
        {
            return SitemapService.GetNewsSitemapXML();
        }
    }
}

