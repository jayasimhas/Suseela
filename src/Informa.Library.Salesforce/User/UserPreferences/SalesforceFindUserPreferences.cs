namespace Informa.Library.Salesforce.User.UserPreferences
{
    using EBIWebServices;
    using Library.User.UserPreference;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Linq;

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
                Sitecore.Context.Database.GetItem("/sitecore/media library/SalesforceXML/" + cleanUesr.Trim() + "preferences");
            QueryPreferenceResponse xmlResponse = null;

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
                        DeserializeXMLFileToObject<QueryPreferenceResponse>(innerXmdDoc.InnerXml);
                }
            }
            if (xmlResponse != null && xmlResponse.UserSelectedPreferences != null)
            {
                return new UserPreferences
                {
                    PreferredChannels = xmlResponse.UserSelectedPreferences.channels
                    .Select(ch => new Channel
                    {
                        ChannelCode = ch.code,
                        ChannelName = ch.name,
                        IsFollowing = ch.isfollowing,
                        Topics = ch.topics.Select(tp => new Topic { TopicCode = tp.code, TopicName = tp.name, IsFollowing = tp.isfollowing }).ToList()
                    }).ToList()
                };
            }

            IN_ProfilePreferencesQueryResponse preferencesResponse = null;
            try
            {
                preferencesResponse = Service.Execute(s => s.IN_queryProfilePreferences(username));
            }
            catch (Exception exp)
            {
                return null;
            }

            if (preferencesResponse == null)
            {
                return null;
            }

            if (!preferencesResponse.IsSuccess() || preferencesResponse.channelPreferences == null)
            {
                return null;
            }

            var result = JsonConvert.DeserializeObject<UserPreferences>(preferencesResponse.channelPreferences.Replace("[CDATA[", "").Replace("]]", ""));
            if (result != null)
                result.LastUpdateOn = preferencesResponse.additionalPreferences;
            return (result as IUserPreferences);
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

        IUserPreferences IFindUserPreferences.Find(string username)
        {
            return Find(username);
        }
    }

    [XmlType("channel")]
    public class channelxml
    {
        [XmlElement("code")]
        public string code { get; set; }
        [XmlElement("name")]
        public string name { get; set; }
        [XmlElement("isfollowing")]
        public bool isfollowing { get; set; }
        [XmlArray("topics")]
        public List<topicxml> topics { get; set; }
    }

    [XmlType("topic")]
    public class topicxml
    {
        [XmlElement("code")]
        public string code { get; set; }
        [XmlElement("name")]
        public string name { get; set; }
        [XmlElement("isfollowing")]
        public bool isfollowing { get; set; }
    }

    public class Preferences
    {
        [XmlArray("preferredchannels")]
        public List<channelxml> channels { get; set; }
    }
    [XmlRoot("QueryPreferenceResponse")]
    public class QueryPreferenceResponse
    {
        [XmlElement("preferences")]
        public Preferences UserSelectedPreferences { get; set; }
    }
}
