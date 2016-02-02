//using System.Collections.Generic;
//using System.Linq;
//using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
//using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
//using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Navigation;
//using Jabberwocky.Glass.Autofac.Attributes;
//using Jabberwocky.Glass.Models;

//namespace Informa.Library.Navigation
//{
//    [AutowireService(LifetimeScope.SingleInstance)]
//    public class TaxonomyItemTreeFactory : ITaxonomyItemTreeFactory
//    {
//        public IEnumerable<INavigation> Create(I___BaseTaxonomy taxonomyItem)
//        {
//            if (taxonomyItem == null)
//            {
//                return Enumerable.Empty<INavigation>();
//            }

//            var looseNavigation = new List<Navigation>();
//            var navigation = new List<Navigation>();
//            var navigationLinkItems = taxonomyItem.Taxonomies.Select(x => GetParentLinkItems(taxonomyItem));

//            foreach (var navigationLinkItem in navigationLinkItems)
//            {
//                var navigationChild = CreateNavigation(navigationLinkItem);

//                if (navigationLinkItem is INavigation_Header)
//                {
//                    var headerChildItems = GetChildLinkItems(navigationLinkItem).Where(cli => cli.Link != null);

//                    navigationChild.Children = headerChildItems.Select(hci => CreateNavigation(hci)).ToList();

//                    navigation.Add(navigationChild);
//                }
//                else if (navigationLinkItem.Link != null)
//                {
//                    looseNavigation.Add(navigationChild);
//                }
//            }

//            if (looseNavigation.Any())
//            {
//                navigation.Add(new Navigation
//                {
//                    Children = looseNavigation
//                });
//            }

//            return navigation;
//        }

//        public Navigation CreateNavigation(INavigation_Link navigationLinkItem)
//        {
//            return new Navigation
//            {
//                Link = navigationLinkItem.Link,
//                Text = string.IsNullOrWhiteSpace(navigationLinkItem.Text) ? (navigationLinkItem.Link == null ? navigationLinkItem._Name : navigationLinkItem.Link.Text) : navigationLinkItem.Text
//            };
//        }

//        public IEnumerable<ITaxonomy_Item> GetParentLinkItems(IGlassBase item)
//        {
//            if (item._Parent is ITaxonomy_Item)
//                yield return item._Parent as ITaxonomy_Item;
//        }
//    }
//}