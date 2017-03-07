using Elsevier.Web.VWB.Report.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Elsevier.Web.VWB.Report;
using System.Web.UI.WebControls;

namespace Informa.Web.VWB.Report.Columns
{
    public class ArticlePublicationNameColumn : IVwbColumn
    {
        public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
        {
            return x.PublicationName.ToLower().CompareTo(y.PublicationName.ToLower());
        }

        public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
        {
            return new TableCell { Text = articleItemWrapper.PublicationName };
        }

        public Dictionary<string, string> GetDropDownValues(List<ArticleItemWrapper> results)
        {
            throw new NotImplementedException();
        }

        public string GetHeader()
        {
            return "Publication Name";
        }

        public string Key()
        {
            return "publicationname";
        }
    }
}