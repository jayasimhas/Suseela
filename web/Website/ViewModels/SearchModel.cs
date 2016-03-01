using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Informa.Library.Article.Search;
using Informa.Library.Globalization;
using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class SearchModel : ISearchModel
    {
        protected readonly ITextTranslator TextTranslator;

        public SearchModel() { }

        public SearchModel(
          ITextTranslator textTranslator)
        {
            TextTranslator = textTranslator;
        }

        public string PageFirstText => TextTranslator.Translate("Search.Page.First");
        public string PageLastText => TextTranslator.Translate("Search.Page.Last");

    }
}