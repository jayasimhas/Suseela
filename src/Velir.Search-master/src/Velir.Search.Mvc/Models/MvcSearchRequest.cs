using System.Web.Mvc;
using Jabberwocky.Glass.Factory;
using Velir.Search.Core.Models;
using Velir.Search.Core.Page;
using Velir.Search.Mvc.Binders;

namespace Velir.Search.Mvc.Models
{
	[ModelBinder(typeof(MvcSearchRequestBinder))]
	public class MvcSearchRequest : SearchRequest
	{
		public MvcSearchRequest(ISearchPageParser parser, IGlassInterfaceFactory factory) : base(parser, factory)
		{
		}

		public MvcSearchRequest(int page, int perPage) : base(page, perPage)
		{
		}
	}
}
