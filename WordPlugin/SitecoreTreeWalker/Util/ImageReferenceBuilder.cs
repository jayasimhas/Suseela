using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using InformaSitecoreWord.Util.CharacterStyles;
using InformaSitecoreWord.Util.Document;
using PluginModels;
using Microsoft.Office.Interop.Word;
using InformaSitecoreWord.Sitecore;

namespace InformaSitecoreWord.Util
{
    public class ImageReferenceBuilder
    {

		protected Dictionary<string, WordStyleStruct> ParagraphStyles = new Dictionary<string, WordStyleStruct>();
        public static List<string> ImageStyles = new List<string> { DocumentAndParagraphStyles.ExhibitNumberStyle, DocumentAndParagraphStyles.ExhibitTitleStyle, DocumentAndParagraphStyles.ImagePreviewStyle, DocumentAndParagraphStyles.SourceStyle, DocumentAndParagraphStyles.ExhibitCaptionStyle52 };
        protected OptimizedCharacterStyleTransformer Transformer;

		public ImageReferenceBuilder(Dictionary<string, WordStyleStruct> styles, OptimizedCharacterStyleTransformer transformer)
        {
            ParagraphStyles = styles;
            Transformer = transformer;
        }

        public XElement Parse(Paragraph paragraph)
        {
            Style style = (Style)paragraph.get_Style();
            if (style.NameLocal == DocumentAndParagraphStyles.ImagePreviewStyle)
            {

                IEnumerable<Hyperlink> hs = paragraph.Range.Hyperlinks.Cast<Hyperlink>().ToArray();
                if (hs.Count() == 0)
                {
                    return null;
                }

                try
                {
                    var hyperline = hs.First();

                    if (!WordUtils.IsHyperlinkValid(hyperline))
                    {
                        return null;
                    }

                    Uri tempUri = new Uri(hyperline.Address);
                    var src = tempUri.AbsolutePath;
                    //XElement wrapper = GetImageElement(src, hyperline.ScreenTip);
                    XElement wrapper = GetImageElement(src);
                    return wrapper;
                    
                }
                catch (WebException e)
                {
                    Globals.SitecoreAddin.LogException("", e);
                    Globals.SitecoreAddin.AlertConnectionFailure();
                }
                catch (Exception e)
                {
                    Globals.SitecoreAddin.LogException("", e);
                    throw;
                }
            }
            if (ImageStyles.Contains(style.NameLocal))
            {
				WordStyleStruct w;
                if (!ParagraphStyles.TryGetValue(style.NameLocal, out w)) return null;
                var element = new XElement("p");
                element.SetAttributeValue("class", w.CssClass);
                element = Transformer.GetCharacterStyledElement(element, paragraph, CharacterStyleFactory.GetCharacterStyles(), false);//new XElement(w.CssElement);

                //var value = Transformer.GetCharacterStylesValue(paragraph).Replace("\a", "");
                string value = element.Value;
                if (value.StartsWith("SOURCE: "))
                {
                    element.Value = value.Remove(0, 8);
                }
                return element;
            }
            return null;
        }

        //TamerM - 2015-02-25: Method overload removed, because float should be set on the article-image div not the image only to float with the text content such as the header, title, caption etc...
        //private XElement GetImageElement(string src, string floatType)
        //{
        //    var url = src.Replace("%22", string.Empty).Replace("\"", "");
        //    var wrapper = new XElement("a");

        //    var floatClass = string.Empty;
        //    try
        //    {
        //        string classValue;
        //        floatClass = imageFloatDictionary.TryGetValue(floatType.ToLower(), out classValue) ? classValue : string.Empty;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    var img = new XElement("img");
        //    img.SetAttributeValue("src", url);
        //    img.SetAttributeValue("class", floatClass);
        //    wrapper.SetAttributeValue("class", "enlarge");
        //    wrapper.Add(img);
        //    wrapper.Add(new XElement("br"));
        //    return wrapper;
        //}

        private XElement GetImageElement(string src)
        {
            var url = src.Replace("%22", string.Empty);
            var wrapper = new XElement("a");

            //wrapper.SetAttributeValue("target", "_blank");
            //wrapper.SetAttributeValue("href", url);
            var img = new XElement("img");
            img.SetAttributeValue("src", url);
            wrapper.SetAttributeValue("class", "enlarge");
			wrapper.SetAttributeValue("href", url);
			wrapper.SetAttributeValue("target", "_blank");			
			wrapper.Add(img);
            wrapper.Add(new XElement("br"));
            return wrapper;
        }
    }
}
