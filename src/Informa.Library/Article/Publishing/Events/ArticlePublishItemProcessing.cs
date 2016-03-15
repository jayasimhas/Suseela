using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Publishing.Pipelines.PublishItem;
using System;

namespace Informa.Library.Article.Publishing.Events
{
	public class ArticlePublishItemProcessing
	{
		public void Process(object sender, EventArgs args)
		{
			var publishArgs = args as ItemProcessingEventArgs;

			if (publishArgs == null)
			{
				return;
			}

			var item = publishArgs.Context.PublishHelper.GetSourceItem(publishArgs.Context.ItemId);

			if (item == null || 
				item.TemplateID != IArticleConstants.TemplateId || 
				((DateField)item.Fields[IArticleConstants.Actual_Publish_DateFieldName]).DateTime != default(DateTime))
			{
				return;
			}

			item.Editing.BeginEdit();
			item[IArticleConstants.Actual_Publish_DateFieldName] = DateUtil.ToIsoDate(DateTime.Now);
			item.Editing.EndEdit();
		}
	}
}
