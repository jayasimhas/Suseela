using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Newtonsoft.Json.Serialization;

namespace Informa.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
			// Web API configuration and services
			// This is now configured in RegisterHttpRoutes.cs
		}
    }
}
