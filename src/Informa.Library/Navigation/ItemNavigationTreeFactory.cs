using System;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Navigation;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Models;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Services.Global;
using Jabberwocky.Core.Caching;
using Informa.Library.Utilities.Extensions;
using System.Web;
using Informa.Library.Globalization;
using System.Diagnostics;

namespace Informa.Library.Navigation
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ItemNavigationTreeFactory : IItemNavigationTreeFactory
	{
	    private readonly IDependencies _dependencies;
        private readonly ITextTranslator TextTranslator;

        [AutowireService(true)]
	    public interface IDependencies
	    {
            IGlobalSitecoreService GlobalService { get; }
            ICacheProvider CacheProvider { get; }
	    }

	    public ItemNavigationTreeFactory(IDependencies dependencies,ITextTranslator textTranslator)
	    {
	        _dependencies = dependencies;
            TextTranslator = textTranslator;
        }

	    public IEnumerable<INavigation> Create(Guid navigationRootGuid)
	        => Create(_dependencies.GlobalService.GetItem<INavigation_Root>(navigationRootGuid));

	    public IEnumerable<INavigation> Create(INavigation_Root navigationRootItem)
	    {
            if (navigationRootItem == null) {
                return Enumerable.Empty<INavigation>();
            }
            Stopwatch sw = Stopwatch.StartNew();
            StringExtensions.WriteSitecoreLogs("Reached ItemNavigation Create method at :", sw,"ItemSideNavigationTreeFactory");
            string cacheKey = $"{nameof(ItemNavigationTreeFactory)}-Create-{navigationRootItem._Id}";
            return _dependencies.CacheProvider.GetFromCache(cacheKey, () => BuildNavigation(navigationRootItem));
        }

        private IEnumerable<INavigation> BuildNavigation(INavigation_Root navigationRootItem)
        {
            Stopwatch sw = Stopwatch.StartNew();
            var navigation = new List<Navigation>();
            try
            {
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
                        //var headerChildItems = GetChildLinkItems(navigationLinkItem).Where(cli => cli.Navigation_Link != null);
                        var headerChildItems = GetChildLinkItems(navigationLinkItem);
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
            Stopwatch sw = Stopwatch.StartNew();
            var childItems = item._ChildrenWithInferType.OfType<INavigation_Link>();
            if (childItems != null && childItems.Any())
            {
                List<INavigation_Link> listNavigation = new List<INavigation_Link>();
                foreach (var nav in childItems)
                {
                    if (nav.Navigation_Link != null)
                        listNavigation.Add(nav);
                }
                StringExtensions.WriteSitecoreLogs("GetChildLinkItems Method if Condition at :", sw, "ItemNavigationTreeFactory");
                return listNavigation;
            }
            StringExtensions.WriteSitecoreLogs("GetChildLinkItems Method else Condition at :", sw, "ItemNavigationTreeFactory");
            return childItems;
        }
    }
}
