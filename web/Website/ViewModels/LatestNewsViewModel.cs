﻿using System;
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
using Informa.Library.Utilities.References;
using Informa.Model.DCD;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Web.ViewModels.Articles;
using Informa.Library.Search.ComputedFields.Facets;
using Jabberwocky.Glass.Autofac.Mvc.Services;
using System.Diagnostics;

namespace Informa.Web.ViewModels
{
    public class LatestNewsViewModel : GlassViewModel<IGlassBase>
    {
        protected readonly IArticleSearch ArticleSearch;
        protected readonly IItemManuallyCuratedContent ItemManuallyCuratedContent;
        protected readonly IArticleListItemModelFactory ArticleListableFactory;
        protected readonly ITextTranslator TextTranslator;
        protected readonly IAuthorService AuthorService;
        protected IDCDReader DcdReader;
        protected IGlassBase Datasource;

        public LatestNewsViewModel(IGlassBase datasource,
            IRenderingContextService renderingParametersService,
            IArticleSearch articleSearch,
            IItemManuallyCuratedContent itemManuallyCuratedContent,
            IArticleListItemModelFactory articleListableFactory,
            ISiteRootContext rootContext,
            ITextTranslator textTranslator,
            IAuthorService authorService,
            IDCDReader dcdReader)
        {
            Datasource = datasource;
            ArticleSearch = articleSearch;
            ItemManuallyCuratedContent = itemManuallyCuratedContent;
            ArticleListableFactory = articleListableFactory;
            TextTranslator = textTranslator;
            AuthorService = authorService;
            DcdReader = dcdReader;

            Stopwatch sw = Stopwatch.StartNew();
            StringExtensions.WriteSitecoreLogs("Reading rendering parameters at:", sw, "Latest News");

            Authors = new List<string>();
            Parameters = renderingParametersService.GetCurrentRenderingParameters<ILatest_News_Options>();

            StringExtensions.WriteSitecoreLogs("Reading rendering parameters ends at:", sw, "Latest News");

            if (Parameters == null) return;

            DisplayTitle = Parameters.Display_Title;
            ItemsToDisplay = Parameters.Number_To_Display?.Value ?? 6;
            SeeAllLink = Parameters.Show_See_All ? new Link
            {
                Text = TextTranslator.Translate("Article.LatestFrom.SeeAllLink")
            } : null;

            StringExtensions.WriteSitecoreLogs("Reading publicationNames at:", sw, "Latest News");

            var publicationNames = Parameters.Publications.Any()
                ? Parameters.Publications.Select(p => p.Publication_Name)
                : new[] { rootContext.Item.Publication_Name };

            StringExtensions.WriteSitecoreLogs("Reading publicationNames ends at:", sw, "Latest News");

            StringExtensions.WriteSitecoreLogs("Reading Authers at:", sw, "Latest News");

            Authors = Parameters.Authors?.Select(p => RemoveSpecialCharactersFromGuid(p._Id.ToString())).ToArray();
            CompanyRecordNumbers = string.IsNullOrEmpty(Parameters.CompanyID)
                ? (IList<string>)new List<string>()
                : Parameters.CompanyID.Split(',');

            StringExtensions.WriteSitecoreLogs("Reading Authers ends at:", sw, "Latest News");

            if (IsAuthorPage)
            {
                Author_Page();
                if (!Parameters.Publications.Any()) // authors page shouldn't filter on the current publication
                    publicationNames = Enumerable.Empty<string>();
            }
            else if (datasource._TemplateId.ToString() == ICompany_PageConstants.TemplateIdString)
            {
                Company_Page();
                if (!Parameters.Publications.Any()) // authors page shouldn't filter on the current publication
                    publicationNames = Enumerable.Empty<string>();
            }
            else
            {
                Other_Page();
            }

            News = GetLatestNews(datasource._Id, Parameters.Subjects.Select(s => s._Id), publicationNames, Authors, CompanyRecordNumbers, ItemsToDisplay);
        }

        private bool IsAuthorPage => Datasource._TemplateId.ToString() == IAuthor_PageConstants.TemplateIdString;
        private bool IsCompanyPage => Datasource._TemplateId.ToString() == ICompany_PageConstants.TemplateIdString;

        private IStaff_Item CurrentAuthor => AuthorService.GetCurrentAuthor();

        private string CurrentAuthorName => (CurrentAuthor != null) ? $"{CurrentAuthor.First_Name} {CurrentAuthor.Last_Name}" : string.Empty;

        public void Author_Page()
        {
            if (DisplayTitle)
                TitleText = GetTitleText(CurrentAuthorName);

            Authors = new List<string> { RemoveSpecialCharactersFromGuid(CurrentAuthor._Id.ToString()) };

            if (SeeAllLink != null)
                SeeAllLink.Url = string.Format("/search#?{0}={1}", Constants.QueryString.AuthorFullName, AuthorNamesField.ToAuthorName(CurrentAuthor));
        }
        public string AnalyticsName { get; private set; }
        public void Company_Page()
        {
            ItemsToDisplay = 4;
            var recordNumber = HttpContext.Current.Request.Url.Segments.Last();
            bool isFound = RedirectIfRecordId(recordNumber);
            if (isFound)
            {
                CompanyRecordNumbers = new List<string> { recordNumber };

                var company = DcdReader.GetCompanyByRecordNumber(recordNumber);
                AnalyticsName = company.Title;

                if (DisplayTitle)
                {
                    TitleText = GetTitleText(company.Title);
                }
                if (SeeAllLink != null)
                {
                    SeeAllLink.Url = string.Format("/search#?{0}={1}", Constants.QueryString.Company, company.Title);
                }
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
                string url = SearchTaxonomyUtil.GetSearchUrl(Parameters.Subjects.ToArray());
                if (Authors.Count > 0)
                {
                    var appender = (url.Contains("?")) ? "&" : string.Empty;
                    url = $"{url}{appender}author={string.Join(",", Authors)}";
                }
                if (Parameters.Publications?.Count() > 0)
                {
                    var appender = (url.Contains("?")) ? "&" : string.Empty;
                    url = $"{url}{appender}publication={string.Join(";", Parameters.Publications.Select(s => s.Publication_Name))}";
                }
                SeeAllLink.Url = url;
            }
        }

        public IList<string> Topics { get; set; }
        public IList<string> Authors { get; set; }
        public IList<string> CompanyRecordNumbers { get; set; }
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
                title = title + " &amp;" + Topics.Last();
            }
            return title;
        }

        public string EventSourceValue =>
            IsAuthorPage ? CurrentAuthorName
                : IsCompanyPage ? $"{AnalyticsName} articles"
                    : TitleText;

        public string EventSource
            => IsAuthorPage ? "see_all_articles"
                : IsCompanyPage ? "see_all_deals"
                    : "see_all_topic";

        private string GetTitleText(string title)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} {1}", TextTranslator.Translate("Article.LatestFrom"), title);
            return sb.ToString();
        }

        private IEnumerable<IListableViewModel> GetLatestNews(Guid datasourceId, IEnumerable<Guid> subjectIds, IEnumerable<string> publicationNames,
            IEnumerable<string> authorGuids, IEnumerable<string> companyRecordNumbers, int itemsToDisplay)
        {
            Stopwatch sw = Stopwatch.StartNew();
            StringExtensions.WriteSitecoreLogs("Reading GetLatestNews at:", sw, "Latest News");

            var manuallyCuratedContent = ItemManuallyCuratedContent.Get(datasourceId);
            var filter = ArticleSearch.CreateFilter();

            filter.Page = 1;
            filter.PageSize = itemsToDisplay;
            if (manuallyCuratedContent != null) filter.ExcludeManuallyCuratedItems.AddRange(manuallyCuratedContent);
            if (subjectIds != null) filter.TaxonomyIds.AddRange(subjectIds);
            if (publicationNames != null) filter.PublicationNames.AddRange(publicationNames);
            if (authorGuids != null) filter.AuthorGuids.AddRange(authorGuids);
            if (companyRecordNumbers != null) filter.CompanyRecordNumbers.AddRange(companyRecordNumbers);

            var results = ArticleSearch.Search(filter);
            var articles =
                results.Articles.Where(a => a != null)
                    .Select(a => ArticleListableFactory.Create(a).Alter(l => l.DisplayImage = false));
            if (IsAuthorPage) // articles pages are wildcard and don't have a title
                articles = articles.Select(a => a.Alter(b => b.PageTitle = CurrentAuthorName));

            StringExtensions.WriteSitecoreLogs("Reading GetLatestNews ends at:", sw, "Latest News");
            return articles;
        }

        public string RemoveSpecialCharactersFromGuid(string guid)
        {
            return guid.Replace("-", "").Replace("{", "").Replace("}", "").ToLower();
        }

        private bool RedirectIfRecordId(string segment)
        {
            if (!segment.StartsWith("_id")) { return true; }  //Not a record id, yay!

            int id;
            if (!int.TryParse(segment.Substring(3), out id)) return true;

            var company = DcdReader.GetCompanyByRecordId(id);
            if (!(company?.RecordNumber).HasContent()) return true;
            if (!HttpContext.Current.Response.IsRequestBeingRedirected)
                HttpContext.Current.Response.Redirect($"/companies/{company.RecordNumber}", true);
            return false;
        }
    }
}