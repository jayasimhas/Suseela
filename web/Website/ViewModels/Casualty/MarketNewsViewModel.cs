using Informa.Library.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.ViewModels.Casualty
{
    public class MarketNewsViewModel
    {
        protected readonly ITextTranslator TextTranslator;
        public MarketNewsViewModel(ITextTranslator textTranslator)
        {
            TextTranslator = textTranslator;
        }
        public string Title => TextTranslator.Translate("Market.News.Component.Title");
    }
}