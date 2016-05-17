using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Site
{
	[AutowireService]
	public class SiteHostName : ISiteHostName
	{
		public string HostName => Sitecore.Context.Site.HostName;
	}
}
