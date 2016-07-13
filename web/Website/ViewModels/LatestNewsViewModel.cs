using System;
using Informa.Library.Article.Search;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
using Informa.Library.Utilities.Extensions;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Fields;
using Informa.Library.Authors;
using Informa.Library.ContentCuration;
using Informa.Library.Globalization;
using Informa.Library.Search.Utilities;
using Informa.Library.Site;
using Informa.Library.Utilities.WebUtils;
using Informa.Model.DCD;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Web.ViewModels.Articles;
using Jabberwocky.Glass.Autofac.Mvc.Services;

namespace Informa.Web.ViewModels
{
    public class LatestNewsViewModel : GlassViewModel<IGlassBase>
    {
        protected readonly IArticleSearch ArticleSearch;
        protected readonly IItemManuallyCuratedContent ItemManuallyCuratedContent;
        protected readonly IArticleListItemModelFactory ArticleListableFactory;
        protected readonly ITextTranslator TextTranslator;
        protected readonly ISitecoreService SitecoreService;
        protected readonly IAuthorIndexClient AuthorIndexClient;
        protected IDCDReader DcdReader;

        public LatestNewsViewModel(IGlassBase datasource,
            IRenderingContextService renderingParametersService,
            IArticleSearch articleSearch,
            IItemManuallyCuratedContent itemManuallyCuratedContent,
            IArticleListItemModelFactory articleListableFactory,
            ISiteRootContext rootContext,
            ITextTranslator textTranslator, ISitecoreService sitecoreService, IAuthorIndexClient authorIndexClient, IDCDReader dcdReader)
        {
            ArticleSearch = articleSearch;
            ItemManuallyCuratedContent = itemManuallyCuratedContent;
            ArticleListableFactory = articleListableFactory;
            TextTranslator = textTranslator;
            SitecoreService = sitecoreService;
            AuthorIndexClient = authorIndexClient;
            DcdReader = dcdReader;

            Authors = new List<string>();
            Parameters = renderingParametersService.GetCurrentRenderingParameters<ILatest_News_Options>();
            if (Parameters == null) return;

            DisplayTitle = Parameters.Display_Title;
            ItemsToDisplay = Parameters.Number_To_Display?.Value ?? 6;
            SeeAllLink = Parameters.Show_See_All ? new Link
            {
                Text = TextTranslator.Translate("Article.LatestFrom.SeeAllLink")
            } : null;
            var publicationNames = Parameters.Publications.Any()
                ? Parameters.Publications.Select(p => p.Publication_Name)
                : new[] { rootContext.Item.Publication_Name };

            Authors = Parameters.Authors.Select(p => RemoveSpecialCharactersFromGuid(p._Id.ToString())).ToArray();

            if (datasource._TemplateId.ToString() == IAuthor_PageConstants.TemplateIdString)
            {
                Author_Page();
            }
            else if (datasource._TemplateId.ToString() == ICompany_PageConstants.TemplateIdString)
            {
                Company_Page();
            }
            else
            {
                Other_Page();
            }

            News = GetLatestNews(datasource._Id, Parameters.Subjects.Select(s => s._Id), publicationNames, Authors, ItemsToDisplay);
        }

        public void Author_Page()
        {
            var nameFromUrl = HttpContext.Current.Request.Url.Segments.Last();
            Guid? author = AuthorIndexClient.GetAuthorIdByUrlName(nameFromUrl);
            if (author == null) return;

            var currentAuthor = SitecoreService.GetItem<IStaff_Item>(author.Value);
            if (currentAuthor == null) return;

            if (DisplayTitle)
            {
                TitleText = GetTitleText($"{currentAuthor.First_Name} {currentAuthor.Last_Name}");
            }

            Authors = new List<string> { RemoveSpecialCharactersFromGuid(author.ToString()) };

            if (SeeAllLink != null)
            {
                SeeAllLink.Url = string.Format("/search#?author={0}", RemoveSpecialCharactersFromGuid(author.ToString()));
            }
        }

        public void Company_Page()
        {
            ItemsToDisplay = 4;
            CompanyRecordNumber = HttpContext.Current.Request.Url.Segments.Last();
            RedirectIfRecordId(CompanyRecordNumber);

            var company = DcdReader.GetCompanyByRecordNumber(CompanyRecordNumber);

            if (DisplayTitle)
            {
                TitleText = GetTitleText(company.Title);
            }

        }
        
        public void Other_Page()
        {
            if (DisplayTitle)
            {
                Topics = Parameters.Subjects.Select(s => s.Item_Name).ToArray();
                TitleText = GetTitleText();
            }

            if (SeeAllLink != null)
            {
                SeeAllLink.Url = SearchTaxonomyUtil.GetSearchUrl(Parameters.Subjects.ToArray());
            }
        }

        public IList<string> Topics { get; set; }
        public IList<string> Authors { get; set; }
        public string CompanyRecordNumber { get; set; }
        public IEnumerable<IListableViewModel> News { get; set; }
        public string TitleText { get; set; }
        public bool DisplayTitle { get; set; }
        public Link SeeAllLink { get; set; }
        public ILatest_News_Options Parameters { get; set; }
        public int ItemsToDisplay { get; set; }

        private string GetTitleText()
        {
            var take = Topics.Count - 1;
            var title = GetTitleText(string.Join(", ", Topics.Take(take > 0 ? take : 1)));
            if (take > 0)
            {
                title = title + "&amp;" + Topics.Last();
            }
            return title;
 ;       }

        private string GetTitleText(string title)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} {1}",TextTranslator.Translate("Article.LatestFrom"), title);
            return sb.ToString();
        }

        private IEnumerable<IListableViewModel> GetLatestNews(Guid datasourceId, IEnumerable<Guid> subjectIds, IEnumerable<string> publicationNames, IEnumerable<string> authorNames,
            int itemsToDisplay)
        {
            var manuallyCuratedContent = ItemManuallyCuratedContent.Get(datasourceId);
            var filter = ArticleSearch.CreateFilter();

            filter.Page = 1;
            filter.PageSize = itemsToDisplay;
            filter.ExcludeManuallyCuratedItems.AddRange(manuallyCuratedContent);
            filter.TaxonomyIds.AddRange(subjectIds);
            filter.PublicationNames.AddRange(publicationNames);
            filter.AuthorNames.AddRange(authorNames);

            var results = ArticleSearch.Search(filter);
            var articles =
                results.Articles.Where(a => a != null)
                    .Select(a => ArticleListableFactory.Create(a).Alter(l => l.DisplayImage = false));

            return articles;
        }
        public string RemoveSpecialCharactersFromGuid(string guid)
        {
            return guid.Replace("-", "").Replace("{", "").Replace("}", "").ToLower();
        }

        private void RedirectIfRecordId(string segment)
        {
            if (!segment.StartsWith("_id")) { return; }  //Not a record id, yay!

            int id;
            if (!int.TryParse(segment.Substring(3), out id)) return;

            var company = DcdReader.GetCompanyByRecordId(id);
            if (!(company?.RecordNumber).HasContent()) return;
            HttpContext.Current.Response.Redirect($"/companies/{company.RecordNumber}");
        }
    }
}