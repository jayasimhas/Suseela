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
                        foreach (var tax in result.TaxonomyPath)
                        {
                            if (!dictTaxonomies.ContainsKey(tax.Key))
                                dictTaxonomies.Add(tax.Key, tax.Value);
                        }
                    }
                }
            return dictTaxonomies.OrderBy(k=>k.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
        } 
    }
}