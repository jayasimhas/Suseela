using System.Collections.Generic;
using Informa.Library.Search.Utilities;
using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Search.ComputedFields.Facets
{
	public class CompaniesField : BaseGlassComputedField<IArticle>
	{
		public override object GetFieldValue(IArticle indexItem)
		{
			if (string.IsNullOrEmpty(indexItem.Referenced_Companies))
			{
				return new List<string>();
			}

			return SearchCompanyUtil.GetCompanyNames(indexItem.Referenced_Companies);
		}
	}
}
