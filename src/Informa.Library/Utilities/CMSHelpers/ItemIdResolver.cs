using Glass.Mapper.Sc;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Library.Utilities.References;
using Jabberwocky.Core.Caching;
using Jabberwocky.Glass.Autofac.Util;
using Jabberwocky.Glass.Services;
using Sitecore.Configuration;
using Sitecore.Xml;
using System;
using System.Collections.Generic;
using System.Xml;
using Autofac;

namespace Informa.Library.Utilities.CMSHelpers
{

    public class ItemIdResolver
    {       
        /// <summary>
        /// Method for reading Guid value from config based on key
        /// </summary>
        /// <param name="itemKey">Key</param>
        /// <returns>Value: Guid</returns>
        public static string GetItemIdByKey(string itemKey)
        {
            string itemId = string.Empty;
            string vertical = string.Empty;
            using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
            {
                var reader = scope.Resolve<IVerticalRootContext>();
                vertical = reader.Item.Vertical_Name;
            }
            if (!string.IsNullOrEmpty(vertical))
            {
                itemId = Sitecore.Configuration.Settings.GetSetting(itemKey + "." + "global");
                if (string.IsNullOrEmpty(itemId))
                    itemId = Sitecore.Configuration.Settings.GetSetting(itemKey + "." + vertical.ToLower());
            }
            else
            {
                itemId = Sitecore.Configuration.Settings.GetSetting(itemKey);
            }
            return itemId;
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
                if (XmlUtil.GetAttribute("name", node).StartsWith("Content.") || XmlUtil.GetAttribute("name", node).EndsWith(".pharma"))
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
