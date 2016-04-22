using System.Collections.Generic;
using System.Web.Http;
using Informa.Library.User.Content;
using Informa.Library.User.Search;

namespace Informa.Web.Controllers.Search
{
	public class SavedSearchesController : ApiController
	{
		private readonly IUserContentService<ISavedSearchSaveable, ISavedSearchDisplayable> _savedSearchService;

		public SavedSearchesController(IUserContentService<ISavedSearchSaveable, ISavedSearchDisplayable> savedSearchService)
		{
			_savedSearchService = savedSearchService;
		}

		public IEnumerable<ISavedSearchDisplayable> Get()
		{
			return _savedSearchService.GetContent();
		}

		public IContentResponse Post(ISavedSearchSaveable model)
		{
			return _savedSearchService.SaveContent(model);
		}

		public IContentResponse Delete(ISavedSearchSaveable model)
		{
			return _savedSearchService.DeleteContent(model);
		}
	}
}