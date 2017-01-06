using Glass.Mapper.Sc.Fields;
using Informa.Library.Globalization;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System;

namespace Informa.Web.ViewModels.BuyersGuide
{
    public class BuyersDetailsViewModel: GlassViewModel<IBuyers_Detail_Page>
    {
        protected readonly ITextTranslator TextTranslator;
        public BuyersDetailsViewModel(ITextTranslator textTranslator)
        {
            TextTranslator = textTranslator;
        }

        /// <summary>
        /// Banner
        /// </summary>
        public Image Banner => GlassModel?.Banner;

        /// <summary>
        /// Agrow Logo
        /// </summary>
        public Image AgrowLogo => GlassModel?.AgrowLogo;

        /// <summary>
        /// Job Title
        /// </summary>
        public string AgrowTitle => GlassModel?.Title;
        /// <summary>
        /// Job SubTitle
        /// </summary>
        public string AgrowSubTitle => GlassModel?.Sub_Title;
        /// <summary>
        /// Job Description
        /// </summary>
        public string AgrowDescription => GlassModel?.Body;
        /// <summary>
        /// Published Text
        /// </summary>
        public string PublishedText => TextTranslator.Translate("JobsAndClassifieds.PublishedText");
    }
}
