using System;
using System.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Publishing.Scheduled.History;
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
		private readonly IFindItemScheduledPublishHistories _publishHistory;
		private readonly ILog _logger;
		private readonly Func<Database, ISitecoreService> _serviceFactory;

		public ItemsPublishingProcessor(ILog logger, Func<Database, ISitecoreService> serviceFactory,
			INlmExportService exportService, IFindItemScheduledPublishHistories publishHistory)
		{
			if (logger == null) throw new ArgumentNullException(nameof(logger));
			if (serviceFactory == null) throw new ArgumentNullException(nameof(serviceFactory));
			if (exportService == null) throw new ArgumentNullException(nameof(exportService));
			if (publishHistory == null) throw new ArgumentNullException(nameof(publishHistory));
			_logger = logger;
			_serviceFactory = serviceFactory;
			_exportService = exportService;
			_publishHistory = publishHistory;
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

						_logger.Info($"Exported NLM delete for article: '{itemId}'");
					}
						break;

					// Handle 'New Version' Publish
					case PublishAction.PublishVersion:
					{
						var database = _serviceFactory(context.PublishOptions.SourceDatabase);
						var article = database.Cast<ArticleItem>(sourceItem);
						var isFirstScheduledPublishForItem = !_publishHistory.Find(sourceItem.ID.Guid).Any();

						// Check if the current article is migrated content. If so, don't generate NLM
						if (!string.IsNullOrEmpty(article.Legacy_Article_Number))
						{
							_logger.Info($"Skipping NLM export for article {article.Article_Number} (article is a legacy PMBI article {article.Legacy_Article_Number}): article ID - '{itemId}'");
							return;
						}
						if (!string.IsNullOrEmpty(article.Escenic_ID))
						{
							_logger.Info($"Skipping NLM export for article {article.Article_Number} (article is legacy Escenic article {article.Escenic_ID}): article ID - '{itemId}'");
							return;
						}

						if (!isFirstScheduledPublishForItem)
						{
							_logger.Info($"Skipping NLM export for item (article already published via Scheduled Publishing): '{itemId}'");
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

    public class DictionaryCacheClearer
    {
        /// <summary>
        /// Clears the whole dictionary domain cache.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        public void ClearCache(object sender, EventArgs args)
        {
            Sitecore.Globalization.Translate.ResetCache();
            Sitecore.Diagnostics.Log.Info("Global Dictionary cleared", "DicionaryCache Clearer");
        }
    }
}