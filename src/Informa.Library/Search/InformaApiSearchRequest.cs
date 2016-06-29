using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Informa.Library.Utilities.References;
using Velir.Search.WebApi.Binders;
using Velir.Search.WebApi.Models;

namespace Informa.Library.Search
{
	public class ApiSearchRequestModelBinder : ApiSearchRequestBinder
	{
		public override bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
		{
			bool success = base.BindModel(actionContext, bindingContext);

			if (success)
			{
				int perPage = GetPerPage(bindingContext);

				var request = bindingContext.Model as ApiSearchRequest;

				if (request != null && perPage > 0)
				{
					request.PerPage = perPage;
				}

				bindingContext.Model = request;
			}

			return success;
		}

		protected virtual int GetPerPage(ModelBindingContext bindingContext)
		{
			string pageValue = GetBindingParameter(bindingContext, Constants.QueryString.PerPageKey);
			if (!string.IsNullOrEmpty(pageValue))
			{
				int page = 0;
				if (int.TryParse(pageValue, out page) && page > 0 && Constants.PerPageSizes.Contains(page))
				{
					return page;
				}
			}

			return -1;
		}
	}
}
