using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Logging
{
    public class SitecoreInfoLogger : IInfoLogger
    {
        public void Log(string message, string owner)
        {
            Sitecore.Diagnostics.Log.Info(message, owner);
        }
    }
}
