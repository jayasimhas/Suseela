using Informa.Library.User.UserPreference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public class SalesforceContentPreferencesFactory : ISalesforceContentPreferencesFactory
    {
        private const string ContentPersonalization = "Content Filter";
        private const string ContentPersonalizationParent = "ContentPersonalizationParent";
        private const string OrderFieldFormat = "{0},{1}";
        private const char OrderSeparator = ',';
        public AddProductPreferenceRequest Create(string userName, string verticaleName,
            string publicationCode, string contentPreferences)
        {
            if (!string.IsNullOrWhiteSpace(contentPreferences))
            {
                var userPreferences = JsonConvert.DeserializeObject<UserPreferences>(contentPreferences.Replace("[CDATA[", "").Replace("]]", ""));
                if (userPreferences != null && userPreferences.PreferredChannels.Any())
                {
                    AddProductPreferenceRequest request = new AddProductPreferenceRequest();
                    request.records = new List<ProductPreferenceRequestRecord>();
                    request.records.Add(
                        new ProductPreferenceRequestRecord()
                        {
                            attributes = new AddProductPreferenceRequestAttributes()
                            {
                                type = "Product_Preference__c",
                                referenceId = Guid.NewGuid().ToString()
                            },
                            Product_Vertical__c = verticaleName,
                            Type__c = ContentPersonalization,
                            Username__c = userName,
                            Value1__c = publicationCode,
                            Value2__c = ContentPersonalizationParent,
                            Value3__c = DateTime.Now.ToString(),
                            Value4__c = userPreferences.IsChannelLevel.ToString(),
                            Value5__c = userPreferences.IsNewUser.ToString()
                        });
                    foreach (Channel channel in userPreferences.PreferredChannels)
                    {
                        if (channel.Topics != null && channel.Topics.Any())
                        {
                            foreach (Topic topic in channel.Topics)
                            {
                                request.records.Add(
                             new ProductPreferenceRequestRecord()
                             {
                                 attributes = new AddProductPreferenceRequestAttributes()
                                 {
                                     type = "Product_Preference__c",
                                     referenceId = Guid.NewGuid().ToString()
                                 },
                                 Product_Vertical__c = verticaleName,
                                 Type__c = ContentPersonalization,
                                 Username__c = userName,
                                 Value1__c = publicationCode,
                                 Value2__c = channel.ChannelCode,
                                 Value3__c = channel.ChannelId,
                                 Value4__c = channel.IsFollowing.ToString(),
                                 Value5__c = topic.TopicCode.ToString(),
                                 Value6__c = topic.TopicId.ToString(),
                                 Value7__c = topic.IsFollowing.ToString(),
                                 Value8__c = string.Format(OrderFieldFormat, channel.ChannelOrder, topic.TopicOrder)
                             });
                            }
                        }
                        else
                        {
                            request.records.Add(
                         new ProductPreferenceRequestRecord()
                         {
                             attributes = new AddProductPreferenceRequestAttributes()
                             {
                                 type = "Product_Preference__c",
                                 referenceId = Guid.NewGuid().ToString()
                             },
                             Product_Vertical__c = verticaleName,
                             Type__c = ContentPersonalization,
                             Username__c = userName,
                             Value1__c = publicationCode,
                             Value2__c = channel.ChannelCode,
                             Value3__c = channel.ChannelId,
                             Value4__c = channel.IsFollowing.ToString(),
                             Value8__c = string.Format(OrderFieldFormat, channel.ChannelOrder, "0")
                         });
                        }
                    }
                    if (request != null && request.records.Count() > 0)
                    {
                        return request;
                    }
                }
            }
            return null;
        }

        public IUserPreferences Create(ProductPreferencesResult entity)
        {
            var userPreferences = new UserPreferences();
            userPreferences.PreferredChannels = new List<Channel>();
            if (entity != null && entity.records != null && entity.records.Any())
            {
                var parentItem = entity.records.Where(record => !string.IsNullOrWhiteSpace(record.Value2__c) && record.Value2__c
                .Equals(ContentPersonalizationParent, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (parentItem != null)
                {
                    userPreferences.LastUpdateOn = parentItem.Value3__c;
                    userPreferences.IsChannelLevel = Convert.ToBoolean(parentItem.Value4__c);
                    userPreferences.IsNewUser = Convert.ToBoolean(parentItem.Value5__c);
                }
                var preferenceRecords = entity.records.Where(record => !string.IsNullOrWhiteSpace(record.Value2__c) && !record.Value2__c
                .Equals(ContentPersonalizationParent, StringComparison.OrdinalIgnoreCase)).OrderBy(record => record.Value2__c).ToList();
                var channelCode = string.Empty;
                Channel ch = null;
                foreach (Record record in preferenceRecords)
                {
                    var orders = !string.IsNullOrWhiteSpace(record.Value8__c) ?
                        record.Value8__c.Split(OrderSeparator) : null;
                    if (channelCode.Equals(record.Value2__c, StringComparison.OrdinalIgnoreCase))
                    {
                        ch.Topics.Add(
                           new Topic()
                           {
                               TopicCode = record.Value5__c,
                               TopicId = record.Value6__c,
                               IsFollowing = Convert.ToBoolean(record.Value7__c),
                               TopicOrder = orders != null && orders.Count() > 1 ? Convert.ToInt16(orders[1]) : 0
                           });
                    }
                    else
                    {
                        channelCode = record.Value2__c;
                        if (ch != null)
                        {
                            userPreferences.PreferredChannels.Add(ch);
                        }
                        ch = new Channel()
                        {
                            ChannelCode = record.Value2__c,
                            ChannelId = record.Value3__c,
                            IsFollowing = Convert.ToBoolean(record.Value4__c),
                            ChannelOrder = orders != null && orders.Count() > 0 ? Convert.ToInt16(orders[0]) : 0
                        };
                        ch.Topics = new List<Topic>();
                        ch.Topics.Add(
                            new Topic()
                            {
                                TopicCode = record.Value5__c,
                                TopicId = record.Value6__c,
                                IsFollowing = Convert.ToBoolean(record.Value7__c),
                                TopicOrder = orders != null && orders.Count() > 1 ? Convert.ToInt16(orders[1]) : 0
                            });
                    }

                }
            }
            return userPreferences;
        }


        public AddProductPreferenceRequest Create(string Username, string accessToken, string verticalname, string documentId, string documentDescription, string documentName)
        {         
            return null;
        }
    }
}
