using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.XPath;

namespace Informa.Library.Utilities.StringUtils
{
    public class WordUtil
    {
        public static int GetWordCount(string text)
        {
            int wordCount = 0;
            int index = 0;

            while (index < text.Length)
            {
                while (index < text.Length && Char.IsWhiteSpace(text[index]) == false)
                    index++;

                wordCount++;

                while (index < text.Length && Char.IsWhiteSpace(text[index]) == true)
                    index++;
            }
            return wordCount;
        }

        public static string GetSubStringWords(int numberOfWords, string stringWords)
        {
            string[] strArray = stringWords.Split(' ');
            StringBuilder sbReturnString = new StringBuilder();

            for (int i = 0; i < numberOfWords; i++)
                sbReturnString.Append(strArray[i] + " ");

            return sbReturnString.ToString();
        }

        public static string TruncateArticle(string text, int maxWordCount)
        {
            string result;
            try
            {
                int maxWordCountLessEllipsis = maxWordCount;

                if (WordUtil.GetWordCount(text) <= maxWordCount)
                    result = text;

                else
                {
                    result = WordUtil.TruncateHtml(text, maxWordCountLessEllipsis);
                    var lastWordPosition = result.LastIndexOf(' ');

                    if (lastWordPosition < 0) lastWordPosition = 0;

                    result = result.Substring(0, lastWordPosition).Trim(new[] { '.', ',', '!', '?' }) + "...";

                }
            }
            catch (Exception)
            {
                return text;
            }

            return result;
        }


        public static string TruncateHtml(string text, int maxWordCount)
        {
            text = HttpUtility.HtmlDecode(text);
            text = "<body>" + text + "</body>";
            int wordsAvailable = maxWordCount;

            XmlDocument xml = new XmlDocument();
            xml.LoadXml("" + text + "");
            XPathNavigator navigator = xml.CreateNavigator();
            XPathNavigator breakPoint = null;
            string lastText = string.Empty;

            // find the text node we need:
            while (navigator.MoveToFollowing(XPathNodeType.Text))
            {

                if (navigator.Value.Trim() != string.Empty)
                    lastText = WordUtil.GetWordCount(navigator.Value.Trim()) > wordsAvailable
                        ? WordUtil.GetSubStringWords(wordsAvailable, navigator.Value.Trim())
                        : navigator.Value;

                wordsAvailable = wordsAvailable - WordUtil.GetWordCount(navigator.Value);

                if (wordsAvailable <= 0)
                {
                    navigator.SetValue(lastText);
                    breakPoint = navigator.Clone();
                    break;
                }
            }

            //first remove text nodes
            while (navigator.MoveToFollowing(XPathNodeType.Text))
            {
                if (navigator.ComparePosition(breakPoint) == XmlNodeOrder.After)
                    navigator.DeleteSelf();
            }

            //// moves to parent, then move the rest
            navigator.MoveTo(breakPoint);
            while (navigator.MoveToFollowing(XPathNodeType.Element))
            {
                if (navigator.ComparePosition(breakPoint) == XmlNodeOrder.After)
                    navigator.DeleteSelf();
            }

            navigator.MoveToRoot();
            return navigator.InnerXml;
        }
    }
}