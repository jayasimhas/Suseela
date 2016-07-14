using System;
using Informa.Library.Utilities.References;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Models;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Informa.Library.Wrappers
{
    public interface ISitecoreUrlWrapper
    {
        string GetHostName();
        string GetItemUrl(IGlassBase glassItem);
        string GetItemUrl(Guid itemId);
        string GetMediaUrl(IGlassBase glassItem);
        string GetMediaUrl(Guid mediaItemId);
    }

    [AutowireService]
    public class SitecoreUrlWrapper : ISitecoreUrlWrapper
    {
        private readonly IDependencies _dependencies;
				public Database ContextDatabase { get; private set; }

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            IHttpContextProvider HttpContextProvider { get; }
        }

        public SitecoreUrlWrapper(IDependencies dependencies)
        {
            _dependencies = dependencies;
	        ContextDatabase = Sitecore.Context.ContentDatabase ?? Sitecore.Context.Database;
        }

        public string GetHostName() => Sitecore.Web.WebUtil.GetHostName();

        public string GetItemUrl(IGlassBase glassItem) => glassItem != null ? GetItemUrl(glassItem._Id) : null;
        public string GetItemUrl(Guid itemId)
        {
            var item = ContextDatabase.GetItem(new ID(itemId));
            var itemUrl = Sitecore.Links.LinkManager.GetItemUrl(item);
            var scheme = _dependencies.HttpContextProvider.RequestUrl?.Scheme + "://";

            return itemUrl;
        }

        public string GetMediaUrl(IGlassBase glassItem) => glassItem != null ? GetMediaUrl(glassItem._Id) : null;
        public string GetMediaUrl(Guid itemId)
        {
            MediaItem imageItem = ContextDatabase.GetItem(new ID(itemId));

            if (imageItem == null) return null;

            var scheme = _dependencies.HttpContextProvider.RequestUrl?.Scheme + "://";

            return scheme + GetHostName() + Sitecore.Resources.Media.MediaManager.GetMediaUrl(imageItem);
        }

    }
}