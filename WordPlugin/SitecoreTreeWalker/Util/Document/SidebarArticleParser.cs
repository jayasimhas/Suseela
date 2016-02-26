using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Word;
using SitecoreTreeWalker.Sitecore;

namespace SitecoreTreeWalker.Util.Document
{
	public class SidebarArticleParser
	{
		public List<Guid> SidebarArticleGuids { get { return _sidebarArticleGuids; } }
		public const string SidebarStyle = "9.0 Sidebar";
		private readonly List<Guid> _sidebarArticleGuids;

		public SidebarArticleParser()
		{
			_sidebarArticleGuids = new List<Guid>();
		}

		public string RetrieveSidebarToken(Paragraph paragraph)
		{
			return RetrieveSidebarToken(paragraph.Range.Text);
		}

		public string RetrieveSidebarToken(string paragraph)
		{
			const string regex = @"\[Sidebar#.*\]";
			string articleGuid = "";
			Match match = Regex.Match(paragraph, regex);
			if(match.Success)
			{
				string matchStr = match.Groups[0].Value;
				string articleNumber = matchStr.Replace("[Sidebar#", "");
				articleNumber = articleNumber.Replace("]", "");
				articleGuid = SitecoreClient.GetArticleGuidByArticleNumber(articleNumber);
				if(new Guid(articleGuid) == Guid.Empty)
				{
					throw new ArgumentException("Invalid sidebar article number inside paragraph contents: \"" + paragraph.TrimEnd() + "\"");
				}
			}
			else
			{
				throw new ArgumentException("Invalid content for sidebar articles: \"" + paragraph.TrimEnd()+"\"");
			}
			if(!articleGuid.IsNullOrEmpty())
			{
				Guid actualGuid = new Guid(articleGuid);
				if(!_sidebarArticleGuids.Contains(actualGuid)) _sidebarArticleGuids.Add(actualGuid);
				return "[Sidebar#{" + articleGuid + "}]";
			}
			return null;
		}
	}
}
