using Glass.Mapper.Sc.Fields;
using Informa.Library.Globalization;
using Informa.Library.PDF;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.PDF;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Informa.Web.ViewModels.MyView;
using Informa.Web.Models;

namespace Informa.Web.ViewModels.PDF
{
    public class PDFLibraryViewModel : GlassViewModel<IManually_Curated_PDF_Page>
    {

        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ISiteRootContext SiterootContext;
        protected readonly ITextTranslator TextTranslator;
        private readonly IAuthenticatedUserContext _authenticatedUserContext;

        public PDFLibraryViewModel(IGlobalSitecoreService globalService, ISiteRootContext siterootContext, ITextTranslator textTranslator,
            IAuthenticatedUserContext authenticatedUserContext)
        {
            GlobalService = globalService;
            SiterootContext = siterootContext;
            TextTranslator = textTranslator;
            _authenticatedUserContext = authenticatedUserContext;
        }
        /// <summary>
        /// PDF title
        /// </summary>
        public string PdfTitle => GlassModel?.Title;
        /// <summary>
        /// Download Link Text
        /// </summary>
        public string DownloadLinkText => TextTranslator.Translate("Pdf.DownloadLinkText");
        /// <summary>
        /// Issue Date Header Text
        /// </summary>
        public string IssueDateHeadlineText => TextTranslator.Translate("Pdf.IssueDateHeadlineText");
        /// <summary>
        /// Issue Number Header Text
        /// </summary>
        public string IssueNumberHeadlineText => TextTranslator.Translate("Pdf.IssueNumberHeadlineText");
        /// <summary>
        /// Data tool link description
        /// </summary>
        public string DataToolLinkDesc => TextTranslator.Translate("DataToolLandingPage.Link.Description");
        /// <summary>
        /// Data tool link Text
        /// </summary>
        public string DataToolLinkText => TextTranslator.Translate("DataToolLandingPage.Link.Text");

        /// <summary>
        /// Method call to Get PDFs
        /// </summary>
        public IList<Pdf> Pdfs => GetPdfs();
        /// <summary>
        /// Method call to Get Personalized PDFs
        /// </summary>
        public IList<Pdf> PersonalizedPdfs => GetPersonalizedPdfs();
        /// <summary>
        /// User email Id
        /// </summary>
        public string userEmail => _authenticatedUserContext.User?.Email;
        /// <summary>
        /// Method to get all published PDFs
        /// </summary>
        /// <returns>List of PDFs</returns>
        public IList<Pdf> GetPdfs()
        {
            var pdfs = new List<Pdf>();

            var currentItem = GlobalService.GetItem<IGeneral_Content_Page>(Sitecore.Context.Item.ID.ToString()); 

            if (currentItem != null)
            {
                var PdfsRootItem = currentItem._ChildrenWithInferType.OfType<IPage_Assets>().FirstOrDefault();

                if (PdfsRootItem != null)
                {
                    //Static PDFs
                    var staticPdfs = PdfsRootItem._ChildrenWithInferType.OfType<IStatic_PDF_Page>();
                    if(staticPdfs!=null && staticPdfs.Any())
                    {
                        foreach (var staticPdf in staticPdfs)
                        {
                            string linkType = staticPdf.PDF_Url?.Type.ToString();
                            string link=string.Empty;

                            if (staticPdf.PDF_Url != null&& string.Equals(linkType, "External", StringComparison.OrdinalIgnoreCase))
                            {
                                link = staticPdf.PDF_Url?.Url;
                            }
                            else if (staticPdf.PDF_Url != null && string.Equals(linkType, "Internal", StringComparison.OrdinalIgnoreCase))
                            {
                               link = Sitecore.Resources.Media.MediaManager.GetMediaUrl(GlobalService.GetItem<Item>(new Guid(staticPdf.PDF_Url?.TargetId.ToString())));
                            }

                            pdfs.Add(new Pdf
                            {
                                TypeOfPdf = PdfType.Static,
                                IssueDate = staticPdf.Issue_Date,
                                IssueNumber = staticPdf.Issue_Number,
                                PdfTitle = staticPdf.Title,
                                PdfPageUrl = link
                            });
                        }
                    }
                    //Manually Curated PDFs
                    var manualPdfs = PdfsRootItem._ChildrenWithInferType.OfType<IManually_Curated_PDF_Page>();

                    if (manualPdfs != null && manualPdfs.Any())
                    {
                        foreach (var manualPdf in manualPdfs)
                        {
                            pdfs.Add(new Pdf
                            {
                                TypeOfPdf = PdfType.Manual,
                                IssueDate = manualPdf.Issue_Date,
                                IssueNumber = manualPdf.Issue_Number,
                                PdfTitle = manualPdf.Title,
                                PdfPageUrl = manualPdf._AbsoluteUrl
                            });
                        }
                    }
                    return pdfs.OrderByDescending(n=>n.IssueDate).ToList();
                }
            }
            return null;
        }

        /// <summary>
        /// Method to get all personalized PDFs
        /// </summary>
        /// <returns>Personalized PDFs</returns>
        public IList<Pdf> GetPersonalizedPdfs()
        {
            var pdfs = new List<Pdf>();

            var currentItem = GlobalService.GetItem<IGeneral_Content_Page>(Sitecore.Context.Item.ID.ToString());

            var homeItem = GlobalService.GetItem<IHome_Page>(SiterootContext.Item._Id.ToString()).
               _ChildrenWithInferType.OfType<IHome_Page>().FirstOrDefault();

            bool enablePersonlize = SiterootContext.Item.Enable_MyView_Toggle;

            if (currentItem != null && enablePersonlize)
            {
                //var PdfsRootItem = homeItem._ChildrenWithInferType.OfType<IPdfs_Root>().FirstOrDefault();
                var PdfsRootItem = currentItem._ChildrenWithInferType.OfType<IPage_Assets>().FirstOrDefault();

                if (PdfsRootItem != null)
                {
                    //Personalized PDFs 
                    var personalizedPdfs = PdfsRootItem._ChildrenWithInferType.OfType<IPersonalized_PDF_Page>();
                    if (personalizedPdfs != null && personalizedPdfs.Any())
                    {
                        foreach (var personalizePdfs in personalizedPdfs)
                        {
                            pdfs.Add(new Pdf
                            {
                                TypeOfPdf = PdfType.Personalized,
                                IssueDate = personalizePdfs.Issue_Date,
                                PubStartDate = personalizePdfs.Publish_StartDate,
                                PubEndDate = personalizePdfs.Publish_EndDate,
                                ArticleSize = personalizePdfs.Article_Size,
                                PdfPageUrl = personalizePdfs._AbsoluteUrl,
                                PdfTitle = personalizePdfs.Title,
                            });
                        }
                    }
                    return pdfs;
                }
            }
            return null;
        }
    }
}