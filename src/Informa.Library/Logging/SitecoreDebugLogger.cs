namespace Informa.Library.Logging
{
    public class SitecoreDebugLogger : IDebugLogger
    {
        public void Log(string message)
        {
            Sitecore.Diagnostics.Log.Debug(message);
        }
    }
}