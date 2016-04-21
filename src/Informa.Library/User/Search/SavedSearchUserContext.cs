using System.Collections.Generic;
using System.Linq;
using Informa.Library.User.Authentication;

namespace Informa.Library.User.Search
{
	public class SavedSearchUserContext : ISavedSearchUserContext
	{
		private const string SessionKey = nameof(SavedSearchUserContext);

		protected readonly IUserContentRepository<ISavedSearchEntity> Repository;
		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IAuthenticatedUserSession UserSession;

		public SavedSearchUserContext(IUserContentRepository<ISavedSearchEntity> repository, IAuthenticatedUserContext userContext, IAuthenticatedUserSession userSession)
		{
			Repository = repository;
			UserContext = userContext;
			UserSession = userSession;
		}

		public void Add(ISavedSearchEntity entity)
		{
			if (UserContext.IsAuthenticated)
			{
				entity.Username = UserContext.User.Name;
				Repository.Add(entity);
				Clear();
			}
		}

		public void Update(ISavedSearchEntity entity)
		{
			if (UserContext.IsAuthenticated)
			{
				entity.Username = UserContext.User.Name;
				Repository.Update(entity);
				Clear();
			}
		}

		public void Delete(ISavedSearchEntity entity)
		{
			if (UserContext.IsAuthenticated)
			{
				entity.Username = UserContext.User.Name;
				Repository.Delete(entity);
				Clear();
			}
		}

		public ISavedSearchEntity GetById(object id)
		{
			return Repository.GetById(id);
		}

		public IEnumerable<ISavedSearchEntity> GetMany(string username = null)
		{
			if (!UserContext.IsAuthenticated)
			{
				return Enumerable.Empty<ISavedSearchEntity>();
			}

			var savedSearches = UserSession.Get<IEnumerable<ISavedSearchEntity>>(SessionKey);

			if (savedSearches.HasValue)
			{
				return savedSearches.Value;
			}

			var savedDocuments = Repository.GetMany(UserContext.User.Username);

			UserSession.Set(SessionKey, savedDocuments);

			return savedDocuments;
		}

		public void Clear()
		{
			UserSession.Clear(SessionKey);
		}
	}
}