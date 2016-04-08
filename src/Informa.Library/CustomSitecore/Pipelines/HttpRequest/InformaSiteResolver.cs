using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.IO;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.Sites;
using Sitecore.Web;

namespace Informa.Library.CustomSitecore.Pipelines.HttpRequest
{
    public class InformaSiteResolver : SiteResolver
    {
        protected override SiteContext ResolveSiteContext(HttpRequestArgs args)
        {
            // we only want to set the site context in edit or preview mode
            List<string> modes = new List<string>() { "preview", "edit" };
            string qsMode = WebUtil.GetQueryString("sc_mode").ToLower();
            if(!modes.Contains(qsMode))
                return base.ResolveSiteContext(args);

            // context isn't available yet so we've got to pull the db from config factory
            var db = Factory.GetDatabase("master");
            if (db == null)
                return base.ResolveSiteContext(args);
            
            // if a query string ID was found, get the item for page editor and front-end preview mode
            string id = WebUtil.GetQueryString("sc_itemid");
            if (string.IsNullOrEmpty(id))
                return base.ResolveSiteContext(args);
            
            var item = db.GetItem(id);
            if(item == null)
                return base.ResolveSiteContext(args);

            // look for a site that is an ancestor of the item
            foreach (var site in Factory.GetSiteInfoList())
            {
                var homePage = db.GetItem($"{site.RootPath}{site.StartItem}");
                if (homePage != null && item.Paths.FullPath.StartsWith(homePage.Paths.FullPath))
                    return Factory.GetSite(site.Name);
            }

            return base.ResolveSiteContext(args);
        }
    }
}
