using System;
using System.Linq;
using System.Net;
using Jabberwocky.Glass.Autofac.Attributes;
using log4net;
using System.Text;
using System.IO;

namespace Informa.Library.DataTools
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class TableauUtil : ITableauUtil
    {
        private ILog _logger;
        public TableauUtil()
        {
            _logger = LogManager.GetLogger("LogFileAppender");
        }
        public string GenerateSecureTicket(string serverName, string userName)
        {
            var request = (HttpWebRequest)WebRequest.Create(string.Format("{0}trusted", serverName));

            var data = Encoding.ASCII.GetBytes(string.Format("username={0}&server={1}", userName, serverName));

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            var token = string.Empty;
            try
            {
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                token = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (Exception e)
            {
                _logger.Error("Tableu Secure token is not created:", e);
                return token;
            }
            return token;
        }
    }
}

