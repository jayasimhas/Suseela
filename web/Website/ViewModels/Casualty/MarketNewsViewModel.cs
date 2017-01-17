using Informa.Library.Globalization;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.ViewModels.Casualty
{
    public class MarketNewsViewModel : GlassViewModel<IGlassBase>
    {
        protected readonly ITextTranslator TextTranslator;
        public MarketNewsViewModel(ITextTranslator textTranslator)
        {
            TextTranslator = textTranslator;
        }
        public string Title => TextTranslator.Translate("Market.News.Component.Title");
    }
}