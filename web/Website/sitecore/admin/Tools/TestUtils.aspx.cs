using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Autofac;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Utilities.Security;
using Informa.Model.DCD;
using Jabberwocky.Glass.Autofac.Util;

namespace Informa.Web.sitecore.admin.Tools
{
    public class TestUtils : Page
    {
        protected TextBox InputTxt { get; set; }
        protected TextBox KeyTxt { get; set; }
        protected TextBox OutputTxt { get; set; }
        protected TextBox DCDOutput { get; set; }

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

        protected void GetCompaniesClick(object sender, EventArgs e)
        {
            var recordId = int.Parse(DCDOutput.Text);
            using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
            {
                var reader = scope.Resolve<IDCDReader>();
                var company = reader.GetCompanyByRecordId(recordId);

                DCDOutput.Text = $"{company.Title} | {company.RecordId} | {company.RecordNumber} | {company.Content}";
            }
        }
    }
}