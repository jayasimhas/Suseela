using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Navigation;
using Jabberwocky.Glass.Autofac.Attributes;
using Jabberwocky.Glass.Models;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Navigation
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ItemNavigationTreeFactory : IItemNavigationTreeFactory
	{
		public IEnumerable<INavigation> Create(INavigation_Root navigationRootItem)
		{
			if (navigationRootItem == null)
			{
				return Enumerable.Empty<INavigation>();
			}

			var looseNavigation = new List<Navigation>();
			var navigation = new List<Navigation>();
			var navigationLinkItems = GetChildLinkItems(navigationRootItem);

			foreach (var navigationLinkItem in navigationLinkItems)
			{
				var navigationChild = CreateNavigation(navigationLinkItem);

				if (navigationLinkItem is INavigation_Header)
				{
					var headerChildItems = GetChildLinkItems(navigationLinkItem).Where(cli => cli.Link != null);

					navigationChild.Children = headerChildItems.Select(hci => CreateNavigation(hci)).ToList();

					navigation.Add(navigationChild);
				}
				else if (navigationLinkItem.Link != null)
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

			return navigation;
		}

		public Navigation CreateNavigation(INavigation_Link navigationLinkItem)
		{
			return new Navigation
			{
				Link = navigationLinkItem.Link,
				Text = string.IsNullOrWhiteSpace(navigationLinkItem.Text) ? (navigationLinkItem.Link == null ? navigationLinkItem._Name : navigationLinkItem.Link.Text) : navigationLinkItem.Text
			};
		}

		public IEnumerable<INavigation_Link> GetChildLinkItems(IGlassBase item)
		{
			return item._ChildrenWithInferType.OfType<INavigation_Link>().ToList();
		}
	}
}
