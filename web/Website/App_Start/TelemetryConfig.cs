using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Jabberwocky.Glass.Autofac.Attributes;
using Newtonsoft.Json.Serialization;
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
