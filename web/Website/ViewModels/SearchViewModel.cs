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
        
    }
}