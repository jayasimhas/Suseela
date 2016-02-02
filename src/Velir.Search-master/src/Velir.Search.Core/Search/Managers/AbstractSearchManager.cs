using Glass.Mapper.Sc;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Velir.Search.Core.Search.Queries;
using Velir.Search.Core.Search.Results;

namespace Velir.Search.Core.Search.Managers
{
	/// <summary>
	/// Abstract class to provide base functionality to querying Sitecore via it's ContentSearchManager class
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class AbstractSearchManager<T> : ISearchManager<T> where T : SearchResultItem
	{
		protected ISearchIndex SearchIndex { get; set; }

		protected AbstractSearchManager(string databaseName)
		{
			SearchIndex = GetSearchIndex(databaseName);
		}

		protected AbstractSearchManager(string indexName = null, ISitecoreService service = null)
		{
			SearchIndex = GetSearchIndex(indexName, service);
		}

		private ISearchIndex GetSearchIndex(string indexOrDatabase = null, ISitecoreService service = null)
		{
			string indexName = "sitecore_{0}_index";

			if (!string.IsNullOrEmpty(indexOrDatabase) && service != null)
			{
				indexName = indexOrDatabase;
			}

			return ContentSearchManager.GetIndex(string.Format(indexName, service != null ? service.Database.Name : !string.IsNullOrEmpty(indexOrDatabase)? indexOrDatabase : string.Empty));
		}

		protected AbstractSearchManager(ISearchIndex index)
		{
			SearchIndex = index;
		} 

		public abstract IQueryResults GetItems(ISearchQueryable<T> queryArguments);
		public abstract int GetCount(ISearchQueryable<T> queryArguments);
	}
}
