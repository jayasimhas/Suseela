using System;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Utilities.Extensions
{
	public static class ArticleExtensions
	{
		public static DateTime GetDate(this IArticle article)
		{
			return Sitecore.Context.PageMode.IsPreview &&
			       !article.Planned_Publish_Date.Equals(DateTime.MinValue)
				? article.Planned_Publish_Date
				: article.Actual_Publish_Date;
		}
	}
}