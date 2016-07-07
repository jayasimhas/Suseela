﻿using System;
using System.Collections;
using Informa.Library.Article.Search;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
using Informa.Library.Utilities.Extensions;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Autofac;
using Glass.Mapper.Sc.Fields;
using Informa.Library.Authors;
using Informa.Library.ContentCuration;
using Informa.Library.Globalization;
using Informa.Library.Search.Utilities;
using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Mvc.Services;
using Jabberwocky.Glass.Autofac.Util;

namespace Informa.Web.ViewModels
{
    public class LatestNewsViewModel : GlassViewModel<IGlassBase>
    {
        protected readonly IArticleSearch ArticleSearch;
        protected readonly IItemManuallyCuratedContent ItemManuallyCuratedContent;
        protected readonly IArticleListItemModelFactory ArticleListableFactory;
        protected readonly ITextTranslator TextTranslator;

        public LatestNewsViewModel(IGlassBase datasource,
            IRenderingContextService renderingParametersService,
            IArticleSearch articleSearch,
            IItemManuallyCuratedContent itemManuallyCuratedContent,
            IArticleListItemModelFactory articleListableFactory,
            ISiteRootContext rootContext,
            ITextTranslator textTranslator)
        {
            ArticleSearch = articleSearch;
            ItemManuallyCuratedContent = itemManuallyCuratedContent;
            ArticleListableFactory = articleListableFactory;
            TextTranslator = textTranslator;

            var parameters = renderingParametersService.GetCurrentRenderingParameters<ILatest_News_Options>();
            DisplayTitle = parameters.Display_Title;
            if (DisplayTitle)
            {
                Topics = parameters.Subjects.Select(s => s.Item_Name).ToArray();
                TitleText = GetTitleText();
            }
            int itemsToDisplay = parameters.Number_To_Display?.Value ?? 6;

            var publicationNames = parameters.Publications.Any()
                ? parameters.Publications.Select(p => p.Publication_Name)
                : new[] { rootContext.Item.Publication_Name };
            
            var authorGuids = GetAuthor();

            if (authorGuids.Any() && parameters.Authors.Any())
            {
                authorGuids.AddRange(parameters.Authors.Select(p => RemoveSpecialCharactersFromGuid(p._Id.ToString())));
            }

            News = GetLatestNews(datasource._Id, parameters.Subjects.Select(s => s._Id), publicationNames, authorGuids, itemsToDisplay);
            SeeAllLink = parameters.Show_See_All ? new Link
            {
                Text = textTranslator.Translate("Article.LatestFrom.SeeAllLink"),
                Url = SearchTaxonomyUtil.GetSearchUrl(parameters.Subjects.ToArray())
            } : null;
        }

        public List<string> GetAuthor()
        {
            List<string> authorGuids = new List<string>();
            var nameFromUrl = HttpContext.Current.Request.Url.Segments.Last();
            using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
            {
                var authorClient = scope.Resolve<IAuthorIndexClient>();
                var author = authorClient.GetAuthorIdByUrlName(nameFromUrl);

                if (author != null)
                {
                    authorGuids.Add(RemoveSpecialCharactersFromGuid(author.ToString()));
                }
            }
            return authorGuids;
        }

        public string RemoveSpecialCharactersFromGuid(string guid)
        {
            return guid.Replace("-", "").Replace("{", "").Replace("}", "").ToLower();
        }

        public IList<string> Topics { get; set; }
        public IEnumerable<IListableViewModel> News { get; set; }
        public string TitleText { get; set; }
        public bool DisplayTitle { get; set; }
        public Link SeeAllLink { get; set; }

        private string GetTitleText()
        {
            var take = Topics.Count - 1;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} {1}",
                TextTranslator.Translate("Article.LatestFrom"),
                string.Join(", ", Topics.Take(take > 0 ? take : 1)));
            if (take > 0)
                sb.AppendFormat(" &amp; {0}", Topics.Last());

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
    }
}