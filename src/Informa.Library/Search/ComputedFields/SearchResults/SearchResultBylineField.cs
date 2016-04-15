using System.Linq;
using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Search.ComputedFields.SearchResults
{
	public class SearchResultBylineField : BaseGlassComputedField<IArticle>
	{
		public override object GetFieldValue(IArticle indexItem)
		{
			if (indexItem.Authors == null)
			{
				return string.Empty;
			}

			var authorCount = indexItem.Authors.Count();
			if (authorCount == 0)
			{
				return string.Empty;
			}

			//TamerM - 2016-04-03: comma separate except the last one and replace with 'and'
			return string.Join(", ", indexItem.Authors?.Take(authorCount - 1).Select(x => $"{x.First_Name} {x.Last_Name}")) + ((authorCount > 1 ? " and " : "") + indexItem.Authors.Select(x => $"{x.First_Name} {x.Last_Name}").LastOrDefault());
		}
	}
}
