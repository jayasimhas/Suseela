using System;
using System.Collections.Generic;
using Glass.Mapper.Sc.Fields;
using Informa.Models.FactoryInterface;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Text;

namespace Informa.Web.ViewModels
{
    public class ArticleListItemModel : IListableViewModel
	{       
		public bool DisplayImage { get; set; }

        public IEnumerable<ILinkable> ListableAuthors { get; set; }
        public DateTime ListableDate { get; set; }
		public string ListableImage { get; set; }
		public string ListableSummary { get; set; }
		public string ListableTitle { get; set; }
		public string ListableByline { get; set; }
		public virtual IEnumerable<ILinkable> ListableTopics { get; set; }
		public string ListableType { get; set; }
		public virtual Link ListableUrl { get; set; }

		#region Implementation of ILinkable

		public string LinkableText { get; set; }
		public string LinkableUrl { get; set; }
		public string Publication { get; set; }

        #endregion

        /// <summary>
        /// Truncate article to specified number of words and appends ...
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxWordCount"></param>
        /// <returns></returns>
        public string TruncateArticle(string text, int maxWordCount)
        {
            string result;
            try
            {
                int maxWordCountLessEllipsis = maxWordCount;

                if (GetWordCount(text) <= maxWordCount)
                    result = text;

                else
                {
                    result = TruncateHtml(text, maxWordCountLessEllipsis);
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

        private string TruncateHtml(string text, int maxWordCount)
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
                lastText = GetWordCount(navigator.Value.Trim()) > wordsAvailable ? GetSubStringWords(wordsAvailable, navigator.Value.Trim()) : navigator.Value;

                wordsAvailable = wordsAvailable - GetWordCount(navigator.Value);

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

        private int GetWordCount(string text)
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

        private string GetSubStringWords(int numberOfWords, string stringWords)
        {
            string[] strArray = stringWords.Split(' ');
            StringBuilder sbReturnString = new StringBuilder();

            for (int i=0; i < numberOfWords; i++)
                sbReturnString.Append(strArray[i] + " ");

            return sbReturnString.ToString();
        }
    }
}