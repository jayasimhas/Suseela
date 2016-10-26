using System;
using System.Collections.Generic;
using Elsevier.Library.CustomItems.Global.ArticleCategory;
using Elsevier.Library.CustomItems.Global.ArticleSizes;
using Elsevier.Library.CustomItems.Global.Staff;
using System.Linq;
using System.Web;
using Elsevier.Library.CustomItems.Publication.General;
using Elsevier.Library.Metadata;
using Elsevier.Library.Reference;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Social.Infrastructure.Utils;
using Sitecore.Web;
using Velir.Utilities.Extensions.System.Collections.Generic;
using ArticleItem = Elsevier.Library.CustomItems.Publication.General.ArticleItem;

namespace Elsevier.Web.VWB.Report
{
    public class ArticleItemWrapper
    {
        public int NumArticlesInIssue;
        public string ArticleNumber;
        public string Title;
        public DateTime ArticleCreation;
        public IEnumerable<string> Authors;
        public ArticleItem InnerItem;
        public IEnumerable<string> Editors;
        public string IssueDateValue;
        public int WordCount;
        public string PreviewUrl;
        public DateTime SAPDateTime;
        public DateTime WebPublicationDateTime;
        public string HomePageCategory;

        private string _articleLengthEstimate;
        public string ArticleLengthEstimate
        {
            get
            {
                if (string.IsNullOrEmpty(_articleLengthEstimate))
                {
                    _articleLengthEstimate = _estimator.GetArticleLengthEstimate(InnerItem.ID.ToGuid()).ToString();
                }
                return _articleLengthEstimate;
            }
        }

        public string EmailPriority;

        public string MediaType;

        public string ContentType;

        public string TaxonomyString;

        public List<string> SidebarArticleNumbers = new List<string>();
        public string NotesToEditorial;
        public string NotesToProduction;
        public DateTime EmbargoDate;
        public Boolean Embargoed;
        public bool IsFirstArticleInCategory;
        public bool ArticlesInCategoryShouldBeGrouped;
        public Guid ArticleCategoryGuid;
        public int NumArticlesInCategory;

        public string PublicationName;

        public ArticleItemWrapper(ArticleItem articleItem)
            : this(articleItem, new ArticleLengthEstimator())
        { }

        public string GetPreviewUrl(string itemId)
        {
            return HttpContext.Current.Request.Url.Scheme + "://" + WebUtil.GetHostName() + "/?sc_itemid={" + itemId + "}&sc_mode=preview&sc_lang=en";
        }

        public ArticleItemWrapper(ArticleItem articleItem, ArticleLengthEstimator estimator)
        {
            _estimator = estimator;

            Database masterDb = Factory.GetDatabase("master");

            Item articleBaseItem = masterDb.GetItem(articleItem.ID.ToString());

            IArticle article = articleBaseItem.GlassCast<IArticle>(inferType: true);

            InnerItem = articleBaseItem;

            //var pubRoot =  _globalService.GetSiteRootAncestor(source._Id);

            var siteRootItem = getSiteRoot(articleBaseItem);
            string pubName = string.Empty;
            if (siteRootItem != null && siteRootItem.Fields["Publication Name"] != null)
                pubName = siteRootItem.Fields["Publication Name"].Value;

            //TODO
            PreviewUrl = GetPreviewUrl(article._Id.ToString());
            ArticleNumber = article.Article_Number;
            if (ArticleNumber == null)
            {
                ArticleNumber = "";
            }

            PublicationName = pubName;
            //Title = article.Title + " : " + article._Path;
            Title = article.Title;

            if (article.Authors != null)
            {
                Authors = article.Authors.Select(a => a.First_Name + " " + a.Last_Name);
            }


            if (article.Created_Date == DateTime.MinValue)
            {
                ArticleCreation = articleBaseItem.Statistics.Created.ToServerTimeZone();
            }
            else
            {
                ArticleCreation = article.Created_Date.ToServerTimeZone();
            }

            if (string.IsNullOrEmpty(article.Word_Count))
            {
                WordCount = 0;
            }
            else
            {
                int parsedCount = 0;
                if (Int32.TryParse(article.Word_Count, out parsedCount))
                {
                    WordCount = parsedCount;
                }
                else
                {
                    WordCount = 0;
                }
            }

            var ra = (article.Referenced_Articles != null)
                ? article.Referenced_Articles.Select(a => ((IArticle)a).Article_Number).Where(b => !string.IsNullOrEmpty(b))
                : Enumerable.Empty<string>();

            if (ra.Any())
                SidebarArticleNumbers.AddRange(ra);
            
            SidebarArticleNumbers.Sort();

            TaxonomyString = (article.Taxonomies != null)
                ? string.Join(",", article.Taxonomies.Select(t => t.Item_Name))
                : string.Empty;
		    
            ContentType = "";

            if (article.Content_Type != null)
            {
                ContentType = article.Content_Type.Item_Name;
            }

            MediaType = "";
            if (article.Media_Type != null)
            {
                MediaType = article.Media_Type.Item_Name;
            }

            EmailPriority = "";
            EmailPriority = article.Sort_Order.ToString();

            NotesToEditorial = article.Editorial_Notes;


            Embargoed = article.Embargoed;
            EmbargoDate = articleBaseItem.Publishing.PublishDate.ToServerTimeZone();

            SAPDateTime = article.Planned_Publish_Date.ToServerTimeZone();
            WebPublicationDateTime = article.Actual_Publish_Date.ToServerTimeZone();
        }

        public Item getSiteRoot(Item child)
        {
            if (child.ParentID.Guid == Guid.Empty)
                return null;

            if (child.Parent.TemplateID.Guid == new Guid("{DD003F89-D57D-48CB-B428-FFB519AACA56}"))
            {
                return child.Parent;
            }

            return getSiteRoot(child.Parent);
        }

        public const string NE = "N/E";
        public const string NA = "N/A";

        public static IEnumerable<ArticleItemWrapper> GetArticleItemProxies(IEnumerable<ArticleItem> articles)
        {
            return articles.Select(a => new ArticleItemWrapper(a));
        }

        protected ArticleLengthEstimator _estimator;
    }
}