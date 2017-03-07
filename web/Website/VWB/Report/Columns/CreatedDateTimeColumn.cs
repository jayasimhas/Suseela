using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Elsevier.Library.Reference;
using Elsevier.Web.VWB.Util;

namespace Elsevier.Web.VWB.Report.Columns
{
	public class CreatedDateTimeColumn : IVwbColumn
	{
		public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
		{
			return x.ArticleCreation.CompareTo(y.ArticleCreation);
		}

		public string GetHeader()
		{
			return "Created Date";
		}

		public string Key()
		{
			return "acdt";
		}

		public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
		{
			string text;
			if (articleItemWrapper.ArticleCreation == DateTime.MinValue)
			{
				text = "N/E";
			}
			else
			{
				text =  VWBUtil.GetFormattedDateTime(articleItemWrapper.ArticleCreation);
			}
			return new TableCell {Text = text};
		}

        public Dictionary<string, string> GetDropDownValues(List<ArticleItemWrapper> results)
        {
            throw new NotImplementedException();
        }
    }
}