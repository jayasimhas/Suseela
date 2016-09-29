using System;
using System.Web.UI.WebControls;
using Elsevier.Web.VWB.Report;
using Elsevier.Web.VWB.Report.Columns;
using Informa.Library.Utilities.Extensions;

namespace Informa.Web.VWB.Report.Columns
{
    public class ArticleCheckboxes : IVwbColumn
	{
		#region IVwbColumn Members

		public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
		{
			return string.Compare(x.ArticleNumber, y.ArticleNumber, StringComparison.Ordinal);
		}

		public string GetHeader()
		{
			return "";
		}

		string IVwbColumn.Key()
		{
			return Key();
		}

		public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
		{
		    return new TableCell().Alter(tc =>
		        tc.Controls.Add(new CheckBox()
		            .Alter(cb => cb.CssClass = "js-article-checkbox")
		            .Alter(cb => cb.InputAttributes["value"] = articleItemWrapper.InnerItem.SitecoreId)));

		    //   checkbox.InputAttributes["value"] = articleItemWrapper.InnerItem.SitecoreId;

		    //tc.Controls.Add(checkbox);

		    //return tc;
		}

		public static string Key()
		{
			return "cb";
		}

		#endregion
	}
}