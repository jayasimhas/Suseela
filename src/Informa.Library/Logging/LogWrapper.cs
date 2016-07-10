using System;
using Jabberwocky.Autofac.Attributes;
using log4net;

namespace Informa.Library.Logging
{
    public interface ILogWrapper
    {
        void SitecoreError(string message, Exception ex = null, object owner = null);
        void SitecoreWarn(string message, Exception ex = null, object owner = null);
        void SitecoreDebug(string message, object owner = null);
        void SitecoreInfo(string message, object owner = null);
    }

    [AutowireService]
    public class LogWrapper : ILogWrapper
    {
        //Default Sitecore Log
        public void SitecoreError(string message, Exception ex = null, object owner = null)
            => Sitecore.Diagnostics.Log.Error(message, ex, owner ?? this);

        public void SitecoreWarn(string message, Exception ex = null, object owner = null)
            => Sitecore.Diagnostics.Log.Warn(message, ex, owner ?? this);

        public void SitecoreDebug(string message, object owner = null)
            => Sitecore.Diagnostics.Log.Debug(message, owner ?? this);

        public void SitecoreInfo(string message, object owner = null)
            => Sitecore.Diagnostics.Log.Info(message, owner ?? this);

        //Log4Net
        public ILog GetLogger(Type type) => LogManager.GetLogger(type);
    }
}