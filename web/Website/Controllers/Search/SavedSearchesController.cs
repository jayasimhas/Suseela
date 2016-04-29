using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
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

		public bool Get(string url)
		{
			return _savedSearchService.Exists(new SavedSearchInput
			{
				Url = url
			});
		}

		public IEnumerable<ISavedSearchDisplayable> Get()
		{
			return _savedSearchService.GetContent();
		}

		public IHttpActionResult Post(SavedSearchInput model)
		{
			var result = _savedSearchService.SaveContent(model);

			return Ok(new
			{
				success = result.Success,
				message = result.Message
			});
		}

		public IHttpActionResult Delete(SavedSearchInput model)
		{
			var result = _savedSearchService.DeleteContent(model);
			return Ok(new
			{
				success = result.Success,
				message = result.Message
			});
		}
	}

	public class SavedSearchInputModelBinder : IModelBinder
	{
		public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
		{
			bindingContext.Model = new SavedSearchInput
			{
				Title = bindingContext.ValueProvider.GetValue("Title").RawValue as string
			};

			return true;
		}
	}
}