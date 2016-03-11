using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Informa.Library.DCD.XMLImporting
{
    public static class XMLFileUtilities
    {
        public static bool IsOpen(string fileName)
        {
            if (!File.Exists(fileName))
                return false;
            try
            {
                using (File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None)) ;
            }
            catch
            {
                return true;
            }
            return false;
        }

        public static string SanitizeXmlString(string xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }

            //convert the encoding from windows-1252 to utf8
            Encoding windows = Encoding.GetEncoding("Windows-1252");
            Encoding utf8 = Encoding.UTF8;

            byte[] isoBytes = windows.GetBytes(xml);
            byte[] utfBytes = Encoding.Convert(windows, utf8, isoBytes);
            string line = utf8.GetString(utfBytes);

            StringBuilder buffer = new StringBuilder(line.Length);

            foreach (char c in line)
            {
                if (isLegalXmlChar(c))
                {
                    buffer.Append(c);
                }
            }

            return buffer.ToString();
        }

        private static bool isLegalXmlChar(int character)
        {
            return
            (
                 character == 0x9 /* == '\t' == 9   */          ||
                 character == 0xA /* == '\n' == 10  */          ||
                 character == 0xD /* == '\r' == 13  */          ||
                (character >= 0x20 && character <= 0xD7FF) ||
                (character >= 0xE000 && character <= 0xFFFD) ||
                (character >= 0x10000 && character <= 0x10FFFF)
            );
        }

        public static string RemoveBrackets(string input)
        {
            if (input == null)
            {
                return null;
            }
            return Regex.Replace(input, "[\\[\\]\\{\\}]", "");
        }
    }
}
