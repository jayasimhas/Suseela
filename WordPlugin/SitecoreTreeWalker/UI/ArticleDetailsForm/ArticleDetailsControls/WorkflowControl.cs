using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using InformaSitecoreWord.Sitecore;
using InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.Interfaces;
using PluginModels;

namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls
{
	public partial class WorkflowControl : ArticleDetailsPageUserControl
	{
		public List<ArticleWorkflowCommand> Commands;
		protected List<StaffStruct> _staff;
		public ArticleStruct _ArticleStruct;

		public WorkflowControl()
		{
			InitializeComponent();
			uxNotifyPicker.Enabled = false;
			uxNotifyList.Enabled = false;
			uxNotifyAdd.Enabled = false;
			txtNotificationText.Enabled = false;
		}

        public void PopulateStaff(Guid verticalGuid = default(Guid))
        {
            if (_staff == null)
            {
                _staff = SitecoreClient.GetStaffAndGroups();
            }

            if(verticalGuid != default(Guid))
            {
                _staff = SitecoreClient.GetStaffAndGroups(verticalGuid);
            }

            uxNotifyPicker.DataSource = _staff;
            uxNotifyPicker.DisplayMember = "Name";
            uxNotifyPicker.ValueMember = "ID";
        }
		public void UpdateFields(ArticleWorkflowState state, ArticleStruct articleStruct)
		{
            PopulateStaff();

            if (state == null)
			{
				return;
			}
			_ArticleStruct = articleStruct;
			uxCurrentWorkflowValue.Text = state.DisplayName;
			Commands = new List<ArticleWorkflowCommand>();
			Commands.Insert(0, new ArticleWorkflowCommand { DisplayName = "Move in Workflow...", StringID = Guid.Empty.ToString() });
			if (state.Commands != null)
			{
				Commands.AddRange(state.Commands);
			}
			else
			{
				Globals.SitecoreAddin.Log("No workflow actions were available.");
				uxWorkflowActions.Enabled = false;
			}
			uxWorkflowActions.DataSource = Commands;
			uxWorkflowActions.DisplayMember = "DisplayName";
			uxWorkflowActions.ValueMember = "StringID";
			uxUnlockOnSave.Checked = false;
			subjectLbl.Text = "Subject :";
		}

		public string GetNotificationText()
		{
			return txtNotificationText.Text;
		}
		protected void SetNotificationOptions()
		{
			if (uxWorkflowActions.SelectedValue is ArticleWorkflowCommand)
			{
				if (((ArticleWorkflowCommand)uxWorkflowActions.SelectedValue).StringID == Guid.Empty.ToString())
				{
					uxNotifyPicker.Enabled = false;
					uxNotifyList.Enabled = false;
					uxNotifyAdd.Enabled = false;
					txtNotificationText.Enabled = false;
					uxUnlockOnSave.Enabled = false;
					return;
				}
				EnableControls();
				var command = ((ArticleWorkflowCommand)uxWorkflowActions.SelectedValue);

				if (command.GlobalNotifyList != null) { uxNotifyList.ResetUnremovableStaff(command.GlobalNotifyList.ToList()); }
			}
			else if (uxWorkflowActions.SelectedValue is string)
			{
				if (uxWorkflowActions.SelectedValue.ToString() == Guid.Empty.ToString())
				{
					uxNotifyPicker.Enabled = false;
					uxNotifyList.Enabled = false;
					uxNotifyAdd.Enabled = false;
					txtNotificationText.Enabled = false;
					uxUnlockOnSave.Enabled = false;
					return;
				}
				EnableControls();
				var command = Commands.Where(c => c.StringID == uxWorkflowActions.SelectedValue.ToString()).FirstOrDefault();

				if (command != null && command.GlobalNotifyList != null) { uxNotifyList.ResetUnremovableStaff(command.GlobalNotifyList.ToList()); }
			}


		}

		private void EnableControls()
		{
			uxNotifyPicker.Enabled = true;
			uxNotifyList.Enabled = true;
			uxNotifyAdd.Enabled = true;
			txtNotificationText.Enabled = true;
			uxUnlockOnSave.Enabled = true;
		}

		public void PreLinkEnable()
		{
			//Visible = false;
		}

		public void PostLinkEnable()
		{
			Visible = true;
			ResetNotificationList();
		}

		public ArticleWorkflowCommand GetSelectedCommandState()
		{
			return uxWorkflowActions.SelectedItem as ArticleWorkflowCommand;
		}

		public Guid GetSelectedCommand()
		{
			var commandId = uxWorkflowActions?.SelectedValue?.ToString();
			return string.IsNullOrEmpty(commandId) ? Guid.Empty : Guid.Parse(commandId);
		}

		private void uxWorkflowActions_SelectedIndexChanged(object sender, EventArgs e)
		{
			var selectedItem = uxWorkflowActions.SelectedItem as ArticleWorkflowCommand;
			if (selectedItem != null)
			{
				subjectLbl.Text = $"Subject: {WebUtility.HtmlDecode(_ArticleStruct.Title)} has been moved to {selectedItem.DisplayName}";
			}
			uxUnlockOnSave.Checked = true;
			SetNotificationOptions();
		}

		private void uxNotifyAdd_Click(object sender, EventArgs e)
		{
			var selectedStaffMember = uxNotifyPicker.SelectedItem as StaffStruct;

			if (selectedStaffMember != null)
			{
				uxNotifyList.Add(selectedStaffMember);
			}
		}

		public List<StaffStruct> GetNotifyList()
		{
			return uxNotifyList.Selected;
		}

		public void ResetNotificationList()
		{
			uxNotifyList.Reset();
		}
	}
}
