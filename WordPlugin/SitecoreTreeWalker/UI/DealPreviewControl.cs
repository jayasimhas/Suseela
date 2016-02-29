using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PluginModels;

namespace SitecoreTreeWalker.UI 
{
	class DealPreviewControl : TableLayoutPanel
	{
		public Label Title;
		public Label ID;
		public Label DealDate;
		public Label LastUpdated;

		private readonly Label IDLabel;
		private readonly Label DealDateLabel;
		private readonly Label LastUpdatedLabel;
		public DealPreviewControl()
		{
			Title = new Label
			{
				Font = new Font(Font, FontStyle.Bold),
				AutoSize = false,
				AutoEllipsis = true
			};

			ID = new Label
			{
				AutoSize = true
			};
			DealDate = new Label
			{
				AutoSize = true
			};
			LastUpdated = new Label
			{
				AutoSize = true
			};

			IDLabel = new Label()
			{
				Text = @"Deal ID:",
				Font = new Font(Font, FontStyle.Underline)
			};
			DealDateLabel = new Label()
			{
				Text = @"Deal Date:",
				Font = new Font(Font, FontStyle.Underline)
			};
			LastUpdatedLabel = new Label()
			{
				Text = @"Last Updated:",
				Font = new Font(Font, FontStyle.Underline)
			};
		}

		public bool UpdatePreview(DealInfo info)
		{
			Controls.Clear();			
			//TODO - Work on this to check if info is null
			//if(info == null || string.IsNullOrEmpty(info.ID))
			{
				Title.Text = @"No Deal with that ID exists";
				SetColumnSpan(Title, 2);
				Controls.Add(Title);
				Title.Width = Title.Parent.Width;
				return false;
			}
			Title.Text = info.Name;
			ID.Text = info.ID;
			DealDate.Text = String.Format("{0:M-d-yyyy}", info.DealDate);
			LastUpdated.Text = String.Format("{0:M-d-yyyy}", info.LastUpdated);

			SetColumnSpan(Title, 2);
			Controls.Add(Title);
			Title.Width = Title.Parent.Width;
			Title.Height = Title.Parent.Height;
			Controls.Add(IDLabel);
			Controls.Add(ID);
			Controls.Add(DealDateLabel);
			Controls.Add(DealDate);
			Controls.Add(LastUpdatedLabel);
			Controls.Add(LastUpdated);
			return true;
		}
	}
}
