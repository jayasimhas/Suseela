using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Utilities.Settings
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SiteSettings : ISiteSettings
	{
		private string GetSetting(string key, string defaultValue)
		{
			return Sitecore.Configuration.Settings.GetSetting(key, defaultValue);
		}
	}
}
