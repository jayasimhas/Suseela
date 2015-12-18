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
			this.WorkflowGroupBox = new System.Windows.Forms.GroupBox();
			this.uxNotifyList = new SitecoreTreeWalker.UI.EasyRemoveListView();
			this.uxNotifyAdd = new System.Windows.Forms.Button();
			this.uxNotifyPicker = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.uxUnlockOnSave = new System.Windows.Forms.CheckBox();
			this.uxWorkflowActions = new System.Windows.Forms.ComboBox();
			this.uxWorkflowLabel = new System.Windows.Forms.Label();
			this.WorkflowGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// WorkflowGroupBox
			// 
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
			this.WorkflowGroupBox.Size = new System.Drawing.Size(650, 129);
			this.WorkflowGroupBox.TabIndex = 1;
			this.WorkflowGroupBox.TabStop = false;
			this.WorkflowGroupBox.Text = "Workflow";
			// 
			// uxNotifyList
			// 
			this.uxNotifyList.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uxNotifyList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.uxNotifyList.Location = new System.Drawing.Point(369, 68);
			this.uxNotifyList.Name = "uxNotifyList";
			this.uxNotifyList.Size = new System.Drawing.Size(275, 55);
			this.uxNotifyList.TabIndex = 8;
			this.uxNotifyList.UseCompatibleStateImageBehavior = false;
			this.uxNotifyList.View = System.Windows.Forms.View.Details;
			// 
			// uxNotifyAdd
			// 
			this.uxNotifyAdd.Font = new System.Drawing.Font("Segoe UI", 8F);
			this.uxNotifyAdd.Location = new System.Drawing.Point(575, 39);
			this.uxNotifyAdd.Name = "uxNotifyAdd";
			this.uxNotifyAdd.Size = new System.Drawing.Size(69, 23);
			this.uxNotifyAdd.TabIndex = 7;
			this.uxNotifyAdd.Text = "Add";
			this.uxNotifyAdd.UseVisualStyleBackColor = true;
			this.uxNotifyAdd.Click += new System.EventHandler(this.uxNotifyAdd_Click);
			// 
			// uxNotifyPicker
			// 
			this.uxNotifyPicker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.uxNotifyPicker.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uxNotifyPicker.FormattingEnabled = true;
			this.uxNotifyPicker.Location = new System.Drawing.Point(369, 41);
			this.uxNotifyPicker.Name = "uxNotifyPicker";
			this.uxNotifyPicker.Size = new System.Drawing.Size(200, 21);
			this.uxNotifyPicker.TabIndex = 5;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(366, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(52, 17);
			this.label1.TabIndex = 4;
			this.label1.Text = "Notify...";
			// 
			// uxUnlockOnSave
			// 
			this.uxUnlockOnSave.AutoSize = true;
			this.uxUnlockOnSave.Font = new System.Drawing.Font("Segoe UI", 8F);
			this.uxUnlockOnSave.Location = new System.Drawing.Point(8, 84);
			this.uxUnlockOnSave.Name = "uxUnlockOnSave";
			this.uxUnlockOnSave.Size = new System.Drawing.Size(142, 17);
			this.uxUnlockOnSave.TabIndex = 3;
			this.uxUnlockOnSave.Text = "Unlock Article On Save";
			this.uxUnlockOnSave.UseVisualStyleBackColor = true;
			// 
			// uxWorkflowActions
			// 
			this.uxWorkflowActions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.uxWorkflowActions.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.uxWorkflowActions.FormattingEnabled = true;
			this.uxWorkflowActions.Location = new System.Drawing.Point(8, 53);
			this.uxWorkflowActions.Name = "uxWorkflowActions";
			this.uxWorkflowActions.Size = new System.Drawing.Size(200, 25);
			this.uxWorkflowActions.TabIndex = 2;
			this.uxWorkflowActions.SelectedIndexChanged += new System.EventHandler(this.uxWorkflowActions_SelectedIndexChanged);
			// 
			// uxWorkflowLabel
			// 
			this.uxWorkflowLabel.AutoEllipsis = true;
			this.uxWorkflowLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.uxWorkflowLabel.Location = new System.Drawing.Point(8, 17);
			this.uxWorkflowLabel.Name = "uxWorkflowLabel";
			this.uxWorkflowLabel.Size = new System.Drawing.Size(150, 33);
			this.uxWorkflowLabel.TabIndex = 1;
			this.uxWorkflowLabel.Text = "Example";
			this.uxWorkflowLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// WorkflowControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Green;
			this.Controls.Add(this.WorkflowGroupBox);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.Name = "WorkflowControl";
			this.Size = new System.Drawing.Size(650, 131);
			this.WorkflowGroupBox.ResumeLayout(false);
			this.WorkflowGroupBox.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox WorkflowGroupBox;
		public System.Windows.Forms.CheckBox uxUnlockOnSave;
		private System.Windows.Forms.ComboBox uxWorkflowActions;
		private System.Windows.Forms.Label uxWorkflowLabel;
		private System.Windows.Forms.Button uxNotifyAdd;
		private System.Windows.Forms.ComboBox uxNotifyPicker;
		private System.Windows.Forms.Label label1;
		private EasyRemoveListView uxNotifyList;
	}
}
