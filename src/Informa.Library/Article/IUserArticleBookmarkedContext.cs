using System;

namespace Informa.Library.Article
{
	public interface IUserArticleBookmarkedContext
	{
		bool IsBookmarked(Guid id);
	}
}
