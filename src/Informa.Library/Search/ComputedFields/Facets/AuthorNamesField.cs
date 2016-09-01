using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.Search.Utilities;
using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;

namespace Informa.Library.Search.ComputedFields.Facets
{
	public class AuthorNamesField : BaseGlassComputedField<IArticle>
	{
		public override object GetFieldValue(IArticle indexItem)
		{
			if (!indexItem.Authors.Any())
			{
				return new List<string>();
			}

			return indexItem.Authors.Select(ToAuthorName).ToList<string>();
		}

		public static string ToAuthorName(IStaff_Item staffMember)
		{
			return string.Join(" ", staffMember.First_Name, staffMember.Last_Name);
		}
	}
}
