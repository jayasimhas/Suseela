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
			uxWorkflowState.Text = parent?.ArticleDetails?.ArticleWorkflowState != null ? parent.ArticleDetails.ArticleWorkflowState.DisplayName : "N/A";
			uxPublishedOn.Text =
					parent.ArticleDetails.WebPublicationDate.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US"));
		}
	}
}
