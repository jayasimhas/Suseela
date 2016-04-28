using System;
using System.Collections.Generic;

namespace Informa.Library.User.Content
{
	public interface IUserContentService<TIn, TOut>
	{
		IEnumerable<TOut> GetContent();
		IContentResponse SaveContent(TIn input);
		IContentResponse DeleteContent(TIn input);
	}
}