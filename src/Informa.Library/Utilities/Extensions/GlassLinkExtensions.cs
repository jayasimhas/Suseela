using Glass.Mapper.Sc.Fields;

namespace Informa.Library.Utilities.Extensions
{
	public static class GlassLinkExtensions
	{
		public static string GetLink(this Link link)
		{
			const string hash = "#";

			if (link == null)
				return hash;

			return string.IsNullOrEmpty(link.Url)
				? hash
				: link.Url;
		}
	}
}
