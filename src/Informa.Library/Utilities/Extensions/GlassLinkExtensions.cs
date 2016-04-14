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

        public static string LinkUrl(this Link lf)
        {
            switch (lf.Type)
            {
                case LinkType.NotSet:
                    break;
                case LinkType.Anchor:
                    return !string.IsNullOrEmpty(lf.Anchor) ? "#" + lf.Anchor : string.Empty;
                case LinkType.External:
                    return lf.Url;
                case LinkType.JavaScript:
                    return lf.Url;
                case LinkType.Internal:
                    return lf.TargetId != null ? Sitecore.Links.LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(lf.TargetId))) : string.Empty;
                case LinkType.MailTo:
                    return lf.Url;
                case LinkType.Media:
                    return lf.TargetId != null ? Sitecore.Resources.Media.MediaManager.GetMediaUrl(Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(lf.TargetId))) : string.Empty;
                default:
                    return lf.Url;
            }

            return lf.Url;
        }
    }
}
