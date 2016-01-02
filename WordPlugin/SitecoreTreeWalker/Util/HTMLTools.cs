using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace SitecoreTreeWalker.Util
{
	public class HTMLTools
	{
		public static List<string> CheckForInsecureIFrames(string paragraphText)
		{
			HtmlNode.ElementsFlags.Remove("form");
			string html = string.Format("<html><head></head><body>{0}</body></html>", paragraphText);
			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(html);
			HtmlNode embedNodeParent = doc.DocumentNode.SelectSingleNode("//body");
			var nodes = embedNodeParent.SelectNodes("//iframe");
			var insecureIFrames = new List<string>();
			if (nodes != null)
			{
				foreach (HtmlNode node in nodes)
				{
					if (node.Attributes["src"] != null &&
						(!node.Attributes["src"].Value.StartsWith("https") && !node.Attributes["src"].Value.StartsWith("//")))
					{
						insecureIFrames.Add(node.Attributes["src"].Value);
					}
				}
			}
			return insecureIFrames;
		}

		//return bool
		public static bool ContainsForExternalNodes(string paragraphText)
		{
			HtmlNode.ElementsFlags.Remove("form");
			string html = string.Format("<html><head></head><body>{0}</body></html>", paragraphText);
			HtmlDocument document = new HtmlDocument();
			document.LoadHtml(html);
			var scriptNodes = document.DocumentNode.SelectNodes("//script");
			var linkNodes = document.DocumentNode.SelectNodes("//link");
			var formNodes = document.DocumentNode.SelectNodes("//form");
			var styleNodes = document.DocumentNode.SelectNodes("//style");


			//return the statement
			return !((formNodes == null || formNodes.Count == 0)
					&& (scriptNodes == null || scriptNodes.Count == 0)
					&& (linkNodes == null || linkNodes.Count == 0)
					&& (styleNodes == null || styleNodes.Count == 0));

		}
	}
}
