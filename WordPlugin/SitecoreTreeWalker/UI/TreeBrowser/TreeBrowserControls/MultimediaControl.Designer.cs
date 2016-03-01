namespace InformaSitecoreWord.UI.TreeBrowser.TreeBrowserControls
{
	partial class MultimediaControl
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
			this.uxViewFullSize = new System.Windows.Forms.Button();
			this.uxRefresh = new System.Windows.Forms.Button();
			this.HeaderPanel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.uxInsertMultimedia = new System.Windows.Forms.Button();
			this.multimediahelp = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uxPreview)).BeginInit();
			this.HeaderPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// uxBrowseImages
			// 
			this.uxBrowseImages.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.uxBrowseImages.Location = new System.Drawing.Point(4, 121);
			this.uxBrowseImages.Name = "uxBrowseImages";
			this.uxBrowseImages.Size = new System.Drawing.Size(295, 138);
			this.uxBrowseImages.TabIndex = 15;
			this.uxBrowseImages.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.NodeExpanded);
			this.uxBrowseImages.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.uxBrowseImages_AfterSelect);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.uxPreview);
			this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
			this.groupBox1.ForeColor = System.Drawing.Color.Gray;
			this.groupBox1.Location = new System.Drawing.Point(4, 265);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(304, 171);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Image Preview";
			// 
			// uxPreview
			// 
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
			this.uxInsertIntoArticle.Location = new System.Drawing.Point(97, 442);
			this.uxInsertIntoArticle.Name = "uxInsertIntoArticle";
			this.uxInsertIntoArticle.Size = new System.Drawing.Size(103, 23);
			this.uxInsertIntoArticle.TabIndex = 29;
			this.uxInsertIntoArticle.Text = "Insert Into Article";
			this.uxInsertIntoArticle.UseVisualStyleBackColor = true;
			this.uxInsertIntoArticle.Click += new System.EventHandler(this.uxInsertIntoArticle_Click);
			// 
			// uxViewFullSize
			// 
			this.uxViewFullSize.Location = new System.Drawing.Point(4, 442);
			this.uxViewFullSize.Name = "uxViewFullSize";
			this.uxViewFullSize.Size = new System.Drawing.Size(87, 23);
			this.uxViewFullSize.TabIndex = 28;
			this.uxViewFullSize.Text = "View Full Size";
			this.uxViewFullSize.UseVisualStyleBackColor = true;
			this.uxViewFullSize.Click += new System.EventHandler(this.uxViewFullSize_Click);
			// 
			// uxRefresh
			// 
			this.uxRefresh.Location = new System.Drawing.Point(209, 442);
			this.uxRefresh.Name = "uxRefresh";
			this.uxRefresh.Size = new System.Drawing.Size(78, 23);
			this.uxRefresh.TabIndex = 30;
			this.uxRefresh.Text = "Refresh";
			this.uxRefresh.UseVisualStyleBackColor = true;
			this.uxRefresh.Click += new System.EventHandler(this.uxRefresh_Click);
			// 
			// HeaderPanel1
			// 
			this.HeaderPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.HeaderPanel1.Controls.Add(this.label1);
			this.HeaderPanel1.Location = new System.Drawing.Point(0, 82);
			this.HeaderPanel1.Name = "HeaderPanel1";
			this.HeaderPanel1.Size = new System.Drawing.Size(803, 27);
			this.HeaderPanel1.TabIndex = 31;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
			this.label1.ForeColor = System.Drawing.Color.Gray;
			this.label1.Image = global::InformaSitecoreWord.Properties.Resources.graphics_tabheader;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(310, 24);
			this.label1.TabIndex = 11;
			// 
			// uxInsertMultimedia
			// 
			this.uxInsertMultimedia.Location = new System.Drawing.Point(67, 53);
			this.uxInsertMultimedia.Name = "uxInsertMultimedia";
			this.uxInsertMultimedia.Size = new System.Drawing.Size(143, 23);
			this.uxInsertMultimedia.TabIndex = 32;
			this.uxInsertMultimedia.Text = "Insert Multimedia";
			this.uxInsertMultimedia.UseVisualStyleBackColor = true;
			this.uxInsertMultimedia.Click += new System.EventHandler(this.uxInsertMultimedia_Click);
			// 
			// multimediahelp
			// 
			this.multimediahelp.AutoSize = true;
			this.multimediahelp.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			this.multimediahelp.Cursor = System.Windows.Forms.Cursors.Hand;
			this.multimediahelp.Font = new System.Drawing.Font("Segoe UI", 10.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
			this.multimediahelp.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.multimediahelp.Location = new System.Drawing.Point(75, 0);
			this.multimediahelp.Name = "multimediahelp";
			this.multimediahelp.Size = new System.Drawing.Size(16, 20);
			this.multimediahelp.TabIndex = 34;
			this.multimediahelp.Text = "?";
			this.toolTip1.SetToolTip(this.multimediahelp, "Embed video, audio or interactive content from sites like Youtube, Soundcloud, & Datawrapper. Click the '?' for more information.");
			this.multimediahelp.Click += new System.EventHandler(this.label3_Click);
			// 
			// toolTip1
			// 
			this.toolTip1.AutoPopDelay = 2000;
			this.toolTip1.InitialDelay = 500;
			this.toolTip1.ReshowDelay = 100;
			this.toolTip1.ToolTipTitle = "Multimedia Help";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 30);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(162, 13);
			this.label3.TabIndex = 35;
			this.label3.Text = "Videos, audio and interactives";
			// 
			// label2
			// 
			this.label2.Image = global::InformaSitecoreWord.Properties.Resources.multimedia_tabheader;
			this.label2.Location = new System.Drawing.Point(-1, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(310, 24);
			this.label2.TabIndex = 33;
			// 
            // MultimediaControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.label3);
			this.Controls.Add(this.multimediahelp);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.uxInsertMultimedia);
			this.Controls.Add(this.HeaderPanel1);
			this.Controls.Add(this.uxRefresh);
			this.Controls.Add(this.uxInsertIntoArticle);
			this.Controls.Add(this.uxViewFullSize);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.uxBrowseImages);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.Name = "MultimediaControl";
			this.Size = new System.Drawing.Size(311, 501);
			this.Load += new System.EventHandler(this.GraphicsControl_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uxPreview)).EndInit();
			this.HeaderPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TreeView uxBrowseImages;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.PictureBox uxPreview;
		private System.Windows.Forms.Button uxInsertIntoArticle;
		private System.Windows.Forms.Button uxViewFullSize;
		private System.Windows.Forms.Button uxRefresh;
		private System.Windows.Forms.Panel HeaderPanel1;
		private System.Windows.Forms.Button uxInsertMultimedia;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label multimediahelp;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Label label3;
		
		
	}
}
