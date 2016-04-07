using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Rss;

namespace Informa.Library.Rss.Interfaces
{
   public interface IRssFeedGeneration
   {
       SyndicationFeed GetRssFeed(I_Base_Rss_Feed rssFeed, ISitecoreContext sitecoreContext, IItemReferences itemReferences);
       SyndicationFeed AddFeedLinksToFeed(SyndicationFeed feed, I_Base_Rss_Feed rssFeed);
   }
}
