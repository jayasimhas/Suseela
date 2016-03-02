namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
	partial class GeneralTagsControl
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
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.uxSuggestions = new System.Windows.Forms.ListBox();
			this.uxKeyword = new System.Windows.Forms.TextBox();
			this.uxPanelTagCloud = new System.Windows.Forms.Panel();
			this.uxTagCloud = new System.Windows.Forms.FlowLayoutPanel();
			this.label12 = new System.Windows.Forms.Label();
			this.uxSelectedTags = new System.Windows.Forms.TableLayoutPanel();
			this.uxPanelSelectedTags = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.uxPanelTagCloud.SuspendLayout();
			this.uxPanelSelectedTags.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
			this.label1.ForeColor = System.Drawing.Color.Gray;
			this.label1.Image = global::InformaSitecoreWord.Properties.Resources.generaltags_tabheader;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(610, 30);
			this.label1.TabIndex = 12;
			this.label1.Paint += new System.Windows.Forms.PaintEventHandler(this.label1_Paint);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.label9.ForeColor = System.Drawing.SystemColors.GrayText;
			this.label9.Location = new System.Drawing.Point(0, 261);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(66, 19);
			this.label9.TabIndex = 32;
			this.label9.Text = "Selected";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.label10.ForeColor = System.Drawing.SystemColors.GrayText;
			this.label10.Location = new System.Drawing.Point(3, 40);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(54, 19);
			this.label10.TabIndex = 30;
			this.label10.Text = "Search";
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.uxSuggestions);
			this.panel1.Controls.Add(this.uxKeyword);
			this.panel1.Controls.Add(this.uxPanelTagCloud);
			this.panel1.Controls.Add(this.label12);
			this.panel1.Location = new System.Drawing.Point(0, 60);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(610, 198);
			this.panel1.TabIndex = 31;
			// 
			// uxSuggestions
			// 
			this.uxSuggestions.FormattingEnabled = true;
			this.uxSuggestions.Location = new System.Drawing.Point(20, 55);
			this.uxSuggestions.Name = "uxSuggestions";
			this.uxSuggestions.Size = new System.Drawing.Size(567, 95);
			this.uxSuggestions.TabIndex = 0;
			// 
			// uxKeyword
			// 
			this.uxKeyword.Location = new System.Drawing.Point(20, 33);
			this.uxKeyword.Name = "uxKeyword";
			this.uxKeyword.Size = new System.Drawing.Size(574, 22);
			this.uxKeyword.TabIndex = 16;
			this.uxKeyword.TextChanged += new System.EventHandler(this.uxKeyword_TextChanged);
			this.uxKeyword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uxKeyword_KeyDown);
		//	this.uxKeyword.Leave += new System.EventHandler(this.uxKeyword_Leave);
			// 
			// uxPanelTagCloud
			// 
			this.uxPanelTagCloud.BackColor = System.Drawing.Color.White;
			this.uxPanelTagCloud.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.uxPanelTagCloud.Controls.Add(this.uxTagCloud);
			this.uxPanelTagCloud.Location = new System.Drawing.Point(20, 58);
			this.uxPanelTagCloud.Name = "uxPanelTagCloud";
			this.uxPanelTagCloud.Size = new System.Drawing.Size(574, 87);
			this.uxPanelTagCloud.TabIndex = 15;
			// 
			// uxTagCloud
			// 
			this.uxTagCloud.BackColor = System.Drawing.Color.White;
			this.uxTagCloud.Location = new System.Drawing.Point(-1, -1);
			this.uxTagCloud.Name = "uxTagCloud";
			this.uxTagCloud.Size = new System.Drawing.Size(574, 87);
			this.uxTagCloud.TabIndex = 20;
			this.uxTagCloud.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.uxTagCloud_ControlAdded);
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.label12.Location = new System.Drawing.Point(16, 11);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(79, 19);
			this.label12.TabIndex = 13;
			this.label12.Text = "Keywords:";
			// 
			// uxSelectedTags
			// 
			this.uxSelectedTags.AutoSize = true;
			this.uxSelectedTags.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.uxSelectedTags.ColumnCount = 2;
			this.uxSelectedTags.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.uxSelectedTags.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 570F));
			this.uxSelectedTags.Location = new System.Drawing.Point(0, 0);
			this.uxSelectedTags.Name = "uxSelectedTags";
			this.uxSelectedTags.RowCount = 1;
			this.uxSelectedTags.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.uxSelectedTags.Size = new System.Drawing.Size(590, 0);
			this.uxSelectedTags.TabIndex = 33;
			// 
			// uxPanelSelectedTags
			// 
			this.uxPanelSelectedTags.AutoScroll = true;
			this.uxPanelSelectedTags.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.uxPanelSelectedTags.Controls.Add(this.uxSelectedTags);
			this.uxPanelSelectedTags.Location = new System.Drawing.Point(0, 287);
			this.uxPanelSelectedTags.Name = "uxPanelSelectedTags";
			this.uxPanelSelectedTags.Size = new System.Drawing.Size(610, 145);
			this.uxPanelSelectedTags.TabIndex = 34;
			// 
			// GeneralTagsControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.uxPanelSelectedTags);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label9);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.Name = "GeneralTagsControl";
			this.Size = new System.Drawing.Size(610, 449);
			this.Load += new System.EventHandler(this.GeneralTagsControl_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.uxPanelTagCloud.ResumeLayout(false);
			this.uxPanelSelectedTags.ResumeLayout(false);
			this.uxPanelSelectedTags.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.FlowLayoutPanel uxTagCloud;
		private System.Windows.Forms.Panel uxPanelTagCloud;
		private System.Windows.Forms.TableLayoutPanel uxSelectedTags;
		private System.Windows.Forms.Panel uxPanelSelectedTags;
		private System.Windows.Forms.TextBox uxKeyword;
		private System.Windows.Forms.ListBox uxSuggestions;
	}
}
