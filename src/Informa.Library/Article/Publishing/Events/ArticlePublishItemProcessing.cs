using Informa.Library.Publishing.Events;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data;
using System.Collections.Generic;

namespace Informa.Library.Article.Publishing.Events
{
	public abstract class ArticlePublishItemProcessing : PublishItemProcessing
	{
		public override IEnumerable<ID> TemplateIds => new List<ID> { IArticleConstants.TemplateId };
	}
}
