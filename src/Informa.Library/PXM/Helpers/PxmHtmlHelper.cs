using HtmlAgilityPack;
using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;

namespace Informa.Library.PXM.Helpers {
    public interface IPxmHtmlHelper
	{
		string ProcessIframe(string content);
		string ProcessQuickFacts(string content);
        string ProcessTableStyles(string content);
        string ProcessPullQuotes(string content);
        string ProcessQandA(string content);
    }

	[AutowireService]
	public class PxmHtmlHelper : IPxmHtmlHelper
	{
		private readonly IDependencies _dependencies;
		private const string ClassAttributeName = "childstyle";
        private const string QuickFactsTextStyle = "quick-facts__text";
		private const string ColumnHeadingStyle = "table_subhead 1";
		private const string SubHeadingStyle = "table_subhead 2";
		private const string SubHeadAltStyle = "table_subhead 3";
		private const string StoryTextAltStyle = "table_body";

		[AutowireService(IsAggregateService = true)]
		public interface IDependencies
		{

		}

		public PxmHtmlHelper(IDependencies dependencies)
		{
			_dependencies = dependencies;
		}

		public string ProcessIframe(string content)
		{
			var doc = CreateDocument(content);
            var mobileFrames = GetNodes(doc, @"//div[contains(@class, 'iframe-component__mobile')]");
            foreach(HtmlNode m in mobileFrames) {
                m.ParentNode.RemoveChild(m);
            }

            var iframes = GetNodes(doc, @"//div[contains(@class, 'ewf-desktop-iframe')]/iframe");
            foreach(HtmlNode i in iframes) {
                if (i == null)
                    continue;

                var wrapperNode = i?.ParentNode?.ParentNode ?? null;
                if (wrapperNode == null)
                    continue;

                var titleNode = GetPrevNode(wrapperNode, "iframe-title");
                HandleNode(doc, wrapperNode, titleNode, "exhibit_title");
                
                var headerNode = GetPrevNode(wrapperNode, "iframe-header");
                HandleNode(doc, wrapperNode, headerNode, "exhibit_number");
                
                var urlNode = doc.CreateElement("p");
                urlNode.InnerHtml = (i.Attributes["src"] != null) ? i.Attributes["src"].Value : string.Empty;
                wrapperNode.RemoveChild(i.ParentNode);
                HandleNode(doc, wrapperNode, urlNode, "exhibit_url");
                
                var captionNode = GetNextNode(wrapperNode, "iframe-caption");
                HandleNode(doc, wrapperNode, captionNode, "exhibit_caption");
                
                var sourceNode = GetNextNode(wrapperNode, "iframe-source");
                HandleNode(doc, wrapperNode, sourceNode, "exhibit_source");
                
                wrapperNode.Name = "pre";
            }
            
			return doc.DocumentNode.OuterHtml;
		}
        
        public HtmlNode GetPrevNode(HtmlNode n, string cssClass) {

            string attrName = "class";

            var p1 = n.PreviousSibling;
            if (p1 == null)
                return null;

            HtmlAttribute a1 = p1.Attributes[attrName];
            if (a1 != null && a1.Value.Contains(cssClass))
                return p1;

            var p2 = p1.PreviousSibling;
            if (p2 == null)
                return null;

            HtmlAttribute a2 = p1.Attributes[attrName];
            if (a2 != null && a2.Value.Contains(cssClass))
                return p2;

            return null;
        }

        public HtmlNode GetNextNode(HtmlNode n, string cssClass) {

            string attrName = "class";

            var p1 = n.NextSibling;
            if (p1 == null)
                return null;

            HtmlAttribute a1 = p1.Attributes[attrName];
            if (a1 != null && a1.Value.Contains(cssClass))
                return p1;

            var p2 = p1.NextSibling;
            if (p2 == null)
                return null;

            HtmlAttribute a2 = p1.Attributes[attrName];
            if (a2 != null && a2.Value.Contains(cssClass))
                return p2;

            return null;
        }
        
        public void HandleNode(HtmlDocument doc, HtmlNode parent, HtmlNode oldNode, string cssClass) {

            var newNode = doc.CreateElement("p");
            newNode.Attributes.Add("class", cssClass);
            parent.ChildNodes.Append(newNode);
            if (oldNode == null)
                return;

            newNode.InnerHtml = oldNode.InnerHtml;
            if (oldNode.ParentNode == null)
                return;

            oldNode.ParentNode.RemoveChild(oldNode);
        }
        
		public string ProcessQuickFacts(string content)
		{
			var result = AddCssClassToQuickFactsText(content);
			var doc = CreateDocument(result);

			var xpath = @"//div[@class='quick-facts']";
			var nodes = doc.DocumentNode.SelectNodes(xpath);
		
			if (nodes != null)
			{
				var aside = doc.CreateElement("pre");
				doc.DocumentNode.FirstChild.ChildNodes.Insert(0, aside);
                string qfBody = "qf_body";
                foreach (var node in nodes) {
                    foreach (var childNode in node.ChildNodes) {
                        if (!childNode.Name.Equals("p"))
                            continue;
                        var cAttr = childNode.Attributes["class"];
                        if (cAttr == null)
                            childNode.Attributes.Add("class", qfBody);                        
                    }
                }
                ModifyHtmlStructure(doc, aside, nodes);
			}
			
			return doc.DocumentNode.OuterHtml;
		}

		internal void ModifyHtmlStructure(HtmlDocument doc, HtmlNode root, IEnumerable<HtmlNode> nodes)
		{		
			foreach (var node in nodes)
			{
				AppendAndDeleteOriginal(doc, root, node);
			}
		}
        
		internal void AppendAndDeleteOriginal(HtmlDocument doc, HtmlNode root, HtmlNode element)
		{
			if (element != null)
			{
				var parent = element.ParentNode;
				root.AppendChild(element);
				parent.RemoveChild(element);
			}
		}

		public string AddCssClassToQuickFactsText(string content)
		{
			var xpath = @"//div[@class='quick-facts']/p[not(@class)]";
			var result = AddCssClassToElements(content, xpath, ClassAttributeName, QuickFactsTextStyle);
			return result;
		}

		public string ProcessTableStyles(string content)
		{
			var result = ProcessColumnHeading(content);
			result = ProcessSubHeading(result);
			result = ProcessSubHeadAlt(result);
			result = ProcessStoryTextAlt(result);
			var xpath = @"//table/tbody/tr/td/p";
			result = MoveTableContent(result, xpath);
            result = ProcessPullQuotes(result);
            return result;
		}

		internal string ProcessColumnHeading(string content)
		{
			var xpath = @"//table/tbody/tr/td[@class='header colored']/p";
			var result = AddCssClassToElements(content, xpath, ClassAttributeName, ColumnHeadingStyle);
            return result;
		}

		internal string ProcessSubHeading(string content)
		{
			var xpath = @"//table/tbody/tr/td[@class='cell']/p[contains(@class,'highlight')]";
			var result = AddCssClassToElements(content, xpath, ClassAttributeName, SubHeadingStyle);
			return result;
		}

		internal string ProcessSubHeadAlt(string content)
		{
			var xpath = @"//table/tbody/tr/td[@class='cell colored']/p[contains(@class,'highlight')]";
			var result = AddCssClassToElements(content, xpath, ClassAttributeName, SubHeadAltStyle);
			return result;
		}

		internal string ProcessStoryTextAlt(string content)
		{
			var xpath = @"//table/tbody/tr/td[@class='cell colored']/p[contains(@class,'small')]";
			var result = AddCssClassToElements(content, xpath, ClassAttributeName, StoryTextAltStyle);
			return result;
		}
        
        internal string AddCssClassToElements(string content, string xpath, string attributeName, string attributeValue)
		{
			var doc = CreateDocument(content);
			var elements = doc.DocumentNode.SelectNodes(xpath);
			if (elements == null)
			{
				return doc.DocumentNode.OuterHtml;
			}
			foreach (HtmlNode element in elements)
			{
				var attribute = element.ParentNode.Attributes[attributeName];
				if (attribute == null)
				{
					element.ParentNode.Attributes.Add(attributeName, attributeValue);
				}
				else
				{
					attribute.Value = attributeValue;
				}
			}
			return doc.DocumentNode.OuterHtml;
		}

		internal string MoveTableContent(string content, string xpath)
		{
			var doc = CreateDocument(content);
			var elements = doc.DocumentNode.SelectNodes(xpath);
			if (elements == null)
			{
				return doc.DocumentNode.OuterHtml;
			}
			foreach (HtmlNode element in elements)
			{
                var v = element.Attributes["class"];
                if (v == null)
                    element.Attributes.Append(doc.CreateAttribute("class", "table_paragraph"));
				var contentText = element.InnerHtml;
                if(element.ParentNode != null)
				    element.ParentNode.InnerHtml += contentText;
			}
			return doc.DocumentNode.OuterHtml;
		}

		internal HtmlDocument CreateDocument(string content)
		{
			var doc = new HtmlDocument();
			doc.LoadHtml(content);
			return doc;
		}

	    public string ProcessPullQuotes(string content)
	    {
	        var result = ProcessPullQuotesStyle(content);
	        result = ProcessPullQuotesHtml(result);
	        return result;
	    }

	    private string ProcessPullQuotesHtml(string content)
	    {
	        var xpath = @"//div[contains(@class, 'sidebar-body')]//blockquote[contains(@class,'article-pullquote')]";
	        var doc = CreateDocument(content);
	        var elements = doc.DocumentNode.SelectNodes(xpath);
	        if (elements == null)
	        {
	            return doc.DocumentNode.OuterHtml;
	        }
	        foreach (HtmlNode element in elements)
	        {
	            var nodeStr = element.OuterHtml.Replace("blockquote", "p");
	            var blockquote = HtmlNode.CreateNode(nodeStr);
	            element.ParentNode.ReplaceChild(blockquote, element);
	        }
	        return doc.DocumentNode.OuterHtml;
	    }

        private string ProcessPullQuotesStyle(string content) {
            var xpath = @"//div[contains(@class, 'sidebar-body')]//blockquote[contains(@class,'article-pullquote')]/p";
            var doc = CreateDocument(content);
            var elements = doc.DocumentNode.SelectNodes(xpath);
            if (elements == null)
                return doc.DocumentNode.OuterHtml;

            string classAttr = "class";
            string SidebarPullQuote = "sidebar_quote";
            foreach (HtmlNode element in elements) {
                var attribute = element.Attributes[classAttr];
                if (attribute == null)
                    element.Attributes.Add(classAttr, SidebarPullQuote);
                else
                    attribute.Value = SidebarPullQuote;
            }
            return doc.DocumentNode.OuterHtml;
        }




        public string ProcessQandA(string content) {
            var doc = CreateDocument(content);
            var paras = GetNodes(doc, @"//div[contains(@class, 'article-interview__answer')]//p");
            foreach(var q in paras) {
                q.Name = "div";
                var a = q.Attributes["class"];
                if (a != null && !a.Value.Contains("article-interview__answer")) 
                    a.Value = $"{a.Value} article-interview__answer";
                else
                    q.Attributes.Add("class", "article-interview__answer");

                var oldWrapNode = q.ParentNode;
                var sib = GetNewAnswerNode(oldWrapNode);
                
                sib.AppendChild(q);
                oldWrapNode.RemoveChild(q);
            }
            

            var divs = GetNodes(doc, @"//div[contains(@class, 'article-interview__answer')]");
            foreach(var d in divs) {
                
                var sib = GetNewAnswerNode(d);
                var newNode = HtmlNode.CreateNode($"<p class=\"article-interview__answer\">{d.InnerText}</p>");
                sib.PrependChild(newNode);

                d.ParentNode.RemoveChild(d);
            }


            return doc.DocumentNode.OuterHtml;
        }

        private HtmlNode GetNewAnswerNode(HtmlNode answerRoot) {
            var sib = answerRoot.NextSibling;
            if (sib.Attributes["class"] == null || !sib.Attributes["class"].Value.Equals("answer-wrap")) {
                sib = HtmlNode.CreateNode($"<p class=\"answer-wrap\"></p>");
                answerRoot.ParentNode.InsertAfter(sib, answerRoot);
            }

            return sib;
        }


        

        private IEnumerable<HtmlNode> GetNodes(HtmlDocument doc, string xPath) {
            var nodes = doc.DocumentNode.SelectNodes(xPath);
            return (nodes == null)
                ? Enumerable.Empty<HtmlNode>()
                : nodes.ToList();
        }

    }
}