using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Site
{
	[AutowireService]
	public class IsUrlCurrentSite : IIsUrlCurrentSite
	{
		protected readonly ISiteHostName SiteHostName;

		public IsUrlCurrentSite(
			ISiteHostName siteHostName)
		{
			SiteHostName = siteHostName;
		}

		public bool Check(string url)
		{
			return url.StartsWith("/") || url.Contains(SiteHostName.HostName);
		}
	}
}
