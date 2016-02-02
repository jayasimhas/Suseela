using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Glass.Mapper.Sc;
using Jabberwocky.Glass.Factory;
using Velir.Search.Core.Extensions;
using Velir.Search.Core.Models;
using Velir.Search.Core.Page;
using Velir.Search.Core.Reference;
using Velir.Search.WebApi.Models;

namespace Velir.Search.WebApi.Binders
{
	public class ApiSearchRequestBinder : SearchRequestBinderBase
	{
		private static readonly string[] _ignoredQueryParams =
		{
			SiteSettings.QueryString.PageIdKey,
			SiteSettings.QueryString.PageKey,
			SiteSettings.QueryString.SortKey,
			SiteSettings.QueryString.SortOrderKey
		};

		public override bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
		{
			if (bindingContext.ModelType != typeof(ApiSearchRequest))
			{
				return false;
			}

			var pageId = GetPageId(bindingContext);
			var page = GetPage(bindingContext);
			var sort = GetSort(bindingContext);
			var sortOrder = GetSortOrder(bindingContext);

			// Add extraneous query parameters (excludes pageId, page, sort)
			var queryParams = actionContext.Request.GetQueryNameValuePairs().Where(kvp => !IgnoredQueryParams.Contains(kvp.Key) && !string.IsNullOrEmpty(kvp.Value)).ToQueryParamDictionary();

			// Get the request lifetime scope so you can resolve services.
			var requestScope = actionContext.Request.GetDependencyScope();

			// Resolve the service you want to use.
			var glassFactory = requestScope.GetService(typeof(IGlassInterfaceFactory)) as IGlassInterfaceFactory;

			var searchPageParser = requestScope.GetService(typeof(ISearchPageParser)) as ISearchPageParser;

			var result = new ApiSearchRequest(searchPageParser, glassFactory) { Page = page, PageId = pageId, SortBy = sort, SortOrder = sortOrder, QueryParameters = queryParams };
			bindingContext.Model = result;

			return true;
		}

		protected override string[] IgnoredQueryParams
		{
			get { return _ignoredQueryParams; }
		}
	}
}