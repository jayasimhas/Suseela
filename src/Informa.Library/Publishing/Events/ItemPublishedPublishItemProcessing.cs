using Sitecore.Data.Items;
using Sitecore.Publishing.Pipelines.PublishItem;
using Sitecore.Data.Fields;
using Sitecore.Data;
using System.Collections.Generic;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using System.Linq;

namespace Informa.Library.Publishing.Events
{
	public class ActualPublishDateArticlePublishItemProcessing : PublishItemProcessing
	{
		public override IEnumerable<ID> TemplateIds => Enumerable.Empty<ID>();

		public override void ProcessPublish(Item item, ItemProcessingEventArgs args)
		{
			var itemPublishedField = ((CheckboxField)item.Fields[I___PublishStatusConstants.___Item_PublishedFieldName]);

			if (itemPublishedField == null || itemPublishedField.Checked)
			{
				return;
			}

			item.Editing.BeginEdit();
			itemPublishedField.Checked = true;
			item.Editing.EndEdit();
		}
	}
}
