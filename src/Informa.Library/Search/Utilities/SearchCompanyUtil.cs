using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Models.DCD;
using Sitecore.Diagnostics;

namespace Informa.Library.Search.Utilities
{
    public class SearchCompanyUtil
    {

        public static List<string> GetCompanyNames(string recordNumbers)
        {
            if (string.IsNullOrEmpty(recordNumbers.Trim()))
            {
                return new List<string>();
            }

            List<string> companyNames = new List<string>();
            if (recordNumbers.Contains(","))
            {
                string[] numbers = recordNumbers.Split(',');

                foreach (string number in numbers)
                {
                    Models.DCD.Company company = GetCompanyByRecordNumber(number.Trim());

                    if (company == null)
                    {
                        continue;
                    }

                    companyNames.Add(company.Title.Trim());
                }
            }
            else
            {
                Models.DCD.Company company = GetCompanyByRecordNumber(recordNumbers.Trim());

                if (company != null)
                {
                    companyNames.Add(company.Title.Trim());
                }
            }

 
            return companyNames;
        }

        public static Models.DCD.Company GetCompanyByRecordNumber(string recordNumber)
        {
            if (!Sitecore.Configuration.Settings.ConnectionStringExists("dcd"))
            {
                return null;
            }

            try
            {
                Models.DCD.Company company = null;
                using (DCDContext dContext = new DCDContext(Sitecore.Configuration.Settings.GetConnectionString("dcd")))
                {
                    company = dContext.Companies.FirstOrDefault(deal => deal.RecordNumber.Trim() == recordNumber.Trim());
                }

                return company;
            }
            catch (Exception exc)
            {
                Log.Error("Error Trying to Get Company From DCD Database: " + exc.Message, "SearchCompanyUtil.GetCompanyByRecordNumber");
            }

            return null;
        }
    }
}
