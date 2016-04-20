using System;
using System.Linq;
using Sitecore.Data.Items;
using Sitecore.Web;

namespace Informa.Library.Utilities.Extensions
{
	public static class ItemExtensions
	{
		public static SiteInfo GetSite(this Item item)
		{
			return Sitecore.Configuration.Factory.GetSiteInfoList().FirstOrDefault(x => x.Domain == "extranet" && item.Paths.FullPath.StartsWith($"{x.RootPath}{x.StartItem}", StringComparison.InvariantCultureIgnoreCase));
		}

		public static string GetSiteName(this Item item)
		{
			return item?.GetSite()?.Name ?? string.Empty;
		}
	}
}
