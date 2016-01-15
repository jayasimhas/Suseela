using System.Collections.Generic;
using System.Linq;
using Jabberwocky.Glass.Models;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.Models
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class PageLinksFactory : IPageLinksFactory
	{
		public IEnumerable<IPageLink> Create(IEnumerable<IGlassBase> items)
		{
			if (items == null)
			{
				return Enumerable.Empty<IPageLink>();
			}

			return items.Select(item => new PageLink
			{
				Text = item._Name,
				Url = item._Url
			});
		}
	}
}