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

		public static string GetSearchUrl(params ITaxonomy_Item[] taxonomyItems)
		{
            Item rootItem = null;
            Item currentItem = null;
            if (taxonomyItems.Length > 0)
            {
                if (Sitecore.Context.ContentDatabase != null)
                {
                    currentItem = Sitecore.Context.ContentDatabase.GetItem(new ID(taxonomyItems[0]._Id));
                }
                else if(Sitecore.Context.Database != null)
                {
                    currentItem = Sitecore.Context.Database.GetItem(new ID(taxonomyItems[0]._Id));
                }
                rootItem = currentItem.Axes.GetAncestors().FirstOrDefault(ancestor => ancestor.TemplateID.ToString() == "{DE3615F6-1562-4CB4-80EA-7FA45F49B7B7}");
            }
            var dict = new Dictionary<string,string>();
			foreach (var item in taxonomyItems)
			{
                string parentPath = item._Parent._Path;
				string key = string.Empty;
                string verticalName = (rootItem != null) ? rootItem.Name : string.Empty;

				//Subject
				if (IsSubjectTaxonomy(parentPath, verticalName))
				{
					key = Subjects;
				}

				//Region
				if (IsRegionTaxonomy(parentPath))
				{
					key = Regions;
				}

				//Area
				if (IsAreaTaxonomy(parentPath, verticalName))
				{
					key = Areas;
				}

				//Industry
				if (IsIndustryTaxonomy(parentPath, verticalName))
				{
					key = Industries;
				}

				//Device Area
				if (IsDeviceAreaTaxonomy(parentPath, verticalName))
				{
					key = DeviceAreas;
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
				url.AppendFormat("{0}={1}", pair.Key, pair.Value);
			}
			
			return $"/search#?{url.ToString()}";
		}

		public static bool IsSubjectTaxonomy(string itemPath, string verticlaName)
		{
			return itemPath.ToLower()
					.StartsWith(string.Format(Settings.GetSetting("Taxonomy.SubjectPath"), verticlaName).ToLower());
		}
		public static bool IsRegionTaxonomy(string itemPath)
		{
			return itemPath.ToLower()
					.StartsWith(Settings.GetSetting("Taxonomy.RegionPath"));
		}
		public static bool IsAreaTaxonomy(string itemPath, string verticalName)
		{
			return itemPath.ToLower()
					.StartsWith(string.Format(Settings.GetSetting("Taxonomy.AreaPath"), verticalName).ToLower());
		}

		public static bool IsIndustryTaxonomy(string itemPath, string verticalName)
		{
			return itemPath.ToLower()
					.StartsWith(string.Format(Settings.GetSetting("Taxonomy.IndustryPath"), verticalName).ToLower());
		}

		public static bool IsDeviceAreaTaxonomy(string itemPath, string verticalName)
		{
			return itemPath.ToLower()
					.StartsWith(string.Format(Settings.GetSetting("Taxonomy.DeviceAreaPath"), verticalName).ToLower());
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
