using System.Drawing;
using InformaSitecoreWord.SitecoreTree;
using InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.Interfaces;

namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
	public partial class CompanyControl : ArticleDetailsPageUserControl
	{
		protected bool _isLive;
		public CompanyControl()
		{
			InitializeComponent();
		}

		public void UpdateFields(ArticleStruct articleDetails)
		{
			_isLive = articleDetails.IsPublished;
			label36.Refresh();
		}

		private void label36_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (_isLive)
			{
				e.Graphics.DrawImage(InformaSitecoreWord.Properties.Resources.live, 570, 1, 28, 28);
				e.Graphics.DrawString("Live!", new Font("SegoeUI", 18), Brushes.Green, 510, 1);
			}
		}
	}
}
