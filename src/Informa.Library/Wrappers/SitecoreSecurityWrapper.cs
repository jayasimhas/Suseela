using System;
using Jabberwocky.Autofac.Attributes;
using Sitecore.SecurityModel;

namespace Informa.Library.Wrappers
{
    public interface ISitecoreSecurityWrapper
    {
        void WithSecurityDisabled(Action action);
        TResult WithSecurityDisabled<TResult>(Func<TResult> function);
    }

    [AutowireService]
    public class SitecoreSecurityWrapper : ISitecoreSecurityWrapper
    {
        public void WithSecurityDisabled(Action action)
        {
            using (new SecurityDisabler())
            {
                action();
            }
        }

        public TResult WithSecurityDisabled<TResult>(Func<TResult> function)
        {
            using (new SecurityDisabler())
            {
                return function();
            }
        }
    }
}
