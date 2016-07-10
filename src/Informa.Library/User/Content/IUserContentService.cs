using System;
using System.Collections.Generic;

namespace Informa.Library.User.Content
{
	public interface IUserContentService<TIn, TOut>
	{
		bool Exists(TIn input);
		IEnumerable<TOut> GetContent();
		IContentResponse SaveContent(TIn input);
		IContentResponse DeleteContent(TIn input);
	}
}