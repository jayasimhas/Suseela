using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Elsevier.Web.VWB.Report.Columns
{
	public class ArticleLengthEstimateColumn : IVwbColumn
	{
		public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
		{
			return x.ArticleLengthEstimate.CompareTo(y.ArticleLengthEstimate);
		}

		public string GetHeader()
		{
			return "Estimated Page Length";
		}

		public string Key()
		{
			return "epl";
		}

		public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
		{
			var tc = new TableCell();
			tc.Text = articleItemWrapper.ArticleLengthEstimate;
			return tc;
		}

        public Dictionary<string, string> GetDropDownValues(List<ArticleItemWrapper> results)
        {
            throw new NotImplementedException();
        }
    }
}