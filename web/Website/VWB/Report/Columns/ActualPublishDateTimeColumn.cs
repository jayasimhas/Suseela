using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Elsevier.Web.VWB.Util;

namespace Elsevier.Web.VWB.Report.Columns
{
	public class ActualPublishDateTimeColumn : IVwbColumn
	{
		public string GetHeader()
		{
			return "Actual Publish Date";
		}

		public string Key()
		{
			return "wapdt";
		}

		public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
		{
			string text;
			if (articleItemWrapper.WebPublicationDateTime == DateTime.MinValue)
			{
				text = "N/E";
			}
			else
			{
				text = VWBUtil.GetFormattedDateTime(articleItemWrapper.WebPublicationDateTime);
			}
			return new TableCell { Text = text };
		}

		public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
		{
			return x.WebPublicationDateTime.CompareTo(y.WebPublicationDateTime);
		}

        public Dictionary<string, string> GetDropDownValues(List<ArticleItemWrapper> results)
        {
            throw new NotImplementedException();
        }
    }

}