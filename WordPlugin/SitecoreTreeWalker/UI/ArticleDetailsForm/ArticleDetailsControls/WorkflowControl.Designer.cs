namespace SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls
{
	partial class WorkflowControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.uxWorkflowLabel = new System.Windows.Forms.Label();
			this.uxWorkflowActions = new System.Windows.Forms.ComboBox();
			this.uxUnlockOnSave = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.uxNotifyPicker = new System.Windows.Forms.ComboBox();
			this.uxNotifyAdd = new System.Windows.Forms.Button();
			this.uxNotifyList = new SitecoreTreeWalker.UI.EasyRemoveListView();
			this.lblCurrentWorkflowValue = new System.Windows.Forms.Label();
			this.blMoveToWorkflowStateLabel = new System.Windows.Forms.Label();
			this.txtNotificationText = new System.Windows.Forms.TextBox();
			this.WorkflowGroupBox = new System.Windows.Forms.GroupBox();
			this.lblNotificationText = new System.Windows.Forms.Label();
			this.lblUnlockOnSave = new System.Windows.Forms.Label();
			this.WorkflowGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// uxWorkflowLabel
			// 
			this.uxWorkflowLabel.AutoEllipsis = true;
			this.uxWorkflowLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.uxWorkflowLabel.Location = new System.Drawing.Point(6, 41);
			this.uxWorkflowLabel.Name = "uxWorkflowLabel";
			this.uxWorkflowLabel.Size = new System.Drawing.Size(204, 33);
			this.uxWorkflowLabel.TabIndex = 1;
			this.uxWorkflowLabel.Text = "Current WorkFlow State: ";
			this.uxWorkflowLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// uxWorkflowActions
			// 
			this.uxWorkflowActions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.uxWorkflowActions.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.uxWorkflowActions.FormattingEnabled = true;
			this.uxWorkflowActions.Location = new System.Drawing.Point(201, 77);
			this.uxWorkflowActions.Name = "uxWorkflowActions";
			this.uxWorkflowActions.Size = new System.Drawing.Size(200, 25);
			this.uxWorkflowActions.TabIndex = 2;
			this.uxWorkflowActions.SelectedIndexChanged += new System.EventHandler(this.uxWorkflowActions_SelectedIndexChanged);
			// 
			// uxUnlockOnSave
			// 
			this.uxUnlockOnSave.AutoSize = true;
			this.uxUnlockOnSave.Font = new System.Drawing.Font("Segoe UI", 8F);
			this.uxUnlockOnSave.Location = new System.Drawing.Point(138, 427);
			this.uxUnlockOnSave.Name = "uxUnlockOnSave";
			this.uxUnlockOnSave.Size = new System.Drawing.Size(15, 14);
			this.uxUnlockOnSave.TabIndex = 3;
			this.uxUnlockOnSave.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(7, 117);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(87, 17);
			this.label1.TabIndex = 4;
			this.label1.Text = "Notify Users :";
			// 
			// uxNotifyPicker
			// 
			this.uxNotifyPicker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.uxNotifyPicker.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uxNotifyPicker.FormattingEnabled = true;
			this.uxNotifyPicker.Location = new System.Drawing.Point(6, 137);
			this.uxNotifyPicker.Name = "uxNotifyPicker";
			this.uxNotifyPicker.Size = new System.Drawing.Size(200, 21);
			this.uxNotifyPicker.TabIndex = 5;
			// 
			// uxNotifyAdd
			// 
			this.uxNotifyAdd.Font = new System.Drawing.Font("Segoe UI", 8F);
			this.uxNotifyAdd.Location = new System.Drawing.Point(224, 137);
			this.uxNotifyAdd.Name = "uxNotifyAdd";
			this.uxNotifyAdd.Size = new System.Drawing.Size(69, 23);
			this.uxNotifyAdd.TabIndex = 7;
			this.uxNotifyAdd.Text = "Add";
			this.uxNotifyAdd.UseVisualStyleBackColor = true;
			this.uxNotifyAdd.Click += new System.EventHandler(this.uxNotifyAdd_Click);
			// 
			// uxNotifyList
			// 
			this.uxNotifyList.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uxNotifyList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.uxNotifyList.Location = new System.Drawing.Point(6, 177);
			this.uxNotifyList.Name = "uxNotifyList";
			this.uxNotifyList.Size = new System.Drawing.Size(287, 90);
			this.uxNotifyList.TabIndex = 8;
			this.uxNotifyList.UseCompatibleStateImageBehavior = false;
			this.uxNotifyList.View = System.Windows.Forms.View.Details;
			// 
			// lblCurrentWorkflowValue
			// 
			this.lblCurrentWorkflowValue.AutoSize = true;
			this.lblCurrentWorkflowValue.Location = new System.Drawing.Point(182, 46);
			this.lblCurrentWorkflowValue.Name = "lblCurrentWorkflowValue";
			this.lblCurrentWorkflowValue.Size = new System.Drawing.Size(37, 21);
			this.lblCurrentWorkflowValue.TabIndex = 9;
			this.lblCurrentWorkflowValue.Text = "XXX";
			// 
			// blMoveToWorkflowStateLabel
			// 
			this.blMoveToWorkflowStateLabel.AutoEllipsis = true;
			this.blMoveToWorkflowStateLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.blMoveToWorkflowStateLabel.Location = new System.Drawing.Point(6, 74);
			this.blMoveToWorkflowStateLabel.Name = "blMoveToWorkflowStateLabel";
			this.blMoveToWorkflowStateLabel.Size = new System.Drawing.Size(189, 33);
			this.blMoveToWorkflowStateLabel.TabIndex = 10;
			this.blMoveToWorkflowStateLabel.Text = "Move to Workflow State: ";
			this.blMoveToWorkflowStateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtNotificationText
			// 
			this.txtNotificationText.Location = new System.Drawing.Point(6, 308);
			this.txtNotificationText.Multiline = true;
			this.txtNotificationText.Name = "txtNotificationText";
			this.txtNotificationText.Size = new System.Drawing.Size(287, 97);
			this.txtNotificationText.TabIndex = 13;
			// 
			// WorkflowGroupBox
			// 
			this.WorkflowGroupBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.WorkflowGroupBox.Controls.Add(this.lblUnlockOnSave);
			this.WorkflowGroupBox.Controls.Add(this.txtNotificationText);
			this.WorkflowGroupBox.Controls.Add(this.lblNotificationText);
			this.WorkflowGroupBox.Controls.Add(this.blMoveToWorkflowStateLabel);
			this.WorkflowGroupBox.Controls.Add(this.lblCurrentWorkflowValue);
			this.WorkflowGroupBox.Controls.Add(this.uxNotifyList);
			this.WorkflowGroupBox.Controls.Add(this.uxNotifyAdd);
			this.WorkflowGroupBox.Controls.Add(this.uxNotifyPicker);
			this.WorkflowGroupBox.Controls.Add(this.label1);
			this.WorkflowGroupBox.Controls.Add(this.uxUnlockOnSave);
			this.WorkflowGroupBox.Controls.Add(this.uxWorkflowActions);
			this.WorkflowGroupBox.Controls.Add(this.uxWorkflowLabel);
			this.WorkflowGroupBox.Font = new System.Drawing.Font("Segoe UI", 12F);
			this.WorkflowGroupBox.Location = new System.Drawing.Point(0, -1);
			this.WorkflowGroupBox.Name = "WorkflowGroupBox";
			this.WorkflowGroupBox.Size = new System.Drawing.Size(650, 480);
			this.WorkflowGroupBox.TabIndex = 1;
			this.WorkflowGroupBox.TabStop = false;
			this.WorkflowGroupBox.Text = "Workflow";
			// 
			// lblNotificationText
			// 
			this.lblNotificationText.AutoSize = true;
			this.lblNotificationText.Location = new System.Drawing.Point(6, 284);
			this.lblNotificationText.Name = "lblNotificationText";
			this.lblNotificationText.Size = new System.Drawing.Size(317, 21);
			this.lblNotificationText.TabIndex = 12;
			this.lblNotificationText.Text = "Notification Text (email will be sent on save):";
			// 
			// lblUnlockOnSave
			// 
			this.lblUnlockOnSave.AutoSize = true;
			this.lblUnlockOnSave.Location = new System.Drawing.Point(6, 421);
			this.lblUnlockOnSave.Name = "lblUnlockOnSave";
			this.lblUnlockOnSave.Size = new System.Drawing.Size(126, 21);
			this.lblUnlockOnSave.TabIndex = 14;
			this.lblUnlockOnSave.Text = "Unlock on save? ";
			// 
			// WorkflowControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.WorkflowGroupBox);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.Name = "WorkflowControl";
			this.Size = new System.Drawing.Size(650, 520);
			this.WorkflowGroupBox.ResumeLayout(false);
			this.WorkflowGroupBox.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label uxWorkflowLabel;
		private System.Windows.Forms.ComboBox uxWorkflowActions;
		public System.Windows.Forms.CheckBox uxUnlockOnSave;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox uxNotifyPicker;
		private System.Windows.Forms.Button uxNotifyAdd;
		private EasyRemoveListView uxNotifyList;
		private System.Windows.Forms.Label lblCurrentWorkflowValue;
		private System.Windows.Forms.Label blMoveToWorkflowStateLabel;
		private System.Windows.Forms.TextBox txtNotificationText;
		private System.Windows.Forms.GroupBox WorkflowGroupBox;
		private System.Windows.Forms.Label lblUnlockOnSave;
		private System.Windows.Forms.Label lblNotificationText;
	}
}
