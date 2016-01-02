using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Word;

namespace SitecoreTreeWalker.Util.Document
{
	public class InlineReferenceParser
	{
		public List<Guid> InlineReferenceGuids { get { return _inlineReferenceGuids; } }
		private readonly List<Guid> _inlineReferenceGuids;

		//public List<Guid> InlineReferenceRanges { get { return _inlineReferenceRanges; } }
		//private readonly List<Guid> _inlineReferenceRanges;

		public InlineReferenceParser()
		{
			_inlineReferenceGuids = new List<Guid>();
		}

		public void ParseDocument(Microsoft.Office.Interop.Word.Document doc)
		{
			string text = doc.Range().Text;
			ParseForInlineReferences(text);
		}

		public string ParseForInlineReferences(string doc)
		{
			const string regex = @"\[A#.*?\]";
			string parsedDoc = doc;
			var match = Regex.Match(doc, regex);

			while (match.Success)
			{
				string articleNumber = match.ToString();
				articleNumber = Regex.Replace(articleNumber, @"\<.*?>", "");
				articleNumber = Regex.Replace(articleNumber, @"\[A#", "");
				articleNumber = Regex.Replace(articleNumber, @"\]", "");
				if(!SitecoreArticle.DoesArticleExist(articleNumber))
				{
					match = match.NextMatch();
					continue;
				}
				var articleGuid = new Guid(SitecoreArticle.GetArticleGuidByArticleNumber(articleNumber));
				if (articleGuid == Guid.Empty)
				{
					match = match.NextMatch(); 
					continue;
				}
				if (!_inlineReferenceGuids.Contains(articleGuid))
				{
					_inlineReferenceGuids.Add(articleGuid);
				}
				match = match.NextMatch();
			}
			return parsedDoc;
		}
	}
}
