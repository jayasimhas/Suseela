using System.Collections.Generic;
using Informa.Library.Search.Utilities;
using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Items;
using Sitecore.Configuration;
using Sitecore.Data;
using System.Linq;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;

namespace Informa.Library.Search.ComputedFields.Facets
{
    public class CompaniesField : BaseGlassComputedField<IArticle>
    {
        public override object GetFieldValue(IArticle indexItem)
        {
            Item rootItem = null;
            var dbContext = Factory.GetDatabase("master");
            Item item = dbContext.GetItem(new ID(indexItem._Id));

            if (item != null)
            {
                rootItem = item.Axes.GetAncestors().FirstOrDefault(ancestor => ancestor.TemplateID.ToString() == Settings.GetSetting("VerticalTemplate.global"));
           
                if (rootItem != null && rootItem.Name.Contains("Agri") || rootItem.Name.Contains("Maritime"))
                {
                    if (indexItem?.Taxonomies != null)
                    {
                        var companiesTaxonomy = indexItem.Taxonomies.Where(x => SearchTaxonomyUtil.IsCompaniesTaxonomy(x._Path, rootItem.Name));
                        var companysearchtaxonomy = companiesTaxonomy.Where(x => !string.IsNullOrEmpty(x.Item_Name)).Select(x => x.Item_Name.Trim()).ToList();
                        return companysearchtaxonomy;
                    }
                }
                else if (string.IsNullOrEmpty(indexItem.Referenced_Companies))
                {
                    return new List<string>();
                }
                else if (rootItem.Name.Contains("Pharma"))
                {
                    return SearchCompanyUtil.GetCompanyNames(indexItem.Referenced_Companies);
                }
            }           
                return new List<string>();
           
        }
    }

    //public class CompaniesField : BaseGlassComputedField<I___BaseTaxonomy>
    //{
    //    public override object GetFieldValue(I___BaseTaxonomy indexItem)
    //    {
    //        Item rootItem = null;
    //        var dbContext = Factory.GetDatabase("master");
    //        Item item = dbContext.GetItem(new ID(indexItem._Id));

    //        if (item != null)
    //            rootItem = item.Axes.GetAncestors().FirstOrDefault(ancestor => ancestor.TemplateID.ToString() == Settings.GetSetting("VerticalTemplate.global"));
    //        if (rootItem != null && rootItem.Name.Contains("Agri") || rootItem.Name.Contains("Maritime"))
    //        {
    //            if (indexItem?.Taxonomies != null)
    //            {
    //                var companyTaxonomyItems = indexItem.Taxonomies.Where(x => SearchTaxonomyUtil.IsCompaniesTaxonomy(x._Path));
    //                return SearchTaxonomyUtil.GetHierarchicalFacetFieldValue(companyTaxonomyItems);
    //            }
    //        }
    //        return new List<string>();

    //    }
    //}

}
