using System;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Informa.Library.User.Content
{
	public interface IUserContentRepository<T>
	{
		IContentResponse Add(T entity);
		IContentResponse Update(T entity);
		IContentResponse Delete(T entity);
		T GetById(object id);
		IEnumerable<T> GetMany(string username, Func<T, bool> @where = null);
	}
}
