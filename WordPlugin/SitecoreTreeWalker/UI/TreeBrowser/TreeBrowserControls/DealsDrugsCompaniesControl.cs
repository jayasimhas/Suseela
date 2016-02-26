using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using PluginModels;
using Microsoft.Office.Interop.Word;
using SitecoreTreeWalker.Sitecore;
using SitecoreTreeWalker.Util;

namespace SitecoreTreeWalker.UI.TreeBrowser.TreeBrowserControls
{
	public partial class DealsDrugsCompaniesControl : UserControl
	{
		public const string DealTooltip = "Deal";

		protected IEnumerable<CompanyWrapper> _companies;

		public DealsDrugsCompaniesControl()
		{
			InitializeComponent();
		}


		/// <summary>
		/// Execute the next step. If a deal number is in the preview, insert it and remove from preview. 
		/// If no deal is in the preview, try place it in preview.
		/// </summary>
		public void DealNumberGo()
		{
			companyTreeView1.Visible = false;
			uxPreviewDeals.Visible = true;
			if(uxPreviewDeals.Tag == null)
			{
				DealInfo dealInfo = SitecoreClient.GetDealInfo(uxDealNumber.Text);
				uxPreviewDeals.UpdatePreview(dealInfo);
				uxPreviewDeals.Tag = dealInfo;
				uxRetrieveInformation.Visible = false;
				uxInsertIntoArticle.Visible = true;
				uxViewDetails.Enabled = true;
			}
			else
			{
				//TODO - Uncomment the line below and get it working.
				//InsertDealIntoDocument(uxPreviewDeals.Tag as DealInfo);
				SetToRetrieveDealMode();
			}
		}

		protected void CompanyNameGo()
		{
            companyTreeView1.Visible = true;
			uxPreviewDeals.Visible = false;
			var matchText = uxCompanyName.Text;

            companyTreeView1.Filter(matchText);
		}

		protected void InsertCompany()
		{
		    CompanyWrapper company = companyTreeView1.SelectedCompany;
            if (company == null)
			{
				Globals.SitecoreAddin.Log("DealsDrugsCompaniesControl.InsertCompany: Trying to add a company when none are selected!");
				return;
			}

			Range selection = Globals.SitecoreAddin.Application.Selection.Range;
			
			selection.Text = company.Title;
			selection.Font.Bold = -1;
			selection.Collapse(WdCollapseDirection.wdCollapseEnd);
			selection.Text = " ";
			selection.Font.Bold = 0;
			selection.Collapse(WdCollapseDirection.wdCollapseEnd);
			selection.Select();

			//InsertLinkIntoDocument("C", company.RecordNumber, company.URL, CompanyTooltip);
		}

		public static void InsertDealIntoDocument(DealInfo dealInfo)
		{
			InsertLinkIntoDocument("W", dealInfo.ID, dealInfo.Url, DealTooltip);
		}

		protected static void InsertLinkIntoDocument(string prefix, string ID, string URL, string tooltip)
		{
			var app = Globals.SitecoreAddin.Application;
			Range selection = app.Selection.Range;
			selection.Text = "[" + prefix + "#" + ID + "]";
			var address = URL;
			app.ActiveDocument.Hyperlinks.Add(selection, address, null, tooltip);
			app.ActiveDocument.Range(selection.End, selection.End).Select();
		}

		/// <summary>
		/// Deal hyperlinks in document are the same as the Sitecore token, so thus should
		/// be transferred to Sitecore as is if the author does not edit the hyperlink text
		/// </summary>
		/// <param name="hyperlink"></param>
		/// <returns></returns>
		public static bool IsADealHyperlink(Hyperlink hyperlink)
		{
		    if (!WordUtils.IsHyperlinkValid(hyperlink))
		    {
		        return false;
		    }

			return hyperlink != null &&
			       (hyperlink.ScreenTip != null && 
				   (hyperlink.ScreenTip.Equals(DealTooltip)));
		}

		private void SetToRetrieveDealMode()
		{
			uxPreviewDeals.Controls.Clear();
			uxPreviewDeals.Tag = null;
			uxRetrieveInformation.Visible = true;
			uxInsertIntoArticle.Visible = false;
			uxViewDetails.Enabled = false;
		}

		public const string DealRegex = @"\[W#(.*?)\]";

		private void uxCompanyName_TextChanged(object sender, EventArgs e)
		{
			CompanyNameGo();
		    uxCompanyName.Focus();
		}

		/* COMPANIES */

		private void uxCompanyName_KeyDown(object sender, KeyEventArgs e)
		{

				if (e.KeyCode == Keys.Up)
				{
                    companyTreeView1.MoveUp();
				    e.SuppressKeyPress = true;
				}
				else if (e.KeyCode == Keys.Down)
				{
					companyTreeView1.MoveDown();
                    e.SuppressKeyPress = true;
				}
				else if (e.KeyCode == Keys.Enter)
				{
					CompanyNameGo();
                    uxCompanyName.Focus();
				}

				uxViewDetails.Enabled = false;
				uxRetrieveInformation.Visible = false;
				uxInsertIntoArticle.Visible = true;
				uxDealNumber.Clear();

		}

		private void uxDealNumber_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				DealNumberGo();
				e.SuppressKeyPress = true;
			}
			else
			{
				SetToRetrieveDealMode();
			}
			uxCompanyName.Clear();
			uxPreviewPanel.Visible = true;
			companyTreeView1.Visible = false;
		}

		private void uxCompanyNameGo_Click(object sender, EventArgs e)
		{
            CompanyNameGo();
            uxCompanyName.Focus();
		}

		private void uxDealNumberGo_Click(object sender, EventArgs e)
		{
			DealNumberGo();
		}

		private void uxInsertIntoArticle_Click(object sender, EventArgs e)
		{
			if (!uxDealNumber.Text.IsNullOrEmpty())
			{
				//TODO - Uncomment the line below and get it working.
				//var deal = uxPreviewDeals.Tag as DealInfo;
				//if (deal != null)
				//{
				//	InsertDealIntoDocument(deal);
				//}
				uxPreviewDeals.Tag = null;
				uxPreviewDeals.Controls.Clear();
				SetToRetrieveDealMode();
			}
			else if (!uxCompanyName.Text.IsNullOrEmpty())
			{
				InsertCompany();
			}
		}

		private void uxRetrieveInformation_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(uxDealNumber.Text))
			{
				DealInfo dealInfo = SitecoreClient.GetDealInfo(uxDealNumber.Text);
				uxPreviewDeals.UpdatePreview(dealInfo);
				uxPreviewDeals.Tag = dealInfo;
				uxRetrieveInformation.Visible = false;
				uxInsertIntoArticle.Visible = true;
				uxViewDetails.Enabled = true;
			}
		}

		private void uxViewDetails_Click(object sender, EventArgs e)
		{
			//TODO - Uncomment the line below and get it working.
			//var deal = uxPreviewDeals.Tag as DealInfo;
			//if (deal != null && !string.IsNullOrEmpty(deal.Url))
			//{
			//	Process.Start(deal.Url); 
			//}
		}

		private void DealsDrugsCompaniesControl_Load(object sender, EventArgs e)
		{
			if (!DesignMode)
			{
                //_companies = SitecoreClient.GetAllCompaniesWithRelated();

                //companyTreeView1.LoadCompanies(_companies);

				//CompanyNameGo();

				ImageList list = new ImageList();
                list.Images.Add(Properties.Resources.company); 
                companyTreeView1.SetImageList(list);
			}
		}

        private void companyTreeView1_CompanyDoubleClicked(CompanyWrapper wrapper)
        {
            InsertCompany();
        }
	}
}
