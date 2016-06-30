using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Informa.Library.Utilities.WebApi.Filters
{
	public class ValidateReasonsAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			var modelState = actionContext.ModelState;

			if (!modelState.IsValid)
			{
				var reasons = new List<string>();

				reasons.AddRange(modelState.SelectMany(ms => ms.Value.Errors.Select(e => e.ErrorMessage)).Distinct());

				if (!reasons.Any())
				{
					reasons.Add("Unknown");
				}

				var response = new
				{
					success = false,
					reasons = reasons
				};

				actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.OK, response);
			}
		}
	}
}
