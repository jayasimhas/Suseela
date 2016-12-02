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
        GlobalElements globalElement = new GlobalElements();

        /// <summary>
        /// Controller method for downloading pdf
        /// </summary>
        /// <param name="pdfPageUrl">Page URL</param>
        /// <param name="userEmail">User email who is downloading PDF</param>
        /// <param name="PdfTitle">PDF Title</param>
        /// <param name="DataToolLinkDesc">Data tool description</param>
        /// <param name="DataToolLinkText">Data Tool Link Text</param>
        /// <returns></returns>
        public ActionResult GenerateAndDownloadPdf(string pdfPageUrl, string userEmail, string PdfTitle, string DataToolLinkDesc, string DataToolLinkText)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document();
                if (document.PageNumber == 1)
                {
                    document.SetMargins(40, 40, 10, 30);
                }
                else
                {
                    document.SetMargins(40, 40, 50, 70);
                }
                PdfWriter writer = PdfWriter.GetInstance(document, ms);


                globalElement.CommonHeader = PdfTitle;
                writer.PageEvent = globalElement;
                document.Open();

                GeneratePdfBody(writer, document, pdfPageUrl, userEmail, DataToolLinkDesc, DataToolLinkText);
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
        /// <summary>
        /// Generating PDF Body
        /// </summary>
        /// <param name="writer">PDF writer</param>
        /// <param name="document">PDF Document</param>
        /// <param name="pdfPageUrl">Page URL</param>
        /// <param name="userEmail">User email who is downloading PDF</param>
        /// <param name="PdfTitle">PDF Title</param>
        /// <param name="DataToolLinkDesc">Data tool description</param>
        /// <param name="DataToolLinkText">Data Tool Link Text</param>
        private void GeneratePdfBody(PdfWriter writer, Document document, string pdfPageUrl, string userEmail, string DataToolLinkDesc, string DataToolLinkText)
        {
           
            if (!string.IsNullOrEmpty(pdfPageUrl))
            {
                #region For Static HTML
                //string url = pdfPageUrl;
                //var fullPageHtml = System.IO.File.ReadAllText(url);
                #endregion

                if (!pdfPageUrl.StartsWith("http"))
                {
                    pdfPageUrl = HttpContext.Request.Url.Scheme + "://" + pdfPageUrl;
                }
                HttpWebRequest webRequest = WebRequest.Create(pdfPageUrl) as HttpWebRequest;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
                webRequest.Method = "POST";
                System.Net.WebClient client = new WebClient();
                byte[] data = client.DownloadData(pdfPageUrl);
                string fullPageHtml = Encoding.UTF8.GetString(data);


                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(fullPageHtml);
                string html = doc.GetElementbyId("mainContentPdf").InnerHtml;

                var replacements = new Dictionary<string, string>
                {
                    ["<p>"] = "<p style=\"color:#58595b; font-size:18px; line-height:30px;\">",
                    ["</p>"] = "</p><br /><br />",
                    ["#UserName#"] = userEmail,
                    ["#HeaderDate#"] = DateTime.Now.ToString("dd MMMM yyyy"),
                    ["#FooterDate#"] = DateTime.Now.ToString("dd MMM yyyy")
                };
                html = html.ReplacePatternCaseInsensitive(replacements);
                string decodedHtml = HtmlEntity.DeEntitize(html);

                HtmlDocument ReqdDoc = new HtmlDocument();
                ReqdDoc.LoadHtml(decodedHtml);


                HtmlNode CommonFooterNode = ReqdDoc.DocumentNode.SelectSingleNode("//div[@class='pdf-footer']");
                if (CommonFooterNode != null)
                {
                    globalElement.CommonFooter = CommonFooterNode.InnerText;
                    CommonFooterNode.ParentNode.RemoveChild(CommonFooterNode);
                }
                var domain = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host;

                var images = ReqdDoc.DocumentNode.SelectNodes("//img/@src")?.ToList();
                if (images != null && images.Any())
                {
                    foreach (HtmlNode img in images)
                    {
                        if (!img.Attributes["src"].Value.StartsWith("http") && !img.Attributes["src"].Value.StartsWith("https") && !img.Attributes["src"].Value.StartsWith("www"))
                        {
                            img.SetAttributeValue("src", domain + img.Attributes["src"].Value);
                        }
                    }
                }
                var links = ReqdDoc.DocumentNode.SelectNodes("//a/@href")?.ToList();
                if (links != null && links.Any())
                {
                    foreach (var link in links)
                    {
                        if (!link.Attributes["href"].Value.StartsWith("http") && !link.Attributes["href"].Value.StartsWith("https") && !link.Attributes["href"].Value.StartsWith("www"))
                        {
                            link.SetAttributeValue("href", domain + link.Attributes["href"].Value);
                            link.SetAttributeValue("target", "_blank");

                        }
                        if (!link.Attributes.Contains(@"style"))
                        {
                            link.SetAttributeValue("style", "color:#be1e2d; text-decoration:none");
                        }
                    }
                }

                var articleNodes = ReqdDoc.DocumentNode.SelectNodes("//div[@class='article-body-content']")?.ToList();
                if (articleNodes != null && articleNodes.Any())
                {
                    foreach (var articleNode in articleNodes)
                    {
                        var articleTitleNode = articleNode.SelectSingleNode(".//a[@class='article-title']");
                        var articleURL = articleTitleNode?.Attributes["href"].Value;

                        //AMcharts in Article
                        var amchartNodes = articleNode.SelectNodes(".//div[@class='amchart-section-pdf']")?.ToList();
                        if (amchartNodes != null && amchartNodes.Any())
                        {
                            foreach (var amchartNode in amchartNodes)
                            {
                                var newNodeHtml = "<p style=\"margin: 0 0 16px 0; color:#58595b; font-size:18px; line-height:30px;\">" + DataToolLinkDesc + "</p><br /><br />" + "<a style=\"color:#be1e2d; text-decoration:none; font-size:18px; line-height:30px;\" href=\"" + articleURL + "\">" + DataToolLinkText + "</a>";
                                HtmlNode newNode = HtmlNode.CreateNode("div");
                                newNode.InnerHtml = newNodeHtml;
                                amchartNode.ParentNode.ReplaceChild(newNode, amchartNode);
                            }
                        }

                        //Tableau in Article
                        var tableauNodes = articleNode.SelectNodes(".//div[@class='article-data-tool-placeholder']")?.ToList();
                        if (tableauNodes != null && tableauNodes.Any())
                        {
                            foreach (var tableauNode in tableauNodes)
                            {
                                var newNodeHtml = "<p style=\"margin: 0 0 16px 0; color:#58595b; font-size:18px; line-height:30px;\">" + DataToolLinkDesc + "</p><br /><br />" + "<a style=\"color:#be1e2d; text-decoration:none; font-size:18px; line-height:30px;\" href=\"" + articleURL + "\">" + DataToolLinkText + "</a>";
                                HtmlNode newNode = HtmlNode.CreateNode("div");
                                newNode.InnerHtml = newNodeHtml;
                                tableauNode.ParentNode.ReplaceChild(newNode, tableauNode);
                            }
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
        /// <summary>
        /// Converting Chart to Flat Image
        /// </summary>
        /// <param name="chartNode"></param>
        /// <returns></returns>
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

    public class GlobalElements : PdfPageEventHelper
    {

        // This is the contentbyte object of the writer
        PdfContentByte cb;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;

        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;
        public string CommonHeader { get; set; }
        public string CommonFooter { get; set; }

        /// <summary>
        /// Overriding PDF OnOpenDocument for reading PDF writer content on document open 
        /// </summary>
        /// <param name="writer">PDF writer</param>
        /// <param name="document">PDF document</param>
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
            }
            catch (DocumentException de)
            {

            }
            catch (IOException ioe)
            {

            }
        }
        /// <summary>
        /// Adding Common Header and Footer
        /// </summary>
        /// <param name="writer">PDF writer</param>
        /// <param name="document">PDF document</param>
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
                cb.SetColorFill(BaseColor.DARK_GRAY);
                cb.SetTextMatrix(document.PageSize.GetRight(40), document.PageSize.GetBottom(30));
                cb.ShowText(PageNumber);
                cb.EndText();
            }

            //Adding common header from page number-2
            if (writer.PageNumber > 1)
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 12);
                cb.SetColorFill(BaseColor.DARK_GRAY);
                cb.SetTextMatrix(document.PageSize.GetLeft(40), document.PageSize.GetTop(30));
                cb.ShowText(!string.IsNullOrEmpty(CommonHeader) ? CommonHeader : string.Empty);
                cb.EndText();
            }
            //Adding common footer
            cb.BeginText();
            cb.SetFontAndSize(bf, 12);
            cb.SetColorFill(BaseColor.DARK_GRAY);
            cb.SetTextMatrix(document.PageSize.GetLeft(40), document.PageSize.GetBottom(30));
            cb.ShowText(!string.IsNullOrEmpty(CommonFooter) ? CommonFooter : string.Empty);
            cb.EndText();

            //Move the pointer and draw line to separate header section from rest of page
            if (writer.PageNumber == 1)
            {
                cb.MoveTo(40, document.PageSize.Height - 100);
                cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 100);
                cb.Stroke();
                cb.SetColorStroke(BaseColor.DARK_GRAY);
            }
            else
            {
                cb.MoveTo(40, document.PageSize.Height - 50);
                cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 50);
                cb.Stroke();
                cb.SetColorStroke(BaseColor.DARK_GRAY);
            }

            //Move the pointer and draw line to separate footer section from rest of page
            cb.MoveTo(40, document.PageSize.GetBottom(50));
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
            cb.Stroke();
            cb.SetColorStroke(BaseColor.DARK_GRAY);
        }
    }
}
