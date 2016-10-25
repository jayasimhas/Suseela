using Glass.Mapper.Sc;
using Informa.Library.Services.Global;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Autofac.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Utilities.CMSHelpers
{
    [AutowireService(LifetimeScope.PerScope)]
    public class GlassVerticalRootContext : IVerticalRootContext
    {
        protected readonly ISitecoreContext SitecoreContext;
        protected readonly IGlobalSitecoreService GlobalService;
       
        public GlassVerticalRootContext(ISitecoreContext sitecoreContext, IGlobalSitecoreService globalService)
        {
            SitecoreContext = sitecoreContext;
            GlobalService = globalService;
        }
        public Guid verticalGuid => new Guid(SitecoreContext?.GetRootItem<ISite_Root>()._Parent._Id.ToString());
        public IVertical_Root Item => GlobalService.GetItem<IVertical_Root>(verticalGuid);
    }
}
