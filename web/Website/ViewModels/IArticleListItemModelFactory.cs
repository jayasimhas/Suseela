﻿using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Web.ViewModels
{
	public interface IArticleListItemModelFactory
	{
		IListableViewModel Create(IArticle article);
	}
}
