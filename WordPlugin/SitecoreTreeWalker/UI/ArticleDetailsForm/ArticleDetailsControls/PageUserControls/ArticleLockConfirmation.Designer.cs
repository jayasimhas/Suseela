namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
    partial class ArticleLockConfirmation
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
            this.lblLine1 = new System.Windows.Forms.Label();
            this.lblLine2 = new System.Windows.Forms.Label();
            this.lblLine3 = new System.Windows.Forms.Label();
            this.btnYes = new System.Windows.Forms.Button();
            this.BtnNo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblLine1
            // 
            this.lblLine1.AutoSize = true;
            this.lblLine1.Location = new System.Drawing.Point(36, 25);
            this.lblLine1.Name = "lblLine1";
            this.lblLine1.Size = new System.Drawing.Size(335, 13);
            this.lblLine1.TabIndex = 0;
            this.lblLine1.Text = "This article is currently unlocked. Would you like to lock it and edit it ?";
            // 
            // lblLine2
            // 
            this.lblLine2.AutoSize = true;
            this.lblLine2.Location = new System.Drawing.Point(36, 53);
            this.lblLine2.Name = "lblLine2";
            this.lblLine2.Size = new System.Drawing.Size(548, 13);
            this.lblLine2.TabIndex = 1;
            this.lblLine2.Text = "If you select \'Yes\' it will be locked for you and will remain locked until you un" +
    "lock it or move it through the Workflow.";
            // 
            // lblLine3
            // 
            this.lblLine3.AutoSize = true;
            this.lblLine3.Location = new System.Drawing.Point(36, 79);
            this.lblLine3.Name = "lblLine3";
            this.lblLine3.Size = new System.Drawing.Size(264, 13);
            this.lblLine3.TabIndex = 2;
            this.lblLine3.Text = "If you select no, the article will open in read only mode.";
            // 
            // btnYes
            // 
            this.btnYes.Location = new System.Drawing.Point(225, 108);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(75, 23);
            this.btnYes.TabIndex = 3;
            this.btnYes.Text = "Yes";
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // BtnNo
            // 
            this.BtnNo.Location = new System.Drawing.Point(329, 108);
            this.BtnNo.Name = "BtnNo";
            this.BtnNo.Size = new System.Drawing.Size(75, 23);
            this.BtnNo.TabIndex = 4;
            this.BtnNo.Text = "No";
            this.BtnNo.UseVisualStyleBackColor = true;
            this.BtnNo.Click += new System.EventHandler(this.BtnNo_Click);
            // 
            // ArticleLockConfirmation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 145);
            this.Controls.Add(this.BtnNo);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.lblLine3);
            this.Controls.Add(this.lblLine2);
            this.Controls.Add(this.lblLine1);
            this.Name = "ArticleLockConfirmation";
            this.Text = "Insight Platform";
            this.Load += new System.EventHandler(this.ArticleLockConfirmation_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLine1;
        private System.Windows.Forms.Label lblLine2;
        private System.Windows.Forms.Label lblLine3;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button BtnNo;
    }
}