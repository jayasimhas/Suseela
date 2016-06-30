using System;
using Jabberwocky.Autofac.Attributes;
using Sitecore.SecurityModel;

namespace Informa.Library.Wrappers
{
    public interface ISitecoreSecurityWrapper
    {
        void SecurityDisabledAction(Action action);
        TResult SecurityDisabledFunction<TResult>(Func<TResult> function);
    }

    [AutowireService]
    public class SitecoreSecurityWrapper : ISitecoreSecurityWrapper
    {
        public void SecurityDisabledAction(Action action)
        {
            using (new SecurityDisabler())
            {
                action();
            }
        }

        public TResult SecurityDisabledFunction<TResult>(Func<TResult> function)
        {
            using (new SecurityDisabler())
            {
                return function();
            }
        }
    }
}
