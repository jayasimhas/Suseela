using Informa.Library.Site;
using Jabberwocky.Autofac.Attributes;
using System;

namespace Informa.Library.User.UserPreference
{
    [AutowireService(LifetimeScope = LifetimeScope.PerScope)]
    public class MyViewToggleRedirectUrlFactory : IMyViewToggleRedirectUrlFactory
    {
        protected readonly ISiteRootContext SiterootContext;
        public MyViewToggleRedirectUrlFactory(ISiteRootContext siterootContext)
        {
            SiterootContext = siterootContext;
        }
        public string create()
        {
            return SiterootContext.Item?.MyView_Page?._Url;
        }
    }
}
