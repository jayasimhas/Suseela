﻿using System;
using System.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Publishing.Scheduled;
using Informa.Library.Publishing.Switcher;
using Informa.Library.Services.NlmExport;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Pipelines.Processors;
using log4net;
using Sitecore.Common;
using Sitecore.Data;
using Sitecore.Publishing;
using Sitecore.Publishing.Pipelines.PublishItem;

namespace Informa.Library.CustomSitecore.Pipelines.PublishItem
{
    public class ItemsPublishingProcessor : ProcessorBase<PublishItemContext>
    {
        private readonly INlmExportService _exportService;
        private readonly IFindScheduledPublishes _scheduledPublishes;
        private readonly ILog _logger;
        private readonly Func<Database, ISitecoreService> _serviceFactory;

        public ItemsPublishingProcessor(ILog logger, Func<Database, ISitecoreService> serviceFactory,
            INlmExportService exportService, IFindScheduledPublishes scheduledPublishes)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (serviceFactory == null) throw new ArgumentNullException(nameof(serviceFactory));
            if (exportService == null) throw new ArgumentNullException(nameof(exportService));
            if (scheduledPublishes == null) throw new ArgumentNullException(nameof(scheduledPublishes));
            _logger = logger;
            _serviceFactory = serviceFactory;
            _exportService = exportService;
            _scheduledPublishes = scheduledPublishes;
        }

        protected override void Run(PublishItemContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                _logger.Debug("Export to NLM started on Publish. Context.Action = [" + context.Action + "].");

                var itemId = context.ItemId;
                var sourceItem = context.VersionToPublish ?? context.PublishOptions.SourceDatabase.GetItem(itemId);
                var targetItem = context.PublishOptions.TargetDatabase.GetItem(itemId);

                // Check if the current item is actually an Article item
                if (sourceItem == null || sourceItem.TemplateID != IArticleConstants.TemplateId)
                {
                    _logger.Debug($"Skipping NLM export for item (not an article): '{itemId}'");
                    return;
                }

                switch (context.Action)
                {
                    // Handle 'Delete' Publish
                    case PublishAction.DeleteTargetItem:
                        {
                            var database = _serviceFactory(context.PublishOptions.TargetDatabase);
                            var article = database.Cast<ArticleItem>(targetItem);

                            // Export a _del.xml file
                            _exportService.DeleteNlm(article);

                            _logger.Debug($"Exported NLM delete for article: '{itemId}'");
                        }
                        break;

                    // Handle 'New Version' Publish
                    case PublishAction.PublishVersion:
                        {
                            var database = _serviceFactory(context.PublishOptions.SourceDatabase);
                            var article = database.Cast<ArticleItem>(sourceItem);

                            var isFirstScheduledPublishForItem = _scheduledPublishes.Find(sourceItem.ID.Guid, sourceItem.Language.ToString(), sourceItem.Version.Number.ToString())
                                    .Any(pub => !pub.Published);
                            var isCurrentPublishScheduled = Switcher<ScheduledState>.CurrentValue;

                            if (!isFirstScheduledPublishForItem || isCurrentPublishScheduled != ScheduledState.IsScheduledPublish)
                            {
                                _logger.Debug($"Skipping NLM export for item (article already published via Scheduled Publishing): '{itemId}'");
                                return;
                            }

                            // Export the article as an NLM file
                            _exportService.ExportNlm(article, ExportType.Scheduled);

                            context.AddMessage($"NLM export of article '{itemId}' was successful.");
                        }
                        break;
                }

                _logger.Debug("Export to NLM ended on Publish.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Unexpected error occurred while exporting Article ID {context.ItemId} on publish.", ex);
            }
        }
    }
}
