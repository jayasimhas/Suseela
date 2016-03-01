namespace InformaSitecoreWord.UI.TreeBrowser.TreeBrowserControls
{
	partial class SuggestedURL
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SuggestedURL));
			this.uxSuggestedUrlTB = new System.Windows.Forms.TextBox();
			this.uxSuggestedUrlLabel = new System.Windows.Forms.Label();
			this.uxTestUrlBtn = new System.Windows.Forms.Button();
			this.uxInsertUrlBtn = new System.Windows.Forms.Button();
			this.uxCloseSuggested = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// uxSuggestedUrlTB
			// 
			this.uxSuggestedUrlTB.Location = new System.Drawing.Point(12, 34);
			this.uxSuggestedUrlTB.Name = "uxSuggestedUrlTB";
			this.uxSuggestedUrlTB.Size = new System.Drawing.Size(463, 20);
			this.uxSuggestedUrlTB.TabIndex = 0;
			// 
			// uxSuggestedUrlLabel
			// 
			this.uxSuggestedUrlLabel.AutoSize = true;
			this.uxSuggestedUrlLabel.Location = new System.Drawing.Point(12, 13);
			this.uxSuggestedUrlLabel.Name = "uxSuggestedUrlLabel";
			this.uxSuggestedUrlLabel.Size = new System.Drawing.Size(83, 13);
			this.uxSuggestedUrlLabel.TabIndex = 1;
			this.uxSuggestedUrlLabel.Text = "Suggested URL";
			// 
			// uxTestUrlBtn
			// 
			this.uxTestUrlBtn.Location = new System.Drawing.Point(169, 60);
			this.uxTestUrlBtn.Name = "uxTestUrlBtn";
			this.uxTestUrlBtn.Size = new System.Drawing.Size(106, 23);
			this.uxTestUrlBtn.TabIndex = 2;
			this.uxTestUrlBtn.Text = "Open in browser";
			this.uxTestUrlBtn.UseVisualStyleBackColor = true;
			this.uxTestUrlBtn.Click += new System.EventHandler(this.uxTestUrlBtn_Click);
			// 
			// uxInsertUrlBtn
			// 
			this.uxInsertUrlBtn.Location = new System.Drawing.Point(281, 60);
			this.uxInsertUrlBtn.Name = "uxInsertUrlBtn";
			this.uxInsertUrlBtn.Size = new System.Drawing.Size(106, 23);
			this.uxInsertUrlBtn.TabIndex = 3;
			this.uxInsertUrlBtn.Text = "Use this URL";
			this.uxInsertUrlBtn.UseVisualStyleBackColor = true;
			this.uxInsertUrlBtn.Click += new System.EventHandler(this.uxInsertUrlBtn_Click);
			// 
			// uxCloseSuggested
			// 
			this.uxCloseSuggested.Location = new System.Drawing.Point(393, 60);
			this.uxCloseSuggested.Name = "uxCloseSuggested";
			this.uxCloseSuggested.Size = new System.Drawing.Size(75, 23);
			this.uxCloseSuggested.TabIndex = 4;
			this.uxCloseSuggested.Text = "Close";
			this.uxCloseSuggested.UseVisualStyleBackColor = true;
			this.uxCloseSuggested.Click += new System.EventHandler(this.uxCloseSuggested_Click);
			// 
			// SuggestedURL
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(487, 122);
			this.Controls.Add(this.uxCloseSuggested);
			this.Controls.Add(this.uxInsertUrlBtn);
			this.Controls.Add(this.uxTestUrlBtn);
			this.Controls.Add(this.uxSuggestedUrlLabel);
			this.Controls.Add(this.uxSuggestedUrlTB);
			this.ForeColor = System.Drawing.Color.Black;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(503, 160);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(503, 160);
			this.Name = "SuggestedURL";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Suggest Secure URL";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox uxSuggestedUrlTB;
		private System.Windows.Forms.Label uxSuggestedUrlLabel;
		private System.Windows.Forms.Button uxTestUrlBtn;
		private System.Windows.Forms.Button uxInsertUrlBtn;
		private System.Windows.Forms.Button uxCloseSuggested;
	}
}