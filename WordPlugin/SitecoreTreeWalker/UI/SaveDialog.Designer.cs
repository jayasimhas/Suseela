namespace InformaSitecoreWord.UI
{
	partial class SaveDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveDialog));
			this.uxSaveToSitecore = new System.Windows.Forms.Button();
			this.uxSaveLocal = new System.Windows.Forms.Button();
			this.uxDontSave = new System.Windows.Forms.Button();
			this.uxCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// uxSaveToSitecore
			// 
			this.uxSaveToSitecore.Location = new System.Drawing.Point(11, 46);
			this.uxSaveToSitecore.Name = "uxSaveToSitecore";
			this.uxSaveToSitecore.Size = new System.Drawing.Size(156, 23);
			this.uxSaveToSitecore.TabIndex = 0;
			this.uxSaveToSitecore.Text = "Save to Sitecore and Unlock";
			this.uxSaveToSitecore.UseVisualStyleBackColor = true;
			this.uxSaveToSitecore.Click += new System.EventHandler(this.uxSaveToSitecore_Click);
			// 
			// uxSaveLocal
			// 
			this.uxSaveLocal.Location = new System.Drawing.Point(173, 46);
			this.uxSaveLocal.Name = "uxSaveLocal";
			this.uxSaveLocal.Size = new System.Drawing.Size(113, 23);
			this.uxSaveLocal.TabIndex = 1;
			this.uxSaveLocal.Text = "Save a Local Copy";
			this.uxSaveLocal.UseVisualStyleBackColor = true;
			this.uxSaveLocal.Click += new System.EventHandler(this.uxSaveLocal_Click);
			// 
			// uxDontSave
			// 
			this.uxDontSave.Location = new System.Drawing.Point(292, 46);
			this.uxDontSave.Name = "uxDontSave";
			this.uxDontSave.Size = new System.Drawing.Size(75, 23);
			this.uxDontSave.TabIndex = 2;
			this.uxDontSave.Text = "Don\'t Save";
			this.uxDontSave.UseVisualStyleBackColor = true;
			this.uxDontSave.Click += new System.EventHandler(this.uxDontSave_Click);
			// 
			// uxCancel
			// 
			this.uxCancel.Location = new System.Drawing.Point(406, 46);
			this.uxCancel.Name = "uxCancel";
			this.uxCancel.Size = new System.Drawing.Size(75, 23);
			this.uxCancel.TabIndex = 3;
			this.uxCancel.Text = "Cancel";
			this.uxCancel.UseVisualStyleBackColor = true;
			this.uxCancel.Click += new System.EventHandler(this.uxCancel_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(470, 34);
			this.label1.TabIndex = 4;
			this.label1.Text = "You have unsaved changes to this article. Would you like to save the changes to S" +
    "itecore, or save a local copy of the document and transfer later?";
			// 
			// SaveDialog
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(494, 77);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.uxCancel);
			this.Controls.Add(this.uxDontSave);
			this.Controls.Add(this.uxSaveLocal);
			this.Controls.Add(this.uxSaveToSitecore);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "SaveDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Insight Platform - Unsaved Changes";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button uxSaveToSitecore;
		private System.Windows.Forms.Button uxSaveLocal;
		private System.Windows.Forms.Button uxDontSave;
		private System.Windows.Forms.Button uxCancel;
		private System.Windows.Forms.Label label1;
	}
}