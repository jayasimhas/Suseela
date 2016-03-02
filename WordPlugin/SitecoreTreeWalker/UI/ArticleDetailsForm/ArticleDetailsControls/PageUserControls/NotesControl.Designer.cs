namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
	partial class NotesControl
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
            this.uxProductionNotes = new System.Windows.Forms.RichTextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.uxEditorNotes = new System.Windows.Forms.RichTextBox();
            this.label84 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // uxProductionNotes
            // 
            this.uxProductionNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxProductionNotes.Location = new System.Drawing.Point(307, 64);
            this.uxProductionNotes.Name = "uxProductionNotes";
            this.uxProductionNotes.Size = new System.Drawing.Size(286, 67);
            this.uxProductionNotes.TabIndex = 36;
            this.uxProductionNotes.Text = "";
            this.uxProductionNotes.TextChanged += new System.EventHandler(this.uxProductionNotes_TextChanged);
            // 
            // label26
            // 
            this.label26.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.Location = new System.Drawing.Point(304, 45);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(110, 16);
            this.label26.TabIndex = 35;
            this.label26.Text = "Notes to Design:";
            // 
            // uxEditorNotes
            // 
            this.uxEditorNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxEditorNotes.Location = new System.Drawing.Point(3, 64);
            this.uxEditorNotes.Name = "uxEditorNotes";
            this.uxEditorNotes.Size = new System.Drawing.Size(277, 67);
            this.uxEditorNotes.TabIndex = 34;
            this.uxEditorNotes.Text = "";
            this.uxEditorNotes.TextChanged += new System.EventHandler(this.uxEditorNotes_TextChanged);
            // 
            // label84
            // 
            this.label84.AutoSize = true;
            this.label84.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label84.Location = new System.Drawing.Point(3, 45);
            this.label84.Name = "label84";
            this.label84.Size = new System.Drawing.Size(104, 16);
            this.label84.TabIndex = 33;
            this.label84.Text = "Notes to Editor:";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Image = global::InformaSitecoreWord.Properties.Resources.notes_tabheader;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(610, 30);
            this.label1.TabIndex = 30;
            this.label1.Paint += new System.Windows.Forms.PaintEventHandler(this.label1_Paint);
            // 
            // NotesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.uxProductionNotes);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.uxEditorNotes);
            this.Controls.Add(this.label84);
            this.Controls.Add(this.label1);
            this.Name = "NotesControl";
            this.Size = new System.Drawing.Size(610, 141);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RichTextBox uxProductionNotes;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.RichTextBox uxEditorNotes;
		private System.Windows.Forms.Label label84;
	}
}
