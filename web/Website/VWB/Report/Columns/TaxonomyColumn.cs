using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Elsevier.Web.VWB.Report;
using Elsevier.Web.VWB.Report.Columns;

namespace Informa.Web.VWB.Report.Columns
{
    public class TaxonomyColumn : IVwbColumn
    {
        public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
        {
            return x.TaxonomyString.CompareTo(y.TaxonomyString);
        }

        public string GetHeader()
        {
            return "Taxonomy";
        }

        public string Key()
        {
            return "taxonomy";
        }

        public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
        {
            return new TableCell { Text = articleItemWrapper.TaxonomyString };
        }

        public Dictionary<string, string> GetDropDownValues(List<ArticleItemWrapper> results)
        {
            Dictionary<string, string> dictTaxonomies = new Dictionary<string, string>();
            if (results != null)
            {
                dictTaxonomies.Add("0", "Select");
                foreach (var result in results)
                {
                    if (result != null && !string.IsNullOrEmpty(result.TaxonomyString))
                    {
                        var taxonomies = result.TaxonomyString.Split(',').ToArray();
                        foreach (var tax in taxonomies)
                        {
                            if (!dictTaxonomies.ContainsValue(tax))
                                dictTaxonomies.Add(tax, tax);
                        }
                    }
                }
            }
            return dictTaxonomies;
        }
    }
}