using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Search.Results
{
	public interface ILegacyArticleNumber
	{
		[IndexField(IArticleConstants.Legacy_Article_NumberFieldName)]
		string LegacyArticleNumber { get; set; }
	}
}
