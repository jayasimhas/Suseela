﻿using Sitecore.Shell.Framework.Commands;
using System.Web;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Configuration;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;
using Sitecore.Web.UI.Sheer;

namespace Informa.Library.CustomSitecore.Ribbon
{
	class DownloadWord : Command
	{
		public override void Execute(CommandContext context)
		{
			//Sitecore.Context.ClientPage.ClientResponse.Alert("Testing my button");
			Assert.ArgumentNotNull(context, "context");
			Item[] items = context.Items;
			if (items.Length == 1)
			{
				Item article = items[0];
				LinkField wordDocument = article.Fields[IArticleConstants.Word_DocumentFieldName];
				if (wordDocument != null)
				{
					var masterDb = Factory.GetDatabase("master");
					Item item = masterDb.GetItem(wordDocument.TargetID);
					if (item != null && MediaManager.HasMediaContent(item))
					{
						string siteRoot = HttpContext.Current.Request.Url.Host;
						Item docItem = masterDb.GetItem(wordDocument.TargetID);

						string url = siteRoot + MediaManager.GetMediaUrl(docItem) + "?sc_mode=preview";
						SheerResponse.Eval("window.open('" + HttpContext.Current.Request.Url.Scheme + "://" + url +
										   "','_blank','height=800,width=800,menubar=no,toolbar=no,resizable=yes,scrollbars=yes,titlebar=no,location=no')");
					}
					else
					{
						Sitecore.Context.ClientPage.ClientResponse.Alert("No file has been attached");
					}
				}
				else
				{
					Sitecore.Context.ClientPage.ClientResponse.Alert("No file has been attached");
				}
			}
			else
			{
				Sitecore.Context.ClientPage.ClientResponse.Alert("Unable to download file.");
			}
		}
	}
}
