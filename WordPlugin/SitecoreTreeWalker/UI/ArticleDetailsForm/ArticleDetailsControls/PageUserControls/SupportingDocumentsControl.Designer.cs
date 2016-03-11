namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
	partial class SupportingDocumentsControl
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
			this.label1 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.uxBrowse = new System.Windows.Forms.TreeView();
			this.label53 = new System.Windows.Forms.Label();
			this.uxSelected = new System.Windows.Forms.ListView();
			this.label5 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
			this.label1.ForeColor = System.Drawing.Color.Gray;
			this.label1.Image = global::InformaSitecoreWord.Properties.Resources.supportingdocs_tabheader;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(610, 30);
			this.label1.TabIndex = 28;
			this.label1.Paint += new System.Windows.Forms.PaintEventHandler(this.label1_Paint);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.button2);
			this.panel1.Controls.Add(this.button1);
			this.panel1.Controls.Add(this.uxBrowse);
			this.panel1.Controls.Add(this.label53);
			this.panel1.Location = new System.Drawing.Point(0, 38);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(610, 197);
			this.panel1.TabIndex = 27;
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Location = new System.Drawing.Point(301, 166);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(98, 23);
			this.button2.TabIndex = 35;
			this.button2.Text = "View Document";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(202, 166);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(93, 23);
			this.button1.TabIndex = 34;
			this.button1.Text = "Add Document";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// uxBrowse
			// 
			this.uxBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.uxBrowse.Location = new System.Drawing.Point(3, 28);
			this.uxBrowse.Name = "uxBrowse";
			this.uxBrowse.Size = new System.Drawing.Size(602, 132);
			this.uxBrowse.TabIndex = 33;
			// 
			// label53
			// 
			this.label53.AutoSize = true;
			this.label53.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.label53.ForeColor = System.Drawing.SystemColors.GrayText;
			this.label53.Location = new System.Drawing.Point(3, 9);
			this.label53.Name = "label53";
			this.label53.Size = new System.Drawing.Size(239, 19);
			this.label53.TabIndex = 32;
			this.label53.Text = "Browse for Supporting Documents";
			// 
			// uxSelected
			// 
			this.uxSelected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.uxSelected.Location = new System.Drawing.Point(-1, 257);
			this.uxSelected.Name = "uxSelected";
			this.uxSelected.Size = new System.Drawing.Size(610, 162);
			this.uxSelected.TabIndex = 31;
			this.uxSelected.UseCompatibleStateImageBehavior = false;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.label5.ForeColor = System.Drawing.SystemColors.GrayText;
			this.label5.Location = new System.Drawing.Point(3, 238);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(66, 19);
			this.label5.TabIndex = 30;
			this.label5.Text = "Selected";
			// 
			// SupportingDocumentsControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.uxSelected);
			this.Controls.Add(this.label5);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.Name = "SupportingDocumentsControl";
			this.Size = new System.Drawing.Size(610, 449);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView uxSelected;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label53;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TreeView uxBrowse;
	}
}
