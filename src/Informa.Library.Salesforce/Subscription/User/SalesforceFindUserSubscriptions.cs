using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.Subscription;
using Informa.Library.Subscription.User;
using Informa.Library.User.UserPreference;
using Newtonsoft.Json;

namespace Informa.Library.Salesforce.Subscription.User
{
    public class SalesforceFindUserSubscriptions : IFindUserSubscriptions
    {
        protected readonly ISalesforceServiceContext Service;

        public SalesforceFindUserSubscriptions(
            ISalesforceServiceContext service)
        {
            Service = service;
        }

        public IEnumerable<ISubscription> Find(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }


            string cleanUser = username.Split('@')[0];
            //cleanUesr = cleanUesr.Replace("@example.com", "");

            Sitecore.Data.Items.Item SalesForceXmlItem =
                Sitecore.Context.Database.GetItem("/sitecore/media library/SalesforceXML/" + cleanUser.Trim());
            QuerySubscriptionsAndPurchasesResponse xmlResponse = null;

            if (SalesForceXmlItem != null)
            {
                Sitecore.Data.Items.Item xmlMedia = new Sitecore.Data.Items.MediaItem(SalesForceXmlItem);
                var fileType = xmlMedia.Fields["Extension"].Value;
                if (fileType == "xml" || fileType == "XML")
                {
                    XmlDocument xmdDoc = new XmlDocument();
                    xmdDoc.Load(Sitecore.Resources.Media.MediaManager.GetMedia(xmlMedia).GetStream().Stream);
                    XmlDocument innerXmdDoc = new XmlDocument();
                    innerXmdDoc.LoadXml(xmdDoc.LastChild.OuterXml);
                    xmlResponse =
                        DeserializeXMLFileToObject<QuerySubscriptionsAndPurchasesResponse>(innerXmdDoc.InnerXml);
                }
            }
            if (xmlResponse != null && xmlResponse.SubscriptionsAndPurchasesResponse.Any())
            {
                return xmlResponse.SubscriptionsAndPurchasesResponse
                .Where(sap => string.Equals(sap.productType, "publication", StringComparison.InvariantCultureIgnoreCase))
                .Select(sap => new SalesforceSubscription
                {
                    DocumentID = sap.documentId,
                    ProductCode = sap.productCode,
                    ProductGuid = sap.productGUID,
                    SubscriptionType = sap.subscriptionType,
                    Publication = sap.name,
                    ProductType = sap.productType,
                    ExpirationDate = sap.expirationDate,
                    SubscribedChannels = (sap.channels.Select(ch => new ChannelSubscription
                    {
                         ChannelId = ch.code,
                        ChannelName = ch.name,
                        ExpirationDate = ch.expirydate,
                        SubscribedTopics = (ch.topics.Select(tp => new TopicSubscription
                        {
                            TopicId = tp.code,
                            TopicName = tp.name,
                            ExpirationDate = tp.expirydate
                        })).ToList()
                    })).ToList(),
                    

                }).ToList();

            }

            var response = Service.Execute(s => s.INquerySubscriptionsAndPurchases(username));

            if (!response.IsSuccess())
            {
                return null;
            }

            if (response.subscriptionsAndPurchases == null || !response.subscriptionsAndPurchases.Any())
            {
                return Enumerable.Empty<ISubscription>();
            }

            return response.subscriptionsAndPurchases
                .Where(sap => string.Equals(sap.productType, "publication", StringComparison.InvariantCultureIgnoreCase))
                .Select(sap => new SalesforceSubscription
                {
                    DocumentID = sap.documentId,
                    ProductCode = sap.productCode,
                    ProductGuid = sap.productGUID,
                    SubscriptionType = sap.subscriptionType,
                    Publication = sap.name,
                    ProductType = sap.productType,
                    ExpirationDate = (sap.expirationDateSpecified) ? sap.expirationDate.Value : DateTime.Now
                })
                .ToList();
        }

        public string SerializeObject(object obj)
        {
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                serializer.Serialize(ms, obj);
                ms.Position = 0;
                xmlDoc.Load(ms);
                return xmlDoc.InnerXml;
            }
        }

        public static T DeserializeXMLFileToObject<T>(string XmlFile)
        {
            T returnObject = default(T);
            if (string.IsNullOrEmpty(XmlFile)) return default(T);

            try
            {
                //StreamReader xmlStream = new StreamReader(XmlFilename);
                TextReader reader = new StringReader(XmlFile);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                returnObject = (T)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {

            }
            return returnObject;
        }

    }
    [XmlType("channel")]
    public class channelxml
    {
        [XmlElement("code")]
        public string code { get; set; }
        [XmlElement("name")]
        public string name { get; set; }
        [XmlElement("expirydate")]
        public DateTime expirydate { get; set; }
        [XmlElement("subscribed")]
        public string subscribed { get; set; }
        public List<topicxml> topics { get; set; }
    }

    [XmlType("topic")]
    public class topicxml
    {
        [XmlElement("code")]
        public string code { get; set; }
        [XmlElement("name")]
        public string name { get; set; }
        [XmlElement("expirydate")]
        public DateTime expirydate { get; set; }
        
     }

    public class subscriptionsAndPurchases
    {
        [XmlElement("ArchiveCode")]
        public string ArchiveCode { get; set; }
        [XmlElement("description")]
        public string description { get; set; }
        [XmlElement("documentId")]
        public string documentId { get; set; }
        [XmlElement("expirationDate")]
        public DateTime expirationDate { get; set; }
        [XmlElement("name")]
        public string name { get; set; }
        [XmlElement("productCode")]
        public string productCode { get; set; }
        [XmlElement("productGUID")]
        public string productGUID { get; set; }
        [XmlElement("productType")]
        public string productType { get; set; }
        [XmlElement("purchaseDate")]
        public string purchaseDate { get; set; }
        [XmlElement("subscriptionType")]
        public string subscriptionType { get; set; }
        [XmlArray("channels")]
        public List<channelxml> channels { get; set; }
  }
    [XmlRoot("QuerySubscriptionsAndPurchasesResponse")]
    public class QuerySubscriptionsAndPurchasesResponse
    {
        [XmlElement("subscriptionsAndPurchases")]
        public List<subscriptionsAndPurchases> SubscriptionsAndPurchasesResponse { get; set; }
    }
}
