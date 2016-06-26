using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Informa.Web.Util.Reports
{
    public partial class UsersRolesReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var allUsers = DomainManager.GetDomain("sitecore").GetUsers();
                List<UsersRolesReportViewModel> lstUsers = new List<UsersRolesReportViewModel>();

                foreach (var item in allUsers)
                {
                    var obj = new UsersRolesReportViewModel();
                    obj.UserName = item.Name;

                    var roles = Sitecore.Security.Accounts.RolesInRolesManager.GetRolesForUser(item, true);
                    obj.Roles = string.Join(", ", roles.Select(s => s.DisplayName));

                    lstUsers.Add(obj);
                }

                grdUsersRoles.DataSource = lstUsers;
                grdUsersRoles.DataBind();
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.ToString(), this);
            }
        }

        void ExportToExcel(DataTable dt, string FileName)
        {
            if (dt.Rows.Count > 0)
            {
                string filename = FileName + ".xls";
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dt;
                dgGrid.DataBind();

                //Get the HTML for the control.
                dgGrid.RenderControl(hw);
                //Write the HTML back to the browser.
                //Response.ContentType = application/vnd.ms-excel;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition",
                                      "attachment; filename=" + filename + "");
                this.EnableViewState = false;
                Response.Write(tw.ToString());
                Response.End();
            }
        }

        protected void lnkExport_Click(object sender, EventArgs e)
        {
            var allUsers = DomainManager.GetDomain("sitecore").GetUsers();
            DataTable dt = new DataTable("Users/Roles Report");
            dt.Columns.Add("UserName");
            dt.Columns.Add("Roles");

            foreach (var item in allUsers)
            {
                var drow = dt.NewRow();
                drow["UserName"] = item.Name;

                var roles = Sitecore.Security.Accounts.RolesInRolesManager.GetRolesForUser(item, true);
                drow["Roles"] = string.Join(", ", roles.Select(s => s.DisplayName));

                dt.Rows.Add(drow);
            }

            ExportToExcel(dt, "UsersRolesReport");
        }
    }

    public class UsersRolesReportViewModel
    {
        public string UserName { get; set; }
        public string Roles { get; set; }
    }
}