using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using Informa.Library.Rss;
using Informa.Library.Search.TypeAhead;
using Informa.Library.Utilities.References;
using Jabberwocky.Core.Caching;
using Newtonsoft.Json;
using Sitecore.Web;

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
        //private readonly ICacheProvider _cache;

        //public TypeAhead(ICacheProvider cache)
        //{
        //    _cache = cache;
        //}

        [WebMethod]
        public void TypeAheadCompanies()
        {
            List<CompanyTypeAheadResponseItem> companies = new List<CompanyTypeAheadResponseItem>();

            var company1 = new CompanyTypeAheadResponseItem("Acme");
            var company2 = new CompanyTypeAheadResponseItem("Velir");
            var company3 = new CompanyTypeAheadResponseItem("Bioware");

            companies.Add(company1);
            companies.Add(company2);
            companies.Add(company3);

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(new JavaScriptSerializer().Serialize(companies));

        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void TypeAheadCompaniesFromSearch()
        {
            List<CompanyTypeAheadResponseItem> companies = new List<CompanyTypeAheadResponseItem>();

            string searchPageId = new ItemReferences().SearchPage.ToString().ToLower().Replace("{", "").Replace("}", "");
            string url = string.Format("http://{0}/api/informasearch?pId={1}", WebUtil.GetHostName(), searchPageId);

            url = string.Format("{0}&sortBy=relevance&sortOrder=asc", url);
            

            var client = new WebClient();
            var content = client.DownloadString(url);

            var results = JsonConvert.DeserializeObject<SearchResults>(content);

            foreach (var facetGroupResult in results.facets)
            {
                if (facetGroupResult.id.ToLower() == "companies")
                {
                    foreach (SearchFacetResult result in facetGroupResult.values)
                    {
                        if (!string.IsNullOrEmpty(result.name))
                        {
                            companies.Add(new CompanyTypeAheadResponseItem(result.name));
                        }

                    }
                }
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(new JavaScriptSerializer().Serialize(companies));
       
        }
    }
}
