using System.Collections.Generic;
using Informa.Models.FactoryInterface;

namespace Informa.Web.ViewModels.Articles
{
	public interface IArticleTagsViewModel
	{
		IEnumerable<ILinkable> Tags { get; }
	}
}
