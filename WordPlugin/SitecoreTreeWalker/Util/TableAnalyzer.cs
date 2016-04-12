using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;

namespace InformaSitecoreWord.Util
{
	class TableAnalyzer
	{
		public Table Table;
		public List<decimal> TableCellWidthSums
		{
			get { return _tableCellWidthSums; }
		}
		public decimal TotalWidth
		{
			get { return _totalWidth; }
		}

		public List<decimal> TableCellWidths
		{
			get { return _tableCellWidths; }
		}

		public List<decimal> CellWidthPercentages
		{
			get { return _cellWidthPercentages;  }
		}

		private readonly List<decimal> _cellWidthPercentages;
		private readonly List<decimal> _tableCellWidths;

		private readonly List<decimal> _tableCellWidthSums;
		private readonly decimal _totalWidth;
		public TableAnalyzer(Table table)
		{
			Table = table;
			_tableCellWidthSums = GetTableCellWidthSums();
			_tableCellWidths = new List<decimal>();
			_tableCellWidthSums.Sort();
			foreach (decimal widthSum in _tableCellWidthSums)
			{
				decimal thatWidth = widthSum - _totalWidth;
				_tableCellWidths.Add(thatWidth);
				_totalWidth += thatWidth;
			}
			_cellWidthPercentages = new List<decimal>();
			foreach (decimal width in _tableCellWidths)
			{
				_cellWidthPercentages.Add((width/TableCellWidthSums.Last()) * 100);
			}
		}

		private List<decimal> GetTableCellWidthSums()
		{
			var widthSums = new List<decimal>();
			int rowIndex = 1;
			//try
			//{
				while(rowIndex <= Table.Rows.Count)
				{
					RowCellWidthSums(rowIndex).ForEach(f => widthSums.AddIfNotThere(f));
					rowIndex++;
				}
			//}
			//catch (Exception)
			//{
				return widthSums;
			//}
		}

		private List<decimal> RowCellWidthSums(int rowIndex)
		{
			var widthSums = new List<decimal>();
			decimal sum = 0;
			//int colIndex = 1;
			for (int col = 1; col <= Table.Columns.Count; col++ )
			{	//major hacks because documentation is lacking
				//and i can't find another solution

				//table.Rows[] is inaccessble if there are merged cells
				//across rows

				//table.Columns[] is inaccessible if there are merged
				//cells across columns

				//so how do i find out the number of cells in a particular table row?
				//WE KEEP GOING TILL WE BUST
				try
				{
					var curWidth = (decimal)Table.Cell(rowIndex, col).Width;
					sum = (decimal)(curWidth + (decimal)sum);
					//sum = (float)Math.Round(sum, 1);

					widthSums.Add(sum);
				}
				catch (System.Runtime.InteropServices.COMException)
				{

					continue;
				}
			}
			
			return widthSums;
		}
	}
}
