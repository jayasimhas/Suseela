using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Elsevier.Web.VWB.Report;
using Elsevier.Web.VWB.Report.Columns;
using System.Linq;

namespace Informa.Web.VWB.Report.Columns
{
    public class MediaTypeColumn : IVwbColumn
    {
        public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
        {
            if (x.MediaType == null || y.MediaType == null)
            {
                return 0;
            }

            return x.MediaType.CompareTo(y.MediaType);
        }

        public string GetHeader()
        {
            return "Media Type";
        }

        public string Key()
        {
            return "mediatype";
        }

        public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
        {
            return new TableCell { Text = articleItemWrapper.MediaType };
        }

        public Dictionary<string, string> GetDropDownValues(List<ArticleItemWrapper> results)
        {
            Dictionary<string, string> dictMediaTypes = new Dictionary<string, string>();
            if (results != null)
            {
                dictMediaTypes.Add("0", "Select");
                foreach (var result in results)
                {
                    if (!string.IsNullOrEmpty(result.MediaType) && !dictMediaTypes.ContainsValue(result.MediaType))
                        dictMediaTypes.Add(result.MediaType, result.MediaType);
                }
            }
            return dictMediaTypes.OrderBy(k => k.Key).ToDictionary(m => m.Key, n => n.Value);
        }
    }
}