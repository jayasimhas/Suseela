using System.Drawing;
using System.Windows.Forms;
using SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.Interfaces;

namespace SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
	public partial class NotesControl : ArticleDetailsPageUserControl
	{
		protected bool _isLive;

		public NotesControl()
		{
			InitializeComponent();
		}

		public void ResetFields()
		{
			uxEditorNotes.Clear();
			uxProductionNotes.Clear();
		}

		public string GetNotesToProduction()
		{
			return uxProductionNotes.Text;
		}

		public string GetNotesToEditors()
		{
			return uxEditorNotes.Text;
		}

		public void PreLinkEnable()
		{
			uxEditorNotes.Enabled = false;
			uxProductionNotes.Enabled = false;
		}

		public void PostLinkEnable()
		{
			uxEditorNotes.Enabled = true;
			uxProductionNotes.Enabled = true;
		}

		public void UpdateFields(SitecoreTree.ArticleStruct articleStruct)
		{
			uxEditorNotes.Text = articleStruct.NotesToEditorial;
			_isLive = articleStruct.IsPublished;
			label1.Refresh();
		}

		private void label1_Paint(object sender, PaintEventArgs e)
		{
			if (_isLive)
			{
				e.Graphics.DrawImage(Properties.Resources.live, 570, 1, 28, 28);
				e.Graphics.DrawString("Live!", new Font("SegoeUI", 18), Brushes.Green, 510, 1);
			}
		}

		public void IndicateChanged()
		{
			if (MenuItem != null)
			{
				MenuItem.HasChanged = true;
				MenuItem.UpdateBackground();
			}
		}

		private void uxEditorNotes_TextChanged(object sender, System.EventArgs e)
		{
			IndicateChanged();
		}

		private void uxProductionNotes_TextChanged(object sender, System.EventArgs e)
		{
			IndicateChanged();
		}
	}
}
