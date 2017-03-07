using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Elsevier.Web.VWB.Report.Columns
{
	public class LockStatusColumn : IVwbColumn
	{
		public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
		{
			var xlocking = x.InnerItem.InnerItem.Locking;
			var ylocking = y.InnerItem.InnerItem.Locking;
			var boolcomp = xlocking.IsLocked().CompareTo(ylocking.IsLocked());
			if (boolcomp != 0) return boolcomp;
			return xlocking.GetOwner().CompareTo(ylocking.GetOwner());
		}

		public string GetHeader()
		{
			return "Lock Status";
		}

		public string Key()
		{
			return "ls";
		}

		public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
		{
			var cell = new TableCell();
			var locking = articleItemWrapper.InnerItem.InnerItem.Locking;
			string lockUser = locking.GetOwner() ?? "";
			lockUser = lockUser.Substring(lockUser.IndexOf(@"\") + 1);

			var lockimg = new Image { ImageUrl = "/VWB/images/vwb/icon_lock.png" };
			lockimg.Attributes.Add("align", "absmiddle");
			lockimg.Attributes.Add("width", "16");
			lockimg.Attributes.Add("height", "16");
			lockimg.Attributes.Add("title", lockUser);

			cell.Controls.Add(lockimg);

			return cell;
		}

        public Dictionary<string, string> GetDropDownValues(List<ArticleItemWrapper> results)
        {
            throw new NotImplementedException();
        }
    }
}