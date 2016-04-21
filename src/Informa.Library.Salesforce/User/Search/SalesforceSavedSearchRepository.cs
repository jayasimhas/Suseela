using System.Collections.Generic;
using System.Linq;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.User;
using Informa.Library.User.Search;

namespace Informa.Library.Salesforce.User.Search
{
	public class SalesforceSavedSearchRepository : IUserContentRepository<ISavedSearchEntity>
	{
		protected readonly ISalesforceServiceContext Service;

		public SalesforceSavedSearchRepository(ISalesforceServiceContext service)
		{
			Service = service;
		}

		public void Add(ISavedSearchEntity entity)
		{
			if (string.IsNullOrEmpty(entity?.Username) || string.IsNullOrEmpty(entity.Title) || string.IsNullOrEmpty(entity.Url))
			{
				return;
			}

			var response = Service.Execute(s => s.createSavedSearchItem(entity.Username, entity.Url, entity.Title, entity.HasAlert));
		}

		public void Update(ISavedSearchEntity entity)
		{
			if (string.IsNullOrEmpty(entity?.Username) || string.IsNullOrEmpty(entity.Title) || string.IsNullOrEmpty(entity.Url))
			{
				return;
			}

			var response = Service.Execute(s => s.updateSavedSearchItem(entity.Username, entity.Url, entity.Title, entity.HasAlert));
		}

		public void Delete(ISavedSearchEntity entity)
		{
			if (string.IsNullOrEmpty(entity?.Username) || string.IsNullOrEmpty(entity.Title))
			{
				return;
			}

			var response = Service.Execute(s => s.deleteSavedSearch(entity.Username, entity.Title));
		}

		public ISavedSearchEntity GetById(object id)
		{
			var itemId = id as ISavedSearchItemId;

			if (string.IsNullOrEmpty(itemId?.Username) || string.IsNullOrEmpty(itemId.Title)) return default(ISavedSearchEntity);

			return GetMany(itemId.Username).FirstOrDefault(s => s.Title == itemId.Title);
		}

		public IEnumerable<ISavedSearchEntity> GetMany(string username)
		{
			if (string.IsNullOrEmpty(username))
			{
				return Enumerable.Empty<ISavedSearchEntity>();
			}

			var response = Service.Execute(s => s.querySavedSearchItems(username));

			if (!response.IsSuccess() || response.savedSearches == null)
			{
				return Enumerable.Empty<ISavedSearchEntity>();
			}

			var savedDocuments = response.savedSearches.Select(ssi => new SavedSearchEntity
			{
				Username = username,
				Title = ssi.name,
				Url = ssi.searchString,
				HasAlert = ssi.IsReceivingEmailAlertSpecified,
				DateSaved = ssi.SaveDate.GetValueOrDefault()
			});

			return savedDocuments;
		}
	}
}
