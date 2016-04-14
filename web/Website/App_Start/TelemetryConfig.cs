using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Configuration;

namespace Informa.Web
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class AppInsightsConfig : IAppInsightsConfig
    {
        #region Implementation of IAppInsightsConfig

        public bool Enabled => Settings.GetSetting("ApplicationInsights.Enabled") == "true";
        public string InstrumentationKey => Settings.GetSetting("ApplicationInsights.InstrumentationKey");
        public string ResourceId => Settings.GetSetting("ApplicationInsights.ResourceID");

        #endregion
    }

    public interface IAppInsightsConfig
    {
        bool Enabled { get; }
        string InstrumentationKey { get; }
        string ResourceId { get; }
    }
}
