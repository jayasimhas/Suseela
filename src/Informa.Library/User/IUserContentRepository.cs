using System.Collections.Generic;

namespace Informa.Library.User
{
	public interface IUserContentRepository<T>
	{
		void Add(T entity);
		void Update(T entity);
		void Delete(T entity);
		T GetById(object id);
		IEnumerable<T> GetMany(string username = null);
	}
}
