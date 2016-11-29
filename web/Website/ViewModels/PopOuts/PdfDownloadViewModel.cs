using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Web.Mvc;

namespace Informa.Web.ViewModels.PopOuts
{
    [ValidateInput(false)]
    public class PdfDownloadViewModel
    {
        public string Title => "Pdf Download component";
        public string PdfDownloadButtonText => "Pdf Download";        
        public string MainContentHtml { get; set;}
        public string pdfPageUrl { get; set; }
    }
}