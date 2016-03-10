using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Informa.Library.Search.TypeAhead;

namespace Informa.Web.velir.services
{
    /// <summary>
    /// Summary description for TypeAhead
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TypeAhead : System.Web.Services.WebService
    {

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<CompanyTypeAheadResponseItem> TypeAheadCompanies()
        {
            List<CompanyTypeAheadResponseItem> companies = new List<CompanyTypeAheadResponseItem>();

            var company1 = new CompanyTypeAheadResponseItem("Acme");
            var company2 = new CompanyTypeAheadResponseItem("Velir");
            var company3 = new CompanyTypeAheadResponseItem("Bioware");

            companies.Add(company1);
            companies.Add(company2);
            companies.Add(company3);

            return companies;
        }
    }
}
