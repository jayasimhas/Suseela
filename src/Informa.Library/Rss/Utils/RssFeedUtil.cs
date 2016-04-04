using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Rss;
using Newtonsoft.Json;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Velir.Search.Models;

namespace Informa.Library.Rss.Utils
{
    public class RssFeedUtil
    {
        private readonly XNamespace atom = "http://www.w3.org/2005/Atom";
        protected string _hostName;
        protected ItemReferences _itemReferences;
        protected ISitecoreContext _sitecoreContext;

        public RssFeedUtil(ISitecoreContext sitecoreContext, string hostName, ItemReferences itemReferences)
        {
            _sitecoreContext = sitecoreContext;
            _hostName = hostName;
            _itemReferences = itemReferences;
        }

        //public SyndicationFeed GetSearchRssFeed(ISearch_Rss_Feed rssFeedItem)
        //{
        //    var feed = new SyndicationFeed();
        //    feed.AttributeExtensions.Add(new XmlQualifiedName("atom", XNamespace.Xmlns.NamespaceName),
        //        atom.NamespaceName);

        //    feed.Title = new TextSyndicationContent(GetSearchFeedTitle());
        //    feed.Language = "en-US";

        //    feed = AddFeedLinksToFeed(feed);
        //    feed = AddCopyrightTextToFeed(feed, _sitecoreContext, _itemReferences.SiteConfig);

        //    return feed;
        //}

      




       
    }
}