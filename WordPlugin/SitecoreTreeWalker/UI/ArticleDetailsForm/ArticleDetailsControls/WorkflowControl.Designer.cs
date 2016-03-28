namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkflowControl));
			this.uxWorkflowLabel = new System.Windows.Forms.Label();
			this.uxWorkflowActions = new System.Windows.Forms.ComboBox();
			this.uxUnlockOnSave = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.uxNotifyPicker = new System.Windows.Forms.ComboBox();
			this.uxNotifyAdd = new System.Windows.Forms.Button();
			this.uxNotifyList = new InformaSitecoreWord.UI.EasyRemoveListView();
			this.uxCurrentWorkflowValue = new System.Windows.Forms.Label();
			this.blMoveToWorkflowStateLabel = new System.Windows.Forms.Label();
			this.txtNotificationText = new System.Windows.Forms.TextBox();
			this.lblUnlockOnSave = new System.Windows.Forms.Label();
			this.lblNotificationText = new System.Windows.Forms.Label();
			this.uxWorkflowPanel = new System.Windows.Forms.Panel();
			this.subjectLbl = new System.Windows.Forms.Label();
			this.lblWorkflow = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.uxWorkflowPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// uxWorkflowLabel
			// 
			this.uxWorkflowLabel.AutoEllipsis = true;
			this.uxWorkflowLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uxWorkflowLabel.Location = new System.Drawing.Point(11, 11);
			this.uxWorkflowLabel.Name = "uxWorkflowLabel";
			this.uxWorkflowLabel.Size = new System.Drawing.Size(158, 33);
			this.uxWorkflowLabel.TabIndex = 1;
			this.uxWorkflowLabel.Text = "Current WorkFlow State: ";
			this.uxWorkflowLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// uxWorkflowActions
			// 
			this.uxWorkflowActions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.uxWorkflowActions.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.uxWorkflowActions.FormattingEnabled = true;
			this.uxWorkflowActions.Location = new System.Drawing.Point(175, 52);
			this.uxWorkflowActions.Name = "uxWorkflowActions";
			this.uxWorkflowActions.Size = new System.Drawing.Size(200, 25);
			this.uxWorkflowActions.TabIndex = 2;
			this.uxWorkflowActions.SelectedIndexChanged += new System.EventHandler(this.uxWorkflowActions_SelectedIndexChanged);
			// 
			// uxUnlockOnSave
			// 
			this.uxUnlockOnSave.AutoSize = true;
			this.uxUnlockOnSave.Checked = true;
			this.uxUnlockOnSave.CheckState = System.Windows.Forms.CheckState.Checked;
			this.uxUnlockOnSave.Font = new System.Drawing.Font("Segoe UI", 8F);
			this.uxUnlockOnSave.Location = new System.Drawing.Point(116, 361);
			this.uxUnlockOnSave.Name = "uxUnlockOnSave";
			this.uxUnlockOnSave.Size = new System.Drawing.Size(15, 14);
			this.uxUnlockOnSave.TabIndex = 3;
			this.uxUnlockOnSave.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 77);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(82, 15);
			this.label1.TabIndex = 4;
			this.label1.Text = "Notify Users :";
			// 
			// uxNotifyPicker
			// 
			this.uxNotifyPicker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.uxNotifyPicker.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uxNotifyPicker.FormattingEnabled = true;
			this.uxNotifyPicker.Location = new System.Drawing.Point(14, 104);
			this.uxNotifyPicker.Name = "uxNotifyPicker";
			this.uxNotifyPicker.Size = new System.Drawing.Size(200, 21);
			this.uxNotifyPicker.TabIndex = 5;
			// 
			// uxNotifyAdd
			// 
			this.uxNotifyAdd.Font = new System.Drawing.Font("Segoe UI", 8F);
			this.uxNotifyAdd.Location = new System.Drawing.Point(220, 104);
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
			this.uxNotifyList.Location = new System.Drawing.Point(14, 131);
			this.uxNotifyList.Name = "uxNotifyList";
			this.uxNotifyList.Size = new System.Drawing.Size(287, 90);
			this.uxNotifyList.TabIndex = 8;
			this.uxNotifyList.UseCompatibleStateImageBehavior = false;
			this.uxNotifyList.View = System.Windows.Forms.View.Details;
			// 
			// uxCurrentWorkflowValue
			// 
			this.uxCurrentWorkflowValue.AutoSize = true;
			this.uxCurrentWorkflowValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uxCurrentWorkflowValue.Location = new System.Drawing.Point(175, 19);
			this.uxCurrentWorkflowValue.Name = "uxCurrentWorkflowValue";
			this.uxCurrentWorkflowValue.Size = new System.Drawing.Size(28, 15);
			this.uxCurrentWorkflowValue.TabIndex = 9;
			this.uxCurrentWorkflowValue.Text = "XXX";
			// 
			// blMoveToWorkflowStateLabel
			// 
			this.blMoveToWorkflowStateLabel.AutoEllipsis = true;
			this.blMoveToWorkflowStateLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.blMoveToWorkflowStateLabel.Location = new System.Drawing.Point(11, 44);
			this.blMoveToWorkflowStateLabel.Name = "blMoveToWorkflowStateLabel";
			this.blMoveToWorkflowStateLabel.Size = new System.Drawing.Size(158, 33);
			this.blMoveToWorkflowStateLabel.TabIndex = 10;
			this.blMoveToWorkflowStateLabel.Text = "Move to Workflow State: ";
			this.blMoveToWorkflowStateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtNotificationText
			// 
			this.txtNotificationText.Location = new System.Drawing.Point(14, 260);
			this.txtNotificationText.Multiline = true;
			this.txtNotificationText.Name = "txtNotificationText";
			this.txtNotificationText.Size = new System.Drawing.Size(287, 97);
			this.txtNotificationText.TabIndex = 13;
			// 
			// lblUnlockOnSave
			// 
			this.lblUnlockOnSave.AutoSize = true;
			this.lblUnlockOnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblUnlockOnSave.Location = new System.Drawing.Point(11, 360);
			this.lblUnlockOnSave.Name = "lblUnlockOnSave";
			this.lblUnlockOnSave.Size = new System.Drawing.Size(99, 15);
			this.lblUnlockOnSave.TabIndex = 14;
			this.lblUnlockOnSave.Text = "Unlock on save? ";
			// 
			// lblNotificationText
			// 
			this.lblNotificationText.AutoSize = true;
			this.lblNotificationText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblNotificationText.Location = new System.Drawing.Point(12, 242);
			this.lblNotificationText.Name = "lblNotificationText";
			this.lblNotificationText.Size = new System.Drawing.Size(257, 15);
			this.lblNotificationText.TabIndex = 12;
			this.lblNotificationText.Text = "Notification Text (email will be sent on save):";
			// 
			// uxWorkflowPanel
			// 
			this.uxWorkflowPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.uxWorkflowPanel.Controls.Add(this.subjectLbl);
			this.uxWorkflowPanel.Controls.Add(this.uxUnlockOnSave);
			this.uxWorkflowPanel.Controls.Add(this.lblUnlockOnSave);
			this.uxWorkflowPanel.Controls.Add(this.uxWorkflowLabel);
			this.uxWorkflowPanel.Controls.Add(this.txtNotificationText);
			this.uxWorkflowPanel.Controls.Add(this.blMoveToWorkflowStateLabel);
			this.uxWorkflowPanel.Controls.Add(this.lblNotificationText);
			this.uxWorkflowPanel.Controls.Add(this.uxCurrentWorkflowValue);
			this.uxWorkflowPanel.Controls.Add(this.uxNotifyList);
			this.uxWorkflowPanel.Controls.Add(this.uxWorkflowActions);
			this.uxWorkflowPanel.Controls.Add(this.uxNotifyAdd);
			this.uxWorkflowPanel.Controls.Add(this.label1);
			this.uxWorkflowPanel.Controls.Add(this.uxNotifyPicker);
			this.uxWorkflowPanel.Location = new System.Drawing.Point(3, 39);
			this.uxWorkflowPanel.Name = "uxWorkflowPanel";
			this.uxWorkflowPanel.Size = new System.Drawing.Size(646, 461);
			this.uxWorkflowPanel.TabIndex = 2;
			// 
			// subjectLbl
			// 
			this.subjectLbl.AutoSize = true;
			this.subjectLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.subjectLbl.Location = new System.Drawing.Point(12, 224);
			this.subjectLbl.Name = "subjectLbl";
			this.subjectLbl.Size = new System.Drawing.Size(55, 15);
			this.subjectLbl.TabIndex = 15;
			this.subjectLbl.Text = "Subject :";
			// 
			// lblWorkflow
			// 
			this.lblWorkflow.AutoSize = true;
			this.lblWorkflow.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblWorkflow.Location = new System.Drawing.Point(13, 16);
			this.lblWorkflow.Name = "lblWorkflow";
			this.lblWorkflow.Size = new System.Drawing.Size(78, 20);
			this.lblWorkflow.TabIndex = 16;
			this.lblWorkflow.Text = "Workflow";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
			this.label2.ForeColor = System.Drawing.Color.Gray;
			this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
			this.label2.Location = new System.Drawing.Point(0, 6);
			this.label2.Margin = new System.Windows.Forms.Padding(0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(610, 30);
			this.label2.TabIndex = 17;
			// 
			// WorkflowControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lblWorkflow);
			this.Controls.Add(this.uxWorkflowPanel);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.Name = "WorkflowControl";
			this.Size = new System.Drawing.Size(652, 500);
			this.uxWorkflowPanel.ResumeLayout(false);
			this.uxWorkflowPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label uxWorkflowLabel;
		private System.Windows.Forms.ComboBox uxWorkflowActions;
		public System.Windows.Forms.CheckBox uxUnlockOnSave;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox uxNotifyPicker;
		private System.Windows.Forms.Button uxNotifyAdd;
		private EasyRemoveListView uxNotifyList;
		private System.Windows.Forms.Label uxCurrentWorkflowValue;
		private System.Windows.Forms.Label blMoveToWorkflowStateLabel;
		private System.Windows.Forms.TextBox txtNotificationText;
		private System.Windows.Forms.Label lblUnlockOnSave;
		private System.Windows.Forms.Label lblNotificationText;
		private System.Windows.Forms.Panel uxWorkflowPanel;
		private System.Windows.Forms.Label lblWorkflow;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label subjectLbl;
	}
}
