using Sitecore;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class SitecoreUserContext : ISitecoreUserContext
    {
        public Sitecore.Security.Accounts.User User => Context.User;                 
    }
}
