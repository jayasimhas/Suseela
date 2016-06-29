using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Informa.Library.Publication.Entity;

namespace Informa.Library.Publication
{
	public interface IArticleMappingRepository
	{
		void Insert(Guid articleId, string articleNumber, Guid pmbiArticleId, string pmbiArticleNumber);
	}
}
