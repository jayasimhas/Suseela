﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Autofac;
using Elsevier.Web.VWB.Report;
using Elsevier.Web.VWB.Report.Columns;
using Glass.Mapper.Sc;
using Informa.Library.VirtualWhiteboard;
using Informa.Web.App_Start;
using Microsoft.Practices.ServiceLocation;
using Sitecore.Caching;
using Sitecore.Configuration;
using Sitecore.Mvc.Extensions;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Web;
using Sitecore.Web.Authentication;
using DateTime = System.DateTime;
using Jabberwocky.Autofac.Attributes;

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
			if (IsPostBack)
			{
				//let the event handlers for the control causing postback
				//execute
				return;
			}

		    if (Request.QueryString.Count == 0 || (Request.QueryString.Count == 1 && Request.QueryString["sc_lang"] != null))
		    {
                RunQuery(true);
            }

            //var logoItem = ItemReference.VWBLogo.InnerItem;
            LogoUrl = "/-/media/scriplogo.jpg";

			const string defaultTime = "12:00 AM";
			txtStartTime.Text = defaultTime;
			txtEndTime.Text = defaultTime;

			UpdateFields();
			BuildOptionalColumnDropdown();
		    BuildExistingIssuesList();

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

		private static int? GetMaxNumResults()
		{

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
			return 60;
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

        #region issue management

        protected void NewIssueSubmitButton_OnClick(object sender, EventArgs e)
        {
            var model = new Informa.Library.VirtualWhiteboard.Models.IssueModel
            {
                Title = IssueTitleInput.Value,
                PublishedDate = DateTime.Parse(IssuePublishedDateInput.Value),
                ArticleIds = IssueArticleIdsInput.Value.Split('|').Select(Guid.Parse)
            };

            using (var scope = Jabberwocky.Glass.Autofac.Util.AutofacConfig.ServiceLocator.BeginLifetimeScope())
            {
                var issueBuilder = scope.Resolve<IIssuesService>();
                var result = issueBuilder.CreateIssueFromModel(model);

                if (result.IsSuccess)
                {
                    Response.Redirect("http://example.com/issue-page");
                }
            }
        }

        protected void BuildExistingIssuesList()
        {
            ExistingIssuesDdl.CssClass = "js-existing-issue";
            ExistingIssuesDdl.Items.Add(new ListItem("Select an existing issue...", "DEFAULT"));

	        using (var scope = Jabberwocky.Glass.Autofac.Util.AutofacConfig.ServiceLocator.BeginLifetimeScope())
	        {
		        var issuesService = scope.Resolve<IIssuesService>();
		        var issues = issuesService.GetActiveIssues();
		        issues.Each(i => ExistingIssuesDdl.Items.Add(new ListItem(i._Name, i._Id.ToString())));
	        }
        }

        #endregion


    }
}