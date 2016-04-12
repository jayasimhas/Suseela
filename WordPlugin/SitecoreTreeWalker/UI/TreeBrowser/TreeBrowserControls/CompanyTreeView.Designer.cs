namespace InformaSitecoreWord.UI.TreeBrowser.TreeBrowserControls
{
    partial class CompanyTreeView
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
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tabControl1 = new TablessTabControl();
            this.tabTreeView = new System.Windows.Forms.TabPage();
            this.tabResultsView = new System.Windows.Forms.TabPage();
            this.noFlickerListView1 = new NoFlickerListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkViewInTree = new System.Windows.Forms.LinkLabel();
            this.tabControl1.SuspendLayout();
            this.tabTreeView.SuspendLayout();
            this.tabResultsView.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(3, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(400, 290);
            this.treeView1.TabIndex = 0;
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabTreeView);
            this.tabControl1.Controls.Add(this.tabResultsView);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(414, 325);
            this.tabControl1.TabIndex = 1;
            // 
            // tabTreeView
            // 
            this.tabTreeView.Controls.Add(this.treeView1);
            this.tabTreeView.Location = new System.Drawing.Point(4, 25);
            this.tabTreeView.Name = "tabTreeView";
            this.tabTreeView.Padding = new System.Windows.Forms.Padding(3);
            this.tabTreeView.Size = new System.Drawing.Size(406, 296);
            this.tabTreeView.TabIndex = 0;
            this.tabTreeView.Text = "Tree View";
            this.tabTreeView.UseVisualStyleBackColor = true;
            // 
            // tabResultsView
            // 
            this.tabResultsView.Controls.Add(this.noFlickerListView1);
            this.tabResultsView.Controls.Add(this.panel1);
            this.tabResultsView.Location = new System.Drawing.Point(4, 25);
            this.tabResultsView.Name = "tabResultsView";
            this.tabResultsView.Padding = new System.Windows.Forms.Padding(3);
            this.tabResultsView.Size = new System.Drawing.Size(406, 296);
            this.tabResultsView.TabIndex = 1;
            this.tabResultsView.Text = "Results View";
            this.tabResultsView.UseVisualStyleBackColor = true;
            // 
            // noFlickerListView1
            // 
            this.noFlickerListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.noFlickerListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noFlickerListView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.noFlickerListView1.Location = new System.Drawing.Point(3, 30);
            this.noFlickerListView1.MultiSelect = false;
            this.noFlickerListView1.Name = "noFlickerListView1";
            this.noFlickerListView1.Size = new System.Drawing.Size(400, 263);
            this.noFlickerListView1.TabIndex = 0;
            this.noFlickerListView1.UseCompatibleStateImageBehavior = false;
            this.noFlickerListView1.View = System.Windows.Forms.View.Details;
            this.noFlickerListView1.SelectedIndexChanged += new System.EventHandler(this.noFlickerListView1_SelectedIndexChanged);
            this.noFlickerListView1.DoubleClick += new System.EventHandler(this.noFlickerListView1_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Company Name";
            this.columnHeader1.Width = 384;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.linkViewInTree);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(400, 27);
            this.panel1.TabIndex = 2;
            // 
            // linkViewInTree
            // 
            this.linkViewInTree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkViewInTree.AutoSize = true;
            this.linkViewInTree.Location = new System.Drawing.Point(326, 7);
            this.linkViewInTree.Name = "linkViewInTree";
            this.linkViewInTree.Size = new System.Drawing.Size(67, 13);
            this.linkViewInTree.TabIndex = 0;
            this.linkViewInTree.TabStop = true;
            this.linkViewInTree.Text = "View In Tree";
            this.linkViewInTree.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkViewInTree_LinkClicked);
            // 
            // CompanyTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "CompanyTreeView";
            this.Size = new System.Drawing.Size(414, 325);
            this.Load += new System.EventHandler(this.CompanyTreeView_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabTreeView.ResumeLayout(false);
            this.tabResultsView.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private TablessTabControl tabControl1;
        private System.Windows.Forms.TabPage tabTreeView;
        private System.Windows.Forms.TabPage tabResultsView;
        private NoFlickerListView noFlickerListView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel linkViewInTree;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}
