using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
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
			if (string.IsNullOrEmpty(url)) return false;

			return _savedSearchService.Exists(new SavedSearchInput
			{
				Url = url
			});
		}

		public IEnumerable<ISavedSearchDisplayable> Get()
		{
			return _savedSearchService.GetContent();
		}

		public IHttpActionResult Post([ModelBinder(typeof(SavedSearchInputPostModelBinder))]SavedSearchInput model)
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

	public class SavedSearchInputPostModelBinder : IModelBinder
	{
		public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
		{
			var result = actionContext.Request.Content.ReadAsStringAsync().Result;
			var collection = HttpUtility.ParseQueryString(result);

			var input = new SavedSearchInput
			{
				Title = HttpUtility.UrlDecode(collection["Title"]),
				Url = HttpUtility.UrlDecode(collection["Url"]),
				AlertEnabled = GetBoolValue(HttpUtility.UrlDecode(collection["AlertEnabled"]))
			};
			// reverse the value passed.
			bool alert = Regex.IsMatch(result, @"alert-toggle-\d+?=off");
			if (alert)
			{
				input.AlertEnabled = alert;
			}

			bindingContext.Model = input;

			return true;
		}

		private bool GetBoolValue(string rawValue)
		{
			bool value = false;
			bool.TryParse(rawValue, out value);

			return value;
		}
	}
}