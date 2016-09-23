using System.Collections.Generic;

namespace Informa.Library.Utilities.Settings
{
	public interface ISiteSettings
	{
		string GetSetting(string key, string defaultValue = "");

		SiteInfoModel GetCurrentSiteInfo();

		IEnumerable<SiteInfoModel> GetSiteInfoList();

		string NlmExportPath { get; }

		string MailFromAddress { get; }
        string OldDealsUrl { get; }
        string PMBIDealsUrl { get; }
        string PMBICompaniesUrl { get; }
    }
}
