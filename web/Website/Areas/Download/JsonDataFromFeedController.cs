using Informa.Library.Services.ExternalFeeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Informa.Web.Areas.Download
{
    public class JsonDataFromFeedController : Controller
    {
        protected readonly ICompaniesResultService CompanyResultService;
        public JsonDataFromFeedController(ICompaniesResultService companyResultService)
        {
            CompanyResultService = companyResultService;
        }
        // GET: Account/JsonDataFromFeed
        public string ReadJsonShippingMovements(string feed, string area, string movementType)
        {
            if (!string.IsNullOrEmpty(feed) && !string.IsNullOrEmpty(area) && !string.IsNullOrEmpty(movementType))
            {
                string feedUrl = string.Format(feed, area, movementType);
                return CompanyResultService.GetCompanyFeeds(feed).Result;
            }
            else
            {
                return System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Views/Casualty/ShippingMovements.json"));
            }
        }
    }
}
