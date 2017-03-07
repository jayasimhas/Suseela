using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Elsevier.Web.VWB.Report.Columns
{
	public class WordCountColumn : IVwbColumn
	{
		public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
		{
            return x.WordCount.CompareTo(y.WordCount);
		}

		public string GetHeader()
		{
			return "Word Count";
		}

		public string Key()
		{
			return "asz";
		}

		public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
		{
            return new TableCell { Text = articleItemWrapper.WordCount.ToString() };
        }

        public Dictionary<string, string> GetDropDownValues(List<ArticleItemWrapper> results)
        {
            throw new NotImplementedException();
        }
    }
}