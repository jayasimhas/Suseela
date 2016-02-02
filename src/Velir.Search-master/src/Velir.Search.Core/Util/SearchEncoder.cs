using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Velir.Search.Core.Reference;

namespace Velir.Search.Core.Util
{
	public static class SearchEncoder
	{
		private static readonly Lazy<HashSet<char>> LazySpecialChars = new Lazy<HashSet<char>>(() => new HashSet<char>(SiteSettings.SpecialCharacters.Select(s => s[0])));
		private static ISet<char> SpecialCharacters { get { return LazySpecialChars.Value; } }

		public static string EncodeValue(string term)
		{
			var builder = new StringBuilder();

			foreach (var @char in term)
			{
				if (SpecialCharacters.Contains(@char))
				{
					builder.Append(SiteSettings.EscapeSequence);
				}
				builder.Append(@char);
			}

			return builder.ToString();
		}
	}
}
