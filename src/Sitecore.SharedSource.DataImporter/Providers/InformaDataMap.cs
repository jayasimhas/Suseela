using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using HtmlAgilityPack;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SharedSource.DataImporter.Extensions;
using Sitecore.SharedSource.DataImporter.Utility;
using HtmlDocument = Sitecore.WordOCX.HtmlDocument.HtmlDocument;
using Sitecore.SharedSource.DataImporter.Logger;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Resources.Media;
using System.Net;
using Sitecore.SecurityModel;
using System.Xml.Linq;
using System.Web;

namespace Sitecore.SharedSource.DataImporter.Providers
{
    public class EscenicAutonomyArticleDataMap : BaseDataMap
    {
        #region Properties

        public string PublicationPrefix { get; set; }

        protected StringBuilder log;

        #endregion Properties

        #region Constructor

        public EscenicAutonomyArticleDataMap(Database db, string ConnectionString, Item importItem, ILogger l)
                : base(db, ConnectionString, importItem, l)
        {
            PublicationPrefix = ImportItem.GetItemField("Publication Prefix", Logger);
        }

        #endregion Constructor

        #region Override Methods


        public override IEnumerable<object> GetImportData(string site, string publication)
        {
            int importErrorCount = 0;
            if (!Directory.Exists(this.Query))
            {
                Logger.Log("N/A", string.Format("the folder '{0}' could not be found. Try moving the folder under the webroot.", this.Query), ProcessStatus.ImportDefinitionError);
                return Enumerable.Empty<object>();
            }

            //XMLDataLogger.WriteLog("_______________________________________________________________________________________","");

            List<Dictionary<string, string>> l = new List<Dictionary<string, string>>();

            long artNumber = GetNextArticleNumber();

            string[] files = Directory.GetFiles(this.Query);
            foreach (string f in files)
            {
                string articleNumber = string.Empty;
                try {

                    string errorLog = "XML read with Error ArticleId: ";
                   
                    string successLog = null;
                    string successwithmissingLog = null;
                    Dictionary<string, string> ao = new Dictionary<string, string>();
                    XmlDocument d = GetXmlDocument(f);
                    if (d == null)
                        continue;

                    //reading article no
                    string curFileName = new FileInfo(f).Name;
                    ao["ARTICLE NUMBER"] = $"{PublicationPrefix}{artNumber:D6}";

                    //reading article author name
                    string authorNode = "STORYAUTHORNAME";
                    ao.Add(authorNode, AuthorHelper.Authors(GetXMLData(d, authorNode)));

                    string bodyNode = "BODY";
                    string bodyTitleHtml = GetXMLData(d, bodyNode);

                    string titleNode = "STORYTITLE";
                    string cleanTitleHtml = CleanTitleHtml(GetXMLData(d, titleNode));

                    //reading Legacy Publications from Web.config
                    //string taxonomyTitleHtml = WebConfigurationManager.AppSettings["LegacyPublications"];
                  //  ao.Add("PUBLICATIONNAME", taxonomyTitleHtml);

                    //reading summary and replace with body it its empty
                    string summaryTitleHtml = GetXMLData(d, "SUMMARY");
                    if (string.IsNullOrEmpty(summaryTitleHtml))
                    {
                        summaryTitleHtml = bodyTitleHtml;
                    }


                    //reading LEADIMAGE and adding on top of body
                    string leadImageTitleHtml = GetXMLData(d, "LEADIMAGE");
                    if (!string.IsNullOrEmpty(leadImageTitleHtml))
                    {
                        //bodyTitleHtml = leadImageTitleHtml + bodyTitleHtml  ;
                        ao.Add("LEADIMAGE", leadImageTitleHtml);
                    }
                    else
                    {
                        ao.Add("LEADIMAGE", "");
                    }

                    XmlNodeList elemList = d.GetElementsByTagName("IMAGE");

                    // string bodyHtmlupdated = ReplaceRelationwithImage(bodyTitleHtml, elemList);
                    //reading VIDEO ,getting video type and adding on end of body
                    string videoTitleHtml = GetXMLData(d, "VIDEO");
                    if ((!string.IsNullOrEmpty(videoTitleHtml)))
                    {
                        string ext = Path.GetExtension(videoTitleHtml);
                        List<string> ImageExtensions = new List<string>() { ".PNG", ".JPG", ".JPEG", ".BMP", ".GIF", };

                        if (ImageExtensions.Contains(ext))
                        {
                            videoTitleHtml = "<img src = '" + videoTitleHtml + "' >";
                        }
                        else
                        {
                            videoTitleHtml = "<video controls >< source src = '" + videoTitleHtml + "'  >Your browser does not support the video tag. </ video >";
                        }
                    }

                    if (!string.IsNullOrEmpty(videoTitleHtml))
                    {
                        string wordToFind = Regex.Match(bodyTitleHtml, @"<VIDEOREL\s*(.+?)\s*</VIDEOREL>").ToString();
                        if (!string.IsNullOrEmpty(wordToFind))
                        {
                            bodyTitleHtml = Regex.Replace(bodyTitleHtml, wordToFind, videoTitleHtml, RegexOptions.IgnoreCase);
                        }
                        else
                        {
                            bodyTitleHtml = bodyTitleHtml + videoTitleHtml;
                        }
                    }



                    //reading images and adding at the end of body
                    string imageTitleHtml = GetXMLData(d, "IMAGE");
                    if (!string.IsNullOrEmpty(imageTitleHtml))
                    {
                        ao.Add("BODYIMAGE", "Y");
                        string wordToFind = Regex.Match(bodyTitleHtml, @"<PICTUREREL\s*(.+?)\s*</PICTUREREL>").ToString();
                        if (!string.IsNullOrEmpty(wordToFind))
                        {
                            bodyTitleHtml = Regex.Replace(bodyTitleHtml, wordToFind, imageTitleHtml, RegexOptions.IgnoreCase);
                        }
                        else
                        {
                            bodyTitleHtml = bodyTitleHtml + imageTitleHtml;
                        }


                    }

                    else
                    {
                        ao.Add("BODYIMAGE", "N");
                    }

                    ao.Add("SUMMARY", summaryTitleHtml);
                    ao.Add("STORYTITLE", cleanTitleHtml);
                    ao.Add("FILENAME", cleanTitleHtml);
                    ao.Add("META TITLE OVERRIDE", cleanTitleHtml);

                    // siddharth
                    errorLog += curFileName.Substring(0, curFileName.Length - 4);
                    articleNumber = curFileName.Substring(0, curFileName.Length - 4);

                    if (GetXMLData(d, "ID") != "")
                    {
                        ao.Add("ARTICLEID", GetXMLData(d, "ID"));
                    }

                    else
                    {
                        importErrorCount = 1;
                        errorLog += "||" + "ArticleId is missing";
                        XMLDataLogger.WriteLog(articleNumber, "ArticleIdMissingLog");
                    }
                    if (GetXMLData(d, "PUBLISHDATE") == "")
                    {
                        importErrorCount = 1;
                        errorLog += "||" + "PublishDate is missing";
                        XMLDataLogger.WriteLog(articleNumber, "PublishDateMissingLog");
                    }


                    if (GetXMLData(d, "PUBLISHDATE") != "" && (DateTime.Parse(GetXMLData(d, "PUBLISHDATE")).AddYears(Int32.Parse(WebConfigurationManager.AppSettings["PublishedYear"])) <= DateTime.Now))
                    {
                        importErrorCount = 2;
                        errorLog += "||" + "Publish Date is more than 2 years old";
                        XMLDataLogger.WriteLog(articleNumber, "OldPublishDateLog");
                    }
                    //reading for ContentType

                    Dictionary<string, string> TaxonomyList = new Dictionary<string, string>();
                    TaxonomyList = GetXMLDataTaxonomyList(d, "TAXONOMY");

                    //reading taxonomy for comodities  adding to ao
                    Dictionary<string, string> Taxonomy = new Dictionary<string, string>();
                    Taxonomy = GetXMLDataTaxonomy(d, "TAXONOMY", site, publication);
                    if (Taxonomy.Count > 0)
                    {
                        // GetRegion().FirstOrDefault(w => w == word);
                        if (publication == "commodities")
                        {
                            if (Taxonomy.Values.Any(k => k.Contains("dairy")))
                            {
                                string taxonomyTitleHtml = WebConfigurationManager.AppSettings["LegacyPublications_dairy"];
                                ao.Add("PUBLICATIONNAME", taxonomyTitleHtml);
                            }
                            else if (Taxonomy.Values.Any(k => k.Contains("public")))
                            {
                                string taxonomyTitleHtml = WebConfigurationManager.AppSettings["LegacyPublications_public_ledger"];
                                ao.Add("PUBLICATIONNAME", taxonomyTitleHtml);
                            }

                            else if (Taxonomy.Values.Any(k => k.Contains("foodnews")))
                            {
                                string taxonomyTitleHtml = WebConfigurationManager.AppSettings["LegacyPublications_foodnews"];
                                ao.Add("PUBLICATIONNAME", taxonomyTitleHtml);
                            }
                            // ao.Add("PUBLICATIONNAME", taxonomyTitleHtml);

                        }

                        foreach (KeyValuePair<string, string> pair in Taxonomy)
                        {
                            ao.Add(pair.Key, pair.Value);
                        }

                        // Siddharth
                        //reading ContentType according to agr mapping  adding to ao
                        if (TaxonomyList.Values.Count != 0)
                        {
                            string contentTypeHtml = string.Empty;
                            string contentTypeSetHtml = string.Empty;
                            foreach (KeyValuePair<string, string> pair in TaxonomyList)
                            {
                                // contentTypeHtml = GetContentType(pair.Value.ToString());
                                contentTypeHtml = GetusingXML(pair.Value.ToString(), publication, "contenttype", site);
                                if (contentTypeHtml != " " && contentTypeHtml == "news")
                                {
                                    contentTypeSetHtml = contentTypeHtml;
                                    //ao.Add("SECTION", contentTypeHtml);
                                    // break;
                                }
                                else if (contentTypeHtml == "analysis" || contentTypeHtml == "opinion" || contentTypeHtml == "interviews")
                                {
                                    contentTypeSetHtml = contentTypeHtml;
                                    // ao.Add("SECTION", contentTypeHtml);
                                    break;
                                }
                            }


                            if (contentTypeSetHtml != string.Empty)
                            {

                                ao.Add("SECTION", contentTypeSetHtml);
                            }
                            else
                            {
                                ao.Add("SECTION", "news");
                            }


                        }
                        else
                        {
                            ao.Add("SECTION", "news");
                            successwithmissingLog += "||" + "ContentType is missing";
                            XMLDataLogger.WriteLog(articleNumber, "ContentTypeMissingLog");
                        }

                        //readingTableau and adding to ao
                        Dictionary<string, string> Tableau = new Dictionary<string, string>();
                        Tableau = GetXMLDataTaxonomy(d, "TABLEAU", site, publication);
                        if (Tableau.Count > 0)
                        {
                            foreach (KeyValuePair<string, string> pair in Tableau)
                            {
                                ao.Add(pair.Key, pair.Value);
                            }
                        }

                        // Siddharth
                        if (importErrorCount != 2)
                        {
                            if (GetXMLData(d, bodyNode) == "")
                            {
                                importErrorCount = 1;
                                errorLog += "||" + "Body is missing ";
                                XMLDataLogger.WriteLog(articleNumber, "BodyMissingLog");

                            }

                            //reading mediaType according to agr mapping  adding to ao

                            if (!ao.ContainsKey("MEDIA"))
                            {
                                if (Tableau.Count > 0)
                                {
                                    ao.Add("MEDIA", "interactivedashboards");
                                }
                                else if (CheckTableau(GetXMLData(d, bodyNode)))
                                {
                                    ao.Add("MEDIA", "interactivedashboards");
                                }
                                else if (!string.IsNullOrEmpty(imageTitleHtml))
                                {
                                    ao.Add("MEDIA", "Chart/Table");
                                }
                                else if (CheckTable(GetXMLData(d, bodyNode)))
                                {
                                    ao.Add("MEDIA", "Chart/Table");
                                }
                                else if (CheckIframe(GetXMLData(d, bodyNode)))
                                {
                                    ao.Add("MEDIA", "video");
                                }
                                
                                //else if (!string.IsNullOrEmpty(bodyTitleHtml))
                                //{
                                //    ao.Add("MEDIA", "image");
                                //}
                                else
                                {
                                    ao.Add("MEDIA", "");
                                }
                            }




                            ao.Add("INDUSTRIES", "");


                            // Siddharth
                            if (ao["MEDIA"] == "")
                            {

                                successwithmissingLog += "||" + "Media is missing";
                                XMLDataLogger.WriteLog(articleNumber, "MediaMissingLog");
                            }

                            if (!(ao.ContainsKey("COMMODITY")))
                            {

                                successwithmissingLog += "||" + "COMMODITY is missing";
                                // XMLDataLogger.WriteLog(articleNumber, "COMMODITYMissingLog");
                            }
                        }
                        if (importErrorCount == 1)
                        {
                            XMLDataLogger.WriteLog(errorLog, "");
                            // XMLDataLogger.WriteLog(articleNumber, "BodyMissingLog");
                        }
                        if (importErrorCount == 2)
                        {
                            XMLDataLogger.WriteLog(errorLog, "");
                            //  XMLDataLogger.WriteLog(articleNumber, "OldPublishDateLog");
                        }
                        else
                        {
                            successLog = "XML read successfully for" + " ArticleId: " + ao["ARTICLEID"] + successwithmissingLog;
                            XMLDataLogger.WriteLog(successLog, "");
                            XMLDataLogger.WriteLog(successLog, "Success");
                        }


                        ao.Add("STORYBODY", bodyTitleHtml);

                        string summarySearch = " ";
                        if (GetXMLData(d, "SUMMARY") != null)
                        {
                            summarySearch = GetXMLData(d, "SUMMARY");
                        }

                        if (!(bodyTitleHtml.Length == 0 && cleanTitleHtml.Length == 0))

                        {

                            string BodyTextremovehtml = FindingTextFromHTML(bodyTitleHtml);
                            string BodyText = RemovespecialcharactersfromString(BodyTextremovehtml);
                            string cleanTitle = RemovespecialcharactersfromString(cleanTitleHtml);
                            string summSearch = RemovespecialcharactersfromString(summarySearch);
                            string AgencyCompanyTextSearch = " " + cleanTitle + " " + BodyText + " " + summSearch;
                            string RegionTextSearch = " " + cleanTitle + " " + BodyText.Substring(0, Math.Min(BodyText.Length, 200)) + " " + summSearch;
                            string specialcommoditysearch = " " + cleanTitle + " " + summarySearch;
                            string Country = "";
                            string Companies = "";
                            string Agency = "";
                            string Commodity = "";
                            string CommodityFactor = "";
                            string AnimalHealth = "";


                            if (site != "Maritime")
                            {
                                string textForagency = AgencyCompanyTextSearch;
                                textForagency = textForagency.Replace("(", "");
                                textForagency = textForagency.Replace(")", "");
                                textForagency = textForagency.Replace("’", "'");

                                List<string> agencySearchResults = GetListFromXml(publication, "agency", site).FindAll(s => textForagency.ToLower().Contains(" " + s + " "));
                                // List<string> agencySearchResults = GetListFromXml(publication, "agency", site).FindAll(s => AgencyCompanyTextSearch.ToLower().Contains(" " + s + " "));

                                List<string> commoditySearchResults = null;
                                List<string> commodityfactorSearchResults = null;
                                List<string> animalhealthSearchResults = null;



                                if (publication == "AnimalPharma")
                                {
                                    animalhealthSearchResults = GetListFromXml(publication, "animalhealth", site).FindAll(s => AgencyCompanyTextSearch.ToLower().Contains(" " + s + " "));

                                }
                                if (publication == "Agrow")
                                {

                                    commoditySearchResults = GetListFromXml(publication, "commodity", site).FindAll(s => AgencyCompanyTextSearch.ToLower().Contains(" " + s + " "));

                                }

                                if (publication == "commodities")
                                {
                                    commoditySearchResults = GetListFromXml(publication, "commoditysearch", site).FindAll(s => specialcommoditysearch.ToLower().Contains(" " + s + " "));
                                }

                                if (publication == "commodities")
                                {

                                    commodityfactorSearchResults = GetListFromXml(publication, "commodityfactor", site).FindAll(s => AgencyCompanyTextSearch.ToLower().Contains(" " + s + " "));
                                }
                                foreach (string agency in agencySearchResults)
                                {
                                    if (publication == "commodities")
                                    {
                                        if (!((agency.ToLower() == "imf") && ao["PUBLICATIONNAME"].ToString() == "Dairy Markets") && !((agency.ToLower() == "international monetary fund") && ao["PUBLICATIONNAME"].ToString() == "Dairy Markets"))
                                            Agency += agency + ",";
                                    }
                                    else
                                    {
                                        Agency += agency + ",";
                                    }

                                }
                                if (commoditySearchResults != null)
                                {
                                    foreach (string commodity in commoditySearchResults)
                                    {
                                        if (!(commodity.ToLower() == "palm" && commoditySearchResults.Contains("palm beach")))
                                            Commodity += commodity + ",";

                                    }
                                }
                                if (commodityfactorSearchResults != null)
                                {
                                    foreach (string commodityfactor in commodityfactorSearchResults)
                                    {
                                        if (!(commodityfactor.ToLower() == "energy" && commodityfactorSearchResults.Contains("energy drinks")) || !(commodityfactor.ToLower() == "energy drinks" && commodityfactorSearchResults.Contains("energy drinks")))
                                            CommodityFactor += commodityfactor + ",";

                                    }
                                }
                                if (animalhealthSearchResults != null)
                                {
                                    foreach (string animalhealth in animalhealthSearchResults)
                                    {
                                        AnimalHealth += animalhealth + ",";

                                    }
                                }
                            }

                            // List<string> companySearchResults = GetListFromXml(publication, "companies", site).FindAll(s => AgencyCompanyTextSearch.ToLower().Contains(" " + s + " "));

                            string textForCompany = AgencyCompanyTextSearch;
                            textForCompany = textForCompany.Replace("’", "'");
                            textForCompany = textForCompany.Replace("(", "'");
                            textForCompany = textForCompany.Replace(")", "");
                            List<string> companySearchResults = GetListFromXml(publication, "companies", site).FindAll(s => textForCompany.ToLower().Contains(" " + s + " "));
                            //List<string> companySearchResults = GetListFromXml(publication, "companies", site).FindAll(s => AgencyCompanyTextSearch.ToLower().Contains(" " + s + " "));



                            foreach (string company in companySearchResults)
                            {
                                if (!(publication == "AnimalPharma" && company == "bayer cropscience") && !(publication == "Agrow" && company == "bayer animal health") && !(publication == "commodities" && company == "bayer animal health"))
                                    Companies += company + ",";

                            }

                            string textSearchForRegion =RegionTextSearch.Replace("’", "'");
                            List<string> regionSearchResults = GetListFromXml(publication, "country", site).FindAll(s => textSearchForRegion.ToLower().Contains(" " + s + " "));
                            foreach (string region in regionSearchResults)
                            {
                                Country += region + ",";

                            }


                            if (ao.ContainsKey("COUNTRY"))
                            {
                                if (Country != "")
                                {

                                    Country = ao["COUNTRY"].ToString() + Country;
                                    ao.Remove("COUNTRY");
                                    ao.Add("COUNTRY", Country);

                                }
                            }
                            else
                            {

                                ao.Add("COUNTRY", Country);
                            }





                            if (Companies == "")
                            {
                                ao.Add("COMPANIES", "");
                            }
                            else
                            {
                                ao.Add("COMPANIES", Companies);

                            }
                            if (Agency == "")
                            {
                                ao.Add("AGENCY", "");
                            }
                            else
                            {

                                ao.Add("AGENCY", Agency);
                            }
                            if (ao.ContainsKey("COMMODITY"))
                            {
                                if (Commodity != "")

                                {

                                    Commodity = ao["COMMODITY"].ToString() + Commodity;
                                    ao.Remove("COMMODITY");
                                    ao.Add("COMMODITY", Commodity);

                                }
                            }
                            else
                            {

                                ao.Add("COMMODITY", Commodity);
                            }

                            if (ao.ContainsKey("COMMODITYFACTOR"))
                            {
                                if (CommodityFactor != "")

                                {

                                    CommodityFactor = ao["COMMODITYFACTOR"].ToString() + CommodityFactor;
                                    ao.Remove("COMMODITYFACTOR");
                                    ao.Add("COMMODITYFACTOR", CommodityFactor);

                                }
                            }
                            else
                            {

                                ao.Add("COMMODITYFACTOR", CommodityFactor);
                            }

                            if (ao.ContainsKey("ANIMALHEALTH"))
                            {
                                if (AnimalHealth != "")

                                {

                                    AnimalHealth = ao["ANIMALHEALTH"].ToString() + AnimalHealth;
                                    ao.Remove("ANIMALHEALTH");
                                    ao.Add("ANIMALHEALTH", AnimalHealth);

                                }
                            }
                            else
                            {

                                ao.Add("ANIMALHEALTH", AnimalHealth);
                            }


                        }
                        else
                        {

                            if (!ao.ContainsKey("COUNTRY"))
                            {
                                ao.Add("COUNTRY", "");
                            }
                            ao.Add("COMPANIES", "");
                            ao.Add("AGENCY", "");
                            ao.Add("Commodity", "");
                        }

                        if (importErrorCount != 2)
                        {
                            l.Add(ao);
                        }
                        artNumber++;


                        //autonomy fields
                        string autFile = $@"{this.Query}\..\Autonomy\{curFileName}";
                        List<string> autNodes = new List<string>() { "CATEGORY", "COMPANY", "STORYUPDATE", "KEYWORD" };


                        importErrorCount = 0;
                        if (!File.Exists(autFile))
                        {
                            Logger.Log("N/A", "File not found", ProcessStatus.NotFoundError, "File", autFile);
                            foreach (string n in autNodes)
                                ao.Add(n, string.Empty);

                            //default back to the date from escenic
                            string dateVal = GetXMLData(d, "PUBLISHDATE");

                            DateTime date;
                            if (!DateTimeUtil.ParseInformaDate(dateVal, out date))
                                Logger.Log("N/A", "No Date to parse error", ProcessStatus.DateParseError, "Missing Autonomy File Name", autFile);
                            else
                                ao["STORYUPDATE"] = dateVal;

                            continue;
                        }

                        XmlDocument d2 = GetXmlDocument(autFile);
                        if (d2 == null)
                            continue;

                        foreach (string n in autNodes)
                            ao.Add(n, GetXMLData(d2, n));
                    }
                }
                catch(Exception ex)
                {

                    XMLDataLogger.WriteLog(articleNumber, "ErrorReading");
                }

            }

           
            //XMLDataLogger.WriteLog("");
            return l;

        }


        private bool CheckTable(string searchtable)
        {

            Regex regex = new Regex("<table (.*)</table>");
            var v = regex.Match(searchtable);


            if ((v != null) && (v.Length>0))
            {
                return true;

            }
            else
                return false;

        }

        private bool CheckTableau(string searchtableau)
        {

            string wordToFind = Regex.Match(searchtableau, @"<object \s*(.+?)\s*</object>").ToString();

            if (!string.IsNullOrEmpty(wordToFind))
            {
                return true;

            }
            else
                return false;
        }
        private bool CheckIframe(string searchIframe)
        {

            Regex regex = new Regex("<iframe frameborder=(.*)</iframe>");
            var v = regex.Match(searchIframe);

            if ((v != null) && v.Length > 0)
            {
               return true;

            }
            else { return false; }
        }


        public override IEnumerable<object> ImportImages(IDataMap map)
        {
            // int importErrorCount = 0;
            if (!Directory.Exists(this.Query))
            {
                Logger.Log("N/A", string.Format("the folder '{0}' could not be found. Try moving the folder under the webroot.", this.Query), ProcessStatus.ImportDefinitionError);
                return Enumerable.Empty<object>();
            }

            //XMLDataLogger.WriteLog("_______________________________________________________________________________________","");

            //List<Dictionary<string, string>> l = new List<Dictionary<string, string>>();
            List<string> totalimglist = new List<string>();

            string[] files = Directory.GetFiles(this.Query);
            string images = "";
            string leadimgs = "";
            foreach (string f in files)
            {
                try
                {
                    Dictionary<string, string> ao = new Dictionary<string, string>();
                    XmlDocument d = GetXmlDocument(f);
                    if (d == null)
                        continue;
                    string Articleid = GetXMLData(d, "ID");
                    string dateVal = GetXMLData(d, "PUBLISHDATE");
                    string fileimages = "";
                    string fileleadimage = "";
                    fileimages = GetImgData(d, "IMAGE", dateVal, Articleid);
                    fileleadimage = GetImgData(d, "LEADIMAGE", dateVal, Articleid);
                    if (fileimages != "")
                    {
                        images += fileimages + ",";
                    }

                    if (fileleadimage != "")
                    {
                        leadimgs += fileleadimage + ",";
                    }
                }

                catch (Exception ex)
                {

                }

            }
            totalimglist = (images + leadimgs).Split(',').ToList<string>();

            return DownloadImages(totalimglist, map);

            // return totalimglist;
            //return Enumerable.Empty<object>();
        }

        public static string FindingTextFromHTML(string RTEInput)
        {
            //  bool _isListAvailable = false;
            String result = string.Empty;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            doc.LoadHtml(RTEInput);
            var nodes = doc.DocumentNode.DescendantsAndSelf().ToList();

            foreach (HtmlAgilityPack.HtmlNode node in nodes)
            {
                if ((node.Name.Equals("#text", StringComparison.OrdinalIgnoreCase) || node.Name.Equals("ol", StringComparison.OrdinalIgnoreCase)))
                {
                    if (node.InnerText.Length > 2)
                    {
                        // _isListAvailable = true;
                        result += node.InnerText + " ";
                    }
                }
            }


            return result;
        }

        public static string RemovespecialcharactersfromString(string RTEInput)
        {

            var charsToRemove = new string[] { "\n", ">", ".", ";", ",", "<", "/",":" };
            foreach (var cha in charsToRemove)
            {
                if (cha == "\n")
                {
                    RTEInput = RTEInput.Replace(cha, " ");
                }
                else
                {
                    RTEInput = RTEInput.Replace(cha, string.Empty);
                }

            }

            return RTEInput;
        }



        //public static string ReplaceRelationwithImage(string RTEInput, XmlNodeList imglist)
        //    {
        //        //  bool _isListAvailable = false;
        //        String result = string.Empty;
        //        var document = new HtmlDocument();
        //        document.LoadHtml(RTEInput);
        //        var tryGetNodes = document.DocumentNode.DescendantNodesAndSelf().ToList();


        //        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
        //        doc.LoadHtml(RTEInput);

        //        var nodes = doc.DocumentNode.DescendantsAndSelf().ToList();
        //        var testnodes = new Queue<HtmlNode>(tryGetNodes);
        //      //  var testnodes = document.

        //        foreach (HtmlNode node in tryGetNodes)
        //        {
        //            if ((node.Name.Equals("relation", StringComparison.OrdinalIgnoreCase)))
        //            {
        //                foreach(XmlNode img in imglist)
        //                {
        //                    // if(node.Attributes["sourceid"].Value == img.)
        //                    if (node.Attributes["sourceid"].Value == img.Attributes["sourceid"].Value)

        //                    {
        //                        nodes.Remove(node);
        //                        //HtmlNode imgnode = doc.CreateElement("Image");
        //                        HtmlAgilityPack.HtmlNode imgnew = doc.CreateElement("Image");
        //                        imgnew.Attributes["sourceid"].Value = img.Attributes["sourceid"].Value;
        //                       // HtmlAgilityPack.HtmlControls.Image imgnode = new Web.UI.HtmlControls.Image();
        //                        //imgnode.Attributes["sourceid"] = img.Attributes["sourceid"].Value;

        //                        nodes.Add(imgnew);

        //                    }

        //                }

        //            }
        //            else
        //            {
        //                result += node.InnerHtml;
        //            }
        //        }

        //        return result.Trim();
        //    }

        public string CleanTitleHtml(string html)
        {
            List<string> unwantedTags = new List<string>() { "a", "b", "body", "blockquote", "br", "button", "center", "td", "tr", "em", "i",
                                "embed", "form", "frame", "iframe", "h1", "h2", "h3", "h4", "h5", "h6", "hr", "img", "legend", "li", "ul", "ol", "map",
                                "script", "strong", "sup", "sub", "p", "thead", "tbody", "u", "span", "table", "div", "label", "font" };

            if (String.IsNullOrEmpty(html))
                return html;

            var document = new HtmlDocument();
            document.LoadHtml(html);

            HtmlNodeCollection tryGetNodes = document.DocumentNode.SelectNodes("./*|./text()");

            if (tryGetNodes == null || !tryGetNodes.Any())
                return html;

            var nodes = new Queue<HtmlNode>(tryGetNodes);

            int i = 0;
            while (nodes.Count > 0)
            {
                var node = nodes.Dequeue();
                var nodeName = node.Name.ToLower();
                var parentNode = node.ParentNode;
                var childNodes = node.SelectNodes("./*|./text()");

                if (childNodes != null)
                {
                    foreach (var child in childNodes)
                        nodes.Enqueue(child);
                }

                if (unwantedTags.Any(tag => tag == nodeName))
                { // if this node is one to remove
                    if (childNodes != null)
                    { // make sure children are added back
                        foreach (var child in childNodes)
                            parentNode.InsertBefore(child, node);
                    }

                    parentNode.RemoveChild(node);
                }
            }

            return document.DocumentNode.InnerHtml;
        }

        protected virtual int GetNextArticleNumber()
        {
            IEnumerable<string> articles = ImportToWhere.Axes.GetDescendants()
                   .Where(a => a.TemplateName.Equals(ImportToWhatTemplate.DisplayName))
                   .Select(b => b.Fields["Article Number"].Value)
                   .OrderByDescending(c => c);

            var articles3 = ImportToWhere.Axes.GetDescendants();

            var articles1 = ImportToWhere.Axes.GetDescendants().Where(a => a.TemplateName.Equals(ImportToWhatTemplate.DisplayName));

            var articles2 = ImportToWhere.Axes.GetDescendants().Where(a => a.TemplateName.Equals(ImportToWhatTemplate.DisplayName))
                             .Select(b => b.Fields["Article Number"].Value);


            if (articles == null || !articles.Any())
                return 1;

            try
            {

                string num = articles.First().Replace(PublicationPrefix, "");
                int n = int.Parse(num);
                return n + 1;
            }
            catch
            {
                return 1;
            }
        }
        //Siddharth
        public string GetusingXML(string contentName, string publication, string type, string site)
        {



            //  XElement doc = XElement.Load((WebConfigurationManager.AppSettings["xmlContentImport"]));
            XElement doc = XElement.Load(string.Format(@"{0}sitecore modules\Shell\Data Import\CMConfig\ContentMigrationMappingConfigs.xml", HttpRuntime.AppDomainAppPath));

            if (doc.Descendants(site).Descendants(publication).Descendants(type).Descendants().Any(x => x.Attribute("name").Value.ToLower() == contentName.ToLower()))
            {
                var elemValue = from c in doc.Descendants(site).Descendants(publication).Descendants(type).Descendants().Where(x => x.Attribute("name").Value == contentName)
                                select c.Value.ToLower();



                if (elemValue.ElementAt(0) != null)
                {
                    return elemValue.ElementAt(0).ToString().ToLower();

                }

                else
                {
                    return "";
                }
            }

            else
            {

                return "";
            }
        }

        public bool CheckifExistsusingXML(string contentName, string publication, string type, string site)
        {


            XElement doc = XElement.Load(string.Format(@"{0}sitecore modules\Shell\Data Import\CMConfig\ContentMigrationMappingConfigs.xml", HttpRuntime.AppDomainAppPath));

            // XElement doc = XElement.Load((WebConfigurationManager.AppSettings["xmlContentImport"]));


            var elemValue = doc.Descendants(site).Descendants(publication).Descendants(type).Descendants().Any(x => x.Attribute("name").Value.ToLower() == contentName.ToLower());

            return elemValue;

        }
        public string GetContentType(string contentName)
        {


            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("dairy_markets", "News");
            d.Add("dairy_markets_subscribe_free_demo", "News");
            d.Add("dairy_markets_features", "Analysis");
            d.Add("dairy_markets_market_focus", "Analysis");
            d.Add("dairy_markets_downloads", "News");
            d.Add("dairy_markets_filedownloads", "News");
            d.Add("dairy_markets_subscribe_free_demo_mobile", "News");
            d.Add("dairy_markets_resources_dairy_ezine_mobile", "News");
            d.Add("dairy_markets_markets", "News");
            d.Add("dairy_markets_markets_butter", "News");
            d.Add("dairy_markets_markets_cheese", "News");
            d.Add("dairy_markets_markets_milk", "News");
            d.Add("dairy_markets_markets_milk_liquid", "News");
            d.Add("dairy_markets_markets_milk_raw", "News");
            d.Add("dairy_markets_markets_powders", "News");
            d.Add("dairy_markets_markets_powders_casein", "News");
            d.Add("dairy_markets_markets_powders_imf", "News");
            d.Add("dairy_markets_markets_powders_lactose", "News");
            d.Add("dairy_markets_markets_powders_smp", "News");
            d.Add("dairy_markets_markets_powders_wmp", "News");
            d.Add("dairy_markets_markets_powders_whey_powder", "News");
            d.Add("dairy_markets_markets_other_dairy", "News");
            d.Add("dairy_markets_markets_other_dairy_fresh_products", "News");
            d.Add("dairy_markets_markets_other_dairy_frozen_products", "News");
            d.Add("dairy_markets_analysis", "Analysis");
            d.Add("dairy_markets_analysis_company", "News");
            d.Add("dairy_markets_analysis_policy", "News");
            d.Add("dairy_markets_analysis_trade", "Analysis");




            d.Add("foodnews_market_focus", "Analysis");
            d.Add("foodnews_features", "Analysis)");
            d.Add("foodnews_interviews", "Interview");
            d.Add("foodnews_opinion", "Opinion");
            d.Add("foodnews_beverages", "News");
            d.Add("foodnews_beverages_fruit_juices", "News");
            d.Add("foodnews_beverages_fruit_juices_passion_fruit", "News");
            d.Add("foodnews_beverages_fruit_juices_cranberry", "News");
            d.Add("foodnews_beverages_fruit_juices_apple", "News");
            d.Add("foodnews_beverages_fruit_juices_blends", "News");
            d.Add("foodnews_beverages_fruit_juices_grape", "News");
            d.Add("foodnews_beverages_fruit_juices_lemon", "News");
            d.Add("foodnews_beverages_fruit_juices_mango", "News");
            d.Add("foodnews_beverages_fruit_juices_nfc", "News");
            d.Add("foodnews_beverages_fruit_juices_oils", "News");
            d.Add("foodnews_beverages_fruit_juices_orange", "News");
            d.Add("foodnews_beverages_fruit_juices_pineapple", "News");
            d.Add("foodnews_beverages_fruit_juices_superfruits", "News");
            d.Add("foodnews_beverages_fruit_juices_tropical", "News");
            d.Add("foodnews_beverages_fruit_juices_vegetable", "News");
            d.Add("foodnews_beverages_alcoholic_drinks", "News");
            d.Add("foodnews_beverages_alcoholic_drinks_wines", "News");
            d.Add("foodnews_beverages_alcoholic_drinks_wines_still", "News");
            d.Add("foodnews_beverages_alcoholic_drinks_wines_sparkling", "News");
            d.Add("foodnews_beverages_alcoholic_drinks_cider", "News");
            d.Add("foodnews_beverages_soft_drinks", "News");
            d.Add("foodnews_beverages_soft_drinks_bottled_water", "News");
            d.Add("foodnews_beverages_soft_drinks_carbonates", "News");
            d.Add("foodnews_beverages_soft_drinks_fruit_drinks", "News");
            d.Add("foodnews_beverages_soft_drinks_smoothies", "News");
            d.Add("foodnews_beverages_soft_drinks_sports_energy_drinks", "News");
            d.Add("foodnews_beverages_puree", "News");
            d.Add("foodnews_beverages_puree_apple", "News");
            d.Add("foodnews_beverages_puree_tropicals", "News");
            d.Add("foodnews_beverages_puree_apricot", "News");
            d.Add("foodnews_beverages_puree_banana", "News");
            d.Add("foodnews_beverages_puree_mango", "News");
            d.Add("foodnews_beverages_puree_peach", "News");
            d.Add("foodnews_beverages_puree_pear", "News");
            d.Add("foodnews_beverages_wines", "News");
            d.Add("foodnews_beverages_wines_sparkling", "News");
            d.Add("foodnews_beverages_wines_still", "News");
            d.Add("foodnews_canned", "News");
            d.Add("foodnews_canned_canned_fruit", "News");
            d.Add("foodnews_canned_canned_fruit_apricots", "News");
            d.Add("foodnews_canned_canned_fruit_fruit_cocktail", "News");
            d.Add("foodnews_canned_canned_fruit_mandarins", "News");
            d.Add("foodnews_canned_canned_fruit_peaches", "News");
            d.Add("foodnews_canned_canned_fruit_pears", "News");
            d.Add("foodnews_canned_canned_fruit_pineapple", "News");
            d.Add("foodnews_canned_canned_vegetables", "News");
            d.Add("foodnews_canned_canned_vegetables_artichokes", "News");
            d.Add("foodnews_canned_canned_vegetables_asparagus", "News");
            d.Add("foodnews_canned_canned_vegetables_beans", "News");
            d.Add("foodnews_canned_canned_vegetables_mushrooms", "News");
            d.Add("foodnews_canned_canned_vegetables_peas", "News");
            d.Add("foodnews_canned_canned_vegetables_sweetcorn", "News");
            d.Add("foodnews_canned_canned_fish", "News");
            d.Add("foodnews_canned_canned_fish_anchovies", "News");
            d.Add("foodnews_canned_canned_fish_mackerel", "News");
            d.Add("foodnews_canned_canned_fish_salmon", "News");
            d.Add("foodnews_canned_canned_fish_sardines", "News");
            d.Add("foodnews_canned_canned_fish_tuna", "News");
            d.Add("foodnews_canned_canned_meat", "News");
            d.Add("foodnews_canned_canned_meat_beef", "News");
            d.Add("foodnews_canned_canned_meat_pork", "News");
            d.Add("foodnews_frozen", "News");
            d.Add("foodnews_frozen_frozen_fruit", "News");
            d.Add("foodnews_frozen_frozen_fruit_blueberries", "News");
            d.Add("foodnews_frozen_frozen_fruit_cranberries", "News");
            d.Add("foodnews_frozen_frozen_fruit_raspberries", "News");
            d.Add("foodnews_frozen_frozen_fruit_strawberries", "News");
            d.Add("foodnews_frozen_frozen_fruit_other_fruit", "News");
            d.Add("foodnews_frozen_frozen_vegetables", "News");
            d.Add("foodnews_frozen_frozen_vegetables_other_vegetables", "News");
            d.Add("foodnews_frozen_frozen_vegetables_asparagus", "News");
            d.Add("foodnews_frozen_frozen_vegetables_beans", "News");
            d.Add("foodnews_frozen_frozen_vegetables_carrots", "News");
            d.Add("foodnews_frozen_frozen_vegetables_cauliflower", "News");
            d.Add("foodnews_frozen_frozen_vegetables_mushrooms", "News");
            d.Add("foodnews_frozen_frozen_vegetables_peas", "News");
            d.Add("foodnews_frozen_frozen_vegetables_potatoes", "News");
            d.Add("foodnews_frozen_frozen_vegetables_sweetcorn", "News");
            d.Add("foodnews_frozen_frozen_foods", "News");
            d.Add("foodnews_frozen_frozen_foods_ice_cream", "News");
            d.Add("foodnews_frozen_frozen_foods_prepared_foods", "News");
            d.Add("foodnews_dfn", "News");
            d.Add("foodnews_dfn_nuts", "News");
            d.Add("foodnews_dfn_nuts_nut_milks", "News");
            d.Add("foodnews_dfn_nuts_almonds", "News");
            d.Add("foodnews_dfn_nuts_brazil_nuts", "News");
            d.Add("foodnews_dfn_nuts_cashews", "News");
            d.Add("foodnews_dfn_nuts_desiccated_coconut", "News");
            d.Add("foodnews_dfn_nuts_hazelnuts", "News");
            d.Add("foodnews_dfn_nuts_macadamias", "News");
            d.Add("foodnews_dfn_nuts_peanuts", "News");
            d.Add("foodnews_dfn_nuts_pecans", "News");
            d.Add("foodnews_dfn_nuts_pistachios", "News");
            d.Add("foodnews_dfn_nuts_walnuts", "News");
            d.Add("foodnews_dfn_nuts_other_nuts", "News");
            d.Add("foodnews_dfn_dried_fruit", "News");
            d.Add("foodnews_dfn_dried_fruit_apple", "News");
            d.Add("foodnews_dfn_dried_fruit_apricots", "News");
            d.Add("foodnews_dfn_dried_fruit_dates", "News");
            d.Add("foodnews_dfn_dried_fruit_figs", "News");
            d.Add("foodnews_dfn_dried_fruit_prunes", "News");
            d.Add("foodnews_dfn_dried_fruit_tropicals", "News");
            d.Add("foodnews_dfn_dried_fruit_vine_fruit", "News");
            d.Add("foodnews_dfn_dried_fruit_other_fruit", "News");
            d.Add("foodnews_dfn_dehydrates", "News");
            d.Add("foodnews_dfn_dehydrates_apple", "News");
            d.Add("foodnews_dfn_dehydrates_banana", "News");
            d.Add("foodnews_dfn_dehydrates_mango", "News");
            d.Add("foodnews_dfn_dehydrates_pineapple", "News");
            d.Add("foodnews_dfn_dehydrates_strawberries", "News");
            d.Add("foodnews_raw_material", "News");
            d.Add("foodnews_raw_material_fresh_fruit", "News");
            d.Add("foodnews_raw_material_fresh_fruit_pineapple", "News");
            d.Add("foodnews_raw_material_fresh_fruit_tomatoes", "News");
            d.Add("foodnews_raw_material_fresh_fruit_berries", "News");
            d.Add("foodnews_raw_material_fresh_fruit_tropicals", "News");
            d.Add("foodnews_raw_material_fresh_fruit_apples", "News");
            d.Add("foodnews_raw_material_fresh_fruit_apricots", "News");
            d.Add("foodnews_raw_material_fresh_fruit_bananas", "News");
            d.Add("foodnews_raw_material_fresh_fruit_grapes", "News");
            d.Add("foodnews_raw_material_fresh_fruit_lemons", "News");
            d.Add("foodnews_raw_material_fresh_fruit_oranges", "News");
            d.Add("foodnews_raw_material_fresh_fruit_peaches", "News");
            d.Add("foodnews_raw_material_fresh_fruit_pears", "News");
            d.Add("foodnews_raw_material_fresh_fruit_plums", "News");
            d.Add("foodnews_raw_material_fresh_vegetables", "News");
            d.Add("foodnews_raw_material_fresh_vegetables_artichokes", "News");
            d.Add("foodnews_raw_material_fresh_vegetables_other_vegetables", "News");
            d.Add("foodnews_raw_material_fresh_vegetables_root_vegetables", "News");
            d.Add("foodnews_raw_material_fresh_vegetables_asparagus", "News");
            d.Add("foodnews_raw_material_fresh_vegetables_beans", "News");
            d.Add("foodnews_raw_material_fresh_vegetables_mushrooms", "News");
            d.Add("foodnews_raw_material_fresh_vegetables_peas", "News");
            d.Add("foodnews_raw_material_fresh_vegetables_sweetcorn", "News");
            d.Add("foodnews_tomato_products", "News");
            d.Add("foodnews_tomato_products_canned", "News");
            d.Add("foodnews_tomato_products_ketchup", "News");
            d.Add("foodnews_tomato_products_paste", "News");
            d.Add("foodnews_analysis", "Analysis");
            d.Add("foodnews_analysis_company", "News");
            d.Add("foodnews_analysis_company_operations", "News");
            d.Add("foodnews_analysis_company_investment", "News");
            d.Add("foodnews_analysis_company_personnel", "News");
            d.Add("foodnews_analysis_company_results", "News");
            d.Add("foodnews_analysis_company_manda", "News");
            d.Add("foodnews_analysis_company_trends", "Analysis");
            d.Add("foodnews_analysis_personnel", "News");
            d.Add("foodnews_analysis_trends", "Analysis");
            d.Add("foodnews_analysis_food_safety", "News");
            d.Add("foodnews_analysis_freight", "News");
            d.Add("foodnews_analysis_weather", "News");



            d.Add("public_ledger_opinion", "Opinion");
            d.Add("public_ledger_world_grain_market_report", "Analysis");
            d.Add("public_ledger_country_profiles", "Analysis");
            d.Add("public_ledger_market_focus", "Analysis");
            d.Add("public_ledger_features", "Analysis");
            d.Add("public_ledger_interviews", "Interviews");
            d.Add("public_ledger_daily_futures_reviews", "News");
            d.Add("public_ledger_daily_futures_reviews_cbot_corn", "News");
            d.Add("public_ledger_daily_futures_reviews_cbot_rice", "News");
            d.Add("public_ledger_daily_futures_reviews_cbot_soy_oil", "News");
            d.Add("public_ledger_daily_futures_reviews_cbot_soybeans", "News");
            d.Add("public_ledger_daily_futures_reviews_cbot_soymeal", "News");
            d.Add("public_ledger_daily_futures_reviews_cbot_wheat", "News");
            d.Add("public_ledger_daily_futures_reviews_ice_canola", "News");
            d.Add("public_ledger_daily_futures_reviews_ice_cocoa", "News");
            d.Add("public_ledger_daily_futures_reviews_ice_coffee", "News");
            d.Add("public_ledger_daily_futures_reviews_ice_sugar", "News");
            d.Add("public_ledger_commodities", "News");
            d.Add("public_ledger_commodities_grains", "News");
            d.Add("public_ledger_commodities_grains_pulses", "News");
            d.Add("public_ledger_commodities_grains_barley", "News");
            d.Add("public_ledger_commodities_grains_corn", "News");
            d.Add("public_ledger_commodities_grains_rice", "News");
            d.Add("public_ledger_commodities_grains_wheat", "News");
            d.Add("public_ledger_commodities_oils_oilseeds", "News");
            d.Add("public_ledger_commodities_oils_oilseeds_coconut_oil", "News");
            d.Add("public_ledger_commodities_oils_oilseeds_olive_oil", "News");
            d.Add("public_ledger_commodities_oils_oilseeds_palm_kernel_oil", "News");
            d.Add("public_ledger_commodities_oils_oilseeds_palm_oil", "News");
            d.Add("public_ledger_commodities_oils_oilseeds_rapeseed", "News");
            d.Add("public_ledger_commodities_oils_oilseeds_rapeseed_oil", "News");
            d.Add("public_ledger_commodities_oils_oilseeds_soy", "News");
            d.Add("public_ledger_commodities_oils_oilseeds_soy_oil", "News");
            d.Add("public_ledger_commodities_oils_oilseeds_sunflowerseed", "News");
            d.Add("public_ledger_commodities_oils_oilseeds_sunflower_oil", "News");
            d.Add("public_ledger_commodities_oils_oilseeds_other_oils_oilseeds", "News");
            d.Add("public_ledger_commodities_feed", "News");
            d.Add("public_ledger_commodities_feed_ddgs", "News");
            d.Add("public_ledger_commodities_feed_rapemeal", "News");
            d.Add("public_ledger_commodities_feed_soymeal", "News");
            d.Add("public_ledger_commodities_feed_sunflowermeal", "News");
            d.Add("public_ledger_commodities_softs", "News");
            d.Add("public_ledger_commodities_softs_cocoa", "News");
            d.Add("public_ledger_commodities_softs_coffee", "News");
            d.Add("public_ledger_commodities_softs_sugar", "News");
            d.Add("public_ledger_commodities_softs_tea", "News");
            d.Add("public_ledger_commodities_spices", "News");
            d.Add("public_ledger_commodities_spices_cardamom", "News");
            d.Add("public_ledger_commodities_spices_chilli", "News");
            d.Add("public_ledger_commodities_spices_cinnamon", "News");
            d.Add("public_ledger_commodities_spices_cloves", "News");
            d.Add("public_ledger_commodities_spices_coriander_seed", "News");
            d.Add("public_ledger_commodities_spices_cumin", "News");
            d.Add("public_ledger_commodities_spices_garlic", "News");
            d.Add("public_ledger_commodities_spices_ginger", "News");
            d.Add("public_ledger_commodities_spices_nutmeg", "News");
            d.Add("public_ledger_commodities_spices_pepper", "News");
            d.Add("public_ledger_commodities_spices_vanilla", "News");
            d.Add("public_ledger_commodities_spices_other_spices", "News");
            d.Add("public_ledger_commodities_exotics", "News");
            d.Add("public_ledger_commodities_exotics_essential_oils", "News");
            d.Add("public_ledger_commodities_exotics_other_spices", "News");
            d.Add("public_ledger_commodities_exotics_waxes_and_gums", "News");
            d.Add("public_ledger_commodities_exotics_honey", "News");
            d.Add("public_ledger_commodities_dried_fruit", "News");
            d.Add("public_ledger_commodities_dried_fruit_apple", "News");
            d.Add("public_ledger_commodities_dried_fruit_apricots", "News");
            d.Add("public_ledger_commodities_dried_fruit_dates", "News");
            d.Add("public_ledger_commodities_dried_fruit_figs", "News");
            d.Add("public_ledger_commodities_dried_fruit_prunes", "News");
            d.Add("public_ledger_commodities_dried_fruit_tropicals", "News");
            d.Add("public_ledger_commodities_dried_fruit_vine_fruit", "News");
            d.Add("public_ledger_commodities_nuts", "News");
            d.Add("public_ledger_commodities_nuts_almonds", "News");
            d.Add("public_ledger_commodities_nuts_brazil_nuts", "News");
            d.Add("public_ledger_commodities_nuts_cashews", "News");
            d.Add("public_ledger_commodities_nuts_desiccated_coconut", "News");
            d.Add("public_ledger_commodities_nuts_hazelnuts", "News");
            d.Add("public_ledger_commodities_nuts_macadamias", "News");
            d.Add("public_ledger_commodities_nuts_peanuts", "News");
            d.Add("public_ledger_commodities_nuts_pecans", "News");
            d.Add("public_ledger_commodities_nuts_pistachios", "News");
            d.Add("public_ledger_commodities_nuts_walnuts", "News");
            d.Add("public_ledger_commodities_nuts_other_nuts", "News");
            d.Add("public_ledger_analysis", "Analysis");
            d.Add("public_ledger_analysis_company", "News");
            d.Add("public_ledger_analysis_company_operations", "News");
            d.Add("public_ledger_analysis_company_investments", "News");
            d.Add("public_ledger_analysis_company_manda", "News");
            d.Add("public_ledger_analysis_company_results", "News");
            d.Add("public_ledger_analysis_energy", "News");
            d.Add("public_ledger_analysis_freight", "News");
            d.Add("public_ledger_analysis_personnel", "News");
            d.Add("public_ledger_analysis_trading", "News");
            d.Add("public_ledger_analysis_weather", "News");

            return d.ContainsKey(contentName) ? d[contentName] : " ";
        }

        public Dictionary<string, string> GetXMLDataTaxonomy(XmlDocument xd, string nodeName, string site, string publication)
        {
            Dictionary<string, string> Taxonomy = new Dictionary<string, string>();
            XmlNode xn = xd.SelectSingleNode($"//{nodeName}");

            if (xn != null)
            {

                if (nodeName.Equals("TABLEAU"))
                {
                    XmlNodeList TABLEAUList = xd.SelectNodes($"//{nodeName}");

                    int numberofTableau = 0;
                    if (TABLEAUList.Count > 0)
                    {
                        foreach (XmlNode nodes in TABLEAUList)
                        {

                            numberofTableau++;
                            Taxonomy.Add("tableau" + numberofTableau + "-sourceid", nodes.Attributes["sourceid"].Value);
                            foreach (XmlNode node in nodes)
                            {
                                if (node.Attributes["name"] != null)
                                {
                                    Taxonomy.Add("tableau" + numberofTableau + node.Attributes["name"].Value, node.InnerText);
                                }
                                else if ((node.Name == "creator") && (node.Attributes["first-name"] != null))
                                {
                                    Taxonomy.Add("tableau" + numberofTableau + "first-name", node.InnerText);
                                }
                            }
                        }
                        Taxonomy.Add("Nooftableau", numberofTableau.ToString());
                    }

                }
                else if (nodeName.Equals("TAXONOMY"))
                {
                    if (publication == "Agrow")
                    {
                        Taxonomy.Add("CROPPROTECTION", "");
                        Taxonomy.Add("PRODUCT", "");
                        Taxonomy.Add("COMMERCIAL", "");
                        Taxonomy.Add("COUNTRY", "");
                        string CropProtection = string.Empty;
                        string Product = string.Empty;
                        string Commercial = string.Empty;
                        string Country = string.Empty;
                        foreach (XmlNode node in xn)
                        {
                            if (node.Attributes["unique-name"] != null)
                            {

                                if (CheckifExistsusingXML(node.Attributes["unique-name"].Value, publication, "cropprotection", site))
                                {

                                    CropProtection += node.Attributes["unique-name"].Value + ",";
                                    Taxonomy["CROPPROTECTION"] = CropProtection;

                                }
                                else if (CheckifExistsusingXML(node.Attributes["unique-name"].Value, publication, "commercial", site))
                                {

                                    Commercial += node.Attributes["unique-name"].Value + ",";
                                    Taxonomy["COMMERCIAL"] = Commercial;


                                }
                                else if (CheckifExistsusingXML(node.Attributes["unique-name"].Value, publication, "product", site))
                                {

                                    Product += node.Attributes["unique-name"].Value + ",";
                                    Taxonomy["PRODUCT"] = Commercial;

                                }     //countCommodityFactor++;

                                else if (CheckifExistsusingXML(node.Attributes["unique-name"].Value, publication, "region", site))
                                {
                                    Country += node.Attributes["unique-name"].Value + ",";
                                    Taxonomy["COUNTRY"] = Country;
                                }
                                //  countCommodityFactor++;

                            }

                        }

                    }

                    if (publication == "commodities")
                    {
                        Taxonomy.Add("COMMODITY", "");
                        Taxonomy.Add("COMMODITYFACTOR", "");
                        Taxonomy.Add("COMMERCIAL", "");
                        string Commodity = string.Empty;
                        string CommodityFactor = string.Empty;
                        string Commercial = string.Empty;
                        foreach (XmlNode node in xn)
                        {
                            if (node.Attributes["unique-name"] != null)
                            {

                                if (CheckifExistsusingXML(node.Attributes["unique-name"].Value, publication, "commodity", site))
                                {

                                    Commodity += node.Attributes["unique-name"].Value + ",";
                                    Taxonomy["COMMODITY"] = Commodity;

                                }
                                else if (CheckifExistsusingXML(node.Attributes["unique-name"].Value, publication, "commercial", site))
                                {

                                    Commercial += node.Attributes["unique-name"].Value + ",";
                                    Taxonomy["COMMERCIAL"] = Commercial;


                                }
                                else if (CheckifExistsusingXML(node.Attributes["unique-name"].Value, publication, "commodityfactor", site))
                                {
                                    if ((node.Attributes["unique-name"].Value) == "dairy_markets_analysis_trade")
                                    {
                                        CommodityFactor += "dairy_markets_analysis_trade_Import" + "," + "dairy_markets_analysis_trade_Export";
                                        //Taxonomy["COMMODITYFACTOR" + countCommodityFactor] = "dairy_markets_analysis_trade_Import";
                                        //countCommodityFactor++;
                                        //Taxonomy["COMMODITYFACTOR" + countCommodityFactor] = "dairy_markets_analysis_trade_Export";
                                        //countCommodityFactor++;
                                    }
                                    if (!((node.Attributes["unique-name"].Value) == "dairy_markets_analysis_trade"))
                                    {
                                        CommodityFactor += node.Attributes["unique-name"].Value + ",";
                                        Taxonomy["COMMODITYFACTOR"] = CommodityFactor;
                                    }
                                    //  countCommodityFactor++;

                                }

                            }

                        }

                    }

                    if (publication == "AnimalPharma")
                    {
                        Taxonomy.Add("ANIMALHEALTH", "");
                        Taxonomy.Add("COMMERCIAL", "");
                        Taxonomy.Add("COUNTRY", "");
                        string AnimalPharma = string.Empty;
                        string Commercial = string.Empty;
                        string Country = string.Empty;
                        foreach (XmlNode node in xn)
                        {
                            if (node.Attributes["unique-name"] != null)
                            {

                                if (CheckifExistsusingXML(node.Attributes["unique-name"].Value, publication, "animalhealth", site))
                                {

                                    AnimalPharma += node.Attributes["unique-name"].Value + ",";
                                    Taxonomy["ANIMALHEALTH"] = AnimalPharma;

                                }
                                else if (CheckifExistsusingXML(node.Attributes["unique-name"].Value, publication, "commercial", site))
                                {

                                    Commercial += node.Attributes["unique-name"].Value + ",";
                                    Taxonomy["COMMERCIAL"] = Commercial;


                                }


                                else if (CheckifExistsusingXML(node.Attributes["unique-name"].Value, publication, "region", site))
                                {
                                    Country += node.Attributes["unique-name"].Value + ",";
                                    Taxonomy["COUNTRY"] = Country;
                                }
                                //  countCommodityFactor++;

                            }

                        }

                    }


                    if (publication == "ID")
                    {

                        Taxonomy.Add("MARKET", "");
                        Taxonomy.Add("TOPICS", "");
                        Taxonomy.Add("COUNTRY", "");
                        Taxonomy.Add("SECTINREF", "");
                        Taxonomy.Add("SECTORS", "");
                        string Market = string.Empty;
                        string Topic = string.Empty;
                        string Country = string.Empty;
                        string SectionRef = string.Empty;
                        foreach (XmlNode node in xn)
                        {
                            if (node.Attributes["unique-name"] != null)
                            {
                                if (CheckifExistsusingXML(node.Attributes["unique-name"].Value, publication, "market", site))
                                {
                                    Market += node.Attributes["unique-name"].Value + ",";
                                    Taxonomy["MARKET"] = Market;

                                }
                                else if (CheckifExistsusingXML(node.Attributes["unique-name"].Value, publication, "topics", site))
                                {

                                    Topic += node.Attributes["unique-name"].Value + ",";
                                    Taxonomy["TOPICS"] = Topic;
                                }


                                else if (CheckifExistsusingXML(node.Attributes["unique-name"].Value, publication, "country", site))
                                {
                                    Country += node.Attributes["unique-name"].Value + ",";
                                    Taxonomy["COUNTRY"] = Country;
                                }
                                else if (CheckifExistsusingXML(node.Attributes["unique-name"].Value, publication, "sectors", site))
                                {
                                    Country += node.Attributes["unique-name"].Value + ",";
                                    Taxonomy["SECTORS"] = Country;
                                }
                                else
                                {
                                    SectionRef += node.Attributes["unique-name"].Value + ",";
                                    Taxonomy["SECTINREF"] = SectionRef;

                                }

                            }
                        }
                    }



                    if (publication == "LL")
                    {
                        Taxonomy.Add("COUNTRY", "");
                        Taxonomy.Add("HOTTOPICS", "");
                        Taxonomy.Add("MARKET", "");
                        Taxonomy.Add("REGULARS", "");
                        Taxonomy.Add("SECTORS", "");
                        Taxonomy.Add("TOPICS", "");
                        Taxonomy.Add("SECTINREF", "");

                        string Country = string.Empty;
                        string HotTopics = string.Empty;
                        string Media = string.Empty;
                        string Market = string.Empty;
                        string Regulars = string.Empty;
                        string Sectors = string.Empty;
                        string Topic = string.Empty;
                        string SectionRef = string.Empty;

                        foreach (XmlNode node in xn)
                        {
                            if (node.Attributes["unique-name"] != null)
                            {
                                if (CheckifExistsusingXML(node.Attributes["unique-name"].Value, publication, "market", site))
                                {
                                    Market += node.Attributes["unique-name"].Value + ",";
                                    Taxonomy["MARKET"] = Market;

                                }
                                else if (CheckifExistsusingXML(node.Attributes["unique-name"].Value, publication, "topics", site))
                                {

                                    Topic += node.Attributes["unique-name"].Value + ",";
                                    Taxonomy["TOPICS"] = Topic;
                                }


                                else if (CheckifExistsusingXML(node.Attributes["unique-name"].Value, publication, "country", site))
                                {
                                    Country += node.Attributes["unique-name"].Value + ",";
                                    Taxonomy["COUNTRY"] = Country;
                                }
                                else if (CheckifExistsusingXML(node.Attributes["unique-name"].Value, publication, "sectors", site))
                                {
                                    Sectors += node.Attributes["unique-name"].Value + ",";
                                    Taxonomy["SECTORS"] = Sectors;
                                }
                                else if (CheckifExistsusingXML(node.Attributes["unique-name"].Value, publication, "regulars", site))
                                {
                                    Regulars += node.Attributes["unique-name"].Value + ",";
                                    Taxonomy["REGULARS"] = Regulars;
                                }
                                else if (CheckifExistsusingXML(node.Attributes["unique-name"].Value, publication, "mediatype", site))
                                {
                                    Media = node.Attributes["unique-name"].Value;
                                    if (!Taxonomy.ContainsKey("MEDIA"))
                                    {
                                        Taxonomy.Add("MEDIA", "");
                                    }

                                    Taxonomy["MEDIA"] = Media;
                                }
                                else if (CheckifExistsusingXML(node.Attributes["unique-name"].Value, publication, "hottopics", site))
                                {
                                    HotTopics += node.Attributes["unique-name"].Value + ",";
                                    Taxonomy["HOTTOPICS"] = HotTopics;
                                }
                                else
                                {
                                    SectionRef += node.Attributes["unique-name"].Value + ",";
                                    Taxonomy["SECTINREF"] = SectionRef;
                                }
                            }
                        }
                    }
                }
            }

            return Taxonomy;
        }

        public Dictionary<string, string> GetXMLDataTaxonomyList(XmlDocument xd, string nodeName)
        {
            Dictionary<string, string> TaxonomyList = new Dictionary<string, string>();
            XmlNode xn = xd.SelectSingleNode($"//{nodeName}");

            if (xn != null)
            {


                if (nodeName.Equals("TAXONOMY"))
                {


                    int count = 1;
                    foreach (XmlNode node in xn)
                    {
                        if (node.Attributes["unique-name"] != null)
                        {


                            TaxonomyList[count.ToString()] = node.Attributes["unique-name"].Value;

                            count++;

                        }

                    }
                }
            }
            return TaxonomyList;
        }

        public string GetXMLData(XmlDocument xd, string nodeName)
        {
            if (!nodeName.Equals("THERAPY_SECTOR")
                    && !nodeName.Equals("TREATABLE_CONDITION")
                    && !nodeName.Equals("COMPANY")
                    && !nodeName.Equals("COUNTRY"))
            {

                XmlNode xn = xd.SelectSingleNode($"//{nodeName}");


                if (nodeName == "IMAGE")
                {
                    XmlNodeList imgList = xd.SelectNodes($"//{nodeName}");
                    StringBuilder imgStrb = new StringBuilder();
                    foreach (XmlNode node in imgList)
                    {
                        string nodeValue = "<image src='" + node["SRC"].InnerText + "' sourceid ='" + node.Attributes["sourceid"].Value + "'/> ";
                        if (imgStrb.Length > 0)
                            imgStrb.Append("");
                        imgStrb.Append(nodeValue);
                    }
                    return imgStrb.ToString();
                }
                // adding image an video tag  
                if (nodeName.Equals("LEADIMAGE"))
                {
                    return (xn != null) ? "<img src='" + xn["SRC"].InnerText + "' >" : string.Empty;
                }
                //else if (nodeName.Equals("IMAGE"))
                //{
                //    return (xn != null) ? "<img src='" + xn["SRC"].InnerText + "' >" : string.Empty;
                //}
                else if (nodeName.Equals("VIDEO"))
                {
                    return (xn != null) ? xn["SRC"].InnerText : string.Empty;
                }
                else
                {
                    return (xn != null) ? xn.InnerText : string.Empty;
                }


            }


            XmlNodeList list = xd.SelectNodes(string.Format("//{0}", nodeName));
            StringBuilder sb = new StringBuilder();
            foreach (XmlNode xn in list)
            {
                string nodeValue = xn.InnerText;
                if (sb.Length > 0)
                    sb.Append(",");
                sb.Append(nodeValue);
            }
            return sb.ToString();
        }

        public string GetImgData(XmlDocument xd, string nodeName, string publisheddate, string articleid)
        {


            XmlNode xn = xd.SelectSingleNode($"//{nodeName}");

            StringBuilder imgStrb = new StringBuilder();
            if (nodeName == "IMAGE")
            {
                XmlNodeList imgList = xd.SelectNodes($"//{nodeName}");


                foreach (XmlNode node in imgList)
                {
                    string nodeValue = node["SRC"].InnerText;
                    if (imgStrb.Length > 0)
                        imgStrb.Append(",");
                    imgStrb.Append(nodeValue + "date" + publisheddate + "articleid" + articleid);
                }

            }
            // adding image an video tag  
            if (nodeName.Equals("LEADIMAGE"))
            {
                string strLeadimg = xn["SRC"].InnerText;
                if (xn != null)
                {
                    imgStrb.Append(strLeadimg + "date" + publisheddate + "articleid" + articleid);
                }
            }



            return imgStrb.ToString();
        }
        public static string UpperCaseUrlEncode(string s)
        {

            char[] temp = System.Web.HttpUtility.UrlEncode(s).ToCharArray();
            for (int i = 0; i < temp.Length - 2; i++)
            {
                if (temp[i] == '%')
                {
                    temp[i + 1] = char.ToUpper(temp[i + 1]);
                    temp[i + 2] = char.ToUpper(temp[i + 2]);
                }
            }

            string test = new string(temp);
            test = test.Replace("(", "%28").Replace(")", "%29");
            return test;
        }

        public MediaItem ImportImage(string url, string fileName, string newPath, MediaItem mediaItem = null)
        {

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null)
                return null;

            try
            {
                // download data 
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream stream1 = response.GetResponseStream();
                MemoryStream stream2 = new MemoryStream();
                stream1.CopyTo(stream2);
                // Create the options
                MediaCreatorOptions options = new MediaCreatorOptions();
                options.FileBased = false;
                options.IncludeExtensionInItemName = false;
                options.KeepExisting = false;
                options.Versioned = false;
                options.Destination = newPath;
                options.Database = Sitecore.Configuration.Factory.GetDatabase("master");

                // upload to sitecore
                MediaCreator creator = new MediaCreator();
                using (new SecurityDisabler()) // Use the SecurityDisabler object to override the security settings
                {
                    mediaItem = creator.CreateFromStream(stream2, fileName, options);

                    response.Close();
                    XMLDataLogger.WriteLog("Image create in media library:" + mediaItem.Path, "ImageLog");
                    return mediaItem;
                }
            }
            catch (WebException ex)
            {
                XMLDataLogger.WriteLog("Image create error in  media library:" + ex.Message, "ImageLog");
                return null;
            }
        }
        public IEnumerable<Item> GetMediaItems(IDataMap map)
        {
            string cacheKey = "Images";
            IEnumerable<Item> o = Context.Items[cacheKey] as IEnumerable<Item>;
            if (o != null)
                return o;

            Item rootItem = map.ToDB.GetItem(Sitecore.Data.ID.Parse(WebConfigurationManager.AppSettings["Image_path"]));
            IEnumerable<Item> images = rootItem.Axes.GetDescendants();
            Context.Items.Add(cacheKey, images);

            return images;
        }
        public List<MediaItem> DownloadImages(List<string> urls, IDataMap map)
        {
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                List<MediaItem> mediaitemlst = new List<MediaItem>();
                string url = string.Empty;
                string articleid = string.Empty;
                string date = string.Empty;
                foreach (string item in urls)
                {
                    try
                    {
                        if (item != "")
                        {
                            url = item.Substring(0, item.IndexOf("date"));
                            date = item.Substring(item.IndexOf("date"), item.Length - item.IndexOf("date") - 15).Remove(0, 4);
                            articleid = item.Substring(item.IndexOf("articleid"), item.Length - item.IndexOf("articleid")).Remove(0, 9);

                            IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);
                            DateTime dt = DateTime.Parse(date, culture, System.Globalization.DateTimeStyles.AssumeLocal);

                            url = url.Replace("192.168.45.101:8080", "www.scripintelligence.com")
                        .Replace("62.73.128.229", "www.scripintelligence.com");
                            if (url.StartsWith("/scripnews") || url.StartsWith("/multimedia"))
                                url = $"http://www.scripintelligence.com{url}";
                            else if (url.Contains("scripnews.com"))
                                url = url.Replace("scripnews.com", "scripintelligence.com");

                            //if (url.Contains(" "))
                            //{
                            //    url = url.Replace(" ", "+");
                            //}

                            url = new Regex("/[^/]*$").Replace(url, "/" + UpperCaseUrlEncode(url.Split('/').Last()));

                            // see if the url is badly formed
                            //if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                            //{
                            //    XMLDataLogger.WriteLog(ArticleId, "InvalidImageLog");
                            //    map.Logger.Log("ArticleId: " + ArticleId + articlePath, "malformed image URL", ProcessStatus.FieldError, url);
                            //    return null;
                            //}

                            //get file info
                            List<string> uri = url.Split(new string[] { "?" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            List<string> parts = uri[0].Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            string filePath = parts[parts.Count - 1].Trim();
                            string[] fileParts = filePath.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                            string fileName = (fileParts.Length > 0) ? StringUtility.GetValidItemName(fileParts[0], map.ItemNameMaxLength) : string.Empty;

                            //date info
                            string newFilePath = (dt.Year != 1800) ? $"{dt.ToString("yyyy/MMMM")}/{fileName}" : fileName;

                            // see if it exists in med lib

                            Item rootItem = map.ToDB.GetItem(Sitecore.Data.ID.Parse(WebConfigurationManager.AppSettings["Image_path"]));
                            IEnumerable<Item> matches = GetMediaItems(map)
                                .Where(a => a.Paths.FullPath.EndsWith(fileName));

                            MediaItem mediaitem = null;
                            if (matches != null && matches.Any())
                            {
                                if (matches.Count() > 0)
                                {
                                    foreach (Item match in matches)
                                    {
                                        if (match.Paths.FullPath.Contains(newFilePath))
                                        {
                                            XMLDataLogger.WriteLog("Image exist in Meidia Library:" + match.Paths.FullPath, "ImageLog");
                                            //return new MediaItem(matches.First());
                                            //return match;
                                            mediaitem = match;
                                        }
                                    }

                                }

                                //   map.Logger.Log(articlePath, $"Sitecore image matched {matches.Count()} images", ProcessStatus.FieldError, filePath);
                            }

                            MediaItem m = ImportImage(url, filePath, $"{rootItem.Paths.FullPath}/{newFilePath}", mediaitem);

                            mediaitemlst.Add(m);
                        }

                    }

                    catch (Exception ex)
                    {
                        XMLDataLogger.WriteLog(articleid, "ImageFormatIncorrect");
                        continue;
                    }


                }
                return mediaitemlst;
            }

        }

        protected void Log(string errorType, string message)
        {
            log.AppendFormat("{0} : {1}", errorType, message).AppendLine().AppendLine();
        }

        /// There is no custom data for this type
        public override void ProcessCustomData(ref Item newItem, object importRow)
        {

        }

        /// gets a field value from an item
        public override string GetFieldValue(object importRow, string fieldName)
        {
            Dictionary<string, string> r = (Dictionary<string, string>)importRow;

            return r[fieldName];
        }

        #endregion Override Methods

        #region Methods

        protected List<string> SplitString(string str, string splitter)
        {
            // string split options set to none so that empty columns are allowed
            // useful for importing large csv files, so you don't have to check the content
            return str.Split(new string[] { splitter }, StringSplitOptions.None).ToList();
        }

        protected string GetFileAsString(string path)
        {
            Encoding et = Encoding.GetEncoding("utf-8");
            byte[] aBytes = GetFileBytes(path);
            return et.GetString(aBytes);
        }

        protected byte[] GetFileBytes(string filePath)
        {
            //open the file selected
            FileInfo f = new FileInfo(filePath);
            FileStream s = f.OpenRead();
            byte[] bytes = new byte[s.Length];
            s.Position = 0;
            s.Read(bytes, 0, int.Parse(s.Length.ToString()));
            return bytes;
        }

        protected XmlDocument GetXmlDocument(string filePath)
        {
            string data = GetFileAsString(filePath);

            XmlDocument d = new XmlDocument();
            try
            {
                d.LoadXml(data);
                return d;
            }
            catch (Exception ex)
            {
                Logger.Log("N/A", string.Format("Xml file data was malformed: {0}", ex.Message), ProcessStatus.Error, "File", filePath);
            }
            return null;
        }

        public virtual Dictionary<string, string> GetMapping()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            //  d.Add("dairy_markets", "Dairy");
            //  d.Add("dairy_markets_subscribe_free_demo", "Dairy");
            //  d.Add("dairy_markets_features", "Dairy");
            // d.Add("dairy_markets_downloads", "Dairy");
            //   d.Add("dairy_markets_filedownloads", "Dairy");
            //   d.Add("dairy_markets_subscribe_free_demo_mobile", "Dairy");
            //   d.Add("dairy_markets_resources_dairy_ezine_mobile", "Dairy");
            d.Add("dairy_markets_markets", "Dairy");
            d.Add("dairy_markets_markets_butter", "Butter");
            d.Add("dairy_markets_markets_cheese", "Cheese");
            d.Add("dairy_markets_markets_milk", "Milk");
            d.Add("dairy_markets_markets_milk_liquid", "Milk");
            d.Add("dairy_markets_markets_milk_raw", "Milk");
            d.Add("dairy_markets_markets_powders", "Dairy");
            d.Add("dairy_markets_markets_powders_casein", "Casein");
            d.Add("dairy_markets_markets_powders_imf", "IMF");
            d.Add("dairy_markets_markets_powders_lactose", "Lactose");
            d.Add("dairy_markets_markets_powders_smp", "SMP");
            d.Add("dairy_markets_markets_powders_wmp", "WMP");
            d.Add("dairy_markets_markets_powders_whey_powder", "Whey Powder");
            d.Add("dairy_markets_markets_other_dairy", "Dairy Products");
            d.Add("dairy_markets_markets_other_dairy_fresh_products", "Dairy Products");
            d.Add("dairy_markets_markets_other_dairy_frozen_products", "Dairy Products");
            d.Add("dairy_markets_analysis", "Dairy");
            d.Add("dairy_markets_analysis_company", "Dairy");




            d.Add("foodnews_beverages_fruit_juices", "Beverages");
            d.Add("foodnews_beverages_fruit_juices_passion_fruit", "Passion Fruit Juice");
            d.Add("foodnews_beverages_fruit_juices_cranberry", "Cranberry Juice");
            d.Add("foodnews_beverages_fruit_juices_apple", "Apple Juice");
            d.Add("foodnews_beverages_fruit_juices_blends", "Blended Juices");
            d.Add("foodnews_beverages_fruit_juices_grape", "Grape Juice");
            d.Add("foodnews_beverages_fruit_juices_lemon", "Lemon Juice");
            d.Add("foodnews_beverages_fruit_juices_mango", "Mango Juice");
            d.Add("foodnews_beverages_fruit_juices_nfc", "NFC Juices");
            d.Add("foodnews_beverages_fruit_juices_oils", "Citrus Oils");
            d.Add("foodnews_beverages_fruit_juices_orange", "Orange Juice");
            d.Add("foodnews_beverages_fruit_juices_pineapple", "Pineapple Juice");
            d.Add("foodnews_beverages_fruit_juices_superfruits", "Superfruit Juices");
            d.Add("foodnews_beverages_fruit_juices_tropical", "Tropical Juices");
            d.Add("foodnews_beverages_fruit_juices_vegetable", "Vegetable Juice");
            d.Add("foodnews_beverages_alcoholic_drinks", "Beverages");
            d.Add("foodnews_beverages_alcoholic_drinks_wines", "Wines");
            d.Add("foodnews_beverages_alcoholic_drinks_wines_still", "Wines");
            d.Add("foodnews_beverages_alcoholic_drinks_wines_sparkling", "Wines");
            d.Add("foodnews_beverages_alcoholic_drinks_cider", "Cider");
            d.Add("foodnews_beverages_soft_drinks", "Beverages");
            d.Add("foodnews_beverages_soft_drinks_bottled_water", "Bottled Water");
            d.Add("foodnews_beverages_soft_drinks_carbonates", "Carbonates");
            d.Add("foodnews_beverages_soft_drinks_fruit_drinks", "Fruit Drinks");
            d.Add("foodnews_beverages_soft_drinks_smoothies", "Smoothies");
            d.Add("foodnews_beverages_soft_drinks_sports_energy_drinks", "Sports/Energy Drinks");
            d.Add("foodnews_beverages_puree", "Beverages");
            d.Add("foodnews_beverages_puree_apple", "Apple Puree");
            d.Add("foodnews_beverages_puree_tropicals", "Tropical Puree");
            d.Add("foodnews_beverages_puree_apricot", "Apricot Puree");
            d.Add("foodnews_beverages_puree_banana", "Banana Puree");
            d.Add("foodnews_beverages_puree_mango", "Mango Puree");
            d.Add("foodnews_beverages_puree_peach", "Peach Puree");
            d.Add("foodnews_beverages_puree_pear", "Pear Puree");
            d.Add("foodnews_beverages_wines", "Wines");
            d.Add("foodnews_beverages_wines_sparkling", "Wines");
            d.Add("foodnews_beverages_wines_still", "Wines");
            d.Add("foodnews_canned", "Canned & Tomato Products");
            d.Add("foodnews_canned_canned_fruit", "Canned & Tomato Products");
            d.Add("foodnews_canned_canned_fruit_apricots", "Canned Apricots");
            d.Add("foodnews_canned_canned_fruit_fruit_cocktail", "Canned Fruit Cocktail");
            d.Add("foodnews_canned_canned_fruit_mandarins", "Canned Mandarins");
            d.Add("foodnews_canned_canned_fruit_peaches", "Canned Peaches");
            d.Add("foodnews_canned_canned_fruit_pears", "Canned Pears");
            d.Add("foodnews_canned_canned_fruit_pineapple", "Canned Pineapple");
            d.Add("foodnews_canned_canned_vegetables", "Canned & Tomato Products");
            d.Add("foodnews_canned_canned_vegetables_artichokes", "Canned Artichokes");
            d.Add("foodnews_canned_canned_vegetables_asparagus", "Canned Asparagus");
            d.Add("foodnews_canned_canned_vegetables_beans", "Canned Beans");
            d.Add("foodnews_canned_canned_vegetables_mushrooms", "Canned Mushrooms");
            d.Add("foodnews_canned_canned_vegetables_peas", "Canned Peas");
            d.Add("foodnews_canned_canned_vegetables_sweetcorn", "Canned Sweetcorn");
            d.Add("foodnews_canned_canned_fish", "Canned & Tomato Products");
            d.Add("foodnews_canned_canned_fish_anchovies", "Canned Anchovies");
            d.Add("foodnews_canned_canned_fish_mackerel", "Canned Mackerel");
            d.Add("foodnews_canned_canned_fish_salmon", "Canned Salmon");
            d.Add("foodnews_canned_canned_fish_sardines", "Canned Sardines");
            d.Add("foodnews_canned_canned_fish_tuna", "Canned Tuna");
            d.Add("foodnews_canned_canned_meat", "Canned Meat");
            d.Add("foodnews_canned_canned_meat_beef", "Canned Meat");
            d.Add("foodnews_canned_canned_meat_pork", "Canned Meat");
            d.Add("foodnews_frozen", "Frozen Foods");
            d.Add("foodnews_frozen_frozen_fruit", "");
            d.Add("foodnews_frozen_frozen_fruit_blueberries", "Frozen Blueberries");
            d.Add("foodnews_frozen_frozen_fruit_cranberries", "Frozen Cranberries");
            d.Add("foodnews_frozen_frozen_fruit_raspberries", "Frozen Raspberries");
            d.Add("foodnews_frozen_frozen_fruit_strawberries", "Frozen Strawberries");
            d.Add("foodnews_frozen_frozen_fruit_other_fruit", "Frozen Foods");
            d.Add("foodnews_frozen_frozen_vegetables", "Frozen Foods");
            d.Add("foodnews_frozen_frozen_vegetables_other_vegetables", "Frozen Foods");
            d.Add("foodnews_frozen_frozen_vegetables_asparagus", "Frozen Asparagus");
            d.Add("foodnews_frozen_frozen_vegetables_beans", "Frozen Beans");
            d.Add("foodnews_frozen_frozen_vegetables_carrots", "Frozen Carrots");
            d.Add("foodnews_frozen_frozen_vegetables_cauliflower", "Frozen Cauliflower");
            d.Add("foodnews_frozen_frozen_vegetables_mushrooms", "Frozen Mushrooms");
            d.Add("foodnews_frozen_frozen_vegetables_peas", "Frozen Peas");
            d.Add("foodnews_frozen_frozen_vegetables_potatoes", "Frozen Potatoes");
            d.Add("foodnews_frozen_frozen_vegetables_sweetcorn", "Frozen Sweetcorn");
            d.Add("foodnews_frozen_frozen_foods", "Frozen Foods");
            d.Add("foodnews_frozen_frozen_foods_ice_cream", "Ice Cream");
            d.Add("foodnews_frozen_frozen_foods_prepared_foods", "Prepared Frozen Foods");
            d.Add("foodnews_dfn", "Dried Fruit & Nuts");
            d.Add("foodnews_dfn_nuts", "Dried Fruit & Nuts");
            d.Add("foodnews_dfn_nuts_nut_milks", "Nut Milks");
            d.Add("foodnews_dfn_nuts_almonds", "Almonds");
            d.Add("foodnews_dfn_nuts_brazil_nuts", "Brazil Nuts");
            d.Add("foodnews_dfn_nuts_cashews", "Cashews");
            d.Add("foodnews_dfn_nuts_desiccated_coconut", "Desiccated Coconut");
            d.Add("foodnews_dfn_nuts_hazelnuts", "Hazelnuts");
            d.Add("foodnews_dfn_nuts_macadamias", "Macadamias");
            d.Add("foodnews_dfn_nuts_peanuts", "Peanuts");
            d.Add("foodnews_dfn_nuts_pecans", "Pecans");
            d.Add("foodnews_dfn_nuts_pistachios", "Pistachios");
            d.Add("foodnews_dfn_nuts_walnuts", "Walnuts");
            d.Add("foodnews_dfn_nuts_other_nuts", "Dried Fruit & Nuts");
            d.Add("foodnews_dfn_dried_fruit", "Dried Fruit & Nuts");
            d.Add("foodnews_dfn_dried_fruit_apple", "Dried Apple");
            d.Add("foodnews_dfn_dried_fruit_apricots", "Dried Apricots");
            d.Add("foodnews_dfn_dried_fruit_dates", "Dates");
            d.Add("foodnews_dfn_dried_fruit_figs", "Figs");
            d.Add("foodnews_dfn_dried_fruit_prunes", "Prunes");
            d.Add("foodnews_dfn_dried_fruit_tropicals", "Dried Tropical Fruit");
            d.Add("foodnews_dfn_dried_fruit_vine_fruit", "Dried Vine Fruit");
            d.Add("foodnews_dfn_dried_fruit_other_fruit", "Dried Fruit & Nuts");
            d.Add("foodnews_dfn_dehydrates", "Dried Fruit & Nuts");
            d.Add("foodnews_dfn_dehydrates_apple", "Dehydrated Apple");
            d.Add("foodnews_dfn_dehydrates_banana", "Dehydrated Banana");
            d.Add("foodnews_dfn_dehydrates_mango", "Dehydrated Mango");
            d.Add("foodnews_dfn_dehydrates_pineapple", "Dehydrated Pineapple");
            d.Add("foodnews_dfn_dehydrates_strawberries", "Dehydrated Strawberries");
            d.Add("foodnews_raw_material", "Fresh Fruit & Vegetables");
            d.Add("foodnews_raw_material_fresh_fruit", "Fresh Fruit & Vegetables");
            d.Add("foodnews_raw_material_fresh_fruit_pineapple", "Fresh Pineapple");
            d.Add("foodnews_raw_material_fresh_fruit_tomatoes", "Fresh Tomatoes");
            d.Add("foodnews_raw_material_fresh_fruit_berries", "Fresh Berries");
            d.Add("foodnews_raw_material_fresh_fruit_tropicals", "Fresh Tropical Fruit");
            d.Add("foodnews_raw_material_fresh_fruit_apples", "Fresh Apples");
            d.Add("foodnews_raw_material_fresh_fruit_apricots", "Fresh Apricots");
            d.Add("foodnews_raw_material_fresh_fruit_bananas", "Fresh Bananas");
            d.Add("foodnews_raw_material_fresh_fruit_grapes", "Fresh Grapes");
            d.Add("foodnews_raw_material_fresh_fruit_lemons", "Fresh Lemons");
            d.Add("foodnews_raw_material_fresh_fruit_oranges", "Fresh Oranges");
            d.Add("foodnews_raw_material_fresh_fruit_peaches", "Fresh Peaches");
            d.Add("foodnews_raw_material_fresh_fruit_pears", "Fresh Pears");
            d.Add("foodnews_raw_material_fresh_fruit_plums", "Fresh Plums");
            d.Add("foodnews_raw_material_fresh_vegetables", "Fresh Fruit & Vegetables");
            d.Add("foodnews_raw_material_fresh_vegetables_artichokes", "Fresh Artichokes");
            d.Add("foodnews_raw_material_fresh_vegetables_other_vegetables", "Fresh Fruit & Vegetables");
            d.Add("foodnews_raw_material_fresh_vegetables_root_vegetables", "Fresh Root Vegetables");
            d.Add("foodnews_raw_material_fresh_vegetables_asparagus", "Fresh Asparagus");
            d.Add(" foodnews_raw_material_fresh_vegetables_beans", "Fresh Beans");
            d.Add("foodnews_raw_material_fresh_vegetables_mushrooms", "Fresh Mushrooms");
            d.Add("foodnews_raw_material_fresh_vegetables_peas", "Fresh Peas");
            d.Add("foodnews_raw_material_fresh_vegetables_sweetcorn", "Fresh Sweetcorn");
            d.Add("foodnews_tomato_products", "Canned & Tomato Products");
            d.Add("foodnews_tomato_products_canned", "Canned Tomatoes");
            d.Add("foodnews_tomato_products_ketchup", "Tomato Ketchup");
            d.Add("foodnews_tomato_products_paste", "Tomato Paste");



            d.Add("public_ledger_world_grain_market_report", "Grains & Feed");


            d.Add("public_ledger_daily_futures_reviews", "Grains & Feed");

            d.Add("public_ledger_daily_futures_reviews_cbot_corn", "Corn");

            d.Add("public_ledger_daily_futures_reviews_cbot_rice", "Rice");

            d.Add("public_ledger_daily_futures_reviews_cbot_soy_oil", "Soy Oil");

            d.Add("public_ledger_daily_futures_reviews_cbot_soybeans", "Soy");

            d.Add("public_ledger_daily_futures_reviews_cbot_soymeal", "Soymeal");

            d.Add("public_ledger_daily_futures_reviews_cbot_wheat", "Wheat");

            d.Add("public_ledger_daily_futures_reviews_ice_canola", "Rapeseed");

            d.Add("public_ledger_daily_futures_reviews_ice_cocoa", "Cocoa");

            d.Add("public_ledger_daily_futures_reviews_ice_coffee", "Coffee");

            d.Add("public_ledger_daily_futures_reviews_ice_sugar", "Sugar");

            d.Add("public_ledger_commodities_grains_pulses", "Pulses");

            d.Add("public_ledger_commodities_grains_barley", "Barley");

            d.Add("public_ledger_commodities_grains_corn", "Corn");

            d.Add("public_ledger_commodities_grains_rice", "Rice");

            d.Add("public_ledger_commodities_grains_wheat", "Wheat");


            d.Add("public_ledger_commodities_oils_oilseeds", "Oils & Oilseeds");

            d.Add("public_ledger_commodities_oils_oilseeds_coconut_oil", "Coconut Oil");

            d.Add("public_ledger_commodities_oils_oilseeds_olive_oil", " Olive Oil");

            d.Add("public_ledger_commodities_oils_oilseeds_palm_kernel_oil", "Palm Kernel Oil");

            d.Add("public_ledger_commodities_oils_oilseeds_palm_oil", "Palm Oil");

            d.Add("public_ledger_commodities_oils_oilseeds_rapeseed", "Rapeseed");

            d.Add("public_ledger_commodities_oils_oilseeds_rapeseed_oil", "Rapeseed Oil");

            d.Add("public_ledger_commodities_oils_oilseeds_soy", "Soy");

            d.Add("public_ledger_commodities_oils_oilseeds_soy_oil", "Soy Oil");

            d.Add("public_ledger_commodities_oils_oilseeds_sunflowerseed", "Sunflowerseed");

            d.Add("public_ledger_commodities_oils_oilseeds_sunflower_oil", "Sunflower Oil");

            d.Add("public_ledger_commodities_oils_oilseeds_other_oils_oilseeds", "Oils & Oilseeds");

            d.Add("public_ledger_commodities_feed", "Grains & Feed");


            d.Add("public_ledger_commodities_feed_ddgs", "DDGS");

            d.Add("public_ledger_commodities_feed_rapemeal", "Rapemeal");

            d.Add("public_ledger_commodities_feed_soymeal", "Soymeal");

            d.Add("public_ledger_commodities_feed_sunflowermeal", "Sunflowermeal");


            d.Add("public_ledger_commodities_softs_cocoa", "Cocoa");

            d.Add("public_ledger_commodities_softs_coffee", "Coffee");

            d.Add("public_ledger_commodities_softs_sugar", "Sugar");

            d.Add("public_ledger_commodities_softs_tea", "Tea");

            d.Add("public_ledger_commodities_spices", "Spices & Exotics");

            d.Add("public_ledger_commodities_spices_cardamom", "Cardamom");

            d.Add("public_ledger_commodities_spices_chilli", "Chilli");

            d.Add("public_ledger_commodities_spices_cinnamon", "Cinnamon");

            d.Add("public_ledger_commodities_spices_cloves", "Cloves");

            d.Add("public_ledger_commodities_spices_coriander_seed", "Coriander Seed");

            d.Add("public_ledger_commodities_spices_cumin", "Cumin");

            d.Add("public_ledger_commodities_spices_garlic", "Garlic");

            d.Add("public_ledger_commodities_spices_ginger", "Ginger");

            d.Add("public_ledger_commodities_spices_nutmeg", "Nutmeg");

            d.Add("public_ledger_commodities_spices_pepper", "Pepper");

            d.Add("public_ledger_commodities_spices_vanilla", "Vanilla");

            d.Add("public_ledger_commodities_spices_other_spices", "Spices & Exotics");

            d.Add("public_ledger_commodities_exotics", "Spices & Exotics");

            d.Add("public_ledger_commodities_exotics_essential_oils", "Essential Oils");

            d.Add("public_ledger_commodities_exotics_other_spices", "Spices & Exotics");

            d.Add("public_ledger_commodities_exotics_waxes_and_gums", "Waxes & Gums");

            d.Add("public_ledger_commodities_exotics_honey", "Honey");

            d.Add("public_ledger_commodities_dried_fruit", "Dried Fruit & Nuts");



            d.Add("public_ledger_commodities_dried_fruit_apple", " Dried Apple");

            d.Add("public_ledger_commodities_dried_fruit_apricots", "Dried Apricots");

            d.Add("public_ledger_commodities_dried_fruit_dates", "Dates");

            d.Add("public_ledger_commodities_dried_fruit_figs", "Figs");

            d.Add("public_ledger_commodities_dried_fruit_prunes", "Prunes");

            d.Add("public_ledger_commodities_dried_fruit_tropicals", "Dried Tropical Fruit");

            d.Add("public_ledger_commodities_dried_fruit_vine_fruit", "Dried Vine Fruit");

            d.Add("public_ledger_commodities_nuts", "Almonds");

            d.Add("public_ledger_commodities_nuts_almonds", "Dried Fruit & Nuts");

            d.Add("public_ledger_commodities_nuts_brazil_nuts", "Brazil Nuts");

            d.Add("public_ledger_commodities_nuts_cashews", "Cashews");

            d.Add("public_ledger_commodities_nuts_desiccated_coconut", "Desiccated Coconut");

            d.Add("public_ledger_commodities_nuts_hazelnuts", "Hazelnuts");

            d.Add("public_ledger_commodities_nuts_macadamias", "Macadamias");

            d.Add("public_ledger_commodities_nuts_peanuts", "Peanuts");

            d.Add("public_ledger_commodities_nuts_pecans", "Pecans");

            d.Add("public_ledger_commodities_nuts_pistachios", "Pistachios");

            d.Add("public_ledger_commodities_nuts_walnuts", "Walnuts");

            d.Add("public_ledger_commodities_nuts_other_nuts", "Dried Fruit & Nuts");



            return d;
        }

        public virtual Dictionary<string, string> GetMappingCommercial()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("foodnews_analysis_company", "Commercial");
            d.Add("foodnews_analysis_company_operations", "Operations");
            d.Add("foodnews_analysis_company_investment", "Investment");
            d.Add("foodnews_analysis_company_personnel", "People");
            d.Add("foodnews_analysis_company_results", "Financial Results");
            d.Add("foodnews_analysis_company_manda", "M & A");
            d.Add("foodnews_analysis_personnel", "People");
            d.Add("foodnews_analysis_company_trends", "Commercial");
            d.Add("public_ledger_analysis", "Commercial");
            d.Add("public_ledger_analysis_company", "Commercial");
            d.Add("public_ledger_analysis_company_operations", "Operations");
            d.Add("public_ledger_analysis_company_investments", "Investment");
            d.Add("public_ledger_analysis_company_manda", "M&A");
            d.Add("public_ledger_analysis_company_results", "Financial Results");
            d.Add("public_ledger_analysis_personnel", "People");
            d.Add("public_ledger_analysis_trading", "Commercial");
            d.Add("dairy_markets_analysis_company", "Commercial");



            return d;

        }

        public virtual Dictionary<string, string> GetMappingCommodityFactor()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();


            d.Add("dairy_markets_analysis_policy", "Policy");
            d.Add("dairy_markets_analysis_trade", "Imports");
            d.Add("dairy_markets_analysis_trade_Import", "Imports");
            d.Add("dairy_markets_analysis_trade_Export", "Exports");
            d.Add("dairy_markets_market_focus", "Price");

            d.Add("foodnews_market_focus", "Commodity Factors");
            d.Add("foodnews_analysis_trends", "Commodities Food Safety");
            d.Add("foodnews_analysis_freight", "Freight");
            d.Add("foodnews_analysis_weather", "Weather");

            d.Add("public_ledger_market_focus", "Price");


            d.Add("public_ledger_analysis_energy", "Energy");
            d.Add("public_ledger_analysis_freight", "Freight");
            d.Add("public_ledger_analysis_weather", "Weather");
            return d;

        }



        public List<string> GetRegion()
        {

            return new List<string>()
                                    {

                "Afghanistan",
                "Algeria",
                "Angola",
                "Bahrain",
                "Benin",
                "Botswana",
                "Burkina Faso",
                "Burundi",
                "Cabo Verde",
                "Cameroon",
                "Central African Republic",
                "Chad",
                "Comoros",
                "Congo",
                "Congo (Democratic Republic)",
                "Côte d'Ivoire",
                "Djibouti",
                "Egypt",
                "Equatorial Guinea",
                "Eritrea",
                "Ethiopia",
                "Gabon",
                "Gambia",
                "Ghana",
                "Guinea",
                "Guinea-Bissau",
                "Iran",
                "Iraq",
                "Israel",
                "Jordan",
                "Kenya",
                "Kuwait",
                "Lebanon",
                "Lesotho",
                "Liberia",
                "Libya",
                "Madagascar",
                "Malawi",
                "Mali",
                "Mauritania",
                "Mauritius",
                "Mayotte",
                "Morocco",
                "Mozambique",
                "Namibia",
                "Niger",
                "Nigeria",
                "Oman",
                "Palestine",
                "Qatar",
                "Réunion",
                "Rwanda",
                "Saint Helena, Ascension and Tristan da Cunha",
                "Sao Tome and Principe",
                "Saudi Arabia",
                "Senegal",
                "Seychelles",
                "Sierra Leone",
                "Somalia",
                "South Africa",
                "South Sudan",
                "Sudan",
                "Swaziland",
                "Syria",
                "Tanzania",
                "Togo",
                "Tunisia",
                "Turkey",
                "Uganda",
                "United Arab Emirates",
                "Western Sahara",
                "Yemen",
                "Zambia",
                "Zimbabwe",
                "American Samoa",
                "Australia",
                "Bangladesh",
                "Bhutan",
                "British Indian Ocean Territory",
                "Brunei Darussalam",
                "Cambodia",
                "China",
                "Christmas Island",
                "Cocos (Keeling) Islands",
                "Cook Islands",
                "Fiji",
                "French Polynesia",
                "Georgia",
                "Guam",
                "Heard Island and McDonald Islands",
                "Hong Kong",
                "India",
                "Indonesia",
                "Japan",
                "Kazakhstan",
                "Kiribati",
                "Kyrgyzstan",
                "Laos",
                "Macao",
                "Malaysia",
                "Maldives",
                "Marshall Islands",
                "Micronesia",
                "Mongolia",
                "Myanmar",
                "Nauru",
                "Nepal",
                "New Caledonia",
                "New Zealand",
                "Niue",
                "Norfolk Island",
                "North Korea",
                "Northern Mariana Islands",
                "Pakistan",
                "Palau",
                "Papua New Guinea",
                "Philippines",
                "Pitcairn",
                "Russian Federation",
                "Samoa",
                "Singapore",
                "Solomon Islands",
                "South Korea",
                "Sri Lanka",
                "Taiwan",
                "Tajikistan",
                "Thailand",
                "Timor-Leste",
                "Tokelau",
                "Tonga",
                "Turkmenistan",
                "Tuvalu",
                "Uzbekistan",
                "Vanuatu",
                "Vietnam",
                "Wallis and Futuna",
                "Åland Islands",
                "Albania",
                "Andorra",
                "Armenia",
                "Austria",
                "Azerbaijan",
                "Belarus",
                "Belgium",
                "Bosnia And Herzegovina",
                "Bouvet Island",
                "Bulgaria",
                "Croatia",
                "Cyprus",
                "Czech Republic",
                "Denmark",
                "Estonia",
                "Faroe Islands",
                "Finland",
                "France",
                "Germany",
                "Gibraltar",
                "Greece",
                "Guernsey",
                "Holy See",
                "Hungary",
                "Iceland",
                "Ireland",
                "Isle of Man",
                "Italy",
                "Jersey",
                "Latvia",
                "Liechtenstein",
                "Lithuania",
                "Luxembourg",
                "Macedonia",
                "Malta",
                "Moldova",
                "Monaco",
                "Montenegro",
                "Netherlands",
                "Norway",
                "Poland",
                "Portugal",
                "Romania",
                "San Marino",
                "Serbia",
                "Slovakia",
                "Slovenia",
                "Spain",
                "Svalbard and Jan Mayen",
                "Sweden",
                "Switzerland",
                "Ukraine",
                "United Kingdom",
                "Anguilla",
                "Antigua And Barbuda",
                "Aruba",
                "Bahamas",
                "Barbados",
                "Belize",
                "Bermuda",
                "Bonaire, Sint Eustatius and Saba",
                "Canada",
                "Cayman Islands",
                "Costa Rica",
                "Cuba",
                "Curaçao",
                "Dominica",
                "Dominican Republic",
                "El Salvador",
                "Greenland",
                "Grenada",
                "Guadeloupe",
                "Guatemala",
                "Haiti",
                "Honduras",
                "Jamaica",
                "Martinique",
                "Mexico",
                "Montserrat",
                "Nicaragua",
                "Panama",
                "Puerto Rico",
                "Saint Barthélemy",
                "Saint Kitts and Nevis",
                "Saint Lucia",
                "Saint Martin (French)",
                "Saint Pierre and Miquelon",
                "Saint Vincent and the Grenadines",
                "Sint Maarten (Dutch)",
                "Trinidad And Tobago",
                "Turks and Caicos Islands",
                "United States",
                "United States Minor Outlying Islands",
                "Virgin Islands (British)",
                "Virgin Islands (U.S.)",
                "Argentina",
                "Bolivia",
                "Brazil",
                "Chile",
                "Colombia",
                "Ecuador",
                "Falkland Islands (Malvinas)",
                "French Guiana",
                "Guyana",
                "Paraguay",
                "Peru",
                "Suriname",
                "Uruguay",
                "Venezuela",
                "Antarctica",
                "French Southern Territories",
                "South Georgia and the South Sandwich Islands",
                "EU",
                "EEA",
                "EFTA",
                "NAFTA",
                "Mercosur",
                "ASEAN",
                "Middle East & Africa",
                "Asia Pacific",
                "Europe",
                "North America",
                "South America",
                "Antarctica",
                "International US States",
                "Blocs",
                "Ivory Coast",
                "Congo (Democratic Republic)",
                "Democratic Republic of Congo",
                "UAE",
                "Burma",
                "PNG",
                "Russia",
                "Ceylon",
                "Viet Nam",
                "UK",
                "US",
                "USA",
                "NZ"

            };







        }

        public List<string> GetListFromXml(string publication, string type, string site)

        {

            List<string> keyList = new List<string>();

            //XElement doc = XElement.Load((WebConfigurationManager.AppSettings["xmlContentImport"]));
            XElement doc = XElement.Load(string.Format(@"{0}sitecore modules\Shell\Data Import\CMConfig\ContentMigrationMappingConfigs.xml", HttpRuntime.AppDomainAppPath));


            var elemValue = doc.Descendants(site).Descendants(type).Descendants();
            foreach (XElement elem in elemValue)
            {

                keyList.Add(elem.Attribute("name").Value.ToLower());

            }

            return keyList;




        }


        public List<string> GetCompanies()
        {
            return new List<string>()
            {
        "2 Sisters Food Group",
        "A G Barr",
        "AAK Miyoshi JP",
        "AarhusKarlshamn",
        "Abaxis",
        "Abengoa",
        "ABF",
        "ABP Food Group",
        "Adama Agricultural Solutions",
        "Adecoagro",
        "ADM",
        "Agrana",
        "AgriLabs",
        "Agriland",
        "Agriterra",
        "Agro-Kanesho",
        "Agropur",
        "Albaugh",
        "Alcogroup",
        "Aldebaran Commodities",
        "Alivira Animal Health",
        "Allanasons",
        "Alltech",
        "Almond Board of Australia",
        "Almond Board of California",
        "Almond Co Australia",
        "Alvean",
        "Amberwood Trading",
        "American Peanut Council",
        "American Vanguard",
        "Andes Quality",
        "Aratana Therapeutics",
        "Ardo",
        "Arla",
        "Aromatum",
        "Arysta LifeScience",
        "Atanor",
        "Aust & Hachmann (Canada)",
        "Australian Agricultural Co",
        "Australian Premium Dried Fruits",
        "Baronie",
        "Barry Callebaut",
        "BASF",
        "Bata Food",
        "Bayas del Sur",
        "Bayer Animal Health",
        "Bayer CropScience",
        "BayWa Group",
        "Bernard Matthews",
        "Berrico",
        "Berrifine",
        "Bimeda",
        "Biogénesis Bagó",
        "Biosev",
        "Birdsong Peanuts",
        "Blair Impex",
        "Blue Diamond Almonds",
        "Boecker",
        "Boehringer Ingelheim Animal Health",
        "Boesch Boden Spices",
        "Bonduelle",
        "Boustead",
        "Brasil Foods",
        "Britvic",
        "Bunge",
        "Campofrio",
        "Camposol",
        "Caplenco",
        "Cargill",
        "Carozzi",
        "Cassia Co-op",
        "Catz International",
        "CCAB Agro",
        "Ceva Santé Animale",
        "Chanelle",
        "CHB",
        "Chelmer Foods",
        "Cherkizovo",
        "Chilean Walnut Commission",
        "China Animal Husbandry Industry Company",
        "CHS Inc",
        "Ciruelas Chile",
        "Citrovita",
        "Coca-Cola",
        "Cofco",
        "ConAgra",
        "Consorzio Casalasco",
        "Cooxupe",
        "Copersucar",
        "Coruripe",
        "Cosan",
        "Costa Coffee",
        "Cott",
        "Cranswick",
        "Cutrale",
        "Daarnhouwer",
        "Dairy Farmers of America",
        "Danish Crown",
        "Danone",
        "Dantza",
        "David Berryman",
        "Dawn Meats",
        "Dean Foods",
        "Dechra Pharmaceuticals",
        "Diamond Green Diesel",
        "Dimes",
        "DMK",
        "Doehler",
        "Dow AgroSciences",
        "Dried Fruit Technical Services",
        "Dried Fruit Valley SPA",
        "Dryfrut",
        "Dunbia",
        "DuPont",
        "Eckes",
        "Eco Animal Health",
        "Ecom",
        "Ecuphar",
        "EDF Man",
        "Elanco",
        "Emperor Akbar Green Cardamoms/Samex Agency",
        "Eurovanille",
        "Excel Crop Care",
        "FAAS Trade & Investment",
        "Felda",
        "Ferrero",
        "Findus",
        "Flint Hills",
        "FMC",
        "Fonterra",
        "Freeworld Trading",
        "FrieslandCampina",
        "Fruits Du Sud",
        "Frutexsa",
        "Fuerst Day Lawson",
        "Gam Shmuel",
        "Gat",
        "Gavilon",
        "Georgalos Peanut World",
        "Gleadell",
        "Glencore",
        "Global Trading",
        "Golden Agri-Resources",
        "Goodvalley",
        "Gowan",
        "GrainCorp",
        "Granol",
        "Granor Passi",
        "Green & Gold Macadamias",
        "Green Plains",
        "Groupe Avril/Saipol",
        "Grover Sons",
        "Haisheng",
        "Haribo",
        "Hassas",
        "Heinz",
        "Henry Schein",
        "Hershey's",
        "Heska",
        "Hilton Food Group",
        "Hipra",
        "Hokko Chemical Industry",
        "Huapont-Nutrichem",
        "Huiyuan",
        "Huvepharma",
        "ICGrain",
        "IDEXX Laboratories",
        "IDT Biologika",
        "Iharabras",
        "Illovo Sugar",
        "illycaffe",
        "Ingredion",
        "Innocent",
        "InVivo",
        "Isagro",
        "Ishihara Sangyo Kaisha",
        "ITI Tropical",
        "J.M. Smucker",
        "JAB Holding",
        "Jacobs Douwe Egberts",
        "Jaguar Animal Health",
        "Jantzen & Deeke",
        "JBS",
        "JHB International",
        "Jiangsu Yangnong Chemical",
        "Jinyu Group",
        "Kagome",
        "Kamani Oil Industries",
        "Kemin Industries",
        "Kenkko Corporation",
        "KEPAK",
        "Kernel",
        "Keurig Green Mountain",
        "Kindred Biosciences",
        "KMG Robust",
        "Kolmar",
        "Kraft",
        "Kraft Heinz",
        "Krispy Kreme",
        "Krka",
        "Kumiai Chemical",
        "Kyoritsu Seiyaku",
        "La Doria",
        "Lactalis",
        "LaDoria",
        "Lamb Weston",
        "Larchfield",
        "Lianyungang Yuda Food Company",
        "Lindt",
        "Louis Dreyfus",
        "Maple Leaf Foods",
        "Marfrig",
        "Mariani",
        "Marrone Bio Innovations",
        "Marubeni Corp",
        "Massimo Zanetti Beverage Group",
        "McCain",
        "Meiji",
        "Mengniu",
        "Merck",
        "Mercuria",
        "Merial",
        "Meridian Nut Growers",
        "MHP",
        "Minerva ",
        "Miratorg",
        "Mitr Phol",
        "Mitsubishi",
        "Mitsui Chemicals Agro",
        "Miyoshi Oils & Fats Co",
        "Moageira e Agrícola",
        "Mondelez",
        "Monsanto",
        "Morning Star",
        "Moy Park",
        "Muller",
        "Musim Mas",
        "Mutti",
        "MWI Veterinary Supply",
        "Nanjing Redsun",
        "Natsupply",
        "Neogen",
        "Neste Oil",
        "Nestlé",
        "Neumann Kaffee Gruppe",
        "Nexvet Biopharma",
        "NH Foods Australia",
        "Nidera",
        "Nihon Nohyaku",
        "Nippon Soda",
        "Nippon Zenyaku Kogyo",
        "Nissan Chemical",
        "Noble",
        "Noble Agri",
        "Norbrook Laboratories",
        "North Andre",
        "Nufarm",
        "Nutland",
        "Nutreco",
        "Ocean Spray",
        "Odebrecht",
        "Olam",
        "Olam Americas",
        "Old Orchard",
        "Oleoplan",
        "Orion Animal Health",
        "Orix",
        "OSI Food Solutions",
        "Ouro Fino Saúde Animale",
        "Pago",
        "Palm Nuts & More",
        "Paramount Farms",
        "Parnell Pharmaceuticals",
        "Patterson Companies",
        "PBA Brokerage",
        "Pepperdesk",
        "Pepsi-Cola",
        "Perrigo",
        "Petrobras",
        "Pfeifer & Langen",
        "Phibro Animal Health",
        "Pistachio Investment",
        "Plant Health Care",
        "Platform Specialty Products",
        "POET",
        "Prevtec Microbia",
        "Primex International",
        "Princes",
        "Prunesco",
        "QFN Trading & Agency",
        "Quicornac",
        "Rainbow Chemical",
        "Raizen",
        "Rallis India",
        "Red River",
        "Refresco",
        "Regency Spices",
        "Renewable Energy Group",
        "Rusagro",
        "Samsons Trading",
        "Sanderson Farms",
        "Sanonda/Hubei Sanonda",
        "Santa Terezinha",
        "Sao Martinho",
        "Saputo",
        "Saradipour",
        "Savencia",
        "Schweppes",
        "SDS Biotech",
        "Shell",
        "Shenzhen Noposion Agrochemicals",
        "Shree Renuka",
        "Sigma Alimentos",
        "Sime Darby",
        "Simplot",
        "Sinochem International",
        "Sipcam-Oxon",
        "Skypeople",
        "Smithfield",
        "Sodiaal",
        "Sofruco",
        "Sonderjansen",
        "Spicexim",
        "Starbucks",
        "Strauss Group",
        "Stute",
        "Sucden",
        "Südzucker",
        "Sugal",
        "Sumitomo Chemical",
        "Sun Valley Raisins",
        "Sun-Maid Growers ",
        "SunProd",
        "Sunripe Europe",
        "Sunrose",
        "Sunsweet Ingredients",
        "Surfrut",
        "Syngenta",
        "Tata",
        "Tate & Lyle",
        "Tchibo",
        "Tereos Internacional",
        "Teys",
        "The Golden Peanut Company",
        "The Green Valley Pecan Company",
        "The Pistachio Company",
        "Tillbrook Products",
        "Tipco",
        "TM Duche",
        "Tonnies ",
        "Touton",
        "Treehouse California Almonds",
        "Trigon Agri",
        "Tripper",
        "Tropicana",
        "Tyson Foods",
        "Unilever",
        "UPL",
        "Uren",
        "Valero Energy",
        "Van der Does Spice Brokers",
        "Vanilla Corporation of America",
        "Verbio",
        "Vetoquinol",
        "Vion",
        "Virbac",
        "Virdhara International",
        "Viterra",
        "Vitol",
        "VKL Spices",
        "Voicevale",
        "Volcafe",
        "Welch's",
        "Wesergold",
        "Wessex Grain",
        "WH Group",
        "Wilmar International",
        "Yili",
        "Young Pecan",
        "Yurun Group",
        "Zhejiang Jinfanda Biochemical",
        "Zhejiang Wynca Chemical",
        "Zhongpin",
        "Zoetis",
        "Zydus Animal Health"


            };

        }

        public List<string> GetAgency()
        {
            return new List<string>()
            {
            "AMS",
            "Anses",
            "APHIS",
            "APVMA",
            "Argentina Senasa",
            "ARS",
            "ASA",
            "BfR",
            "Brazil Anvisa",
            "Brazil Embrapa",
            "Brazil Ibama",
            "Brazil Mapa",
            "CBO",
            "CFIA",
            "CFSAN",
            "CFTC",
            "Codex Alimentarius",
            "ComAgri",
            "Coreper",
            "Council of Ministers",
            "CRD",
            "CRS",
            "CVM",
            "DEFRA",
            "DOE",
            "DOJ",
            "EFSA",
            "EIA",
            "EMA",
            "EPA",
            "ERS",
            "European Commission",
            "European Council",
            "European Court of Auditors",
            "European Court of Justice",
            "European Parliament",
            "FAO",
            "FAS",
            "FDA",
            "Federal Reserve",
            "FNS",
            "FSIS",
            "FTC",
            "GAO",
            "House",
            "IEA",
            "IMF",
            "IRS",
            "JMAFF",
            "NASS",
            "NMFS",
            "NZ EPA",
            "OBPA",
            "OCE",
            "OECD",
            "OIE",
            "OMB",
            "OPEC",
            "OPP",
            "OTAQ",
            "PMRA",
            "SCA",
            "SEC",
            "Senate",
            "Supreme Court",
            "UK FSA",
            "UN",
            "US FSA",
            "USDA",
            "USITC",
            "USTR",
            "WHO",
            "WOAB",
            "World Bank",
            "WTO" };

        }





















        #endregion Methods
    }

    public static class AuthorHelper
    {
        public static string Authors(string value)
        {
            return value
                    .Replace("kelly@informa.com", ",")
                    .Replace("brizmohun@informa.com", ",")
                    .Replace("cathy.kelly@informa.com", ",")
                    .Replace("neena.brizmohun@informa.com", ",")
                    .Replace("(analysis)", ",")
                    .Replace("(commentary)", ",")
                    .Replace("(reporting);", ",")
                    .Replace("ShimmingsPial", "Shimmings,Pial")
                    .Replace("Ch ristopher", "Christopher")
                    .Replace("Scrip Intelligence", ",")
                    .Replace("Datamonitor", ",")
                    .Replace("Dr", "")
                    .Replace("Professor", "")
                    .Replace("Jr", "")
                    .Replace("is the Author", ",")
                    .Replace("&middot;", " ")
                    .Replace("SimonVarcoe", "Simon Varcoe")
                    .Replace("Head of Life Sciences at Stevens & Bolton LLP", "")
                    .Replace("Head of Life Sciences at Stevens Bolton LLP", "")
                    .Replace("Head of Life Sciences at Stevens", "")
                    .Replace("IP Litigation Partner", "")
                    .Replace("with additional reporting from", ",")
                    .Replace("with reporting from India by", ",")
                    .Replace("reporting and", ",")
                    .Replace("reporting from India by", ",")
                    .Replace("with contributions from", ",")
                    .Replace("with input from", ",")
                    .Replace("analysis", ",")
                    .Replace("commentary", ",")
                    .Replace("reporting", ",")
                    .Replace(" and ", ",")
                    .Replace("amp;", ",")
                    .Replace("amp", ",")
                    .Replace("LLP", "")
                    .Replace(".", " ")
                    .Replace(";", ",")
                    .Replace("&", ",")
                    .Replace("  ", " ");
        }
    }

    public class EscenicAutonomyAuthorDataMap : EscenicAutonomyArticleDataMap
    {
        #region Constructor

        public EscenicAutonomyAuthorDataMap(Database db, string ConnectionString, Item importItem, ILogger l)
                : base(db, ConnectionString, importItem, l)
        {

        }

        #endregion Constructor

        #region Override Methods

        public override IEnumerable<object> GetImportData(string site, string channel)
        {
            if (!Directory.Exists(this.Query))
            {
                Logger.Log("N/A", string.Format("the folder: '{0}' could not be found. Try moving the folder under the webroot.", this.Query), ProcessStatus.ImportDefinitionError);
                return Enumerable.Empty<object>();
            }

            List<Dictionary<string, string>> l = new List<Dictionary<string, string>>();

            string[] files = Directory.GetFiles(this.Query);
            foreach (string f in files)
            {
                Encoding et = Encoding.GetEncoding("utf-8");
                byte[] bytes = GetFileBytes(f);
                string data = et.GetString(bytes);
                List<string> AuthorList = new List<string>();


                XmlDocument d = new XmlDocument();
                try
                {
                    d.LoadXml(data);
                }
                catch (Exception ex)
                {
                    Logger.Log("N/A", string.Format("Xml file data was malformed: {0}", ex.Message), ProcessStatus.Error, "File", f);
                    continue;
                }

                //XmlNode nameNode = d.SelectSingleNode("//STORYAUTHORNAME");
                //string name = (nameNode != null) ? nameNode.InnerText : string.Empty;
                //string[] nameArr = AuthorHelper.Authors(name).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                //XmlNode emailNode = d.SelectSingleNode("//STORYAUTHOREMAIL");
                //    string email = (emailNode != null) ? emailNode.InnerText : string.Empty;
                //    string[] emailArr = email.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                //   for (int i = 0; i < nameArr.Length; i++)
                // {
                //   string n = nameArr[i];

                // List<string> nameParts = n.Trim().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                //  if (nameParts.Count < 2)
                //  {
                //      Logger.Log("N/A", string.Format("Author name was too short so it was ignored: {0}", n), ProcessStatus.FieldError, "STORYAUTHORNAME", name);
                //      continue;
                //  }
                // Dictionary<string, string> ao = new Dictionary<string, string>();
                //   ao.Add("STORYAUTHORNAME", n.Trim());
                //   ao.Add("FIRSTNAME", nameParts[0].Trim());
                //   ao.Add("LASTNAME", string.Join(" ", nameParts.Skip(1).ToArray()).Trim());
                //  string curEmail = (i < emailArr.Length) ? emailArr[i] : string.Empty;
                //  ao.Add("EMAIL", curEmail);
                // if (!string.IsNullOrEmpty(curEmail))
                //    Logger.Log("N/A", string.Format("Matching {0} with {1}", n.Trim(), curEmail));

                //   l.Add(ao);
                //   }

                var result = d.SelectNodes($"//STORYAUTHORNAME");

                AuthorList = result.Cast<XmlNode>().Select(node => node.InnerText).ToList();

                foreach (string authorName in AuthorList)
                {
                    string name = (authorName != null) ? authorName : string.Empty;


                    string[] nameArr = AuthorHelper.Authors(authorName).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < nameArr.Length; i++)
                    {
                        string n = nameArr[i];

                        List<string> nameParts = n.Trim().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        if (nameParts.Count < 2)
                        {
                            Logger.Log("N/A", string.Format("Author name was too short so it was ignored: {0}", n), ProcessStatus.FieldError, "STORYAUTHORNAME", name);
                            continue;
                        }
                        Dictionary<string, string> ao = new Dictionary<string, string>();
                        ao.Add("STORYAUTHORNAME", n.Trim());
                        ao.Add("FIRSTNAME", nameParts[0].Trim());
                        ao.Add("LASTNAME", string.Join(" ", nameParts.Skip(1).ToArray()).Trim());
                        //   string curEmail = (i < emailArr.Length) ? emailArr[i] : string.Empty;
                        string curEmail = "";
                        ao.Add("EMAIL", curEmail);
                        if (!string.IsNullOrEmpty(curEmail))
                            Logger.Log("N/A", string.Format("Matching {0} with {1}", n.Trim(), curEmail));

                        l.Add(ao);
                    }
                }

               // Dictionary<string, string> ao = new Dictionary<string, string>();
               // l.Add(ao);
            }

            return l;
        }

        #endregion Override Methods
    }

}

