
using Sitecore.Shell.Applications.Dialogs.Publish;

namespace Informa.Library.CustomSitecore.Dialogs
{
	public class CustomPublishForm : PublishForm
	{
		public CustomPublishForm()
			: base()
		{

		}

		protected override void OnLoad(System.EventArgs e)
		{
			base.OnLoad(e);

			//Set publish with children as unchecked by default to prevent accidental publishing with subitems and to prevent users with Single Publish Roles from holding the previous value of the checkbox
			base.PublishChildren.Checked = false;
		}
	}
}
