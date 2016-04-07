using System.Collections.Generic;

namespace Informa.Library.User.Profile
{
	public interface IUpdateNewsletterUserOptInsContext
	{
		bool Update(IEnumerable<INewsletterUserOptIn> optIns);
	}
}