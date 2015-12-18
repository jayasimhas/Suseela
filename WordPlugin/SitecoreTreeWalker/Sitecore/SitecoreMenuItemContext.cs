using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SitecoreTreeWalker.Config;

namespace SitecoreTreeWalker.Sitecore
{
	public class SitecoreMenuItemContext : ContextMenu
	{
		public SitecoreMenuItemContext(string text, string path) {
			BuildItems();
			Text = text;
			Path = path;
		}

		private string Text { get; set; }
		private string Path { get; set; }

		private void BuildItems()
		{ 
			var insertHyperlink = new MenuItem{ Text = @"Insert Hyperlink" };
			insertHyperlink.Click += (InsertHyperlink_Click);
			MenuItems.Add(insertHyperlink);

			var insertPicture = new MenuItem { Text = @"Insert Picture" };
			insertPicture.Click += (InsertPicture);
			MenuItems.Add(insertPicture);
		}

		private void InsertHyperlink_Click(object sender, EventArgs e)
		{
			var selection = ControlContext.Current.Application.Selection;
			string text = (selection.Characters.Count == 1) ? Text : null;
			selection.Hyperlinks.Add(selection.Range, Path, null, null, text, null);
            selection.set_Style(_hyperlinkStyle);
		}

		private SitecoreImageTransfer Transfer;
		private void InsertPicture(object sender, EventArgs e)
		{
			Transfer = new SitecoreImageTransfer(Path);
			Transfer.TransferComplete += (TransferComplete);
			var thread = new Thread(() => 
			{
				try
				{
					Transfer.ProcessTransfer();
				}
				catch (WebException)
				{
					MessageBox.Show(@"Connection with Sitecore server lost. Please check your internet connection. If the problem persists, contact ed.schwehm@velir.com. Just kidding. All hope is lost.");
				}
			});
			thread.Start();
		}

		private void TransferComplete()
		{
			if(Transfer == null) return;

			var selection = ControlContext.Current.Application.Selection;
			selection.InlineShapes.AddPicture(Transfer.FileName,true,true,selection.Range);
		}

        protected object _hyperlinkStyle = ApplicationConfig.GetPropertyValue(Constants.HYPERLINK_STYLE);
	}
}
