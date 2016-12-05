using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.PDF;

namespace Informa.Web.ViewModels.PDF
{
    public class ManuallyCuratedPDFViewModel : GlassViewModel<IManually_Curated_PDF_Page>
    {
        protected readonly ISiteRootContext SiteRootContext;
        public ManuallyCuratedPDFViewModel(ISiteRootContext siteRootContext)
        {
            SiteRootContext = siteRootContext;
        }
        /// <summary>
        /// PDF Header
        /// </summary>
        public string PDFHeader => SiteRootContext.Item?.PDF_Header;
        /// <summary>
        /// PDF Footer
        /// </summary>
        public string PDFFooter => SiteRootContext.Item?.PDF_Footer;
        /// <summary>
        /// PDF Title
        /// </summary>
        public string PDFTitle => GlassModel?.Title;
        /// <summary>
        /// PDF SubTitle
        /// </summary>
        public string PDFSubTitle => !string.IsNullOrEmpty(GlassModel?.Sub_Title) ? GlassModel?.Sub_Title : GlassModel?.Body;
    }
}