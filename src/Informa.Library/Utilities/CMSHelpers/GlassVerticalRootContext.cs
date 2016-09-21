using Glass.Mapper.Sc;
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
        public GlassVerticalRootContext(ISitecoreContext sitecoreContext)

        {
            SitecoreContext = sitecoreContext;
        }


        private IVertical_Root _item;
        public IVertical_Root Item => _item ??
            (_item = SitecoreContext?.GetRootItem<IVertical_Root>());
    }
}
