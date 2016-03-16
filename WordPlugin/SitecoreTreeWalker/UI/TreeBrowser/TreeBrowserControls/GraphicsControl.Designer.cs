namespace InformaSitecoreWord.UI.TreeBrowser.TreeBrowserControls
{
	partial class GraphicsControl
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
			this.uxBrowseImages = new System.Windows.Forms.TreeView();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.uxPreview = new System.Windows.Forms.PictureBox();
			this.uxInsertIntoArticle = new System.Windows.Forms.Button();
			this.uxRefresh = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uxPreview)).BeginInit();
			this.SuspendLayout();
			// 
			// uxBrowseImages
			// 
			this.uxBrowseImages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.uxBrowseImages.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.uxBrowseImages.Location = new System.Drawing.Point(4, 36);
			this.uxBrowseImages.Name = "uxBrowseImages";
			this.uxBrowseImages.Size = new System.Drawing.Size(303, 138);
			this.uxBrowseImages.TabIndex = 15;
			this.uxBrowseImages.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.NodeExpanded);
			this.uxBrowseImages.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.uxBrowseImages_AfterSelect);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.uxPreview);
			this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
			this.groupBox1.ForeColor = System.Drawing.Color.Gray;
			this.groupBox1.Location = new System.Drawing.Point(3, 180);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(304, 171);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Image Preview";
			// 
			// uxPreview
			// 
			this.uxPreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.uxPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.uxPreview.Location = new System.Drawing.Point(0, 21);
			this.uxPreview.Name = "uxPreview";
			this.uxPreview.Size = new System.Drawing.Size(295, 144);
			this.uxPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.uxPreview.TabIndex = 2;
			this.uxPreview.TabStop = false;
			this.uxPreview.Visible = false;
			// 
			// uxInsertIntoArticle
			// 
			this.uxInsertIntoArticle.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.uxInsertIntoArticle.Location = new System.Drawing.Point(82, 357);
			this.uxInsertIntoArticle.Name = "uxInsertIntoArticle";
			this.uxInsertIntoArticle.Size = new System.Drawing.Size(103, 23);
			this.uxInsertIntoArticle.TabIndex = 29;
			this.uxInsertIntoArticle.Text = "Insert";
			this.uxInsertIntoArticle.UseVisualStyleBackColor = true;
			this.uxInsertIntoArticle.Click += new System.EventHandler(this.uxInsertIntoArticle_Click);
			// 
			// uxRefresh
			// 
			this.uxRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.uxRefresh.Location = new System.Drawing.Point(82, 386);
			this.uxRefresh.Name = "uxRefresh";
			this.uxRefresh.Size = new System.Drawing.Size(103, 23);
			this.uxRefresh.TabIndex = 30;
			this.uxRefresh.Text = "Refresh";
			this.uxRefresh.UseVisualStyleBackColor = true;
			this.uxRefresh.Click += new System.EventHandler(this.uxRefresh_Click);
			// 
			// toolTip1
			// 
			this.toolTip1.AutoPopDelay = 2000;
			this.toolTip1.InitialDelay = 500;
			this.toolTip1.ReshowDelay = 100;
			this.toolTip1.ToolTipTitle = "Multimedia Help";
			// 
			// GraphicsControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.uxRefresh);
			this.Controls.Add(this.uxInsertIntoArticle);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.uxBrowseImages);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.Name = "GraphicsControl";
			this.Size = new System.Drawing.Size(319, 452);
			this.Load += new System.EventHandler(this.GraphicsControl_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uxPreview)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TreeView uxBrowseImages;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.PictureBox uxPreview;
		private System.Windows.Forms.Button uxInsertIntoArticle;
		private System.Windows.Forms.Button uxRefresh;
        private System.Windows.Forms.ToolTip toolTip1;
		
		
	}
}
