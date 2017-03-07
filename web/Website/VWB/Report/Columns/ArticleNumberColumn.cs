using System;
using System.Web.UI.WebControls;
using Elsevier.Library.CustomItems.Publication.General;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using Sitecore.Links;
using System.Web;
using System.Collections.Generic;

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
            if (!string.IsNullOrEmpty(articleItemWrapper.ArticleNumber))
            {
                var label = new Label { Text = articleItemWrapper.ArticleNumber };

                var CMShlink = new HyperLink();
                CMShlink = GetArticleItemLink(articleItemWrapper);
                if (CMShlink != null)
                {
                    //CMShlink.Attributes.Add("href", CMShlink.);
                    //CMShlink.Attributes.Add("target", "_blank");
                    var imgCMS = new Image { ImageUrl = "/VWB/images/vwb/cmsicon.png" };
                    imgCMS.Attributes.Add("align", "absmiddle");
                    imgCMS.Attributes.Add("width", "16");
                    imgCMS.Attributes.Add("height", "16");
                    imgCMS.Attributes.Add("alt", "CmsItem");

                    CMShlink.Controls.Add(imgCMS);
                    tc.Controls.Add(CMShlink);
                }

                string Wordurl = GetDownloadLink(articleItemWrapper.InnerItem);
                if (Wordurl != "")
                {
                    Wordurl += "?sc_mode=preview" + string.Format("&ts={0}", System.DateTime.Now.ToString("yyyyMMddHHmmss"));
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
                    hlink.Attributes.Add("href", Wordurl);
                    hlink.Attributes.Add("target", "_blank");

                    var imgWord = new Image { ImageUrl = "/VWB/images/vwb/wordicon.png" };
                    imgWord.Attributes.Add("align", "absmiddle");
                    imgWord.Attributes.Add("width", "16");
                    imgWord.Attributes.Add("height", "16");
                    imgWord.Attributes.Add("alt", "Hyperlink");

                    hlink.Controls.Add(imgWord);
                    tc.Controls.Add(hlink);
                }
                tc.Controls.Add(label);
            }
            return tc;
        }

        private HyperLink GetArticleItemLink(ArticleItemWrapper articleItemWrapper, bool isMobile = false)
        {
            // return articleBaseItem != null ? LinkManager.GetItemUrl(articleBaseItem) : string.Empty;
            var link = new HyperLink {/*Text = articleItemWrapper.Title*/};
            string mobileQueryParam = String.Empty;
            if (isMobile)
            {
                mobileQueryParam = "&mobile=1";
            }
            if (HttpContext.Current.Request.IsSecureConnection)
            {
                link.Attributes.Add("href", "/VWB/Util/LoginRedirectToPreview.aspx?redirect=" + HttpUtility.UrlEncode(articleItemWrapper.CmsItemUrl + mobileQueryParam));
            }
            else
            {
                link.Attributes.Add("href", "/VWB/Util/LoginRedirectToPreview.aspx?redirect=" + HttpUtility.UrlEncode(articleItemWrapper.CmsItemUrl + mobileQueryParam));
            }
            link.Attributes.Add("target", "_blank");
            return link;
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

        public Dictionary<string, string> GetDropDownValues(List<ArticleItemWrapper> results)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}