using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
	public interface ISetPublicationsNewsletterUserOptIns
	{
		bool Set(IEnumerable<string> publications);
        bool SetUpdate(IEnumerable<Publications> Publication);

    }
}