using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.ViewModels.FinanceCompanies
{
    public class CompanyModel
    {
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyLandingPageUrl { get; set; }
        public string CompanyGraphPageUrl { get; set; }
        public IEnumerable<AMGraph> Graphs { get; set; }
    }
}