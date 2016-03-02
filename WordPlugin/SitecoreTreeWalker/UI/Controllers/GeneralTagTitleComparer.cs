using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InformaSitecoreWord.UI.Controllers
{
	class GeneralTagTitleComparer : IComparer<string>
	{
		private readonly string _substring;

		public GeneralTagTitleComparer(string substring)
		{
			_substring = substring;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns>A negative number if the substring appears earlier in string x
		/// A positive number if the substring appears earlier in string y
		/// If the substring appears at the same index, a negative number if string x has fewer characters
		/// If the substring appears at the same index, a positive number if string y has fewer characters
		/// 0 if the substring appears at the same index for both string x and string y and they both have the same length</returns>
		public int Compare(string x, string y)
		{
			if (x == null)
			{
				if (y == null)
				{
					return 0;
				}

				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			string xname = x.ToLower();
			string yname = y.ToLower();
			int retVal = xname.IndexOf(_substring).CompareTo(yname.IndexOf(_substring));
			if (retVal != 0)
			{
				return retVal;
			}
			//if the strings have the substring at the same index
			//compare the lengths of the strings instead
			return x.Length - y.Length;
		}
	}
}
