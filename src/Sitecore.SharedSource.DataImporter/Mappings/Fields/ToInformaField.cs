using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using Sitecore.SharedSource.DataImporter.Providers;
using Sitecore.SharedSource.DataImporter.Utility;
using HtmlDocument = Sitecore.WordOCX.HtmlDocument.HtmlDocument;
using Sitecore.SecurityModel;

namespace Sitecore.SharedSource.DataImporter.Mappings.Fields
{
    #region Scrip

    public class ToArticleNumberText : ToText
    {
        #region Constructor

        public ToArticleNumberText(Item i) : base(i) { }

        #endregion Constructor

        #region Methods

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
        {
            // connect to the company database and get the ID to store
            if (importValue.Equals(string.Empty))
                return;


            //store the imported value as is
            Field f = newItem.Fields[NewItemField];
            if (f == null)
                return;

            //don't overwrite the number if the item was already created. this occurs on second runs
            if (string.IsNullOrEmpty(f.Value))
                f.Value = importValue;
        }

        #endregion Methods
    }

    public class ToInformaSummaryField : ToText
    {
        #region Constructor

        public ToInformaSummaryField(Item i) : base(i) { }

        #endregion Constructor

        #region Methods

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
        {
            if (string.IsNullOrEmpty(importValue))
                return;

            //strip out the tags, attributes and remap images
            string newImportValue = CleanHtml(importValue);

            //store the imported value as is
            Field f = newItem.Fields[NewItemField];
            if (f != null)
                f.Value = newImportValue;
        }

        public string CleanHtml(string html)
        {
            List<string> unwantedTags = new List<string>() { "embed", "iframe", "script", "table", "font", "img", "preform" };

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
                else if (node.HasAttributes)
                { // if it's not being removed
                    node.Attributes.RemoveAll();
                }
            }

            return document.DocumentNode.FirstChild.OuterHtml;
        }

        #endregion Methods
    }

    public class ToInformaBodyField : ToText
    {
        #region Constructor

        public ToInformaBodyField(Item i) : base(i) { }

        #endregion Constructor

        #region Methods

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
        {
            //replace <h1> with <h2>
            importValue = importValue.Replace("h1", "h2");

            //strip out the tags, attributes and remap images
            List<string> removeTags = new List<string>() { "font", "preform" };
            List<string> removeAttrs = new List<string>() { "style", "align", "height", "width" };
            DateTime dt = new DateTime(1800, 1, 1);
            DateField df = newItem.Fields["Created Date"];
            if (df != null && !string.IsNullOrEmpty(df.Value))
                dt = df.DateTime;
            string newImportValue = CleanHtml(map, newItem.Paths.FullPath, dt, importValue, removeTags, removeAttrs, id);

            //store the imported value as is
            Field f = newItem.Fields[NewItemField];
            if (f != null)
            {
                f.Value = newImportValue;
            }
        }

        public string CleanHtml(IDataMap map, string articlePath, DateTime articleDate, string html, List<string> unwantedTags, List<string> unwantedAttrs, string ArticleId)
        {
            if (String.IsNullOrEmpty(html))
                return html;

            var document = new HtmlDocument();
            document.LoadHtml(html);
            string imageId = string.Empty;

            HtmlNodeCollection tryGetNodes = document.DocumentNode.SelectNodes("./*|./text()");
            HtmlNodeCollection imgNodes = document.DocumentNode.SelectNodes("image");
            HtmlNodeCollection tableauNodes = document.DocumentNode.SelectNodes("strong");
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

                if (i == 0 && (nodeName.Equals("table") || nodeName.Equals("image"))) //log table or img as first paragraph
                    map.Logger.Log(articlePath, $"first element was a(n) '{nodeName}'", ProcessStatus.Warning, NewItemField, html);

                if (childNodes != null)
                {
                    foreach (var child in childNodes)
                    {

                        nodes.Enqueue(child);

                    }
                }
                if ((imgNodes != null || tableauNodes != null) && node.Name.Equals("relation"))
                {
                    if (imgNodes != null)
                    {
                        foreach (HtmlNode imgnode in imgNodes)
                        {
                            if (imgnode.Attributes["sourceid"].Value == node.Attributes["sourceid"].Value)
                            {
                                parentNode.InsertBefore(imgnode, node);

                                parentNode.RemoveChild(node);

                                document.DocumentNode.RemoveChild(imgnode);
                                // imageId = imageId + "|" + imgnode.Attributes["sourceid"].Value;
                            }
                        }
                    }

                    if (tableauNodes != null)
                    {
                        foreach (HtmlNode tableaunode in tableauNodes)
                        {
                            if (tableaunode.Attributes["id"].Value == node.Attributes["sourceid"].Value)
                            {
                                //tableaunode.Attributes["id"].Remove();
                                parentNode.InsertBefore(tableaunode, node);

                                parentNode.RemoveChild(node);

                                document.DocumentNode.RemoveChild(tableaunode);
                                //imageId = imageId + "|" + imgnode.Attributes["sourceid"].Value;
                            }
                        }
                    }


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
                else if (node.HasAttributes)
                { // if it's not being removed
                    if (nodeName.Equals("table"))
                    {
                        node.Attributes.RemoveAll();
                    }
                    else if (!nodeName.Equals("iframe") && !nodeName.Equals("image"))
                    { //skip iframe and imgs
                        foreach (string s in unwantedAttrs)
                        {
                            // remove unwanted attributes
                            node.Attributes.Remove(s);
                        }
                    }

                    if (nodeName.Equals("iframe") || nodeName.Equals("embed") || nodeName.Equals("form") || nodeName.Equals("script")) // warn about iframes, embed, form or script in body
                        map.Logger.Log(articlePath, $"content contains a(n) {nodeName}'", ProcessStatus.Warning, NewItemField, html);

                    //replace images
                    if (nodeName.Equals("image"))
                    {

                        // see if it exists
                        string imgWidthStr = node.Attributes["width"]?.Value ?? string.Empty;
                        string imgSrc = node.Attributes["src"]?.Value ?? string.Empty;
                        AricleImportImageHandler objSitecoreMediaHandler = new AricleImportImageHandler();
                        MediaItem newImg = objSitecoreMediaHandler.HandleImage(map, articlePath, articleDate, imgSrc, ArticleId);
                        if (newImg != null)
                        {
                            string newSrc = $"-/media/{newImg.ID.ToShortID().ToString()}.ashx";

                            // replace the node with sitecore tag
                            node.SetAttributeValue("src", newSrc);

                            //If no width was found, use the sitecore width field on the med lib image 
                            if (string.IsNullOrEmpty(imgWidthStr))
                            {
                                Field f = newImg.InnerItem.Fields["Width"];
                                imgWidthStr = (f != null) ? f.Value : string.Empty;
                            }
                        }

                        //if a 'width' attribute is available, that attribute will be read.
                        int imgWidth = 0;
                        if (!int.TryParse(imgWidthStr, out imgWidth))
                            imgWidth = 0;

                        //If the width read in either case is greater than 787, a warning will be logged.
                        if (imgWidth > 787)
                            map.Logger.Log(articlePath, $"image too wide: '{imgWidth}'", ProcessStatus.Warning, NewItemField, node.OuterHtml);
                    }
                }

                i++;
            }

            // HtmlNodeCollection tryGetallNodes = document.DocumentNode.ChildNodes;
            //// HtmlNodeCollection imgNodes = document.DocumentNode.SelectNodes("image");
            // if (tryGetNodes == null || !tryGetNodes.Any())
            //     return html;

            // var nodeslatest = new Queue<HtmlNode>(tryGetallNodes);

            // while (nodeslatest.Count > 0)
            // {
            //     var imgnode = nodeslatest.Dequeue();
            //     var parentNode = imgnode.ParentNode;
            //     // var nodeName = node.Name;


            //     //replace images
            //     if (imgnode.Name.Equals("image"))
            //     {
            //        if((imageId.Contains(imgnode.Attributes["sourceid"].Value)))
            //             {

            //             parentNode.RemoveChild(imgnode);

            //             }
            //     }


            // }


            return document.DocumentNode.InnerHtml;
        }

        #endregion Methods
    }

    public class ToInformaLeadImageField : ToText
    {
        #region Constructor

        public ToInformaLeadImageField(Item i) : base(i) { }

        #endregion Constructor

        #region Methods

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
        {

            DateTime dt = new DateTime(1800, 1, 1);
            DateField df = newItem.Fields["Created Date"];
            if (df != null && !string.IsNullOrEmpty(df.Value))
                dt = df.DateTime;
            string newImportValue = CleanHtml(map, newItem.Paths.FullPath, dt, importValue, id);

            //store the imported value as is
            Field f = newItem.Fields[NewItemField];
            if (f != null)
            {
                f.Value = newImportValue;
                f.Value = f.Value.Replace("img", "image");
                f.Value = f.Value.Replace(">", "/>");
            }
        }

        public string CleanHtml(IDataMap map, string articlePath, DateTime articleDate, string html, string ArticleId)
        {
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

                if (nodeName.Equals("img"))
                {

                    // see if it exists
                    string imgWidthStr = node.Attributes["width"]?.Value ?? string.Empty;
                    string imgSrc = node.Attributes["src"]?.Value ?? string.Empty;
                    AricleImportImageHandler objSitecoreMediaHandler = new AricleImportImageHandler();
                    MediaItem newImg = objSitecoreMediaHandler.HandleImage(map, articlePath, articleDate, imgSrc, ArticleId);
                    if (newImg != null)
                    {
                        // string newSrc = $"-/media/{newImg.ID.ToShortID().ToString()}.ashx";
                        // replace the node with sitecore tag
                        //node.SetAttributeValue("src", "");
                        string newMediaId = $"{newImg.ID.ToString()}";
                        node.SetAttributeValue("mediaid", newMediaId);
                        XMLDataLogger.WriteLog("NewImageMediaId" + newMediaId, "ImageLog");
                        //If no width was found, use the sitecore width field on the med lib image 
                        if (string.IsNullOrEmpty(imgWidthStr))
                        {
                            Field f = newImg.InnerItem.Fields["Width"];
                            imgWidthStr = (f != null) ? f.Value : string.Empty;
                        }
                    }

                    //if a 'width' attribute is available, that attribute will be read.
                    int imgWidth = 0;
                    if (!int.TryParse(imgWidthStr, out imgWidth))
                        imgWidth = 0;

                    //If the width read in either case is greater than 787, a warning will be logged.
                    if (imgWidth > 787)
                        map.Logger.Log(articlePath, $"image too wide: '{imgWidth}'", ProcessStatus.Warning, NewItemField, node.OuterHtml);
                }
            }

            i++;

            XMLDataLogger.WriteLog("Inner HTML" + document.DocumentNode.InnerHtml, "ImageLog");
            return document.DocumentNode.InnerHtml;
        }

        #endregion
    }

    public class AricleImportImageHandler
    {

        public MediaItem HandleImage(IDataMap map, string articlePath, DateTime dt, string url, string ArticleId)
        {
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
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                XMLDataLogger.WriteLog(ArticleId, "InvalidImageLog");
                map.Logger.Log("ArticleId: " + ArticleId + articlePath, "malformed image URL", ProcessStatus.FieldError, url);
                return null;
            }

            //get file info
            List<string> uri = url.Split(new string[] { "?" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> parts = uri[0].Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            string filePath = parts[parts.Count - 1].Trim();
            string[] fileParts = filePath.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            string fileName = (fileParts.Length > 0) ? StringUtility.GetValidItemName(fileParts[0], map.ItemNameMaxLength) : string.Empty;

            //date info
            string newFilePath = (dt.Year != 1800) ? $"{dt.ToString("yyyy/MMMM")}/{fileName}" : fileName;

            // see if it exists in med lib
            Item rootItem = map.ToDB.GetItem(Sitecore.Data.ID.Parse("{CDC0468D-CFAE-4E65-9CE7-BF47848A8A81}"));
            IEnumerable<Item> matches = GetMediaItems(map)
                .Where(a => a.Paths.FullPath.EndsWith(fileName));

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
                            return match;
                        }
                    }

                }

                map.Logger.Log(articlePath, $"Sitecore image matched {matches.Count()} images", ProcessStatus.FieldError, filePath);
            }

            MediaItem m = ImportImage(url, filePath, $"{rootItem.Paths.FullPath}/{newFilePath}");
            if (m == null)
                map.Logger.Log(articlePath, "Image not found", ProcessStatus.FieldError, url);

            return m;
        }

        public string UpperCaseUrlEncode(string s)
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
            return new string(temp);
        }

        public IEnumerable<Item> GetMediaItems(IDataMap map)
        {
            string cacheKey = "Images";
            IEnumerable<Item> o = Context.Items[cacheKey] as IEnumerable<Item>;
            if (o != null)
                return o;

            Item rootItem = map.ToDB.GetItem(Sitecore.Data.ID.Parse("{CDC0468D-CFAE-4E65-9CE7-BF47848A8A81}"));
            IEnumerable<Item> images = rootItem.Axes.GetDescendants();
            Context.Items.Add(cacheKey, images);

            return images;
        }

        public MediaItem ImportImage(string url, string fileName, string newPath)
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
                    MediaItem mediaItem = creator.CreateFromStream(stream2, fileName, options);
                    response.Close();
                    XMLDataLogger.WriteLog("Image create in media library:" + mediaItem.Path, "ImageLog");
                    return mediaItem;
                }
            }
            catch (WebException ex)
            {
                return null;
            }
        }

    }

    public class ToInformaCompanyField : ToText
    {
        #region Constructor

        public ToInformaCompanyField(Item i) : base(i) { }

        #endregion Constructor

        #region Methods

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
        {
            // connect to the company database and get the ID to store
            if (importValue.Equals(string.Empty))
                return;

            //try exact search first
            Dictionary<string, string> fileCompanies = GetFileCompanies();
            Dictionary<string, string> dbCompanies = GetDBCompanies(map, newItem.Paths.FullPath);

            IEnumerable<string> importCompanies = importValue.Split(new string[] { "," },
                StringSplitOptions.RemoveEmptyEntries).Distinct();

            List<string> cIDs = new List<string>();

            foreach (string cName in importCompanies)
            {
                string casedValue = cName.ToLower();
                KeyValuePair<string, string> pair;
                if (fileCompanies.ContainsKey(casedValue))
                {
                    pair = fileCompanies.First(a => a.Key.Equals(casedValue));
                }
                else if (dbCompanies.ContainsKey(casedValue))
                {
                    pair = dbCompanies.First(a => a.Key.Equals(casedValue));
                }
                else
                {
                    var ids = dbCompanies.Where(a => a.Key.Contains(casedValue));
                    if (ids == null || !ids.Any())
                    {
                        map.Logger.Log(newItem.Paths.FullPath, "Company not found", ProcessStatus.FieldError, NewItemField, cName);
                        continue;
                    }
                    else if (ids.Count() > 1)
                    { // format as (number-name)
                        map.Logger.Log(newItem.Paths.FullPath, $"Company not selected. Possible matches '{string.Join(" ", ids.Select(m => $"{m.Key}-{m.Value}"))}'", ProcessStatus.FieldError, NewItemField, cName);
                        continue;
                    }
                    pair = ids.First();
                }

                string businessID = pair.Value;
                if (string.IsNullOrEmpty(businessID))
                    continue;

                Regex companyPattern = new Regex($"(?<!<[^>]*)({cName})(?![^<]*</a)", RegexOptions.IgnoreCase);
                //the name should be the importValue and not the value from the database
                Field sf = newItem.Fields["Summary"];
                if (sf != null)
                {
                    sf.Value = companyPattern.Replace(sf.Value, match => $"<strong>[C#{businessID}:{match.Value}]</strong>", 1);
                }

                Field bf = newItem.Fields["Body"];
                if (bf != null)
                {
                    bf.Value = companyPattern.Replace(bf.Value, match => $"<strong>[C#{businessID}:{match.Value}]</strong>", 1);
                }

                cIDs.Add(businessID);
            }

            //store the imported value as is
            Field f = newItem.Fields[NewItemField];
            if (f == null)
                return;

            //set ids in field as comma delim list
            f.Value = string.Join(",", cIDs);
        }

        public Dictionary<string, string> GetDBCompanies(IDataMap map, string itemPath)
        {
            string cacheKey = "Companies";
            Dictionary<string, string> o = Context.Items[cacheKey] as Dictionary<string, string>;
            if (o != null)
                return o;

            Dictionary<string, string> companies = new Dictionary<string, string>();

            string query = "SELECT[RecordNumber], [Title] FROM [Companies] ORDER BY [Title] DESC";
            string conn = ConfigurationManager.ConnectionStrings["dcd"].ConnectionString;

            SqlConnection dbCon = null;
            DataTable returnObj = null;
            try
            {
                dbCon = new SqlConnection(conn);
                dbCon.Open();

                SqlCommand cmd = new SqlCommand(query);
                cmd.Connection = new SqlConnection(conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                returnObj = ds.Tables[0].Copy();
            }
            catch (Exception e)
            {
                map.Logger.Log(itemPath, "GetAllCompanies Failed", ProcessStatus.FieldError, NewItemField);
            }
            finally
            {
                if (dbCon != null)
                    dbCon.Close();
            }

            foreach (DataRow r in returnObj.Rows)
            {

                companies[r["Title"].ToString().ToLower()] = r["RecordNumber"].ToString();
            }

            Context.Items[cacheKey] = companies;
            return companies;
        }

        public Dictionary<string, string> GetDBCompaniesById(IDataMap map, string itemPath)
        {
            string cacheKey = "CompaniesById";
            Dictionary<string, string> o = Context.Items[cacheKey] as Dictionary<string, string>;
            if (o != null)
                return o;

            Dictionary<string, string> companies = new Dictionary<string, string>();

            string query = "SELECT[RecordNumber], [Title] FROM [Companies] ORDER BY [Title] DESC";
            string conn = ConfigurationManager.ConnectionStrings["dcd"].ConnectionString;

            SqlConnection dbCon = null;
            DataTable returnObj = null;
            try
            {
                dbCon = new SqlConnection(conn);
                dbCon.Open();

                SqlCommand cmd = new SqlCommand(query);
                cmd.Connection = new SqlConnection(conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                returnObj = ds.Tables[0].Copy();
            }
            catch (Exception e)
            {
                map.Logger.Log(itemPath, "GetAllCompanies Failed", ProcessStatus.FieldError, NewItemField);
            }
            finally
            {
                if (dbCon != null)
                    dbCon.Close();
            }

            foreach (DataRow r in returnObj.Rows)
            {
                companies[r["RecordNumber"].ToString()] = r["Title"].ToString();
            }

            Context.Items[cacheKey] = companies;
            return companies;
        }

        public Dictionary<string, string> GetFileCompanies()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();

            d.Add("4sc", "200001301");
            d.Add("abbot", "198600101");
            d.Add("abbott", "198600101");
            d.Add("acadia", "199700102");
            d.Add("acambis", "199300262");
            d.Add("actelion", "199800188");
            d.Add("activus pharma", "201500144");
            d.Add("adcock ingram", "198800225");
            d.Add("adeona pharmaceuticals", "200600835");
            d.Add("adventrx", "199700459");
            d.Add("ajinomoto", "198700287");
            d.Add("akorn", "199000427");
            d.Add("alcon", "198601361");
            d.Add("aldrich", "199000633");
            d.Add("alexion", "199300121");
            d.Add("alfacell", "198600271");
            d.Add("alios", "200700864");
            d.Add("alkermes", "198800695");
            d.Add("allergan", "198601285");
            d.Add("alliance pharma", "199600487");
            d.Add("alligator bioscience", "201500486");
            d.Add("alnylam", "200200681");
            d.Add("altea", "199600470");
            d.Add("alter", "198700592");
            d.Add("altus pharmaceuticals", "200100009");
            d.Add("amarin", "198900628");
            d.Add("amgen", "198600111");
            d.Add("amri", "199500499");
            d.Add("anaphore", "200400257");
            d.Add("angion biomedica", "201400229");
            d.Add("angiotech", "198601320");
            d.Add("anika therapeutics", "199300161");
            d.Add("anthera", "200500234");
            d.Add("antigenics", "198601321");
            d.Add("antisense pharma", "200400074");
            d.Add("apogee biotechnology", "201500212");
            d.Add("apotex", "198601232");
            d.Add("aradigm", "199400342");
            d.Add("arana therapeutics", "199100165");
            d.Add("arena", "199800421");
            d.Add("ariad", "199200091");
            d.Add("arigen", "200700952");
            d.Add("ark therapeutics", "199800145");
            d.Add("arqule", "199500526");
            d.Add("artisan pharma", "200600580");
            d.Add("asahi kasei pharma", "200600424");
            d.Add("asklepion pharmaceuticals", "201500011");
            d.Add("astellas", "198600364");
            d.Add("asterand", "199600308");
            d.Add("astrazenec", "198601342");
            d.Add("astrazeneca", "198601342");
            d.Add("avant immunotherapeutics", "198600219v");
            d.Add("avax technologies", "199500500");
            d.Add("aveo", "200200462");
            d.Add("avi biopharma", "199000359");
            d.Add("avigen", "199200384");
            d.Add("avineuro", "200900360");
            d.Add("avontec", "200201029");

            d.Add("basilea pharmaceutica", "200001083");
            d.Add("bavarian nordic", "199700439");
            d.Add("bayer", "198600358");
            d.Add("bayhill therapeutics", "200200715");
            d.Add("becton dickinson", "198600118");
            d.Add("benitec", "200300357");
            d.Add("bial", "200800001");
            d.Add("bind biosciences", "200700856");
            d.Add("biocompatibles", "199000041");
            d.Add("biocon", "200400289");
            d.Add("biodel", "200600510");
            d.Add("biogen idec", "198601366");
            d.Add("bioimage", "199902134");
            d.Add("biomet", "198600122");
            d.Add("bioms medical", "200001375");
            d.Add("bioinvent", "200000438");
            d.Add("biomarin", "199400477");
            d.Add("bionomics", "200101261");
            d.Add("biota", "198600193");
            d.Add("biotecnol", "200900412");
            d.Add("biotest", "200600477");
            d.Add("bioton", "200500313");
            d.Add("biovail", "198601277");
            d.Add("biovex", "200001412");
            d.Add("boehringer ingelhei", "198600820");
            d.Add("boehringer ingelheim", "198600820");
            d.Add("borean pharma", "200400257");
            d.Add("bracco", "198900374");
            d.Add("bradmer pharmaceuticals", "201400677");
            d.Add("bristol-myers squibb", "198601245");
            d.Add("bt pharma", "200700411");
            d.Add("btg", "198601174");

            d.Add("cambridge enterprise", "200600225");
            d.Add("cancer research technology", "200300248");
            d.Add("cangene", "198600453");
            d.Add("cardinal health", "198600127");
            d.Add("cardiokine", "200400329");
            d.Add("cardiome", "199200060");
            d.Add("cardion", "199700461");
            d.Add("carna biosciences", "201500363");
            d.Add("celera", "199100413");
            d.Add("celgene", "198601148");
            d.Add("cell genesys", "198601263");
            d.Add("cell therapeutics", "200500330");
            d.Add("cellectar", "200000801");
            d.Add("cellzome", "200100381");
            d.Add("cenes", "198900655");
            d.Add("cephalon", "198800712");
            d.Add("champions biotechnology", "200700142");
            d.Add("chantal", "198600587");
            d.Add("charleston laboratories", "201400433");
            d.Add("chiesi", "199200192");
            d.Add("choongwae", "199100373");
            d.Add("ck life sciences", "200700571");
            d.Add("clarion", "199901879");
            d.Add("cogenesys", "200600071");
            d.Add("columbia laboratories", "198800302");
            d.Add("compugen", "199800393");
            d.Add("covidien", "201100770");
            d.Add("creabilis therapeutics", "200400106");
            d.Add("crucell", "199400312");
            d.Add("csl", "198600617");
            d.Add("cubist", "199400401");
            d.Add("cue biotech", "200200298");
            d.Add("curis", "199400401");
            d.Add("cydex", "199800025");
            d.Add("cyrenaic", "201400249");
            d.Add("cytyc", "199000273");

            d.Add("dabur pharma", "200800383");
            d.Add("daiichi sanky", "198700764");
            d.Add("daiichi sankyo", "198700764");
            d.Add("debiopharm", "199100420");
            d.Add("deltagen", "199500495");
            d.Add("dexcel pharma", "200200830");
            d.Add("diamyd medical", "200600508");
            d.Add("dnx", "198700297");
            d.Add("dompe", "199200120");
            d.Add("dor biopharma", "199100198");
            d.Add("dr falk pharma", "199000506");
            d.Add("dr reddy's", "199700124");
            d.Add("drug discovery", "199902921");
            d.Add("dyax", "199000627");

            d.Add("eisa", "198800610");
            d.Add("eisai", "198800610");
            d.Add("elan", "198600151");
            d.Add("eli lill", "198600152");
            d.Add("eli lilly", "198600152");
            d.Add("emmaus medical", "201300503");
            d.Add("endo pharmaceuticals", "200200679");
            d.Add("engene", "200800179");
            d.Add("enobia", "200500069");
            d.Add("ensemble discovery", "200400436");
            d.Add("enzon", "198600154");
            d.Add("epicept", "199902983");
            d.Add("epitope", "198600593");
            d.Add("ethypharm", "199903139");
            d.Add("eurand", "199700451");
            d.Add("evolutec", "200000803");
            d.Add("evolva biotech", "200400035");
            d.Add("evotec", "199200398");
            d.Add("exonhit therapeutics", "199800438");
            d.Add("eyetech", "199200398");

            d.Add("favrille", "200200512");
            d.Add("ferrer", "199000258");
            d.Add("ferring", "199200432");
            d.Add("flamel technologies", "199600229");
            d.Add("forbes medi-tech", "199800033");
            d.Add("forest laboratories", "198600158");
            d.Add("forma therapeutics", "200900023");
            d.Add("freseniu", "198601363");
            d.Add("fresenius", "198601363");
            d.Add("fujirebio", "198700298");
            d.Add("fuso", "199700419");

            d.Add("galderma", "199200273");
            d.Add("gemin x biotechnologies", "200100031");
            d.Add("genechem", "200000104");
            d.Add("generex", "200000856");
            d.Add("genesis pharma", "199700131");
            d.Add("genetix pharmaceuticals", "199300041");
            d.Add("geneuro", "200700276");
            d.Add("genexine", "200200575");
            d.Add("genphar", "200200312");
            d.Add("genta", "198900047");
            d.Add("genzyme", "198600234");
            d.Add("geron", "199300040");
            d.Add("gerot", "201200228");
            d.Add("gilead sciences", "198601242");
            d.Add("glaxosmithkline", "198601356");
            d.Add("glenmark", "200400041");
            d.Add("glycomar", "200600009");
            d.Add("gni", "200400838");
            d.Add("gpc biotech", "199400474");
            d.Add("gruenenthal", "198600470");
            d.Add("grifols", "200900443");
            d.Add("gtc biotherapeutics", "198800747");
            d.Add("gulf", "199700019");

            d.Add("hana biosciences", "200400227");
            d.Add("handok", "199800134");
            d.Add("helsinn", "200100277");
            d.Add("henderson morley", "199903011");
            d.Add("hisamitsu", "200400004");
            d.Add("horizon therapeutics", "200700010");
            d.Add("hra pharma", "200700883");
            d.Add("hyperion", "200700657");
            d.Add("hyundai pharm", "201500795");

            d.Add("ibiopharma", "200800506");
            d.Add("ico therapeutics", "200500485");
            d.Add("igi", "198700466");
            d.Add("imed", "200800172");
            d.Add("immunomedics", "198600168");
            d.Add("immunotec", "200400478");
            d.Add("immunotope", "201400539");
            d.Add("inbiopro", "201300268");
            d.Add("incyte corporation", "198601279");
            d.Add("indevus", "198900430");
            d.Add("innate pharma", "200200602");
            d.Add("innocoll", "200500387");
            d.Add("inotek", "200400020");
            d.Add("inovio", "199300031");
            d.Add("intranasal therapeutics", "200100290");
            d.Add("iomed", "199200036");
            d.Add("ionix", "200100849");
            d.Add("irx therapeutics", "200600267");
            d.Add("is pharma", "200000427");
            d.Add("isolagen", "200200631");
            d.Add("isotechnika", "199903161");
            d.Add("ista pharmaceuticals", "200000483");
            d.Add("italfarmaco", "200300621");

            d.Add("jerini", "200101110");
            d.Add("johnson matthey", "198600981");
            d.Add("jubilant organosys", "200800242");

            d.Add("kai pharmaceuticals", "200300865");
            d.Add("karo bio", "198900367");
            d.Add("kinex", "200300779");
            d.Add("kirin pharma", "199400214");
            d.Add("kowa", "200200841");
            d.Add("kyowa hakko", "198700398");

            d.Add("labopharm", "199500072");
            d.Add("lacer", "199100200");
            d.Add("lavipharm", "199902043");
            d.Add("lexicon pharmaceuticals", "199500461");
            d.Add("life technologies", "198601261");
            d.Add("lifecycle pharma", "200300724");
            d.Add("ligand", "198700772");
            d.Add("light sciences", "200500551");
            d.Add("lipogen", "198800557");
            d.Add("lundbeck", "198700814");
            d.Add("lupin", "199000010");
            d.Add("lynx", "199200209");

            d.Add("madaus", "199903050");
            d.Add("manhattan pharmaceuticals", "199600484");
            d.Add("martek biosciences", "199300244");
            d.Add("maxygen", "199700251");
            d.Add("mdrna", "198800048");
            d.Add("meda", "200200740");
            d.Add("medac", "199600282");
            d.Add("medical devices", "199200072");
            d.Add("medicines company", "199600430");
            d.Add("medicinova", "200001022");
            d.Add("medical discoveries", "199901167");
            d.Add("medicure", "200100652");
            d.Add("medigene", "199100031");
            d.Add("medivation", "200500497");
            d.Add("medivir", "199500405");
            d.Add("medtronic", "198600185");
            d.Add("meiji dairies", "199500029");
            d.Add("menarini", "198800771");
            d.Add("mentor", "198600926");
            d.Add("merz", "199000351");
            d.Add("metabolex", "199700023");
            d.Add("metabolic solutions", "201000468");
            d.Add("methylgene", "199600144");
            d.Add("migenix", "199100395");
            d.Add("mirus", "200000014");
            d.Add("moberg derma", "200700547");
            d.Add("molteni farmaceutici", "200001267");
            d.Add("morria biopharmaceuticals", "200700480");
            d.Add("mpex", "200300839");
            d.Add("mundipharma", "198900663");

            d.Add("nanobio", "200300753");
            d.Add("nanotherapeutics", "201400322");
            d.Add("neurimmune therapeutics", "200700849");
            d.Add("neuropharm", "200700194");
            d.Add("neurosearch", "199000144");
            d.Add("nexbio", "200900482");
            d.Add("nih", "198600351");
            d.Add("nitec pharma", "200400783");
            d.Add("nobelpharma", "201100708");
            d.Add("nordic bioscience", "201100468");
            d.Add("norgine", "200200747");
            d.Add("novartis", "198600519");
            d.Add("novelos therapeutics", "200000801");
            d.Add("novo nordisk", "198700137");
            d.Add("novocell", "199200428");
            d.Add("novozymes", "200200498");
            d.Add("nsgene", "199903102");
            d.Add("nycomed pharma", "198700797");

            d.Add("omnichem", "198900398");
            d.Add("omrix", "200600032");
            d.Add("oni biopharma", "200400781");
            d.Add("ono", "199000105");
            d.Add("onyx pharmaceuticals", "199200104");
            d.Add("optimer", "200001371");
            d.Add("ore pharmaceuticals", "199400458");
            d.Add("orthologic", "198700799");
            d.Add("oryzon", "201400198");
            d.Add("osi pharmaceuticals", "198600253");
            d.Add("osteologix", "200500099");
            d.Add("otsuka", "198700860");
            d.Add("ovation pharmaceuticals", "200200417");
            d.Add("oxford genome sciences", "200500163");
            d.Add("oxis", "198600150");

            d.Add("par pharmaceutical", "198601284");
            d.Add("pci biotech", "200900158");
            d.Add("peptcell", "200900485");
            d.Add("pfize", "198600199");
            d.Add("pfizer", "198600199");
            d.Add("pharmacopeia", "200400357");
            d.Add("pharmascience", "199200256");
            d.Add("pharming", "199300356");
            d.Add("phytopharm", "199100209");
            d.Add("pierre fabre", "198700584");
            d.Add("pipex pharmaceuticals", "200600835");
            d.Add("piramal", "199400120");
            d.Add("pola", "201100566");
            d.Add("ppd", "198700837");
            d.Add("praxis", "199902028");
            d.Add("pro - pharmaceuticals", "200300106");
            d.Add("proethic", "200500155");
            d.Add("progen", "200000021");
            d.Add("prostrakan", "199800263");
            d.Add("protein delivery", "199100400");
            d.Add("proteome sciences", "200200490");
            d.Add("proteo", "200400926");
            d.Add("protox therapeutics", "200400833");
            d.Add("provectus pharmaceuticals", "200200914");
            d.Add("provid", "200200345");
            d.Add("proximagen neuroscience", "200400545");
            d.Add("psivida", "200500553");

            d.Add("qlt", "198700035");
            d.Add("qmax", "198700208");
            d.Add("qr pharma", "201500330");

            d.Add("ranbaxy", "199000613");
            d.Add("ratiopharm", "199400021");
            d.Add("reckitt benckiser", "199000115");
            d.Add("recordati", "198601005");
            d.Add("renovo", "200001321");
            d.Add("repair", "200000542");
            d.Add("replidyne", "200200552");
            d.Add("research corporation technologies", "200100742");
            d.Add("respironics", "198601203");
            d.Add("retroscreen", "201200299");
            d.Add("rib-x pharmaceuticals", "200200095");
            d.Add("rottapharm madaus", "199600141");
            d.Add("ruxton pharmaceuticals", "200400611");

            d.Add("samyang", "199100157");
            d.Add("sanguine", "199400264");
            d.Add("sanofi-aventis", "198601345");
            d.Add("sapphire therapeutics", "200100530");
            d.Add("sbio", "200900480");
            d.Add("schering-plough", "198600260");
            d.Add("schwabe", "201200150");
            d.Add("sciele pharma", "199700395");
            d.Add("sepracor", "198600347");
            d.Add("sequenom", "199600433");
            d.Add("shionogi", "198700350");
            d.Add("shire", "198601293");
            d.Add("sidus", "198800569");
            d.Add("siga", "199700257");
            d.Add("sigma-tau", "198700534");
            d.Add("simcere pharmaceuticals", "200600652");
            d.Add("sinclair pharma", "199000168");
            d.Add("skyepharma", "199200414");
            d.Add("sla pharma", "200200193");
            d.Add("solvay", "198600973");
            d.Add("sonus pharmaceuticals", "199300077");
            d.Add("sosei", "199700078");
            d.Add("sound pharmaceuticals", "201600176");
            d.Add("stiefel laboratories", "199000425");
            d.Add("sts", "200300285");
            d.Add("supergen", "199100265");
            d.Add("swedish orphan", "200100680");

            d.Add("taisho", "198700590");
            d.Add("taiwan liposome company", "201200416");
            d.Add("takeda", "198600337");
            d.Add("taro pharmaceutical", "200000010");
            d.Add("taurx therapeutics", "201000238");
            d.Add("teijin", "198600626");
            d.Add("terumo", "198700811");
            d.Add("teva", "198601169");
            d.Add("theraquest biosciences", "200400553");
            d.Add("thrombogenics", "200101324");
            d.Add("tigenix", "200300560");
            d.Add("tolerrx", "200100825");
            d.Add("toray", "198700013");
            d.Add("toyama", "198800549");
            d.Add("transdel pharmaceuticals", "200700820");
            d.Add("transderm", "200600741");
            d.Add("transdermal", "199901331");
            d.Add("transgene", "198700558");
            d.Add("tripep", "200200556");

            d.Add("ucb", "198900659");
            d.Add("unigene", "198600551");
            d.Add("uriach", "199300362");
            d.Add("urigen", "199300312");

            d.Add("vaccine technologies", "200900043");
            d.Add("valeant", "198600166");
            d.Add("vantia", "200800216");
            d.Add("vasogen", "200100069");
            d.Add("vaxin", "199800300");
            d.Add("ventech", "198600445");
            d.Add("vernalis", "198700854");
            d.Add("vertex pharmaceuticals", "198900222");
            d.Add("vion pharmaceuticals", "199200386");
            d.Add("viralytics", "201400183");
            d.Add("viropharma", "199500305");
            d.Add("vivalis", "200300399");

            d.Add("watson", "198601285");
            d.Add("wellstat", "200900459");
            d.Add("wilex", "199902912");
            d.Add("wockhardt", "200200261");

            d.Add("xenome", "200101271");
            d.Add("xention", "201100036");
            d.Add("xigen", "200300238");

            d.Add("yaupon therapeutics", "200400876");
            d.Add("ym biosciences", "200000253");

            d.Add("zambon", "198800493");
            d.Add("zelos therapeutics", "200300266");
            d.Add("zetiq", "200700504");

            return d;
        }

        #endregion Methods
    }

    public class ToInformaContentType : ListToGuid
    {
        public ToInformaContentType(Item i) : base(i) { }

        #region Methods

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
        {
            if (string.IsNullOrEmpty(importValue))
                return;

            //get parent item of list to search
            var sourceItems = GetSourceItems(newItem.Database);
            if (sourceItems == null)
                return;

            Dictionary<string, string> d = GetMapping();

            string lowerValue = importValue.ToLower();
            string transformValue = (d.ContainsKey(lowerValue)) ? d[lowerValue] : string.Empty;
            if (string.IsNullOrEmpty(transformValue))
            {
                map.Logger.Log(newItem.Paths.FullPath, "Content Type not converted", ProcessStatus.FieldError, NewItemField, importValue);
                return;
            }

            //loop through children and look for anything that matches by name
            string cleanName = StringUtility.GetValidItemName(transformValue, map.ItemNameMaxLength);
            IEnumerable<Item> t = sourceItems.Where(c => c.DisplayName.Equals(cleanName));

            //if you find one then store the id
            if (!t.Any())
            {
                map.Logger.Log(newItem.Paths.FullPath, "Content Type not matched", ProcessStatus.FieldError, NewItemField, importValue);
                return;
            }

            Field f = newItem.Fields[NewItemField];
            if (f == null)
                return;

            string ctID = t.First().ID.ToString();
            if (!f.Value.Contains(ctID))
                //   f.Value = (f.Value.Length > 0) ? $"{f.Value}|{ctID}" : ctID;
                f.Value = ctID;
        }

        protected virtual Dictionary<string, string> GetMapping()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();

            d.Add("news", "News");
            d.Add("analysis", "Analysis");
            d.Add("interviews", "Interviews");
            d.Add("analyst", "Analysis");
            d.Add("opinion", "Opinion");
            d.Add("asia", "News");
            d.Add("briefstory", "News");
            d.Add("business", "News");
            d.Add("business news", "News");
            d.Add("clinical research", "News");
            d.Add("comment", "Opinion");
            d.Add("commentary", "Opinion");
            d.Add("companies", "News");
            d.Add("conference reports", "News");
            d.Add("dataofday", "News");
            d.Add("drug delivery", "News");
            d.Add("ece_incoming", "News");
            d.Add("edcarticle", "News");
            d.Add("editorial", "Opinion");
            d.Add("event stories", "News");
            d.Add("events", "News");
            d.Add("eventstory", "News");
            d.Add("executive briefing", "News");
            d.Add("executivebriefing", "News");
            d.Add("expert view", "Analysis");
            d.Add("expertview", "Analysis");
            d.Add("face-to-face", "News");
            return d;
        }

        #endregion Methods

    }

    public class ToInformaTherapyAreas : ListToGuid
    {
        public ToInformaTherapyAreas(Item i) : base(i) { }

        #region Methods

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
        {
            if (string.IsNullOrEmpty(importValue))
                return;

            //get parent item of list to search
            Item i = newItem.Database.GetItem(SourceList);
            if (i == null)
                return;

            Field f = newItem.Fields[NewItemField];
            if (f == null)
                return;

            Dictionary<string, string> d = GetMapping();

            string[] importArr = importValue.Split(new string[] { Delimiter }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string importPart in importArr)
            {
                string lowerValue = importPart.ToLower();
                string transformValue = (d.ContainsKey(lowerValue)) ? d[lowerValue] : string.Empty;
                if (string.IsNullOrEmpty(transformValue))
                {
                    map.Logger.Log(newItem.Paths.FullPath, "Therapy Area(s) not converted", ProcessStatus.FieldError, NewItemField, importPart);
                    continue;
                }

                string[] parts = transformValue.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries);

                Item[] cl = i.Axes.GetDescendants();

                //loop through children and look for anything that matches by name
                foreach (string area in parts)
                {
                    string cleanName = StringUtility.GetValidItemName(area, map.ItemNameMaxLength);
                    IEnumerable<Item> t = cl.Where(c => c.DisplayName.Equals(cleanName));

                    //if you find one then store the id
                    if (!t.Any())
                    {
                        map.Logger.Log(newItem.Paths.FullPath, "Therapy Area(s) not found in list", ProcessStatus.FieldError, NewItemField, area);
                        continue;
                    }

                    string ctID = t.First().ID.ToString();
                    if (!f.Value.Contains(ctID))
                        f.Value = (f.Value.Length > 0) ? $"{f.Value}|{ctID}" : ctID;
                }
            }
        }

        private Dictionary<string, string> GetMapping()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();

            d.Add("abdominal aortic aneurysm", "Cardiovascular");
            d.Add("abnormal uterine bleeding", "Gynecology & Urology");
            d.Add("abortion", "Gynecology & Urology");
            d.Add("absence epilepsy", "Neurology");
            d.Add("acidosis", "Blood &Coagulation Disorders");
            d.Add("acne", "Dermatology");
            d.Add("acromegaly", "Metabolic Disorders");
            d.Add("actinic keratosis", "Dermatology");
            d.Add("acute coronary syndrome", "Cardiovascular");
            d.Add("acute lymphocytic leukaemia", "Cancer");
            d.Add("acute myelogenous leukaemia", "Cancer");
            d.Add("acute renal failure", "Renal");
            d.Add("addiction", "Neurology");
            d.Add("addison's disease", "Metabolic Disorders");
            d.Add("adenosine deaminase deficiency", "Metabolic Disorders");
            d.Add("adenovirus", "Infectious Diseases");
            d.Add("adhesive capsulitis", "");
            d.Add("adrenal cancer", "Cancer");
            d.Add("adrenoleukodystrophy", "Metabolic Disorders::Neurology");
            d.Add("adult respiratory distress syndrome", "Respiratory");
            d.Add("aesthetics", "Aesthetics");
            d.Add("african trypanosomiasis", "Infectious Diseases");
            d.Add("age-related macular degeneration", "Ophthalmic");
            d.Add("aids-related dementia", "Immune Disorders::Neurology");
            d.Add("alcohol addiction", "Neurology");
            d.Add("alcohol intolerance", "Neurology");
            d.Add("allergic rhinitis", "Ear, Nose &Throat::Immune Disorders");
            d.Add("allergy", "Immune Disorders");
            d.Add("alopecia", "Dermatology");
            d.Add("alpha-mannosidosis", "");
            d.Add("alzheimer's disease", "Neurology");
            d.Add("amenorrhoea", "Gynecology & Urology");
            d.Add("american trypanosomiasis", "Infectious Diseases");
            d.Add("amnesia", "Neurology");
            d.Add("amyloidosis", "Metabolic Disorders::Inflammation");
            d.Add("amyotrophic lateral sclerosis", "Neurology");
            d.Add("anaemia", "Blood & Coagulation Disorders");
            d.Add("anaesthesia", "Neurology");
            d.Add("anal dysplasia", "");
            d.Add("anal fissure", "");
            d.Add("anal fistula", "");
            d.Add("anaphylaxis", "Immune Disorders");
            d.Add("androgenic alopecia", "Aesthetics");
            d.Add("anemia", "Blood &Coagulation Disorders");
            d.Add("aneurysm", "Cardiovascular");
            d.Add("angina", "Cardiovascular");
            d.Add("ankylosing spondylitis", "Inflammation::Orthopedics");
            d.Add("anorexia", "Neurology");
            d.Add("anorexia nervosa", "Neurology");
            d.Add("anthrax", "Infectious Diseases");
            d.Add("antibiotic Resistance", "Infectious Diseases");
            d.Add("antithrombin iii deficiency", "");
            d.Add("anxiety", "Neurology");
            d.Add("aphthous ulcer", "");
            d.Add("apnoea", "Cardiovascular");
            d.Add("appendicitis", "Gastrointestinal");
            d.Add("arrhythmia", "Cardiovascular");
            d.Add("arterial thrombosis", "Cardiovascular");
            d.Add("arthritis", "Orthopedics");
            d.Add("aspergillus", "Immune Disorders");
            d.Add("asthma", "Respiratory");
            d.Add("atherosclerosis", "Cardiovascular");
            d.Add("atopic eczema", "Immune Disorders::Inflammation");
            d.Add("atrial fibrillation", "Cardiovascular");
            d.Add("atrial tachycardia", "Cardiovascular");
            d.Add("attention deficit disorder", "Neurology");
            d.Add("attention deficit hyperactivity disorder", "Neurology");
            d.Add("autism", "Neurology");
            d.Add("autoimmune disorders", "Immune Disorders");
            d.Add("bacterial", "Infectious Diseases");
            d.Add("barrett's esophagus", "Gastrointestinal");
            d.Add("Barrett's oesophagus", "Gastrointestinal");
            d.Add("basal cell cancer", "Cancer");
            d.Add("basal cell carcinoma", "Cancer");
            d.Add("b-cell lymphoma", "Cancer");
            d.Add("behcet's disease", "");
            d.Add("benign prostate hypertrophy", "Gynecology & Urology");
            d.Add("benign prostatic hyperplasia", "Gynecology & Urology");
            d.Add("biliary cancer", "Cancer");
            d.Add("biliary cirrhosis", "Gastrointestinal");
            d.Add("bipolar depression", "Neurology");
            d.Add("bipolar disorder", "Neurology");
            d.Add("bladder cancer", "Cancer");
            d.Add("bladder disease", "Gynecology & Urology");
            d.Add("blepharitis", "");
            d.Add("blepharospasm", "");
            d.Add("blood & coagulation disorders", "Blood & Coagulation Disorders");
            d.Add("blood cancer", "Cancer");
            d.Add("blood substitutes", "Blood & Coagulation Disorders");
            d.Add("blood volume", "Blood &Coagulation Disorders");
            d.Add("bone", "Orthopedics");
            d.Add("bone cancer", "Cancer");
            d.Add("bone disorder", "");
            d.Add("bone regeneration", "");
            d.Add("bowel incontinence", "Gastrointestinal");
            d.Add("bradycardia", "Cardiovascular");
            d.Add("brain cancer", "Cancer");
            d.Add("brain inflammation", "Neurology::Inflammation");
            d.Add("breast cancer", "Cancer");
            d.Add("bronchiectasis", "");
            d.Add("bronchitis", "Respiratory");
            d.Add("bulimia", "");
            d.Add("bunyaviridae", "");
            d.Add("burkholderia pseudomallei", "");
            d.Add("burns", "Wound Healing &Tissue Repair");
            d.Add("bursitis", "Orthopedics::Inflammation");
            d.Add("cachexia", "");
            d.Add("campylobacter", "");
            d.Add("cancer", "Cancer");
            d.Add("cancer fatigue", "Cancer");
            d.Add("cancer pain", "Cancer");
            d.Add("candida", "Infectious Diseases");
            d.Add("candida albicans", "Infectious Diseases");
            d.Add("carcinoid", "");
            d.Add("carcinoid syndrome", "");
            d.Add("cardiac injury", "Cardiovascular");
            d.Add("cardiovascular", "Cardiovascular");
            d.Add("carpal tunnel syndrome", "Orthopedics");
            d.Add("cartilage regeneration", "Orthopedics");
            d.Add("cartilage repair", "Orthopedics");
            d.Add("castleman's disease", "");
            d.Add("cataplexy", "");
            d.Add("cataract", "Ophthalmic");
            d.Add("cerebral haemorrhage", "Cardiovascular");
            d.Add("cerebral infarction", "Cardiovascular");
            d.Add("cerebral ischaemia", "Cardiovascular");
            d.Add("cerebral oedema", "Cardiovascular");
            d.Add("cerebral thrombosis", "Cardiovascular");
            d.Add("cerebral vasospasm", "Cardiovascular");
            d.Add("cervical cancer", "Cancer");
            d.Add("cervical dysplasia", "Gynecology & Urology");
            d.Add("cervical dystonia", "Gynecology &Urology");
            d.Add("chlamydia", "Infectious Diseases");
            d.Add("cholera", "Infectious Diseases");
            d.Add("cholesterol", "Cardiovascular");
            d.Add("chronic fatigue syndrome", "Neurology::Immune Disorders");
            d.Add("chronic granulomatous disease", "");
            d.Add("chronic inflammatory demyelinating polyneuropathy", "");
            d.Add("chronic lymphocytic leukaemia", "Cancer");
            d.Add("chronic myelogenous leukaemia", "Cancer");
            d.Add("chronic obstructive pulmonary disease", "Cardiovascular");
            d.Add("chronic obstructive pulmonary disease(copd)", "Respiratory");
            d.Add("chronic progressive multiple sclerosis", "Neurology");
            d.Add("chronic renal failure", "Renal");
            d.Add("churg-strauss syndrome", "");
            d.Add("cirrhosis", "Liver & Hepatic");
            d.Add("clostridium", "");
            d.Add("clostridium botulinum", "");
            d.Add("clostridium difficile", "Infectious Diseases");
            d.Add("clotting disorders", "Blood & Coagulation Disorders");
            d.Add("cluster headache", "Neurology");
            d.Add("cns", "Neurology");
            d.Add("cns cancer", "Cancer");
            d.Add("cocaine addiction", "Neurology");
            d.Add("cockayne syndrome", "");
            d.Add("coeliac disease", "Gastrointestinal");
            d.Add("cognitive disorders", "Neurology");
            d.Add("cold", "Respiratory");
            d.Add("colitis", "Gastrointestinal");
            d.Add("colon cancer", "Cancer");
            d.Add("colorectal cancer", "Cancer");
            d.Add("complex regional pain", "Neurology");
            d.Add("congenital adrenal hyperplasia", "");
            d.Add("conjunctivitis", "Ophthalmic");
            d.Add("constipation", "Gastrointestinal");
            d.Add("contraceptive", "Gynecology & Urology");
            d.Add("coronary artery bypass grafting", "Cardiovascular");
            d.Add("coronary artery disease", "Cardiovascular");
            d.Add("coronary thrombosis", "Cardiovascular");
            d.Add("coronavirus", "Infectious Diseases");
            d.Add("cough", "Respiratory");
            d.Add("coxsackievirus", "Infectious Diseases");
            d.Add("creutzfeldt-jakob disease", "");
            d.Add("crohn's disease", "Gastrointestinal");
            d.Add("croup", "Infectious Diseases");
            d.Add("cryptosporidiosis", "");
            d.Add("cushing's disease", "");
            d.Add("cystic fibrosis", "Respiratory");
            d.Add("cystinosis", "");
            d.Add("cystitis", "Gynecology &Urology");
            d.Add("cytomegalovirus", "Infectious Diseases");
            d.Add("deafness", "Ear,Nose & Throat");
            d.Add("deep vein thrombosis", "Cardiovascular");
            d.Add("dengue fever", "Infectious Diseases");
            d.Add("dengue virus", "Infectious Diseases");
            d.Add("dental-oral", "Dental-Oral");
            d.Add("depression", "Neurology");
            d.Add("dermal inflammation", "");
            d.Add("dermatitis", "Dermatology");
            d.Add("dermatological", "Dermatology");
            d.Add("dermatology", "Dermatology");
            d.Add("diabetes", "Metabolic Disorders");
            d.Add("diabetic cardiomyopathy", "Metabolic Disorders::Cardiovascular");
            d.Add("diabetic complication", "Metabolic Disorders");
            d.Add("diabetic macular oedema", "Metabolic Disorders::Ophthalmic");
            d.Add("diabetic nephropathy", "Metabolic Disorders::Neurology");
            d.Add("diabetic neuropathy", "Metabolic Disorders::Neurology");
            d.Add("diabetic retinopathy", "Metabolic Disorders::Ophthalmic");
            d.Add("diabetic ulcer", "Wound Healing & Tissue Repair");
            d.Add("diabetic ulcers", "Wound Healing & Tissue Repair");
            d.Add("diagnostic", "");
            d.Add("diarrhea", "Gastrointestinal");
            d.Add("diarrhoea", "Gastrointestinal");
            d.Add("dilated cardiomyopathy", "Cardiovascular");
            d.Add("diphtheria", "Infectious Diseases");
            d.Add("diptheria", "Infectious Diseases");
            d.Add("disseminated intravascular coagulation", "");
            d.Add("distributive shock", "");
            d.Add("diverticulitis", "");
            d.Add("down's syndrome", "Neurology");
            d.Add("drug addiction", "Neurology");
            d.Add("drug poisoning", "Poisoning");
            d.Add("dry eye", "Ophthalmic");
            d.Add("duodenal ulcer", "Gastrointestinal");
            d.Add("dupuytren's disease", "");
            d.Add("dwarfism", "");
            d.Add("dyskinesia", "");
            d.Add("dysmenorrhoea", "Gynecology & Urology");
            d.Add("dyspareunia", "");
            d.Add("dyspepsia", "");
            d.Add("dysplasia", "");
            d.Add("dystonia", "");
            d.Add("dysuria", "");
            d.Add("ear, nose & throat", "Ear, Nose & Throat");
            d.Add("ear, nose &throat", "Ear, Nose & Throat");
            d.Add("eastern equine encephalitis virus", "Infectious Diseases");
            d.Add("eating disorders", "Metabolic Disorders");
            d.Add("ebola virus", "Infectious Diseases");
            d.Add("ectodermal dysplasia", "");
            d.Add("eczema", "Dermatology");
            d.Add("emotional lability", "");
            d.Add("emphysema", "Respiratory");
            d.Add("encephalitis", "Infectious Diseases");
            d.Add("endocrine", "Metabolic Disorders");
            d.Add("endocrine cancer", "Cancer");
            d.Add("endometrial cancer", "Cancer");
            d.Add("endometriosis", "Gynecology & Urology");
            d.Add("end-stage renal disease", "Renal");
            d.Add("enterovirus 71", "Infectious Diseases");
            d.Add("enuresis", "");
            d.Add("epidermolysis bullosa", "");
            d.Add("epilepsy", "Neurology");
            d.Add("epstein barr", "Infectious Diseases");
            d.Add("epstein-barr virus", "Infectious Diseases");
            d.Add("erectile dysfunction", "Gynecology & Urology");
            d.Add("escherichia coli", "Infectious Diseases");
            d.Add("esophageal cancer", "Cancer");
            d.Add("esophagitis", "Gastrointestinal");
            d.Add("ewing's sarcoma", "Cancer");
            d.Add("fabry disease", "Metabolic Disorders");
            d.Add("fabry's disease", "");
            d.Add("factor xiii deficiency", "");
            d.Add("fallopian tube cancer", "Cancer");
            d.Add("familial cold autoinflammatory syndrome", "");
            d.Add("fat malabsorption", "");
            d.Add("female", "Gynecology & Urology");
            d.Add("female contraception", "Gynecology &Urology");
            d.Add("female infertility", "Gynecology &Urology");
            d.Add("female sexual dysfunction", "Gynecology & Urology");
            d.Add("fever", "");
            d.Add("fibroids", "Gynecology & Urology");
            d.Add("fibromyalgia", "Immune Disorders");
            d.Add("fibrosarcoma", "Cancer");
            d.Add("fibrosis", "");
            d.Add("filariasis", "Infectious Diseases");
            d.Add("flatulence", "Gastrointestinal");
            d.Add("food allergy", "Immune Disorders");
            d.Add("fractures", "Orthopedics");
            d.Add("fragile x syndrome", "");
            d.Add("francisella tularensis", "");
            d.Add("friedreich's ataxia", "");
            d.Add("fungal", "Infectious Diseases");
            d.Add("fungal infection", "Infectious Diseases");
            d.Add("gallstone", "Liver &Hepatic");
            d.Add("gastointestinal", "Gastrointestinal");
            d.Add("gastric ulcer", "Gastrointestinal");
            d.Add("gastritis", "Gastrointestinal");
            d.Add("gastroenteritis", "Gastrointestinal");
            d.Add("gastroesophageal Reflux Disorder (Gerd)", "Gastrointestinal");
            d.Add("gastrointestinal", "Gastrointestinal");
            d.Add("gastrointestinal cancer", "Cancer");
            d.Add("gastrointestinal ulcer", "Gastrointestinal");
            d.Add("gastrokinetic", "Gastrointestinal");
            d.Add("gastro-oesophageal reflux", "Gastrointestinal");
            d.Add("gastroparesis", "Gastrointestinal");
            d.Add("gaucher disease", "Metabolic Disorders");
            d.Add("gaucher's disease", "Metabolic Disorders");
            d.Add("gelineau's syndrome", "");
            d.Add("generalized anxiety disorder", "Neurology");
            d.Add("genital warts", "Infectious Diseases");
            d.Add("genitourinary", "Gynecology & Urology");
            d.Add("gi injury", "");
            d.Add("glaucoma", "Ophthalmic");
            d.Add("globoid cell leukodystrophy", "");
            d.Add("glomerulonephritis", "");
            d.Add("gonorrhea", "Gynecology & Urology::Infectious Diseases");
            d.Add("gonorrhoea", "Gynecology & Urology::Infectious Diseases");
            d.Add("gouty arthritis", "Immune Disorders");
            d.Add("gram-negative", "Infectious Diseases");
            d.Add("gram-positive", "Infectious Diseases");
            d.Add("granulocytopenia", "");
            d.Add("growth disorders", "Metabolic Disorders");
            d.Add("growth hormone deficiency", "Metabolic Disorders");
            d.Add("guillain-barre syndrome", "");
            d.Add("gynaecomastia", "");
            d.Add("gynecology & urology", "Gynecology & Urology");
            d.Add("haemolytic anaemia", "");
            d.Add("haemolytic uraemic syndrome", "");
            d.Add("haemophilia", "Blood & Coagulation Disorders");
            d.Add("haemophilia a", "Blood & Coagulation Disorders");
            d.Add("haemophilia b", "Blood & Coagulation Disorders");
            d.Add("haemophilus influenzae", "Blood & Coagulation Disorders");
            d.Add("haemorrhage", "");
            d.Add("haemorrhagic fever", "Infectious Diseases");
            d.Add("hair loss", "Dermatology");
            d.Add("hairy cell leukaemia", "Cancer");
            d.Add("head and neck cancer", "Cancer");
            d.Add("head trauma", "");
            d.Add("headache", "Neurology");
            d.Add("hearing loss", "Ear, Nose & Throat::Neurology");
            d.Add("heart failure", "Cardiovascular");
            d.Add("heart valve Disease", "Cardiovascular");
            d.Add("helicobacter pylori", "Infectious Diseases");
            d.Add("hemophilia", "Blood & Coagulation Disorders");
            d.Add("hemorrhoid", "Wound Healing & Tissue Repair");
            d.Add("hepatic", "Liver & Hepatic");
            d.Add("hepatic cirrhosis", "Gastrointestinal");
            d.Add("hepatic dysfunction", "Gastrointestinal");
            d.Add("hepatic encephalopathy", "Infectious Diseases");
            d.Add("hepatitis a", "Infectious Diseases");
            d.Add("hepatitis b", "Infectious Diseases");
            d.Add("hepatitis c", "Infectious Diseases");
            d.Add("hepatitis virus", "Infectious Diseases::Gastrointestinal");
            d.Add("hepatitis-b", "Infectious Diseases::Gastrointestinal");
            d.Add("hepatitis-b virus", "Infectious Diseases::Gastrointestinal");
            d.Add("hepatitis-c", "Infectious Diseases::Gastrointestinal");
            d.Add("hepatitis-e", "Infectious Diseases::Gastrointestinal");
            d.Add("hereditary blindness", "Ophthalmic");
            d.Add("hereditary tyrosinaemia", "");
            d.Add("herpes", "Infectious Diseases");
            d.Add("herpes simplex", "Infectious Diseases");
            d.Add("herpetic keratitis", "");
            d.Add("hips", "Orthopedics");
            d.Add("hirsutism", "");
            d.Add("hiv", "Infectious Diseases");
            d.Add("hiv/aids", "Infectious Diseases");
            d.Add("hiv-aids", "Infectious Diseases");
            d.Add("hodgkin's disease", "Cancer");
            d.Add("hodgkin's lymphoma", "Cancer");
            d.Add("homocystinuria", "");
            d.Add("hormonal", "Metabolic Disorders");
            d.Add("hormone replacement", "Gynecology & Urology");
            d.Add("hormone replacement therapy", "");
            d.Add("htlv", "");
            d.Add("human metapneumovirus", "Infectious Diseases");
            d.Add("human papilloma virus", "Infectious Diseases");
            d.Add("human papilloma virus(hpv)", "Infectious Diseases");
            d.Add("huntington's disease", "Neurology");
            d.Add("hyperammonaemia", "");
            d.Add("hyperbilirubinaemia", "");
            d.Add("hypercalcaemia", "");
            d.Add("hypercalcaemia of malignancy", "");
            d.Add("hypercalcemia", "Metabolic Disorders");
            d.Add("hypercholesterolaemia", "Metabolic Disorders");
            d.Add("hypereosinophilic syndrome", "");
            d.Add("hyperhidrosis", "");
            d.Add("hyperkalaemia", "");
            d.Add("hyperlipidaemia", "Metabolic Disorders");
            d.Add("hyperlipidemia", "Metabolic Disorders");
            d.Add("hyperoxaluria", "");
            d.Add("hyperparathyroidism", "");
            d.Add("hyperphenylalaninaemia", "");
            d.Add("hyperphosphataemia", "Metabolic Disorders");
            d.Add("hyperphsphatemia", "Metabolic Disorders");
            d.Add("hyperprolactinaemia", "");
            d.Add("hypertension", "Cardiovascular");
            d.Add("hyperthyroidism", "Immune Disorders::Metabolic Disorders");
            d.Add("hypertriglyceridaemia", "");
            d.Add("hyperuricaemia", "");
            d.Add("hypoalbuminaemia", "");
            d.Add("hypoglycaemia", "");
            d.Add("hypogonadism", "Metabolic Disorders");
            d.Add("hyponatraemia", "");
            d.Add("hypoparathyroidism", "Immune Disorders::Metabolic Disorders");
            d.Add("hypophosphataemia", "");
            d.Add("hypotension", "Cardiovascular");
            d.Add("hypothyroidism", "Immune Disorders::Metabolic Disorders");
            d.Add("ichthyosis", "");
            d.Add("idiopathic myelofibrosis", "");
            d.Add("ileus", "");
            d.Add("immune disorders", "Immune Disorders");
            d.Add("immunodeficiency", "Immune Disorders");
            d.Add("immunological", "Immune Disorders");
            d.Add("impetigo", "");
            d.Add("impotence", "Gynecology & Urology");
            d.Add("incontinence", "Gynecology & Urology");
            d.Add("induction", "");
            d.Add("infant respiratory distress syndrome", "");
            d.Add("infarction", "");
            d.Add("infection", "Infectious Diseases");
            d.Add("infectious diseases", "Infectious Diseases");
            d.Add("infertility", "Gynecology & Urology");
            d.Add("inflammation", "Inflammation");
            d.Add("inflammatory bowel disease", "Gastrointestinal");
            d.Add("influenza", "Infectious Diseases");
            d.Add("insomnia", "Neurology");
            d.Add("insulin-related metabolic syndrome", "Metabolic Disorders");
            d.Add("iron disorders", "Blood & Coagulation Disorders");
            d.Add("irritable bowel syndrome", "Gastrointestinal");
            d.Add("ischaemia", "");
            d.Add("ischaemic cardiomyopathy", "");
            d.Add("ischaemic optic neuropathy", "");
            d.Add("ischemia", "Cardiovascular");
            d.Add("japanese encephalitis", "Infectious Diseases");
            d.Add("jaundice", "Liver & Hepatic");
            d.Add("kaposi's sarcoma", "Cancer");
            d.Add("kawasaki disease", "");
            d.Add("keloid", "Wound Healing & Tissue Repair");
            d.Add("keratoconjunctivitis", "");
            d.Add("keratoconus", "");
            d.Add("keratosis", "");
            d.Add("kidney disease", "Renal");
            d.Add("kidney failure", "Renal");
            d.Add("kidney stones", "Renal");
            d.Add("kidney transplant", "Renal");
            d.Add("knees", "Orthopedics");
            d.Add("lactic acidosis", "");
            d.Add("lambert-Eaton myasthenic syndrome", "");
            d.Add("lassa virus", "Infectious Diseases");
            d.Add("lateral epicondylitis", "");
            d.Add("leber's congenital amaurosis", "");
            d.Add("leber's hereditary optic neuropathy", "");
            d.Add("left atrial appendage", "Cardiovascular");
            d.Add("leiomyosarcoma", "Cancer");
            d.Add("leishmaniasis", "Infectious Diseases");
            d.Add("lennox-bastaut syndrome", "");
            d.Add("leprechaunism", "");
            d.Add("leprosy", "Infectious Diseases");
            d.Add("leukaemia", "Cancer");
            d.Add("leukemia", "Cancer");
            d.Add("leukopenia", "");
            d.Add("leukoplakia", "");
            d.Add("li-fraumeni syndrome", "");
            d.Add("lipodystrophy", "Metabolic Disorders");
            d.Add("lipoma", "");
            d.Add("liposarcoma", "Cancer");
            d.Add("liver & hepatic", "Liver & Hepatic");
            d.Add("liver cancer", "Cancer");
            d.Add("liver failure", "Liver & Hepatic");
            d.Add("liver fibrosis", "Gastrointestinal");
            d.Add("long qt syndrome", "Cardiovascular");
            d.Add("lookup", "Top Level");
            d.Add("lower respiratory tract infection", "Cardiovascular");
            d.Add("lung cancer", "Cancer");
            d.Add("lupus", "Immune Disorders");
            d.Add("lupus erythematosus", "Immune Disorders");
            d.Add("lupus nephritis", "Immune Disorders");
            d.Add("lyme disease", "Infectious Diseases");
            d.Add("lymphoma", "Cancer");
            d.Add("macular degeneration", "Ophthalmic");
            d.Add("macular oedema", "Ophthalmic");
            d.Add("major depressive disorder", "Neurology");
            d.Add("malaria", "Infectious Diseases");
            d.Add("male", "Gynecology & Urology");
            d.Add("male contraception", "Gynecology & Urology");
            d.Add("male infertility", "Gynecology & Urology");
            d.Add("male sexual dysfunction", "Gynecology & Urology");
            d.Add("malignant pleural effusion", "");
            d.Add("marburg virus", "Infectious Diseases");
            d.Add("mastalgia", "");
            d.Add("mastocytosis", "");
            d.Add("measles", "Infectious Diseases");
            d.Add("meconium aspiration syndrome", "");
            d.Add("melanoma", "Cancer");
            d.Add("melasma", "");
            d.Add("memory disorders", "Neurology");
            d.Add("meniere's disease", "");
            d.Add("meningitis", "Infectious Diseases");
            d.Add("menopausal symptoms", "Gynecology & Urology");
            d.Add("menopause", "Gynecology & Urology");
            d.Add("menorrhagia", "Gynecology & Urology");
            d.Add("mental retardation", "Neurology");
            d.Add("mesothelioma", "Cancer");
            d.Add("metabolic", "Metabolic Disorders");
            d.Add("metabolic disorders", "Metabolic Disorders");
            d.Add("metachromatic leukodystrophy", "");
            d.Add("migraine", "Neurology");
            d.Add("miscarriage", "Gynecology & Urology");
            d.Add("mitochondrial disease", "Metabolic Disorders");
            d.Add("mitochondrial encephalomyopathy", "Metabolic Disorders");
            d.Add("motility dysfunction", "Gynecology & Urology");
            d.Add("motor neurone disease", "Neurology");
            d.Add("mrsa", "Infectious Diseases");
            d.Add("mssa", "");
            d.Add("muckle-wells syndrome", "");
            d.Add("mucopolysaccharidosis", "");
            d.Add("mucositis", "");
            d.Add("multiple sclerosis", "Immune Disorders::Neurology");
            d.Add("multiple system atrophy", "");
            d.Add("mumps", "Infectious Diseases");
            d.Add("muscle & connective tissue", "Orthopedics");
            d.Add("muscle spasm", "Orthopedics");
            d.Add("muscular atrophy", "Immune Disorders");
            d.Add("muscular dystrophy", "Neurology");
            d.Add("musculoskeletal", "Orthopedics");
            d.Add("musculoskeletal pain", "Orthopedics::Neurology");
            d.Add("myasthenia gravis", "");
            d.Add("mycobacterium avium complex", "");
            d.Add("mycobacterium ulcerans", "");
            d.Add("myelodysplastic syndrome", "Blood & Coagulation Disorders");
            d.Add("myeloma", "Cancer");
            d.Add("myocardial fibrosis", "");
            d.Add("myocardial infarction", "Cardiovascular");
            d.Add("myocarditis", "");
            d.Add("myoma", "");
            d.Add("myopia", "Ophthalmic");
            d.Add("narcolepsy", "Neurology");
            d.Add("narcotic addiction", "Neurology");
            d.Add("nasopharyngeal cancer", "Cancer");
            d.Add("nausea and vomiting", "Gastrointestinal");
            d.Add("neck cancer", "Cancer");
            d.Add("necrotizing enterocolitis", "");
            d.Add("neisseria meningitidis", "");
            d.Add("nephritis", "Renal");
            d.Add("nephropathy", "Renal");
            d.Add("nerve injury", "Neurology");
            d.Add("neuroblastoma", "");
            d.Add("neurodegenerative disease", "Neurology");
            d.Add("neurofibromatosis", "");
            d.Add("neurology", "Neurology");
            d.Add("neuropathic pain", "Neurology");
            d.Add("neuropathy", "Neurology");
            d.Add("neuroses", "Neurology");
            d.Add("neutropenia", "Blood & Coagulation Disorders");
            d.Add("nicotine addiction", "Neurology");
            d.Add("niemann-pick disease", "");
            d.Add("night blindness", "Ophthalmic");
            d.Add("nocturnal polyuria", "");
            d.Add("non-hodgkin's lymphoma", "Cancer");
            d.Add("non-small cell lung cancer", "Cancer");
            d.Add("norwalk virus", "Infectious Diseases");
            d.Add("nosocomial", "Infectious Diseases");
            d.Add("nutrition", "");
            d.Add("obesity", "Metabolic Disorders");
            d.Add("obsessive compulsive disorder", "Neurology");
            d.Add("obsessive-compulsive disorder", "Neurology");
            d.Add("obstetrics", "Gynecology & Urology");
            d.Add("ocular infection", "Ophthalmic::Infectious Diseases");
            d.Add("ocular inflammation", "Ophthalmic");
            d.Add("oedema", "");
            d.Add("oesophageal cancer", "Cancer");
            d.Add("oesophageal ulcer", "");
            d.Add("oesophagitis", "");
            d.Add("onchocerciasis", "");
            d.Add("onychomycosis", "");
            d.Add("ophthalmic", "Ophthalmic");
            d.Add("opiate addiction", "Neurology");
            d.Add("oral cancer", "Cancer");
            d.Add("organ transplants", "Immune Disorders");
            d.Add("orthopedics", "Orthopedics");
            d.Add("osteoarthritis", "Orthopedics");
            d.Add("osteodystrophy", "");
            d.Add("osteogenesis imperfecta", "");
            d.Add("osteomalacia", "");
            d.Add("osteonecrosis", "");
            d.Add("osteoporosis", "Orthopedics");
            d.Add("osteosarcoma", "Cancer");
            d.Add("otitis", "Ear, Nose & Throat");
            d.Add("ovarian cancer", "Cancer");
            d.Add("overactive bladder", "Gynecology & Urology");
            d.Add("paget's disease", "");
            d.Add("pain", "Neurology");
            d.Add("pancreatic cancer", "Cancer");
            d.Add("pancreatic dysfunction", "Gastrointestinal");
            d.Add("pancreatic insufficiency", "Gastrointestinal");
            d.Add("pancreatitis", "Gastrointestinal");
            d.Add("panic disorder", "Neurology");
            d.Add("parainfluenza virus", "Infectious Diseases");
            d.Add("parasitic", "Infectious Diseases");
            d.Add("parkinson's disease", "Neurology");
            d.Add("partial epilepsy", "Neurology");
            d.Add("parvovirus", "Infectious Diseases");
            d.Add("patent ductus arteriosus", "");
            d.Add("patent foramen ovale(pfo)", "Cardiovascular");
            d.Add("pemphigus", "");
            d.Add("perennial allergic rhinitis", "Immune Disorders");
            d.Add("periodontitis", "Dental-Oral");
            d.Add("peripheral vascular disease", "Cardiovascular");
            d.Add("peritoneal cancer", "Cancer");
            d.Add("pernicious anaemia", "Blood & Coagulation Disorders");
            d.Add("pertussis infection", "Infectious Diseases");
            d.Add("peyronie's disease", "");
            d.Add("pharyngitis", "Ear, Nose & Throat");
            d.Add("phobia", "Neurology");
            d.Add("photodamage", "");
            d.Add("pigmentation disorders", "Dermatology");
            d.Add("plasma substitute", "");
            d.Add("pneumococcal infection", "Infectious Diseases");
            d.Add("pneumocystis jiroveci", "Infectious Diseases");
            d.Add("pneumonia", "Respiratory");
            d.Add("poisoning", "Poisoning");
            d.Add("polio", "Infectious Diseases::Neurology");
            d.Add("pollakisuria", "");
            d.Add("polycystic ovarian syndrome", "");
            d.Add("polycythaemia vera", "");
            d.Add("pompe's disease", "");
            d.Add("porphyria", "");
            d.Add("portal hypertension", "");
            d.Add("post-herpetic pain", "");
            d.Add("post-operative pain", "Neurology");
            d.Add("post-polio syndrome", "Infectious Diseases::Neurology");
            d.Add("post-traumatic stress disorder", "Neurology");
            d.Add("pouchitis", "");
            d.Add("prader-willi syndrome", "");
            d.Add("precocious puberty", "Gynecology & Urology");
            d.Add("pre-eclampsia", "Gynecology & Urology");
            d.Add("premature ejaculation", "Gynecology & Urology");
            d.Add("premenstrual syndrome", "Gynecology & Urology");
            d.Add("presbyopia", "Ophthalmic");
            d.Add("pressure sores", "Wound Healing & Tissue Repair");
            d.Add("preterm labour", "Gynecology & Urology");
            d.Add("progressive multifocal leukoencephalopathy", "Infectious Diseases");
            d.Add("prostate cancer", "Cancer");
            d.Add("proteinuria", "");
            d.Add("pruritus", "");
            d.Add("pseudomonas", "Infectious Diseases");
            d.Add("psoriasis", "Dermatology");
            d.Add("psoriatic arthritis", "Immune Disorders::Dermatology");
            d.Add("psychiatric disorders", "Neurology");
            d.Add("psychosis", "Neurology");
            d.Add("pulmonary", "Respiratory");
            d.Add("pulmonary fibrosis", "Cardiovascular");
            d.Add("pulmonary hypertension", "Cardiovascular");
            d.Add("pulmonary inflammation", "Cardiovascular");
            d.Add("pulmonary thrombosis", "Cardiovascular");
            d.Add("rabies", "Infectious Diseases::Neurology");
            d.Add("radiation poisoning", "Poisoning");
            d.Add("raynaud's disease", "Cardiovascular::Immune Disorders");
            d.Add("refractive errors", "Ophthalmic");
            d.Add("regeneration", "");
            d.Add("rejection", "Immune Disorders");
            d.Add("relapsing-remitting multiple sclerosis", "Neurology");
            d.Add("renal", "Renal");
            d.Add("renal cancer", "Cancer");
            d.Add("renal failure", "Renal");
            d.Add("renal injury", "Renal");
            d.Add("renal ischaemia", "Renal");
            d.Add("reperfusion injury", "Cardiovascular");
            d.Add("respiratory", "Respiratory");
            d.Add("respiratory depression", "Cardiovascular");
            d.Add("respiratory disease", "Cardiovascular");
            d.Add("respiratory distress syndrome", "Respiratory");
            d.Add("respiratory syncytial virus", "Respiratory");
            d.Add("respiratory tract infection", "Cardiovascular");
            d.Add("restenosis", "Cardiovascular");
            d.Add("restless legs syndrome", "Neurology");
            d.Add("retinal detachment", "Ophthalmic");
            d.Add("retinal vein occlusion", "Ophthalmic");
            d.Add("retinitis", "Ophthalmic");
            d.Add("retinopathy", "Ophthalmic");
            d.Add("rhabdomyosarcoma", "Cancer");
            d.Add("rhesus haemolytic disease", "");
            d.Add("rheumatoid arthritis", "Immune Disorders");
            d.Add("rhinitis", "Respiratory");
            d.Add("rhinovirus", "Infectious Diseases");
            d.Add("rickets", "");
            d.Add("rickettsia", "");
            d.Add("rosacea", "Dermatology");
            d.Add("rotavirus", "Infectious Diseases");
            d.Add("rubella", "Infectious Diseases");
            d.Add("salmonella", "Infectious Diseases");
            d.Add("sarcoidosis", "");
            d.Add("sarcoma", "Cancer");
            d.Add("sarcopenia", "");
            d.Add("sars", "Infectious Diseases");
            d.Add("scabies", "");
            d.Add("scarring", "Wound Healing & Tissue Repair");
            d.Add("schistosoma", "");
            d.Add("schistosomiasis", "");
            d.Add("schizophrenia", "Neurology");
            d.Add("scleroderma", "Orthopedics");
            d.Add("seasonal allergic rhinitis", "Immune Disorders");
            d.Add("seborrhoea", "Dermatology");
            d.Add("seborrhoeic eczema", "Dermatology");
            d.Add("senile dementia", "Neurology");
            d.Add("sensory", "");
            d.Add("sepsis", "Infectious Diseases");
            d.Add("severe combined immunodeficiency", "Immune Disorders");
            d.Add("sexual deviations", "Neurology");
            d.Add("sexual health", "Gynecology & Urology");
            d.Add("sexually transmitted diseases", "Infectious Diseases");
            d.Add("shigella", "");
            d.Add("short-bowel syndrome", "Gastrointestinal");
            d.Add("sickle cell anaemia", "Blood & Coagulation Disorders");
            d.Add("sinusitis", "Immune Disorders");
            d.Add("sjogren's syndrome", "Immune Disorders");
            d.Add("skin cancer", "Cancer");
            d.Add("skin disorder", "Dermatology");
            d.Add("skin infection", "Immune Disorders::Infectious Diseases");
            d.Add("skin ulcers", "Wound Healing & Tissue Repair");
            d.Add("sleep apnea", "Respiratory");
            d.Add("sleep disorders", "Neurology");
            d.Add("small bones", "Orthopedics");
            d.Add("small cell lung cancer", "Cancer");
            d.Add("smallpox", "Infectious Diseases");
            d.Add("social anxiety disorder", "Neurology");
            d.Add("soft tissue damage", "Orthopedics");
            d.Add("soft tissue sarcoma", "Cancer");
            d.Add("spastic paralysis", "Neurology");
            d.Add("spinal cord injuries", "Neurology");
            d.Add("spinal cord injury", "Neurology");
            d.Add("spinal muscular atrophy", "Neurology");
            d.Add("spine", "Orthopedics");
            d.Add("spinocerebellar ataxia", "");
            d.Add("squamous cell cancer", "Cancer");
            d.Add("squamous cell carcinoma", "Cancer");
            d.Add("staphylococcal", "Infectious Diseases");
            d.Add("staphylococcus", "Infectious Diseases");
            d.Add("std", "Infectious Diseases::Gynecology & Urology");
            d.Add("steatohepatitis", "Gastrointestinal");
            d.Add("steatorrhoea", "");
            d.Add("stem cell mobilization", "");
            d.Add("sterilization", "Gynecology & Urology");
            d.Add("stomach cancer", "Cancer");
            d.Add("stomatitis", "");
            d.Add("strabismus", "");
            d.Add("streptococcal", "Infectious Diseases");
            d.Add("streptococcus", "Infectious Diseases");
            d.Add("stress urinary incontinence", "Gynecology & Urology");
            d.Add("stroke", "Neurology");
            d.Add("structural heart disease", "Cardiovascular");
            d.Add("subarachnoid haemorrhage", "");
            d.Add("sucrase isomaltase deficiency", "");
            d.Add("supraventricular tachycardia", "Cardiovascular");
            d.Add("surgical wounds", "Wound Healing & Tissue Repair");
            d.Add("synovial sarcoma", "Cancer");
            d.Add("synovitis", "");
            d.Add("systemic inflammatory response syndrome", "");
            d.Add("tachycardia", "Cardiovascular");
            d.Add("tardive dyskinesia", "");
            d.Add("t-cell cancer", "Cancer");
            d.Add("tendinitis", "Orthopedics");
            d.Add("testicular cancer", "Cancer");
            d.Add("tetanus", "Infectious Diseases");
            d.Add("thalassaemia", "Blood & Coagulation Disorders");
            d.Add("thalassemia", "Blood & Coagulation Disorders");
            d.Add("thrombocytopenia", "Blood & Coagulation Disorders");
            d.Add("thrombocytopenic purpura", "Blood & Coagulation Disorders");
            d.Add("thrombocytosis", "Blood & Coagulation Disorders");
            d.Add("thrombophlebitis", "");
            d.Add("thromboprophylaxis", "");
            d.Add("thrombosis", "Cardiovascular");
            d.Add("thymoma", "");
            d.Add("thyroid", "Metabolic Disorders");
            d.Add("thyroid cancer", "Cancer");
            d.Add("tick-borne encephalitis", "Infectious Diseases");
            d.Add("tinnitus", "Ear, Nose & Throat");
            d.Add("tonic-clonic epilepsy", "");
            d.Add("tonsillitis", "Ear, Nose & Throat");
            d.Add("toxoplasmosis", "");
            d.Add("transplant rejection", "Immune Disorders");
            d.Add("transverse myelitis", "");
            d.Add("traumatic brain injury", "Neurology");
            d.Add("trichomoniasis", "");
            d.Add("trypanosomiasis", "");
            d.Add("tuberculosis", "Infectious Diseases");
            d.Add("tumors, liquid", "Cancer");
            d.Add("tumors, solid", "Cancer");
            d.Add("turner's syndrome", "");
            d.Add("type 1 diabetes", "Metabolic Disorders");
            d.Add("type 2 diabetes", "Metabolic Disorders");
            d.Add("type i diabetes", "Metabolic Disorders");
            d.Add("type ii diabetes", "Metabolic Disorders");
            d.Add("typhoid", "Infectious Diseases");
            d.Add("ulcer", "Gastrointestinal");
            d.Add("ulcerative colitis", "Gastrointestinal");
            d.Add("unstable angina", "Cardiovascular");
            d.Add("unverricht-lundborg disease", "");
            d.Add("upper respiratory tract infection", "Cardiovascular::Infectious Diseases");
            d.Add("uraemia", "");
            d.Add("urethritis", "Gynecology & Urology");
            d.Add("urge incontinence", "Gynecology & Urology");
            d.Add("urinary incontinence", "Gynecology & Urology");
            d.Add("urinary retention", "Gynecology & Urology");
            d.Add("urinary tract", "Gynecology & Urology");
            d.Add("urinary tract infection", "Gynecology & Urology");
            d.Add("urticaria", "");
            d.Add("uterine bleeding", "Gynecology & Urology");
            d.Add("uterine cancer", "Cancer");
            d.Add("uterine fibrosis", "Gynecology & Urology");
            d.Add("uveitis", "Ophthalmic");
            d.Add("vaccine adjunct", "Infectious Diseases");
            d.Add("vaginosis", "Gynecology & Urology");
            d.Add("varicella zoster virus", "Infectious Diseases");
            d.Add("varicose veins", "Cardiovascular");
            d.Add("vascular dementia", "Neurology");
            d.Add("vascular endothelial dysfunction", "");
            d.Add("vasospasm", "");
            d.Add("venezuelan equine encephalitis", "Infectious Diseases");
            d.Add("venous insufficiency", "Cardiovascular");
            d.Add("venous stasis ulcers", "Wound Healing & Tissue Repair");
            d.Add("venous thrombosis", "Cardiovascular");
            d.Add("ventricular fibrillation", "Cardiovascular");
            d.Add("ventricular tachycardia", "Cardiovascular");
            d.Add("vertebral compression fractures", "Orthopedics");
            d.Add("vertigo", "Ear, Nose & Throat::Neurology");
            d.Add("vesicoureteral reflux", "");
            d.Add("viral", "Infectious Diseases");
            d.Add("vision correction", "Ophthalmic");
            d.Add("vitamin d deficiency", "Metabolic Disorders");
            d.Add("vitiligo", "");
            d.Add("von willebrand's disease", "");
            d.Add("water retention", "");
            d.Add("wegener's granulomatosis", "");
            d.Add("werner's syndrome", "");
            d.Add("west nile encephalitis", "Infectious Diseases");
            d.Add("western equine encephalitis", "Infectious Diseases");
            d.Add("wilson's disease", "");
            d.Add("wolff-parkinson-white syndrome", "Cardiovascular");
            d.Add("wound", "Wound Healing & Tissue Repair");
            d.Add("wound healing & tissue repair", "Wound Healing & Tissue Repair");
            d.Add("xerophthalmia", "");
            d.Add("xerostomia", "");
            d.Add("yellow fever", "Infectious Diseases");
            d.Add("tersinia pestis", "");
            d.Add("zollinger-ellison syndrome", "");

            return d;
        }

        #endregion Methods

    }

    public class InformaListToRegions : ListToGuid
    {

        public InformaListToRegions(Item i) : base(i) { }

        #region Methods

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
        {
            if (string.IsNullOrEmpty(importValue))
                return;

            //get parent item of list to search
            var sourceItems = GetSourceItems(newItem.Database);
            if (sourceItems == null)
                return;

            Dictionary<string, string> d = GetMapping();

            var values = importValue.Split(GetFieldValueDelimiter()?[0] ?? ',');

            foreach (var val in values)
            {
                string upperValue = val.ToString();
                string transformValue = (d.ContainsKey(upperValue)) ? d[upperValue] : string.Empty;
                if (string.IsNullOrEmpty(transformValue))
                {
                    map.Logger.Log(newItem.Paths.FullPath, "Region not converted", ProcessStatus.FieldError, NewItemField, val);
                    continue;
                }

                string cleanName = StringUtility.GetValidItemName(transformValue, map.ItemNameMaxLength);
                IEnumerable<Item> t = sourceItems.Where(c => c.Name.Equals(cleanName));

                //if you find one then store the id
                if (!t.Any())
                {
                    map.Logger.Log(newItem.Paths.FullPath, "Region(s) not found in list", ProcessStatus.FieldError, NewItemField, val);
                    continue;
                }

                Field f = newItem.Fields[NewItemField];
                if (f == null)
                    continue;

                if (NewItemField == "Taxonomy")
                {
                    TaxonomyList.Add(t.First().ID.ToString());
                }
            }
        }



        public virtual Dictionary<string, string> GetMapping()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("Middle East & Africa", "Middle East & Africa");
            d.Add("Afghanistan", "Afghanistan");
            d.Add("Algeria", "Algeria");
            d.Add("Angola", "Angola");
            d.Add("Bahrain", "Bahrain");
            d.Add("Benin", "Benin");
            d.Add("Botswana", "Botswana");
            d.Add("Burkina Faso", "Burkina Faso");
            d.Add("Burundi", "Burundi");
            d.Add("Cabo Verde", "Cabo Verde");
            d.Add("Cameroon", "Cameroon");
            d.Add("Central African Republic", "Central African Republic");
            d.Add("Chad", "Chad");
            d.Add("Comoros", "Comoros");
            d.Add("Congo", "Congo");
            d.Add("Congo (Democratic Republic)", "Congo (Democratic Republic)");
            d.Add("Côte d'Ivoire", "Côte d'Ivoire");
            d.Add("Djibouti", "Djibouti");
            d.Add("Egypt", "Egypt");
            d.Add("Equatorial Guinea", "Equatorial Guinea");
            d.Add("Eritrea", "Eritrea");
            d.Add("Ethiopia", "Ethiopia");
            d.Add("Gabon", "Gabon");
            d.Add("Gambia", "Gambia");
            d.Add("Ghana", "Ghana");
            d.Add("Guinea", "Guinea");
            d.Add("Guinea-Bissau", "Guinea-Bissau");
            d.Add("Iran", "Iran");
            d.Add("Iraq", "Iraq");
            d.Add("Israel", "Israel");
            d.Add("Jordan", "Jordan");
            d.Add("Kenya", "Kenya");
            d.Add("Kuwait", "Kuwait");
            d.Add("Lebanon", "Lebanon");
            d.Add("Lesotho", "Lesotho");
            d.Add("Liberia", "Liberia");
            d.Add("Libya", "Libya");
            d.Add("Madagascar", "Madagascar");
            d.Add("Malawi", "Malawi");
            d.Add("Mali", "Mali");
            d.Add("Mauritania", "Mauritania");
            d.Add("Mauritius", "Mauritius");
            d.Add("Mayotte", "Mayotte");
            d.Add("Morocco", "Morocco");
            d.Add("Mozambique", "Mozambique");
            d.Add("Namibia", "Namibia");
            d.Add("Niger", "Niger");
            d.Add("Nigeria", "Nigeria");
            d.Add("Oman", "Oman");
            d.Add("Palestine", "Palestine");
            d.Add("Qatar", "Qatar");
            d.Add("Réunion", "Réunion");
            d.Add("Rwanda", "Rwanda");
            d.Add("Saint Helena, Ascension and Tristan da Cunha", "Saint Helena, Ascension and Tristan da Cunha");
            d.Add("Sao Tome and Principe", "Sao Tome and Principe");
            d.Add("Saudi Arabia", "Saudi Arabia");
            d.Add("Senegal", "Senegal");
            d.Add("Seychelles", "Seychelles");
            d.Add("Sierra Leone", "Sierra Leone");
            d.Add("Somalia", "Somalia");
            d.Add("South Africa", "South Africa");
            d.Add("South Sudan", "South Sudan");
            d.Add("Sudan", "Sudan");
            d.Add("Swaziland", "Swaziland");
            d.Add("Syria", "Syria");
            d.Add("Tanzania", "Tanzania");
            d.Add("Togo", "Togo");
            d.Add("Tunisia", "Tunisia");
            d.Add("Turkey", "Turkey");
            d.Add("Uganda", "Uganda");
            d.Add("United Arab Emirates", "United Arab Emirates");
            d.Add("Western Sahara", "Western Sahara");
            d.Add("Yemen", "Yemen");
            d.Add("Zambia", "Zambia");
            d.Add("Zimbabwe", "Zimbabwe");
            d.Add("Asia Pacific", "Asia Pacific");
            d.Add("American Samoa", "American Samoa");
            d.Add("Australia", "Australia");
            d.Add("Bangladesh", "Bangladesh");
            d.Add("Bhutan", "Bhutan");
            d.Add("British Indian Ocean Territory", "British Indian Ocean Territory");
            d.Add("Brunei Darussalam", "Brunei Darussalam");
            d.Add("Cambodia", "Cambodia");
            d.Add("China", "China");
            d.Add("Christmas Island", "Christmas Island");
            d.Add("Cocos (Keeling) Islands", "Cocos (Keeling) Islands");
            d.Add("Cook Islands", "Cook Islands");
            d.Add("Fiji", "Fiji");
            d.Add("French Polynesia", "French Polynesia");
            d.Add("Georgia", "Georgia");
            d.Add("Guam", "Guam");
            d.Add("Heard Island and McDonald Islands", "Heard Island and McDonald Islands");
            d.Add("Hong Kong", "Hong Kong");
            d.Add("India", "India");
            d.Add("Indonesia", "Indonesia");
            d.Add("Japan", "Japan");
            d.Add("Kazakhstan", "Kazakhstan");
            d.Add("Kiribati", "Kiribati");
            d.Add("Kyrgyzstan", "Kyrgyzstan");
            d.Add("Laos", "Laos");
            d.Add("Macao", "Macao");
            d.Add("Malaysia", "Malaysia");
            d.Add("Maldives", "Maldives");
            d.Add("Marshall Islands", "Marshall Islands");
            d.Add("Micronesia", "Micronesia");
            d.Add("Mongolia", "Mongolia");
            d.Add("Myanmar", "Myanmar");
            d.Add("Nauru", "Nauru");
            d.Add("Nepal", "Nepal");
            d.Add("New Caledonia", "New Caledonia");
            d.Add("New Zealand", "New Zealand");
            d.Add("Niue", "Niue");
            d.Add("Norfolk Island", "Norfolk Island");
            d.Add("North Korea", "North Korea");
            d.Add("Northern Mariana Islands", "Northern Mariana Islands");
            d.Add("Pakistan", "Pakistan");
            d.Add("Palau", "Palau");
            d.Add("Papua New Guinea", "Papua New Guinea");
            d.Add("Philippines", "Philippines");
            d.Add("Pitcairn", "Pitcairn");
            d.Add("Russian Federation", "Russian Federation");
            d.Add("Samoa", "Samoa");
            d.Add("Singapore", "Singapore");
            d.Add("Solomon Islands", "Solomon Islands");
            d.Add("South Korea", "South Korea");
            d.Add("Sri Lanka", "Sri Lanka");
            d.Add("Taiwan", "Taiwan");
            d.Add("Tajikistan", "Tajikistan");
            d.Add("Thailand", "Thailand");
            d.Add("Timor-Leste", "Timor-Leste");
            d.Add("Tokelau", "Tokelau");
            d.Add("Tonga", "Tonga");
            d.Add("Turkmenistan", "Turkmenistan");
            d.Add("Tuvalu", "Tuvalu");
            d.Add("Uzbekistan", "Uzbekistan");
            d.Add("Vanuatu", "Vanuatu");
            d.Add("Vietnam", "Vietnam");
            d.Add("Wallis and Futuna", "Wallis and Futuna");
            d.Add("Europe", "Europe");
            d.Add("Åland Islands", "Åland Islands");
            d.Add("Albania", "Albania");
            d.Add("Andorra", "Andorra");
            d.Add("Armenia", "Armenia");
            d.Add("Austria", "Austria");
            d.Add("Azerbaijan", "Azerbaijan");
            d.Add("Belarus", "Belarus");
            d.Add("Belgium", "Belgium");
            d.Add("Bosnia And Herzegovina", "Bosnia And Herzegovina");
            d.Add("Bouvet Island", "Bouvet Island");
            d.Add("Bulgaria", "Bulgaria");
            d.Add("Croatia", "Croatia");
            d.Add("Cyprus", "Cyprus");
            d.Add("Czech Republic", "Czech Republic");
            d.Add("Denmark", "Denmark");
            d.Add("Estonia", "Estonia");
            d.Add("Faroe Islands", "Faroe Islands");
            d.Add("Finland", "Finland");
            d.Add("France", "France");
            d.Add("Germany", "Germany");
            d.Add("Gibraltar", "Gibraltar");
            d.Add("Greece", "Greece");
            d.Add("Guernsey", "Guernsey");
            d.Add("Holy See", "Holy See");
            d.Add("Hungary", "Hungary");
            d.Add("Iceland", "Iceland");
            d.Add("Ireland", "Ireland");
            d.Add("Isle of Man", "Isle of Man");
            d.Add("Italy", "Italy");
            d.Add("Jersey", "Jersey");
            d.Add("Latvia", "Latvia");
            d.Add("Liechtenstein", "Liechtenstein");
            d.Add("Lithuania", "Lithuania");
            d.Add("Luxembourg", "Luxembourg");
            d.Add("Macedonia", "Macedonia");
            d.Add("Malta", "Malta");
            d.Add("Moldova", "Moldova");
            d.Add("Monaco", "Monaco");
            d.Add("Montenegro", "Montenegro");
            d.Add("Netherlands", "Netherlands");
            d.Add("Norway", "Norway");
            d.Add("Poland", "Poland");
            d.Add("Portugal", "Portugal");
            d.Add("Romania", "Romania");
            d.Add("San Marino", "San Marino");
            d.Add("Serbia", "Serbia");
            d.Add("Slovakia", "Slovakia");
            d.Add("Slovenia", "Slovenia");
            d.Add("Spain", "Spain");
            d.Add("Svalbard and Jan Mayen", "Svalbard and Jan Mayen");
            d.Add("Sweden", "Sweden");
            d.Add("Switzerland", "Switzerland");
            d.Add("Ukraine", "Ukraine");
            d.Add("United Kingdom", "United Kingdom");
            d.Add("North America", "North America");
            d.Add("Anguilla", "Anguilla");
            d.Add("Antigua And Barbuda", "Antigua And Barbuda");
            d.Add("Aruba", "Aruba");
            d.Add("Bahamas", "Bahamas");
            d.Add("Barbados", "Barbados");
            d.Add("Belize", "Belize");
            d.Add("Bermuda", "Bermuda");
            d.Add("Bonaire, Sint Eustatius and Saba", "Bonaire, Sint Eustatius and Saba");
            d.Add("Canada", "Canada");
            d.Add("Cayman Islands", "Cayman Islands");
            d.Add("Costa Rica", "Costa Rica");
            d.Add("Cuba", "Cuba");
            d.Add("Curaçao", "Curaçao");
            d.Add("Dominica", "Dominica");
            d.Add("Dominican Republic", "Dominican Republic");
            d.Add("El Salvador", "El Salvador");
            d.Add("Greenland", "Greenland");
            d.Add("Grenada", "Grenada");
            d.Add("Guadeloupe", "Guadeloupe");
            d.Add("Guatemala", "Guatemala");
            d.Add("Haiti", "Haiti");
            d.Add("Honduras", "Honduras");
            d.Add("Jamaica", "Jamaica");
            d.Add("Martinique", "Martinique");
            d.Add("Mexico", "Mexico");
            d.Add("Montserrat", "Montserrat");
            d.Add("Nicaragua", "Nicaragua");
            d.Add("Panama", "Panama");
            d.Add("Puerto Rico", "Puerto Rico");
            d.Add("Saint Barthélemy", "Saint Barthélemy");
            d.Add("Saint Kitts and Nevis", "Saint Kitts and Nevis");
            d.Add("Saint Lucia", "Saint Lucia");
            d.Add("Saint Martin (French)", "Saint Martin (French)");
            d.Add("Saint Pierre and Miquelon", "Saint Pierre and Miquelon");
            d.Add("Saint Vincent and the Grenadines", "Saint Vincent and the Grenadines");
            d.Add("Sint Maarten (Dutch)", "Sint Maarten (Dutch)");
            d.Add("Trinidad And Tobago", "Trinidad And Tobago");
            d.Add("Turks and Caicos Islands", "Turks and Caicos Islands");
            d.Add("United States", "United States");
            d.Add("United States Minor Outlying Islands", "United States Minor Outlying Islands");
            d.Add("Virgin Islands (British)", "Virgin Islands (British)");
            d.Add("Virgin Islands (U.S.)", "Virgin Islands (U.S.)");
            d.Add("South America", "South America");
            d.Add("Argentina", "Argentina");
            d.Add("Bolivia", "Bolivia");
            d.Add("Brazil", "Brazil");
            d.Add("Chile", "Chile");
            d.Add("Colombia", "Colombia");
            d.Add("Ecuador", "Ecuador");
            d.Add("Falkland Islands (Malvinas)", "Falkland Islands (Malvinas)");
            d.Add("French Guiana", "French Guiana");
            d.Add("Guyana", "Guyana");
            d.Add("Paraguay", "Paraguay");
            d.Add("Peru", "Peru");
            d.Add("Suriname", "Suriname");
            d.Add("Uruguay", "Uruguay");
            d.Add("Venezuela", "Venezuela");
            d.Add("Antarctica", "Antarctica");
            d.Add("French Southern Territories", "French Southern Territories");
            d.Add("South Georgia and the South Sandwich Islands", "South Georgia and the South Sandwich Islands");
            d.Add("International", "International");
            d.Add("US States", "US States");
            d.Add("Blocs", "Blocs");
            d.Add("EU", "Europe");
            d.Add("EEA", "EEA");
            d.Add("EFTA", "EFTA");
            d.Add("NAFTA", "NAFTA");
            d.Add("Mercosur", "Mercosur");
            d.Add("ASEAN", "ASEAN");
            d.Add("Ivory Coast", "Côte d'Ivoire");
            d.Add("Democratic Republic of Congo", "Democratic Republic of Congo");
            d.Add("UAE", "United Arab Emirates");
            d.Add("Burma", "Myanmar");
            d.Add("PNG", "Papua New Guinea");
            d.Add("Russia", "Russian Federation");
            d.Add("Ceylon", "Sri Lanka");
            d.Add("Viet Nam", "Vietnam");
            d.Add("UK", "United Kingdom");
            d.Add("US", "United States");
            d.Add("USA", "United States");
            d.Add("NA", "New Zealand");


            return d;
        }

        #endregion Methods

    }

    public class ToInformaSubjects : ListToGuid
    {
        public ToInformaSubjects(Item i) : base(i) { }

        #region Methods

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
        {
            if (string.IsNullOrWhiteSpace(importValue))
                return;

            var sourceItems = GetSourceItems(newItem.Database);
            if (sourceItems == null)
                return;

            Field f = newItem.Fields[NewItemField];
            if (f == null)
                return;

            Dictionary<string, string> d = GetMapping();

            var values = importValue.Split(GetFieldValueDelimiter()?[0] ?? ',');

            foreach (var value in values)
            {
                string lowerValue = value.ToLower();
                string transformValue = (d.ContainsKey(lowerValue)) ? d[lowerValue] : string.Empty;
                if (string.IsNullOrEmpty(transformValue))
                {
                    map.Logger.Log(newItem.Paths.FullPath, "Subject(s) not converted", ProcessStatus.FieldError, NewItemField, value);
                    continue;
                }

                string[] parts = transformValue.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries);

                //loop through children and look for anything that matches by name
                foreach (string area in parts)
                {
                    string cleanName = StringUtility.GetValidItemName(area, map.ItemNameMaxLength);
                    IEnumerable<Item> t = sourceItems.Where(c => c.DisplayName.Equals(cleanName));

                    //if you find one then store the id
                    if (!t.Any())
                    {
                        map.Logger.Log(newItem.Paths.FullPath, "Subject(s) not found in list", ProcessStatus.FieldError, NewItemField, area);
                        continue;
                    }

                    string ctID = t.First().ID.ToString();
                    if (!f.Value.Contains(ctID))
                        f.Value = (f.Value.Length > 0) ? $"{f.Value}|{ctID}" : ctID;
                }
            }
        }

        public virtual Dictionary<string, string> GetMapping()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();

            d.Add("1st quarter", "Companies");
            d.Add("2nd quarter", "Companies");
            d.Add("3rd quarter", "Companies");
            d.Add("4th quarter", "Companies");
            d.Add("access to medicines", "Market Access");
            d.Add("acquisition", "Deals");
            d.Add("active pharmaceutical ingredient", "");
            d.Add("additional data request", "Approvals");
            d.Add("advanced therapies", "Therapy Areas");
            d.Add("adverse drug reactions", "Post-Marketing");
            d.Add("adverse event reporting", "Post-Marketing");
            d.Add("advertising", "Advertising & Marketing");
            d.Add("agm", "");
            d.Add("annual", "Companies");
            d.Add("antitrust", "Companies::Policy");
            d.Add("benefit/risk assessment", "Approvals");
            d.Add("bill", "Legislation");
            d.Add("bills", "Legislation");
            d.Add("biological", "");
            d.Add("biosimilar medicines", "Policy::Regulation");
            d.Add("biosimilars", "Policy::Regulation");
            d.Add("board appointment", "Companies");
            d.Add("branded medicines", "");
            d.Add("business practice", "Companies");
            d.Add("business", "Companies");
            d.Add("chapter 11", "Companies");
            d.Add("clinical trial results", "Clinical Trials");
            d.Add("clinical trials", "Clinical Trials");
            d.Add("code of conduct", "");
            d.Add("commercial (sales, marketing, promotion and distribution)", "Companies");
            d.Add("company deals", "");
            d.Add("congress", "Regulation");
            d.Add("contract sales", "");
            d.Add("court case", "");
            d.Add("critical path", "");
            d.Add("cutbacks", "Companies");
            d.Add("deals", "Deals");
            d.Add("democrats", "Regulation");
            d.Add("directives", "Regulation");
            d.Add("divestment", "Companies");
            d.Add("domestic provision", "Policy");
            d.Add("draft legislation", "Policy");
            d.Add("drug combination/formulation approval", "Approvals");
            d.Add("drug combination/formulation filing", "Approvals");
            d.Add("drug combination/formulation launch", "Approvals");
            d.Add("drug label changes", "Approvals");
            d.Add("egm", "");
            d.Add("enforcement", "");
            d.Add("essential drugs", "Policy");
            d.Add("eu chmp negative opinion", "Regulation");
            d.Add("eu chmp opinion", "");
            d.Add("eu chmp positive opinion", "Regulation");
            d.Add("events", "Events");
            d.Add("fda advisory panel", "");
            d.Add("fda advisory panel meeting", "Approvals");
            d.Add("fda approvable letter", "");
            d.Add("fda complete response letter", "Regulation");
            d.Add("fda non-approvable letter", "");
            d.Add("financial updates", "Companies");
            d.Add("financials", "Companies");
            d.Add("forecast", "Companies");
            d.Add("foreign aid", "Policy");
            d.Add("funding", "Funding");
            d.Add("general approval submission", "");
            d.Add("generic approval", "Approvals");
            d.Add("generic approval filing", "Approvals");
            d.Add("generic competition", "Strategy");
            d.Add("generic launch", "Companies");
            d.Add("generic substitution", "Strategy::Regulation");
            d.Add("generics", "Strategy::Regulation");
            d.Add("geographical expansion", "Strategy::Companies");
            d.Add("good manufacturing practice", "Regulation");
            d.Add("guidance", "Companies");
            d.Add("guidelines", "");
            d.Add("half-year", "Companies");
            d.Add("harmonisation", "Policy::Regulation");
            d.Add("headquarters", "");
            d.Add("health insurance", "");
            d.Add("healthcare budgets", "Market Access::Policy");
            d.Add("healthcare provision", "");
            d.Add("healthcare systems", "Policy::Market Access");
            d.Add("herbal medicines", "");
            d.Add("house of representatives", "Regulation");
            d.Add("hta", "Health Technology Assessment");
            d.Add("infringement", "Regulation");
            d.Add("injunction", "Regulation");
            d.Add("innovative medicines initiative", "Policy");
            d.Add("inspections", "Regulation");
            d.Add("internet trade", "");
            d.Add("investment", "Companies");
            d.Add("ip", "Companies");
            d.Add("ipo", "Companies");
            d.Add("joint venture", "Companies::Deals");
            d.Add("kickbacks", "");
            d.Add("lawsuit", "Companies");
            d.Add("legal", "");
            d.Add("legislation", "Policy::Legislation");
            d.Add("licensing", "Companies::Deals");
            d.Add("litigation", "Companies");
            d.Add("loan", "Companies::Deals");
            d.Add("lobbying", "Regulation");
            d.Add("manufacturing", "Companies");
            d.Add("market data", "Markets::Market Intelligence");
            d.Add("market statistics", "Market Access");
            d.Add("marketing withdrawal", "Market Access");
            d.Add("medicaid", "Market Access");
            d.Add("medical ethics", "");
            d.Add("medical records", "");
            d.Add("medicare", "Market Access");
            d.Add("merger", "Companies::Deals");
            d.Add("nas/nce approval", "Approvals");
            d.Add("nas/nce approval filing", "Approvals");
            d.Add("nas/nce launch", "Approvals");
            d.Add("new appointment", "Companies");
            d.Add("new molecular entity", "");
            d.Add("ngo alliances", "");
            d.Add("nhs policy", "");
            d.Add("nine-month", "");
            d.Add("open offer", "");
            d.Add("orphan drugs", "Approvals");
            d.Add("outsourcing", "");
            d.Add("over-the-counter", "Markets");
            d.Add("paediatric medicines", "Approvals");
            d.Add("paediatric trials", "Clinical Trials");
            d.Add("parallel trade", "Regulation::Strategy");
            d.Add("patient information", "Clinical Trials");
            d.Add("pBMs", "");
            d.Add("pharma growth", "");
            d.Add("pharmaceutical promotions", "");
            d.Add("pharmacovigilance", "Post-Marketing::Regulation");
            d.Add("pipeline", "Clinical Trials::Approvals");
            d.Add("pipeline update", "Clinical Trials::Approvals");
            d.Add("placing", "Companies::Deals");
            d.Add("postmarketing", "Post-Marketing");
            d.Add("preclinical trials", "");
            d.Add("president", "");
            d.Add("prevention", "Policy");
            d.Add("pricing", "");
            d.Add("private financing", "Companies::Deals");
            d.Add("private healthcare", "");
            d.Add("product pricing and reimbursement", "");
            d.Add("product approvals and launches", "");
            d.Add("product delays and withdrawals", "");
            d.Add("product discontinuation", "Strategy::Companies");
            d.Add("product filing withdrawal", "Approvals");
            d.Add("product regulatory submission", "");
            d.Add("product safety", "");
            d.Add("product types", "");
            d.Add("professional discipline", "");
            d.Add("promotion", "Companies");
            d.Add("promotions", "");
            d.Add("public financing", "Companies::Deals");
            d.Add("r&d", "Companies");
            d.Add("rebates", "");
            d.Add("reforms", "Policy");
            d.Add("regulations", "Regulation");
            d.Add("regulatory agencies", "Regulation");
            d.Add("reimbursement", "Market Access");
            d.Add("relocation", "");
            d.Add("reorganisation", "Companies");
            d.Add("republicans", "Regulation");
            d.Add("research", "Clinical Trials");
            d.Add("resignation", "Companies");
            d.Add("restructuring", "Companies");
            d.Add("retirement", "Companies");
            d.Add("reverse payments", "");
            d.Add("reverse takeover", "Companies");
            d.Add("review extension", "Approvals");
            d.Add("rights issue", "");
            d.Add("scientific advice", "Approvals");
            d.Add("senate", "Regulation");
            d.Add("setting", "Policy");
            d.Add("settlement", "Companies");
            d.Add("shareholder", "");
            d.Add("spin out", "Companies::Deals");
            d.Add("start-ups", "Companies");
            d.Add("stockpiling", "Strategy");
            d.Add("supplemental approval", "Approvals");
            d.Add("supplemental approval filing", "Approvals");
            d.Add("technology", "Policy");
            d.Add("tentative approval", "Approvals");
            d.Add("termination", "Companies");
            d.Add("third party payers", "Market Access");
            d.Add("trade", "Companies::Strategy");
            d.Add("trademarks", "Companies");
            d.Add("vaccination programmes", "Policy::Market Access");
            d.Add("whistleblower", "");
            d.Add("white house", "Regulation");

            return d;
        }

        #endregion Methods

    }

    #endregion Scrip

    #region PMBI

    public class ToLegacySitecoreId : ToText
    {
        public ToLegacySitecoreId(Item i) : base(i) { }

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
        {
            Field f = newItem.Fields[NewItemField];
            f.Value = id;
        }
    }

    public class ToLegacyArticlePath : ToText
    {
        public ToLegacyArticlePath(Item i) : base(i)
        {
        }

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
        {
            Field f = newItem.Fields[NewItemField];
            var item = Database.GetDatabase("pmbiContent").GetItem(new ID(id));
            f.Value = item.Paths.FullPath.ToLower();
        }
    }

    public class ToArticleNumber : ToText
    {
        public ToArticleNumber(Item i) : base(i)
        {
        }

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
        {
            var pmbiDatamap = map as PmbiDataMap;
            if (pmbiDatamap != null)
            {
                var f = newItem.Fields[NewItemField];
                var prefix = pmbiDatamap.ArticleNumberPrefix;
                f.Value = $"{prefix}{(++pmbiDatamap.ArticleNumber).ToString("D6")}";
            }
        }
    }

    public class ToRegion : ListToGuid
    {
        public ToRegion(Item i) : base(i)
        {
        }

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
        {
            var mapping = TaxonomyMapping.RegionMapping;
            FillTaxonomyField(map, ref newItem, importValue, mapping);
        }
    }

    public class ToTherapeuticCategory : ListToGuid
    {
        public ToTherapeuticCategory(Item i) : base(i)
        {
        }

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
        {
            var mapping = new Dictionary<string, string>();
            // Article to import
            var pmbiItem = Sitecore.Data.Database.GetDatabase("pmbiContent").GetItem(new ID(id));
            // Taxonomy folder in pmbi database
            var oldSourceList = Sitecore.Data.Database.GetDatabase("pmbiContent").GetItem(OldSourceList);

            if (pmbiItem != null)
            {
                var tcVal = pmbiItem.Fields["Industries"].Value.Split('|');

                // If current article is categorized as Biopharmaceuticals or Consumer Products
                if (tcVal.Contains("{CAC059FE-41BA-403C-B36F-BFEBF3DC16ED}") ||
                    tcVal.Contains("{608C58C2-6268-4B6B-908B-1D3F5E637016}"))
                {
                    mapping = TaxonomyMapping.PharmaTherapyMapping;
                }
                // If current article is categorized as Medical Device
                else if (tcVal.Contains("{1CEB75FF-46C5-4A8E-90AF-530883EE3C89}"))
                {
                    mapping = TaxonomyMapping.DeviceMarketMapping;
                }
                else
                {
                    return;
                }

                FillTaxonomyField(map, ref newItem, importValue, mapping);
            }
        }
    }

    public class ToContentType : ListToGuid
    {
        public ToContentType(Item i) : base(i)
        {
        }

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
        {
            var pmbiItem = Sitecore.Data.Database.GetDatabase("pmbiContent").GetItem(new ID(id));
            var templateName = pmbiItem.TemplateName;
            var mapping = TaxonomyMapping.ContentTypeMapping.ContainsKey(templateName)
                ? TaxonomyMapping.ContentTypeMapping[templateName]
                : null;
            if (mapping != null)
            {
                FillTaxonomyField(map, ref newItem, importValue, mapping);
            }
        }
    }

    public class ToSubject : ListToGuid
    {
        public ToSubject(Item i) : base(i)
        {
        }

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
        {
            var mapping = TaxonomyMapping.SubjectMapping;
            FillTaxonomyField(map, ref newItem, importValue, mapping);
        }
    }

    public class ToIndustry : ListToGuid
    {
        public ToIndustry(Item i) : base(i)
        {
        }

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
        {
            var mapping = TaxonomyMapping.IndustryMapping;
            FillTaxonomyField(map, ref newItem, importValue, mapping);
        }
    }

    public class ToMediaType : ListToGuid
    {
        public ToMediaType(Item i) : base(i)
        {
        }

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
        {
            var pmbiItem = Sitecore.Data.Database.GetDatabase("pmbiContent").GetItem(new ID(id));
            var field = newItem.Fields[NewItemField];
            var item = newItem.Database.GetItem(SourceList);

            if (pmbiItem != null && item != null)
            {
                var audioVal = item.GetChildren().FirstOrDefault(i => i.Fields["Item Name"].Value == "Audio")?.ID.ToString();
                var videoVal = item.GetChildren().FirstOrDefault(i => i.Fields["Item Name"].Value == "Video")?.ID.ToString();

                if (importValue.StartsWith("Podcast"))
                {
                    field.Value = audioVal;
                }

                if (importValue.StartsWith("Video"))
                {
                    field.Value = videoVal;
                }
            }
        }
    }

    public class ToAuthors : ListToGuid
    {
        public ToAuthors(Item i) : base(i)
        {
        }

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
        {
            var field = newItem.Fields[NewItemField];
            if (!string.IsNullOrWhiteSpace(importValue) && field != null)
            {
                var item = newItem.Database.GetItem(SourceList);
                if (item != null)
                {
                    var strs = importValue.Split('|');
                    var transformedValue = string.Empty;
                    var descendants = item.Axes.GetDescendants();

                    foreach (var str in strs)
                    {
                        var authorItem = Database.GetDatabase("pmbiContent").GetItem(new ID(str));

                        if (authorItem == null)
                        {
                            map.Logger.Log(newItem.Paths.FullPath, "Could not find matching Author item in PMBI for given ID", ProcessStatus.FieldError, NewItemField, str);
                            continue;
                        }

                        var firstName = StringUtility.TrimInvalidChars(authorItem.Fields["First Name"].Value.ToLower());
                        var lastName = StringUtility.TrimInvalidChars(authorItem.Fields["Last Name"].Value.ToLower());
                        var email = StringUtility.TrimInvalidChars(authorItem.Fields["Email"].Value.ToLower());

                        var valCollection = descendants.Where(
                            i =>
                                StringUtility.TrimInvalidChars(i?.Fields["First Name"]?.Value?.ToLower() ?? string.Empty) == firstName
                                && StringUtility.TrimInvalidChars(i?.Fields["Last Name"]?.Value?.ToLower() ?? string.Empty) == lastName
                                && (string.IsNullOrWhiteSpace(i?.Fields["Email Address"]?.Value) || string.IsNullOrWhiteSpace(email) || StringUtility.TrimInvalidChars(i.Fields["Email Address"].Value.ToLower()) == email)).ToList();

                        if (valCollection.Count == 0)
                        {
                            var staffitem = item.Add(ItemUtil.ProposeValidItemName($"{firstName} {lastName}"), new TemplateID(IStaff_ItemConstants.TemplateId));
                            using (new EditContext(staffitem))
                            {
                                staffitem.Fields[IStaff_ItemConstants.First_NameFieldName].Value = authorItem.Fields["First Name"].Value;
                                staffitem.Fields[IStaff_ItemConstants.Last_NameFieldName].Value = authorItem.Fields["Last Name"].Value;
                                staffitem.Fields[IStaff_ItemConstants.Email_AddressFieldName].Value = authorItem.Fields["Email"].Value;
                            }

                            valCollection = new List<Item> { staffitem };
                        }

                        if (valCollection.Count > 1)
                        {
                            map.Logger.Log(newItem.Paths.FullPath, "Find more than 1 authors in target DB", ProcessStatus.FieldError, NewItemField, str, $"Name:{firstName} {lastName}--Email: {email}");
                            continue;
                        }

                        var val = valCollection.FirstOrDefault()?.ID.ToString();
                        if (string.IsNullOrWhiteSpace(val))
                        {
                            map.Logger.Log(newItem.Paths.FullPath, $"{FieldName}(s) not found in target DB", ProcessStatus.FieldError, NewItemField, str, $"Name:{firstName} {lastName}--Email: {email}");
                            continue;
                        }

                        // Avoid adding duplicate GUID
                        if (!field.Value.Contains(val))
                        {
                            transformedValue = string.IsNullOrWhiteSpace(transformedValue) ? val : $"{transformedValue}|{val}";
                        }
                    }
                    field.Value = transformedValue;
                }
            }
        }
    }

    public static class TaxonomyMapping
    {
        public static Dictionary<string, string> RegionMapping => new Dictionary<string, string>
        {
            {"Africa", "Africa"},
            {"Argentina", "Argentina"},
            {"Asia-Pacific", "Asia"},
            {"Australia", "Australia"},
            {"Austria", "Austria"},
            {"Belgium", "Belgium"},
            {"Brazil", "Brazil"},
            {"Canada", "Canada"},
            {"Caribbean", "Americas"},
            {"Chile", "Chile"},
            {"China", "China"},
            {"Colombia", "Colombia"},
            {"Czech Republic", "Czech Republic"},
            {"Denmark", "Denmark"},
            {"Egypt", "Egypt"},
            {"Europe", "Europe"},
            {"Finland", "Finland"},
            {"France", "France"},
            {"Germany", "Germany"},
            {"Greece", "Greece"},
            {"India", "India"},
            {"Indonesia", "Indonesia"},
            {"Ireland", "Ireland"},
            {"Israel", "Israel"},
            {"Italy", "Italy"},
            {"Japan", "Japan"},
            {"Jordan", "Jordan"},
            {"Latin America", "Americas"},
            {"Lebanon", "Lebanon"},
            {"Malaysia", "Malaysia"},
            {"MENA", ""},
            {"Mexico", "Mexico"},
            {"Middle East", "Middle East"},
            {"Morocco", "Morocco"},
            {"Netherlands", "Netherlands"},
            {"New Zealand", "New Zealand"},
            {"North Africa", "North Africa"},
            {"North America", "North America"},
            {"North Korea", "North Korea"},
            {"Norway", "Norway"},
            {"Pacific Rim", "Asia"},
            {"Pakistan", "Pakistan"},
            {"Paraguay", "Paraguay"},
            {"Philippines", "Philippines"},
            {"Poland", "Poland"},
            {"Portugal", "Portugal"},
            {"Russia", "Russia"},
            {"Saudi Arabia", "Saudi Arabia"},
            {"Singapore", "Singapore"},
            {"Slovenia", "Slovenia"},
            {"South Africa", "South Africa"},
            {"South America", "Americas"},
            {"South Korea", "South Korea"},
            {"Spain", "Spain"},
            {"Sweden", "Sweden"},
            {"Switzerland", "Switzerland"},
            {"Taiwan", "Taiwan"},
            {"Thailand", "Thailand"},
            {"Turkey", "Turkey"},
            {"United Kingdom", "United Kingdom"},
            {"United States", "United States"},
            {"Uruguay", "Uruguay"},
            {"Venezeula", "Venezeula"}
        };

        public static Dictionary<string, string> PharmaTherapyMapping => new Dictionary<string, string>
        {
            {"Aesthetics", "Aesthetics"},
            {"Blood & Coagulation Disorders", "Blood & Coagulation Disorders"},
            {"Cancer", "Cancer"},
            {"Cardiovascular", "Cardiovascular"},
            {"Dental - Oral", "Dental - Oral"},
            {"Dermatology", "Dermatology"},
            {"ENT", "Ear, Nose & Throat"},
            {"Gastrointestinal", "Gastrointestinal"},
            {"Gynecology and Urology", "Gynecology & Urology"},
            {"Immune Disorders", "Immune Disorders"},
            {"Infectious Diseases", "Infectious Diseases"},
            {"Inflammation", "Inflammation"},
            {"Liver and Hepatic", "Liver & Hepatic"},
            {"Metabolic Disorders", "Metabolic Disorders"},
            {"Neurology", "Neurology"},
            {"Ophthalmic", "Ophthalmic"},
            {"Orthopedics", "Orthopedics"},
            {"Poisoning", "Poisoning"},
            {"Renal", "Renal"},
            {"Respiratory", "Respiratory"},
            {"Wound Healing & Tissue Repair", "Wound Healing & Tissue Repair"}
        };

        public static Dictionary<string, string> DeviceMarketMapping => new Dictionary<string, string>
        {
            {"Therapeutic Categories", "Device Market Area"},
            {"Aesthetics", ""},
            {"Blood & Coagulation Disorders", "Blood Disorders"},
            {"Cancer", "Cancer"},
            {"Cardiovascular", "Cardiology"},
            {"Dental - Oral", ""},
            {"Dermatology", "Dermatology"},
            {"ENT", ""},
            {"Gastrointestinal", "Gastroenterology"},
            {"Gynecology and Urology", "Gynecology & Urology"},
            {"Immune Disorders", "Immunology"},
            {"Infectious Diseases", ""},
            {"Inflammation", ""},
            {"Liver and Hepatic", ""},
            {"Metabolic Disorders", "Metabolic"},
            {"Neurology", "Neurology"},
            {"Ophthalmic", "Ophthalmology"},
            {"Orthopedics", "Orthopedics"},
            {"Poisoning", ""},
            {"Renal", ""},
            {"Respiratory", "Respiratory"},
            {"Wound Healing & Tissue Repair", "Wound Management"}
        };

        public static Dictionary<string, Dictionary<string, string>> ContentTypeMapping
        {
            get
            {
                var result = new Dictionary<string, Dictionary<string, string>>();
                var inVivo = new Dictionary<string, string>
                {
                    {"Feature Articles", "News"},
                    {"Around The Industry", "News"},
                    {"Company Close-Up", "News"},
                    {"Dealmakers", "News"},
                    {"Deals In Depth", "Analysis"},
                    {"In The Spotlight", "News"},
                    {"Noteworthy", "News"},
                    {"Regulatory Impact", "Analysis"},
                    {"Scorecard", "Analysis"},
                    {"Starting Out", "News"},
                    {"Deal Statistics Quarterly", "Analysis"},
                    {"Alliance Watch", "Analysis"},
                    {"Best of the Blog", "Opinion"},
                    {"Corporate Strategies", "Analysis"},
                    {"Diarist", "News"},
                    {"European Focus", "News"},
                    {"Executive Summaries", "News"},
                    {"In Focus", "News"},
                    {"Marketplace Strategies", "Analysis"},
                    {"Quarterly Stats", "Analysis"},
                    {"Sales Force Impact", "News"},
                    {"Second Opinion", "Opinion"},
                    {"Sidebar", "News"},
                    {"Emerging Company Profiles", "News"},
                    {"Start Up Previews", "News"},
                    {"The Pulse Of Competition", "News"},
                    {"On The Move: A Personnel Database", "News"},
                    {"Deals Shaping The Medical Industry", "News"},
                    {"Publisher's Spotlight", "News"}
                };
                result.Add("In Vivo Article", inVivo);

                var medInsight = new Dictionary<string, string>
                {
                    {"Feature Articles", "Analysis"},
                    {"MTI Global", "News"},
                    {"Market & Industry Briefs", "News"},
                    {"Deals Update", "News"},
                    {"Washington Roundup", "News"},
                    {"Start-Up News", "News"},
                    {"Medtech Execs On The Move", "News"},
                    {"Publisher's Spotlight", "News"},
                    {"Business & Technology Briefs", "News"},
                    {"Clinical Update", "News"},
                    {"Dealmaking Roundup", "News"},
                    {"Healthcare Trends", "Analysis"},
                    {"In Brief", "News"},
                    {"Cover Story", "Analysis"}
                };
                result.Add("Medtech Insight Article", medInsight);

                var pharmaApprovals = new Dictionary<string, string>
                {
                    {"Publisher's Spotlight", "News"},
                    {"Year In Review", "Analysis"},
                    {"New Drug Approvals", "News"},
                    {"Drug Review Profile", "News"},
                    {"Pipeline Overview", "News"},
                    {"Pipeline Review", "News"},
                    {"Therapeutic Area Overview", "News"},
                    {"NME Update", "News"},
                    {"Therapeutic Area Updates", "News"},
                    {"FDA Performance Tracker", "Analysis"},
                    {"New Drug Reviews", "News"},
                    {"Adaptive Trial Design", "News"},
                    {"Adaptive Trials", "News"},
                    {"Advisory Cmte", "News"},
                    {"American College Of Cardiology", "News"},
                    {"Approvals", "News"},
                    {"Cder Annual Report", "News"},
                    {"Chart", "News"},
                    {"Charts", "News"},
                    {"Company Profile", "News"},
                    {"Development Funding", "News"},
                    {"Dia", "News"},
                    {"Disease Focus", "News"},
                    {"F D C Management", "News"},
                    {"Fda 2003 Performance", "News"},
                    {"Fdc 2003 Performance", "News"},
                    {"Fdc Policy", "News"},
                    {"In Line Chart", "News"},
                    {"International Markets", "News"},
                    {"IPO", "News"},
                    {"Licensing", "News"},
                    {"Meeting Coverage", "News"},
                    {"New Drugs On Deck", "News"},
                    {"New Submissions", "News"},
                    {"Oncology Strategy", "News"},
                    {"Pending Applications", "News"},
                    {"Pending Submission", "News"},
                    {"Pipeline Spotlight", "News"},
                    {"Quarterly Coverage", "News"},
                    {"Second Quarter Coverage", "News"},
                    {"Submissions", "News"},
                    {"Supplemental Approvals", "News"},
                    {"Trademark Watch", "News"},
                    {"Upcoming User Fees", "News"},
                    {"Briefs", "News"},
                    {"Drug Development Profile", "News"},
                    {"Regulatory Policy", "News"},
                    {"R&D News", "News"},
                    {"Pipeline Updates", "News"},
                    {"FDA Policy", "News"}
                };
                result.Add("Pharmaceutical Approvals Monthly Article", pharmaApprovals);

                var pharmAsiaNews = new Dictionary<string, string>
                {
                    {"Features", "Analysis"},
                    {"Analysis", "Analysis"},
                    {"April", "News"},
                    {"Asia", "News"},
                    {"Australia", "News"},
                    {"China", "News"},
                    {"Chinese Language Press", "News"},
                    {"Editor's Picks - News From The Web", "News"},
                    {"India", "News"},
                    {"Insider Analysis", "News"},
                    {"Interviews", "News"},
                    {"Japan", "News"},
                    {"Japanese Language Press", "News"},
                    {"Korea", "News"},
                    {"Other Asia News", "News"},
                    {"Other Emerging Markets", "News"},
                    {"Pricing and Reimbursement", "News"},
                    {"Ranbaxy", "News"},
                    {"Regulatory", "News"},
                    {"World Press", "News"}
                };
                result.Add("Pharmasia News Article", pharmAsiaNews);

                var rpmReport = new Dictionary<string, string>
                {
                    {"Whats New", "News"},
                    {"Feature Articles", "Analysis"},
                    {"Ask The Experts", "Opinion"},
                    {"FDA Beat", "Analysis"},
                    {"CMS Beat", "Analysis"},
                    {"Street Smarts", "Analysis"},
                    {"Pointed View", "Opinion"},
                    {"Free Speech", "Opinion"},
                    {"Sidebar", "News"},
                    {"Leads & Contacts", "News"},
                    {"Editor's Letter", "Opinion"},
                    {"Inside The FDA", "Analysis"},
                    {"Cases & Solutions", "Analysis"},
                    {"Publisher's Spotlight", "News"}
                };
                result.Add("RPM Report Article", rpmReport);

                var startUp = new Dictionary<string, string>
                {
                    {"Feature Articles", "Analysis"},
                    {"Valuation Watch", "Analysis"},
                    {"Venture Round", "Analysis"},
                    {"Capital Matters", "Analysis"},
                    {"Science Matters", "Analysis"},
                    {"Advice Of Counsel", "Opinion"},
                    {"Best of the Blog", "Opinion"},
                    {"Executive Summaries", "News"},
                    {"Grouped Start-Ups", "Analysis"},
                    {"Start-Ups Across Health Care", "Analysis"},
                    {"Nothing Ventured", "Analysis"},
                    {"Emergings in Brief", "News"},
                    {"Slice of the Industry", "News"},
                    {"Start-Up Quarterly Statistics", "Analysis"},
                    {"On The Move", "News"},
                    {"Technology Strategies", "Analysis"},
                    {"University Beat", "Analysis"},
                    {"Venture Beat", "Analysis"},
                    {"Recent Financings Of Private Companies", "Analysis"},
                    {"Tech Transfer Deals", "Analysis"},
                    {"Publisher's Spotlight", "News"}
                };
                result.Add("Start Up Article", startUp);

                var goldSheet = new Dictionary<string, string>
                {
                    {"Brief", "News"},
                    {"In Line Chart", "News"},
                    {"Publisher's Spotlight", "News"}
                };
                result.Add("The Gold Sheet Article", goldSheet);

                var pinkSheet = new Dictionary<string, string>
                {
                    {"Drug Review Profile", "Analysis"},
                    {"Marketing/Advertising", "News"},
                    {"On Capitol Hill", "News"},
                    {"Regulatory Update", "News"},
                    {"FDA", "News"},
                    {"Generic Drugs", "News"},
                    {"Reimbursement", "News"},
                    {"Special Features", "News"},
                    {"International", "News"},
                    {"Biosimilars", "News"},
                    {"Business & Finance", "News"},
                    {"R & D", "News"},
                    {"FDA Performance Tracker", "Analysis"},
                    {"Drug Safety", "News"},
                    {"Pipeline Update", "News"},
                    {"Advisory Committees", "News"},
                    {"New Products", "News"},
                    {"Wholesalers & Distributors", "News"},
                    {"Year In Review And The Year Ahead", "Analysis"},
                    {"Patents", "News"},
                    {"Litigation", "News"},
                    {"Manufacturing", "News"},
                    {"People In The News", "News"},
                    {"Biotechnology", "News"},
                    {"Recalls", "News"},
                    {"Companion Diagnostics", "News"},
                    {"Comparative Effectiveness Research", "News"},
                    {"Elections", "News"},
                    {"Pharmacy", "News"},
                    {"Electronic Health", "News"},
                    {"Medicaid", "News"},
                    {"Health Reform", "News"},
                    {"Rx-To-OTC Switch", "News"},
                    {"Medicare", "News"},
                    {"People", "News"},
                    {"Around the Industry", "News"},
                    {"Around the World", "News"},
                    {"Briefing", "News"},
                    {"Briefs", "News"},
                    {"Chart", "News"},
                    {"Correction", "News"},
                    {"Deal", "News"},
                    {"Deals in Depth", "Analysis"},
                    {"Diagnostics", "News"},
                    {"Feature Articles", "Analysis"},
                    {"General", "News"},
                    {"Global Perspective", "News"},
                    {"In Line Chart", "News"},
                    {"Introduction", "News"},
                    {"Mergers & Acquisitions", "News"},
                    {"News From Europe", "News"},
                    {"Policy Perspective", "Analysis"},
                    {"Sales & Earnings", "News"},
                    {"State News", "News"},
                    {"White House", "News"},
                    {"On The Hill Or Congress", "News"},
                    {"Publisher's Spotlight", "News"}
                };
                result.Add("The Pink Sheet Article", pinkSheet);

                var graySheet = new Dictionary<string, string>
                {
                    {"Top Stories", "News"},
                    {"Weekly Roundup", "News"},
                    {"Features and Analysis", "Analysis"},
                    {"Multimedia", "News"},
                    {"Regulatory and Policy News", "News"},
                    {"Regulatory Correspondence", "News"},
                    {"Around The Industry", "News"},
                    {"Quality Replay", "News"},
                    {"Compliance Corner", "News"},
                    {"Year In Review", "Analysis"},
                    {"Medtech Talk", "News"},
                    {"Conference Coverage", "News"},
                    {"FDA Decision Data", "Analysis"},
                    {"FDA Review Trends", "Analysis"},
                    {"International", "News"},
                    {"Clinical Trials Tracker", "Analysis"},
                    {"Recalls", "News"},
                    {"Publisher's Spotlight", "News"},
                    {"Clinical Trials", "News"},
                    {"New Products", "News"},
                    {"Reimbursement", "News"},
                    {"Congress", "News"},
                    {"Diagnostics", "News"},
                    {"Technology Assessment", "News"},
                    {"People", "News"},
                    {"Regulatory", "News"},
                    {"Business", "News"},
                    {"Innovations", "News"},
                    {"Compliance & Enforcement", "News"},
                    {"Digital Health", "News"},
                    {"Patents", "News"},
                    {"Health Care Policy", "News"},
                    {"Industry Trends", "News"},
                    {"Litigation", "News"},
                    {"FDA Advisory Panels", "News"},
                    {"Start-Ups", "News"},
                    {"Elections", "News"},
                    {"Marketing", "News"},
                    {"In Brief", "News"},
                    {"Featured Story", "Analysis"},
                    {"Correction", "News"},
                    {"Comparative Effectiveness", "News"},
                    {"Capitol Hill", "News"},
                    {"Mergers & Acquisitions", "News"},
                    {"Mobile Health", "News"},
                    {"Policy", "News"},
                    {"Research & Development", "News"},
                    {"Financings", "News"},
                    {"Personalized Medicine", "News"},
                    {"Chart", "News"},
                    {"CMS", "News"},
                    {"Emerging Markets", "News"},
                    {"Health Care Delivery", "News"},
                    {"Health Care Reform", "News"},
                    {"Imaging", "News"},
                    {"News From Europe", "News"},
                    {"Sales & Earnings", "News"},
                    {"FDA", "News"},
                    {"Reader Favorites", "News"},
                    {"Associations", "News"},
                    {"Advisory Panels", "News"},
                    {"Inactive", "News"},
                    {"News in Brief", "News"},
                    {"Industry In Brief", "News"}
                };
                result.Add("The Grey Sheet Article", graySheet);

                var roseSheet = new Dictionary<string, string>
                {
                    {"Litigation", "News"},
                    {"Regulatory/Legislative", "News"},
                    {"Fragrances", "News"},
                    {"Cosmetic Ingredients", "News"},
                    {"Marketing/Advertising", "News"},
                    {"International", "News"},
                    {"The Marketplace", "News"},
                    {"Sales & Earnings", "News"},
                    {"Business & Finance", "News"},
                    {"PCPC", "News"},
                    {"Regulatory Legislative", "News"},
                    {"New Products", "News"},
                    {"People In The News", "News"},
                    {"Recalls Enforcement", "News"},
                    {"Regulatory", "News"},
                    {"Sales & Earnings Bad", "News"},
                    {"Skin_Care", "News"},
                    {"Mergers & Acquisitions", "News"},
                    {"Natural/Organics", "News"},
                    {"People", "News"},
                    {"Recalls", "News"},
                    {"Mass Market", "News"},
                    {"E-Commerce", "News"},
                    {"Skin Care", "News"},
                    {"Hair Care", "News"},
                    {"Sustainability", "News"},
                    {"R & D", "News"},
                    {"Sun Care", "News"},
                    {"CTFA", "News"},
                    {"Animal Testing Alternatives", "News"},
                    {"Salon Market", "News"},
                    {"Specialty Store Market", "News"},
                    {"State News", "News"},
                    {"Year In Review", "News"},
                    {"In Brief", "News"},
                    {"Publisher's Spotlight", "News"},
                    {"Trademarks", "News"}
                };
                result.Add("The Rose Sheet Article", roseSheet);

                var silverSheet = new Dictionary<string, string>
                {
                    {"Breaking", "News"},
                    {"In Their Words", "News"},
                    {"Compliance Corner", "Analysis"},
                    {"Year In Review", "Analysis"},
                    {"Featured Content", "Analysis"},
                    {"Medtech Talk", "News"},
                    {"Regulatory Correspondence", "News"},
                    {"Odds & Ends", "News"},
                    {"News in Brief", "News"},
                    {"Publisher's Spotlight", "News"},
                    {"Warning Letters & Close-Out Letters", "News"}
                };
                result.Add("The Silver Sheet Article", silverSheet);

                var tanSheet = new Dictionary<string, string>
                {
                    {"OTCs", "News"},
                    {"Marketing/Advertising", "News"},
                    {"Litigation", "News"},
                    {"Regulatory", "News"},
                    {"Dietary Supplements", "News"},
                    {"News in Brief", "News"},
                    {"New Products", "News"},
                    {"FDA", "News"},
                    {"Business & Finance", "News"},
                    {"Rx-to-OTC Switch", "News"},
                    {"On The Hill", "News"},
                    {"Nutritionals", "News"},
                    {"Research & Development", "News"},
                    {"People", "News"},
                    {"State News", "News"},
                    {"Supplement GMPs", "News"},
                    {"International", "News"},
                    {"Sales & Earnings", "News"},
                    {"Chart", "News"},
                    {"E-Commerce", "News"},
                    {"Manufacturing", "News"},
                    {"Nonprescription Drug Advisory Committee", "News"},
                    {"Patents", "News"},
                    {"People In The News", "News"},
                    {"Pharmacy", "News"},
                    {"Recalls Enforcement", "News"},
                    {"Reimbursement", "News"},
                    {"Sports Supplements", "News"},
                    {"Structure Function Claims", "News"},
                    {"Wholesalers/Distributors", "News"},
                    {"Year In Review", "Analysis"},
                    {"Recalls", "News"},
                    {"Trademarks", "News"},
                    {"Publisher's Spotlight", "News"},
                    {"Mergers & Acquisitions", "News"}
                };
                result.Add("The Tan Sheet Article", tanSheet);

                return result;
            }
        }

        public static Dictionary<string, string> SubjectMapping => new Dictionary<string, string>
        {
            {"KeepingTrack", "Keeping Tracker"},
            {"PerformanceTracker", "Performance Tracker"},
            {"Bioterrorism", "Bioterrorism"},
            {"Blood Products", "Blood Products"},
            {"Business Models", "Business Models"},
            {"Business Strategies", "Business Strategies"},
            {"Clinical Development & Trials", "Clinical Development & Trials"},
            {"Commercial", ""},
            {"Advertising", "Advertising & Marketing"},
            {"Distribution", "Distribution"},
            {"Life Cycle Management", "Life Cycle Management"},
            {"Marketing & Sales", "Marketing & Sales"},
            {"Pricing Strategies", "Pricing Strategies"},
            {"Product Launch", "Product Launch"},
            {"Comparative Effectiveness", "Comparative Effectiveness"},
            {"Deals & Dealmaking Strategies", "Deals"},
            {"Emerging Markets", "Emerging Markets"},
            {"Epidemiology", "Epidemiology"},
            {"Financial Strategies", "Financial Strategies"},
            {"Health Care Statistics", "Health Care Statistics"},
            {"Incubators", "Incubators"},
            {"Innovation", "Innovation"},
            {"Legal Issues", "Legal Issues"},
            {"Intellectual Property", "Intellectual Property"},
            {"Patent Litigation", "Patent Litigation"},
            {"Legislation", "Legislation"},
            {"Management Issues", "Management Issues"},
            {"Manufacturing", "Manufacturing"},
            {"Personalized Medicine", "Personalized Medicine"},
            {"Platform Technologies", "Platform Technologies"},
            {"Drug-Device Convergence", "Drug-Device Convergence"},
            {"Nanotechnology", "Nanotechnology"},
            {"Regenerative Medicine", "Regenerative Medicine"},
            {"Regulatory", "Regulatory"},
            {"Antitrust Regulation", "Antitrust Regulation"},
            {"Enforcement", "Enforcement"},
            {"Post-Market Regulation", "Post-Market Regulation"},
            {"Advertising Promotion & Regulation", "Advertising Promotion & Regulation"},
            {"Manufacturing Quality", "Manufacturing Quality"},
            {"Post-Marketing Studies", "Post-Marketing Studies"},
            {"Product Recalls", "Product Recalls"},
            {"Product Safety", "Product Safety"},
            {"Risk Management", "Risk Management"},
            {"Pre-Market Regulation", "Pre-Market Regulation"},
            {"Advisory Committees", "Advisory Committees"},
            {"Clinical Trial Regulation", "Clinical Trial Regulation"},
            {"Drug Approval Standards", "Drug Approval Standards"},
            {"Prescription To Otc Switch", "Prescription To Otc Switch"},
            {"Product Approvals", "Product Approvals"},
            {"Devices", ""},
            {"Drugs", ""},
            {"Securities Regulation", "Securities Regulation"},
            {"Reimbursement", "Reimbursement"},
            {"Cost Effectiveness", "Cost Effectiveness"},
            {"Formularies", "Formularies"},
            {"Government Payors", "Government Payors"},
            {"Ex-United States", ""},
            {"United States", ""},
            {"Medicaid", "Medicaid"},
            {"Medicare", "Medicare"},
            {"Private Payors", "Private Payors"},
            {"Managed Markets", "Managed Markets"},
            {"Pharmacy Benefit Management", "Pharmacy Benefit Management"},
            {"Research & Development Strategies", "Research & Development Strategies"},
            {"Discovery", "Discovery"},
            {"Preclinical Through Proof-Of-Concept", "Preclinical Through Proof-Of-Concept"},
            {"Proof-Of-Concept Through Filing", "Proof-Of-Concept Through Filing"},
            {"Social Media", "Social Media"},
            {"Surgical Procedures", "Surgical Procedures"},
            {"Keeping Track", "Keeping Track"},
            {"Performance Tracker", "Performance Tracker"}
        };
        public static Dictionary<string, string> IndustryMapping => new Dictionary<string, string>
        {
            {"Biopharmaceuticals", "BioPharmaceutical"},
            {"Consumer Products", "Consumer"},
            {"Financial", ""},
            {"In Vitro Diagnostics", "Medical Device"},
            {"Information Technology", ""},
            {"Laboratory Testing Services", ""},
            {"Medical Devices", "Medical Device"},
            {"Services", ""}
        };
    }


    public class InformaListToCommodity : ListToGuid
    {
        public InformaListToCommodity(Item i) : base(i) { }

        #region Methods

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
        {
            if (string.IsNullOrEmpty(importValue))
                return;

            //get parent item of list to search
            var sourceItems = GetSourceItems(newItem.Database);
            if (sourceItems == null)
                return;

            Dictionary<string, string> d = GetMapping();

            var values = importValue.Split(GetFieldValueDelimiter()?[0] ?? ',');

            foreach (var val in values)
            {
                //string upperValue = val.ToUpper();
                // string upperValue = val.ToUpper();
                string transformValue = (d.ContainsKey(val)) ? d[val] : string.Empty;
                if (string.IsNullOrEmpty(transformValue))
                {
                    map.Logger.Log(newItem.Paths.FullPath, "Region not converted", ProcessStatus.FieldError, NewItemField, val);
                    continue;
                }
                if (transformValue.Contains("&"))
                {
                    transformValue = transformValue.Replace("&", "and");
                }
                string cleanName = StringUtility.GetValidItemName(transformValue, map.ItemNameMaxLength);
                IEnumerable<Item> t = sourceItems.Where(c => c.Name.Equals(cleanName));

                //if you find one then store the id
                if (!t.Any())
                {
                    map.Logger.Log(newItem.Paths.FullPath, "Region(s) not found in list", ProcessStatus.FieldError, NewItemField, val);
                    continue;
                }

                Field f = newItem.Fields[NewItemField];
                if (f == null)
                    continue;

                string ctID = t.First().ID.ToString();

                if (NewItemField == "Taxonomy")
                {
                    TaxonomyList.Add(t.First().ID.ToString());
                }

            }
        }

        public virtual Dictionary<string, string> GetMapping()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            //d.Add("dairy_markets", "Dairy");
            //d.Add("dairy_markets_subscribe_free_demo", "Dairy");
            //d.Add("dairy_markets_features", "Dairy");
            //d.Add("dairy_markets_downloads", "Dairy");
            //d.Add("dairy_markets_filedownloads", "Dairy");
            //d.Add("dairy_markets_subscribe_free_demo_mobile", "Dairy");
            //d.Add("dairy_markets_resources_dairy_ezine_mobile", "Dairy");
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
            d.Add("foodnews_dfn", "Frozen Foods");
            d.Add("foodnews_dfn_nuts", "Frozen Foods");
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

            d.Add("public_ledger_commodities_nuts", "Dried Fruit & Nuts");

            d.Add("public_ledger_commodities_nuts_almonds", "Almonds");


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

            d.Add("dairy_markets_analysis_policy", "Policy");
            d.Add("dairy_markets_analysis_trade_Import", "Imports");
            d.Add("dairy_markets_analysis_trade_Export", "Exports");
            // d.Add("dairy_markets_analysis_trade", "Imports,Exports");
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

        #endregion Methods

    }





    public class Commodityfactor : ListToGuid
    {
        public Commodityfactor(Item i) : base(i) { }

        #region Methods

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
        {
            if (string.IsNullOrEmpty(importValue))
                return;

            //get parent item of list to search
            var sourceItems = GetSourceItems(newItem.Database);
            if (sourceItems == null)
                return;

            Dictionary<string, string> d = GetMapping();

            var values = importValue.Split(GetFieldValueDelimiter()?[0] ?? ',');

            foreach (var val in values)
            {
                string upperValue = val.ToUpper();
                string transformValue = (d.ContainsKey(upperValue)) ? d[upperValue] : string.Empty;
                if (string.IsNullOrEmpty(transformValue))
                {
                    map.Logger.Log(newItem.Paths.FullPath, "Region not converted", ProcessStatus.FieldError, NewItemField, val);
                    continue;
                }

                string cleanName = StringUtility.GetValidItemName(transformValue, map.ItemNameMaxLength);
                IEnumerable<Item> t = sourceItems.Where(c => c.DisplayName.Equals(cleanName));

                //if you find one then store the id
                if (!t.Any())
                {
                    map.Logger.Log(newItem.Paths.FullPath, "Region(s) not found in list", ProcessStatus.FieldError, NewItemField, val);
                    continue;
                }

                Field f = newItem.Fields[NewItemField];
                if (f == null)
                    continue;

                if (NewItemField == "Taxonomy")
                {
                    TaxonomyList.Add(t.First().ID.ToString());
                }

            }
        }

        public virtual Dictionary<string, string> GetMapping()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("dairy_markets", "Dairy");
            d.Add("dairy_markets_subscribe_free_demo", "Dairy");
            d.Add("dairy_markets_features", "Dairy");
            d.Add("dairy_markets_market_focus", "Dairy");
            d.Add("dairy_markets_downloads", "Dairy");
            d.Add("dairy_markets_filedownloads", "Dairy");
            d.Add("dairy_markets_subscribe_free_demo_mobile", "Dairy");
            d.Add("dairy_markets_resources_dairy_ezine_mobile", "Dairy");
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
            d.Add("dairy_markets_analysis_policy", "Dairy");
            d.Add("dairy_markets_analysis_trade", "Dairy");

            return d;
        }

        #endregion Methods

    }
















    public class ToInformaMediatType : ListToGuid
    {
        public ToInformaMediatType(Item i) : base(i) { }

        #region Methods

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
        {
            if (string.IsNullOrEmpty(importValue))
                return;

            //get parent item of list to search
            var sourceItems = GetSourceItems(newItem.Database);
            if (sourceItems == null)
                return;

            Dictionary<string, string> d = GetMapping();

            string lowerValue = importValue.ToLower();
            string transformValue = (d.ContainsKey(lowerValue)) ? d[lowerValue] : string.Empty;
            if (string.IsNullOrEmpty(transformValue))
            {
                map.Logger.Log(newItem.Paths.FullPath, "Media Type not converted", ProcessStatus.FieldError, NewItemField, importValue);
                return;
            }

            //loop through children and look for anything that matches by name
            string cleanName = StringUtility.GetValidItemName(transformValue, map.ItemNameMaxLength);
            IEnumerable<Item> t = sourceItems.Where(c => c.DisplayName.Equals(cleanName));

            //if you find one then store the id
            if (!t.Any())
            {
                map.Logger.Log(newItem.Paths.FullPath, "Media Type not matched", ProcessStatus.FieldError, NewItemField, importValue);
                return;
            }

            Field f = newItem.Fields[NewItemField];
            if (f == null)
                return;

            string ctID = t.First().ID.ToString();
            if (!f.Value.Contains(ctID))
                f.Value = ctID;
        }

        protected virtual Dictionary<string, string> GetMapping()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("data", "Data");
            d.Add("image", "Image");
            d.Add("audio", "Audio");
            d.Add("Video", "News");
            d.Add("chartgraph", "Chart/Graph");
            d.Add("timeline ", "Timeline");
            d.Add("dataTable", "Data Table");
            d.Add("webinars", "Webinars");
            d.Add("interactivedashboards", "Interactive Dashboards");
            d.Add("supportingdocument", "Supporting Document");


            return d;
        }

        #endregion Methods

    }


    #endregion


}