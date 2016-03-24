namespace Informa.Library.SiteDebugging
{
	public interface ISiteDebugger
	{
		bool IsDebugging(string key);
		void StartDebugging(string key);
		void StopDebugging(string key);
	}
}