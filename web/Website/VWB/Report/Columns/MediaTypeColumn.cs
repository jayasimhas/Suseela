using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Elsevier.Web.VWB.Report;
using Elsevier.Web.VWB.Report.Columns;

namespace Informa.Web.VWB.Report.Columns
{
    public class MediaTypeColumn : IVwbColumn
    {
        public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
        {
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
    }
}