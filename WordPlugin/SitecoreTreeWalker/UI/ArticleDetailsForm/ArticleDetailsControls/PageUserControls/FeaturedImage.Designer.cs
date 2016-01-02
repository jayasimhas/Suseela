namespace SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
    partial class FeaturedImage
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.uxFeaturedImageTreeView = new System.Windows.Forms.TreeView();
            this.sourceTxtBox = new System.Windows.Forms.TextBox();
            this.captionTxtBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.alttextLbl = new System.Windows.Forms.Label();
            this.alttextLblHeader = new System.Windows.Forms.Label();
            this.filenameLbl = new System.Windows.Forms.Label();
            this.filenameLblHeader = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(6, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(221, 125);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.uxFeaturedImageTreeView);
            this.groupBox1.Controls.Add(this.sourceTxtBox);
            this.groupBox1.Controls.Add(this.captionTxtBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(403, 347);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Featured Image";
            // 
            // uxFeaturedImageTreeView
            // 
            this.uxFeaturedImageTreeView.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.uxFeaturedImageTreeView.Location = new System.Drawing.Point(17, 19);
            this.uxFeaturedImageTreeView.Name = "uxFeaturedImageTreeView";
            this.uxFeaturedImageTreeView.Size = new System.Drawing.Size(372, 112);
            this.uxFeaturedImageTreeView.TabIndex = 10;
            this.uxFeaturedImageTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.NodeExpanded);
            this.uxFeaturedImageTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.uxBrowseImages_AfterSelect);
            // 
            // sourceTxtBox
            // 
            this.sourceTxtBox.Location = new System.Drawing.Point(89, 314);
            this.sourceTxtBox.Name = "sourceTxtBox";
            this.sourceTxtBox.Size = new System.Drawing.Size(300, 20);
            this.sourceTxtBox.TabIndex = 8;
            // 
            // captionTxtBox
            // 
            this.captionTxtBox.Location = new System.Drawing.Point(89, 290);
            this.captionTxtBox.Name = "captionTxtBox";
            this.captionTxtBox.Size = new System.Drawing.Size(300, 20);
            this.captionTxtBox.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(14, 317);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Source :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(14, 293);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Caption :";
            // 
            // alttextLbl
            // 
            this.alttextLbl.AutoSize = true;
            this.alttextLbl.Location = new System.Drawing.Point(233, 87);
            this.alttextLbl.Name = "alttextLbl";
            this.alttextLbl.Size = new System.Drawing.Size(49, 13);
            this.alttextLbl.TabIndex = 4;
            this.alttextLbl.Text = "alttextLbl";
            // 
            // alttextLblHeader
            // 
            this.alttextLblHeader.AutoSize = true;
            this.alttextLblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alttextLblHeader.Location = new System.Drawing.Point(233, 60);
            this.alttextLblHeader.Name = "alttextLblHeader";
            this.alttextLblHeader.Size = new System.Drawing.Size(51, 13);
            this.alttextLblHeader.TabIndex = 3;
            this.alttextLblHeader.Text = "Alt Text";
            // 
            // filenameLbl
            // 
            this.filenameLbl.AutoSize = true;
            this.filenameLbl.Location = new System.Drawing.Point(233, 38);
            this.filenameLbl.Name = "filenameLbl";
            this.filenameLbl.Size = new System.Drawing.Size(60, 13);
            this.filenameLbl.TabIndex = 2;
            this.filenameLbl.Text = "filenameLbl";
            // 
            // filenameLblHeader
            // 
            this.filenameLblHeader.AutoSize = true;
            this.filenameLblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filenameLblHeader.Location = new System.Drawing.Point(233, 16);
            this.filenameLblHeader.Name = "filenameLblHeader";
            this.filenameLblHeader.Size = new System.Drawing.Size(63, 13);
            this.filenameLblHeader.TabIndex = 1;
            this.filenameLblHeader.Text = "File Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.filenameLblHeader);
            this.groupBox2.Controls.Add(this.filenameLbl);
            this.groupBox2.Controls.Add(this.alttextLblHeader);
            this.groupBox2.Controls.Add(this.alttextLbl);
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Location = new System.Drawing.Point(17, 137);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(372, 147);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Preview";
            // 
            // FeaturedImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.groupBox1);
            this.Name = "FeaturedImage";
            this.Size = new System.Drawing.Size(413, 359);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox sourceTxtBox;
        private System.Windows.Forms.TextBox captionTxtBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label alttextLbl;
        private System.Windows.Forms.Label alttextLblHeader;
        private System.Windows.Forms.Label filenameLbl;
        private System.Windows.Forms.Label filenameLblHeader;
        private System.Windows.Forms.TreeView uxFeaturedImageTreeView;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}
