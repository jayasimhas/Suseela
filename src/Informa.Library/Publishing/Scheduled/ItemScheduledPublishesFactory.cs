using Sitecore.Data.Items;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Fields;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;

namespace Informa.Library.Publishing.Scheduled
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class ItemScheduledPublishesFactory : IItemScheduledPublishesFactory
    {
        protected readonly IScheduledPublishingDateTime ScheduledPublishingDateTime;

        public ItemScheduledPublishesFactory(
            IScheduledPublishingDateTime scheduledPublishingDateTime)
        {
            ScheduledPublishingDateTime = scheduledPublishingDateTime;
        }

        public IEnumerable<IScheduledPublish> Create(Item item)
        {
            var scheduledPublishes = new List<ScheduledPublish>();
            var itemId = item.ID.Guid;
            var language = item.Language.Name;
            var version = item.Version.Number.ToString();

            var plannedPublishDateField = (DateField)item.Fields[IArticleConstants.Planned_Publish_DateFieldId];
            var wfState = item.State.GetWorkflowState();
            if (plannedPublishDateField != null)
            {
                if ((plannedPublishDateField.DateTime == DateTime.MinValue || plannedPublishDateField.DateTime == DateTime.MaxValue) && item.TemplateID == ITableau_DashboardConstants.TemplateId)
                {
                    //var parentArticleItem = item.Axes.SelectSingleItem("ancestor-or-self::*[@@templateid = '" + IArticleConstants.TemplateId + "']");
                    plannedPublishDateField.Value = Sitecore.DateUtil.ToIsoDate(DateTime.Now);
                    //wfState = parentArticleItem.State.GetWorkflowState();
                }

                AddScheduledPublish(scheduledPublishes, itemId, string.Empty, string.Empty, plannedPublishDateField.DateTime, ScheduledPublishType.Planned);
                var subItems = item.Axes.GetDescendants();
                if (subItems.Length > 0)
                {
                    foreach (var subItem in subItems)
                    {
                        AddScheduledPublish(scheduledPublishes, subItem.ID.ToGuid(), string.Empty, string.Empty, plannedPublishDateField.DateTime, ScheduledPublishType.Planned);
                    }
                }

                //To prevent publishing of articles according to their PublishDate or ValidFrom before their Planned Publish Date
                if (plannedPublishDateField.DateTime <= ScheduledPublishingDateTime.Now && wfState != null && wfState.FinalState)
                {
                    AddScheduledPublish(scheduledPublishes, itemId, string.Empty, string.Empty, item.Publishing.PublishDate, ScheduledPublishType.From);
                    AddScheduledPublish(scheduledPublishes, itemId, language, version, item.Publishing.ValidFrom, ScheduledPublishType.From);

                    //var subItems1 = item.Axes.GetDescendants();
                    //if (subItems1.Length > 0)
                    //{
                    //    foreach (var subItem in subItems1)
                    //    {
                    //        AddScheduledPublish(scheduledPublishes, subItem.ID.ToGuid(), string.Empty, string.Empty, subItem.Publishing.PublishDate, ScheduledPublishType.From);
                    //        AddScheduledPublish(scheduledPublishes, subItem.ID.ToGuid(), string.Empty, string.Empty, subItem.Publishing.ValidFrom, ScheduledPublishType.From);
                    //    }
                    //}
                }
            }

            //The following have been excluded from the above since they do not result in publishing but unpublishing of articles.
            AddScheduledPublish(scheduledPublishes, itemId, string.Empty, string.Empty, item.Publishing.UnpublishDate, ScheduledPublishType.To);
            AddScheduledPublish(scheduledPublishes, itemId, language, version, item.Publishing.ValidTo, ScheduledPublishType.To);

            return scheduledPublishes;
        }

        public bool HasValidValue(DateTime value)
        {
            return value != DateTime.MinValue && value != DateTime.MaxValue;
        }

        public void AddScheduledPublish(List<ScheduledPublish> scheduledPublishes, Guid itemId, string language, string version, DateTime publishOn, ScheduledPublishType type)
        {
            if (HasValidValue(publishOn))
            {
                scheduledPublishes.Add(new ScheduledPublish
                {
                    ItemId = itemId,
                    Language = language,
                    Published = false,
                    PublishOn = publishOn.ToUniversalTime(),
                    Version = version,
                    Type = type
                });
            }
        }
    }
}
