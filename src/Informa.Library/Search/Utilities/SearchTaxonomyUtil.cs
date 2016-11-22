using System.Collections.Generic;
using System.Text;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Sitecore.Configuration;
using Velir.Search.Core.Reference;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Data;
using System.Linq;

namespace Informa.Library.Search.Utilities
{
	public class SearchTaxonomyUtil
	{
		private const string Subjects = "subjects";
		private const string Regions = "regions";
		private const string Areas = "areas";
		private const string Industries = "industries";
		private const string DeviceAreas = "deviceareas";
        private const string Commercial = "commercial";
        private const string Commodities = "commodities";
        private const string CommodityFactors = "commodityfactors";
        private const string Companies = "companies";

        public static string GetSearchUrl(params ITaxonomy_Item[] taxonomyItems)
		{
            Item rootItem = null;
            Item currentItem = null;
            if (taxonomyItems.Length > 0)
            {
                var dbContext = Sitecore.Configuration.Factory.GetDatabase("master");
                currentItem = dbContext.GetItem(new ID(taxonomyItems[0]._Id));
               
                if(currentItem != null)
                    rootItem = currentItem.Axes.GetAncestors().FirstOrDefault(ancestor => ancestor.TemplateID.ToString() == Settings.GetSetting("VerticalTemplate.global"));
            }
            var dict = new Dictionary<string,string>();
			foreach (var item in taxonomyItems)
			{
                string parentPath = item._Parent._Path;
				string key = string.Empty;
                string verticalName = (rootItem != null) ? rootItem.Name : string.Empty;

				//Subject
				if (IsSubjectTaxonomy(parentPath))
				{
					key = Subjects;
				}

				//Region
				if (IsRegionTaxonomy(parentPath))
				{
					key = Regions;
				}

				//Area
				if (IsAreaTaxonomy(parentPath))
				{
					key = Areas;
				}

				//Industry
				if (IsIndustryTaxonomy(parentPath, verticalName))
				{
					key = Industries;
				}

				//Device Area
				if (IsDeviceAreaTaxonomy(parentPath))
				{
					key = DeviceAreas;
				}

                if(IsCommercialTaxonomy(parentPath))
                {
                    key = Commercial;
                }

                if (IsCommodityTaxonomy(parentPath))
                {
                    key = Commodities;
                }

                if (IsCommodityFactorsTaxonomy(parentPath))
                {
                    key = CommodityFactors;
                }

                if (IsCompaniesTaxonomy(parentPath))
                {
                    key = Companies;
                }

                if (dict.ContainsKey(key))
				{
					dict[key] = $"{dict[key]}{SiteSettings.ValueSeparator}{item.Item_Name}";
				}
				else
				{
					dict[key] = item.Item_Name;
				}
			}

			StringBuilder url = new StringBuilder();
            foreach (var pair in dict)
			{
				url.AppendFormat("{0}={1}", pair.Key, HttpUtility.UrlEncode(pair.Value));
			}
			
			return $"/search#?{url.ToString()}";
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
            List<string> fullTaxonomyList =new List<string>();

            foreach (ITaxonomy_Item taxonomyItem in taxonomyItems)
            {
                fullTaxonomyList.Add(taxonomyItem.Item_Name.Trim());

                if (taxonomyItem._Parent._TemplateId == ITaxonomy_ItemConstants.TemplateId.ToGuid())
                {
                    string facetValue = ((ITaxonomy_Item) taxonomyItem._Parent).Item_Name.Trim();

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
