using Glass.Mapper.Sc;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Models;

namespace Informa.Library.Page
{
	[AutowireService]
	public class PageItemContext : IPageItemContext
	{
		protected readonly ISitecoreContext SitecoreContext;

		public PageItemContext(
			ISitecoreContext sitecoreContext)
		{
			SitecoreContext = sitecoreContext;
		}

		public T Get<T>()
			where T : class, IGlassBase
		{
			return SitecoreContext.GetCurrentItem<T>();
		}
	}
}
