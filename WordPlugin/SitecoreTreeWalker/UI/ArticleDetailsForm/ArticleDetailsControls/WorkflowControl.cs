using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SitecoreTreeWalker.Sitecore;
using SitecoreTreeWalker.SitecoreTree;
using SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.Interfaces;

namespace SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls
{
	public partial class WorkflowControl : ArticleDetailsPageUserControl
	{
		public List<WorkflowCommand> Commands;
		protected List<StaffStruct> _staff;
		

		public WorkflowControl()
		{
			InitializeComponent();
		}

		public void UpdateFields(WorkflowState state)
		{
			if (_staff == null)
			{
			    _staff = SitecoreGetter.GetStaffAndGroups(); 
			}

			uxNotifyPicker.DataSource = _staff;
			uxNotifyPicker.DisplayMember = "Name";
			uxNotifyPicker.ValueMember = "ID";
			if(state == null)
			{
				return;
			}
			uxWorkflowLabel.Text = state.DisplayName;
			Commands = new List<WorkflowCommand>();
			Commands.Insert(0, new WorkflowCommand{DisplayName = "Move in Workflow...", StringID = Guid.Empty.ToString()});
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

		protected void SetNotificationOptions()
		{
			if (uxWorkflowActions.SelectedValue is WorkflowCommand)
			{
				if (((WorkflowCommand) uxWorkflowActions.SelectedValue).StringID == Guid.Empty.ToString())
				{
					uxNotifyPicker.Enabled = false;
					uxNotifyList.Enabled = false;
					uxNotifyAdd.Enabled = false;

					return;
				}

				var command = ((WorkflowCommand)uxWorkflowActions.SelectedValue);

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

        public WorkflowCommand GetSelectedCommandState()
        {
            return uxWorkflowActions.SelectedItem as WorkflowCommand;
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
