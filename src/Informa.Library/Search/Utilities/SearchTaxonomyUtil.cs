﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Sitecore.Configuration;

namespace Informa.Library.Search.Utilities
{
    public class SearchTaxonomyUtil
    {
        public static string GetSearchUrl(ITaxonomy_Item taxonomyItem)
        {
            StringBuilder url = new StringBuilder();

            url.Append("/search#?");

            string parentPath = taxonomyItem._Parent._Path;

            //Subject
            if (IsSubjectTaxonomy(parentPath))
            {
                url.AppendFormat("subjects={0}", taxonomyItem._Name);
            }

            //Region
            if (IsRegionTaxonomy(parentPath))
            {
                url.AppendFormat("regions={0}", taxonomyItem._Name);
            }

            //Area
            if (IsAreaTaxonomy(parentPath))
            {
                url.AppendFormat("areas={0}", taxonomyItem._Name);
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

        public static IEnumerable<string> GetHierarchicalFacetFieldValue(IEnumerable<ITaxonomy_Item> taxonomyItems)
        {
            List<string> fullTaxonomyList =new List<string>();

            foreach (ITaxonomy_Item taxonomyItem in taxonomyItems)
            {
                fullTaxonomyList.Add(taxonomyItem.Item_Name.Trim());

                if (taxonomyItem._Parent._TemplateId == ITaxonomy_ItemConstants.TemplateId.ToGuid())
                {
                    fullTaxonomyList.Add(((ITaxonomy_Item)taxonomyItem._Parent).Item_Name);
                }
            }

            return fullTaxonomyList;
        }
    }
}
