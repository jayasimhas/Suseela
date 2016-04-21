using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore;
using System;
using Sitecore.Data.Items;
using Sitecore.Publishing.Pipelines.PublishItem;
using Sitecore.Data.Fields;

namespace Informa.Library.Article.Publishing.Events
{
	public class ActualPublishDateArticlePublishItemProcessing : ArticlePublishItemProcessing
	{
		public override void ProcessPublish(Item item, ItemProcessingEventArgs args)
		{
			var now = DateTime.Now;

			if (((DateField)item.Fields[IArticleConstants.Actual_Publish_DateFieldName]).DateTime != default(DateTime) ||
				!item.Publishing.IsPublishable(now, false) ||
				!item.Publishing.IsValid(now, true))
			{
				return;
			}

			item.Editing.BeginEdit();
			item[IArticleConstants.Actual_Publish_DateFieldName] = DateUtil.ToIsoDate(DateTime.Now);
			item.Editing.EndEdit();
		}
	}
}
