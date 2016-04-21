using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Informa.Library.User.Search;
using Informa.Library.Utilities.Extensions;
using Velir.Search.Core.Reference;

namespace Informa.Web.Controllers.Search
{
	public class SavedSearchesController : ApiController
	{
		private readonly ISavedSearchUserContext _userContext;

		public SavedSearchesController(ISavedSearchUserContext savedSearchContext)
		{
			_userContext = savedSearchContext;
		}

		public IEnumerable<SavedSearchModel> Get()
		{
			return _userContext.GetMany().Select(s => new SavedSearchModel
			{
				Sources = s.Url.ExtractParamValue("publication").Split(SiteSettings.ValueSeparator),
				Title = s.Title,
				Url = s.Url,
				HasAlert = s.HasAlert,
				DateSaved = s.DateSaved
			});
		}

		public void Post(SavedSearchModel model)
		{
			_userContext.Add(model);
		}

		public void Update(SavedSearchModel model)
		{
			_userContext.Update(model);
		}

		public void Delete(SavedSearchModel model)
		{
			_userContext.Delete(model);
		}
	}

	public class SavedSearchModel : ISavedSearchEntity
	{
		public IEnumerable<string> Sources { get; set; }
		public string Username { get; set; }
		public string Title { get; set; }
		public string Url { get; set; }
		public bool HasAlert { get; set; }
		public System.DateTime DateSaved { get; set; }
	}
}