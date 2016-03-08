using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Elsevier.Web.VWB.Report.Columns
{
	public class AuthorsColumn : IVwbColumn
	{
		public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
		{
			string a = string.Join("", x.Authors.ToArray());
			string b = string.Join("", y.Authors.ToArray());
			return a.CompareTo(b);
		}

		public string GetHeader()
		{
			return "Author(s)";
		}

		public string Key()
		{
			return "ath";
		}

		public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
		{
		    string authors = "";
		    if (articleItemWrapper.Authors != null)
		    {
                authors = string.Join("<br />", articleItemWrapper.Authors.ToArray());

            }
		
			return new TableCell {Text = authors };
		}
	}
}