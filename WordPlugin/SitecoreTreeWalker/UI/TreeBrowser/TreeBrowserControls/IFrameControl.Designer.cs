namespace InformaSitecoreWord.UI.TreeBrowser.TreeBrowserControls
{
	partial class IFrameControl
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
			this.mobileEmbed = new System.Windows.Forms.TextBox();
			this.uxMobilehttpsPreview = new System.Windows.Forms.Button();
			this.uxMobileError = new System.Windows.Forms.Label();
			this.uxInsertIFrame = new System.Windows.Forms.Button();
			this.uxIFrameHeader = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.uxIFrameTitle = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.uxIFrameCaption = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.uxIFrameSource = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.desktopEmbed = new System.Windows.Forms.TextBox();
			this.uxDesktophttpsPreview = new System.Windows.Forms.Button();
			this.uxDesktopError = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.headerLabel = new System.Windows.Forms.Label();
			this.titleLabel = new System.Windows.Forms.Label();
			this.captionLabel = new System.Windows.Forms.Label();
			this.sourceLabel = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// mobileEmbed
			// 
			this.mobileEmbed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mobileEmbed.Font = new System.Drawing.Font("Consolas", 10.5F);
			this.mobileEmbed.Location = new System.Drawing.Point(19, 194);
			this.mobileEmbed.Multiline = true;
			this.mobileEmbed.Name = "mobileEmbed";
			this.mobileEmbed.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.mobileEmbed.Size = new System.Drawing.Size(291, 79);
			this.mobileEmbed.TabIndex = 44;
			this.mobileEmbed.TextChanged += new System.EventHandler(this.mobileEmbed_TextChanged);
			this.mobileEmbed.GotFocus += new System.EventHandler(this.uxMobileEmbed_Focus);
			this.mobileEmbed.LostFocus += new System.EventHandler(this.uxMobileEmbed_LostFocus);
			// 
			// uxMobilehttpsPreview
			// 
			this.uxMobilehttpsPreview.Location = new System.Drawing.Point(159, 279);
			this.uxMobilehttpsPreview.Name = "uxMobilehttpsPreview";
			this.uxMobilehttpsPreview.Size = new System.Drawing.Size(151, 23);
			this.uxMobilehttpsPreview.TabIndex = 45;
			this.uxMobilehttpsPreview.Text = "Suggest Secure URL";
			this.uxMobilehttpsPreview.UseVisualStyleBackColor = true;
			this.uxMobilehttpsPreview.Visible = false;
			this.uxMobilehttpsPreview.Click += new System.EventHandler(this.uxMobilehttpsPreview_Click);
			// 
			// uxMobileError
			// 
			this.uxMobileError.AutoSize = true;
			this.uxMobileError.ForeColor = System.Drawing.Color.Red;
			this.uxMobileError.Location = new System.Drawing.Point(15, 861);
			this.uxMobileError.MaximumSize = new System.Drawing.Size(250, 0);
			this.uxMobileError.Name = "uxMobileError";
			this.uxMobileError.Size = new System.Drawing.Size(0, 13);
			this.uxMobileError.TabIndex = 3;
			// 
			// uxInsertIFrame
			// 
			this.uxInsertIFrame.Location = new System.Drawing.Point(105, 812);
			this.uxInsertIFrame.Name = "uxInsertIFrame";
			this.uxInsertIFrame.Size = new System.Drawing.Size(117, 23);
			this.uxInsertIFrame.TabIndex = 46;
			this.uxInsertIFrame.Text = "Insert";
			this.uxInsertIFrame.UseVisualStyleBackColor = true;
			this.uxInsertIFrame.Click += new System.EventHandler(this.uxInsertIFrame_Click);
			// 
			// uxIFrameHeader
			// 
			this.uxIFrameHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.uxIFrameHeader.Font = new System.Drawing.Font("Arial", 10.5F);
			this.uxIFrameHeader.Location = new System.Drawing.Point(19, 322);
			this.uxIFrameHeader.Name = "uxIFrameHeader";
			this.uxIFrameHeader.Size = new System.Drawing.Size(291, 24);
			this.uxIFrameHeader.TabIndex = 34;
			this.uxIFrameHeader.TextChanged += new System.EventHandler(this.header_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(19, 302);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(44, 13);
			this.label2.TabIndex = 35;
			this.label2.Text = "Header";
			// 
			// uxIFrameTitle
			// 
			this.uxIFrameTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.uxIFrameTitle.Font = new System.Drawing.Font("Arial Black", 10F);
			this.uxIFrameTitle.Location = new System.Drawing.Point(19, 373);
			this.uxIFrameTitle.Name = "uxIFrameTitle";
			this.uxIFrameTitle.Size = new System.Drawing.Size(288, 26);
			this.uxIFrameTitle.TabIndex = 36;
			this.uxIFrameTitle.TextChanged += new System.EventHandler(this.title_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(16, 352);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(28, 13);
			this.label3.TabIndex = 37;
			this.label3.Text = "Title";
			// 
			// uxIFrameCaption
			// 
			this.uxIFrameCaption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.uxIFrameCaption.Font = new System.Drawing.Font("Arial", 10.5F);
			this.uxIFrameCaption.Location = new System.Drawing.Point(19, 425);
			this.uxIFrameCaption.Name = "uxIFrameCaption";
			this.uxIFrameCaption.Size = new System.Drawing.Size(288, 24);
			this.uxIFrameCaption.TabIndex = 38;
			this.uxIFrameCaption.TextChanged += new System.EventHandler(this.caption_TextChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(16, 403);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 13);
			this.label4.TabIndex = 39;
			this.label4.Text = "Caption";
			// 
			// uxIFrameSource
			// 
			this.uxIFrameSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.uxIFrameSource.Font = new System.Drawing.Font("Arial", 10.5F);
			this.uxIFrameSource.Location = new System.Drawing.Point(19, 473);
			this.uxIFrameSource.Name = "uxIFrameSource";
			this.uxIFrameSource.Size = new System.Drawing.Size(288, 24);
			this.uxIFrameSource.TabIndex = 40;
			this.uxIFrameSource.TextChanged += new System.EventHandler(this.source_TextChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(17, 452);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(42, 13);
			this.label5.TabIndex = 41;
			this.label5.Text = "Source";
			// 
			// desktopEmbed
			// 
			this.desktopEmbed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.desktopEmbed.Font = new System.Drawing.Font("Consolas", 10.5F);
			this.desktopEmbed.Location = new System.Drawing.Point(19, 58);
			this.desktopEmbed.Multiline = true;
			this.desktopEmbed.Name = "desktopEmbed";
			this.desktopEmbed.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.desktopEmbed.Size = new System.Drawing.Size(288, 82);
			this.desktopEmbed.TabIndex = 42;
			this.desktopEmbed.GotFocus += new System.EventHandler(this.uxDesktopEmbed_Focus);
			this.desktopEmbed.LostFocus += new System.EventHandler(this.uxDesktopEmbed_LostFocus);
			// 
			// uxDesktophttpsPreview
			// 
			this.uxDesktophttpsPreview.Location = new System.Drawing.Point(163, 146);
			this.uxDesktophttpsPreview.Name = "uxDesktophttpsPreview";
			this.uxDesktophttpsPreview.Size = new System.Drawing.Size(144, 23);
			this.uxDesktophttpsPreview.TabIndex = 44;
			this.uxDesktophttpsPreview.Text = "Suggest Secure URL";
			this.uxDesktophttpsPreview.UseVisualStyleBackColor = true;
			this.uxDesktophttpsPreview.Visible = false;
			this.uxDesktophttpsPreview.Click += new System.EventHandler(this.uxDesktophttpsPreview_Click);
			// 
			// uxDesktopError
			// 
			this.uxDesktopError.AutoSize = true;
			this.uxDesktopError.ForeColor = System.Drawing.Color.Red;
			this.uxDesktopError.Location = new System.Drawing.Point(16, 848);
			this.uxDesktopError.MaximumSize = new System.Drawing.Size(250, 0);
			this.uxDesktopError.Name = "uxDesktopError";
			this.uxDesktopError.Size = new System.Drawing.Size(0, 13);
			this.uxDesktopError.TabIndex = 43;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(17, 33);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 13);
			this.label1.TabIndex = 49;
			this.label1.Text = "Desktop Embed";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(16, 169);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(81, 13);
			this.label6.TabIndex = 50;
			this.label6.Text = "Mobile Embed";
			// 
			// headerLabel
			// 
			this.headerLabel.AutoSize = true;
			this.headerLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
			this.headerLabel.Location = new System.Drawing.Point(10, 35);
			this.headerLabel.Name = "headerLabel";
			this.headerLabel.Size = new System.Drawing.Size(44, 13);
			this.headerLabel.TabIndex = 52;
			this.headerLabel.Text = "Header";
			// 
			// titleLabel
			// 
			this.titleLabel.AutoSize = true;
			this.titleLabel.Location = new System.Drawing.Point(10, 59);
			this.titleLabel.Name = "titleLabel";
			this.titleLabel.Size = new System.Drawing.Size(28, 13);
			this.titleLabel.TabIndex = 53;
			this.titleLabel.Text = "Title";
			// 
			// captionLabel
			// 
			this.captionLabel.AutoSize = true;
			this.captionLabel.Location = new System.Drawing.Point(10, 253);
			this.captionLabel.Name = "captionLabel";
			this.captionLabel.Size = new System.Drawing.Size(48, 13);
			this.captionLabel.TabIndex = 54;
			this.captionLabel.Text = "Caption";
			// 
			// sourceLabel
			// 
			this.sourceLabel.AutoSize = true;
			this.sourceLabel.Location = new System.Drawing.Point(10, 278);
			this.sourceLabel.Name = "sourceLabel";
			this.sourceLabel.Size = new System.Drawing.Size(42, 13);
			this.sourceLabel.TabIndex = 55;
			this.sourceLabel.Text = "Source";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.headerLabel);
			this.groupBox1.Controls.Add(this.captionLabel);
			this.groupBox1.Controls.Add(this.sourceLabel);
			this.groupBox1.Controls.Add(this.titleLabel);
			this.groupBox1.Controls.Add(this.pictureBox1);
			this.groupBox1.Location = new System.Drawing.Point(6, 503);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(301, 303);
			this.groupBox1.TabIndex = 57;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Preview";
			// 
			// pictureBox1
			// 
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox1.Image = global::InformaSitecoreWord.Properties.Resources.imformaVideoPlaceholder2;
			this.pictureBox1.InitialImage = global::InformaSitecoreWord.Properties.Resources.ExampleEmbed;
			this.pictureBox1.Location = new System.Drawing.Point(13, 80);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(282, 166);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 42;
			this.pictureBox1.TabStop = false;
			// 
			// IFrameControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoScroll = true;
			this.AutoSize = true;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.uxMobileError);
			this.Controls.Add(this.uxMobilehttpsPreview);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.uxDesktopError);
			this.Controls.Add(this.mobileEmbed);
			this.Controls.Add(this.uxDesktophttpsPreview);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.desktopEmbed);
			this.Controls.Add(this.uxIFrameSource);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.uxIFrameCaption);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.uxIFrameTitle);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.uxIFrameHeader);
			this.Controls.Add(this.uxInsertIFrame);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.Name = "IFrameControl";
			this.Size = new System.Drawing.Size(330, 885);
			this.Load += new System.EventHandler(this.IFrameControl_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox mobileEmbed;
		private System.Windows.Forms.Button uxInsertIFrame;		
		private System.Windows.Forms.TextBox uxIFrameHeader;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox uxIFrameTitle;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox uxIFrameCaption;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox uxIFrameSource;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.TextBox desktopEmbed;
		private System.Windows.Forms.Label uxMobileError;
		private System.Windows.Forms.Button uxMobilehttpsPreview;
		private System.Windows.Forms.Button uxDesktophttpsPreview;
		private System.Windows.Forms.Label uxDesktopError;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label headerLabel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label captionLabel;
        private System.Windows.Forms.Label sourceLabel;
        private System.Windows.Forms.GroupBox groupBox1;
	}
}
