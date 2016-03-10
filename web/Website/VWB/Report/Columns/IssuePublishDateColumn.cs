using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Elsevier.Web.VWB.Report.Columns
{
	public class IssuePublishDateColumn : IVwbColumn
	{
		public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
		{
			return x.IssueDate.CompareTo(y.IssueDate);
		}

		public string GetHeader()
		{
			return "Issue Date";
		}

		public string Key()
		{
			return "isd";
		}

		public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
		{
			return new TableCell { Text = articleItemWrapper.IssueDate == DateTime.MinValue ? "N/E" : articleItemWrapper.IssueDateValue };
		}
	}
}