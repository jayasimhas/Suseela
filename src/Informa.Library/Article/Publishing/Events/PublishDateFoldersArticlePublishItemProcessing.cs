using Sitecore.Data.Items;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Folders;
using Sitecore.Publishing;
using Sitecore.Publishing.Pipelines.PublishItem;
using Sitecore.Data;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Fields;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;

namespace Informa.Library.Article.Publishing.Events
{
	public class PublishDateFoldersArticlePublishItemProcessing : ArticlePublishItemProcessing
	{
		public override void ProcessPublish(Item item, ItemProcessingEventArgs args)
		{
			var parentFolderItems = new List<Item>();
			var parentItem = item?.Parent;

			while (
				parentItem != null &&
				parentItem.TemplateID == IArticle_Date_FolderConstants.TemplateId)
			{
				if (!((CheckboxField)parentItem.Fields[I___PublishStatusConstants.___Item_PublishedFieldName]).Checked)
				{
					parentFolderItems.Insert(0, parentItem);
				}

				parentItem = parentItem.Parent;
			}

			if (!parentFolderItems.Any())
			{
				return;
			}

			parentFolderItems.Add(item);
			parentFolderItems.ForEach(pfi => PublishManager.PublishItem(
				pfi,
				new Database[] { args.Context.PublishOptions.TargetDatabase },
				new Sitecore.Globalization.Language[] { args.Context.PublishOptions.Language },
				false,
				false,
				false));
		}
	}
}
