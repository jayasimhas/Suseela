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
                string firstStepText = processCompnayTokens(body);

                if (string.IsNullOrEmpty(firstStepText) == false)
                    body = firstStepText;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error ProcessingDCDTokens", ex);
            }
            return body;
        }

        private static string processCompnayTokens(string text)
        {
            //Find all matches with Deal token
            Regex regex = new Regex(DCDConstants.CompanyTokenRegex);

            MatchEvaluator evaluator = new MatchEvaluator(matchEval);
            return regex.Replace(text, evaluator);
        }

        private static string matchEval(Match match)
        {
            string value = match.Value;

            //remove the token part on the sides to keep the company name and number separated by a ':'
            value = value.Replace("[C#", string.Empty).Replace("]", string.Empty);

            //Split the remaining to separate the company name from the number
            string[] companyDet = value.Split(':');

            //return a strong company name (from the token itself) to replace the token
            return string.Format("<strong>{0}</strong>", companyDet[1]);
        }
    }
}
