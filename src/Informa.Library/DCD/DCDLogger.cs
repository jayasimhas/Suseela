using log4net;
using Sitecore.Diagnostics;
using Sitecore.Services.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.DCD
{
    public static class DCDLogger
    {
        private static ILog _log;
        static DCDLogger()
        {
            _log = LogManager.GetLogger("DCDImportAppender");
        }

        public static void Debug(string message)
        {
            _log.Debug(message);
        }

        public static void Info(string message)
        {
            _log.Debug(message);
        }

        public static void Error(Exception ex)
        {
            _log.Debug(ex.ToString());
        }

        public static void Error(string message)
        {
            _log.Debug(message);
        }

        public static void Error(string message, Exception ex)
        {
            _log.Debug(message + ": " + ex.ToString());
        }
    }
}
