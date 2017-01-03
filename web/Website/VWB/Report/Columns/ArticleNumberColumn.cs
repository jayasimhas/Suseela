using System.Web.UI.WebControls;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;

namespace Elsevier.Web.VWB.Report.Columns
{
    public class ArticleNumberColumn : IVwbColumn
	{
		#region IVwbColumn Members

		public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
		{
			return x.ArticleNumber.CompareTo(y.ArticleNumber);
		}

		public string GetHeader()
		{
			return "Article Number";
		}

		string IVwbColumn.Key()
		{
			return Key();
		}

		public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
		{
			var tc = new TableCell();
			string url = GetDownloadLink(articleItemWrapper.InnerItem);
			if (url != "")
			{
				url += "?sc_mode=preview" + string.Format("&ts={0}", System.DateTime.Now.ToString("yyyyMMddHHmmss"));
				var hlink = new HyperLink();
                //if (HttpContext.Current.Request.IsSecureConnection)
                //{
                //    //url = "../Util/LoginRedirectToPreview.aspx?redirect=" + HttpUtility.UrlEncode(url).Replace("http", "https");
                //    url = "../Util/LoginRedirectToPreview.aspx?redirect=" + url.Replace("http", "https");
                //}
                //else
                //{
                //    url = "../Util/LoginRedirectToPreview.aspx?redirect=" + url;
                //}
                hlink.Attributes.Add("href", url);
				hlink.Attributes.Add("target", "_blank");
				hlink.Text = articleItemWrapper.ArticleNumber;

				var img = new Image {ImageUrl = "/VWB/images/vwb/wordicon.png"};
				img.Attributes.Add("align", "absmiddle");
				img.Attributes.Add("width", "16");
				img.Attributes.Add("height", "16");
				img.Attributes.Add("alt", "Hyperlink");

				var label = new Label {Text = articleItemWrapper.ArticleNumber};

				hlink.Controls.Add(img);
				hlink.Controls.Add(label);
				tc.Controls.Add(hlink);
			}
			else
			{
				tc.Text = articleItemWrapper.ArticleNumber;
			}			
	
			return tc;
		}

		public static string Key()
		{
			return "an";
		}

		protected string GetDownloadLink(Item articleBaseItem)
		{
		    Database masterDb = Factory.GetDatabase("master");
            IArticle article = articleBaseItem.GlassCast<IArticle>(inferType: true);

		    if (article.Word_Document == null)
		    {
		        return string.Empty;
		    }

            Item wordDoc = masterDb.GetItem(article.Word_Document.TargetId.ToString());
		    if (wordDoc == null)
		    {
		        return string.Empty;
		    }

            string url = MediaManager.GetMediaUrl(wordDoc);
            url = url.Replace("/-/", "/~/");
            return url.Replace("-", " ");
        }

		#endregion
	}
}