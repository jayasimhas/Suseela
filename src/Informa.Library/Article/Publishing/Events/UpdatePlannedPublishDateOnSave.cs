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
            Assert.ArgumentNotNull(args, "args");

            Item item = Sitecore.Events.Event.ExtractParameter(args, 0) as Item;
            var now = DateTime.Now;

            if (((DateField)item.Fields[IArticleConstants.Planned_Publish_DateFieldName]).DateTime == default(DateTime) || ((DateField)item.Fields[IArticleConstants.Planned_Publish_DateFieldName]).DateTime != DateTime.Now)
            {
                item.Editing.BeginEdit();
                item[IArticleConstants.Planned_Publish_DateFieldName] = DateUtil.ToIsoDate(DateTime.Now);
                item.Editing.EndEdit();
            } 
        }
    }
}
