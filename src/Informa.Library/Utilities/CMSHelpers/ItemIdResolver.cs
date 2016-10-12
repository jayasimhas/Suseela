using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Sitecore.Configuration;
using Sitecore.Xml;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Informa.Library.Utilities.CMSHelpers
{

    public class ItemIdResolver
    {
        protected readonly ISitecoreContext SitecoreContext;
        protected readonly GlassVerticalRootContext VerticalRootContext;
        static ItemIdResolver itemResolver;
        public ItemIdResolver()
        {
            SitecoreContext = new SitecoreContext();
            VerticalRootContext = new GlassVerticalRootContext(SitecoreContext);
        }
        /// <summary>
        /// Method for reading Guid value from config based on key
        /// </summary>
        /// <param name="itemKey">Key</param>
        /// <returns>Value: Guid</returns>
        public static string GetItemIdByKey(string itemKey)
        {
            string itemId = string.Empty;
            itemResolver = new ItemIdResolver();
            //Get vertical name based on current context
            var vertical = itemResolver.GetVerticalName();           
            if (vertical!=null && !string.IsNullOrEmpty(vertical._Path))
            {
                string[] pathParts = vertical._Path.Split('/');
                itemId = Sitecore.Configuration.Settings.GetSetting(itemKey + "." + "global");
                if(string.IsNullOrEmpty(itemId))
                    itemId = Sitecore.Configuration.Settings.GetSetting(itemKey + "." + pathParts[3].ToLower());
            }
            else
            {
                itemId = Sitecore.Configuration.Settings.GetSetting(itemKey);
            }
            return itemId;
        }

        /// <summary>
        /// Method for fetching Vertinal name based on current context
        /// </summary>
        /// <returns>Vertical Name</returns>
        public IVertical_Root GetVerticalName()
        {
           return VerticalRootContext.Item;
        }
    }

    public sealed class SitecoreSettingResolver
    {
        private static volatile SitecoreSettingResolver instance;
        private static object syncRoot = new Object();
        public Dictionary<string, string> ItemSetting { get; set; } //new Dictionary<Guid, string>
        public string ContentRootname { get; set; }
        public string ContentRootGuid { get; set; }
        public Dictionary<string, string> PublicationPrefix { get; set; }


        private void PopulateItemSetting()
        {
            ItemSetting = new Dictionary<string, string>();
            PublicationPrefix = new Dictionary<string, string>();

            foreach (XmlNode node in Factory.GetConfigNodes("settings/setting"))
            {
                if(XmlUtil.GetAttribute("name", node).StartsWith("Content.") || XmlUtil.GetAttribute("name", node).EndsWith(".pharma"))
                    ItemSetting.Add(XmlUtil.GetAttribute("name", node), XmlUtil.GetAttribute("value", node));
            }

            foreach (XmlNode node in Factory.GetConfigNodes("settings/setting"))
            {
                if (XmlUtil.GetAttribute("name", node).StartsWith("Content.") && XmlUtil.GetAttribute("name", node).EndsWith(".Prefix"))
                    PublicationPrefix.Add(XmlUtil.GetAttribute("value", node), XmlUtil.GetAttribute("value", node));
            }

            ContentRootname = Sitecore.Configuration.Settings.GetSetting("Sitecore.ContentRoot.Name");
            ContentRootGuid = ItemSetting["Content.Guid"];
        }
        
        private SitecoreSettingResolver()
        {
            //SitecoreContext = new SitecoreContext();
            PopulateItemSetting();
        }

        public static SitecoreSettingResolver Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SitecoreSettingResolver();
                    }
                }

                return instance;
            }
        }
    }
}
