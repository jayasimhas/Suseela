using Glass.Mapper.Sc;
using Informa.Library.Services.Global;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Autofac.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Informa.Library.Utilities.CMSHelpers
{
    [AutowireService(LifetimeScope.PerScope)]
    public class GlassVerticalRootContext : IVerticalRootContext
    {
        protected readonly ISitecoreContext SitecoreContext;
        protected readonly IGlobalSitecoreService GlobalService;
        protected string ArticleId = (HttpContext.Current != null && !string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["id"]))? HttpContext.Current.Request.QueryString["id"]:(HttpContext.Current != null? (string)(HttpContext.Current.Items["nlmArticle"]): null);
        
        public GlassVerticalRootContext(ISitecoreContext sitecoreContext, IGlobalSitecoreService globalService)
        {
            SitecoreContext = sitecoreContext;
            GlobalService = globalService;
        }
        
        public Guid verticalGuid => new Guid(SitecoreContext?.GetRootItem<ISite_Root>()._Parent._Id.ToString());
        public IVertical_Root Item => GetVerticalRootItem();

        private IVertical_Root GetVerticalRootItem()
        {
            if (string.IsNullOrEmpty(ArticleId))
                return GlobalService.GetItem<IVertical_Root>(verticalGuid);
            Guid id;
            if (!Guid.TryParse(ArticleId, out id))
            {
                return GlobalService.GetItem<IVertical_Root>(verticalGuid);
            }
            return GlobalService.GetVerticalRootAncestor(id);
        }

    }
}
