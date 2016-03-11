using Informa.Library.Utilities.WebUtils;
using Informa.Model.DCD;
using Informa.Models.DCD;
using System;
using System.Web;
using System.Web.UI;

namespace Informa.Web.Util.DCD
{
    public partial class ViewDCDXML : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCompanyNumber.Text))
            {
                string url = UrlUtils.AddParameterToUrl(Request, "type", "company");
                url = UrlUtils.AddParameterToUrl(url, "number", txtCompanyNumber.Text);
                Response.Redirect(url);
            }

            if (!string.IsNullOrEmpty(txtDealNumber.Text))
            {
                string url = UrlUtils.AddParameterToUrl(Request, "type", "deal");
                url = UrlUtils.AddParameterToUrl(url, "number", txtDealNumber.Text);
                Response.Redirect(url);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            string type = Request["type"];
            string number = Request["number"];

            if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(number))
            {
                if (type == "deal")
                {
                    Deal deal = new DCDManager().GetDealByRecordNumber(number);
                    if (deal != null)
                    {
                        Response.ContentType = "application/xml";
                        writer.Write(buildXml(deal));
                    }
                }
                if (type == "company")
                {
                    Company company = new DCDManager().GetCompanyByRecordNumber(number);
                    if (company != null)
                    {
                        Response.ContentType = "application/xml";
                        writer.Write(buildXml(company));
                    }
                }
            }
            else
            {
                base.Render(writer);
            }
        }

        private string buildXml(IDCD dcd)
        {
            string xml = "<Record>";
            xml += "<Identification>";
            xml += "<RecordId>" + dcd.RecordId + "</RecordId>";
            xml += "<RecordNumber>" + dcd.RecordNumber + "</RecordNumber>";
            xml += "<Title>" + HttpUtility.HtmlEncode(dcd.Title) + "</Title>";
            xml += "<Created>" + dcd.Created.ToString("yyyy-MM-ddTHH:mm:ss") + "</Created>";
            xml += "<LastModified>" + dcd.LastModified.ToString("yyyy-MM-ddThh:mm:ss") + "</LastModified>";
            xml += "</Identification>";
            xml += dcd.Content;
            xml += "</Record>";
            return xml;
        }
    }
}