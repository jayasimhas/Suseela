using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace SitecoreTreeWalker.UI.TreeBrowser.TreeBrowserControls
{
	public partial class SuggestedURL : Form
	{
		private string _suggestedUrl;
		private Action<string> _callback;
		
		public SuggestedURL()
		{
			InitializeComponent();
			
		}
		public SuggestedURL(string suggestedUrl, Action<string> callback)
		{
			InitializeComponent();
			_suggestedUrl = suggestedUrl;
			_callback = callback;
			
			uxSuggestedUrlTB.Text = _suggestedUrl;
	
		}

		private void uxTestUrlBtn_Click(object sender, EventArgs e)
		{
			Process.Start(uxSuggestedUrlTB.Text);
		}

		private void uxInsertUrlBtn_Click(object sender, EventArgs e)
		{
			_callback(uxSuggestedUrlTB.Text);
			this.Close();
		}

		

		public static void Open(string originalUrl, Action<string> callback)
		{
			string suggestedUrl = GetSuggestedUrl(originalUrl);

			var suggestedFrom = new SuggestedURL(suggestedUrl,callback);
			suggestedFrom.ShowDialog(Globals.SitecoreAddin.Application.ActiveDocument as IWin32Window);
		}


		public static string GetSuggestedUrl(string originalURL)
		{
			string suggestedUrl;
			if (originalURL.StartsWith("http"))
			{
				suggestedUrl = originalURL.Replace("http", "https");
			}
			else
			{
				suggestedUrl = originalURL.Insert(0, "https://");
			}
			return suggestedUrl;
		}
		private void uxCloseSuggested_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
