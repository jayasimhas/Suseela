namespace SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
	partial class SubjectsControl
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.uxArrowUp = new System.Windows.Forms.Label();
			this.uxArrowDown = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.uxSubjectsSelected = new System.Windows.Forms.ListView();
			this.panel2 = new System.Windows.Forms.Panel();
			this.uxSubjectsViewTree = new System.Windows.Forms.LinkLabel();
			this.uxSubjectsResults = new SitecoreTreeWalker.UI.NoFlickerListView();
			this.uxSubjectsResultsTree = new System.Windows.Forms.TreeView();
			this.uxSubjectsKeywords = new System.Windows.Forms.TextBox();
			this.uxSubjectsViewSearch = new System.Windows.Forms.LinkLabel();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BackColor = System.Drawing.Color.White;
			this.panel1.Controls.Add(this.uxArrowUp);
			this.panel1.Controls.Add(this.uxArrowDown);
			this.panel1.Controls.Add(this.label8);
			this.panel1.Controls.Add(this.uxSubjectsSelected);
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Controls.Add(this.label10);
			this.panel1.Controls.Add(this.label9);
			this.panel1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(610, 449);
			this.panel1.TabIndex = 0;
			// 
			// uxArrowUp
			// 
			this.uxArrowUp.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.uxArrowUp.Image = global::SitecoreTreeWalker.Properties.Resources.arrowup;
			this.uxArrowUp.Location = new System.Drawing.Point(316, 265);
			this.uxArrowUp.Name = "uxArrowUp";
			this.uxArrowUp.Padding = new System.Windows.Forms.Padding(2);
			this.uxArrowUp.Size = new System.Drawing.Size(32, 32);
			this.uxArrowUp.TabIndex = 45;
			// 
			// uxArrowDown
			// 
			this.uxArrowDown.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.uxArrowDown.Image = global::SitecoreTreeWalker.Properties.Resources.arrowdown;
			this.uxArrowDown.Location = new System.Drawing.Point(223, 265);
			this.uxArrowDown.Name = "uxArrowDown";
			this.uxArrowDown.Padding = new System.Windows.Forms.Padding(2);
			this.uxArrowDown.Size = new System.Drawing.Size(32, 32);
			this.uxArrowDown.TabIndex = 44;
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
			this.label8.ForeColor = System.Drawing.Color.Gray;
			this.label8.Image = global::SitecoreTreeWalker.Properties.Resources.subjects_tabheader;
			this.label8.Location = new System.Drawing.Point(0, 0);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(610, 30);
			this.label8.TabIndex = 31;
			this.label8.Paint += new System.Windows.Forms.PaintEventHandler(this.label8_Paint);
			// 
			// uxSubjectsSelected
			// 
			this.uxSubjectsSelected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.uxSubjectsSelected.Location = new System.Drawing.Point(0, 298);
			this.uxSubjectsSelected.Margin = new System.Windows.Forms.Padding(0);
			this.uxSubjectsSelected.MultiSelect = false;
			this.uxSubjectsSelected.Name = "uxSubjectsSelected";
			this.uxSubjectsSelected.Size = new System.Drawing.Size(610, 142);
			this.uxSubjectsSelected.TabIndex = 35;
			this.uxSubjectsSelected.UseCompatibleStateImageBehavior = false;
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel2.Controls.Add(this.uxSubjectsViewTree);
			this.panel2.Controls.Add(this.uxSubjectsResults);
			this.panel2.Controls.Add(this.uxSubjectsResultsTree);
			this.panel2.Controls.Add(this.uxSubjectsKeywords);
			this.panel2.Controls.Add(this.uxSubjectsViewSearch);
			this.panel2.Controls.Add(this.label12);
			this.panel2.Controls.Add(this.label13);
			this.panel2.Location = new System.Drawing.Point(0, 54);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(610, 208);
			this.panel2.TabIndex = 33;
			// 
			// uxSubjectsViewTree
			// 
			this.uxSubjectsViewTree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.uxSubjectsViewTree.AutoSize = true;
			this.uxSubjectsViewTree.Location = new System.Drawing.Point(538, 43);
			this.uxSubjectsViewTree.Name = "uxSubjectsViewTree";
			this.uxSubjectsViewTree.Size = new System.Drawing.Size(56, 13);
			this.uxSubjectsViewTree.TabIndex = 19;
			this.uxSubjectsViewTree.TabStop = true;
			this.uxSubjectsViewTree.Text = "View Tree";
			this.uxSubjectsViewTree.Visible = false;
			// 
			// uxSubjectsResults
			// 
			this.uxSubjectsResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.uxSubjectsResults.Location = new System.Drawing.Point(18, 58);
			this.uxSubjectsResults.MultiSelect = false;
			this.uxSubjectsResults.Name = "uxSubjectsResults";
			this.uxSubjectsResults.Size = new System.Drawing.Size(575, 141);
			this.uxSubjectsResults.TabIndex = 18;
			this.uxSubjectsResults.UseCompatibleStateImageBehavior = false;
			// 
			// uxSubjectsResultsTree
			// 
			this.uxSubjectsResultsTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.uxSubjectsResultsTree.HideSelection = false;
			this.uxSubjectsResultsTree.Location = new System.Drawing.Point(18, 58);
			this.uxSubjectsResultsTree.Name = "uxSubjectsResultsTree";
			this.uxSubjectsResultsTree.Size = new System.Drawing.Size(575, 141);
			this.uxSubjectsResultsTree.TabIndex = 17;
			this.uxSubjectsResultsTree.Visible = false;
			// 
			// uxSubjectsKeywords
			// 
			this.uxSubjectsKeywords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.uxSubjectsKeywords.Location = new System.Drawing.Point(18, 19);
			this.uxSubjectsKeywords.Name = "uxSubjectsKeywords";
			this.uxSubjectsKeywords.Size = new System.Drawing.Size(575, 22);
			this.uxSubjectsKeywords.TabIndex = 14;
			// 
			// uxSubjectsViewSearch
			// 
			this.uxSubjectsViewSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.uxSubjectsViewSearch.AutoSize = true;
			this.uxSubjectsViewSearch.Location = new System.Drawing.Point(488, 43);
			this.uxSubjectsViewSearch.Name = "uxSubjectsViewSearch";
			this.uxSubjectsViewSearch.Size = new System.Drawing.Size(109, 13);
			this.uxSubjectsViewSearch.TabIndex = 15;
			this.uxSubjectsViewSearch.TabStop = true;
			this.uxSubjectsViewSearch.Text = "View Search Results";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
			this.label12.Location = new System.Drawing.Point(15, 3);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(61, 13);
			this.label12.TabIndex = 13;
			this.label12.Text = "Keywords:";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
			this.label13.Location = new System.Drawing.Point(16, 44);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(47, 13);
			this.label13.TabIndex = 12;
			this.label13.Text = "Results:";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.label10.ForeColor = System.Drawing.SystemColors.GrayText;
			this.label10.Location = new System.Drawing.Point(3, 34);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(54, 19);
			this.label10.TabIndex = 32;
			this.label10.Text = "Search";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.label9.ForeColor = System.Drawing.SystemColors.GrayText;
			this.label9.Location = new System.Drawing.Point(3, 278);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(66, 19);
			this.label9.TabIndex = 34;
			this.label9.Text = "Selected";
			// 
			// SubjectsControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Name = "SubjectsControl";
			this.Size = new System.Drawing.Size(610, 449);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.ListView uxSubjectsSelected;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.LinkLabel uxSubjectsViewTree;
		private NoFlickerListView uxSubjectsResults;
		private System.Windows.Forms.TreeView uxSubjectsResultsTree;
		private System.Windows.Forms.TextBox uxSubjectsKeywords;
		private System.Windows.Forms.LinkLabel uxSubjectsViewSearch;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label uxArrowUp;
		private System.Windows.Forms.Label uxArrowDown;
	}
}
