using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SitecoreTreeWalker.UI
{
	public partial class SaveDialog : Form
	{
		public SaveChoice SelectedChoice;

		public SaveDialog()
		{
			InitializeComponent();
		}

		public enum SaveChoice
		{
			SaveToSitecoreAndUnlock,
			SaveLocal,
			DontSave,
			Cancel,
		}

		private void uxSaveToSitecore_Click(object sender, EventArgs e)
		{
			SelectedChoice = SaveChoice.SaveToSitecoreAndUnlock;
			Close();
		}

		private void uxSaveLocal_Click(object sender, EventArgs e)
		{
			SelectedChoice = SaveChoice.SaveLocal;
			Close();
		}

		private void uxDontSave_Click(object sender, EventArgs e)
		{
			SelectedChoice = SaveChoice.DontSave;
			Close();
		}

		private void uxCancel_Click(object sender, EventArgs e)
		{
			SelectedChoice = SaveChoice.Cancel;
			Close();
		}
	}
}
