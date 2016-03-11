using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using Elsevier.Library.Config;
using Elsevier.Library.CustomItems.Publication.General;
using Elsevier.Library.LuceneSearch.Indexes;
using Elsevier.Library.LuceneSearch.Parameters;
using Elsevier.Library.LuceneSearch.Searchers;
using Elsevier.Library.Reference;
using Elsevier.Web.VWB.Report.Columns;
using Informa.Library.Rss;
using Informa.Library.Utilities.References;
using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Web;

namespace Elsevier.Web.VWB.Report
{
	public class ReportBuilder
	{
		public List<IVwbColumn> Columns;
		static readonly ColumnFactory ColumnFactory = ColumnFactory.GetColumnFactory();
		private PublicationItem _publication;
		private List<ArticleItemWrapper> _results;
		IVwbColumn articleNumberColumn;
		IVwbColumn titleColumn;
		private IssueItem _iitem;
		private readonly VwbQuery _query;
		private readonly Page _page;
		public ReportBuilder(Page page,VwbQuery query)
		{
			_page = page;
			_query = query.Clone();
			IEnumerable<string> immutableColKeys = ColumnFactory.ImmutableColumns.Select(i => i.Key());
			if(_query.SortColumnKey != null && !_query.ColumnKeysInOrder.Contains(_query.SortColumnKey) 
				&& !immutableColKeys.Contains(_query.SortColumnKey))
			{
				_query.SortColumnKey = null;
			}
			if(string.IsNullOrEmpty(_query.SortColumnKey))
			{ //if no sort defined, sort by article number
				_query.SortColumnKey = ColumnFactory.GetArticleNumberColumn().Key();
			}
		}

		public void BuildTable(Table report)
		{
			if (!_query.ShouldRun) return;
			//if (AlertNoPublicationSelected(report) || AlertInvalidDateRange(report))
			//{
			//	return;
			//}
			_results = RunSearch(_query).ToList();
			if (AlertTryingToGetNextIssueWhenNoneExists(report))
			{
				return;
			}
			InitializeColumns(_query);
			BuildHeaderRows(report);
			BuildResultRows(report);
		}

		private bool AlertTryingToGetNextIssueWhenNoneExists(Table report)
		{
			if (_query.IssueIdValue == VwbQuery.NextIssueValue && _iitem == null && (_results == null || _results.Count() == 0))
			{
				report.Rows.Add(_publication.IsDaily()
				                	? CreateAlert("There are no articles tomorrow!")
				                	: CreateAlert("There is no future issue for this publication!"));
				return true;
			}
			return false;
		}

		private bool AlertNoPublicationSelected(Table report)
		{
			if(string.IsNullOrEmpty(_query.PublicationIdValue))
			{
				report.Rows.Add(CreateAlert("Please select a publication"));
				return true;
			}
			return false;
		}

		private bool AlertInvalidDateRange(Table report)
		{
			DateTime? startDate = _query.StartDate;
			DateTime? endDate = _query.EndDate;


			if (String.IsNullOrEmpty(_query.IssueIdValue))
			{
				if ((startDate == null || endDate == null))
				{
					report.Rows.Add(CreateAlert("Please specify date range or issue!"));
					return true;
				}

				if (startDate.Value.CompareTo(endDate.Value) > 0)
				{
					report.Rows.Add(CreateAlert("The start date is after the end date!"));
					return true;
				} 
			}
			var appConfig = new ApplicationConfig();
			int maxRange = appConfig.VwbMaximumDateRange;
			if(startDate != null && endDate != null && Math.Abs(endDate.Value.Subtract(startDate.Value).Days) > maxRange)
			{
				report.Rows.Add(CreateAlert(String.Format("Range of start and end date too large! Limit is {0} days.", maxRange)));
				return true;
			}
			return false;
		}

		private TableRow CreateAlert(string text)
		{
			var alertLabel = new Label
			{
				Text = text
			};
			var alertRow = new TableRow();
			var alertCell = new TableCell();
			alertCell.Controls.Add(alertLabel);
			alertRow.Cells.Add(alertCell);

			return alertRow;
		}

		/// <summary>
		/// Builds the table cells using the IVwbColumn's GetCell function
		/// </summary>
		/// <param name="report"></param>
		private void BuildResultRows(Table report)
		{
			bool first = true; //first row in table
			foreach (var result in _results)
			{
				var resultRow = new TableRow();
				
				foreach(var col in Columns)
				{
					if(!first && (result.IsFirstArticleInCategory))
					{
						resultRow.CssClass = "double";
					}
					TableCell tableCell = col.GetCell(result);
					if (tableCell != null)
					{
						resultRow.Cells.Add(tableCell); 
					}
					
				}
				first = false;
				report.Rows.Add(resultRow);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="query"></param>
		/// <returns>Sorted list of ArticleItemProxies based on query</returns>
		public List<ArticleItemWrapper> RunSearch(VwbQuery query)
		{
           // var param = new ArticleSearchParam();
            //param.IncludeSidebarArticles();
            List<ArticleItemWrapper> articles;
            //string issue = query.IssueIdValue;
            //var pubId = new ID(query.PublicationIdValue);
            //DateTime today = DateTime.Today;
            //if (!string.IsNullOrEmpty(issue))
            //{
            //    if (issue == VwbQuery.NextIssueValue)
            //    {
            //        _publication = SitecoreDatabases.Context.GetItem(pubId);

            //        if (_publication.IsDaily())
            //        {
            //            //make the date range encompass "tomorrow"
            //            param.DateRanges.Add(
            //                new DateRangeSearchParam.DateRangeField(
            //                ArticleIndex.SortDateField,
            //                today.AddDays(1),
            //                today.AddDays(2)
            //            ));
            //        }
            //        _iitem = IssueSearcher.GetNextIssue(_publication, today.Subtract(new TimeSpan(1, 0, 0, 0)));
            //        if (_iitem != null)
            //        {
            //            param.LocationIds = _iitem.ID.ToString();
            //        }
            //    }
            //    else
            //    {
            //        param.LocationIds = new ID(issue).ToString();
            //    }
            //}
            //else if (!string.IsNullOrEmpty(query.PublicationIdValue))
            //{
            //    param.LocationIds = pubId.ToString();
            //}

            //if (query.StartDate != null && query.EndDate != null)
            //{
            //    param.DateRanges.Add(
            //        new DateRangeSearchParam.DateRangeField(
            //            ArticleIndex.SortDateField,
            //            query.StartDate.Value,
            //            query.EndDate.Value//.AddDays(1) //making search inclusive to the query's enddate
            //            ));
            //}
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                string searchPageId = new ItemReferences().VwbSearchPage.ToString().ToLower().Replace("{", "").Replace("}", "");
                string url = string.Format("http://{0}/api/informasearch?pId={1}&sortBy=plannedpublishdate&sortOrder=desc", WebUtil.GetHostName(), searchPageId);

                DateTime startDate;
                DateTime endDate;
                if (query.StartDate != null && query.EndDate != null)
                {
                    startDate = query.StartDate ?? DateTime.MinValue;
                   

                    endDate = query.EndDate ?? DateTime.MaxValue;
                    
                }
                else
                {
                    startDate = DateTime.Now;
                    endDate = DateTime.Now.AddDays(30);
                }

                url += "&plannedpublishdate=" + startDate.ToString("MM/dd/yyyy");
                url += ";" + endDate.ToString("MM/dd/yyyy");

                var client = new WebClient();
                var content = client.DownloadString(url);

                var results = JsonConvert.DeserializeObject<SearchResults>(content);

                var resultItems = new List<ArticleItem>();

                foreach (var searchResult in results.results)
                {
                    var theItem = (ArticleItem)Sitecore.Context.Database.GetItem(searchResult.ItemId);

                    if (theItem == null)
                    {
                        continue;
                    }

                    resultItems.Add(theItem);
                }

                articles = ArticleItemWrapper.GetArticleItemProxies(resultItems).ToList();


            }
            if (query.SortColumnKey != null && ColumnFactory.GetColumn(query.SortColumnKey) != null)
            {
                articles.Sort(ColumnFactory.GetColumn(query.SortColumnKey));
            }
            if (query.SortColumnKey == ColumnFactory.GetArticleNumberColumn().Key())
            {
                articles.Sort(ColumnFactory.GetArticleNumberColumn());
            }
            if (query.SortColumnKey == ColumnFactory.GetTitleColumn().Key())
            {
                articles.Sort(ColumnFactory.GetTitleColumn());
            }
            if (query.Descending)
            {
                articles.Reverse();
            }
            if (query.NumResultsValue != null)
            {
                return articles.GetRange(0, Math.Min(articles.Count, (int)query.NumResultsValue));
            }

            return articles;
		}

		private void InitializeColumns(VwbQuery query)
		{
			Columns = query.ColumnKeysInOrder != null 
			          	? ColumnFactory.GetColumns(query.ColumnKeysInOrder).ToList()
			          	: new List<IVwbColumn>();
			articleNumberColumn = ColumnFactory.GetArticleNumberColumn();
			titleColumn = ColumnFactory.GetTitleColumn();
			Columns.Insert(0, titleColumn);
			Columns.Insert(0, articleNumberColumn);
		}

		private void BuildHeaderRows(Table report)
		{
			var header = new TableRow
			             	{
			             		CssClass = "tableheader"
			             	};
			foreach (var col in Columns)
			{
				TableCell tableCell = GetTableHeaderCell(col);
				header.Cells.Add(tableCell);
			}
			report.Rows.Add(header);
			header = new TableRow
			         	{
			         		CssClass = "sort-move"
			         	};
			TableCell cell = GetImmutableSubHeaderCell(articleNumberColumn);
			header.Cells.Add(cell); //add cell for article number column
			cell = GetImmutableSubHeaderCell(titleColumn);
			header.Cells.Add(cell); //add cell for title column
			foreach(var col in Columns)
			{
				if (articleNumberColumn == col || titleColumn == col) continue;
				TableCell tableCell = GetTableSubHeaderCell(col);
				header.Cells.Add(tableCell);
			}
			report.Rows.Add(header);
		}

		/// <summary>
		/// Immutable subheader cells do not have controls to remove
		/// or move them.
		/// Article Number subheader cell and Title fall under this distinction
		/// </summary>
		/// <returns></returns>
		private TableCell GetImmutableSubHeaderCell(IVwbColumn column)
		{
			var cell = new TableCell();
			var link = new HyperLink {CssClass = "right space" };
			LinkButtonToSortColumn(link, column);
			cell.Controls.Add(link);
			return cell;
		}

		private TableCell GetTableSubHeaderCell(IVwbColumn col)
		{
			var tableCell = new TableCell();
			IVwbColumn cur = col;
			AddMoveButtons(tableCell, cur);

			var link = new HyperLink { CssClass = "right space" };
			LinkButtonToSortColumn(link, col);
			tableCell.Controls.Add(link); 
			
			return tableCell;
		}

		private void AddMoveButtons(TableCell tableCell, IVwbColumn column)
		{
			if (column == articleNumberColumn || column == titleColumn) return;
			
			if (_query.ColumnKeysInOrder.Count() > 0)
			{
				if (column.Key() != _query.ColumnKeysInOrder.FirstOrDefault())
				{
					var link = new HyperLink {CssClass = "moveleft"};
					var query = _query.Clone();
					query.MoveColumnLeft(column.Key());
					link.Attributes.Add("href", GetUrlForQuery(query));
					tableCell.Controls.Add(link);
				}
				if (column.Key() != _query.ColumnKeysInOrder.Last())
				{
					var link = new HyperLink {CssClass = "moveright"};
					var query = _query.Clone();
					query.MoveColumnRight(column.Key());
					link.Attributes.Add("href", GetUrlForQuery(query));
					tableCell.Controls.Add(link);
				}
			}
		}

		private string GetUrlForQuery(VwbQuery query)
		{
			return _page.Request.ServerVariables["URL"] + "?" + query.GetQueryString();
		}

		private void LinkButtonToSortColumn(HyperLink link, IVwbColumn column)
		{
			var q = _query.Clone();
			if(q.SortColumnKey == column.Key())
			{
				q.Descending = !_query.Descending;
			}
			else
			{
				q.SortColumnKey = column.Key();
				q.Descending = false;
			}
			var clas = q.Descending ? " descending-active" : null;
			link.CssClass += clas;
			link.Attributes.Add("href", GetUrlForQuery(q));
		}

		private TableCell GetTableHeaderCell(IVwbColumn col)
		{
			var tableCell = new TableCell
			                      	{
			                      		Text = col.GetHeader()
			                      	};
			tableCell.Controls.Add(new Label
			                       	{
			                       		Text = col.GetHeader()
			                       	});
			if (col != articleNumberColumn && col != titleColumn)
			{
				var linkButton = new HyperLink {CssClass = "right space" };
				var q = _query.Clone();
				q.ColumnKeysInOrder.Remove(col.Key());
				linkButton.Attributes.Add("href", _page.Request.ServerVariables["URL"] + "?" + q.GetQueryString());
				tableCell.Controls.Add(linkButton); 
			}
			return tableCell;
		}
	}
}