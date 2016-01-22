using Informa.Models.FactoryInterface;
using System.Collections.Generic;

namespace Informa.Web.ViewModels
{
	public interface IArticleTagsViewModel
	{
		IEnumerable<ILinkable> Tags { get; }
	}
}
