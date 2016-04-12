using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Search.TypeAhead
{
    public class CompanyTypeAheadResponseItem
    {
        public CompanyTypeAheadResponseItem()
        {
            CompanyName = "";
        }

        public CompanyTypeAheadResponseItem(string companyName)
        {
            CompanyName = companyName;
        }

        public string CompanyName { get; set; }
    }
}
