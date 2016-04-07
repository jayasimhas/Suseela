using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
	public interface IUpdateNewsletterUserOptInsContext
	{
		bool Update(IEnumerable<INewsletterUserOptIn> optIns);
	}
}