using System;
using System.Windows.Forms;

namespace SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.Interfaces
{
	public class ArticleDetailsPageUserControl : UserControl
	{
		public MenuSelectorItem MenuItem;

		public virtual void LinkToMenuItem(MenuSelectorItem menuItem)
		{
			MenuItem = menuItem;
		}
		public virtual MenuSelectorItem GetMenuItem()
		{
			return MenuItem;
		}
		public virtual bool DoesNotChangeOnSaveMetadata()
		{
			return false;
		}
	}
}
