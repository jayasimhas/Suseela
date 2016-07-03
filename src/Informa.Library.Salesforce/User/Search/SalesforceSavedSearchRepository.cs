using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.User.Content;
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

		public IContentResponse Add(ISavedSearchEntity entity)
		{
            if( entity == null || 
                new[]{entity.Username, entity.Name, entity.SearchString, entity.UnsubscribeToken}.Any(string.IsNullOrEmpty))
		    {
				return new ContentResponse
				{
					Success = false,
					Message = "Invalid input has been provided."
				};
			}

		    var response =
		        Service.Execute(
		            s =>
		                s.createSavedSearchItem2(entity.Username, entity.SearchString, entity.Name, entity.HasAlert,
		                    entity.UnsubscribeToken, entity.Publication));

			return CreateResponse(response);
		}

		public IContentResponse Update(ISavedSearchEntity entity)
		{
			if (string.IsNullOrEmpty(entity?.Username) || string.IsNullOrEmpty(entity.Name) || string.IsNullOrEmpty(entity.SearchString))
			{
				return new ContentResponse
				{
					Success = false,
					Message = "Invalid input has been provided."
				};
			}

			var response = Service.Execute(s => s.updateSavedSearchItem(entity.Username, entity.SearchString, entity.Name, entity.HasAlert));

			return CreateResponse(response);
		}

		public IContentResponse Delete(ISavedSearchEntity entity)
		{
			if (string.IsNullOrEmpty(entity?.Username) || string.IsNullOrEmpty(entity.Name))
			{
				return new ContentResponse
				{
					Success = false,
					Message = "Invalid input has been provided."
				};
			}

			var response = Service.Execute(s => s.deleteSavedSearch(entity.Username, entity.Name));

			return CreateResponse(response);
		}

		public ISavedSearchEntity GetById(object id)
		{
			var itemId = id as ISavedSearchItemId;

			if (string.IsNullOrEmpty(itemId?.Username) || string.IsNullOrEmpty(itemId.Name)) return default(ISavedSearchEntity);

			return GetMany(itemId.Username, s => s.Name == itemId.Name).FirstOrDefault();
		}

		public IEnumerable<ISavedSearchEntity> GetMany(string username, Func<ISavedSearchEntity, bool> @where = null)
		{
			if (string.IsNullOrEmpty(username))
			{
				return Enumerable.Empty<ISavedSearchEntity>();
			}

			var response = Service.Execute(s => s.querySavedSearchItems2(username));

			if (!response.IsSuccess() || response.savedSearches == null)
			{
				return Enumerable.Empty<ISavedSearchEntity>();
			}

			var savedDocuments = response.savedSearches.Select(ssi => new SavedSearchEntity
			{
				Username = username,
				Name = ssi.name,
				SearchString = ssi.searchString,
				HasAlert = ssi.IsReceivingEmailAlert ?? false,
				DateCreated = ssi.SaveDate.GetValueOrDefault()
			});

			return savedDocuments.Where(@where ?? (x => true));
		}

		private IContentResponse CreateResponse(IEbiResponse webServiceResponse)
		{
			return new ContentResponse
			{
				Success = webServiceResponse.IsSuccess(),
				Message = webServiceResponse.errors?.FirstOrDefault()?.message ?? string.Empty
			};
		}
	}
}
