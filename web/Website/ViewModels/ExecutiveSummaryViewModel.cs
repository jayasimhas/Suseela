using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Informa.Library.Globalization;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels {
    public class ExecutiveSummaryViewModel : GlassViewModel<IGeneral_Content_Page> {

        private ITextTranslator TextTranslator;

        public ExecutiveSummaryViewModel(
            ITextTranslator textTranslator)
        {
            TextTranslator = textTranslator;
        }

        public string ExecutiveSummary => TextTranslator.Translate("SharedContent.ExecutiveSummary");
    }
}