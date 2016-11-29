using System.IO;
using System.Linq;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Informa.Web.ViewModels.PopOuts;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using Informa.Library.Utilities.Extensions;


namespace Informa.Web.Areas.Account.Controllers
{
    public class PdfDownloadController : Controller
    {
        public string headerContent;
        public string footerContent;

        // GET: Account/PdfDownload
        [ValidateInput(false)]
        public ActionResult DownloadPdf()
        {
            return PartialView("~/Views/Shared/PopOuts/DownloadPdf.cshtml", new PdfDownloadViewModel());
        }
        CommonFooter commonFooter = new CommonFooter();
        public ActionResult GenerateAndDownloadPdf(string pdfPageUrl, string userEmail, string PdfTitle)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 40, 40, 50, 50);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);


                commonFooter.HeadertText = PdfTitle;
                writer.PageEvent = commonFooter;
                document.Open();

                GeneratePdfBody(writer, document, pdfPageUrl, userEmail);
                writer.StrictImageSequence = true;

                //Add meta information to the document
                document.AddAuthor("Micke Blomquist");
                document.AddCreator("Sample application using iTextSharp");
                document.AddKeywords("PDF tutorial education");
                document.AddSubject("Document subject - Describing the steps creating a PDF document");
                document.AddTitle("The document title - PDF creation using iTextSharp");

                document.Close();
                writer.Close();

                System.Web.HttpContext.Current.Response.ContentType = "pdf/application";
                //System.Web.HttpContext.Current.Response.AddHeader("content-disposition","attachment;filename=First PDF document.pdf");
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=First PDF document.pdf");
                System.Web.HttpContext.Current.Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            }
            return new EmptyResult();
        }

        private void GeneratePdfBody(PdfWriter writer, Document document, string pdfPageUrl, string userEmail)
        {

            if (!string.IsNullOrEmpty(pdfPageUrl))
            {

                string url = pdfPageUrl;
                var fullPageHtml = System.IO.File.ReadAllText(url);

                //pdfPageUrl = HttpContext.Request.Url.Scheme + "://" + pdfPageUrl;
                //HttpWebRequest webRequest = WebRequest.Create(pdfPageUrl) as HttpWebRequest;
                //webRequest.Method = "POST";
                //System.Net.WebClient client = new WebClient();
                //byte[] data = client.DownloadData(pdfPageUrl);
                //string fullPageHtml = Encoding.UTF8.GetString(data);


                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(fullPageHtml);
                string html = doc.GetElementbyId("mainContentPdf").InnerHtml;

                var replacements = new Dictionary<string, string>
                {
                    ["#UserName#"] = userEmail,
                    ["#TodayDate#"] = DateTime.Now.ToString("dd MMM yyyy")
                };
                html = html.ReplacePatternCaseInsensitive(replacements);
                string decodedHtml = HtmlEntity.DeEntitize(html);

                HtmlDocument ReqdDoc = new HtmlDocument();
                ReqdDoc.LoadHtml(decodedHtml);

                var articleTitleNode = ReqdDoc.DocumentNode.SelectSingleNode("//a[@class='article-title']");
                var articleURL = HttpContext.Request.Url.Scheme + "://" + articleTitleNode?.Attributes["href"].Value;

                var articleNodes = ReqdDoc.DocumentNode.SelectNodes("//div[@class='article-body-content']")?.ToList();
                if (articleNodes != null && articleNodes.Any())
                {
                    foreach (var articleNode in articleNodes)
                    {
                        //AMcharts in Article
                        var amchartNodes = articleNode.SelectNodes("//div[@class='amchart-section-pdf']")?.ToList();
                        if (amchartNodes != null && amchartNodes.Any())
                        {
                            foreach (var amchartNode in amchartNodes)
                            {
                                amchartNode.InnerHtml = "<h3>This Page has data tool, please click on below link to see more details</h3> \n <a href=\"" + articleURL + "\">click here</a>";
                            }
                        }

                        //Tableau in Article
                        var tableauNodes = articleNode.SelectNodes("//div[@class='article-data-tool-placeholder']")?.ToList();
                        if (tableauNodes != null && tableauNodes.Any())
                        {
                            foreach (var tableauNode in tableauNodes)
                            {
                                tableauNode.InnerHtml = "<h3>This Page has data tool, please click on below link to see more details</h3> \n <a href=\"" + articleURL + "\">click here</a>";
                            }
                        }
                    }
                }


                HtmlNode CommonFooterNode = ReqdDoc.DocumentNode.SelectSingleNode("//div[@class='pdf-footer']");
                if (CommonFooterNode != null)
                {
                    commonFooter.FooterText = CommonFooterNode.InnerText;
                    CommonFooterNode.ParentNode.RemoveChild(CommonFooterNode);
                }
                var domain = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host;

                //var images = ReqdDoc.DocumentNode.SelectNodes("//img/@src")?.ToList();
                //if (images != null && images.Any())
                //{
                //    foreach (HtmlNode img in images)
                //    {
                //        if (!img.Attributes["src"].Value.StartsWith("http") && !img.Attributes["src"].Value.StartsWith("https") && !img.Attributes["src"].Value.StartsWith("www"))
                //        {
                //            img.SetAttributeValue("src", domain + img.Attributes["src"].Value);
                //        }
                //    }
                //}

                var links = ReqdDoc.DocumentNode.SelectNodes("//a/@href")?.ToList();
                if (links != null && links.Any())
                {
                    foreach (var link in links)
                    {
                        if (!link.Attributes["href"].Value.StartsWith("http") && !link.Attributes["href"].Value.StartsWith("https") && !link.Attributes["href"].Value.StartsWith("www"))
                        {
                            link.SetAttributeValue("href", domain + link.Attributes["href"].Value);
                        }
                    }
                }

                ReqdDoc.OptionOutputAsXml = true;
                ReqdDoc.OptionCheckSyntax = true;
                ReqdDoc.OptionFixNestedTags = true;

                using (var srHtml = new StringReader(ReqdDoc.DocumentNode.InnerHtml))
                {
                    //Parse the HTML
                    iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, srHtml);
                }
            }
        }

        private string RelpaceChartWithImage(HtmlNode chartNode)
        {
            //Chart1.SaveImage(memoryStream, ChartImageFormat.Png);
            //Image img = Image.GetInstance(memoryStream.GetBuffer();
            //img.ScalePercent(75f);
            //Doc.Add(img);
            //Doc.Close();

            return string.Empty;
        }
    }

    public class CommonFooter : PdfPageEventHelper
    {

        // This is the contentbyte object of the writer
        PdfContentByte cb;

        // we will put the final number of pages in a template
        PdfTemplate headerTemplate, footerTemplate;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;

        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;
        public string HeadertText { get; set; }
        public string FooterText { get; set; }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                headerTemplate = cb.CreateTemplate(100, 100);
                footerTemplate = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {

            }
            catch (IOException ioe)
            {

            }
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            Font baseFontNormal = new Font(Font.FontFamily.HELVETICA, 12f, Font.NORMAL, BaseColor.BLACK);
            Font baseFontBig = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD, BaseColor.BLACK);
            string PageNumber = writer.PageNumber.ToString();

            //Add paging to footer
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 12);
                cb.SetTextMatrix(document.PageSize.GetRight(40), document.PageSize.GetBottom(30));
                cb.ShowText(PageNumber);
                cb.EndText();
            }

            //Adding common header from page number-2
            if (writer.PageNumber > 1)
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 12);
                cb.SetTextMatrix(document.PageSize.GetLeft(40), document.PageSize.GetTop(45));
                cb.ShowText(!string.IsNullOrEmpty(HeadertText) ? HeadertText : string.Empty);
                cb.EndText();
            }
            //Adding common footer
            cb.BeginText();
            cb.SetFontAndSize(bf, 12);
            cb.SetTextMatrix(document.PageSize.GetLeft(40), document.PageSize.GetBottom(30));
            cb.ShowText(!string.IsNullOrEmpty(FooterText) ? FooterText : string.Empty);
            //cb.ShowText("Hi");
            cb.EndText();

            //Move the pointer and draw line to separate header section from rest of page
            if (writer.PageNumber == 1)
            {
                cb.MoveTo(40, document.PageSize.Height - 100);
                cb.LineTo(document.PageSize.Width - 25, document.PageSize.Height - 100);
                cb.Stroke();
                cb.SetColorStroke(BaseColor.GRAY);
            }
            else
            {
                cb.MoveTo(40, document.PageSize.Height - 50);
                cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 50);
                cb.Stroke();
                cb.SetColorStroke(BaseColor.GRAY);
            }

            //Move the pointer and draw line to separate footer section from rest of page
            cb.MoveTo(40, document.PageSize.GetBottom(50));
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
            cb.Stroke();
            cb.SetColorStroke(BaseColor.GRAY);
        }
    }
}
