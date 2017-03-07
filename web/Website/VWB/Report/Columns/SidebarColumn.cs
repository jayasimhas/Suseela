using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Elsevier.Web.VWB.Report.Columns
{
	public class SidebarColumn : IVwbColumn
	{
		public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
		{
			return y.SidebarArticleNumbers.Count.CompareTo(x.SidebarArticleNumbers.Count);
		}

		public string GetHeader()
		{
			return "Sidebar(s)";
		}

		public string Key()
		{
			return "sbc";
		}

		public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
		{
			string value = string.Join("<br />", articleItemWrapper.SidebarArticleNumbers.ToArray());
			return new TableCell { Text = value };
		}

        public Dictionary<string, string> GetDropDownValues(List<ArticleItemWrapper> results)
        {
            throw new NotImplementedException();
        }
    }
}