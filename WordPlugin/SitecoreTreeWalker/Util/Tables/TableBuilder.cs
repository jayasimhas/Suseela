using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Office.Interop.Word;

namespace InformaSitecoreWord.Util.Tables
{
	public class TableBuilder
	{
		public Microsoft.Office.Interop.Word.Tables Tables;
		private readonly bool[] _retrieved;
		public TableBuilder(Microsoft.Office.Interop.Word.Tables tables)
		{
			Tables = tables;
			_retrieved = new bool[tables.Count];
		}

		public Table GetTable(int index)
		{
			//return Tables[index];
			//does not work
			IEnumerator enumer = Tables.GetEnumerator();
			for(int i = 0; i <= index; i++)
			{
				enumer.MoveNext();
			}

			return (Table)enumer.Current;
		}

		public bool HasRetrieved(int index)
		{
			return _retrieved[index];
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="table"></param>
		/// <returns>Zero-based index of table in TableBuilder; -1 if there is no such table</returns>
		public int GetIndex(Table table)
		{
			IEnumerator<Table> enumer = Tables.Cast<Table>().GetEnumerator();
			int index = -1;
			if(enumer.MoveNext())
			{
				index++;
				if(enumer.Current.Equals(table))
				{
					return index;
				}
			}
			return -1;
		}

		public XElement ParseTable(int index)
		{
			
			Table table = GetTable(index);
			_retrieved[index] = true;
			var root = new XElement("table");
			var tbody = new XElement("tbody");
			root.Add(tbody);
			var tableAnalyzer = new TableAnalyzer(table);

			//the table width value which represents a 100% width
			const int fullTableWidth = 514;
			decimal curSetWidth = Math.Truncate(tableAnalyzer.TableCellWidthSums.Last());
			root.SetAttributeValue("width", (curSetWidth/fullTableWidth*100) + "%");
            root.SetAttributeValue("data-mediaid", Guid.NewGuid().ToString("N"));
			ParseRows(tbody, table, tableAnalyzer);
			return root;
		}

		private void ParseRows(XElement tbody, Table table, TableAnalyzer tableAnalyzer)
		{
			List<decimal> cellWidthPercentages = tableAnalyzer.CellWidthPercentages;
			List<decimal> tableCellWidthSums = tableAnalyzer.TableCellWidthSums;
			for (int r = 1; r <= table.Rows.Count; r++ )
			{
				var currentRow = new XElement("tr");
				tbody.Add(currentRow);
				decimal currentWidthSum = 0;
				int numColSpansUsed = 0;
				for (int c = 1; c <= table.Columns.Count; c++)
				{
					
					try
					{
						var curCell = table.Cell(r, c); 
						curCell.Select();
						var app = curCell.Application;
						Selection selection = app.Selection;
						var xmlCell = new XElement(@"td");
						currentRow.Add(xmlCell);
						Paragraphs paragraphs = curCell.Range.Paragraphs;
						var tableBuilder = new TableBuilder(curCell.Tables);
						var paragraphsTransformer = new TableCellParagraphsTransformer(paragraphs);
						PopulateTableCell(xmlCell, paragraphsTransformer, tableBuilder);
						currentWidthSum += (decimal)curCell.Width;
						int widthIndex = tableCellWidthSums.IndexOf(currentWidthSum);
						int currentNumberOfColumns = widthIndex + 1 - numColSpansUsed;
						SetTableCellAttributes(xmlCell, widthIndex, currentNumberOfColumns, cellWidthPercentages, selection);
						numColSpansUsed += currentNumberOfColumns;
					}
					catch (System.Runtime.InteropServices.COMException)
					{ //it seems that the only way to tell if a cell at a particular [row, col] index
						//exists is to call table.Cell(row, col) and see if it throws an exception
						//the reason it may not exist is rowspans and colspans
						int nr = r;
						while(nr > 0)
						{
							try
							{
								currentWidthSum += (decimal)table.Cell(nr, c).Width;
								int widthIndex = tableCellWidthSums.IndexOf(currentWidthSum);
								int curNumCol = widthIndex + 1 - numColSpansUsed;
								numColSpansUsed += curNumCol;
								break;
							}
							catch(System.Runtime.InteropServices.COMException)
							{
								nr--;
							}
						}
					}
				}
			}
		}

		private void SetTableCellAttributes(XElement tableCell, int widthIndex, int curNumCol, List<decimal> cellWidthPercentages, Selection selection)
		{
			tableCell.SetAttributeValue("colspan", curNumCol);
			tableCell.SetAttributeValue("rowspan", GetRowspan(selection));
						
			decimal cellWidth1 = 0;
			for (int i = widthIndex; i >= widthIndex - curNumCol + 1; i--)
			{
				cellWidth1 += cellWidthPercentages[i];
			}
			decimal cellWidth = cellWidth1;
			tableCell.SetAttributeValue("width", cellWidth + "%");
		}

		private void PopulateTableCell(XElement tableCell, TableCellParagraphsTransformer paragraphsTransformer, TableBuilder tableBuilder)
		{
			XNode currentDescendent = paragraphsTransformer.Parse(tableBuilder).FirstNode;
			while (currentDescendent != null)
			{
				tableCell.Add(currentDescendent);
				currentDescendent = currentDescendent.NextNode;
			}
			if (paragraphsTransformer.CellStyle != null)
			{
				tableCell.SetAttributeValue("class", paragraphsTransformer.CellStyle); 
			}
		}

		private int GetRowspan(Selection selection)
		{
			return (int)selection.Information[WdInformation.wdEndOfRangeRowNumber] -
			       (int)selection.Information[WdInformation.wdStartOfRangeRowNumber] + 1;
		}

		public bool IsInATable(Range range)
		{
			return Tables.Cast<Table>().Any(t => range.InRange(t.Range));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="range"></param>
		/// <returns>Zero-based index of table; -1 if range is not in any table in Tables</returns>
		public int GetTableIndexFor(Range range)
		{
			int i = 0;
			foreach(Table t in Tables)
			{
				if(range.InRange(t.Range))
				{
					return i;
				}
				i++;
			}
			return -1;
		}
	}
}
