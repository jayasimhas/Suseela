using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using Sitecore.Data;
using Velir.Search.Core.Reference;

namespace Velir.Search.WebApi.Binders
{
	public abstract class SearchRequestBinderBase : IModelBinder
	{
		protected abstract string[] IgnoredQueryParams { get; }

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

		public abstract bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext);

		protected virtual string GetSortOrder(ModelBindingContext bindingContext)
		{
			string sortOrder = GetBindingParameter(bindingContext, SiteSettings.QueryString.SortOrderKey);
			return !string.IsNullOrEmpty(sortOrder) ? sortOrder : null;
		}
	}
}