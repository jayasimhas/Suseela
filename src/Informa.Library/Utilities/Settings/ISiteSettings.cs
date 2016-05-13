using System.Collections.Generic;

namespace Informa.Library.Utilities.Settings
{
	public interface ISiteSettings
	{
	    string GetSetting(string key, string defaultValue);

	    IEnumerable<SiteInfoModel> GetSiteInfoList();

        string NlmExportPath { get; }

        string MailFromAddress { get; }
	}
}
