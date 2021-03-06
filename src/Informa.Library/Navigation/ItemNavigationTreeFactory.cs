﻿using System;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Navigation;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Models;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Services.Global;
using Jabberwocky.Core.Caching;
using System.Diagnostics;
using Informa.Library.Utilities.Extensions;

namespace Informa.Library.Navigation
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class ItemNavigationTreeFactory : IItemNavigationTreeFactory
    {
        private readonly IDependencies _dependencies;

        [AutowireService(true)]
        public interface IDependencies
        {
            IGlobalSitecoreService GlobalService { get; }
            ICacheProvider CacheProvider { get; }
        }

        public ItemNavigationTreeFactory(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public IEnumerable<INavigation> Create(Guid navigationRootGuid)
            => Create(_dependencies.GlobalService.GetItem<INavigation_Root>(navigationRootGuid));
        private static object _syncRoot = new object();
        public IEnumerable<INavigation> Create(INavigation_Root navigationRootItem)
        {
            if (navigationRootItem == null)
            {
                return Enumerable.Empty<INavigation>();
            }
            Stopwatch sw = Stopwatch.StartNew();
            StringExtensions.WriteSitecoreLogs("Reached ItemNavigation Create method at :", sw, "ItemSideNavigationTreeFactory");
            string cacheKey = $"{nameof(ItemNavigationTreeFactory)}-Create-{navigationRootItem._Id}";
            //return _dependencies.CacheProvider.GetFromCache(cacheKey, () => BuildNavigation(navigationRootItem));
            lock (_syncRoot)
            {
                return _dependencies.CacheProvider.GetFromCache(cacheKey, () => BuildNavigation(navigationRootItem));
            }
        }

        private IEnumerable<INavigation> BuildNavigation(INavigation_Root navigationRootItem)
        {
            var navigation = new List<Navigation>();
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                if (navigationRootItem == null)
                {
                    return Enumerable.Empty<INavigation>();
                }
                var looseNavigation = new List<Navigation>();
                var navigationLinkItems = GetChildLinkItems(navigationRootItem);
                foreach (var navigationLinkItem in navigationLinkItems)
                {
                    var navigationChild = CreateNavigation(navigationLinkItem);

                    if (navigationLinkItem is INavigation_Header)
                    {
                        var headerChildItems = GetChildLinkItems(navigationLinkItem).Where(cli => cli.Navigation_Link != null);

                        navigationChild.Children = headerChildItems.Select(CreateNavigation);

                        navigation.Add(navigationChild);
                    }
                    else if (navigationLinkItem.Navigation_Link != null)
                    {
                        looseNavigation.Add(navigationChild);
                    }
                }

                if (looseNavigation.Any())
                {
                    navigation.Add(new Navigation
                    {
                        Children = looseNavigation
                    });
                }
                StringExtensions.WriteSitecoreLogs("Build Naviugation Method from Sitecore End at :", sw, "SitemapService");
                return navigation;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Navigation Error", ex, "ItemNavigationTreeFactory");
                return navigation;
            }
        }

        public Navigation CreateNavigation(INavigation_Link navigationLinkItem)
        {
            return new Navigation
            {
                Link = navigationLinkItem.Navigation_Link,
                Text = string.IsNullOrWhiteSpace(navigationLinkItem.Navigation_Text) ? (navigationLinkItem.Navigation_Link == null ? navigationLinkItem._Name : navigationLinkItem.Navigation_Link.Text) : navigationLinkItem.Navigation_Text
            };
        }

        public IEnumerable<INavigation_Link> GetChildLinkItems(IGlassBase item)
        {
            return item._ChildrenWithInferType.OfType<INavigation_Link>();
        }
    }
}