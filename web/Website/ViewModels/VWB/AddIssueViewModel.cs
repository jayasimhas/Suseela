using System.Collections.Generic;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Virtual_Whiteboard;
using Informa.Library.Services.Article;

namespace Informa.Web.ViewModels.VWB
{
	public class AddIssueViewModel
	{
		public IIssue Issue { get; set; }
		public IEnumerable<IArticle> Articles { get; set; }
        public IArticleService ArticleService { get; set; }
    }
}