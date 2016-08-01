using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Links;
using Sitecore.Resources.Media;

namespace Informa.Library.CustomSitecore.Providers
{
	public class EmailLinkProvider : LinkProvider
	{
		public override UrlOptions GetDefaultUrlOptions()
		{
			var options = base.GetDefaultUrlOptions();

		
			options.LanguageEmbedding = LanguageEmbedding.Never;
			options.SiteResolving = true;
			options.AlwaysIncludeServerUrl = true;

			return options;
		}
	}
}
