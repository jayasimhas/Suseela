using System;
using System.Collections.Generic;
using System.Linq;

namespace Elsevier.Web.VWB.Report
{
	public class MacroVwbArticleItemCollection : IVwbArticleItemCollection
	{
		public const string IssuesKey = "issues";
		public List<IVwbArticleItemCollection> Collection;

		public static MacroVwbArticleItemCollection GetCollectionByIssue(IVwbArticleItemCollection collection)
		{
			var newCol = new List<IVwbArticleItemCollection>();
			var col = collection.GetCollectionOfArticles();
			
			var issueGuids = col.Select(a => a.IssueGuid).Distinct();
			//if you want to sort the issues, do it here


			foreach(Guid issueGuid in issueGuids)
			{
				Guid copy = issueGuid;
				if (col != null)
				{
					List<ArticleItemWrapper> curIssue = col.Where(a => a.IssueGuid == copy).ToList();
					var first = curIssue.First();
					first.IsFirstArticleInIssue = true;
					first.IssueGuid = issueGuid;
					first.NumArticlesInIssue = curIssue.Count();
					newCol.Add(new VwbArticleItemCollection(curIssue));
				}
			}
			var macroArticleCollection =
				new MacroVwbArticleItemCollection
					{
						Collection = newCol
					};
			return macroArticleCollection;
		}

		public static MacroVwbArticleItemCollection GetCollectionByArticleCategory(IVwbArticleItemCollection collection)
		{
			var newCol = new List<IVwbArticleItemCollection>();
			var col = collection.GetCollectionOfArticles();

			var articleCategoryGuids = col.Select(a => a.ArticleCategoryGuid).Distinct().ToList();
			var issueGuids = col.Select(a => a.IssueGuid).Distinct().ToList();

			foreach (Guid issueGuid in issueGuids)
			{
				foreach (Guid articleCategoryGuid in articleCategoryGuids)
				{
					if (col != null)
					{
						List<ArticleItemWrapper> curCategoryItem = col.Where(a => a.ArticleCategoryGuid == articleCategoryGuid && a.IssueGuid == issueGuid).ToList();
						if (curCategoryItem.Count > 0)
						{
							var first = curCategoryItem.First();
							if (first.ArticlesInCategoryShouldBeGrouped)
							{
								first.IsFirstArticleInCategory = true;
								first.ArticleCategoryGuid = articleCategoryGuid;
								first.NumArticlesInCategory = curCategoryItem.Count();
							}
							newCol.Add(new VwbArticleItemCollection(curCategoryItem));
						}
					}
				} 
			}
			var macroArticleCollection =
				new MacroVwbArticleItemCollection
				{
					Collection = newCol
				};
			return macroArticleCollection;
		}

		public List<ArticleItemWrapper> GetCollectionOfArticles()
		{
			var complete = new List<ArticleItemWrapper>();
			foreach (var item in Collection)
			{
				complete.AddRange(item.GetCollectionOfArticles());
			}

			return complete.ToList();
		}
	}
}