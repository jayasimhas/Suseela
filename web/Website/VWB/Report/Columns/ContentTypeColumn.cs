using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Elsevier.Library.Export.NlmExportObjects.Table;
using Elsevier.Web.VWB.Report;
using Elsevier.Web.VWB.Report.Columns;
using TableCell = System.Web.UI.WebControls.TableCell;

namespace Informa.Web.VWB.Report.Columns
{
    public class ContentTypeColumn : IVwbColumn
    {
        public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
        {
            return x.ContentType.CompareTo(y.ContentType);
        }

        public string GetHeader()
        {
            return "Content Type";
        }

        public string Key()
        {
            return "contenttype";
        }

        public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
        {
            return new TableCell { Text = articleItemWrapper.ContentType };
        }

        public Dictionary<string, string> GetDropDownValues(List<ArticleItemWrapper> results)
        {
            Dictionary<string, string> dictContentTypes = new Dictionary<string, string>();
            if (results != null)
            {
                dictContentTypes.Add("0", "Select");
                foreach (var result in results)
                {
                    if (!string.IsNullOrEmpty(result.ContentType) && !dictContentTypes.ContainsValue(result.ContentType))
                        dictContentTypes.Add(result.ContentType, result.ContentType);
                }
            }
            return dictContentTypes.OrderBy(k => k.Key).ToDictionary(m => m.Key, n => n.Value);
        }
    }
}