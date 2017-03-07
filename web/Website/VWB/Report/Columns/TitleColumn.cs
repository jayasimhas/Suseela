using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Elsevier.Web.VWB.Report.Columns
{
	public class TitleColumn : IVwbColumn
	{
		public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
		{
			return x.Title.CompareTo(y.Title);
		}

		public string GetHeader()
		{
			return "Title";
		}

		public string Key()
		{
			return "t";
		}

		public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
		{
			var link = GetPreviewLink(articleItemWrapper);
			
			

			var img = new Image { ImageUrl = "/VWB/images/vwb/urlicon.png" };
			img.Attributes.Add("align", "absmiddle");
			img.Attributes.Add("width", "16");
			img.Attributes.Add("height", "16");
			img.Attributes.Add("alt", "Hyperlink");

			link.Controls.Add(img);

			if (articleItemWrapper.Embargoed)
			{
				var flagImg = new Image() { ImageUrl = "/VWB/images/vwb/flag_red_h.png" };
				flagImg.Attributes.Add("align", "absmiddle");
				flagImg.Attributes.Add("width", "16");
				flagImg.Attributes.Add("height", "16");

				link.Controls.Add(flagImg);
			}

			
			var label = new Label { Text = articleItemWrapper.Title };
		
			link.Controls.Add(label);

			
			var tc = new TableCell();
         
            var locking = articleItemWrapper.InnerItem.InnerItem.Locking;
            string lockUser = locking.GetOwner();
            if (!string.IsNullOrEmpty(lockUser))
            {
                lockUser = lockUser.Substring(lockUser.IndexOf(@"\") + 1);
                var lockimage = new Image { ImageUrl = "/VWB/images/vwb/icon_lock.png" };
                lockimage.Attributes.Add("align", "absmiddle");
                lockimage.Attributes.Add("width", "16");
                lockimage.Attributes.Add("height", "16");
                lockimage.Attributes.Add("title", "Locked by: " + lockUser);
                lockimage.Attributes.Add("alt", "Locked by: " + lockUser);
                tc.Controls.Add(lockimage);
            }
            tc.Controls.Add(link);

            return tc;
		}

		private HyperLink GetPreviewLink(ArticleItemWrapper articleItemWrapper,bool isMobile = false)
		{
		
			var link = new HyperLink {/*Text = articleItemWrapper.Title*/};

			string mobileQueryParam = String.Empty;

			if (isMobile)
			{
				mobileQueryParam = "&mobile=1";
			}

            if (HttpContext.Current.Request.IsSecureConnection)
            {
                link.Attributes.Add("href", "/VWB/Util/LoginRedirectToPreview.aspx?redirect=" + HttpUtility.UrlEncode(articleItemWrapper.PreviewUrl + mobileQueryParam));
            }
            else
            {
                link.Attributes.Add("href", "/VWB/Util/LoginRedirectToPreview.aspx?redirect=" + HttpUtility.UrlEncode(articleItemWrapper.PreviewUrl + mobileQueryParam));
            }
		
			link.Attributes.Add("target", "_blank");

			

			

			return link;
		}

        public Dictionary<string, string> GetDropDownValues(List<ArticleItemWrapper> results)
        {
            throw new NotImplementedException();
        }

        protected readonly string _siteRoot = ConfigurationManager.AppSettings["Redirect.EBIHostName"];
	}
}