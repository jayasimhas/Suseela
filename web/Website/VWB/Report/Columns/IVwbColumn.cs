using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Elsevier.Web.VWB.Report.Columns
{
	public interface IVwbColumn : IComparer<ArticleItemWrapper>
	{
		string GetHeader();
		string Key();
		TableCell GetCell(ArticleItemWrapper articleItemWrapper);
        Dictionary<string, string> GetDropDownValues(List<ArticleItemWrapper> results);

    }
}