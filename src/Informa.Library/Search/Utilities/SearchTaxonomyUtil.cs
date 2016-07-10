using System.Collections.Generic;
using System.Text;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Sitecore.Configuration;
using Velir.Search.Core.Reference;

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
			var dict = new Dictionary<string,string>();
			foreach (var item in taxonomyItems)
			{
				string parentPath = item._Parent._Path;
				string key = string.Empty;

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
				if (IsIndustryTaxonomy(parentPath))
				{
					key = Industries;
				}

				//Device Area
				if (IsDeviceAreaTaxonomy(parentPath))
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

			url.Append("/search#?");

			foreach (var pair in dict)
			{
				url.AppendFormat("{0}={1}", pair.Key, pair.Value);
			}
			
			return url.ToString();
		}

		public static bool IsSubjectTaxonomy(string itemPath)
		{
			return itemPath.ToLower()
					.StartsWith(Settings.GetSetting("Taxonomy.SubjectPath"));
		}
		public static bool IsRegionTaxonomy(string itemPath)
		{
			return itemPath.ToLower()
					.StartsWith(Settings.GetSetting("Taxonomy.RegionPath"));
		}
		public static bool IsAreaTaxonomy(string itemPath)
		{
			return itemPath.ToLower()
					.StartsWith(Settings.GetSetting("Taxonomy.AreaPath"));
		}

		public static bool IsIndustryTaxonomy(string itemPath)
		{
			return itemPath.ToLower()
					.StartsWith(Settings.GetSetting("Taxonomy.IndustryPath"));
		}

		public static bool IsDeviceAreaTaxonomy(string itemPath)
		{
			return itemPath.ToLower()
					.StartsWith(Settings.GetSetting("Taxonomy.DeviceAreaPath"));
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
