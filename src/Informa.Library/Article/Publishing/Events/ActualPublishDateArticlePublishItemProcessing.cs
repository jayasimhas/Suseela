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
			if (((DateField)item.Fields[IArticleConstants.Actual_Publish_DateFieldName]).DateTime != default(DateTime))
			{
				return;
			}

			item.Editing.BeginEdit();
			item[IArticleConstants.Actual_Publish_DateFieldName] = DateUtil.ToIsoDate(DateTime.Now);
			item.Editing.EndEdit();
		}
	}
}
