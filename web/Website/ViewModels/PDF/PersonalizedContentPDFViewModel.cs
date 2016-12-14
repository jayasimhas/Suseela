using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.PDF;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels.PDF
{
    public class PersonalizedContentPDFViewModel: GlassViewModel<IPersonalized_PDF_Page>
    {
        protected readonly ISiteRootContext SiteRootContext;
        public PersonalizedContentPDFViewModel(ISiteRootContext siteRootContext)
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