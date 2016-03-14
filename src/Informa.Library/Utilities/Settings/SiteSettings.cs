using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Utilities.Settings
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SiteSettings : ISiteSettings
	{
		public string GetSetting(string key, string defaultValue)
		{
			return Sitecore.Configuration.Settings.GetSetting(key, defaultValue);
		}

	    public string NlmExportPath => GetSetting("NLM.ExportPath", string.Empty);
	    public string MailFromAddress => GetSetting("Mail.MailServerFromAddress", string.Empty);
	}
}
