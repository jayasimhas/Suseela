using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.SharedSource.DataImporter.Utility
{
    public static class DateTimeUtil
    {
        public static bool ParseInformaDate(string value, out DateTime date)
        {
			return DateTime.TryParseExact(value, new string[] { "yyyy-MM-dd HH:mm:ss", "yyyy-MM-ddTHH:mm:ss", "d/M/yyyy", "d/M/yyyy HH:mm:ss", "d/M/yyyy HH:mm:ss.fff", "yyyy-MM-dd HH:mm:ss.f", "yyyyMMdd", "yyMMdd", "yyyy" },
						   CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
		}
    }
}
