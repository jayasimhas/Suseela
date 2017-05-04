using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Data;
using Sitecore.Globalization;
using Sitecore.Publishing;
using System.Linq;
using Informa.Library.Publishing.Switcher;
using System;
using Sitecore;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Fields;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;

namespace Informa.Library.Publishing.Scheduled
{
    [AutowireService(LifetimeScope.Default)]
    public class PublishScheduledPublish : IPublishScheduledPublish
    {
        protected readonly IRetrieveItemToPublish RetrieveItemToPublish;
        protected readonly IScheduledPublishingDatabaseContext DatabaseContext;
        protected readonly IScheduledPublishingTargetsContext PublishingTargetsContext;

        public PublishScheduledPublish(
            IRetrieveItemToPublish retrieveItemToPublish,
            IScheduledPublishingDatabaseContext databaseContext,
            IScheduledPublishingTargetsContext publishingTargetsContext)
        {
            RetrieveItemToPublish = retrieveItemToPublish;
            DatabaseContext = databaseContext;
            PublishingTargetsContext = publishingTargetsContext;
        }

        public IPublishingStatus Publish(IScheduledPublish scheduledPublish)
        {
            using (new ScheduledPublishContext())
            {
                using (new DatabaseSwitcher(DatabaseContext.Database))
                {
                    bool? isFinal;
                    var item = RetrieveItemToPublish.Get(scheduledPublish);
                    if (item == null)
                    {
                        return new PublishingStatus
                        {
                            Status = PublishStatus.Failed
                        };
                    }
                    Sitecore.Diagnostics.Log.Warn("item got from scheduledPublish  ", item.ID.ToString());

                    var plannedPublishDateField = (DateField)item.Fields[IArticleConstants.Planned_Publish_DateFieldId];
                    if ((plannedPublishDateField.DateTime == DateTime.MinValue || plannedPublishDateField.DateTime == DateTime.MaxValue) && item.TemplateID == ITableau_DashboardConstants.TemplateId)
                    {
                        var parentArticleItem = item.Axes.SelectSingleItem("ancestor-or-self::*[@@templateid = '" + IArticleConstants.TemplateId + "']");
                        isFinal = Sitecore.Context.Database.WorkflowProvider?.GetWorkflow(parentArticleItem)?.GetState(parentArticleItem)?.FinalState;
                    }
                    else
                    {
                        isFinal =
                            Sitecore.Context.Database.WorkflowProvider?.GetWorkflow(item)?.GetState(item)?.FinalState;
                    }


                    if (isFinal.HasValue && !isFinal.Value)
                    {
                        return new PublishingStatus
                        {
                            Status = PublishStatus.Skipped
                        };
                    }

                    var languages = string.IsNullOrEmpty(scheduledPublish.Language)
                        ? item.Languages
                        : new Language[] { item.Language };
                    var publishingTargetDatabases = PublishingTargetsContext.Databases.ToArray();
                    var handle = PublishManager.PublishItem(item, publishingTargetDatabases, languages, false, false, true);

                    if (handle == null)
                    {
                        return new PublishingStatus
                        {
                            Status = PublishStatus.Failed
                        };
                    }

                    return new PublishingStatus
                    {
                        PublishHandle = handle,
                        Status = PublishStatus.Processing
                    };
                }
            }
        }
    }
}
