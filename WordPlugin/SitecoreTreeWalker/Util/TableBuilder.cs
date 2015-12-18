using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Office.Interop.Word;

namespace SitecoreTreeWalker.Util
{
	public class TableBuilder
	{
		public Tables Tables;
		private bool[] _retrieved;
		public TableBuilder(Tables tables)
		{
			Tables = tables;
			_retrieved = new bool[tables.Count];
		}

		public Table GetTable(int index)
		{
			//does not work
			//return Tables[index];
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

		private float SmallestCellWidth(Table table)
		{
			float width = float.PositiveInfinity;
			foreach(Cell cell in table.Range.Cells)
			{
				if(cell.Width < width)
				{
					width = cell.Width;
				}
			}

			return width;
		}

		//private float TableWidth(Table table)
		//{
		//    int colIndex = 1;
		//    float width = 0;
		//    try
		//    {
		//        while (true)
		//        {	//major hacks because documentation is lacking
		//            and i can't find another solution

		//            table.Rows is inaccessble if there are merged cells
		//            across rows

		//            table.Columns is inaccessible if there are merged
		//            cells across columns

		//            so how do i find out the number of cells in a table row?
		//            WE KEEP GOING TILL WE BUST
		//            width += table.Cell(1, colIndex).Width;
		//            colIndex++;
		//        }
		//    } catch
		//    {
		//        return width;
		//    }
			
		//}

		public XElement ParseTable(int index)
		{
			Table table = GetTable(index);
			_retrieved[index] = true;
			var root = new XElement("table");
			root.SetAttributeValue("class", "data");
			var tbody = new XElement("tbody");
			root.Add(tbody);
			//var rows = table.Rows;
			var wordUtils = new WordUtils();
			var tableAnalyzer = new TableAnalyzer(table);
			const int maxTableWidth = 544;
			decimal curSetWidth = Math.Truncate(tableAnalyzer.TableCellWidthSums.Last());
			if (curSetWidth < maxTableWidth)
			{
				root.SetAttributeValue("width", curSetWidth);
			}
			bool first = true;
			for (int r = 1; r <= table.Rows.Count; r++ )
			{
				//Row row = rows[r];
				var currentRow = new XElement("tr");
				tbody.Add(currentRow);
				//var rowCellEnumerator = row.Cells.Cast<Cell>().GetEnumerator();
				decimal currentWidthSum = 0;
				int numColSpansUsed = 0;
				//while (rowCellEnumerator.MoveNext())
				for (int c = 1; c <= table.Columns.Count; c++)
				{
					try
					{
						var curCell = table.Cell(r, c); //rowCellEnumerator.Current);
						curCell.Select();
						var app = curCell.Application;
						string cellType = first ? "th" : "td";
						var xmlCell = new XElement(cellType);
						currentRow.Add(xmlCell);
						Paragraphs paragraphs = curCell.Range.Paragraphs;
						var tableBuilder = new TableBuilder(curCell.Tables);
						XNode currentDescendent = wordUtils.ParagraphsToXml(paragraphs, tableBuilder).FirstNode;
						while (currentDescendent != null)
						{
							xmlCell.Add(currentDescendent);
							currentDescendent = currentDescendent.NextNode;
						}
						currentWidthSum += (decimal)curCell.Width;
						int widthIndex = tableAnalyzer.TableCellWidthSums.IndexOf(currentWidthSum);
						int curNumCol = widthIndex + 1 - numColSpansUsed;
						xmlCell.SetAttributeValue("colspan", curNumCol);
						xmlCell.SetAttributeValue("rowspan",
							app.Selection.Information[WdInformation.wdEndOfRangeRowNumber] -
							app.Selection.Information[WdInformation.wdStartOfRangeRowNumber] + 1);
						decimal cellWidth = 0;
						for (int i = widthIndex; i >= widthIndex - curNumCol + 1; i--)
						{
							cellWidth += tableAnalyzer.CellWidthPercentages[i];
						}
						xmlCell.SetAttributeValue("width", cellWidth + "%");
						numColSpansUsed += curNumCol;
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
								int widthIndex = tableAnalyzer.TableCellWidthSums.IndexOf(currentWidthSum);
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
					//var colspan = (int)(curCell.Width / smallestWidth);
					//if (colspan > 1) xmlCell.SetAttributeValue("colspan", colspan);
				}
				first = false;
			}
			return root;
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
