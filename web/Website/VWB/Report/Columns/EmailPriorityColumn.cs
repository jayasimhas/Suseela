using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Elsevier.Web.VWB.Report;
using Elsevier.Web.VWB.Report.Columns;

namespace Informa.Web.VWB.Report.Columns
{
    public class EmailPriorityColumn : IVwbColumn
    {
        public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
        {
            return x.EmailPriority.CompareTo(y.EmailPriority);
        }

        public string GetHeader()
        {
            return "Email Priority";
        }

        public string Key()
        {
            return "emailpriority";
        }

        public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
        {
            return new TableCell { Text = articleItemWrapper.EmailPriority };
        }

        public Dictionary<string, string> GetDropDownValues(List<ArticleItemWrapper> results)
        {
            throw new NotImplementedException();
        }
    }
}