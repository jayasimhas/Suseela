using Sitecore.Links;

namespace Informa.Library.CustomSitecore.Providers
{
	public class CustomLinkProvider : LinkProvider
	{
		public override UrlOptions GetDefaultUrlOptions()
		{
			var options = base.GetDefaultUrlOptions();

			options.SiteResolving = Sitecore.Configuration.Settings.Rendering.SiteResolving;

			return options;
		}
	}
}