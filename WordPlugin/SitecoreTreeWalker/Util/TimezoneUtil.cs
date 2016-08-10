using InformaSitecoreWord.Sitecore;
using PluginModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformaSitecoreWord.Util
{
    public static class TimezoneUtil
    {
        private static TimeZoneInfo _serverTimezone;
        private static DateTime _serverTimezoneAge;
        public static TimeZoneInfo ServerTimezone
        {
            get
            {
                if (_serverTimezone == null || (DateTime.Now - _serverTimezoneAge).TotalMinutes > 10)
                {
                    try
                    {
                        _serverTimezone = SitecoreClient.GetServerTimezone();
                    }
                    catch (Exception ex)
                    {
                        Globals.SitecoreAddin.Log("TimezoneUtil.ServerTimezone.Get: " + ex.ToString());
                        _serverTimezone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                    }
                    _serverTimezoneAge = DateTime.Now;
                }

                return _serverTimezone;
            }
        }

        #region ToLocalTimezones
        public static void ConvertArticleDatesToLocalTimezone(ArticleStruct article)
        {
            if (article == null)
                return;

            if (article.PrintPublicationDate != DateTime.MinValue && article.PrintPublicationDate != DateTime.MaxValue)
                article.PrintPublicationDate = TimeZoneInfo.ConvertTime(article.PrintPublicationDate, ServerTimezone, TimeZoneInfo.Local);

            if (article.WebPublicationDate != DateTime.MinValue && article.WebPublicationDate != DateTime.MaxValue)
                article.WebPublicationDate = TimeZoneInfo.ConvertTime(article.WebPublicationDate, ServerTimezone, TimeZoneInfo.Local);

            article.WordDocLastUpdateDate = ConvertDateToLocalTimezone(article.WordDocLastUpdateDate);
        }

        public static void ConvertArticleDatesToLocalTimezone(ArticlePreviewInfo article)
        {
            if (article == null)
                return;

            if (article.Date != DateTime.MinValue && article.Date != DateTime.MaxValue)
                article.Date = TimeZoneInfo.ConvertTime(article.Date, ServerTimezone, TimeZoneInfo.Local);
        }

        public static DateTime ConvertDateToLocalTimezone(DateTime dateOrig)
        {
            return TimeZoneInfo.ConvertTime(dateOrig, ServerTimezone, TimeZoneInfo.Local);
        }

        public static string ConvertDateToLocalTimezone(string dateOrig)
        {
            DateTime dt;
            if (DateTime.TryParse(dateOrig, out dt))
            {
                return TimeZoneInfo.ConvertTime(dt, ServerTimezone, TimeZoneInfo.Local).ToString();
            }

            return dateOrig;
        }
        #endregion  

        #region To Server Timezones
        public static void ConvertArticleDatesToServerTimezone(ArticleStruct article)
        {
            if (article == null)
                return;

            if (article.PrintPublicationDate != DateTime.MinValue && article.PrintPublicationDate != DateTime.MaxValue)
                article.PrintPublicationDate = TimeZoneInfo.ConvertTime(article.PrintPublicationDate, ServerTimezone);

            if (article.WebPublicationDate != DateTime.MinValue && article.WebPublicationDate != DateTime.MaxValue)
                article.WebPublicationDate = TimeZoneInfo.ConvertTime(article.WebPublicationDate, ServerTimezone);

            article.WordDocLastUpdateDate = ConvertDateToServerTimezone(article.WordDocLastUpdateDate);
        }

        public static void ConvertArticleDatesToServerTimezone(ArticlePreviewInfo article)
        {
            if (article == null)
                return;

            if (article.Date != DateTime.MinValue && article.Date != DateTime.MaxValue)
                article.Date = TimeZoneInfo.ConvertTime(article.Date, ServerTimezone);
        }

        public static DateTime ConvertDateToServerTimezone(DateTime dateOrig)
        {
            return TimeZoneInfo.ConvertTime(dateOrig, ServerTimezone);
        }

        public static string ConvertDateToServerTimezone(string dateOrig)
        {
            DateTime dt;
            if (DateTime.TryParse(dateOrig, out dt))
            {
                return TimeZoneInfo.ConvertTime(dt, ServerTimezone).ToString();
            }

            return dateOrig;
        }
        #endregion
    }
}
