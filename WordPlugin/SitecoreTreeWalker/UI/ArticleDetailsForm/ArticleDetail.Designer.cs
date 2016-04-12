using InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls;
using InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls;

namespace InformaSitecoreWord.UI.ArticleDetailsForm
{
    partial class ArticleDetail
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArticleDetail));
			this.uxLoginPanel = new System.Windows.Forms.Panel();
			this.uxArticlePanel = new System.Windows.Forms.Panel();
			this.uxVersionNumber = new System.Windows.Forms.Label();
			this.uxPreview = new System.Windows.Forms.Button();
			this.uxCreateArticle = new System.Windows.Forms.Button();
			this.uxSaveArticle = new System.Windows.Forms.Button();
			this.uxSaveMetadata = new System.Windows.Forms.Button();
			this.articleStatusBar1 = new ArticleStatusBar();
			this.articleDetailsPageSelector = new ArticleDetailsPageSelector();
			this.loginControl1 = new LoginControl();
			this.uxLoginPanel.SuspendLayout();
			this.uxArticlePanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// uxLoginPanel
			// 
			this.uxLoginPanel.BackColor = System.Drawing.Color.White;
			this.uxLoginPanel.Controls.Add(this.uxArticlePanel);
			this.uxLoginPanel.Controls.Add(this.loginControl1);
			this.uxLoginPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.uxLoginPanel.Location = new System.Drawing.Point(0, 0);
			this.uxLoginPanel.Name = "uxLoginPanel";
			this.uxLoginPanel.Size = new System.Drawing.Size(897, 636);
			this.uxLoginPanel.TabIndex = 0;
			// 
			// uxArticlePanel
			// 
			this.uxArticlePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.uxArticlePanel.Controls.Add(this.articleStatusBar1);
			this.uxArticlePanel.Controls.Add(this.uxVersionNumber);
			this.uxArticlePanel.Controls.Add(this.uxPreview);			
			this.uxArticlePanel.Controls.Add(this.uxCreateArticle);
			this.uxArticlePanel.Controls.Add(this.uxSaveArticle);
			this.uxArticlePanel.Controls.Add(this.uxSaveMetadata);
			this.uxArticlePanel.Controls.Add(this.articleDetailsPageSelector);
			this.uxArticlePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.uxArticlePanel.Location = new System.Drawing.Point(0, 0);
			this.uxArticlePanel.Name = "uxArticlePanel";
			this.uxArticlePanel.Size = new System.Drawing.Size(897, 636);
			this.uxArticlePanel.TabIndex = 3;
			this.uxArticlePanel.Visible = false;
			// 
			// uxVersionNumber
			// 
			this.uxVersionNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.uxVersionNumber.AutoSize = true;
			this.uxVersionNumber.Location = new System.Drawing.Point(809, 600);
			this.uxVersionNumber.Name = "uxVersionNumber";
			this.uxVersionNumber.Size = new System.Drawing.Size(76, 13);
			this.uxVersionNumber.TabIndex = 80;
			this.uxVersionNumber.Text = "xxx.xxx.xxx.xxx";
			// 
			// uxPreview
			// 
			this.uxPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.uxPreview.Location = new System.Drawing.Point(3, 380);
			this.uxPreview.Name = "uxPreview";
			this.uxPreview.Size = new System.Drawing.Size(178, 23);
			this.uxPreview.TabIndex = 77;
			this.uxPreview.Text = "Preview Article";
			this.uxPreview.UseVisualStyleBackColor = true;
			this.uxPreview.Click += new System.EventHandler(this.uxPreview_Click);
			// 
			// uxCreateArticle
			// 
			this.uxCreateArticle.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.uxCreateArticle.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.uxCreateArticle.Location = new System.Drawing.Point(3, 335);
			this.uxCreateArticle.Name = "uxCreateArticle";
			this.uxCreateArticle.Size = new System.Drawing.Size(178, 23);
			this.uxCreateArticle.TabIndex = 76;
			this.uxCreateArticle.Text = "Create Article";
			this.uxCreateArticle.UseVisualStyleBackColor = true;
			this.uxCreateArticle.Click += new System.EventHandler(this.uxCreateArticle_Click);
			// 
			// uxSaveArticle
			// 
			this.uxSaveArticle.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.uxSaveArticle.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.uxSaveArticle.Location = new System.Drawing.Point(3, 321);
			this.uxSaveArticle.Name = "uxSaveArticle";
			this.uxSaveArticle.Size = new System.Drawing.Size(178, 24);
			this.uxSaveArticle.TabIndex = 61;
			this.uxSaveArticle.Text = "Save Article";
			this.uxSaveArticle.UseVisualStyleBackColor = true;
			this.uxSaveArticle.Visible = false;
			this.uxSaveArticle.Click += new System.EventHandler(this.uxSaveAndTransfer_Click);
			// 
			// uxSaveMetadata
			// 
			this.uxSaveMetadata.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.uxSaveMetadata.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.uxSaveMetadata.Location = new System.Drawing.Point(3, 351);
			this.uxSaveMetadata.Name = "uxSaveMetadata";
			this.uxSaveMetadata.Size = new System.Drawing.Size(178, 23);
			this.uxSaveMetadata.TabIndex = 59;
			this.uxSaveMetadata.Text = "Save Metadata";
			this.uxSaveMetadata.UseVisualStyleBackColor = true;
			this.uxSaveMetadata.Visible = false;
			this.uxSaveMetadata.Click += new System.EventHandler(this.uxSaveMetadata_Click);
			// 
			// articleStatusBar1
			// 
			this.articleStatusBar1.Location = new System.Drawing.Point(187, 556);
			this.articleStatusBar1.Name = "articleStatusBar1";
			this.articleStatusBar1.Size = new System.Drawing.Size(707, 41);
			this.articleStatusBar1.TabIndex = 81;
			// 
			// articleDetailsPageSelector
			// 
			this.articleDetailsPageSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.articleDetailsPageSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.articleDetailsPageSelector.Location = new System.Drawing.Point(0, 3);
			this.articleDetailsPageSelector.Name = "articleDetailsPageSelector";
			this.articleDetailsPageSelector.Size = new System.Drawing.Size(898, 630);
			this.articleDetailsPageSelector.TabIndex = 78;
			// 
			// loginControl1
			// 
			this.loginControl1.BackColor = System.Drawing.Color.White;
			this.loginControl1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.loginControl1.Location = new System.Drawing.Point(200, 0);
			this.loginControl1.Name = "loginControl1";
			this.loginControl1.Size = new System.Drawing.Size(399, 648);
			this.loginControl1.TabIndex = 74;
			// 
			// ArticleDetail
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.ClientSize = new System.Drawing.Size(897, 636);
			this.Controls.Add(this.uxLoginPanel);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ArticleDetail";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Article Information";
			this.Load += new System.EventHandler(this.ArticleDetail_Load);
			this.uxLoginPanel.ResumeLayout(false);
			this.uxArticlePanel.ResumeLayout(false);
			this.uxArticlePanel.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion


        public System.Windows.Forms.Panel uxLoginPanel;
        public System.Windows.Forms.Panel uxArticlePanel;
        public System.Windows.Forms.Button uxSaveArticle;
        public System.Windows.Forms.Button uxSaveMetadata;
        public LoginControl loginControl1;
        public System.Windows.Forms.Button uxCreateArticle;
        public System.Windows.Forms.Button uxPreview;
		public ArticleDetailsPageSelector articleDetailsPageSelector;		
        public System.Windows.Forms.Label uxVersionNumber;
        public ArticleDetailsControls.PageUserControls.ArticleStatusBar articleStatusBar1;
    }
}