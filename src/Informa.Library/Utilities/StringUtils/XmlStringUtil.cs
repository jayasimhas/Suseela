using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Utilities.StringUtils
{
    public class XmlStringUtil
    {
        public static string UnescapeXMLValue(string xmlString)
        {
            if (xmlString == null)
            {
                throw new ArgumentNullException("xmlString");
            }

            xmlString = xmlString.Replace("&apos;", "'");
            xmlString = xmlString.Replace("&quot;", "\"");
            xmlString = xmlString.Replace("&gt;", ">");
            xmlString = xmlString.Replace("&lt;", "<");
            xmlString = xmlString.Replace("&amp;", "&");

            return xmlString;
        }

        public static string EscapeXMLValue(string xmlString)
        {

            if (xmlString == null)
            {
                throw new ArgumentNullException("xmlString");
            }

            xmlString = xmlString.Replace("&", "&amp;");
            xmlString = xmlString.Replace("'", "&apos;");
            xmlString = xmlString.Replace("\"", "&quot;");
            xmlString = xmlString.Replace(">", "&gt;");
            xmlString = xmlString.Replace("<", "&lt;");

            return xmlString;
        }
    }
}
