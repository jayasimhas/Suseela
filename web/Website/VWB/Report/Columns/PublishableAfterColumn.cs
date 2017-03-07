using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Elsevier.Web.VWB.Report.Columns
{
	public class PublishableAfterColumn : IVwbColumn
	{
		public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
		{
			if (x.Embargoed && !y.Embargoed)
			{
				return 1;
			}
			else if (!x.Embargoed && y.Embargoed)
			{
				return -1;
			}
			else
			{
				// undefined sort. embargo date may be datetime.min, but we don't care.
				return x.EmbargoDate.CompareTo(y.EmbargoDate);
			}
		}

		public string GetHeader()
		{
			return "Publishable After";
		}

		public string Key()
		{
			return "pa";
		}

		public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
		{
			var tc = new TableCell();

			
			if (articleItemWrapper.Embargoed && articleItemWrapper.EmbargoDate > DateTime.Now)
			{
				var label = new Label { Text = articleItemWrapper.EmbargoDate.ToString() };
				label.Style.Add("color", "red");	
				tc.Controls.Add(label);
			}
			else if (articleItemWrapper.Embargoed && articleItemWrapper.EmbargoDate < DateTime.Now)
			{
				var label = new Label { Text = articleItemWrapper.EmbargoDate.ToString() };
				tc.Controls.Add(label);
			}
			else
			{
				tc.Text = "No publishing restriction set";
			}

			return tc;
		}

        public Dictionary<string, string> GetDropDownValues(List<ArticleItemWrapper> results)
        {
            throw new NotImplementedException();
        }
    }
}