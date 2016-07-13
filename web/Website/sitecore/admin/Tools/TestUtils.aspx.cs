using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Autofac;
using Informa.Library.Utilities.Security;
using Informa.Model.DCD;
using Jabberwocky.Glass.Autofac.Util;

namespace Informa.Web.sitecore.admin.Tools
{
    public class TestUtils : Page
    {

#region DCD Inspector
        protected TextBox DCDInput { get; set; }
        protected TextBox DCDOutput { get; set; }
        protected TextBox DCDInputRecordNumber { get; set; }

        protected void GetCompanyClick(object sender, EventArgs e)
        {
            int recordId;
            var hasRecordId = int.TryParse(DCDInput.Text, out recordId);
            var recordNumber = DCDInputRecordNumber.Text;

            using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
            {
                var reader = scope.Resolve<IDCDReader>();
                var company = hasRecordId
                    ? reader.GetCompanyByRecordId(recordId)
                    : reader.GetCompanyByRecordNumber(recordNumber);

                DCDOutput.Text = company != null
                    ? $"{company.Title} | {company.RecordId} | {company.RecordNumber} |\r\n {company.Content}"
                    : "No company found.";
            }
        }

        protected void GetDealClick(object sender, EventArgs e)
        {
            int recordId;
            var hasRecordId = int.TryParse(DCDInput.Text, out recordId);
            var recordNumber = DCDInputRecordNumber.Text;

            using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
            {
                var reader = scope.Resolve<IDCDReader>();
                var deal = hasRecordId
                    ? reader.GetDealByRecordId(recordId)
                    : reader.GetDealByRecordNumber(recordNumber);

                
                DCDOutput.Text = deal != null 
                    ? $"{deal.Title} | {deal.RecordId} | {deal.RecordNumber} |\r\n {deal.Content}"
                    : "No deal found.";
            }
        }

#endregion

        #region Crypto Tester
        protected TextBox InputTxt { get; set; }
        protected TextBox KeyTxt { get; set; }
        protected TextBox OutputTxt { get; set; }

        protected void EncryptClick(object sender, EventArgs e)
        {
            var input = InputTxt.Text;
            var key = KeyTxt.Text;
            using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
            {
                var crypto = scope.Resolve<ICrypto>();
                var cypher = crypto.EncryptStringAes(input, key);
                OutputTxt.Text = HttpUtility.UrlEncode(cypher);
            }
        }

        protected void DecryptClick(object sender, EventArgs e)
        {
            var cipher = HttpUtility.UrlDecode(OutputTxt.Text);
            var key = KeyTxt.Text;
            using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
            {
                var crypto = scope.Resolve<ICrypto>();
                InputTxt.Text = crypto.DecryptStringAes(cipher, key);
            }
        }
#endregion

        
    }
}