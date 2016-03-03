using Informa.Model.DCD;
using Informa.Models.DCD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Informa.Library.Utilities.TokenMatcher
{
    public static class DCDTokenMatchers
    {
        public static string ProcessDCDTokens(string text)
        {
            string body = text;

            try
            {
                string tempText = processCompnayTokens(body);

                if (string.IsNullOrEmpty(tempText) == false)
                    body = tempText;

                tempText = processDealTokens(body);

                if (string.IsNullOrEmpty(tempText) == false)
                    body = tempText;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error ProcessingDCDTokens", ex);
            }
            return body;
        }

        private static string processCompnayTokens(string text)
        {
            //Find all matches with Company token
            Regex regex = new Regex(DCDConstants.CompanyTokenRegex);

            MatchEvaluator evaluator = new MatchEvaluator(companyMatchEval);
            return regex.Replace(text, evaluator);
        }

        private static string processDealTokens(string text)
        {
            //Find all matches with Deal token
            Regex regex = new Regex(DCDConstants.DealTokenRegex);

            MatchEvaluator evaluator = new MatchEvaluator(dealMatchEval);
            return regex.Replace(text, evaluator);
        }

        private static string companyMatchEval(Match match)
        {
            try
            {
                //return a strong company name (from the token itself) to replace the token
                return string.Format("<strong>{0}</strong>", match.Groups[1].Value.Split(':')[1]);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error when evaluating company match token", ex, "LogFileAppender");
                return string.Empty;
            }
        }

        private static string dealMatchEval(Match match)
        {
            try
            {
                Deal deal = new DCDManager().GetDealByRecordNumber(match.Groups[1].Value);

                //return a strong company name (from the token itself) to replace the token
                return string.Format("[<a href=\"{0}\">See Deal</a>]", string.Format(Sitecore.Configuration.Settings.GetSetting("DCD.OldDealsURL"), deal.RecordNumber));
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error when evaluating deal match token", ex, "LogFileAppender");
                return string.Empty;
            }
        }
    }
}
