using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls
{
	public partial class MenuSelectorItem : UserControl
	{
		public bool Selected { set; get; }


		public bool HasChanged { set; get; }

		
		public void UpdateBackground() 
		{
			if(Selected)
			{
				BackgroundImage = HasChanged 
					? Properties.Resources.orangebggradientasterisk 
					: Properties.Resources.orangebggradient;
			}
			else
			{
				BackgroundImage = HasChanged 
					? Properties.Resources.asteriskback 
					: null;
			}
		}

		public MenuSelectorItem()
		{
			InitializeComponent();
			
		}

		public void SetIndicatorIcon(Image icon)
		{
			IndicatorIcon.Image = icon;
		}

		public void SetMenuTitle(string menuTitle)
		{
			MenuTitle.Text = menuTitle;
		}

		public void SetIndicatorNumber(string number)
		{
			IndicatorIcon.Text = number;
		}

		private void MenuTitle_Click(object sender, EventArgs e)
		{
			OnClick(e);
		}

		private void IndicatorIcon_Click(object sender, EventArgs e)
		{
			OnClick(e);
		}

		private void IndicatorIcon_MouseEnter(object sender, EventArgs e)
		{
			this.Cursor = Cursors.Hand;
		}

		private void IndicatorIcon_MouseLeave(object sender, EventArgs e)
		{
			this.Cursor = Cursors.Default;
		}

		private void MenuTitle_MouseEnter(object sender, EventArgs e)
		{
			this.Cursor = Cursors.Hand;
		}

		private void MenuTitle_MouseLeave(object sender, EventArgs e)
		{
			this.Cursor = Cursors.Default;
		}
	}
}
