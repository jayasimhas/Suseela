using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore;
using System;
using Sitecore.Data.Items;
using Sitecore.Publishing.Pipelines.PublishItem;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;

namespace Informa.Library.Publishing.Scheduled.Events
{
    public class UpdatePlannedPublishDateOnSave
    {
        public void Process(object sender, EventArgs args)
        {
            try
            {
                Assert.ArgumentNotNull(args, "args");
                Item item = Sitecore.Events.Event.ExtractParameter(args, 0) as Item;
                var now = DateTime.Now;
                if (item != null && item.Database.Name.ToLower() == "master" && string.Equals(item.TemplateID.ToString(), IArticleConstants.TemplateId.ToString(), StringComparison.OrdinalIgnoreCase) && item.Versions.Count > 0)
                {
                    if (((DateField)item.Fields[IArticleConstants.Planned_Publish_DateFieldName]).DateTime == default(DateTime) || ((DateField)item.Fields[IArticleConstants.Planned_Publish_DateFieldName]).DateTime == null)
                    {
                        item.Editing.BeginEdit();
                        item[IArticleConstants.Planned_Publish_DateFieldName] = DateUtil.ToIsoDate(DateTime.Now);
                        item.Editing.EndEdit();
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error updating planned publish date", ex, Sitecore.Context.User.GetLocalName());
            }
        }
    }
}
