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
			          		//new ArticleNumberColumn(), 
							//this column is special because it is always included,
							//thus can't be removed, thus can't be "added", etc
			          		//new TitleColumn(),
							//TitleColumn is now also "special"
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
            _articleNumberColumn = new ArticleNumberColumn();
            _titleColumn = new TitleColumn();
            ImmutableColumns = new List<IVwbColumn>
                                {
                                    _articleNumberColumn,
                                    _titleColumn
                                };

        }

        public readonly List<IVwbColumn> Columns;
        private static readonly ColumnFactory _columnFactory = new ColumnFactory();
        private static ArticleNumberColumn _articleNumberColumn;
        private static TitleColumn _titleColumn;

        public List<IVwbColumn> ImmutableColumns;

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
            return Columns.Where(c => c.Key().Equals(key)).FirstOrDefault();
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
            if (keys == null || keys.Count() == 0)
            {
                return Columns;
            }
            return Columns.Where(c => !keys.Contains(c.Key()));
        }
    }
}