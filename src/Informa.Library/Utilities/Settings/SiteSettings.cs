using System;
using System.Collections.Generic;
using System.Linq;
using Jabberwocky.Autofac.Attributes;
using Sitecore.Web;

namespace Informa.Library.Utilities.Settings
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SiteSettings : ISiteSettings
	{
		public string GetSetting(string key, string defaultValue = "")
		{
			return Sitecore.Configuration.Settings.GetSetting(key, defaultValue);
		}

		public string NlmExportPath => GetSetting("NLM.ExportPath", string.Empty);
		public string MailFromAddress => GetSetting("Mail.MailServerFromAddress", string.Empty);
		public SiteInfoModel GetCurrentSiteInfo() => Map(Sitecore.Context.Site.SiteInfo);
        public string PMBIDealsUrl => GetSetting("DCD.PMBIDealsURL", string.Empty);
        public string PMBICompaniesUrl => GetSetting("DCD.PMBICompaniesURL", string.Empty);
        public string OldDealsUrl => GetSetting("DCD.OldDealsURL", string.Empty);
        public IEnumerable<SiteInfoModel> GetSiteInfoList() =>
				Sitecore.Configuration.Factory.GetSiteInfoList().Select(Map);

		private static SiteInfoModel Map(SiteInfo scInfo) => new SiteInfoModel
		{
			Name = scInfo.Name,
			HostName = scInfo.HostName,
			Domain = scInfo.Domain,
			RootPath = scInfo.RootPath
		};
	}

	// This model is an incomplete representation of Sitecor.Web.SiteInfo.  Expand as needed.
	public class SiteInfoModel
	{
		public string Name { get; set; }
		public string HostName { get; set; }
		public string Domain { get; set; }
		public string RootPath { get; set; }
	}
}
