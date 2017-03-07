using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Elsevier.Web.VWB.Report.Columns
{
	public class ProofreadingStatusColumn : IVwbColumn
	{
		public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
		{
			throw new NotImplementedException();
		}

		public string GetHeader()
		{
			return "Proofreading Status";
		}

		public string Key()
		{
			return "pfs";
		}

		public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
		{
			throw new NotImplementedException();
		}

        public Dictionary<string, string> GetDropDownValues(List<ArticleItemWrapper> results)
        {
            throw new NotImplementedException();
        }
    }
}