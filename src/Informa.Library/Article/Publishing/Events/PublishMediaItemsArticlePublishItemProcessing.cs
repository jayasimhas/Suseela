using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Items;
using Sitecore.Publishing.Pipelines.PublishItem;
using System.Text.RegularExpressions;
using Informa.Library.Publishing.Events;
using Sitecore.Data;
using Sitecore.Links;
using Sitecore.Publishing;
using Glass.Mapper.Sc.Fields;
using Glass.Mapper.Sc;
using Jabberwocky.Glass.Autofac.Attributes;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Util;
using Autofac;
using Autofac.Features.OwnedInstances;
using Sitecore.Publishing.Pipelines.Publish;

namespace Informa.Library.Article.Publishing.Events
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class PublishMediaItemsArticlePublishItemProcessing : PublishProcessor//PublishItemProcessing
    {
        ISitecoreService _sitecoreService => AutofacConfig.ServiceLocator.Resolve<Owned<ISitecoreService>>().Value;
        public IEnumerable<ID> TemplateIds => new List<ID> { IArticleConstants.TemplateId };

        private List<ID> mediaItems { get; set; }
        private PublishContext processArgs { get; set; }
        public void ProcessPublish(ID itemId)
        {
            try
            {
                var item = processArgs.PublishOptions.SourceDatabase.GetItem(itemId);

                if (TemplateIds.Contains(item.TemplateID) == false)
                    return;

                mediaItems = new List<ID>();

                //Find and Parse media items from Body section
                if (item.Fields["Body"] != null && string.IsNullOrEmpty(item.Fields["Body"].Value) == false)
                {
                    var body = item.Fields["Body"].Value;

                    addMediaItemsFromTextToDictionary(body);
                }

                //Find and Parse media items from Summary section
                if (item.Fields["Summary"] != null && string.IsNullOrEmpty(item.Fields["Summary"].Value) == false)
                {
                    var summary = item.Fields["Summary"].Value;

                    addMediaItemsFromTextToDictionary(summary);
                }

                //Get the media item from the featured Image
                if (item.Fields["Featured Image 16 9"] != null && string.IsNullOrEmpty(item.Fields["Featured Image 16 9"].Value) == false)
                {
                    Regex featuredRegex = new Regex("{.*}");
                    Match match = featuredRegex.Match(item.Fields["Featured Image 16 9"].Value);

                    addMediaItemsByIdToDictionary(new ID(match.Value));
                }

                //Get the media items from the Supporting Documents
                if (item.Fields["Supporting Documents"] != null && string.IsNullOrEmpty(item.Fields["Supporting Documents"].Value) == false)
                {
                    foreach (var doc in item.Fields["Supporting Documents"].Value.Split('|'))
                    {
                        if (string.IsNullOrEmpty(doc) == false)
                        {
                            addMediaItemsByIdToDictionary(new ID(doc));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.ToString(), this);
            }
        }

        private void addMediaItemsFromTextToDictionary(string text)
        {
            Regex regex = new Regex("(-/media/.*\" )");

            var matches = regex.Matches(text);

            foreach (Match match in matches)
            {
                try
                {
                    if (match.Value.Contains("\"") == false)
                        continue;

                    string mediaItemUrl = match.Value.Substring(0, match.Value.LastIndexOf("\""));
                    if (string.IsNullOrEmpty(mediaItemUrl))
                        continue;

                    addMediaItemsFromUrlToDictionary("/" + mediaItemUrl);
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error(ex.ToString(), this);
                }
            }
        }

        private void addMediaItemsFromUrlToDictionary(string url)
        {
            if (url.EndsWith("ashx"))
            {
                DynamicLink dynamicLink;
                if (!DynamicLink.TryParse(url, out dynamicLink))
                    return;

                if (dynamicLink != null)
                {
                    addMediaItemsByIdToDictionary(dynamicLink.ItemId);
                }
            }
            else
            {
                var urlWithoutMedia = url.Substring(url.IndexOf("media/")).Replace("media/", "/sitecore/media library/");
                var urlWithoutExt = urlWithoutMedia.Substring(0, urlWithoutMedia.LastIndexOf("."));
                var urlRevertSpaces = urlWithoutExt.Replace("-", " ");

                var item = processArgs.PublishOptions.SourceDatabase.GetItem(urlRevertSpaces);

                if (item != null)
                {
                    addMediaItemsByIdToDictionary(item.ID);
                }
            }
        }

        private void addMediaItemsByIdToDictionary(ID id)
        {
            //MediaItem mediaItem = processArgs.PublishOptions.SourceDatabase.GetItem(id, Sitecore.Context.Language);

            if (mediaItems.Exists(e => e.Guid.ToString() == id.Guid.ToString()) == false)
                mediaItems.Add(id);
        }

        public override void Process(PublishContext context)
        {
            //Fill mediaItems with items to publish
            processArgs = context;
            foreach (var queueItem in context.Queue)
            {
                var items = queueItem.ToList();
                if (items != null)
                {
                    foreach (var item in queueItem)
                    {
                        ProcessPublish(item.ItemId);
                    }
                }
            }

            if (mediaItems != null && mediaItems.Any())
            {
                PublishOptions options = context.PublishOptions;
                //Make the mode smart in order not to publish already published items
                options.Mode = PublishMode.Smart;
                //Add media items to publish into the current publish context's queue
                context.Queue.Add(mediaItems.Select(s => new PublishingCandidate(s, options)).ToList());
            }
        }
    }
}
