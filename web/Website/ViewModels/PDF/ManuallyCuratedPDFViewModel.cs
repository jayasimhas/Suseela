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
        public string PDFHeader => SiteRootContext.Item?.PDF_Header;       
        public string PDFFooter => SiteRootContext.Item?.PDF_Footer;

        public string PDFTitle => GlassModel?.Title;
        public string PDFSubTitle => GlassModel?.Body;

    }
}