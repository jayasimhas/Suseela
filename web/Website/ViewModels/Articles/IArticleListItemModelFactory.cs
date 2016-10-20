using System;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Web.ViewModels.Articles
{
	public interface IArticleListItemModelFactory
	{
		IListableViewModel Create(IArticle article);

        IListableViewModel Create(Guid articleId);

        IListableViewModel Create(string articleNumber);
    }
}
