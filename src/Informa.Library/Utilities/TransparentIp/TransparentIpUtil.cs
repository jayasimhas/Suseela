using Informa.Library.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Informa.Library.Utilities.TransparentIp
{
    public static class TransparentIpUtil
    {
        public static string transparentIpAddress = "192.168.123.123";

        public static bool IsInIpRange()
        {
            IPAddress current = IPAddress.Parse(GetCurrentRequestIp());
            IPAddress from = IPAddress.Parse(Sitecore.Configuration.Settings.GetSetting("TransparentIp.From"));
            IPAddress to = IPAddress.Parse(Sitecore.Configuration.Settings.GetSetting("TransparentIp.To"));

            IpAddressRangeCheck ipAddressRangeCheck = new IpAddressRangeCheck();
            bool isValid = ipAddressRangeCheck.IsInRange(current, from, to);

            return isValid;
        }

        public static string GetCurrentRequestIp()
        {
            // Need to use - Request.UserHostAddress
            // Using the IP from config for demo.

            // string ip = Request.UserHostAddress;

           // string ip = Sitecore.Configuration.Settings.GetSetting("SpoofedIp");
            string ip = getIpFromXmlFile();

            return ip;
        }

        private static string getIpFromXmlFile()
        {
            string ip = string.Empty;

            var doc = XDocument.Load("d:\\src\\ts3\\ip\\ip.xml");

            ip = doc.Element("ip").Value;

            if (string.IsNullOrEmpty(ip))
            {
                ip = "127.0.0.1";  // returning loopback ip
            }

            return ip;
        }
    }
}
