using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Elsevier.Web.VWB.Report.Columns
{
	public class ArticleSizeColumn : IVwbColumn
	{
		public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
		{
			if (x == y)
			{
				return 0;
			}
			if(x.ArticleSize == null)
			{
				return -1;
			}
			if (y.ArticleSize == null)
			{
				return 1;
			}
			return x.ArticleSize.CompareTo(y.ArticleSize);
		}

		public string GetHeader()
		{
			return "Article Size";
		}

		public string Key()
		{
			return "asz";
		}

		public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
		{
			string text = string.IsNullOrEmpty(articleItemWrapper.ArticleSize) ? ArticleItemWrapper.NE : articleItemWrapper.ArticleSize;
			return new TableCell {Text = text};
		}
	}
}