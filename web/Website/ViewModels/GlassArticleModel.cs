using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Glass.Mapper.Sc;
using Informa.Library.Article.Search;
using Informa.Library.Globalization;
using Informa.Library.Search.Utilities;
using Informa.Library.Site;
using Informa.Library.User.Entitlement;
using Informa.Library.Utilities.Extensions;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Velir.Search.Core.CustomGlass.Models;
using Informa.Library.Utilities.TokenMatcher;
using Informa.Models.Informa.Models.sitecore.templates.System.Media.Unversioned;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using Sitecore.Web;

namespace Informa.Web.ViewModels
{
    public class GlassArticleModel : EntitledViewModel<IArticle>, IArticleModel
    {
        public ISiteRootContext SiterootContext { get; set; }
        protected readonly IArticleListItemModelFactory ArticleListableFactory;
        protected readonly ITextTranslator TextTranslator;
        protected readonly IArticleSearch Searcher;
        protected readonly ISitecoreContext SitecoreContext;
        protected readonly IArticleComponentFactory ArticleComponentFactory;    

        public GlassArticleModel(
            ISiteRootContext siterootContext,
            IArticleListItemModelFactory articleListableFactory,
            ITextTranslator textTranslator, 
            IArticleSearch searcher,
            ISitecoreContext context,
            IArticleComponentFactory articleComponentFactory,
            IEntitledProductContext entitledProductContext) : base(entitledProductContext)
        {
            SiterootContext = siterootContext;
            ArticleListableFactory = articleListableFactory;
            TextTranslator = textTranslator;
            Searcher = searcher;
            SitecoreContext = context;
            ArticleComponentFactory = articleComponentFactory;      
        }

        public IEnumerable<ILinkable> TaxonomyItems
            => GlassModel.Taxonomies.Select(x => new LinkableModel { LinkableText = x.Item_Name, LinkableUrl = SearchTaxonomyUtil.GetSearchUrl(x) });

        public string SelectedMobileMedia => Sitecore.Context.Device.QueryString == "mobilemedia=true" ? ArticleComponentFactory.Component(WebUtil.GetQueryString("selectedid"), GlassModel) : null;

        #region Implementation of IArticleModel

        public string Title => GlassModel.Title;
        public string Sub_Title => GlassModel.Sub_Title;
        public string Body
        {
            get
            {
                string body = IsFree || IsEntitled() ? GlassModel.Body : "";

                //Replace any DCD related tokens with proper names
                body = DCDTokenMatchers.ProcessDCDTokens(body);

                return body;
            }
        }


        public IHierarchyLinks TaxonomyHierarchy => new HierarchyLinksViewModel(GlassModel, TextTranslator);
        public DateTime Date => (Sitecore.Context.PageMode.IsPreview) ? GlassModel.Planned_Publish_Date : GlassModel.Actual_Publish_Date;
        //TODO: Extract to a dictionary.
        public string Content_Type => GlassModel.Content_Type?.Item_Name;
        public string Media_Type => GlassModel.Media_Type?.Item_Name == "Data" ? "chart" : GlassModel.Media_Type?.Item_Name?.ToLower() ?? "";

        public IEnumerable<IPersonModel> Authors => GlassModel.Authors.Select(x => new PersonModel(x));
        public string Category => GlassModel.Article_Category;

        public IEnumerable<IListable> RelatedArticles
        {
            get
            {
                var relatedArticles = GlassModel.Related_Articles.Concat(GlassModel.Referenced_Articles).Take(10).ToList();

                if (relatedArticles.Count < 10)
                {
                    var filter = Searcher.CreateFilter();
                    filter.ReferencedArticle = GlassModel._Id;

                    var results = Searcher.Search(filter);
                    if (results.Articles.Any())
                        relatedArticles.AddRange(results.Articles.Take(10 - relatedArticles.Count));
                }
                return relatedArticles.Select(x => ArticleListableFactory.Create(x)).Cast<IListable>().OrderByDescending(x => x.ListableDate);
            }
        }

        public IEnumerable<IGlassBase> KeyDocuments => GlassModel.Supporting_Documents;
        public IFeaturedImage Image => new ArticleFeaturedImage(GlassModel);

        public string GetTitle(IGlassBase g)
        {
            IFile f = SitecoreContext.GetItem<IFile>(g._Id);
            return (f != null && !string.IsNullOrEmpty(f.Title))
                ? f.Title
                : g._Name;
        }

        public string GetDocumentIcon(IGlassBase g)
        {
            IFile f = SitecoreContext.GetItem<IFile>(g._Id);
            if (f == null || string.IsNullOrEmpty(f.Extension) || f.Extension.Equals("pdf"))
                return "pdf";
            if (f.Extension.Equals(".doc") || f.Extension.Equals(".docx"))
                return "doc";
            if (f.Extension.Equals(".xls") || f.Extension.Equals(".xlsx"))
                return "xls";
            return "pdf";
        }

        public override bool IsFree => GlassModel.Free_Article;

        #endregion

        #region Implementation of ILinkable

        public string LinkableText => Title;
        public string LinkableUrl => GlassModel._Url;

        #endregion

        #region Implementation of IListable

        public IEnumerable<ILinkable> ListableAuthors
            =>
                Authors.Select(
                    x => new LinkableModel { LinkableText = x.Name, LinkableUrl = $"mailto://{x.Email_Address}" });

        public DateTime ListableDate => Date;
        public string ListableImage => Image?.ImageUrl;
        //TODO: Get real summary
        public string ListableSummary
        {
            get
            {
                string summ = GlassModel.Summary;

                //Replace any DCD related tokens with proper names
                summ = DCDTokenMatchers.ProcessDCDTokens(summ);

                return summ;
            }
        }
        public string KeyDocumentHeader => TextTranslator.Translate("Article.KeyDocs");
        public string ListableSummaryHeader => TextTranslator.Translate("Article.ExecSummHeader");
        public string ListableTitle => Title;
        public string ListableByline => Publication;

        public IEnumerable<ILinkable> ListableTopics
            =>
                GlassModel.Taxonomies?.Take(3)
                    .Select(x => new LinkableModel { LinkableText = x.Item_Name, LinkableUrl = SearchTaxonomyUtil.GetSearchUrl(x) });

        public string ListableType => Media_Type;
        public string Publication => SiterootContext.Item.Publication_Name.StripHtml();

        public virtual bool DisplayImage
        {
            get { return !string.IsNullOrWhiteSpace(ListableImage); }
            set { }
        }

        #endregion                              
    }
}