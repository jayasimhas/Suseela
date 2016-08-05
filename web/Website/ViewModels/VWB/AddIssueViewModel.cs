using System.Collections.Generic;
using System.Linq;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Virtual_Whiteboard;
using Informa.Library.Services.Article;
using Informa.Library.Utilities.Extensions;

namespace Informa.Web.ViewModels.VWB
{
	public class AddIssueViewModel
	{
		public IIssue Issue { get; set; }
		public IEnumerable<IArticle> Articles { get; set; }
        public IArticleService ArticleService { get; set; }

	    public string GetSidebars(IArticle article)
	        => string.Join("<br>", 
                article.Referenced_Articles
                    .Select(a => ((IArticle) a)?.Article_Number)
                    .Where(a => a.HasContent()));
	}
}