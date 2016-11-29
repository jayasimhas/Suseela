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
        public string PdfTitle => GlassModel?.Title;
        public string DownloadLinkText => TextTranslator.Translate("Pdf.DownloadLinkText");
        public string IssueDateHeadlineText => TextTranslator.Translate("Pdf.IssueDateHeadlineText");
        public string IssueNumberHeadlineText => TextTranslator.Translate("Pdf.IssueNumberHeadlineText");
        public IList<Pdf> Pdfs => GetPdfs();
        //public IList<Pdf> ManuallyCuratedPdfs => GetManuallyCuratedPdfs();
        public string userEmail => _authenticatedUserContext.User?.Email;
        public IList<Pdf> GetPdfs()
        {
            var pdfs = new List<Pdf>();


            var homeItem = GlobalService.GetItem<IHome_Page>(SiterootContext.Item._Id.ToString()).
               _ChildrenWithInferType.OfType<IHome_Page>().FirstOrDefault();           
            if (homeItem != null)
            {
                Item PdfsRootItem = GlobalService.GetItem<Item>(homeItem._ChildrenWithInferType.OfType<IPdfs_Root>().FirstOrDefault()._Id);

                if (PdfsRootItem != null)
                {
                    var children = PdfsRootItem.Children;
                    foreach (Item child in children)
                    {
                        child.Fields.ReadAll();
                    }
                    foreach (Item pdf in PdfsRootItem.Children.OrderByDescending(n => n.Statistics.Updated))
                    {
                        if (string.Equals(pdf.TemplateName, "Static PDF Page", StringComparison.OrdinalIgnoreCase))
                        {
                            LinkField targetPdf = pdf.Fields["PDF Url"];
                            pdfs.Add(new Pdf
                            {
                                TypeOfPdf = PdfType.Static,
                                IssueDate = pdf.Statistics.Updated.Date,
                                IssueNumber = pdf["Issue Number"],
                                 PdfTitle=pdf["Title"],
                                PdfPageUrl = !string.IsNullOrEmpty(pdf["PDF Url"]) ? Sitecore.Resources.Media.MediaManager.GetMediaUrl(targetPdf.TargetItem) : string.Empty
                            });
                        }
                        if (pdf.TemplateName == "Manually Curated PDF Page")
                        {
                            pdfs.Add(new Pdf
                            {
                                TypeOfPdf = PdfType.Manual,
                                IssueDate = pdf.Statistics.Updated.Date,
                                IssueNumber = string.Empty,
                                PdfTitle = pdf["Title"],
                                PdfPageUrl = Sitecore.Context.Site.HostName + Sitecore.Links.LinkManager.GetItemUrl(pdf)
                            });
                        }
                    }
                    return pdfs;
                }
            }
            return null;
        }
        //public IList<Pdf> GetManuallyCuratedPdfs()
        //{
        //    var pdfs = new List<Pdf>();
        //    var homeItem = GlobalService.GetItem<IHome_Page>(SiterootContext.Item._Id.ToString()).
        //        _ChildrenWithInferType.OfType<IHome_Page>().FirstOrDefault();
        //    if (homeItem != null)
        //    {
        //        var PdfsRootItem = homeItem._ChildrenWithInferType.OfType<IPdfs_Root>().FirstOrDefault();
        //        var CuratedPdfsRootItem = PdfsRootItem?._ChildrenWithInferType.OfType<IManuallyCuratedPdfs_Root>().FirstOrDefault();
        //        if (PdfsRootItem != null && CuratedPdfsRootItem != null)
        //        {
        //            var curatedPdfs = CuratedPdfsRootItem._ChildrenWithInferType.OfType<IManually_Curated_PDF_Page>();
        //            if (curatedPdfs != null && curatedPdfs.Count() > 0)
        //            {
        //                foreach (var pdf in curatedPdfs)
        //                {
        //                    pdfs.Add(new Pdf { IssueDate = DateTime.Now, IssueNumber = 0, PdfPageUrl = pdf._AbsoluteUrl });
        //                }
        //                return pdfs;
        //            }
        //        }
        //    }
        //    return null;
        //}

    }
}