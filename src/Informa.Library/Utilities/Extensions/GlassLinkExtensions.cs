using System.Web;
using Glass.Mapper.Sc.Fields;

namespace Informa.Library.Utilities.Extensions
{
	public static class GlassLinkExtensions
	{
		public static string GetLink(this Link link)
		{
			const string hash = "#";

			if (link == null || string.IsNullOrEmpty(link.Url))
				return hash;

		    if (HttpContext.Current == null || HttpContext.Current.Request == null)
		        return hash;

            var req = HttpContext.Current.Request;
		    return (link.Url.StartsWith("/"))
		        ? $"{req.Url.Scheme}://{req.Url.Host}{link.Url}"
		        : link.Url;
		}
	}
}
