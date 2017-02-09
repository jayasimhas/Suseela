using System.Collections.Generic;
using System.Text;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Sitecore.Configuration;
using Velir.Search.Core.Reference;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Data;
using System.Linq;
using System;
using Informa.Library.Utilities.CMSHelpers;
using Glass.Mapper.Sc;

namespace Informa.Library.Search.Utilities
{
    public class SearchTaxonomyUtil
    {
        //Global
        private const string Regions = "regions";
        //Agri
        private const string AgencyRegulator = "agencyRegulator";
        private const string AnimalHealth = "animalhealth";
        private const string Commercial = "commercial";
        private const string Commodities = "commodities";
        private const string CommodityFactors = "commodityfactors";
        private const string Companies = "companies";
        private const string CropProtection = "cropprotection";
        private const string AgriIndustries = "agriindustries";
        //Maritime        
        private const string MaritimeCompanies = "maritimecompanies";
        private const string HotPicks = "hotpicks";
        private const string Markets = "markets";
        private const string Regulars = "regulars";
        private const string Sectors = "sectors";
        private const string Topic = "topic";
        //pharma
        private const string Subjects = "subjects";
        private const string TherapyAreas = "therapyareas";
        private const string DeviceAreas = "deviceareas";
        private const string PharmaIndustries = "pharmaindustries";


        public static string GetSearchUrl(params ITaxonomy_Item[] taxonomyItems)
        {
            Item rootItem = null;
            Item currentItem = null;
            if (taxonomyItems.Length == 0)
                return "";
            if (taxonomyItems.Length > 0)
            {
                var dbContext = Factory.GetDatabase("master");
                currentItem = dbContext.GetItem(new ID(taxonomyItems[0]._Id));

                if (currentItem != null)
                    rootItem = currentItem.Axes.GetAncestors().FirstOrDefault(ancestor => ancestor.TemplateID.ToString() == Settings.GetSetting("VerticalTemplate.global"));
            }
            var dict = new Dictionary<string, string>();
            if (rootItem != null)
            {
                foreach (var item in taxonomyItems)
                {
                    string parentPath = item._Parent._Path;
                    string key = string.Empty;
                    string verticalName = (rootItem != null) ? rootItem.Name : string.Empty;

                    if (string.Equals(rootItem.Name, "Agri", StringComparison.OrdinalIgnoreCase))
                    {
                        if (IsValidTaxonomy("RegionsTaxonomyFolder", parentPath))
                        {
                            key = Regions;
                        }
                        else if (IsValidTaxonomy("AgencyRegulatorTaxonomyFolder", parentPath))
                        {
                            key = AgencyRegulator;
                        }
                        else if (IsValidTaxonomy("AnimalHealthTaxonomyFolder", parentPath))
                        {
                            key = AnimalHealth;
                        }
                        else if (IsValidTaxonomy("CommercialTaxonomyFolder", parentPath))
                        {
                            key = Commercial;
                        }
                        else if (IsValidTaxonomy("CommodityTaxonomyFolder", parentPath))
                        {
                            key = Commodities;
                        }
                        else if (IsValidTaxonomy("CommodityFactorsTaxonomyFolder", parentPath))
                        {
                            key = CommodityFactors;
                        }
                        else if (IsValidTaxonomy("CompaniesTaxonomyFolder", parentPath))
                        {
                            key = Companies;
                        }
                        else if (IsValidTaxonomy("CropProtectionaxonomyFolder", parentPath))
                        {
                            key = CropProtection;
                        }
                        else if (IsValidTaxonomy("IndustriesTaxonomyFolder", parentPath))
                        {
                            key = AgriIndustries;
                        }
                    }
                    else if (string.Equals(rootItem.Name, "Maritime", StringComparison.OrdinalIgnoreCase))
                    {
                        if (IsValidTaxonomy("RegionsTaxonomyFolder", parentPath))
                        {
                            key = Regions;
                        }
                        else if (IsValidTaxonomy("CompaniesTaxonomyFolder", parentPath))
                        {
                            key = MaritimeCompanies;
                        }
                        else if (IsValidTaxonomy("HotTopicsTaxonomyFolder", parentPath))
                        {
                            key = HotPicks;
                        }
                        else if (IsValidTaxonomy("MarketsTaxonomyFolder", parentPath))
                        {
                            key = Markets;
                        }
                        else if (IsValidTaxonomy("RegularsTaxonomyFolder", parentPath))
                        {
                            key = Regulars;
                        }
                        else if (IsValidTaxonomy("SectorsTaxonomyFolder", parentPath))
                        {
                            key = Sectors;
                        }
                        else if (IsValidTaxonomy("TopicTaxonomyFolder", parentPath))
                        {
                            key = Topic;
                        }
                    }
                    else if (string.Equals(rootItem.Name, "Pharma", StringComparison.OrdinalIgnoreCase))
                    {

                        if (IsValidTaxonomy("SubjectsTaxonomyFolder", parentPath))
                        {
                            key = Subjects;
                        }
                        else if (IsValidTaxonomy("TherapyAreasTaxonomyFolder", parentPath))
                        {
                            key = TherapyAreas;
                        }
                        else if (IsValidTaxonomy("DeviceAreasTaxonomyFolder", parentPath))
                        {
                            key = DeviceAreas;
                        }
                        else if (IsValidTaxonomy("IndustryTaxonomyFolder", parentPath))
                        {
                            key = PharmaIndustries;
                        }
                    }
                    #region Old code
                    ////Subject
                    //if (IsSubjectTaxonomy(parentPath))
                    //{
                    //    key = Subjects;
                    //}

                    ////Region
                    //if (IsRegionTaxonomy(parentPath))
                    //{
                    //    key = Regions;
                    //}

                    ////Area
                    //if (IsAreaTaxonomy(parentPath))
                    //{
                    //    key = Areas;
                    //}

                    ////Industry
                    //if (IsIndustryTaxonomy(parentPath, verticalName))
                    //{
                    //    key = Industries;
                    //}

                    ////Device Area
                    //if (IsDeviceAreaTaxonomy(parentPath))
                    //{
                    //    key = DeviceAreas;
                    //}

                    //if (IsCommercialTaxonomy(parentPath))
                    //{
                    //    key = Commercial;
                    //}

                    //if (IsCommodityTaxonomy(parentPath))
                    //{
                    //    key = Commodities;
                    //}

                    //if (IsCommodityFactorsTaxonomy(parentPath))
                    //{
                    //    key = CommodityFactors;
                    //}

                    //if (IsCompaniesTaxonomy(parentPath))
                    //{
                    //    key = Companies;
                    //}

                    ////Agency regulator
                    //if (IsAgencyRegulatorTaxonomy(parentPath))
                    //{
                    //    key = AgencyRegulator;
                    //}
                    #endregion

                    if (!string.IsNullOrEmpty(key))
                    {
                        if (dict.ContainsKey(key))
                        {
                            dict[key] = $"{dict[key]}{SiteSettings.ValueSeparator}{HttpUtility.UrlEncode(item.Item_Name.Replace("\r", ""))}";
                        }
                        else
                        {
                            dict[key] = HttpUtility.UrlEncode(item.Item_Name.Replace("\r", ""));
                        }
                    }
                }
            }

            StringBuilder url = new StringBuilder();
            foreach (var pair in dict)
            {
                url.AppendFormat("{0}={1}&", pair.Key, pair.Value);
            }
            url = !string.IsNullOrEmpty(url.ToString()) ? url.Remove(url.Length - 1, 1) : url;

            return $"/search#?{url.ToString()}";
        }

        public static bool IsValidTaxonomy(string taxonomyKey, string itemPath)
        {
            try
            {
                return itemPath.ToLower()
                    .StartsWith(Sitecore.Context.Database.GetItem
                    (new ID(new Guid(ItemIdResolver.GetItemIdByKey(taxonomyKey)))).Paths.Path.ToString().ToLower());
            }
            catch (Exception e)
            {
                return false;
            }

        }
        public static bool IsSubjectTaxonomy(string itemPath)
        {
            return itemPath.ToLower()
                    .StartsWith(Settings.GetSetting("Taxonomy.SubjectPath").ToLower());
        }
        public static bool IsRegionTaxonomy(string itemPath)
        {
            return itemPath.ToLower()
                    .StartsWith(Settings.GetSetting("Taxonomy.RegionPath"));
        }
        public static bool IsAreaTaxonomy(string itemPath)
        {
            return itemPath.ToLower()
                    .StartsWith(Settings.GetSetting("Taxonomy.AreaPath").ToLower());
        }

        public static bool IsIndustryTaxonomy(string itemPath, string verticalName)
        {
            return itemPath.ToLower()
                    .StartsWith(string.Format(Settings.GetSetting("Taxonomy.IndustryPath"), verticalName).ToLower());
        }

        public static bool IsDeviceAreaTaxonomy(string itemPath)
        {
            return itemPath.ToLower()
                    .StartsWith(Settings.GetSetting("Taxonomy.DeviceAreaPath").ToLower());
        }

        public static bool IsCommercialTaxonomy(string itemPath)
        {
            return itemPath.ToLower()
                    .StartsWith(Settings.GetSetting("Taxonomy.Commercial").ToLower());
        }

        public static bool IsCommodityTaxonomy(string itemPath)
        {
            return itemPath.ToLower().StartsWith(Settings.GetSetting("Taxonomy.Commodity").ToLower());
        }

        public static bool IsCommodityFactorsTaxonomy(string itemPath)
        {
            return itemPath.ToLower().StartsWith(Settings.GetSetting("Taxonomy.CommodityFactors").ToLower());
        }

        public static bool IsCompaniesTaxonomy(string itemPath)
        {
            return itemPath.ToLower().StartsWith(Settings.GetSetting("Taxonomy.Companies").ToLower());
        }

        public static bool IsAgencyRegulatorTaxonomy(string itemPath)
        {
            return itemPath.ToLower().StartsWith(Settings.GetSetting("Taxonomy.AgencyRegulator").ToLower());
        }

        public static IEnumerable<string> GetHierarchicalFacetFieldValue(IEnumerable<ITaxonomy_Item> taxonomyItems)
        {
            List<string> fullTaxonomyList = new List<string>();

            foreach (ITaxonomy_Item taxonomyItem in taxonomyItems)
            {
                fullTaxonomyList.Add(taxonomyItem.Item_Name.Trim());

                if (taxonomyItem._Parent._TemplateId == ITaxonomy_ItemConstants.TemplateId.ToGuid())
                {
                    string facetValue = ((ITaxonomy_Item)taxonomyItem._Parent).Item_Name.Trim();

                    if (string.IsNullOrEmpty(facetValue))
                    {
                        continue;
                    }

                    fullTaxonomyList.Add(facetValue);
                }
            }

            return fullTaxonomyList;
        }
    }
}
