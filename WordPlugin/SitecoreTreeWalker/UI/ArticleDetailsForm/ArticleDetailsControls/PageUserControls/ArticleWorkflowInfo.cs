using System.Windows.Forms;

namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
	public partial class ArticleWorkflowInfo : Form
	{
		public ArticleWorkflowInfo()
		{
			InitializeComponent();
		}

		public ArticleWorkflowInfo(ArticleDetail parent)
		{
			InitializeComponent();
			if (parent?.ArticleDetails?.ArticleWorkflowState == null) return;
			uxWorkflowState.Text = parent.ArticleDetails.ArticleWorkflowState.DisplayName;
			uxPublishedOn.Text =
				parent.ArticleDetails.WebPublicationDate.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US"));
		}
	}
}
