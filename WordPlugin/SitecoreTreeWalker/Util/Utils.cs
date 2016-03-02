using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace InformaSitecoreWord.Util
{
	public class Utils
	{ 
	}

	public static class EnumerableExtensions
	{
		public static void AddIfNotThere<T>(this List<T> list, T o)
		{
			if (!list.Contains(o))
			{ list.Add(o); }
		}
	}

	public static class StringExtensions
	{
		public static List<string> BlacklistCharacters = new List<string>
		            {
			       		"\a",
						"\b",
						"\f",
						"\n",
						"\r",
						"\t",
						"\v",
						"\r\a",
						"\0",
						Convert.ToChar(31).ToString()
			       	};
		public static string RinseMsChars(this String str)
		{
			var b = new StringBuilder(str);
			foreach (var car in BlacklistCharacters)
			{
				b.Replace(car, "");
			}
			return b.ToString();
		}

		public static bool IsNullOrEmpty(this string s)
		{
			return String.IsNullOrEmpty(s);
		}

		public static bool IsGuid(this string s)
		{
			var isGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);
			bool isValid = false;
			if (s != null)
			{
				if (isGuid.IsMatch(s))
				{
					isValid = true;
				}
			}
			return isValid;
		}

		public static string TextAfterString(this string s, string delim)
		{
			if (s.IndexOf(delim) < 0)
			{ return s; }

			return s.Substring(s.IndexOf(delim)).Remove(0, delim.Length);
		}
	}
}
