using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using HtmlAgilityPack;
using InformaSitecoreWord.Config;
using InformaSitecoreWord.Custom_Exceptions;
using InformaSitecoreWord.Sitecore;
using InformaSitecoreWord.Util.CharacterStyles;
using InformaSitecoreWord.Util.Document;
using InformaSitecoreWord.Util.Tables;
using PluginModels;
using Microsoft.Office.Interop.Word;
using Word = Microsoft.Office.Interop.Word;
using COM = System.Runtime.InteropServices.ComTypes;
using InformaSitecoreWord.UI.TreeBrowser.TreeBrowserControls;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace InformaSitecoreWord.Util
{
    public class AlertDisabler : IDisposable
    {
        private readonly Word.Application _application;
        public AlertDisabler(Word.Application application)
        {
            _application = application;
            _application.DisplayAlerts = WdAlertLevel.wdAlertsNone;
        }

        public void Dispose()
        {
            _application.DisplayAlerts = WdAlertLevel.wdAlertsAll;
        }
    }

    public class WordUtils
    {

        public const string BlockquoteName = "2.4 Quote Box";
        public const string DocxFormatName = "Word12";
        public Dictionary<string, string> imageFloatDictionary = new Dictionary<string, string>
        {
            { "left", "article-exhibit article-exhibit--pull-left"},
            { "right", "article-exhibit article-exhibit--pull-right"},
            { "none", "article-exhibit"},
        };

        //These properties are built so that the accessing of the sitecore webservices is deferred until after
        //the user has actually logged on.
        protected Dictionary<string, WordStyleStruct> _paragraphStyles;

        protected Dictionary<string, WordStyleStruct> ParagraphStyles
        {
            get
            {
                if (_paragraphStyles == null)
                {
                    _paragraphStyles = new Dictionary<string, WordStyleStruct>();
                    List<WordStyleStruct> styles = SitecoreClient.GetParagraphStyles().ToList();
                    foreach (WordStyleStruct style in styles)
                    {
                        if (
                            !_paragraphStyles.Contains(new KeyValuePair<string, WordStyleStruct>(style.WordStyle, style)))
                        {
                            _paragraphStyles.Add(style.WordStyle, style);
                        }
                    }
                }
                return _paragraphStyles;
            }
        }

        protected OptimizedCharacterStyleTransformer _characterStyleByElementTransformer;

        public OptimizedCharacterStyleTransformer CharacterStyleTransformer
        {
            get
            {
                if (_characterStyleByElementTransformer == null)
                {
                    return _characterStyleByElementTransformer = new OptimizedCharacterStyleTransformer();
                }
                return _characterStyleByElementTransformer;
            }
        }

        public List<string> Errors;
        public List<string> QuickFactsStyles;
        private SideboxBuilder _quickFactsSideboxParser;

        protected ImageReferenceBuilder _imageReferenceBuilder;

        protected ImageReferenceBuilder ImageReferenceBuilder
        {
            get
            {
                if (_imageReferenceBuilder == null)
                {
                    _imageReferenceBuilder = new ImageReferenceBuilder(ParagraphStyles, CharacterStyleTransformer);
                }
                return _imageReferenceBuilder;
            }
        }

        public SidebarArticleParser SidebarArticleParser = new SidebarArticleParser();

        public WordUtils()
        {
            QuickFactsStyles = new List<string>
            {
                "3.0 Quick Facts Title",
                "3.1 Quick Facts Text",
                "3.2 Quick Facts Bulleted List",
                "3.3 Quick Facts Numbered List",
                "3.4 Quick Facts Source"
            };

            _quickFactsSideboxParser = new SideboxBuilder(QuickFactsStyles);
        }

        public List<string> GetSupportingDocumentPaths()
        {
            return CharacterStyleTransformer.SupportingDocumentsReferenceBuilder.SupportingDocuments;
        }

        public static bool IsHyperlinkValid(Hyperlink hyperlink)
        {
            var revisions = hyperlink.Range.Revisions.Cast<Revision>().ToArray();

            if (revisions.Count() == 0)
            {
                return true;
            }

            foreach (var revision in revisions)
            {
                Revision r = revision;
                if (r.Type == WdRevisionType.wdRevisionDelete || r.Type == WdRevisionType.wdRevisionMovedTo)
                {
                    return false;
                }
            }

            try
            {
                var testScreenTip = hyperlink.ScreenTip;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool SaveDocument(Word.Document activeDocument)
        {
            activeDocument.Saved = false;

            string originalFormat = activeDocument.Application.DefaultSaveFormat;
            try
            {
                string fullname;
                var path = activeDocument.Path;
                //if doc is readonly, we can't save directly to the file, so we're
                //doing the "save as copy" technique
                if (string.IsNullOrWhiteSpace(path) || !FileExistsAndNotReadOnly(path, activeDocument.Name) ||
                    !Save(activeDocument))
                {
                    //force word to save in actual .docx format
                    activeDocument.Application.DefaultSaveFormat = DocxFormatName;

                    if (!SaveTempDoc(activeDocument, out fullname))
                    {
                        return false;
                    }
                }
                else
                {
                    fullname = activeDocument.FullName;
                }

                string tempFileLocation = ApplicationConfig.GetPropertyValue("TempFileName") + @".temp";
                tempFileLocation = tempFileLocation.Replace("{path}",
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.ApplicationData));

                File.Copy(fullname, tempFileLocation + "doc", true);
            }
            finally
            {
                activeDocument.Application.DefaultSaveFormat = originalFormat;
            }

            return true;
        }

        public static bool Save(Word.Document document)
        {
            try
            {
                using (new AlertDisabler(document.Application))
                {
                    document.Save();
                    document.Saved = true;
                    return true;
                }
            }
            catch (Exception excp)
            {
                Globals.SitecoreAddin.LogException("Unable to save document", excp);
            }

            return false;
        }

        public byte[] GetWordBytes(Word.Document activeDocument)
        {
            if (!SaveDocument(activeDocument))
            {
                return null;
            }

            _tempFileLocation = ApplicationConfig.GetPropertyValue("TempFileName") + @".temp";

            string applicationDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _tempFileLocation = _tempFileLocation.Replace("{path}", applicationDataFolder);

            byte[] data;
            using (var fStream = new FileStream(_tempFileLocation + "doc", FileMode.Open, FileAccess.Read))
            {
                using (var br = new BinaryReader(fStream))
                {
                    data = new byte[fStream.Length];
                    br.Read(data, 0, data.Length);
                    br.Close();
                }
            }

            return data;
        }

        public static bool FileExistsAndNotReadOnly(string path, string fileName)
        {
            string fullPath = string.Format(@"{0}\{1}", path, fileName);

            FileInfo fileInfo;

            try
            {
                fileInfo = new FileInfo(fullPath);
            }
            catch (Exception)
            {
                return false;
            }

            if (!fileInfo.Exists)
            {
                return false;
            }

            if (fileInfo.IsReadOnly)
            {
                return false;
            }

            return true;
        }

        private static bool FileIsInUse(string path)
        {
            FileInfo fileInfo;

            try
            {
                fileInfo = new FileInfo(path);
            }
            catch (Exception)
            {
                return false;
            }

            if (!fileInfo.Exists)
            {
                return false;
            }

            if (fileInfo.IsReadOnly)
            {
                return true;
            }

            try
            {
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    return fs.CanRead && fs.CanWrite;
                }
            }
            catch (IOException)
            {
                return true;
            }
            catch (Exception)
            {
                return true;
            }
        }

        public static bool SaveTempDoc(Word.Document activeDocument, out string filepath)
        {
            string fileName = ApplicationConfig.GetPropertyValue("TempFileName");
            fileName = fileName.Replace("{path}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

            //"Save Copy As" functionality so that the open document
            //does not get associated with the temp file

            bool saveSuccessful = false;
            if (!FileIsInUse(fileName))
            {
                try
                {
                    saveCopyAs(activeDocument, fileName);
                    saveSuccessful = true;
                }
                catch (Exception excp)
                {
                    Globals.SitecoreAddin.LogException("Initial Save Failed", excp);
                }
            }

            if (saveSuccessful)
            {
                filepath = fileName;
                return true;
            }

            bool deleteSuccessful = false;
            if (File.Exists(fileName))
            {
                try
                {
                    File.Delete(fileName);
                    Globals.SitecoreAddin.LogException("Deleting temp file succeeded");
                    deleteSuccessful = true;
                }
                catch (Exception excp)
                {
                    Globals.SitecoreAddin.LogException("Deleting the temp file failed.", excp);
                }

                if (deleteSuccessful && !FileIsInUse(fileName))
                {
                    try
                    {
                        saveCopyAs(activeDocument, fileName);

                        filepath = fileName;
                        return true;
                    }
                    catch (Exception excp)
                    {
                        Globals.SitecoreAddin.LogException("Save retry to temp file failed.", excp);
                    }
                }
            }

            var path = Path.GetTempPath();
            var tempFilePath = Path.Combine(path, Guid.NewGuid() + ".docx");

            try
            {
                saveCopyAs(activeDocument, tempFilePath);
                filepath = tempFilePath;
                return true;
            }
            catch (Exception excp)
            {
                string file = string.Format("Saving to the temp directory failed, {0}.", tempFilePath);
                Globals.SitecoreAddin.LogException(file, excp);
            }

            filepath = null;
            return false;
        }

        private static void saveCopyAs(Word.Document activeDocument, string fileName)
        {
            var pp = (COM.IPersistFile)activeDocument;
            pp.Save(fileName, false);
            pp.SaveCompleted(fileName);
        }

        public XElement GetWordDocTextWithStyles(Word.Document activeDocument)
        {
            Microsoft.Office.Tools.Word.Document x = Globals.Factory.GetVstoObject(activeDocument);
            var paragraphs = x.Paragraphs;
            var tableBuilder = new TableBuilder(x.Tables);
            return ParagraphsToXml(paragraphs, tableBuilder);
        }

        public XElement ParagraphsToXml(Paragraphs paragraphs, TableBuilder tableBuilder, Object builder = null)
        {
            var lop = paragraphs.Cast<Paragraph>().ToList();
            return ParagraphsToXml(lop, tableBuilder, builder);
        }

        /// <summary>
        /// Takes the list of paragraphs and returns the XML representation of these elements.
        /// </summary>
        /// <param name="paragraphs">The list of paragraphs to parse.</param>
        /// <param name="tableBuilder"></param>
        /// <param name="builder"></param>
        /// <returns>An XElement representation of the document.</returns>
        public XElement ParagraphsToXml(List<Paragraph> paragraphs, TableBuilder tableBuilder, Object builder = null)
        {
            SidebarArticleParser = new SidebarArticleParser();
            WdListType previouslistType = paragraphs[0].Range.ListFormat.ListType;
            bool previousWasList = false;

            string previousIFrameStyle = string.Empty;

            bool previousWasIFrame = false;
            bool desktopCodeFound = false;
            bool currentIframeIsInsecure = false;
            bool currentContainsInvalidNodes = false;
            List<string> insecureIFrames = new List<string>();
            bool containsInvalidNodes = false;

            string iframeGroupId = String.Empty;
            XElement iframeGroupElement = new XElement("div");
            iframeGroupElement.SetAttributeValue("class", "iframe-component");

            bool previousWasBlockquote = false;
            var contiguousListElements = new List<Paragraph>();
            var contiguousBlockquoteElements = new List<Paragraph>();
            List<string> StylesToIgnore = ArticleDocumentMetadataParser.GetInstance().MetadataStyles;
            Paragraph lastParagraph = null;
            //var xData = new XElement("root");
            var xData = new XElement("div");
            xData.SetAttributeValue("class", "root");
            Errors = new List<string>();
            int imageTagCount = 0;
            XElement divElement = null;

            for (int i = 0; i < paragraphs.Count; i++)
            {
                Paragraph paragraph = paragraphs[i];
                int tIndex = tableBuilder == null ? -1 : tableBuilder.GetTableIndexFor(paragraph.Range);

                Style style = (Style)paragraph.get_Style();


                if (tableBuilder != null && tIndex != -1 && tableBuilder.HasRetrieved(tIndex))
                {
                    continue;
                }

                var currentStyle = (Style)paragraph.get_Style();
                if (StylesToIgnore.Contains(currentStyle.NameLocal))
                {
                    continue;
                }

                XElement xElement = null;
                bool isList = paragraph.Range.ListParagraphs.Count > 0;
                bool isBlockquote = currentStyle.NameLocal == BlockquoteName;
                WdListType currentListType = paragraph.Range.ListFormat.ListType;

                if (tableBuilder != null && tIndex != -1 && !tableBuilder.HasRetrieved(tIndex))
                {
                    xData.Add(tableBuilder.ParseTable(tIndex));
                    continue;
                }


                if (builder != _quickFactsSideboxParser && _quickFactsSideboxParser != null &&
                    _quickFactsSideboxParser.Match(currentStyle.NameLocal))
                {
                    _quickFactsSideboxParser.Add(paragraph);
                    continue;
                }
                if (_quickFactsSideboxParser != null && !_quickFactsSideboxParser.Match(currentStyle.NameLocal) &&
                    !_quickFactsSideboxParser.IsEmpty())
                {
                    xData.Add(_quickFactsSideboxParser.GetSidebox(this));
                    _quickFactsSideboxParser.Clear();
                }

                lastParagraph = paragraph;
                if (currentStyle.NameLocal == SidebarArticleParser.SidebarStyle)
                {
                    try
                    {
                        xData.Add(paragraph.Range.Text);
                        SidebarArticleParser.RetrieveSidebarToken(paragraph);
                    }
                    catch (ArgumentException e)
                    {
                        Errors.Add(e.Message);
                    }
                    continue;
                }
                if (!isList && previousWasList)
                {
                    xData.Add(GetListStyleElement(contiguousListElements));
                    contiguousListElements = new List<Paragraph>();
                    previousWasList = false;
                }
                if (!isBlockquote && previousWasBlockquote)
                {
                    xData.Add(BlockquoteTransformer.Generate(contiguousBlockquoteElements, CharacterStyleTransformer));
                    contiguousBlockquoteElements = new List<Paragraph>();
                    previousWasBlockquote = false;
                }

                if (isBlockquote)
                {
                    contiguousBlockquoteElements.Add(paragraph);
                    previousWasBlockquote = true;
                    //just forming contiguous blocks, no further processing
                    continue;
                }
                if (isList)
                {
                    if (previouslistType != currentListType && contiguousListElements.Count > 0)
                    {
                        xElement = GetListStyleElement(contiguousListElements);

                        xData.Add(xElement);
                        contiguousListElements = new List<Paragraph>();
                    }
                    //add paragraph to bloc,
                    //even if new list has been started, we are adding it now
                    contiguousListElements.Add(paragraph);
                    previousWasList = true;
                    previouslistType = paragraph.Range.ListFormat.ListType;
                    //just forming contiguous blocks, no further processing
                    continue;
                }


                if (style.NameLocal == DocumentAndParagraphStyles.IFrameCodeStyle ||
                    style.NameLocal == DocumentAndParagraphStyles.IFrameMobileCodeStyle)
                {
                    var iframeElement = new XElement("div");

                    if (!previousWasIFrame)
                    {
                        iframeGroupId = Guid.NewGuid().ToString("N");

                        previousWasIFrame = true;
                        iframeGroupElement.SetAttributeValue("class", "iframe-component");
                    }
                    string cssStyle = string.Empty;

                    if (style.NameLocal == DocumentAndParagraphStyles.IFrameCodeStyle)
                    {
                        desktopCodeFound = true;
                        cssStyle = String.Format("ewf-desktop-iframe_{0}", iframeGroupId);
                        iframeElement.SetAttributeValue("class", $"iframe-component__desktop {cssStyle}");
                        iframeElement.SetAttributeValue("data-mediaid", iframeGroupId);
                    }

                    if (style.NameLocal == DocumentAndParagraphStyles.IFrameMobileCodeStyle)
                    {
                        cssStyle = String.Format("ewf-mobile-iframe_{0}", iframeGroupId);
                        iframeElement.SetAttributeValue("class", $"iframe-component__mobile {cssStyle}");
                    }

                    var insecureIFramesInParagraph = HTMLTools.CheckForInsecureIFrames(paragraph.Range.Text);
                    currentIframeIsInsecure = insecureIFramesInParagraph.Any() || currentIframeIsInsecure;
                    insecureIFrames.AddRange(insecureIFramesInParagraph);

                    currentContainsInvalidNodes = HTMLTools.ContainsForExternalNodes(paragraph.Range.Text);
                    containsInvalidNodes = containsInvalidNodes || currentContainsInvalidNodes;
                    XElement embedElement = IFrameEmbedBuilder.Parse(paragraph, cssStyle, true);
                    if (embedElement != null)
                    {
                        iframeElement.SetAttributeValue("data-embed-link", "enabled");
                        iframeElement.Add(embedElement);
                        iframeGroupElement.Add(iframeElement);
                    }
                    if (style.NameLocal == DocumentAndParagraphStyles.IFrameMobileCodeStyle)
                    {
                        xData.Add(iframeGroupElement);
                        iframeGroupElement = new XElement("div");
                        iframeGroupElement.SetAttributeValue("class", "iframe-component");
                    }

                    previousIFrameStyle = style.NameLocal;
                    continue;
                }

                if (IFrameEmbedBuilder.IFrameStyles.Contains(style.NameLocal))
                {
                    WordStyleStruct w = new WordStyleStruct();
                    //base styles are used becuase the parent level styles only exist in the plugin
                    var baseStyle = (Style)style.get_BaseStyle();
                    if (baseStyle != null)
                    {
                        ParagraphStyles.TryGetValue(baseStyle.NameLocal, out w);
                    }

                    XElement curElement = new XElement("p");
                    string stylesection = String.Empty;
                    bool isNewIframe = false;
                    if (style.NameLocal == DocumentAndParagraphStyles.IFrameHeaderStyle)
                    {
                        stylesection = "header";
                        isNewIframe = true;

                    }
                    if (style.NameLocal == DocumentAndParagraphStyles.IFrameTitleStyle)
                    {
                        stylesection = "title";
                        isNewIframe = previousIFrameStyle != DocumentAndParagraphStyles.IFrameHeaderStyle;

                    }
                    if (style.NameLocal == DocumentAndParagraphStyles.IFrameCaptionStyle)
                    {
                        stylesection = "caption";
                        isNewIframe = previousIFrameStyle != DocumentAndParagraphStyles.IFrameMobileCodeStyle;
                    }

                    if (style.NameLocal == DocumentAndParagraphStyles.IFrameSourceStyle)
                    {
                        stylesection = "source";
                        isNewIframe = previousIFrameStyle != DocumentAndParagraphStyles.IFrameCaptionStyle &&
                                      previousIFrameStyle != DocumentAndParagraphStyles.IFrameMobileCodeStyle;
                    }


                    if (!previousWasIFrame && isNewIframe)
                    {
                        iframeGroupId = Guid.NewGuid().ToString("N");
                        previousWasIFrame = true;
                    }

                    string cssClass = w == null ? string.Empty : w.CssClass;
                    string iframeIdClass = String.Format("{0}-{1}_{2}", IFrameEmbedBuilder.IFrameClassName, stylesection,
                        iframeGroupId);

                    curElement.SetAttributeValue("class",
                        String.Format("{0} {1}",
                            cssClass,
                            iframeIdClass));

                    curElement = CharacterStyleTransformer.GetCharacterStyledElement(curElement, paragraph,
                        CharacterStyleFactory.GetCharacterStyles(), false);
                    xData.Add(curElement);

                    previousIFrameStyle = style.NameLocal;
                    continue;
                }
                else //redundant else but adds code clarity
                {
                    if (currentIframeIsInsecure || currentContainsInvalidNodes ||
                        (!desktopCodeFound && previousWasIFrame))
                    {
                        xData.Elements()
                            .Where(x => x.Attribute("class") != null &&
                                        x.Attribute("class").Value.Contains(iframeGroupId))
                            .Remove();
                    }
                    previousIFrameStyle = String.Empty;
                    previousWasIFrame = false;
                    currentIframeIsInsecure = false;
                    currentContainsInvalidNodes = false;
                    desktopCodeFound = false;
                }

                if (ImageReferenceBuilder.Parse(paragraph) != null)
                {
                    if (imageTagCount == 0)
                    {
                        divElement = new XElement("section");
                        divElement.SetAttributeValue("class", "article-exhibit");
                        xData.Add(divElement);
                    }

                    //Get the float Value from the image hyperlink (if it is an image) and set it to the article-image element
                    var hyprlnk = paragraph.Range.Hyperlinks.Cast<Hyperlink>().FirstOrDefault();
                    if (hyprlnk != null && string.IsNullOrEmpty(hyprlnk.ScreenTip) == false)
                    {
                        string classValue;
                        classValue = imageFloatDictionary.TryGetValue(hyprlnk.ScreenTip.ToLower(), out classValue) ? classValue : string.Empty;
                        divElement.SetAttributeValue("class", classValue);
                    }

                    var imageTag = ImageReferenceBuilder.Parse(paragraph);
                    divElement.Add(imageTag);
                    imageTagCount++;
                    continue;
                }

                imageTagCount = 0;

                WordStyleStruct styleStruct;
                if (ParagraphStyles.TryGetValue(currentStyle.NameLocal, out styleStruct))
                {
                    //if there is a special configuration for the paragraph style, have it configured properly
                    string element = styleStruct.CssElement;
                    if (element.IsNullOrEmpty())
                    {
                        element = null;
                    }
                    string clas = styleStruct.CssClass;
                    if (clas.IsNullOrEmpty())
                    {
                        clas = null;
                    }
                    XElement curElement = element != null ? new XElement(element) : new XElement("p");
                    if (!string.IsNullOrWhiteSpace(clas))
                    {
                        curElement.SetAttributeValue("class", clas);
                    }

                    xElement = CharacterStyleTransformer.GetCharacterStyledElement(curElement, paragraph,
                        CharacterStyleFactory.GetCharacterStyles(), false);

                    //The next section is group multiple paragraphs belonging to the same answer under the same Answer element to prevent multiple answer styling on front-end for a single answer
                    if (clas == "article-interview__answer")
                    {
                        while (paragraphs.Count > i + 1)//If there are still more element to inspect
                        {
                            //Get the styling of the next element
                            var tempAnswerStyle = (Style)paragraphs[i + 1].get_Style();
                            WordStyleStruct tempAnswerStyleStruct;
                            //Get the cssClass of the next element
                            ParagraphStyles.TryGetValue(tempAnswerStyle.NameLocal, out tempAnswerStyleStruct);
                            //if it is also an answer paragraph
                            if (tempAnswerStyleStruct.CssClass == clas)
                            {
                                //add the the next paragraph content into the current answer body
                                xElement.Add(CharacterStyleTransformer.GetCharacterStyledElement(new XElement("p"), paragraphs[i + 1], CharacterStyleFactory.GetCharacterStyles(), false));
                                i++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    xElement = new XElement("p");
                    xElement = CharacterStyleTransformer.GetCharacterStyledElement(xElement, paragraph,
                        CharacterStyleFactory.GetCharacterStyles(), false);
                }
                xData.Add(xElement);
                previouslistType = paragraph.Range.ListFormat.ListType;
            }
            if (insecureIFrames.Any())
            {
                var confirmSave =
                    MessageBox.Show("You have inserted multimedia items using non-secure links. " +
                                    "Click 'OK' to continue saving the article without this content or 'Cancel'" +
                                    " to go back into the article and edit your multimedia items.",
                        "Continue with save?",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (confirmSave != DialogResult.OK)
                {
                    throw new InsecureIFrameException(insecureIFrames);
                }
            }
            if (containsInvalidNodes)
            {
                var confirmSave =
                    MessageBox.Show("You have inserted multimedia items with invalid or non-permitted HTML code. " +
                                    "Click 'OK' to continue saving the article without this content or 'Cancel' to " +
                                    "go back into the article and edit your multimedia items.", "Continue with save?",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (confirmSave != DialogResult.OK)
                {
                    throw new InvalidHtmlException();
                }
            }
            if (lastParagraph != null && lastParagraph.Range.ListParagraphs.Count > 0)
            {
                xData.Add(GetListStyleElement(contiguousListElements));
            }
            if (lastParagraph != null && ((Style)lastParagraph.get_Style()).NameLocal == BlockquoteName)
            {
                xData.Add(BlockquoteTransformer.Generate(contiguousBlockquoteElements, CharacterStyleTransformer));
            }
            if (_quickFactsSideboxParser != null && builder != _quickFactsSideboxParser &&
                !_quickFactsSideboxParser.IsEmpty())
            {
                xData.Add(_quickFactsSideboxParser.GetSidebox(this));
                _quickFactsSideboxParser.Clear();
            }
            return xData;
        }


        protected XElement GetListStyleElement(List<Paragraph> listItems)
        {
            string listType;
            if (listItems[0].Range.ListFormat.ListType == WdListType.wdListBullet)
            {
                listType = "ul";
            }
            else
            {
                listType = "ol";
            }
            var currentListElement = new XElement(listType);
            currentListElement.SetAttributeValue("class", "carrot-list");
            XElement rootListElement = currentListElement;
            WordStyleStruct wstyle;
            Style style = listItems[0].get_Style();
            if (style != null && ParagraphStyles.TryGetValue(style.NameLocal, out wstyle))
            {
                rootListElement.SetAttributeValue("class", wstyle.CssClass);
            }
            int previousIndentLevel = listItems[0].Range.ListFormat.ListLevelNumber;
            foreach (Paragraph item in listItems)
            {
                var liElement = new XElement("li");
                liElement = CharacterStyleTransformer.GetCharacterStyledElement(liElement, item,
                    CharacterStyleFactory.GetCharacterStyles(), false);
                int currentIndentLevel = item.Range.ListFormat.ListLevelNumber;
                if (currentIndentLevel > previousIndentLevel)
                {
                    var sublist = new XElement(listType);
                    var li = liElement;
                    sublist.Add(li);
                    if (currentListElement != null)
                    {
                        currentListElement.Add(sublist);
                    }
                    currentListElement = sublist;
                }
                else
                {
                    int delta = previousIndentLevel - currentIndentLevel;
                    for (int i = 0; i < delta; i++)
                    {
                        if (currentListElement != null)
                        {
                            currentListElement = currentListElement.Parent;
                        }
                    }
                    if (currentListElement != null)
                    {
                        XElement li = liElement;
                        currentListElement.Add(li);
                    }
                }
                previousIndentLevel = currentIndentLevel;
            }

            return rootListElement;
        }

        protected string _tempFileLocation;
    }

    public static class SitecoreWordUtil
    {
        public static string FormatUserName(string userNameWithDomain)
        {
            if (userNameWithDomain.IsNullOrEmpty())
            { return userNameWithDomain; }

            string formattedUserName = userNameWithDomain;

            if (formattedUserName.IndexOf("\\") > 0)
            {
                formattedUserName = formattedUserName.Substring(formattedUserName.IndexOf("\\") + 1);
            }

            return formattedUserName;
        }
    }
}
