using System.Web.Http.ModelBinding;
using Jabberwocky.Glass.Factory;
using Velir.Search.Core.Models;
using Velir.Search.Core.Page;
using Velir.Search.WebApi.Binders;

namespace Velir.Search.WebApi.Models
{
	[ModelBinder(typeof(ApiSearchRequestBinder))]
	public class ApiSearchRequest : SearchRequest
	{
		public ApiSearchRequest(ISearchPageParser parser, IGlassInterfaceFactory factory) : base(parser, factory)
		{
		}

		public ApiSearchRequest(int page, int perPage) : base(page, perPage)
		{
		}
	}
}
