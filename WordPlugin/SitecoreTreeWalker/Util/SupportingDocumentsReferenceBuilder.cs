using System.Collections.Generic;
using System.Net;
using System.Xml.Linq;
using Microsoft.Office.Interop.Word;
using SitecoreTreeWalker.Sitecore;

namespace SitecoreTreeWalker.Util
{
	public class SupportingDocumentsReferenceBuilder
	{
		public const string DocumentHyperlinkTooltip = "Document";

		public List<string> SupportingDocuments = new List<string>();

		public XElement GetHtmlHyperlink(Hyperlink hyperlink)
		{
			if (string.IsNullOrEmpty(hyperlink.ScreenTip) || !hyperlink.ScreenTip.StartsWith(DocumentHyperlinkTooltip))
			{
				return null;
			}
			var tooltip = hyperlink.ScreenTip;
			var colon = new char[1];
			colon[0] = ':';
			var strings = tooltip.Split(colon, 2);
			var path = strings[1];

			var root = new XElement("a");
			root.SetAttributeValue("class", "plugin-hide");
			try
			{
				if (!SupportingDocuments.Contains(path))
				{
					SupportingDocuments.Add(path); 
				}
				var url = SitecoreClient.GetDynamicUrl(path);
				root.SetAttributeValue("href", url);
                root.Value = hyperlink.Range.Text;
				//root.Add(GetDocIcon(url));
			}
			catch (WebException)
			{
				Globals.SitecoreAddin.AlertConnectionFailure();
			}
			return root;
		}
	}
}
