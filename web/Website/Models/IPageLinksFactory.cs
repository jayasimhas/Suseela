using Jabberwocky.Glass.Models;
using System.Collections.Generic;

namespace Informa.Web.Models
{
	public interface IPageLinksFactory
	{
		IEnumerable<IPageLink> Create(IEnumerable<IGlassBase> items);
	}
}
