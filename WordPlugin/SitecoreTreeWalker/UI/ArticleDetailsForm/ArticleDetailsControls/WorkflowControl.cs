using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using InformaSitecoreWord.Sitecore;
using InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.Interfaces;
using PluginModels;

namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls
{
	public partial class WorkflowControl : ArticleDetailsPageUserControl
	{
		public List<ArticleWorkflowCommand> Commands;
		protected List<StaffStruct> _staff;
		

		public WorkflowControl()
		{
			InitializeComponent();

		}

		public void UpdateFields(ArticleWorkflowState state)
		{
			if (_staff == null)
			{
			    _staff = SitecoreClient.GetStaffAndGroups(); 
			}

			uxNotifyPicker.DataSource = _staff;
			uxNotifyPicker.DisplayMember = "Name";
			uxNotifyPicker.ValueMember = "ID";
			if(state == null)
			{
				return;
			}
			uxCurrentWorkflowValue.Text = state.DisplayName;
			Commands = new List<ArticleWorkflowCommand>();
			Commands.Insert(0, new ArticleWorkflowCommand {DisplayName = "Move in Workflow...", StringID = Guid.Empty.ToString()});
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
		}

		public string GetNotificationText()
		{
			return txtNotificationText.Text;
		}
		protected void SetNotificationOptions()
		{
			if (uxWorkflowActions.SelectedValue is ArticleWorkflowCommand)
			{
				if (((ArticleWorkflowCommand) uxWorkflowActions.SelectedValue).StringID == Guid.Empty.ToString())
				{
					uxNotifyPicker.Enabled = false;
					uxNotifyList.Enabled = false;
					uxNotifyAdd.Enabled = false;
					txtNotificationText.Enabled = false;
					uxUnlockOnSave.Enabled = false;
					return;
				}

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

					return;
				}

				var command = Commands.Where(c => c.StringID == uxWorkflowActions.SelectedValue.ToString()).FirstOrDefault();

				if (command != null && command.GlobalNotifyList != null) { uxNotifyList.ResetUnremovableStaff(command.GlobalNotifyList.ToList()); }
			}

			uxNotifyPicker.Enabled = true;
			uxNotifyList.Enabled = true;
			uxNotifyAdd.Enabled = true;

			

			//update globals

		}

		public void PreLinkEnable()
		{
			Visible = false;
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
			return (new Guid(uxWorkflowActions.SelectedValue.ToString()));
		}

		private void uxWorkflowActions_SelectedIndexChanged(object sender, EventArgs e)
		{
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
