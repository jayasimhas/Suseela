using Jabberwocky.Glass.Autofac.Attributes;
using System;

namespace Informa.Library.Article
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class MockUserArticleBookmarkedContext : IUserArticleBookmarkedContext
	{
		public bool IsBookmarked(Guid id)
		{
			return false;
		}
	}
}
