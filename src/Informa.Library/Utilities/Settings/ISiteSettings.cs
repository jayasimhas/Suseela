namespace Informa.Library.Utilities.Settings
{
	public interface ISiteSettings
	{
	    string GetSetting(string key, string defaultValue);
	}
}
