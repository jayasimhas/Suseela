using System.ServiceModel.Syndication;
using Glass.Mapper.Sc;
using Sitecore.Data.Items;

namespace Informa.Library.Rss.Interfaces
{
    public interface IRssItemGeneration
    {
        SyndicationItem GetSyndicationItemFromSitecore(ISitecoreContext sitecoreContext,Item item);
    }
}