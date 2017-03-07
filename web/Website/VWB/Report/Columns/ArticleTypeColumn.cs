using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Elsevier.Web.VWB.Report.Columns
{
	public class ArticleTypeColumn : IVwbColumn
	{
		public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
		{
			throw new NotImplementedException();
		}

		public string GetHeader()
		{
			return "Article Type";
		}

		public string Key()
		{
			return "aty";
		}

		public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
		{
			throw new NotImplementedException();
		}

        public Dictionary<string, string> GetDropDownValues(List<ArticleItemWrapper> results)
        {
            throw new NotImplementedException();
        }
    }
}