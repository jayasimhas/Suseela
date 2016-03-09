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
			uxWorkflowState.Text = parent?.ArticleDetails?.ArticleWorkflowState != null ?
				parent.ArticleDetails.ArticleWorkflowState.DisplayName : "N/A";
			if (parent != null && parent._Live)
			{
				label3.Visible = true;
				uxPublishedOn.Text =
					parent.ArticleDetails.WebPublicationDate.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US"));
			}
			else
			{
				label3.Visible = false;
				uxPublishedOn.Visible = false;
			}
		}
	}
}
