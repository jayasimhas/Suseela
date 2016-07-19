using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Elsevier.Web.VWB.Report;
using Elsevier.Web.VWB.Report.Columns;
using Sitecore.Caching;
using Sitecore.Configuration;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Web;
using Sitecore.Web.Authentication;
using DateTime = System.DateTime;
using System.Data;
using System.Linq;

namespace Elsevier.Web.VWB
{
    public struct ItemStruct
    {
        public string Name { get; set; }
        public Guid ID { get; set; }
    }

    /// <summary>
    /// If a non-button control on this page should cause the report to be built, 
    /// it should redirect on PostBack. If a button should not build the report, 
    /// add it to the blacklist.
    /// </summary>
    public partial class _default : Page
    {
        private VwbQuery _vwbQuery;
        private ReportBuilder _reportBuilder;
        private readonly List<Control> ReportBuilderBlacklist = new List<Control>();
        protected string LogoUrl;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Sitecore.Context.User.IsAuthenticated)
            {
                Response.Redirect(WebUtil.GetFullUrl(Factory.GetSiteInfo("shell").LoginPage) + "?returnUrl=" + Request.RawUrl);
            }

            LogoUrl = $"{HttpContext.Current.Request.Url.Scheme}://{Factory.GetSiteInfo("website")?.HostName ?? WebUtil.GetHostName()}/-/media/scriplogo.jpg";

            if (IsPostBack)
            {
                //let the event handlers for the control causing postback
                //execute
                return;
            }

            fillPublicationsList();

            if (Request.QueryString.Count == 0 || (Request.QueryString.Count == 1 && Request.QueryString["sc_lang"] != null))
            {
                RunQuery(true);
            }

            const string defaultTime = "12:00 AM";
            txtStartTime.Text = defaultTime;
            txtEndTime.Text = defaultTime;

            UpdateFields();
            BuildOptionalColumnDropdown();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            ReportBuilderBlacklist.Add(btnReset);

            _vwbQuery = new VwbQuery(Request);
            if (!IsPostBack
                || (Request.Params.Get("__EVENTTARGET") == ""
                && !ReportBuilderBlacklist.Contains(GetPostBackControl())))
            {
                if (_vwbQuery.ShouldRun)
                {
                    BuildReport();
                }
            }
        }

        public Control GetPostBackControl()
        {
            Control control = null;

            string ctrlname = Request.Params.Get("__EVENTTARGET");
            if (!string.IsNullOrEmpty(ctrlname))
            {
                control = FindControl(ctrlname);
            }
            else
            {
                foreach (string ctl in Request.Form)
                {
                    Control c = FindControl(ctl);
                    if (c is Button)
                    {
                        control = c;
                        break;
                    }
                }
            }
            return control;
        }

        private void fillPublicationsList()
        {
            var dbMaster = Sitecore.Data.Database.GetDatabase("master");

            var pubItems = dbMaster.GetItem("/sitecore/content/").Children;

            DataTable dt = new DataTable();
            dt.Columns.Add("Code");
            dt.Columns.Add("Name");
            foreach (Sitecore.Data.Items.Item item in pubItems)
            {
                try
                {
                    string pubName = item.Fields["Publication Name"].Value;
                    string pubCode = item.Fields["Publication Code"].Value;

                    dt.Rows.Add(pubCode, pubName);
                }
                catch { }
            }

            chkPublications.DataSource = dt;
            chkPublications.DataValueField = "Code";
            chkPublications.DataTextField = "Name";
            chkPublications.DataBind();
        }

        private static int? GetMaxNumResults()
        {
            int count = 250;
            try
            {
                count = int.Parse(HttpContext.Current.Request.QueryString["max"]);
            }
            catch { }
            //TextNodeItem maxResults = ItemReference.MaxResultsPerSearch.InnerItem;
            //if (maxResults != null)
            //{
            //	try
            //	{
            //		return Int32.Parse(maxResults.Text.Text);
            //	}
            //	catch (Exception)
            //	{
            //		return 60;
            //	}
            //}
            return count;
        }

        private void UpdateFields()
        {
            bool startDate = false;
            if (_vwbQuery.StartDate != null && _vwbQuery.StartDate != DateTime.MinValue)
            {
                startDate = true;

                txtStart.Text = string.Format("{0:MM/dd/yyyy}", _vwbQuery.StartDate);
                txtStartTime.Text = string.Format("{0:h:mm tt}", _vwbQuery.StartDate);
                EnableDate();
            }

            bool endDate = false;
            if (_vwbQuery.EndDate != null && _vwbQuery.EndDate.Value.Year != 9999)
            {
                endDate = true;

                txtEnd.Text = string.Format("{0:MM/dd/yyyy}", _vwbQuery.EndDate);
                txtEndTime.Text = string.Format("{0:h:mm tt}", _vwbQuery.EndDate);
                EnableDate();
            }

            if (!startDate && !endDate)
            {
                rbNoDate.Checked = true;

            }

            if (_vwbQuery.InProgressValue)
            {
                chkShowInProgressArticles.Checked = true;
            }

            if (string.IsNullOrEmpty(_vwbQuery.PublicationCodes) == false)
            {
                List<string> codes = _vwbQuery.PublicationCodes.Split(',').ToList();
                foreach (var item in codes)
                {
                    chkPublications.Items.FindByValue(item).Selected = true;
                }
            }
        }

        protected void EnableDate()
        {
            rbDateRange.Checked = true;
            txtStart.Enabled = true;
            txtEnd.Enabled = true;
            txtEndTime.Enabled = true;
            txtStartTime.Enabled = true;
        }

        public void BuildReport()
        {
            _reportBuilder = new ReportBuilder(this, _vwbQuery);
            tblResults.EnableViewState = false;
            _reportBuilder.BuildTable(tblResults);
        }

        protected void RunReport(object sender, EventArgs e)
        {
            var pubCount = chkPublications.Items.Cast<ListItem>().Where(li => li.Selected).Count();
            if (pubCount == 0)
            {
                lblMsg.Text = "You must select at least one publication";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }
            else if (pubCount > 1 && rbNoDate.Checked)
            {
                lblMsg.Text = "You must specify a date range when selecting more than one publication";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }

            RunQuery(true);
        }

        private void RunQuery(bool execute)
        {
            var q = _vwbQuery.Clone();
            if (rbDateRange.Checked)
            {
                DateTime startDate;
                DateTime.TryParse(txtStart.Text, out startDate);
                DateTime endDate;
                DateTime.TryParse(txtEnd.Text, out endDate);

                if (!endDate.Equals(DateTime.MinValue) && !startDate.Equals(DateTime.MinValue))
                {
                    q.StartDate = startDate;
                    //endDate = endDate.AddDays(1); //setting up the range so that it include the end date
                    q.EndDate = endDate;
                }

                if (q.StartDate.HasValue && !string.IsNullOrEmpty(txtStartTime.Text))
                {
                    DateTime startTime;
                    var combinedStartTime = string.Format("{0} {1}", txtStart.Text, txtStartTime.Text);
                    if (DateTime.TryParse(combinedStartTime, out startTime))
                    {
                        q.StartDate = startTime;
                    }
                }

                if (q.EndDate.HasValue && !string.IsNullOrEmpty(txtEndTime.Text))
                {
                    DateTime endTime;
                    var combinedEndTime = string.Format("{0} {1}", txtEnd.Text, txtEndTime.Text);
                    if (DateTime.TryParse(combinedEndTime, out endTime))
                    {
                        q.EndDate = endTime;
                    }
                }
            }
            else
            {
                q.StartDate = null;
                q.EndDate = null;
            }

            if (chkShowInProgressArticles.Checked)
            {
                q.InProgressValue = true;
            }
            else
            { q.InProgressValue = false; }

            q.ShouldRun = execute;

            List<ListItem> selected = chkPublications.Items.Cast<ListItem>().Where(li => li.Selected).ToList();
            q.PublicationCodes = string.Join(",", selected.Select(s => s.Value));

            q.NumResultsValue = GetMaxNumResults();
            RedirectTo(q);
        }

        /// <summary>
        /// Populates ddColumns with all columns not already spoken for in generated report
        /// </summary>
        protected void BuildOptionalColumnDropdown()
        {
            ddColumns.Items.Add("Add New Field...");
            foreach (var column in ColumnFactory.GetColumnFactory().GetColumnsNot(_vwbQuery.ColumnKeysInOrder))
            {
                ddColumns.Items.Add(new ListItem(column.GetHeader(), column.Key()));
            }
        }

        protected string GetCurrentPageUrl()
        {
            bool httpsOn = Request.ServerVariables["HTTPS"].Equals("ON");
            string http = httpsOn ? "https://" : "http://";
            return http + Request.ServerVariables["SERVER_NAME"] + Request.ServerVariables["URL"];
        }

        public void RedirectTo(VwbQuery query)
        {
            Response.Redirect(GetCurrentPageUrl() + "?" +
                                query.GetQueryString());
        }

        protected void PublicationSelected(object sender, EventArgs e)
        {
            RunQuery(false);
        }

        protected void AddColumn(object sender, EventArgs e)
        {
            if (ColumnFactory.GetColumnFactory().GetColumn(ddColumns.SelectedValue) == null) return;
            _vwbQuery.ColumnKeysInOrder.Add(ddColumns.SelectedValue);
            RedirectTo(_vwbQuery);
        }

        protected void ResetReport(object sender, EventArgs e)
        {
            Response.Redirect(GetCurrentPageUrl());
        }

        protected void Logout(object sender, EventArgs e)
        {
            CacheManager.ClearSecurityCache(Sitecore.Context.User.Name);
            RecentDocuments.Remove(Sitecore.Context.User.Name);
            Sitecore.Shell.Framework.Security.Abandon();
            string currentTicketId = TicketManager.GetCurrentTicketId();
            if (string.IsNullOrEmpty(currentTicketId))
                return; TicketManager.RemoveTicket(currentTicketId);


            Response.Redirect(WebUtil.GetFullUrl(Factory.GetSiteInfo("shell").LoginPage) + "?redirect=" + Request.RawUrl);
        }
    }
}