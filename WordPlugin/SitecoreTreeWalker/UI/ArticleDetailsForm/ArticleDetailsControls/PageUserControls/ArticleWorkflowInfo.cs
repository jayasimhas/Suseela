using System.Windows.Forms;

namespace SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
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
            uxWorkflowState.Text = parent.ArticleDetails.WorkflowState.DisplayName;
            uxPublishedOn.Text = parent.ArticleDetails.WebPublicationDate.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US"));
        }
	}
}
