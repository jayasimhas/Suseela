using System.Web;
using System.Web.Mvc;
using Glass.Mapper.Sc;
using Jabberwocky.Glass.Factory;
using Sitecore.Data;
using Velir.Search.Core.Extensions;
using Velir.Search.Core.Page;
using Velir.Search.Core.Reference;
using Velir.Search.Core.Rules.Parser;
using Velir.Search.Mvc.Models;

namespace Velir.Search.Mvc.Binders
{
	public class MvcSearchRequestBinder : IModelBinder
	{
		private static readonly string[] _ignoredQueryParams =
		{
			SiteSettings.QueryString.PageIdKey,
			SiteSettings.QueryString.PageKey,
			SiteSettings.QueryString.SortKey,
			SiteSettings.QueryString.SortOrderKey
		};

		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			if (bindingContext.ModelType != typeof(MvcSearchRequest))
			{
				return null;
			}

			var pageId = GetPageId(bindingContext);
			var page = GetPage(bindingContext);
			var sort = GetSort(bindingContext);
			var sortOrder = GetSortOrder(bindingContext);

			// Add extraneous query parameters (excludes pageId, page, sort)
			var queryParams = controllerContext.HttpContext.Request.QueryString.ToQueryParamDictionary(IgnoredQueryParams);

			// Resolve the service you want to use.
			var glassFactory = DependencyResolver.Current.GetService(typeof(IGlassInterfaceFactory)) as IGlassInterfaceFactory;

			var sitecoreContext = DependencyResolver.Current.GetService(typeof(ISitecoreContext)) as ISitecoreContext;

			var ruleParser = DependencyResolver.Current.GetService(typeof(ISearchRuleParser)) as ISearchRuleParser;

			var searchPageParser = new SearchPageParser(pageId, sitecoreContext, ruleParser);

			var result = new MvcSearchRequest(searchPageParser, glassFactory) { Page = page, PageId = pageId, SortBy = sort, SortOrder = sortOrder, QueryParameters = queryParams };
			
			return result;
		}

		protected string[] IgnoredQueryParams
		{
			get { return _ignoredQueryParams; }
		}

		protected static string GetBindingParameter(ModelBindingContext bindingContext, string parameterName)
		{
			ValueProviderResult val = bindingContext.ValueProvider.GetValue(parameterName);
			if (val == null)
			{
				return null;
			}

			return HttpUtility.HtmlEncode(val.RawValue);
		}

		protected virtual string GetPageId(ModelBindingContext bindingContext)
		{
			string pageId = GetBindingParameter(bindingContext, SiteSettings.QueryString.PageIdKey);
			return !string.IsNullOrEmpty(pageId) && ID.IsID(pageId) ? pageId : null;
		}

		protected virtual int GetPage(ModelBindingContext bindingContext)
		{
			string pageValue = GetBindingParameter(bindingContext, SiteSettings.QueryString.PageKey);
			if (!string.IsNullOrEmpty(pageValue))
			{
				int page = 0;
				if (int.TryParse(pageValue, out page) && page > 0)
				{
					return page;
				}
			}

			return 1;
		}

		protected virtual string GetSort(ModelBindingContext bindingContext)
		{
			string sort = GetBindingParameter(bindingContext, SiteSettings.QueryString.SortKey);
			return !string.IsNullOrEmpty(sort) ? sort : null;
		}

		protected virtual string GetSortOrder(ModelBindingContext bindingContext)
		{
			string sortOrder = GetBindingParameter(bindingContext, SiteSettings.QueryString.SortOrderKey);
			return !string.IsNullOrEmpty(sortOrder) ? sortOrder : null;
		}
	}
}
