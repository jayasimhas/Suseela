using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using InformaSitecoreWord.UI.TreeBrowser.TreeBrowserControls;
using Microsoft.Office.Interop.Word;

namespace InformaSitecoreWord.Util.Document
{
	public class ReferencedDealParser
	{
		public static List<string> GetReferencedDeals(Microsoft.Office.Interop.Word.Document document)
		{
			var deals = new List<string>();
			foreach(var current in document.Hyperlinks)
			{
				var hyperlink = current as Hyperlink;
				if(hyperlink == null)
				{
					break;
				}
				if(DealsDrugsCompaniesControl.IsADealHyperlink(hyperlink))
				{
                    if (!string.IsNullOrEmpty(hyperlink.TextToDisplay))
                    {
                        string deal = Regex.Match(hyperlink.TextToDisplay, DealsDrugsCompaniesControl.DealRegex).Groups[1].ToString();
                        if (!deals.Contains(deal))
                        {
                            deals.Add(deal);
                        }
                    }
                    else
                    {
                        string deal = Regex.Match(hyperlink.Range.Text, DealsDrugsCompaniesControl.DealRegex).Groups[1].ToString();
                        if (!deals.Contains(deal))
                        {
                            deals.Add(deal);
                        }
                    }
				}
			}
			return deals;
		} 
	}
}
