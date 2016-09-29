using System.Collections.Generic;
using System.Linq;
using Informa.Web.VWB.Report.Columns;

namespace Elsevier.Web.VWB.Report.Columns
{
	/// <summary>
	/// Add IVwbColumns here
	/// </summary>
	public class ColumnFactory
	{
		private ColumnFactory()
		{

			Columns = new List<IVwbColumn>
									{
										new AuthorsColumn(),
										new CreatedDateTimeColumn(),
										new PlannedPublishDateTimeColumn(),
										new ActualPublishDateTimeColumn(),
										new SidebarColumn(),
										new NotesToEditorialColumn(),
										new PublishableAfterColumn(),
										new WorkflowStateColumn(),
										new TaxonomyColumn(),
										new WordCountColumn(),
										new ContentTypeColumn(),
										new MediaTypeColumn(),
										new EmailPriorityColumn(),
										new ArticlePublicationNameColumn()
									};

			_articleCheckboxes = new ArticleCheckboxes();
			_articleNumberColumn = new ArticleNumberColumn();
			_titleColumn = new TitleColumn();
			ImmutableColumns = new List<IVwbColumn>
													{
														_articleCheckboxes,
														_articleNumberColumn,
														_titleColumn
													};

		}

		public readonly List<IVwbColumn> Columns;
		private static readonly ColumnFactory _columnFactory = new ColumnFactory();
		private static ArticleCheckboxes _articleCheckboxes;
		private static ArticleNumberColumn _articleNumberColumn;
		private static TitleColumn _titleColumn;

		public List<IVwbColumn> ImmutableColumns;

		public static IVwbColumn GetArticleCheckboxes()
		{
			return _articleCheckboxes;
		}

		public static IVwbColumn GetArticleNumberColumn()
		{
			return _articleNumberColumn;
		}

		public static IVwbColumn GetTitleColumn()
		{
			return _titleColumn;
		}

		public static ColumnFactory GetColumnFactory()
		{
			return _columnFactory;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns>Null if no column with specified key exists; else the column</returns>
		public IVwbColumn GetColumn(string key)
		{
			return Columns.FirstOrDefault(c => c.Key().Equals(key));
		}

		public List<IVwbColumn> GetColumns(IEnumerable<string> keys)
		{
			var columns = new List<IVwbColumn>();
			foreach (string key in keys)
			{
				string curKey = key;
				columns.AddRange(Columns.Where(c => c.Key().Equals(curKey)));
			}
			return columns;
		}

		/// <summary>
		/// Gets columns in factory that do not match any of the specified keys
		/// </summary>
		/// <param name="keys"></param>
		/// <returns></returns>
		public IEnumerable<IVwbColumn> GetColumnsNot(IEnumerable<string> keys)
		{
			var keyArr = keys as string[] ?? keys.ToArray();
			if (keys == null || !keyArr.Any())
			{
				return Columns;
			}
			return Columns.Where(c => !keyArr.Contains(c.Key()));
		}
	}
}