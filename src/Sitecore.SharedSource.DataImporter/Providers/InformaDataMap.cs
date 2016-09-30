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

namespace Sitecore.SharedSource.DataImporter.Providers
{
    public class EscenicAutonomyArticleDataMap : BaseDataMap
    {
        #region Properties

        public string PublicationPrefix { get; set; }

        #endregion Properties

        #region Constructor

        public EscenicAutonomyArticleDataMap(Database db, string ConnectionString, Item importItem, ILogger l)
                : base(db, ConnectionString, importItem, l)
        {
            PublicationPrefix = ImportItem.GetItemField("Publication Prefix", Logger);
        }

        #endregion Constructor

        #region Override Methods

        public override IEnumerable<object> GetImportData()
        {
            if (!Directory.Exists(this.Query))
            {
                Logger.Log("N/A", string.Format("the folder '{0}' could not be found. Try moving the folder under the webroot.", this.Query), ProcessStatus.ImportDefinitionError);
                return Enumerable.Empty<object>();
            }

            List<Dictionary<string, string>> l = new List<Dictionary<string, string>>();

            long artNumber = GetNextArticleNumber();

            string[] files = Directory.GetFiles(this.Query);
            foreach (string f in files)
            {
                Dictionary<string, string> ao = new Dictionary<string, string>();
                XmlDocument d = GetXmlDocument(f);
                if (d == null)
                    continue;

                ////escenic field values
                //string authorNode = "STORYAUTHORNAME";
                //ao.Add(authorNode, AuthorHelper.Authors(GetXMLData(d, authorNode)));
                //string bodyNode = "STORYBODY";
                //ao.Add(bodyNode, GetXMLData(d, bodyNode));
                //string titleNode = "STORYTITLE";
                //string cleanTitleHtml = CleanTitleHtml(GetXMLData(d, titleNode));
                //ao.Add(titleNode, cleanTitleHtml);
                //ao.Add("FILENAME", cleanTitleHtml);
                //ao.Add("META TITLE OVERRIDE", cleanTitleHtml);
                //ao.Add("ARTICLEID", curFileName.Replace(".xml", ""));

                //,EMAILTITLE,TEASERTITLE,TOCTITLE,
                //,STORYSTANDFIRST,ISSECTIONLEAD
                //STORYAUTHORNAME,BODY,TAXONOMY,STORYTITLE,ID


                //generated field
                string curFileName = new FileInfo(f).Name;
                ao["ARTICLE NUMBER"] = $"{PublicationPrefix}{artNumber:D6}";


                string authorNode = "STORYAUTHORNAME";
                ao.Add(authorNode, AuthorHelper.Authors(GetXMLData(d, authorNode)));


                string bodyNode = "BODY";
                string bodyTitleHtml = GetXMLData(d, bodyNode);
              

                string titleNode = "STORYTITLE";
                string cleanTitleHtml = CleanTitleHtml(GetXMLData(d, titleNode));
            

                string taxonomyTitleHtml = GetXMLData(d, "TAXONOMY");
                ao.Add("PUBLICATIONNAME", taxonomyTitleHtml);

                string summaryTitleHtml = GetXMLData(d,"SUMMARY");

                if (string.IsNullOrEmpty(summaryTitleHtml))
                {
                    summaryTitleHtml = bodyTitleHtml;
                }

                ao.Add("SUMMARY", summaryTitleHtml);
                ao.Add("STORYTITLE", cleanTitleHtml);
                ao.Add("FILENAME", cleanTitleHtml);
                ao.Add("META TITLE OVERRIDE", cleanTitleHtml);
                ao.Add("ARTICLEID", curFileName.Replace(".xml", ""));
                //ao.Add("ARTICLEID", GetXMLData(d, "ID"));

                string imageTitleHtml = GetXMLData(d, "LEADIMAGE");

                if (!string.IsNullOrEmpty(imageTitleHtml))
                {
                    bodyTitleHtml= bodyTitleHtml + imageTitleHtml;
                }

                string binaryTitleHtml = GetXMLData(d, "BINARY");
                if (!string.IsNullOrEmpty(binaryTitleHtml))
                {
                    bodyTitleHtml= bodyTitleHtml + binaryTitleHtml;
                }

                ao.Add("STORYBODY", bodyTitleHtml);

                l.Add(ao);
                artNumber++;

                //autonomy fields
                string autFile = $@"{this.Query}\..\Autonomy\{curFileName}";

                 List<string> autNodes = new List<string>() { "CATEGORY", "COMPANY", "STORYUPDATE", "SECTION", "COUNTRY", "KEYWORD", "THERAPY_SECTOR", "TREATABLE_CONDITION" };
                  //if no autonomy file then fill fields with empty
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

            return l;
        }

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

            string num = articles.First().Replace(PublicationPrefix, "");
            int n = int.Parse(num);
            return n + 1;
        }

        public string GetXMLData(XmlDocument xd, string nodeName)
        {
            if (!nodeName.Equals("THERAPY_SECTOR")
                    && !nodeName.Equals("TREATABLE_CONDITION")
                    && !nodeName.Equals("COMPANY")
                    && !nodeName.Equals("COUNTRY"))
            {

                XmlNode xn = xd.SelectSingleNode($"//{nodeName}");

                if (nodeName.Equals("TAXONOMY"))
                {

                    Dictionary<string, string> Publication = new Dictionary<string, string>();
                    Publication.Add("agra_europe", "Agra Europe");
                    Publication.Add("agrow", "Agrow");
                    Publication.Add("animalpharm", "Animal Pharm");
                    Publication.Add("dairy_markets", "Dairy Markets");
                    Publication.Add("food_chemical_news", "Food Chemical News");
                    Publication.Add("foodnews", "Foodnews");
                    Publication.Add("public_ledger", "Public Ledger");
                    Publication.Add("international_coffee_report", "International Coffee Report");
                    Publication.Add("international_sugar_and_sweetener_report", "International Sugar & Sweetener Report");
                    Publication.Add("renewable_chemicals", "Renewable Chemicals");
                    Publication.Add("world_biodiesel_price_report", "World Biodiesel Price Report");
                    Publication.Add("world_ethanol_and_biofuels_report", "World Ethanol & Biofuels Report");
                    Publication.Add("world_ethanol_price_report", "World Ethanol Price Report");
                    Publication.Add("world_molasses_and_feed_ingredients_report", "World Molasses & Feed Ingredients Report");


                    foreach (XmlNode node in xn)
                    {
                        if (node.Attributes["unique-name"] != null)
                        {
                            if (Publication.ContainsKey(node.Attributes["unique-name"].Value))
                            {
                                return Publication[node.Attributes["unique-name"].Value];
                            }

                        }
                    }

                    return string.Empty;
                }
                else if (nodeName.Equals("BINARY"))
                {
                    return (xn != null) ? "<img src=" + xn.InnerText + " alt=" + xn.Attributes["leadtext"].Value + " width =" + " height=" + ">" : string.Empty;

                }
                else if (nodeName.Equals("LEADIMAGE"))
                {
                    return (xn != null) ? "<img src=" + xn.InnerText + " alt=" + xn.Attributes["leadtext"].Value + " width =" + " height=" + ">" : string.Empty;
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

        public override IEnumerable<object> GetImportData()
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

                XmlNode nameNode = d.SelectSingleNode("//STORYAUTHORNAME");
                string name = (nameNode != null) ? nameNode.InnerText : string.Empty;
                string[] nameArr = AuthorHelper.Authors(name).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                XmlNode emailNode = d.SelectSingleNode("//STORYAUTHOREMAIL");
                string email = (emailNode != null) ? emailNode.InnerText : string.Empty;
                string[] emailArr = email.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

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
                    string curEmail = (i < emailArr.Length) ? emailArr[i] : string.Empty;
                    ao.Add("EMAIL", curEmail);
                    if (!string.IsNullOrEmpty(curEmail))
                        Logger.Log("N/A", string.Format("Matching {0} with {1}", n.Trim(), curEmail));

                    l.Add(ao);
                }
            }

            return l;
        }

        #endregion Override Methods
    }
}
