using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;

namespace SitecoreTreeWalker.Util
{
	public class BoldTextFinder
	{
		public static List<String> GetBoldText(Microsoft.Office.Interop.Word.Document document)
		{
			var boldies = new List<string>();
			string currentBoldy = "";
			foreach (Range car in document.Characters)
			{
				if (car == null || car.Text.Contains("\a") 
					|| car.Text.Contains("\n") || car.Text.Contains("\r"))
				{
					AddBoldRange(boldies, currentBoldy);
					currentBoldy = "";
					continue;
				}
				if(car.Font.Bold == -1)
				{
					currentBoldy += car.Text;
				}
				else
				{
					AddBoldRange(boldies, currentBoldy);
					currentBoldy = "";
				}
			}
			AddBoldRange(boldies, currentBoldy);
			return boldies;
		}

		private static void AddBoldRange(List<string> boldies, string currentBoldy)
		{
			if(currentBoldy != "")
			{
				var temp = currentBoldy.Trim();
				if (!boldies.Contains(temp))
				{
					boldies.Add(temp);
				}
			}
		}
	}
}
