using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Elsevier.Library.Reference;
using Elsevier.Web.VWB.Util;

namespace Elsevier.Web.VWB.Report.Columns
{
	public class PlannedPublishDateTimeColumn : IVwbColumn
	{

		public string GetHeader()
		{
			return "Planned Publish Date";
		}

		public string Key()
		{
			return "sapdt";
		}

		public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
		{
			string text;
			if (articleItemWrapper.SAPDateTime == DateTime.MinValue)
			{
				text = "N/E";
			}
			else
			{
				text = VWBUtil.GetFormattedDateTime(articleItemWrapper.SAPDateTime);
			}
			return new TableCell { Text = text };
		}

		public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
		{
			return x.SAPDateTime.CompareTo(y.SAPDateTime);
		}

        public Dictionary<string, string> GetDropDownValues(List<ArticleItemWrapper> results)
        {
            throw new NotImplementedException();
        }
    }
}