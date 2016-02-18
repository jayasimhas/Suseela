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
using System.Threading.Tasks;
using HtmlAgilityPack;
using Sitecore.Collections;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;
using Sitecore.SharedSource.DataImporter.Providers;
using Sitecore.SharedSource.DataImporter.Utility;
using HtmlDocument = Sitecore.WordOCX.HtmlDocument.HtmlDocument;

namespace Sitecore.SharedSource.DataImporter.Mappings.Fields
{
    public class ToArticleNumberText : ToText {
        #region Constructor

        public ToArticleNumberText(Item i) : base(i) { }

        #endregion Constructor

        #region Methods

        public override void FillField(IDataMap map, ref Item newItem, string importValue)
        {
            // connect to the company database and get the ID to store
            if (importValue.Equals(string.Empty))
                return;

            
            //store the imported value as is
            Field f = newItem.Fields[NewItemField];
            if (f == null)
                return;

            //don't overwrite the number if the item was already created. this occurs on second runs
            if(string.IsNullOrEmpty(f.Value))
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

        public override void FillField(IDataMap map, ref Item newItem, string importValue)
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
            List<string> unwantedTags = new List<string>() { "embed", "iframe", "script", "table", "font", "img" };
            
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

                if (childNodes != null) {
                    foreach (var child in childNodes)
                        nodes.Enqueue(child);
                }

                if (unwantedTags.Any(tag => tag == nodeName)) { // if this node is one to remove
                    if (childNodes != null) { // make sure children are added back
                        foreach (var child in childNodes)
                            parentNode.InsertBefore(child, node);
                    }

                    parentNode.RemoveChild(node);
                } else if (node.HasAttributes) { // if it's not being removed
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

        public override void FillField(IDataMap map, ref Item newItem, string importValue)
        {
            //replace <h1> with <h2>
            importValue = importValue.Replace("h1", "h2").Replace("62.73.128.229", "www.scripintelligence.com");

            //strip out the tags, attributes and remap images
            List<string> removeTags = new List<string>() {"font"};
            List<string> removeAttrs = new List<string>() { "style", "align", "height", "width", "class" };
            DateTime dt = new DateTime(1800, 1, 1);
            DateField df = newItem.Fields["Created Date"];
            if (df != null && !string.IsNullOrEmpty(df.Value))
                dt = df.DateTime;
            string newImportValue = CleanHtml(map, newItem.Paths.FullPath, dt, importValue, removeTags, removeAttrs);
            
            //store the imported value as is
            Field f = newItem.Fields[NewItemField];
            if (f != null)
                f.Value = newImportValue;
        }

        public string CleanHtml(IDataMap map, string articlePath, DateTime articleDate, string html, List<string> unwantedTags, List<string> unwantedAttrs)
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
            while (nodes.Count > 0) {
                var node = nodes.Dequeue();
                var nodeName = node.Name.ToLower();
                var parentNode = node.ParentNode;
                var childNodes = node.SelectNodes("./*|./text()");
                
                if(i == 0 && (nodeName.Equals("table") || nodeName.Equals("img"))) //log table or img as first paragraph
                    map.Logger.Log(articlePath, $"first element was a(n) '{nodeName}'", ProcessStatus.Warning, NewItemField, html);

                if (childNodes != null) {
                    foreach (var child in childNodes)
                        nodes.Enqueue(child);
                }

                if (unwantedTags.Any(tag => tag == nodeName)) { // if this node is one to remove
                    if (childNodes != null) { // make sure children are added back
                        foreach (var child in childNodes)
                            parentNode.InsertBefore(child, node);
                    }

                    parentNode.RemoveChild(node);
                } else if (node.HasAttributes) { // if it's not being removed
                    if (nodeName.Equals("table")) {
                        node.Attributes.RemoveAll();
                    } else if (!nodeName.Equals("iframe") && !nodeName.Equals("img")) { //skip iframe and imgs
                        foreach (string s in unwantedAttrs) // remove unwanted attributes
                            node.Attributes.Remove(s);
                    }

                    if (nodeName.Equals("iframe") || nodeName.Equals("embed") || nodeName.Equals("form") || nodeName.Equals("script")) // warn about iframes, embed, form or script in body
                        map.Logger.Log(articlePath, $"content contains a(n) {nodeName}'", ProcessStatus.Warning, NewItemField, html);

                    //replace images
                    if (nodeName.Equals("img")) {
                        // see if it exists
                        string imgSrc = node.Attributes["src"].Value;
                        MediaItem newImg = HandleImage(map, articlePath, articleDate, imgSrc);
                        if (newImg != null)
                        {
                            string newSrc = $"-/media/{newImg.ID.ToShortID().ToString()}.ashx";
                            // replace the node with sitecore tag
                            node.SetAttributeValue("src", newSrc);

                            // log if imgs is wider then 787px
                            Field f = newImg.InnerItem.Fields["Width"];
                            string imgWidthValue = (f != null) ? f.Value : string.Empty;
                            int imgWidth = 0;
                            if (int.TryParse(imgWidthValue, out imgWidth) && imgWidth > 787)
                                map.Logger.Log(articlePath, $"image too wide: '{imgWidth}'", ProcessStatus.Warning, NewItemField, node.OuterHtml);
                        }
                    }
                }

                i++;
            }

            return document.DocumentNode.InnerHtml;
        }

        public MediaItem HandleImage(IDataMap map, string articlePath, DateTime dt, string url)
        {
            // see if the url is badly formed
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute)) {
                map.Logger.Log(articlePath, "malformed image URL", ProcessStatus.FieldError, NewItemField, url);
                return null;
            }

            //get file info
            List<string> uri = url.Split(new string[] { "?" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> parts = uri[0].Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            string filePath = parts[parts.Count-1].Trim();
            string[] fileParts = filePath.Split(new string[] {"."}, StringSplitOptions.RemoveEmptyEntries);
            string fileName = (fileParts.Length > 0) ? StringUtility.GetValidItemName(fileParts[0], map.ItemNameMaxLength) : string.Empty;
            
            //date info
            string newFilePath = (dt.Year != 1800) ? $"{dt.ToString("yyyy/MMMM")}/{fileName}" : fileName;

            // see if it exists in med lib
            Item rootItem = map.ToDB.GetItem(Sitecore.Data.ID.Parse("{CDC0468D-CFAE-4E65-9CE7-BF47848A8A81}"));
            IEnumerable<Item> matches = GetMediaItems(map)
                .Where(a => a.Paths.FullPath.EndsWith(fileName));

            if (matches != null && matches.Any()) {
                if (matches.Count().Equals(1))
                    return new MediaItem(matches.First());

                map.Logger.Log(articlePath, $"Sitecore image matched {matches.Count()} images", ProcessStatus.FieldError, NewItemField, filePath);
                return null;
            }
            
            MediaItem m = ImportImage(url, filePath, $"{rootItem.Paths.FullPath}/{newFilePath}");
            if (m == null)
                map.Logger.Log(articlePath, "Image not found", ProcessStatus.FieldError, NewItemField, url);

            return m;
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

        public MediaItem ImportImage(string url, string fileName, string newPath) {

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null)
                return null;

            try {
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
                MediaItem mediaItem = creator.CreateFromStream(stream2, fileName, options);

                response.Close();

                return mediaItem;
            } catch (WebException ex) {
                return null;
            }
        }

        #endregion Methods
    }
    
    public class ToInformaCompanyField : ToText
    {
        #region Constructor

        public ToInformaCompanyField(Item i) : base(i) { }

        #endregion Constructor

        #region Methods

        public override void FillField(IDataMap map, ref Item newItem, string importValue)
        {
            // connect to the company database and get the ID to store
            if (importValue.Equals(string.Empty))
                return;

            //try exact search first
            Dictionary<string, string> fileCompanies = GetFileCompanies();
            Dictionary<string, string> dbCompanies = GetDBCompanies(map, newItem.Paths.FullPath);

            IEnumerable<string> importCompanies = importValue.Split(new string[] {","},
                StringSplitOptions.RemoveEmptyEntries).Distinct();

            List<string> cIDs = new List<string>();

            foreach(string cName in importCompanies) { 
                string casedValue = cName.ToLower();
                KeyValuePair<string, string> id;
                if (fileCompanies.ContainsKey(casedValue)) { 
                    id = fileCompanies.First(a => a.Key.Equals(casedValue));
                }
                else if (dbCompanies.ContainsKey(casedValue)) { 
                    id = dbCompanies.First(a => a.Key.Equals(casedValue));
                }
                else { 
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
                    id = ids.First();
                }
                
                string businessID = id.Value;
                if (string.IsNullOrEmpty(businessID))
                    return;
                
                //the name should be the importValue and not the value from the database
                Field sf = newItem.Fields["Summary"];
                if (sf != null)
                    sf.Value = Regex.Replace(sf.Value, $"({cName})", delegate (Match match) { return $"[C#{businessID}:{match.Value}]"; }, RegexOptions.IgnoreCase);

                Field bf = newItem.Fields["Body"];
                if (bf != null)
                    bf.Value = Regex.Replace(bf.Value, $"({cName})", delegate (Match match) { return $"[C#{businessID}:{match.Value}]"; }, RegexOptions.IgnoreCase);

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

            string query = "SELECT[RecordNumber], [Title] FROM [ElsevierSupport_DCD].[dbo].[Companies] ORDER BY [Title] DESC";
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
                companies.Add(r["Title"].ToString().ToLower(), r["RecordNumber"].ToString());
            }

            Context.Items[cacheKey] = companies;
            return companies;
        }

        public Dictionary<string, string> GetFileCompanies()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();

            d.Add("abbot","198600101");
            d.Add("abbott","198600101");
            d.Add("acadia","199700102");
            d.Add("acambis","199300262");
            d.Add("actelion","199800188");
            d.Add("adventrx","199700459");
            d.Add("akorn","199000427");
            d.Add("alcon","198601361");
            d.Add("aldrich","199000633");
            d.Add("alexion","199300121");
            d.Add("alios","200700864");
            d.Add("alkermes","198800695");
            d.Add("allergan","198601285");
            d.Add("alnylam","200200681");
            d.Add("alter","198700592");
            d.Add("altus pharmaceuticals","200100009");
            d.Add("amarin","198900628");
            d.Add("amgen","198600111");
            d.Add("amri","199500499");
            d.Add("anaphore","200400257");
            d.Add("angiotech","198601320");
            d.Add("anika therapeutics","199300161");
            d.Add("anthera","200500234");
            d.Add("antigenics","198601321");
            d.Add("antisense pharma","200400074");
            d.Add("apotex","198601232");
            d.Add("arana therapeutics","199100165");
            d.Add("arena","199800421");
            d.Add("ariad","199200091");
            d.Add("ark therapeutics","199800145");
            d.Add("asahi kasei pharma","200600424");
            d.Add("astellas","198600364");
            d.Add("astrazenec","198601342");
            d.Add("astrazeneca","198601342");
            d.Add("aveo","200200462");
            d.Add("basilea pharmaceutica","200001083");
            d.Add("bavarian nordic","199700439");
            d.Add("bayer","198600358");
            d.Add("benitec","200300357");
            d.Add("bial","200800001");
            d.Add("biocompatibles","199000041");
            d.Add("biocon","200400289");
            d.Add("biodel","200600510");
            d.Add("biogen idec","198601366");
            d.Add("bioinvent","200000438");
            d.Add("biomarin","199400477");
            d.Add("bionomics","200101261");
            d.Add("biovail","198601277");
            d.Add("biovex","200001412");
            d.Add("boehringer ingelhei","198600820");
            d.Add("boehringer ingelheim","198600820");
            d.Add("bradmer pharmaceuticals","201400677");
            d.Add("bristol-myers squibb","198601245");
            d.Add("bt pharma","200700411");
            d.Add("btg","198601174");
            d.Add("cancer research technology","200300248");
            d.Add("cangene","198600453");
            d.Add("cardinal health","198600127");
            d.Add("cardiome","199200060");
            d.Add("cardion","199700461");
            d.Add("celera","199100413");
            d.Add("celgene","198601148");
            d.Add("cell genesys","198601263");
            d.Add("cell therapeutics","200500330");
            d.Add("cephalon","198800712");
            d.Add("champions biotechnology","200700142");
            d.Add("charleston laboratories","201400433");
            d.Add("compugen","199800393");
            d.Add("covidien","201100770");
            d.Add("crucell","199400312");
            d.Add("csl","198600617");
            d.Add("cubist","199400401");
            d.Add("curis","199400401");
            d.Add("cydex","199800025");
            d.Add("cytyc","199000273");
            d.Add("dabur pharma","200800383");
            d.Add("daiichi sanky","198700764");
            d.Add("daiichi sankyo","198700764");
            d.Add("debiopharm","199100420");
            d.Add("dexcel pharma","200200830");
            d.Add("diamyd medical","200600508");
            d.Add("dnx","198700297");
            d.Add("dompe","199200120");
            d.Add("dr falk pharma","199000506");
            d.Add("dr reddy's","199700124");
            d.Add("eisa","198800610");
            d.Add("eisai","198800610");
            d.Add("elan","198600151");
            d.Add("eli lill","198600152");
            d.Add("eli lilly","198600152");
            d.Add("endo pharmaceuticals","200200679");
            d.Add("enobia","200500069");
            d.Add("enzon","198600154");
            d.Add("epicept","199902983");
            d.Add("epitope","198600593");
            d.Add("ethypharm","199903139");
            d.Add("eurand","199700451");
            d.Add("evotec","199200398");
            d.Add("exonhit therapeutics","199800438");
            d.Add("eyetech","199200398");
            d.Add("ferring","199200432");
            d.Add("forest laboratories","198600158");
            d.Add("forma therapeutics","200900023");
            d.Add("freseniu","198601363");
            d.Add("fresenius","198601363");
            d.Add("fuso","199700419");
            d.Add("galderma","199200273");
            d.Add("generex","200000856");
            d.Add("genta","198900047");
            d.Add("genzyme","198600234");
            d.Add("geron","199300040");
            d.Add("gilead sciences","198601242");
            d.Add("glaxosmithkline","198601356");
            d.Add("glenmark","200400041");
            d.Add("glycomar","200600009");
            d.Add("gni","200400838");
            d.Add("gpc biotech","199400474");
            d.Add("grifols","200900443");
            d.Add("helsinn","200100277");
            d.Add("henderson morley","199903011");
            d.Add("hisamitsu","200400004");
            d.Add("horizon therapeutics","200700010");
            d.Add("hra pharma","200700883");
            d.Add("imed","200800172");
            d.Add("immunomedics","198600168");
            d.Add("immunotope","201400539");
            d.Add("inbiopro","201300268");
            d.Add("indevus","198900430");
            d.Add("innate pharma","200200602");
            d.Add("innocoll","200500387");
            d.Add("inovio","199300031");
            d.Add("irx therapeutics","200600267");
            d.Add("is pharma","200000427");
            d.Add("isotechnika","199903161");
            d.Add("ista pharmaceuticals","200000483");
            d.Add("johnson matthey","198600981");
            d.Add("jubilant organosys","200800242");
            d.Add("karo bio","198900367");
            d.Add("kinex","200300779");
            d.Add("kirin pharma","199400214");
            d.Add("kowa","200200841");
            d.Add("kyowa hakko","198700398");
            d.Add("labopharm","199500072");
            d.Add("lexicon pharmaceuticals","199500461");
            d.Add("life technologies","198601261");
            d.Add("ligand","198700772");
            d.Add("lundbeck","198700814");
            d.Add("lupin","199000010");
            d.Add("martek biosciences","199300244");
            d.Add("maxygen","199700251");
            d.Add("mdrna","198800048");
            d.Add("meda","200200740");
            d.Add("medac","199600282");
            d.Add("medical devices","199200072");
            d.Add("medicines company","199600430");
            d.Add("medicinova","200001022");
            d.Add("medigene","199100031");
            d.Add("medivation","200500497");
            d.Add("medivir","199500405");
            d.Add("medtronic","198600185");
            d.Add("menarini","198800771");
            d.Add("mentor","198600926");
            d.Add("merz","199000351");
            d.Add("metabolex","199700023");
            d.Add("molteni farmaceutici","200001267");
            d.Add("mpex","200300839");
            d.Add("mundipharma","198900663");
            d.Add("nanobio","200300753");
            d.Add("neuropharm","200700194");
            d.Add("neurosearch","199000144");
            d.Add("nih","198600351");
            d.Add("nobelpharma","201100708");
            d.Add("norgine","200200747");
            d.Add("novartis","198600519");
            d.Add("novelos therapeutics","200000801");
            d.Add("novo nordisk","198700137");
            d.Add("novozymes","200200498");
            d.Add("omnichem","198900398");
            d.Add("omrix","200600032");
            d.Add("oni biopharma","200400781");
            d.Add("ono","199000105");
            d.Add("onyx pharmaceuticals","199200104");
            d.Add("optimer","200001371");
            d.Add("orthologic","198700799");
            d.Add("oryzon","201400198");
            d.Add("osi pharmaceuticals","198600253");
            d.Add("osteologix","200500099");
            d.Add("otsuka","198700860");
            d.Add("ovation pharmaceuticals","200200417");
            d.Add("oxis","198600150");
            d.Add("par pharmaceutical","198601284");
            d.Add("peptcell","200900485");
            d.Add("pfize","198600199");
            d.Add("pfizer","198600199");
            d.Add("pharmacopeia","200400357");
            d.Add("pharmascience","199200256");
            d.Add("pharming","199300356");
            d.Add("phytopharm","199100209");
            d.Add("pierre fabre","198700584");
            d.Add("piramal","199400120");
            d.Add("ppd","198700837");
            d.Add("proethic","200500155");
            d.Add("prostrakan","199800263");
            d.Add("protein delivery","199100400");
            d.Add("proteome sciences","200200490");
            d.Add("proximagen neuroscience","200400545");
            d.Add("psivida","200500553");
            d.Add("qlt","198700035");
            d.Add("ranbaxy","199000613");
            d.Add("ratiopharm","199400021");
            d.Add("reckitt benckiser","199000115");
            d.Add("recordati","198601005");
            d.Add("renovo","200001321");
            d.Add("repair","200000542");
            d.Add("replidyne","200200552");
            d.Add("respironics","198601203");
            d.Add("retroscreen","201200299");
            d.Add("rib-x pharmaceuticals","200200095");
            d.Add("ruxton pharmaceuticals","200400611");
            d.Add("sanguine","199400264");
            d.Add("sanofi-aventis","198601345");
            d.Add("schering-plough","198600260");
            d.Add("schwabe","201200150");
            d.Add("sciele pharma","199700395");
            d.Add("sepracor","198600347");
            d.Add("sequenom","199600433");
            d.Add("shionogi","198700350");
            d.Add("shire","198601293");
            d.Add("sidus","198800569");
            d.Add("siga","199700257");
            d.Add("sigma-tau","198700534");
            d.Add("simcere pharmaceuticals","200600652");
            d.Add("sinclair pharma","199000168");
            d.Add("skyepharma","199200414");
            d.Add("solvay","198600973");
            d.Add("sonus pharmaceuticals","199300077");
            d.Add("sosei","199700078");
            d.Add("stiefel laboratories","199000425");
            d.Add("supergen","199100265");
            d.Add("swedish orphan","200100680");
            d.Add("taisho","198700590");
            d.Add("takeda","198600337");
            d.Add("taro pharmaceutical","200000010");
            d.Add("taurx therapeutics","201000238");
            d.Add("teijin","198600626");
            d.Add("terumo","198700811");
            d.Add("teva","198601169");
            d.Add("theraquest biosciences","200400553");
            d.Add("thrombogenics","200101324");
            d.Add("tigenix","200300560");
            d.Add("tolerrx","200100825");
            d.Add("toray","198700013");
            d.Add("toyama","198800549");
            d.Add("transdermal","199901331");
            d.Add("transgene","198700558");
            d.Add("tripep","200200556");
            d.Add("ucb","198900659");
            d.Add("unigene","198600551");
            d.Add("urigen","199300312");
            d.Add("valeant","198600166");
            d.Add("vantia","200800216");
            d.Add("vasogen","200100069");
            d.Add("ventech","198600445");
            d.Add("vernalis","198700854");
            d.Add("vertex pharmaceuticals","198900222");
            d.Add("vion pharmaceuticals","199200386");
            d.Add("viralytics","201400183");
            d.Add("viropharma","199500305");
            d.Add("vivalis","200300399");
            d.Add("wellstat","200900459");
            d.Add("wilex","199902912");
            d.Add("wockhardt","200200261");
            d.Add("xenome","200101271");
            d.Add("xention","201100036");
            d.Add("xigen","200300238");
            d.Add("yaupon therapeutics","200400876");
            d.Add("ym biosciences","200000253");
            d.Add("zambon","198800493");
            d.Add("zelos therapeutics","200300266");
            
            return d;
        }

        #endregion Methods
    }

    public class ToInformaContentType : ListToGuid
    {
        public ToInformaContentType(Item i) : base(i) { }

        #region Methods

        public override void FillField(IDataMap map, ref Item newItem, string importValue)
        {
            if (string.IsNullOrEmpty(importValue))
                return;

            //get parent item of list to search
            Item i = newItem.Database.GetItem(SourceList);
            if (i == null)
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
            IEnumerable<Item> t = i.GetChildren().Where(c => c.DisplayName.Equals(cleanName));
            
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
                f.Value = (f.Value.Length > 0) ? $"{f.Value}|{ctID}" : ctID;
        }

        private Dictionary<string, string> GetMapping()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();

            d.Add("analysis","Analysis");
            d.Add("analyst","Analysis");
            d.Add("asia","News");
            d.Add("briefstory","News");
            d.Add("business","News");
            d.Add("business news","News");
            d.Add("clinical research","News");
            d.Add("comment","Opinion");
            d.Add("commentary","Opinion");
            d.Add("companies","News");
            d.Add("conference reports","News");
            d.Add("dataofday","News");
            d.Add("drug delivery","News");
            d.Add("ece_incoming","News");
            d.Add("edcarticle","News");
            d.Add("editorial","Opinion");
            d.Add("event stories","News");
            d.Add("events","News");
            d.Add("eventstory","News");
            d.Add("executive briefing","News");
            d.Add("executivebriefing","News");
            d.Add("expert view","Analysis");
            d.Add("expertview","Analysis");
            d.Add("face-to-face","News");
            d.Add("feature","News");
            d.Add("financials","News");
            d.Add("headline news","News");
            d.Add("highlight","News");
            d.Add("home","News");
            d.Add("international news","News");
            d.Add("interview","Analysis");
            d.Add("laboratory","News");
            d.Add("life sciences","News");
            d.Add("main","News");
            d.Add("mainstory","News");
            d.Add("market insight","Analysis");
            d.Add("market sector","News");
            d.Add("marketdata","Analysis");
            d.Add("marketinsight","Analysis");
            d.Add("markets","News");
            d.Add("medical","News");
            d.Add("multimedia","News");
            d.Add("news","News");
            d.Add("patent","News");
            d.Add("patent watch","News");
            d.Add("pdflibrary","News");
            d.Add("people","News");
            d.Add("policy & regulation","News");
            d.Add("policy & regulation news","News");
            d.Add("policy and regulation","News");
            d.Add("process","News");
            d.Add("product liability","News");
            d.Add("products","News");
            d.Add("r & d news","News");
            d.Add("randd","News");
            d.Add("regular","News");
            d.Add("regulation","News");
            d.Add("regulatory","News");
            d.Add("researchdevelopment","News");
            d.Add("science and technology","News");
            d.Add("scripinsight","News");
            d.Add("scriptinsight","News");
            d.Add("stock tracker","News");
            d.Add("webinar","News");

            return d;
        } 

        #endregion Methods

    }

    public class ToInformaTherapyAreas : ListToGuid
    {
        public ToInformaTherapyAreas(Item i) : base(i) { }

        #region Methods

        public override void FillField(IDataMap map, ref Item newItem, string importValue)
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
                    return;
                }

                string[] parts = transformValue.Split(new string[] {"::"}, StringSplitOptions.RemoveEmptyEntries);

                ChildList cl = i.GetChildren();

                
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

            d.Add("abdominal aortic aneurysm","Cardiovascular");
            d.Add("abnormal uterine bleeding","Gynecology & Urology");
            d.Add("abortion","Gynecology & Urology");
            d.Add("absence epilepsy","Neurology");
            d.Add("acidosis","Blood &Coagulation Disorders");
            d.Add("acne","Dermatology");
            d.Add("acromegaly","Metabolic Disorders");
            d.Add("actinic keratosis","Dermatology");
            d.Add("acute coronary syndrome","Cardiovascular");
            d.Add("acute lymphocytic leukaemia","Cancer");
            d.Add("acute myelogenous leukaemia","Cancer");
            d.Add("acute renal failure","Renal");
            d.Add("addiction","Neurology");
            d.Add("addison's disease","Metabolic Disorders");
            d.Add("adenosine deaminase deficiency","Metabolic Disorders");
            d.Add("adenovirus","Infectious Diseases");
            d.Add("adhesive capsulitis","");
            d.Add("adrenal cancer","Cancer");
            d.Add("adrenoleukodystrophy","Metabolic Disorders::Neurology");
            d.Add("adult respiratory distress syndrome","Respiratory");
            d.Add("aesthetics","Aesthetics");
            d.Add("african trypanosomiasis", "Infectious Diseases");
            d.Add("age-related macular degeneration","Ophthalmic");
            d.Add("aids-related dementia", "Immune Disorders::Neurology");
            d.Add("alcohol addiction","Neurology");
            d.Add("alcohol intolerance","Neurology");
            d.Add("allergic rhinitis", "Ear, Nose &Throat::Immune Disorders");
            d.Add("allergy","Immune Disorders");
            d.Add("alopecia", "Dermatology");
            d.Add("alpha-mannosidosis","");
            d.Add("alzheimer's disease","Neurology");
            d.Add("amenorrhoea","Gynecology & Urology");
            d.Add("american trypanosomiasis","Infectious Diseases");
            d.Add("amnesia","Neurology");
            d.Add("amyloidosis","Metabolic Disorders::Inflammation");
            d.Add("amyotrophic lateral sclerosis","Neurology");
            d.Add("anaemia","Blood & Coagulation Disorders");
            d.Add("anaesthesia","Neurology");
            d.Add("anal dysplasia","");
            d.Add("anal fissure","");
            d.Add("anal fistula","");
            d.Add("anaphylaxis","Immune Disorders");
            d.Add("androgenic alopecia","Aesthetics");
            d.Add("anemia","Blood &Coagulation Disorders");
            d.Add("aneurysm", "Cardiovascular");
            d.Add("angina", "Cardiovascular");
            d.Add("ankylosing spondylitis","Inflammation::Orthopedics");
            d.Add("anorexia", "Neurology");
            d.Add("anorexia nervosa","Neurology");
            d.Add("anthrax","Infectious Diseases");
            d.Add("antibiotic Resistance","Infectious Diseases");
            d.Add("antithrombin iii deficiency","");
            d.Add("anxiety", "Neurology");
            d.Add("aphthous ulcer","");
            d.Add("apnoea","Cardiovascular");
            d.Add("appendicitis","Gastrointestinal");
            d.Add("arrhythmia","Cardiovascular");
            d.Add("arterial thrombosis","Cardiovascular");
            d.Add("arthritis","Orthopedics");
            d.Add("aspergillus","Immune Disorders");
            d.Add("asthma","Respiratory");
            d.Add("atherosclerosis","Cardiovascular");
            d.Add("atopic eczema", "Immune Disorders::Inflammation");
            d.Add("atrial fibrillation","Cardiovascular");
            d.Add("atrial tachycardia","Cardiovascular");
            d.Add("attention deficit disorder","Neurology");
            d.Add("attention deficit hyperactivity disorder","Neurology");
            d.Add("autism","Neurology");
            d.Add("autoimmune disorders", "Immune Disorders");
            d.Add("bacterial","Infectious Diseases");
            d.Add("barrett's esophagus","Gastrointestinal");
            d.Add("Barrett's oesophagus","Gastrointestinal");
            d.Add("basal cell cancer","Cancer");
            d.Add("basal cell carcinoma", "Cancer");
            d.Add("b-cell lymphoma", "Cancer");
            d.Add("behcet's disease","");
            d.Add("benign prostate hypertrophy","Gynecology & Urology");
            d.Add("benign prostatic hyperplasia", "Gynecology & Urology");
            d.Add("biliary cancer","Cancer");
            d.Add("biliary cirrhosis","Gastrointestinal");
            d.Add("bipolar depression","Neurology");
            d.Add("bipolar disorder","Neurology");
            d.Add("bladder cancer","Cancer");
            d.Add("bladder disease","Gynecology & Urology");
            d.Add("blepharitis","");
            d.Add("blepharospasm","");
            d.Add("blood & coagulation disorders","Blood & Coagulation Disorders");
            d.Add("blood cancer","Cancer");
            d.Add("blood substitutes","Blood & Coagulation Disorders");
            d.Add("blood volume","Blood &Coagulation Disorders");
            d.Add("bone","Orthopedics");
            d.Add("bone cancer","Cancer");
            d.Add("bone disorder","");
            d.Add("bone regeneration","");
            d.Add("bowel incontinence","Gastrointestinal");
            d.Add("bradycardia","Cardiovascular");
            d.Add("brain cancer","Cancer");
            d.Add("brain inflammation","Neurology::Inflammation");
            d.Add("breast cancer","Cancer");
            d.Add("bronchiectasis","");
            d.Add("bronchitis","Respiratory");
            d.Add("bulimia","");
            d.Add("bunyaviridae","");
            d.Add("burkholderia pseudomallei","");
            d.Add("burns","Wound Healing &Tissue Repair");
            d.Add("bursitis","Orthopedics::Inflammation");
            d.Add("cachexia","");
            d.Add("campylobacter","");
            d.Add("cancer","Cancer");
            d.Add("cancer fatigue","Cancer");
            d.Add("cancer pain","Cancer");
            d.Add("candida","Infectious Diseases");
            d.Add("candida albicans","Infectious Diseases");
            d.Add("carcinoid","");
            d.Add("carcinoid syndrome","");
            d.Add("cardiac injury","Cardiovascular");
            d.Add("cardiovascular","Cardiovascular");
            d.Add("carpal tunnel syndrome","Orthopedics");
            d.Add("cartilage regeneration","Orthopedics");
            d.Add("cartilage repair","Orthopedics");
            d.Add("castleman's disease","");
            d.Add("cataplexy","");
            d.Add("cataract","Ophthalmic");
            d.Add("cerebral haemorrhage","Cardiovascular");
            d.Add("cerebral infarction","Cardiovascular");
            d.Add("cerebral ischaemia","Cardiovascular");
            d.Add("cerebral oedema","Cardiovascular");
            d.Add("cerebral thrombosis","Cardiovascular");
            d.Add("cerebral vasospasm","Cardiovascular");
            d.Add("cervical cancer","Cancer");
            d.Add("cervical dysplasia","Gynecology & Urology");
            d.Add("cervical dystonia","Gynecology &Urology");
            d.Add("chlamydia","Infectious Diseases");
            d.Add("cholera","Infectious Diseases");
            d.Add("cholesterol","Cardiovascular");
            d.Add("chronic fatigue syndrome","Neurology::Immune Disorders");
            d.Add("chronic granulomatous disease","");
            d.Add("chronic inflammatory demyelinating polyneuropathy","");
            d.Add("chronic lymphocytic leukaemia","Cancer");
            d.Add("chronic myelogenous leukaemia","Cancer");
            d.Add("chronic obstructive pulmonary disease","Cardiovascular");
            d.Add("chronic obstructive pulmonary disease(copd)","Respiratory");
            d.Add("chronic progressive multiple sclerosis","Neurology");
            d.Add("chronic renal failure","Renal");
            d.Add("churg-strauss syndrome","");
            d.Add("cirrhosis","Liver & Hepatic");
            d.Add("clostridium","");
            d.Add("clostridium botulinum","");
            d.Add("clostridium difficile","Infectious Diseases");
            d.Add("clotting disorders","Blood & Coagulation Disorders");
            d.Add("cluster headache","Neurology");
            d.Add("cns","Neurology");
            d.Add("cns cancer","Cancer");
            d.Add("cocaine addiction","Neurology");
            d.Add("cockayne syndrome","");
            d.Add("coeliac disease","Gastrointestinal");
            d.Add("cognitive disorders","Neurology");
            d.Add("cold","Respiratory");
            d.Add("colitis","Gastrointestinal");
            d.Add("colon cancer","Cancer");
            d.Add("colorectal cancer","Cancer");
            d.Add("complex regional pain","Neurology");
            d.Add("congenital adrenal hyperplasia","");
            d.Add("conjunctivitis","Ophthalmic");
            d.Add("constipation","Gastrointestinal");
            d.Add("contraceptive","Gynecology & Urology");
            d.Add("coronary artery bypass grafting","Cardiovascular");
            d.Add("coronary artery disease","Cardiovascular");
            d.Add("coronary thrombosis","Cardiovascular");
            d.Add("coronavirus","Infectious Diseases");
            d.Add("cough","Respiratory");
            d.Add("coxsackievirus","Infectious Diseases");
            d.Add("creutzfeldt-jakob disease","");
            d.Add("crohn's disease","Gastrointestinal");
            d.Add("croup","Infectious Diseases");
            d.Add("cryptosporidiosis","");
            d.Add("cushing's disease","");
            d.Add("cystic fibrosis","Respiratory");
            d.Add("cystinosis","");
            d.Add("cystitis","Gynecology &Urology");
            d.Add("cytomegalovirus","Infectious Diseases");
            d.Add("deafness","Ear,Nose & Throat");
            d.Add("deep vein thrombosis","Cardiovascular");
            d.Add("dengue fever","Infectious Diseases");
            d.Add("dengue virus","Infectious Diseases");
            d.Add("dental-oral","Dental-Oral");
            d.Add("depression","Neurology");
            d.Add("dermal inflammation","");
            d.Add("dermatitis","Dermatology");
            d.Add("dermatological","Dermatology");
            d.Add("dermatology","Dermatology");
            d.Add("diabetes","Metabolic Disorders");
            d.Add("diabetic cardiomyopathy","Metabolic Disorders::Cardiovascular");
            d.Add("diabetic complication","Metabolic Disorders");
            d.Add("diabetic macular oedema","Metabolic Disorders::Ophthalmic");
            d.Add("diabetic nephropathy","Metabolic Disorders::Neurology");
            d.Add("diabetic neuropathy","Metabolic Disorders::Neurology");
            d.Add("diabetic retinopathy","Metabolic Disorders::Ophthalmic");
            d.Add("diabetic ulcer", "Wound Healing & Tissue Repair");
            d.Add("diabetic ulcers","Wound Healing & Tissue Repair");
            d.Add("diagnostic","");
            d.Add("diarrhea","Gastrointestinal");
            d.Add("diarrhoea","Gastrointestinal");
            d.Add("dilated cardiomyopathy","Cardiovascular");
            d.Add("diphtheria","Infectious Diseases");
            d.Add("diptheria","Infectious Diseases");
            d.Add("disseminated intravascular coagulation","");
            d.Add("distributive shock","");
            d.Add("diverticulitis","");
            d.Add("down's syndrome","Neurology");
            d.Add("drug addiction","Neurology");
            d.Add("drug poisoning","Poisoning");
            d.Add("dry eye","Ophthalmic");
            d.Add("duodenal ulcer","Gastrointestinal");
            d.Add("dupuytren's disease","");
            d.Add("dwarfism","");
            d.Add("dyskinesia","");
            d.Add("dysmenorrhoea","Gynecology & Urology");
            d.Add("dyspareunia","");
            d.Add("dyspepsia","");
            d.Add("dysplasia","");
            d.Add("dystonia","");
            d.Add("dysuria","");
            d.Add("ear, nose & throat","Ear, Nose & Throat");
            d.Add("ear, nose &throat", "Ear, Nose & Throat");
            d.Add("eastern equine encephalitis virus","Infectious Diseases");
            d.Add("eating disorders","Metabolic Disorders");
            d.Add("ebola virus","Infectious Diseases");
            d.Add("ectodermal dysplasia","");
            d.Add("eczema","Dermatology");
            d.Add("emotional lability","");
            d.Add("emphysema","Respiratory");
            d.Add("encephalitis","Infectious Diseases");
            d.Add("endocrine","Metabolic Disorders");
            d.Add("endocrine cancer","Cancer");
            d.Add("endometrial cancer","Cancer");
            d.Add("endometriosis","Gynecology & Urology");
            d.Add("end-stage renal disease","Renal");
            d.Add("enterovirus 71","Infectious Diseases");
            d.Add("enuresis","");
            d.Add("epidermolysis bullosa","");
            d.Add("epilepsy","Neurology");
            d.Add("epstein barr","Infectious Diseases");
            d.Add("epstein-barr virus","Infectious Diseases");
            d.Add("erectile dysfunction","Gynecology & Urology");
            d.Add("escherichia coli","Infectious Diseases");
            d.Add("esophageal cancer","Cancer");
            d.Add("esophagitis","Gastrointestinal");
            d.Add("ewing's sarcoma","Cancer");
            d.Add("fabry disease","Metabolic Disorders");
            d.Add("fabry's disease","");
            d.Add("factor xiii deficiency","");
            d.Add("fallopian tube cancer","Cancer");
            d.Add("familial cold autoinflammatory syndrome","");
            d.Add("fat malabsorption","");
            d.Add("female","Gynecology & Urology");
            d.Add("female contraception","Gynecology &Urology");
            d.Add("female infertility","Gynecology &Urology");
            d.Add("female sexual dysfunction","Gynecology & Urology");
            d.Add("fever","");
            d.Add("fibroids","Gynecology & Urology");
            d.Add("fibromyalgia","Immune Disorders");
            d.Add("fibrosarcoma","Cancer");
            d.Add("fibrosis","");
            d.Add("filariasis","Infectious Diseases");
            d.Add("flatulence","Gastrointestinal");
            d.Add("food allergy","Immune Disorders");
            d.Add("fractures","Orthopedics");
            d.Add("fragile x syndrome","");
            d.Add("francisella tularensis","");
            d.Add("friedreich's ataxia","");
            d.Add("fungal","Infectious Diseases");
            d.Add("fungal infection","Infectious Diseases");
            d.Add("gallstone","Liver &Hepatic");
            d.Add("gastointestinal","Gastrointestinal");
            d.Add("gastric ulcer","Gastrointestinal");
            d.Add("gastritis","Gastrointestinal");
            d.Add("gastroenteritis","Gastrointestinal");
            d.Add("gastroesophageal Reflux Disorder (Gerd)","Gastrointestinal");
            d.Add("gastrointestinal","Gastrointestinal");
            d.Add("gastrointestinal cancer","Cancer");
            d.Add("gastrointestinal ulcer","Gastrointestinal");
            d.Add("gastrokinetic","Gastrointestinal");
            d.Add("gastro-oesophageal reflux","Gastrointestinal");
            d.Add("gastroparesis","Gastrointestinal");
            d.Add("gaucher disease","Metabolic Disorders");
            d.Add("gaucher's disease","Metabolic Disorders");
            d.Add("gelineau's syndrome","");
            d.Add("generalized anxiety disorder","Neurology");
            d.Add("genital warts","Infectious Diseases");
            d.Add("genitourinary","Gynecology & Urology");
            d.Add("gi injury","");
            d.Add("glaucoma","Ophthalmic");
            d.Add("globoid cell leukodystrophy","");
            d.Add("glomerulonephritis","");
            d.Add("gonorrhea","Gynecology & Urology::Infectious Diseases");
            d.Add("gonorrhoea","Gynecology & Urology::Infectious Diseases");
            d.Add("gouty arthritis","Immune Disorders");
            d.Add("gram-negative","Infectious Diseases");
            d.Add("gram-positive","Infectious Diseases");
            d.Add("granulocytopenia","");
            d.Add("growth disorders","Metabolic Disorders");
            d.Add("growth hormone deficiency","Metabolic Disorders");
            d.Add("guillain-barre syndrome","");
            d.Add("gynaecomastia","");
            d.Add("gynecology & urology","Gynecology & Urology");
            d.Add("haemolytic anaemia","");
            d.Add("haemolytic uraemic syndrome","");
            d.Add("haemophilia","Blood & Coagulation Disorders");
            d.Add("haemophilia a","Blood & Coagulation Disorders");
            d.Add("haemophilia b","Blood & Coagulation Disorders");
            d.Add("haemophilus influenzae","Blood & Coagulation Disorders");
            d.Add("haemorrhage","");
            d.Add("haemorrhagic fever","Infectious Diseases");
            d.Add("hair loss","Dermatology");
            d.Add("hairy cell leukaemia","Cancer");
            d.Add("head and neck cancer","Cancer");
            d.Add("head trauma","");
            d.Add("headache","Neurology");
            d.Add("hearing loss", "Ear, Nose & Throat::Neurology");
            d.Add("heart failure","Cardiovascular");
            d.Add("heart valve Disease","Cardiovascular");
            d.Add("helicobacter pylori","Infectious Diseases");
            d.Add("hemophilia","Blood & Coagulation Disorders");
            d.Add("hemorrhoid","Wound Healing & Tissue Repair");
            d.Add("hepatic","Liver & Hepatic");
            d.Add("hepatic cirrhosis","Gastrointestinal");
            d.Add("hepatic dysfunction","Gastrointestinal");
            d.Add("hepatic encephalopathy","Infectious Diseases");
            d.Add("hepatitis a","Infectious Diseases");
            d.Add("hepatitis b","Infectious Diseases");
            d.Add("hepatitis c","Infectious Diseases");
            d.Add("hepatitis virus","Infectious Diseases::Gastrointestinal");
            d.Add("hepatitis-b","Infectious Diseases::Gastrointestinal");
            d.Add("hepatitis-b virus","Infectious Diseases::Gastrointestinal");
            d.Add("hepatitis-c","Infectious Diseases::Gastrointestinal");
            d.Add("hepatitis-e","Infectious Diseases::Gastrointestinal");
            d.Add("hereditary blindness","Ophthalmic");
            d.Add("hereditary tyrosinaemia","");
            d.Add("herpes","Infectious Diseases");
            d.Add("herpes simplex","Infectious Diseases");
            d.Add("herpetic keratitis","");
            d.Add("hips","Orthopedics");
            d.Add("hirsutism","");
            d.Add("hiv","Infectious Diseases");
            d.Add("hiv/aids","Infectious Diseases");
            d.Add("hiv-aids","Infectious Diseases");
            d.Add("hodgkin's disease","Cancer");
            d.Add("hodgkin's lymphoma","Cancer");
            d.Add("homocystinuria","");
            d.Add("hormonal","Metabolic Disorders");
            d.Add("hormone replacement","Gynecology & Urology");
            d.Add("hormone replacement therapy","");
            d.Add("htlv","");
            d.Add("human metapneumovirus","Infectious Diseases");
            d.Add("human papilloma virus","Infectious Diseases");
            d.Add("human papilloma virus(hpv)","Infectious Diseases");
            d.Add("huntington's disease","Neurology");
            d.Add("hyperammonaemia","");
            d.Add("hyperbilirubinaemia","");
            d.Add("hypercalcaemia","");
            d.Add("hypercalcaemia of malignancy","");
            d.Add("hypercalcemia","Metabolic Disorders");
            d.Add("hypercholesterolaemia","Metabolic Disorders");
            d.Add("hypereosinophilic syndrome","");
            d.Add("hyperhidrosis","");
            d.Add("hyperkalaemia","");
            d.Add("hyperlipidaemia","Metabolic Disorders");
            d.Add("hyperlipidemia","Metabolic Disorders");
            d.Add("hyperoxaluria","");
            d.Add("hyperparathyroidism","");
            d.Add("hyperphenylalaninaemia","");
            d.Add("hyperphosphataemia","Metabolic Disorders");
            d.Add("hyperphsphatemia","Metabolic Disorders");
            d.Add("hyperprolactinaemia","");
            d.Add("hypertension","Cardiovascular");
            d.Add("hyperthyroidism","Immune Disorders::Metabolic Disorders");
            d.Add("hypertriglyceridaemia","");
            d.Add("hyperuricaemia","");
            d.Add("hypoalbuminaemia","");
            d.Add("hypoglycaemia","");
            d.Add("hypogonadism","Metabolic Disorders");
            d.Add("hyponatraemia","");
            d.Add("hypoparathyroidism","Immune Disorders::Metabolic Disorders");
            d.Add("hypophosphataemia","");
            d.Add("hypotension","Cardiovascular");
            d.Add("hypothyroidism","Immune Disorders::Metabolic Disorders");
            d.Add("ichthyosis","");
            d.Add("idiopathic myelofibrosis","");
            d.Add("ileus","");
            d.Add("immune disorders","Immune Disorders");
            d.Add("immunodeficiency","Immune Disorders");
            d.Add("immunological","Immune Disorders");
            d.Add("impetigo","");
            d.Add("impotence","Gynecology & Urology");
            d.Add("incontinence","Gynecology & Urology");
            d.Add("induction","");
            d.Add("infant respiratory distress syndrome","");
            d.Add("infarction","");
            d.Add("infection","Infectious Diseases");
            d.Add("infectious diseases","Infectious Diseases");
            d.Add("infertility","Gynecology & Urology");
            d.Add("inflammation","Inflammation");
            d.Add("inflammatory bowel disease","Gastrointestinal");
            d.Add("influenza","Infectious Diseases");
            d.Add("insomnia","Neurology");
            d.Add("insulin-related metabolic syndrome","Metabolic Disorders");
            d.Add("iron disorders","Blood & Coagulation Disorders");
            d.Add("irritable bowel syndrome","Gastrointestinal");
            d.Add("ischaemia","");
            d.Add("ischaemic cardiomyopathy","");
            d.Add("ischaemic optic neuropathy","");
            d.Add("ischemia","Cardiovascular");
            d.Add("japanese encephalitis","Infectious Diseases");
            d.Add("jaundice","Liver & Hepatic");
            d.Add("kaposi's sarcoma","Cancer");
            d.Add("kawasaki disease","");
            d.Add("keloid","Wound Healing & Tissue Repair");
            d.Add("keratoconjunctivitis","");
            d.Add("keratoconus","");
            d.Add("keratosis","");
            d.Add("kidney disease","Renal");
            d.Add("kidney failure","Renal");
            d.Add("kidney stones","Renal");
            d.Add("kidney transplant","Renal");
            d.Add("knees","Orthopedics");
            d.Add("lactic acidosis","");
            d.Add("lambert-Eaton myasthenic syndrome","");
            d.Add("lassa virus","Infectious Diseases");
            d.Add("lateral epicondylitis","");
            d.Add("leber's congenital amaurosis","");
            d.Add("leber's hereditary optic neuropathy","");
            d.Add("left atrial appendage","Cardiovascular");
            d.Add("leiomyosarcoma","Cancer");
            d.Add("leishmaniasis","Infectious Diseases");
            d.Add("lennox-bastaut syndrome","");
            d.Add("leprechaunism","");
            d.Add("leprosy","Infectious Diseases");
            d.Add("leukaemia","Cancer");
            d.Add("leukemia","Cancer");
            d.Add("leukopenia","");
            d.Add("leukoplakia","");
            d.Add("li-fraumeni syndrome","");
            d.Add("lipodystrophy","Metabolic Disorders");
            d.Add("lipoma","");
            d.Add("liposarcoma","Cancer");
            d.Add("liver & hepatic","Liver & Hepatic");
            d.Add("liver cancer","Cancer");
            d.Add("liver failure","Liver & Hepatic");
            d.Add("liver fibrosis","Gastrointestinal");
            d.Add("long qt syndrome","Cardiovascular");
            d.Add("lookup","Top Level");
            d.Add("lower respiratory tract infection","Cardiovascular");
            d.Add("lung cancer","Cancer");
            d.Add("lupus","Immune Disorders");
            d.Add("lupus erythematosus","Immune Disorders");
            d.Add("lupus nephritis","Immune Disorders");
            d.Add("lyme disease","Infectious Diseases");
            d.Add("lymphoma","Cancer");
            d.Add("macular degeneration","Ophthalmic");
            d.Add("macular oedema","Ophthalmic");
            d.Add("major depressive disorder","Neurology");
            d.Add("malaria","Infectious Diseases");
            d.Add("male","Gynecology & Urology");
            d.Add("male contraception","Gynecology & Urology");
            d.Add("male infertility","Gynecology & Urology");
            d.Add("male sexual dysfunction","Gynecology & Urology");
            d.Add("malignant pleural effusion","");
            d.Add("marburg virus","Infectious Diseases");
            d.Add("mastalgia","");
            d.Add("mastocytosis","");
            d.Add("measles","Infectious Diseases");
            d.Add("meconium aspiration syndrome","");
            d.Add("melanoma","Cancer");
            d.Add("melasma","");
            d.Add("memory disorders","Neurology");
            d.Add("meniere's disease","");
            d.Add("meningitis","Infectious Diseases");
            d.Add("menopausal symptoms","Gynecology & Urology");
            d.Add("menopause","Gynecology & Urology");
            d.Add("menorrhagia","Gynecology & Urology");
            d.Add("mental retardation","Neurology");
            d.Add("mesothelioma","Cancer");
            d.Add("metabolic","Metabolic Disorders");
            d.Add("metabolic disorders","Metabolic Disorders");
            d.Add("metachromatic leukodystrophy","");
            d.Add("migraine","Neurology");
            d.Add("miscarriage","Gynecology & Urology");
            d.Add("mitochondrial disease","Metabolic Disorders");
            d.Add("mitochondrial encephalomyopathy","Metabolic Disorders");
            d.Add("motility dysfunction","Gynecology & Urology");
            d.Add("motor neurone disease","Neurology");
            d.Add("mrsa","Infectious Diseases");
            d.Add("mssa","");
            d.Add("muckle-wells syndrome","");
            d.Add("mucopolysaccharidosis","");
            d.Add("mucositis","");
            d.Add("multiple sclerosis","Immune Disorders::Neurology");
            d.Add("multiple system atrophy","");
            d.Add("mumps","Infectious Diseases");
            d.Add("muscle & connective tissue","Orthopedics");
            d.Add("muscle spasm","Orthopedics");
            d.Add("muscular atrophy","Immune Disorders");
            d.Add("muscular dystrophy","Neurology");
            d.Add("musculoskeletal","Orthopedics");
            d.Add("musculoskeletal pain","Orthopedics::Neurology");
            d.Add("myasthenia gravis","");
            d.Add("mycobacterium avium complex","");
            d.Add("mycobacterium ulcerans","");
            d.Add("myelodysplastic syndrome","Blood & Coagulation Disorders");
            d.Add("myeloma","Cancer");
            d.Add("myocardial fibrosis","");
            d.Add("myocardial infarction","Cardiovascular");
            d.Add("myocarditis","");
            d.Add("myoma","");
            d.Add("myopia","Ophthalmic");
            d.Add("narcolepsy","Neurology");
            d.Add("narcotic addiction","Neurology");
            d.Add("nasopharyngeal cancer","Cancer");
            d.Add("nausea and vomiting","Gastrointestinal");
            d.Add("neck cancer","Cancer");
            d.Add("necrotizing enterocolitis","");
            d.Add("neisseria meningitidis","");
            d.Add("nephritis","Renal");
            d.Add("nephropathy","Renal");
            d.Add("nerve injury","Neurology");
            d.Add("neuroblastoma","");
            d.Add("neurodegenerative disease","Neurology");
            d.Add("neurofibromatosis","");
            d.Add("neurology","Neurology");
            d.Add("neuropathic pain","Neurology");
            d.Add("neuropathy","Neurology");
            d.Add("neuroses","Neurology");
            d.Add("neutropenia","Blood & Coagulation Disorders");
            d.Add("nicotine addiction","Neurology");
            d.Add("niemann-pick disease","");
            d.Add("night blindness","Ophthalmic");
            d.Add("nocturnal polyuria","");
            d.Add("non-hodgkin's lymphoma","Cancer");
            d.Add("non-small cell lung cancer","Cancer");
            d.Add("norwalk virus","Infectious Diseases");
            d.Add("nosocomial","Infectious Diseases");
            d.Add("nutrition","");
            d.Add("obesity","Metabolic Disorders");
            d.Add("obsessive compulsive disorder","Neurology");
            d.Add("obsessive-compulsive disorder","Neurology");
            d.Add("obstetrics","Gynecology & Urology");
            d.Add("ocular infection","Ophthalmic::Infectious Diseases");
            d.Add("ocular inflammation","Ophthalmic");
            d.Add("oedema","");
            d.Add("oesophageal cancer","Cancer");
            d.Add("oesophageal ulcer","");
            d.Add("oesophagitis","");
            d.Add("onchocerciasis","");
            d.Add("onychomycosis","");
            d.Add("ophthalmic","Ophthalmic");
            d.Add("opiate addiction","Neurology");
            d.Add("oral cancer","Cancer");
            d.Add("organ transplants","Immune Disorders");
            d.Add("orthopedics","Orthopedics");
            d.Add("osteoarthritis","Orthopedics");
            d.Add("osteodystrophy","");
            d.Add("osteogenesis imperfecta","");
            d.Add("osteomalacia","");
            d.Add("osteonecrosis","");
            d.Add("osteoporosis","Orthopedics");
            d.Add("osteosarcoma","Cancer");
            d.Add("otitis","Ear, Nose & Throat");
            d.Add("ovarian cancer","Cancer");
            d.Add("overactive bladder","Gynecology & Urology");
            d.Add("paget's disease","");
            d.Add("pain","Neurology");
            d.Add("pancreatic cancer","Cancer");
            d.Add("pancreatic dysfunction","Gastrointestinal");
            d.Add("pancreatic insufficiency","Gastrointestinal");
            d.Add("pancreatitis","Gastrointestinal");
            d.Add("panic disorder","Neurology");
            d.Add("parainfluenza virus","Infectious Diseases");
            d.Add("parasitic","Infectious Diseases");
            d.Add("parkinson's disease","Neurology");
            d.Add("partial epilepsy","Neurology");
            d.Add("parvovirus","Infectious Diseases");
            d.Add("patent ductus arteriosus","");
            d.Add("patent foramen ovale(pfo)","Cardiovascular");
            d.Add("pemphigus","");
            d.Add("perennial allergic rhinitis","Immune Disorders");
            d.Add("periodontitis","Dental-Oral");
            d.Add("peripheral vascular disease","Cardiovascular");
            d.Add("peritoneal cancer","Cancer");
            d.Add("pernicious anaemia","Blood & Coagulation Disorders");
            d.Add("pertussis infection","Infectious Diseases");
            d.Add("peyronie's disease","");
            d.Add("pharyngitis","Ear, Nose & Throat");
            d.Add("phobia","Neurology");
            d.Add("photodamage","");
            d.Add("pigmentation disorders","Dermatology");
            d.Add("plasma substitute","");
            d.Add("pneumococcal infection","Infectious Diseases");
            d.Add("pneumocystis jiroveci","Infectious Diseases");
            d.Add("pneumonia","Respiratory");
            d.Add("poisoning","Poisoning");
            d.Add("polio","Infectious Diseases::Neurology");
            d.Add("pollakisuria","");
            d.Add("polycystic ovarian syndrome","");
            d.Add("polycythaemia vera","");
            d.Add("pompe's disease","");
            d.Add("porphyria","");
            d.Add("portal hypertension","");
            d.Add("post-herpetic pain","");
            d.Add("post-operative pain","Neurology");
            d.Add("post-polio syndrome","Infectious Diseases::Neurology");
            d.Add("post-traumatic stress disorder","Neurology");
            d.Add("pouchitis","");
            d.Add("prader-willi syndrome","");
            d.Add("precocious puberty","Gynecology & Urology");
            d.Add("pre-eclampsia","Gynecology & Urology");
            d.Add("premature ejaculation","Gynecology & Urology");
            d.Add("premenstrual syndrome","Gynecology & Urology");
            d.Add("presbyopia","Ophthalmic");
            d.Add("pressure sores","Wound Healing & Tissue Repair");
            d.Add("preterm labour","Gynecology & Urology");
            d.Add("progressive multifocal leukoencephalopathy","Infectious Diseases");
            d.Add("prostate cancer","Cancer");
            d.Add("proteinuria","");
            d.Add("pruritus","");
            d.Add("pseudomonas","Infectious Diseases");
            d.Add("psoriasis","Dermatology");
            d.Add("psoriatic arthritis","Immune Disorders::Dermatology");
            d.Add("psychiatric disorders","Neurology");
            d.Add("psychosis","Neurology");
            d.Add("pulmonary","Respiratory");
            d.Add("pulmonary fibrosis","Cardiovascular");
            d.Add("pulmonary hypertension","Cardiovascular");
            d.Add("pulmonary inflammation","Cardiovascular");
            d.Add("pulmonary thrombosis","Cardiovascular");
            d.Add("rabies","Infectious Diseases::Neurology");
            d.Add("radiation poisoning","Poisoning");
            d.Add("raynaud's disease","Cardiovascular::Immune Disorders");
            d.Add("refractive errors","Ophthalmic");
            d.Add("regeneration","");
            d.Add("rejection","Immune Disorders");
            d.Add("relapsing-remitting multiple sclerosis","Neurology");
            d.Add("renal","Renal");
            d.Add("renal cancer","Cancer");
            d.Add("renal failure","Renal");
            d.Add("renal injury","Renal");
            d.Add("renal ischaemia","Renal");
            d.Add("reperfusion injury","Cardiovascular");
            d.Add("respiratory","Respiratory");
            d.Add("respiratory depression","Cardiovascular");
            d.Add("respiratory disease","Cardiovascular");
            d.Add("respiratory distress syndrome","Respiratory");
            d.Add("respiratory syncytial virus","Respiratory");
            d.Add("respiratory tract infection","Cardiovascular");
            d.Add("restenosis","Cardiovascular");
            d.Add("restless legs syndrome","Neurology");
            d.Add("retinal detachment","Ophthalmic");
            d.Add("retinal vein occlusion","Ophthalmic");
            d.Add("retinitis","Ophthalmic");
            d.Add("retinopathy","Ophthalmic");
            d.Add("rhabdomyosarcoma","Cancer");
            d.Add("rhesus haemolytic disease","");
            d.Add("rheumatoid arthritis","Immune Disorders");
            d.Add("rhinitis","Respiratory");
            d.Add("rhinovirus","Infectious Diseases");
            d.Add("rickets","");
            d.Add("rickettsia","");
            d.Add("rosacea","Dermatology");
            d.Add("rotavirus","Infectious Diseases");
            d.Add("rubella","Infectious Diseases");
            d.Add("salmonella","Infectious Diseases");
            d.Add("sarcoidosis","");
            d.Add("sarcoma","Cancer");
            d.Add("sarcopenia","");
            d.Add("sars","Infectious Diseases");
            d.Add("scabies","");
            d.Add("scarring","Wound Healing & Tissue Repair");
            d.Add("schistosoma","");
            d.Add("schistosomiasis","");
            d.Add("schizophrenia","Neurology");
            d.Add("scleroderma","Orthopedics");
            d.Add("seasonal allergic rhinitis","Immune Disorders");
            d.Add("seborrhoea","Dermatology");
            d.Add("seborrhoeic eczema","Dermatology");
            d.Add("senile dementia","Neurology");
            d.Add("sensory","");
            d.Add("sepsis","Infectious Diseases");
            d.Add("severe combined immunodeficiency","Immune Disorders");
            d.Add("sexual deviations","Neurology");
            d.Add("sexual health","Gynecology & Urology");
            d.Add("sexually transmitted diseases","Infectious Diseases");
            d.Add("shigella","");
            d.Add("short-bowel syndrome","Gastrointestinal");
            d.Add("sickle cell anaemia","Blood & Coagulation Disorders");
            d.Add("sinusitis","Immune Disorders");
            d.Add("sjogren's syndrome","Immune Disorders");
            d.Add("skin cancer","Cancer");
            d.Add("skin disorder","Dermatology");
            d.Add("skin infection","Immune Disorders::Infectious Diseases");
            d.Add("skin ulcers","Wound Healing & Tissue Repair");
            d.Add("sleep apnea","Respiratory");
            d.Add("sleep disorders","Neurology");
            d.Add("small bones","Orthopedics");
            d.Add("small cell lung cancer","Cancer");
            d.Add("smallpox","Infectious Diseases");
            d.Add("social anxiety disorder","Neurology");
            d.Add("soft tissue damage","Orthopedics");
            d.Add("soft tissue sarcoma","Cancer");
            d.Add("spastic paralysis","Neurology");
            d.Add("spinal cord injuries","Neurology");
            d.Add("spinal cord injury","Neurology");
            d.Add("spinal muscular atrophy","Neurology");
            d.Add("spine","Orthopedics");
            d.Add("spinocerebellar ataxia","");
            d.Add("squamous cell cancer","Cancer");
            d.Add("squamous cell carcinoma","Cancer");
            d.Add("staphylococcal","Infectious Diseases");
            d.Add("staphylococcus","Infectious Diseases");
            d.Add("std","Infectious Diseases::Gynecology & Urology");
            d.Add("steatohepatitis","Gastrointestinal");
            d.Add("steatorrhoea","");
            d.Add("stem cell mobilization","");
            d.Add("sterilization","Gynecology & Urology");
            d.Add("stomach cancer","Cancer");
            d.Add("stomatitis","");
            d.Add("strabismus","");
            d.Add("streptococcal","Infectious Diseases");
            d.Add("streptococcus","Infectious Diseases");
            d.Add("stress urinary incontinence","Gynecology & Urology");
            d.Add("stroke","Neurology");
            d.Add("structural heart disease","Cardiovascular");
            d.Add("subarachnoid haemorrhage","");
            d.Add("sucrase isomaltase deficiency","");
            d.Add("supraventricular tachycardia","Cardiovascular");
            d.Add("surgical wounds","Wound Healing & Tissue Repair");
            d.Add("synovial sarcoma","Cancer");
            d.Add("synovitis","");
            d.Add("systemic inflammatory response syndrome","");
            d.Add("tachycardia","Cardiovascular");
            d.Add("tardive dyskinesia","");
            d.Add("t-cell cancer","Cancer");
            d.Add("tendinitis","Orthopedics");
            d.Add("testicular cancer","Cancer");
            d.Add("tetanus","Infectious Diseases");
            d.Add("thalassaemia","Blood & Coagulation Disorders");
            d.Add("thalassemia","Blood & Coagulation Disorders");
            d.Add("thrombocytopenia","Blood & Coagulation Disorders");
            d.Add("thrombocytopenic purpura","Blood & Coagulation Disorders");
            d.Add("thrombocytosis","Blood & Coagulation Disorders");
            d.Add("thrombophlebitis","");
            d.Add("thromboprophylaxis","");
            d.Add("thrombosis","Cardiovascular");
            d.Add("thymoma","");
            d.Add("thyroid","Metabolic Disorders");
            d.Add("thyroid cancer","Cancer");
            d.Add("tick-borne encephalitis","Infectious Diseases");
            d.Add("tinnitus","Ear, Nose & Throat");
            d.Add("tonic-clonic epilepsy","");
            d.Add("tonsillitis","Ear, Nose & Throat");
            d.Add("toxoplasmosis","");
            d.Add("transplant rejection","Immune Disorders");
            d.Add("transverse myelitis","");
            d.Add("traumatic brain injury","Neurology");
            d.Add("trichomoniasis","");
            d.Add("trypanosomiasis","");
            d.Add("tuberculosis","Infectious Diseases");
            d.Add("tumors, liquid","Cancer");
            d.Add("tumors, solid","Cancer");
            d.Add("turner's syndrome","");
            d.Add("type 1 diabetes","Metabolic Disorders");
            d.Add("type 2 diabetes","Metabolic Disorders");
            d.Add("type i diabetes","Metabolic Disorders");
            d.Add("type ii diabetes","Metabolic Disorders");
            d.Add("typhoid","Infectious Diseases");
            d.Add("ulcer","Gastrointestinal");
            d.Add("ulcerative colitis","Gastrointestinal");
            d.Add("unstable angina","Cardiovascular");
            d.Add("unverricht-lundborg disease","");
            d.Add("upper respiratory tract infection","Cardiovascular::Infectious Diseases");
            d.Add("uraemia","");
            d.Add("urethritis","Gynecology & Urology");
            d.Add("urge incontinence","Gynecology & Urology");
            d.Add("urinary incontinence","Gynecology & Urology");
            d.Add("urinary retention","Gynecology & Urology");
            d.Add("urinary tract","Gynecology & Urology");
            d.Add("urinary tract infection","Gynecology & Urology");
            d.Add("urticaria","");
            d.Add("uterine bleeding","Gynecology & Urology");
            d.Add("uterine cancer","Cancer");
            d.Add("uterine fibrosis","Gynecology & Urology");
            d.Add("uveitis","Ophthalmic");
            d.Add("vaccine adjunct","Infectious Diseases");
            d.Add("vaginosis","Gynecology & Urology");
            d.Add("varicella zoster virus","Infectious Diseases");
            d.Add("varicose veins","Cardiovascular");
            d.Add("vascular dementia","Neurology");
            d.Add("vascular endothelial dysfunction","");
            d.Add("vasospasm","");
            d.Add("venezuelan equine encephalitis","Infectious Diseases");
            d.Add("venous insufficiency","Cardiovascular");
            d.Add("venous stasis ulcers","Wound Healing & Tissue Repair");
            d.Add("venous thrombosis","Cardiovascular");
            d.Add("ventricular fibrillation","Cardiovascular");
            d.Add("ventricular tachycardia","Cardiovascular");
            d.Add("vertebral compression fractures","Orthopedics");
            d.Add("vertigo", "Ear, Nose & Throat::Neurology");
            d.Add("vesicoureteral reflux","");
            d.Add("viral","Infectious Diseases");
            d.Add("vision correction","Ophthalmic");
            d.Add("vitamin d deficiency","Metabolic Disorders");
            d.Add("vitiligo","");
            d.Add("von willebrand's disease","");
            d.Add("water retention","");
            d.Add("wegener's granulomatosis","");
            d.Add("werner's syndrome","");
            d.Add("west nile encephalitis","Infectious Diseases");
            d.Add("western equine encephalitis","Infectious Diseases");
            d.Add("wilson's disease","");
            d.Add("wolff-parkinson-white syndrome","Cardiovascular");
            d.Add("wound","Wound Healing & Tissue Repair");
            d.Add("wound healing & tissue repair","Wound Healing & Tissue Repair");
            d.Add("xerophthalmia","");
            d.Add("xerostomia","");
            d.Add("yellow fever","Infectious Diseases");
            d.Add("tersinia pestis","");
            d.Add("zollinger-ellison syndrome","");

            return d;
        }

        #endregion Methods

    }

    public class InformaListToRegions : ListToGuid
    {
        public InformaListToRegions(Item i) : base(i) { }

        #region Methods

        public override void FillField(IDataMap map, ref Item newItem, string importValue)
        {
            if (string.IsNullOrEmpty(importValue))
                return;

            //get parent item of list to search
            Item i = newItem.Database.GetItem(SourceList);
            if (i == null)
                return;

            Dictionary<string, string> d = GetMapping();

            string transformValue = (d.ContainsKey(importValue)) ? d[importValue] : string.Empty;
            if (string.IsNullOrEmpty(transformValue))
            {
                map.Logger.Log(newItem.Paths.FullPath, "Region not converted", ProcessStatus.FieldError, NewItemField, importValue);
                return;
            }
            
            string cleanName = StringUtility.GetValidItemName(transformValue, map.ItemNameMaxLength);
            IEnumerable<Item> t = i.GetChildren().Where(c => c.DisplayName.Equals(cleanName));

            //if you find one then store the id
            if (!t.Any())
            {
                map.Logger.Log(newItem.Paths.FullPath, "Therapy Area(s) not found in list", ProcessStatus.FieldError, NewItemField, importValue);
                return;
            }

            Field f = newItem.Fields[NewItemField];
            if (f == null)
                return;

            string ctID = t.First().ID.ToString();
            if (!f.Value.Contains(ctID))
                f.Value = (f.Value.Length > 0) ? $"{f.Value}|{ctID}" : ctID;
        }


        public Dictionary<string, string> GetMapping()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("AFGHANISTAN", "Afghanistan");
            d.Add("ALBANIA", "Albania");
            d.Add("ANDORRA", "Andorra");
            d.Add("ANGOLA", "Angola");
            d.Add("ANGUILLA", "Anguilla");
            d.Add("ANTIGUA AND BARBUDA", "Antigua And Barbuda");
            d.Add("ARGENTINA", "Argentina");
            d.Add("ARMENIA", "Armenia");
            d.Add("AUSTRALIA", "Australia");
            d.Add("AUSTRIA", "Austria");
            d.Add("AZERBAIJAN", "Azerbaijan");
            d.Add("BAHRAIN", "Bahrain");
            d.Add("BANGLADESH", "Bangladesh");
            d.Add("BARBADOS", "Barbados");
            d.Add("BELARUS", "Belarus");
            d.Add("BELGIUM", "Belgium");
            d.Add("BELIZE", "Belize");
            d.Add("BENIN", "Benin");
            d.Add("BERMUDA", "Bermuda");
            d.Add("BHUTAN", "Bhutan");
            d.Add("BOLIVIA", "Bolivia");
            d.Add("BOSNIA AND HERZEGOVINA", "Bosnia And Herzegovina");
            d.Add("BOTSWANA", "Botswana");
            d.Add("BRAZIL", "Brazil");
            d.Add("BRUNEI DARUSSALAM", "Brunei Darussalam");
            d.Add("BULGARIA", "Bulgaria");
            d.Add("BURKINA FASO", "Burkina Faso");
            d.Add("BURUNDI", "Burundi");
            d.Add("CAMBODIA", "Cambodia");
            d.Add("CAMEROON", "Cameroon");
            d.Add("CANADA", "Canada");
            d.Add("CAPE VERDE", "Cape Verde");
            d.Add("CAYMAN ISLANDS", "Cayman Islands");
            d.Add("CENTRAL AFRICAN REPUBLIC", "Central African Republic");
            d.Add("CHAD", "Chad");
            d.Add("CHILE", "Chile");
            d.Add("CHINA", "China");
            d.Add("COLOMBIA", "Colombia");
            d.Add("COSTA RICA", "Costa Rica");
            d.Add("COTE DIVOIRE", "Cote Divoire");
            d.Add("CROATIA", "Croatia");
            d.Add("CUBA", "Cuba");
            d.Add("CYPRUS", "Cyprus");
            d.Add("CZECH REPUBLIC", "Czech Republic");
            d.Add("DENMARK", "Denmark");
            d.Add("DJIBOUTI", "Djibouti");
            d.Add("DOMINICA", "Dominica");
            d.Add("DOMINICAN REPUBLIC", "Dominican Republic");
            d.Add("ECUADOR", "Ecuador");
            d.Add("EL SALVADOR", "El Salvador");
            d.Add("EQUATORIAL GUINEA", "Equatorial Guinea");
            d.Add("ERITREA", "");
            d.Add("ESTONIA", "Estonia");
            d.Add("ETHIOPIA", "Ethiopia");
            d.Add("FIJI", "Fiji");
            d.Add("FINLAND", "Finland");
            d.Add("FRANCE", "France");
            d.Add("GABON", "Gabon");
            d.Add("GAMBIA", "Gambia");
            d.Add("GEORGIA", "Georgia");
            d.Add("GERMANY", "Germany");
            d.Add("GHANA", "Ghana");
            d.Add("GREECE", "Greece");
            d.Add("GUATEMALA", "Guatemala");
            d.Add("GUINEA", "Guinea");
            d.Add("GUINEA-BISSAU", "Guinea-Bissau");
            d.Add("GUYANA", "Guyana");
            d.Add("HAITI", "Haiti");
            d.Add("HONDURAS", "Honduras");
            d.Add("HONG KONG", "Hong Kong");
            d.Add("HUNGARY", "Hungary");
            d.Add("ICELAND", "Iceland");
            d.Add("INDIA", "India");
            d.Add("INDONESIA", "Indonesia");
            d.Add("IRAN", "Iran");
            d.Add("IRAQ", "Iraq");
            d.Add("IRELAND", "Ireland");
            d.Add("ISRAEL", "Israel");
            d.Add("ITALY", "Italy");
            d.Add("JAMAICA", "Jamaica");
            d.Add("JAPAN", "Japan");
            d.Add("JORDAN", "Jordan");
            d.Add("KAZAKHSTAN", "Kazakhstan");
            d.Add("KENYA", "Kenya");
            d.Add("KIRIBATI", "Kiribati");
            d.Add("KOREA", "South Korea");
            d.Add("KUWAIT", "Kuwait");
            d.Add("KYRGYZSTAN", "Kyrgyzstan");
            d.Add("LAOS", "Laos");
            d.Add("LATVIA", "Latvia");
            d.Add("LEBANON", "Lebanon");
            d.Add("LESOTHO", "Lesotho");
            d.Add("LIBERIA", "Liberia");
            d.Add("LIECHTENSTEIN", "Liechtenstein");
            d.Add("LITHUANIA", "Lithuania");
            d.Add("LUXEMBOURG", "Luxembourg");
            d.Add("MADAGASCAR", "Madagascar");
            d.Add("MALAWI", "Malawi");
            d.Add("MALAYSIA", "Malaysia");
            d.Add("MALDIVES", "Maldives");
            d.Add("MALI", "Mali");
            d.Add("MALTA", "Malta");
            d.Add("MAURITIUS", "Mauritius");
            d.Add("MEXICO", "Mexico");
            d.Add("MOLDOVA", "Moldova");
            d.Add("MONGOLIA", "Mongolia");
            d.Add("MONTENEGRO", "Montenegro");
            d.Add("MOZAMBIQUE", "Mozambique");
            d.Add("MYANMAR", "Myanmar");
            d.Add("NAMIBIA", "Namibia");
            d.Add("NEPAL", "Nepal");
            d.Add("NETHERLANDS", "Netherlands");
            d.Add("NEW ZEALAND", "New Zealand");
            d.Add("NICARAGUA", "Nicaragua");
            d.Add("NIGER", "Niger");
            d.Add("NIGERIA", "Nigeria");
            d.Add("NORWAY", "Norway");
            d.Add("OMAN", "Oman");
            d.Add("PAKISTAN", "Pakistan");
            d.Add("PANAMA", "Panama");
            d.Add("PAPUA NEW GUINEA", "Papua New Guinea");
            d.Add("PARAGUAY", "Paraguay");
            d.Add("PERU", "Peru");
            d.Add("PHILIPPINES", "Philippines");
            d.Add("POLAND", "Poland");
            d.Add("PORTUGAL", "Portugal");
            d.Add("PUERTO RICO", "Puerto Rico");
            d.Add("QATAR", "Qatar");
            d.Add("ROMANIA", "Romania");
            d.Add("RUSSIAN FEDERATION", "Russian Federation");
            d.Add("RWANDA", "Rwanda");
            d.Add("SAMOA", "Samoa");
            d.Add("SAUDI ARABIA", "Saudi Arabia");
            d.Add("SENEGAL", "Senegal");
            d.Add("SERBIA", "Serbia");
            d.Add("SIERRA LEONE", "Sierra Leone");
            d.Add("SINGAPORE", "Singapore");
            d.Add("SLOVAKIA", "Slovakia");
            d.Add("SLOVENIA", "Slovenia");
            d.Add("SOMALIA", "Somalia");
            d.Add("SOUTH", "");
            d.Add("SOUTH AFRICA", "South Africa");
            d.Add("SPAIN", "Spain");
            d.Add("SRI LANKA", "Sri Lanka");
            d.Add("SUDAN", "Sudan");
            d.Add("SURINAME", "Suriname");
            d.Add("SWAZILAND", "Swaziland");
            d.Add("SWEDEN", "Sweden");
            d.Add("SWITZERLAND", "Switzerland");
            d.Add("SYRIA", "Syria");
            d.Add("TAIWAN", "Taiwan");
            d.Add("TAJIKISTAN", "Tajikistan");
            d.Add("TANZANIA", "Tanzania");
            d.Add("THAILAND", "Thailand");
            d.Add("TIMOR-LESTE", "Timor-Leste");
            d.Add("TOGO", "Togo");
            d.Add("TONGA", "Tonga");
            d.Add("TRINIDAD AND TOBAGO", "Trinidad And Tobago");
            d.Add("TURKEY", "Turkey");
            d.Add("TURKMENISTAN", "Turkmenistan");
            d.Add("UGANDA", "Uganda");
            d.Add("UK", "United Kingdom");
            d.Add("UK:", "United Kingdom");
            d.Add("UKRAINE", "Ukraine");
            d.Add("UNITED ARAB EMIRATES", "United Arab Emirates");
            d.Add("UNITED KINGDOM", "United Kingdom");
            d.Add("UNITED STATES", "United States");
            d.Add("URUGUAY", "Uruguay");
            d.Add("US", "United States");
            d.Add("UZBEKISTAN", "Uzbekistan");
            d.Add("VENEZUELA", "Venezuela");
            d.Add("VIET NAM", "Viet Nam");
            d.Add("YEMEN", "Yemen");
            d.Add("ZAMBIA", "Zambia");
            d.Add("ZIMBABWE", "Zimbabwe");

            return d;
        }

        #endregion Methods

    }

    public class ToInformaSubjects : ListToGuid
    {
        public ToInformaSubjects(Item i) : base(i) { }

        #region Methods

        public override void FillField(IDataMap map, ref Item newItem, string importValue)
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

            string lowerValue = importValue.ToLower();
            string transformValue = (d.ContainsKey(lowerValue)) ? d[lowerValue] : string.Empty;
            if (string.IsNullOrEmpty(transformValue))
            {
                map.Logger.Log(newItem.Paths.FullPath, "Subject(s) not converted", ProcessStatus.FieldError, NewItemField, importValue);
                return;
            }

            string[] parts = transformValue.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries);

            ChildList cl = i.GetChildren();

            StringBuilder sb = new StringBuilder();

            //loop through children and look for anything that matches by name
            foreach (string area in parts)
            {
                string cleanName = StringUtility.GetValidItemName(area, map.ItemNameMaxLength);
                IEnumerable<Item> t = cl.Where(c => c.DisplayName.Equals(cleanName));

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

        public Dictionary<string, string> GetMapping()
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
            d.Add("chapter 11", "");
            d.Add("clinical trial results", "Clinical Trials");
            d.Add("clinical trials", "Clinical Trials");
            d.Add("code of conduct", "");
            d.Add("company deals", "");
            d.Add("congress", "");
            d.Add("contract sales", "");
            d.Add("court case", "");
            d.Add("critical path", "");
            d.Add("cutbacks", "Companies");
            d.Add("democrats", "");
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
            d.Add("fda advisory panel", "");
            d.Add("fda advisory panel meeting", "Approvals");
            d.Add("fda approvable letter", "");
            d.Add("fda complete response letter", "Regulation");
            d.Add("fda non-approvable letter", "");
            d.Add("financial updates", "Companies");
            d.Add("forecast", "Companies");
            d.Add("foreign aid", "Policy");
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
            d.Add("house of representatives", "");
            d.Add("infringement", "Regulation");
            d.Add("injunction", "Regulation");
            d.Add("innovative medicines initiative", "Policy");
            d.Add("inspections", "Regulation");
            d.Add("internet trade", "");
            d.Add("investment", "Companies");
            d.Add("i[p", "Companies");
            d.Add("joint venture", "Companies::Deals");
            d.Add("kickbacks", "");
            d.Add("lawsuit", "Companies");
            d.Add("legal", "");
            d.Add("legislation", "Policy::Legislation");
            d.Add("licensing", "Companies::Deals");
            d.Add("litigation", "Companies");
            d.Add("loan", "Companies::Deals");
            d.Add("lobbying", "");
            d.Add("manufacturing", "Companies");
            d.Add("market data", "Markets::Market Intelligence");
            d.Add("market statistics", "");
            d.Add("marketing withdrawal", "");
            d.Add("medicaid", "");
            d.Add("medical ethics", "");
            d.Add("medical records", "");
            d.Add("medicare", "");
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
            d.Add("paediatric medicines", "");
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
            d.Add("republicans", "");
            d.Add("research", "");
            d.Add("resignation", "Companies");
            d.Add("restructuring", "Companies");
            d.Add("retirement", "Companies");
            d.Add("reverse payments", "");
            d.Add("reverse takeover", "");
            d.Add("review extension", "Approvals");
            d.Add("rights issue", "");
            d.Add("scientific advice", "Approvals");
            d.Add("senate", "");
            d.Add("setting", "Policy");
            d.Add("settlement", "Companies");
            d.Add("shareholder", "");
            d.Add("spin out", "Companies::Deals");
            d.Add("start-ups", "Companies");
            d.Add("stockpiling", "Strategy");
            d.Add("supplemental approval", "Approvals");
            d.Add("supplemental approval filing", "Approvals");
            d.Add("technology", "Policy");
            d.Add("tentative approval", "");
            d.Add("termination", "Companies");
            d.Add("third party payers", "");
            d.Add("trade", "Companies::Strategy");
            d.Add("trademarks", "");
            d.Add("vaccination programmes", "Policy::Market Access");
            d.Add("whistleblower", "");
            d.Add("white house", "");
            
            return d;
        }

        #endregion Methods

    }
}
