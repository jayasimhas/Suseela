﻿using Informa.Library.Services.ExternalFeeds;
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
                try
                {
                    string feedUrl = string.Format(feed, areaCode, movementType);
                    return CompanyResultService.GetCompanyFeeds(feed).Result;
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }
            else
            {
                return System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Views/Casualty/ShippingMovements.json"));
            }
        }

        //GET: Account/JsonDataFromFeed  
        public string ReadJsonMarketFixture(string dateVal, string feedUrl)
        {
            if (!string.IsNullOrEmpty(feedUrl))
            {
                try
                {
                    string fixturefeedUrl = string.Format(feedUrl, dateVal);
                    return CompanyResultService.GetCompanyFeeds(fixturefeedUrl).Result;
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
