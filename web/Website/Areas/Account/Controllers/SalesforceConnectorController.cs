using System.Web.Mvc;
using Informa.Web.Areas.Account.Models;
using Sitecore.Data;
using Sitecore.Social.Client.Api;
using Sitecore.Social.Client.Common.Helpers;
using Sitecore.Social.Client.Mvc.Areas.Social.Controllers;

namespace Informa.Web.Areas.Account.Controllers
{
    public class SalesforceConnectorController : ConnectorController
    {
        public SalesforceConnectorController()
            : base("Salesforce", ID.Parse("{78D8D914-51C8-41F3-8424-021262F148B8}"))
        {
        }

        public ActionResult Index()
        {

            //var something = LoginHelper
            //Sitecore.Web.Authentication.DI.RPCATokenRedirect;
            return this.PartialView("~/Areas/Account/Views/Connector/Login.cshtml");
            //{
            //    Icon = this.GetLoginButtonImageUrl(this.networkIconItemID),
            //    ToolTip = tooltip,
            //    Parameters = str
            //});
            //return this.LoginPartialView();
        }
        //protected PartialViewResult LoginPartialView(string tooltip)
        //{
        //    string str = this.ProtectParameters();
        //    return this.PartialView("~/Areas/Social/Views/Connector/Login.cshtml", (object)new LoginViewModel()
        //    {
        //        //Icon = this.GetLoginButtonImageUrl(this.networkIconItemID),
        //        //ToolTip = tooltip,
        //        //Parameters = str
        //    });
        //}

        //private string ProtectParameters()
        //{
        //    if (Enumerable.Any<KeyValuePair<string, string>>((IEnumerable<KeyValuePair<string, string>>)RenderingContext.Current.Rendering.Parameters))
        //        return this.Protect(Encoding.UTF8.GetBytes(string.Format("{0}|{1}|{2}", (object)Context.Item.Uri, (object)Context.Device.ID.ToShortID(), (object)RenderingContext.Current.Rendering.UniqueId)));
        //    return (string)null;
        //}

        //private string Protect(byte[] data)
        //{
        //    if (data == null || data.Length == 0)
        //        return (string)null;
        //    return System.Convert.ToBase64String(MachineKey.Protect(data));
        //}
    }



}

