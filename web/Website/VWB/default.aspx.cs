using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Autofac;
using Elsevier.Web.VWB.Report;
using Elsevier.Web.VWB.Report.Columns;
using Informa.Library.VirtualWhiteboard;
using Informa.Library.Wrappers;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Virtual_Whiteboard;
using Sitecore.Caching;
using Sitecore.Configuration;
using Sitecore.Mvc.Extensions;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Web;
using Sitecore.Web.Authentication;
using DateTime = System.DateTime;
using System.Web.Mvc;
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
        private const string IssuePageUrl = "/vwb/AddIssue?id=";
        private Sitecore.Data.Database dbMaster = Sitecore.Data.Database.GetDatabase("master");

        protected void Page_Load(object sender, EventArgs e)
        {
            imgLogo.ImageUrl = $"{HttpContext.Current.Request.Url.Scheme}://{Factory.GetSiteInfo("website")?.TargetHostName ?? WebUtil.GetHostName()}/-/media/scriplogo.jpg";
            imgLogo.Attributes.Add("style", "width:317px;height:122px");

            if (!Sitecore.Context.User.IsAuthenticated)
            {
                Response.Redirect(WebUtil.GetFullUrl(Factory.GetSiteInfo("shell").LoginPage) + "?returnUrl=" + Request.RawUrl);
            }
            if (IsPostBack)
            {
                //let the event handlers for the control causing postback
                //ddlVerticals.SelectedIndex = ddlVerticals.Items.IndexOf(ddlVerticals.Items.FindByValue(hdnSelectedVertical.Value));
                return;
            }

            //FillPublicationsList();
            FillVerticals();


            if (Request.QueryString.Count == 0 || (Request.QueryString.Count == 1 && Request.QueryString["sc_lang"] != null))
            {
                RunQuery(true);
            }

            const string defaultTime = "12:00 AM";
            txtStartTime.Text = defaultTime;
            txtEndTime.Text = defaultTime;

            UpdateFields();
            BuildOptionalColumnDropdown();
            if(ddlVerticals.SelectedItem != null && ddlVerticals.SelectedItem.Text != "" && ddlVerticals.SelectedItem.Text != "Select Verticals")
                BuildExistingIssuesList();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            _vwbQuery = new VwbQuery(Request);
            if (!IsPostBack)
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
                return FindControl(ctrlname);

            List<string> names = new List<string>();
            foreach (string s in Request.Form)
                names.Add(s);

            var buttons = names
                                            .Where(a => a.Contains("btn"));

            if (buttons.Any())
                return FindControl(buttons.First());

            return null;
        }
        /// <summary>
        /// Poplates dropdown with available publications for the given vertical
        /// </summary>
        private void FillPublicationsList(string selectedVal)
        {
            Sitecore.Data.ID VerticalID;
            if (Sitecore.Data.ID.TryParse(selectedVal, out VerticalID))
            {
                var pubItems = dbMaster.GetItem(VerticalID).Children;
                //string pubsList = string.Empty;
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
                        //pubsList += pubCode + ",";

                    }
                    catch { }
                }
                //if(pubsList.Contains(','))
                //    pubsList.Remove(pubsList.LastIndexOf(',')-1);

                //hdnSelectedPubs.Value = pubsList;
                ddlPublications.DataSource = dt;
                ddlPublications.DataValueField = "Code";
                ddlPublications.DataTextField = "Name";
                ddlPublications.DataBind();
            }
        }

        /// <summary>
        /// Poplates dropdown with available verticals
        /// </summary>
        private void FillVerticals()
        {
            var childItems = dbMaster.GetItem("/sitecore/content/").Children;
            if (childItems != null && childItems.Any())
            {
                var verticalItems = childItems.Where(p => p.TemplateName.Equals("Vertical Root")).Select(s => new { Sitename = s.Name, SiteId = s.ID.ToString() });
                if (verticalItems.Any())
                {
                    ddlVerticals.DataSource = verticalItems;
                    ddlVerticals.DataValueField = "SiteId";
                    ddlVerticals.DataTextField = "Sitename";
                    ddlVerticals.DataBind();
                    ddlVerticals.Items.Insert(0, new ListItem("Select Verticals", "NA"));
                }
            }


        }

        private static int? GetMaxNumResults()
        {
            int count = 250;
            try
            {
                count = int.Parse(HttpContext.Current.Request.QueryString["max"]);
            }
            catch { }
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

            if (string.IsNullOrEmpty(_vwbQuery.PublicationCodes) == false && _vwbQuery.VerticalRoot != null)
            {
                FillPublicationsList(_vwbQuery.VerticalRoot);
                ddlVerticals.Items.FindByValue(_vwbQuery.VerticalRoot).Selected = true;
                List<string> codes = _vwbQuery.PublicationCodes.Split(',').ToList();
                foreach (var item in codes)
                {
                    ddlPublications.Items.FindByValue(item).Selected = true;
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
            var pubCount = ddlPublications.Items.Cast<ListItem>().Where(li => li.Selected).Count();
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
            q.StartDate = (rbDateRange.Checked) ? GetDateValue(txtStart.Text, txtStartTime.Text) : null;
            q.EndDate = (rbDateRange.Checked) ? GetDateValue(txtEnd.Text, txtEndTime.Text) : null;
            q.InProgressValue = chkShowInProgressArticles.Checked;

            q.ShouldRun = execute;

            List<ListItem> selected = ddlPublications.Items.Cast<ListItem>().Where(li => li.Selected).ToList();
            q.PublicationCodes = string.Join(",", selected.Select(s => s.Value));
            hdnSelectedPubs.Value = string.Join(",", selected.Select(s => s.Value));
            q.NumResultsValue = GetMaxNumResults();
            if (ddlVerticals.SelectedValue != null && ddlVerticals.SelectedValue != "NA")
                q.VerticalRoot = ddlVerticals.SelectedValue;
            RedirectTo(q);
        }

        protected DateTime? GetDateValue(string date, string time)
        {
            if (string.IsNullOrEmpty(date))
                return null;

            DateTime dt;
            string formattedDate = (string.IsNullOrEmpty(time)) ? date : string.Format("{0} {1}", date, time);
            bool validStart = DateTime.TryParse(formattedDate, out dt);
            if (validStart)
                return dt;
            else
                return null;
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

        #region issue management

        protected void NewIssueSubmitButton_OnClick(object sender, EventArgs e)
        {
            DateTime date;

            var model = new Informa.Library.VirtualWhiteboard.Models.IssueModel
            {
                PublishedDate = DateTime.TryParse(IssuePublishedDateInput.Value, out date) ? date : DateTime.Now,
                Title = IssueTitleInput.Value,
                ArticleIds = IssueArticleIdsInput.Value.Split('|').Select(Guid.Parse),
                Vertical = ddlVerticals.SelectedItem.Text
            };

            using (var scope = Jabberwocky.Glass.Autofac.Util.AutofacConfig.ServiceLocator.BeginLifetimeScope())
            {
                var issueBuilder = scope.Resolve<IIssuesService>();
                var result = issueBuilder.CreateIssueFromModel(model);

                if (result.IsSuccess)
                {
                    BuildReport();
                    string url = $"{IssuePageUrl}{result.IssueId}";
                    ClientScript.RegisterStartupScript(GetType(), "", $"<script>window.open('{ResolveUrl(url)}','_blank')</script>");
                }
                else
                {
                    throw new Exception($"Failed to created new issue, error: {result.DebugErrorMessage}");
                }
            }
        }

        protected void BuildExistingIssuesList()
        {
            ExistingIssueSelector.CssClass = "js-existing-issue";
            ExistingIssueSelector.Items.Add(new ListItem("Select an existing issue...", "DEFAULT"));

            string selectedVertical = ddlVerticals.SelectedItem.Text;
            if(selectedVertical == "" && selectedVertical == "Select Verticals")
            {
                lblMsg.Text = "You must select a vertical";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }

            using (var scope = Jabberwocky.Glass.Autofac.Util.AutofacConfig.ServiceLocator.BeginLifetimeScope())
            {
                var issuesService = scope.Resolve<IIssuesService>();
                var issues = issuesService.GetActiveIssues(selectedVertical);
                issues.Each(i => ExistingIssueSelector.Items.Add(new ListItem($"{i._Name} - {Math.Abs(i._Name.GetHashCode())}", i._Id.ToString())));
            }
        }

        protected void btnAddArticleToExistingIssue_OnClick(object sender, EventArgs e)
        {
            Guid issueId;
            if (!Guid.TryParse(ExistingIssueSelector.SelectedItem.Value, out issueId))
                Response.Redirect(Request.Url.PathAndQuery);

            var sitecoreService = DependencyResolver.Current.GetService<ISitecoreServiceMaster>();
            var issuesService = DependencyResolver.Current.GetService<IIssuesService>();
            var issue = sitecoreService.GetItem<IIssue>(issueId);
            if (issue == null)
                Response.Redirect(Request.Url.PathAndQuery);

            var articleIds = IssueArticleIdsInput.Value.Split('|');
            var articlesToAdd = articleIds.Where(i => !issuesService.DoesIssueContains(issueId, i)).Select(i => new Guid(i));
            issuesService.AddArticlesToIssue(issueId, articlesToAdd);
            BuildReport();
            string url = $"{IssuePageUrl}{issueId}";
            ClientScript.RegisterStartupScript(GetType(), "", $"<script>window.open('{ResolveUrl(url)}','_blank')</script>");
        }

        #endregion
        /// <summary>
        /// Event for vertical dropdown onchange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlVerticals_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlVerticals.SelectedItem.Value == "NA")
            {
                ddlPublications.Items.Clear();
                hdnSelectedPubs.Value = string.Empty;
                hdnSelectedVertical.Value = string.Empty;
                return;
            }
            FillPublicationsList(ddlVerticals.SelectedItem.Value);
            hdnSelectedVertical.Value = ddlVerticals.SelectedItem.Value;
            BuildExistingIssuesList();
        }
    }
}
