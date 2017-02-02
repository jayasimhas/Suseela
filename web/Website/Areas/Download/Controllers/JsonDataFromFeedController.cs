using Informa.Library.Services.ExternalFeeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Informa.Web.Areas.Account.Models.User.Management;

namespace Informa.Web.Areas.Download.Controllers
{
    public class JsonDataFromFeedController : Controller
    {
        protected readonly ICompaniesResultService CompanyResultService;
        public JsonDataFromFeedController(ICompaniesResultService companyResultService)
        {
            CompanyResultService = companyResultService;
        }
        // GET: Account/JsonDataFromFeed               
        public string ReadJsonShippingMovements(string feed, string areaCode, string movementType)
        {
            if (!string.IsNullOrEmpty(feed) && !string.IsNullOrEmpty(areaCode) && !string.IsNullOrEmpty(movementType))
            {
                string feedUrl = string.Format(feed, areaCode, movementType);
                return CompanyResultService.GetCompanyFeeds(feed).Result;
            }
            else
            {
                return System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Views/Casualty/ShippingMovements.json"));
            }
        }

        //GET: Account/JsonDataFromFeed  
        public string ReadJsonMarketFixture(string dateVal, string feedUrl)
        {
            if (!string.IsNullOrEmpty(feedUrl) )
            {
                string fixturefeedUrl = string.Format(feedUrl);
                return CompanyResultService.GetCompanyFeeds(fixturefeedUrl).Result;
            }
            else
            {
                if(dateVal == "13-Jan-17")
                    return System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Views/Casualty/MarketFixtureTable.json"));
                else if(dateVal == "31-Dec-16")
                    return System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Views/Casualty/marketFixDummyData.json"));
                else
                    return System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Views/Casualty/MarketFixtureDummy.json"));
            }
        }
    }
}
