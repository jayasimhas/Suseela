namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
	partial class ArticleWorkflowInfo
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
            this.uxPublishedOn = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.uxWorkflowState = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // uxPublishedOn
            // 
            this.uxPublishedOn.AutoSize = true;
            this.uxPublishedOn.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.uxPublishedOn.Location = new System.Drawing.Point(111, 53);
            this.uxPublishedOn.Name = "uxPublishedOn";
            this.uxPublishedOn.Size = new System.Drawing.Size(13, 13);
            this.uxPublishedOn.TabIndex = 6;            
            this.uxPublishedOn.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label3.Location = new System.Drawing.Point(15, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Published on: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Workflow State: ";
            // 
            // uxWorkflowState
            // 
            this.uxWorkflowState.AutoSize = true;
            this.uxWorkflowState.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.uxWorkflowState.Location = new System.Drawing.Point(114, 30);
            this.uxWorkflowState.Name = "uxWorkflowState";
            this.uxWorkflowState.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.uxWorkflowState.Size = new System.Drawing.Size(13, 13);
            this.uxWorkflowState.TabIndex = 7;            
            this.uxWorkflowState.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ArticleWorkflowInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 100);
            this.Controls.Add(this.uxWorkflowState);
            this.Controls.Add(this.uxPublishedOn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ArticleWorkflowInfo";
            this.ShowIcon = false;
            this.Text = "Workflow State";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label uxPublishedOn;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label uxWorkflowState;
	}
}