namespace Informa.Library.Salesforce.User.UserPreferences
{
    using EBIWebServices;
    using Library.User.UserPreference;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;

    public class SalesforceFindUserPreferences : ISalesforceFindUserPreferences, IFindUserPreferences
    {
        protected readonly ISalesforceServiceContext Service;

        public SalesforceFindUserPreferences(ISalesforceServiceContext service)
        {
            Service = service;
        }
        public IUserPreferences Find(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return null;
            }
            string cleanUesr = username;
            cleanUesr = cleanUesr.Replace("@example.com", "");
            Sitecore.Data.Items.Item SalesForceXmlItem =
                Sitecore.Context.Database.GetItem("/sitecore/media library/SalesforceXML/" + cleanUesr.Trim()+"preferences");
            QueryUserSubscriptions xmlResponse = null;

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
                        DeserializeXMLFileToObject<QueryUserSubscriptions>(innerXmdDoc.InnerXml);
                }
            }
            if (xmlResponse != null && xmlResponse.UserSelectedPreferences != null)
            {

                return new UserPreferences
                {
                    PreferredChannels = xmlResponse.UserSelectedPreferences.channels.Select(ch => new Channel { ChannelCode = ch.code, ChannelName = ch.name }).ToList(),
                    PreferredTopics = xmlResponse.UserSelectedPreferences.topics.Select(tp => new Topic { TopicCode = tp.code, TopicName = tp.name }).ToList()
                };
            }

            var preferencesResponse = Service.Execute(s => s.IN_queryProfilePreferences(username));

            if (!preferencesResponse.IsSuccess() || preferencesResponse.channelPreferences == null)
            {
                return null;
            }

            var result = JsonConvert.DeserializeObject<UserPreferences>(preferencesResponse.channelPreferences.Replace("[CDATA[", "").Replace("]]", ""));
            return (result as IUserPreferences);
        }

        IUserPreferences IFindUserPreferences.Find(string username)
        {
            return Find(username);
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
    }

    [XmlType("topic")]
    public class topicxml
    {
        [XmlElement("code")]
        public string code { get; set; }
        [XmlElement("name")]
        public string name { get; set; }       
    }

    public class SelectedPreferences
    {
        [XmlArray("preferredchannels")]
        public List<channelxml> channels { get; set; }
        [XmlArray("preferredtopics")]
        public List<topicxml> topics { get; set; }
       
    }
    [XmlRoot("QueryPreferenceResponse")]
    public class QueryUserSubscriptions
    {
        [XmlElement("preferences")]
        public SelectedPreferences UserSelectedPreferences { get; set; }
    }
}
