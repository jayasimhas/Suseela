using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Informa.Library.Article.Search;
using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels
{

    public class SearchViewModel : GlassViewModel<ISearch>, ISearchViewModel
    {
        protected readonly ITextTranslator TextTranslator;

        public SearchViewModel() { }

        public SearchViewModel(
          ITextTranslator textTranslator)
        {
            TextTranslator = textTranslator;
        }

        public string PageFirstText => TextTranslator.Translate("Search.Page.First");
        public string PageLastText => TextTranslator.Translate("Search.Page.Last");
        public string SearchTipsText => TextTranslator.Translate("Search.Tips");
        public string SearchTitleText => TextTranslator.Translate("Search.Title");
        public string SearchViewHeadlinesOnlyText => TextTranslator.Translate("Search.ViewHeadlinesOnly");
        public string SearchSortByText => TextTranslator.Translate("Search.SortBy");
        public string SearchRelevanceText => TextTranslator.Translate("Search.Relevancy");
        public string SearchShowingResults1Text => TextTranslator.Translate("Search.ShowingResults1");
        public string SearchShowingResults2Text => TextTranslator.Translate("Search.ShowingResults2");
        public string SearchShowingResults3Text => TextTranslator.Translate("Search.ShowingResults3");
        public string SearchShowingResults4Text => TextTranslator.Translate("Search.ShowingResults4");
        public string SearchClearAllText => TextTranslator.Translate("Search.ClearAll");
        public string SearchFilterByText => TextTranslator.Translate("Search.FilterBy");
        
    }
}