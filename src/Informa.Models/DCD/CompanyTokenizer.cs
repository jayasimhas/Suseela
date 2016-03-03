using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Informa.Models.DCD
{
    /// <summary>
	/// Class to help find companies in the database and get data on them.
	/// </summary>
	/// <remarks>This class should be the only place that the company database is accessed.</remarks>
	public static class CompanyTokenizer
    {
        /// <summary>
        /// Replaces all company names within strong elements with their token containing their 
        /// record ID
        /// 
        /// It used to remove the strong element, but now it keeps the strong element.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="companyIdsCsv"></param>
        /// <returns></returns>
        public static string ReplaceStrongCompanyNamesWithToken(string html, out string companyIdsCsv)
        {
            var xhtml = XElement.Parse(html);
            var strongs = xhtml.Descendants("strong");
            var ids = new List<string>();
            var elementReplacements = new List<KeyValuePair<XElement, string>>();
            using (var dc = new DCDContext())
            {
                foreach (var strong in strongs)
                {
                    if (strong == null || strong.Parent == null) continue;

                    Company company = dc.Companies.FirstOrDefault(c => c.Title == strong.Value.Trim());
                    string compID = string.Empty;
                    if (company != null)
                    {
                        compID = company.RecordNumber;
                    }
                    //var id = dc.GetCompanyId(strong.Value.Trim());
                    if (string.IsNullOrEmpty(compID) == false)
                    {
                        elementReplacements.Add(new KeyValuePair<XElement, string>(strong, String.Format(DCDConstants.CompanyTokenFormat, compID, strong.Value.Trim())));
                        ids.Add(compID);
                    }
                }
            }

            foreach (var replacement in elementReplacements)
            {
                if (replacement.Key == null || replacement.Key.Parent == null) continue;
                //replacement.Key.ReplaceWith(replacement.Value);
                replacement.Key.Value = replacement.Value;
            }
            companyIdsCsv = ids.Count > 0 ? ids.Aggregate((total, next) => total + "," + next) : "";
            return xhtml.ToString();
        }
    }
}
