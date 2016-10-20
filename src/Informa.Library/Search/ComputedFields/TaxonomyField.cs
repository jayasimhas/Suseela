namespace Informa.Library.Search.ComputedFields
{
    using Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
    using Models.Informa.Models.sitecore.templates.User_Defined.Pages;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.Data.Items;
    using System.Collections.Generic;
    public class TaxonomyField : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var taxonomyList = new List<string>();
            Item item = indexable as SitecoreIndexableItem;
            string taxonomyFieldName = I___BaseTaxonomyConstants.TaxonomiesFieldName;

            if (item != null && item.TemplateID == IArticleConstants.TemplateId && item.Fields[taxonomyFieldName] != null && !string.IsNullOrWhiteSpace(item.Fields[taxonomyFieldName].Value))
            {
                foreach (var id in item.Fields[taxonomyFieldName].Value.Split('|'))
                {
                    taxonomyList.Add(id.Replace("{", string.Empty).Replace("}", string.Empty).Replace("-", string.Empty).ToLower());
                }
            }

            return taxonomyList;
        }
    }
}
