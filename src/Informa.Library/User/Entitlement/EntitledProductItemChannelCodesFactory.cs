using Informa.Library.Publication;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Entitlement
{
    [AutowireService]
    public class EntitledProductItemChannelCodesFactory : IEntitledProductItemChannelCodesFactory
    {
        protected readonly ISiteRootsContext SiteRootsContext;
        protected readonly ISitePublicationCodeFactory PublicationCodeFactory;
        private string channelCodeFormat = "{0}.{1}";

        public EntitledProductItemChannelCodesFactory(
            ISiteRootsContext siteRootsContext,
            ISitePublicationCodeFactory publicationCodeFactory)
        {
            SiteRootsContext = siteRootsContext;
            PublicationCodeFactory = publicationCodeFactory;
        }

        public IList<string> Create(IArticle item)
        {
            var channelCodes = new List<string>();
            if (item.Taxonomies.Any())
            {
                var siteRoot = SiteRootsContext.SiteRoots.FirstOrDefault(sr => item._Path.StartsWith(sr._Path));
                var productCode = siteRoot?.Publication_Code ?? string.Empty;
                var channelsPage = siteRoot._ChildrenWithInferType.OfType<IHome_Page>()?.FirstOrDefault()?
                                ._ChildrenWithInferType.OfType<IChannels_Page>().FirstOrDefault();
                if (channelsPage != null)
                {
                    var channelItems = channelsPage._ChildrenWithInferType.OfType<IChannel_Page>();
                    if (channelItems.Any())
                    {
                        foreach (IChannel_Page channelPage in channelItems)
                        {
                            var channelCode = string.Empty;
                            if (channelPage.Taxonomies.Any())
                            {
                                foreach (ITaxonomy_Item taxonomy in channelPage.Taxonomies)
                                {
                                    if (item.Taxonomies.Where(tax => tax._Id.Equals(taxonomy._Id)).Any())
                                    {
                                        channelCode = string.Format(channelCodeFormat, productCode, channelPage.Channel_Code);
                                        break;
                                    }
                                }
                            }
                            if (string.IsNullOrWhiteSpace(channelCode))
                            {
                                var pageAssetsItem = channelPage._ChildrenWithInferType.OfType<IPage_Assets>().FirstOrDefault();
                                if (pageAssetsItem != null)
                                {
                                    var topicItems = pageAssetsItem._ChildrenWithInferType.OfType<ITopic>();
                                    if (topicItems.Any())
                                    {
                                        foreach (ITopic topicPage in topicItems)
                                        {
                                            if (topicPage.Taxonomies.Any())
                                            {
                                                foreach (ITaxonomy_Item taxonomy in topicPage.Taxonomies)
                                                {
                                                    if (item.Taxonomies.Where(tax => tax._Id.Equals(taxonomy._Id)).Any())
                                                    {
                                                        channelCode = string.Format(channelCodeFormat, productCode, channelPage.Channel_Code);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(channelCode))
                            {
                                channelCodes.Add(channelCode);
                            }
                        }
                    }
                }
            }
            return channelCodes;
        }
    }
}