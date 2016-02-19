using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SitecoreTreeWalker.Util
{
	public static class PreviewLinkUpdater
	{
		public static Uri GetPreviewURL(string urlstring)
		{
			var uribuilder = new UriBuilder(urlstring);
			uribuilder.Scheme = Uri.UriSchemeHttp;
			uribuilder.Port = 443;
			return uribuilder.Uri;
		}
	}
}
