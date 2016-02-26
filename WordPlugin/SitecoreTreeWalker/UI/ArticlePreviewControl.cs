using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PluginModels;
using ArticleStruct = PluginModels.ArticleStruct;

namespace SitecoreTreeWalker.UI
{
	sealed class ArticlePreviewControl : TableLayoutPanel
	{
		public Label Title;
		public Label Authors;
		public Label ArticleNumber;
		public Label Publication;

		public Label By;
		public Label ArticlePound;
		public Label PubLabel;

		public ArticlePreviewControl()
		{
			Title = new Label
			        	{
			        		Font = new Font(Font, FontStyle.Bold),
							AutoSize = false,
							AutoEllipsis = true
			        	};
			Authors = new Label
			          	{
			          		AutoSize = false,
							AutoEllipsis = true
			          	};
			ArticleNumber = new Label
			{
				AutoSize = true
			};
			Publication = new Label
			{
				AutoSize = true
			};

			By = new Label
							{
								Text = @"By: ",
								Font = new Font(Font, FontStyle.Underline)
							};

			ArticlePound = new Label
			{
				Text = @"Article #: ",
				Font = new Font(Font, FontStyle.Underline)
			};
			PubLabel = new Label
			{
				Text = @"Publication:  ",
				Font = new Font(Font, FontStyle.Underline)
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="articleStruct"></param>
		/// <returns>True if article is found</returns>
		public bool UpdatePreview(ArticlePreviewInfo articleStruct)
		{
			Controls.Clear();
			if(string.IsNullOrEmpty(articleStruct.ArticleNumber))
			{
				Title.Text = @"No article with that article number exists";
				SetColumnSpan(Title, 2);
				Controls.Add(Title);
				Title.Width = Title.Parent.Width;
				return false;
			}
			Title.Text = articleStruct.Title;
			Authors.Text = articleStruct.Authors.Count() > 0
				            ? articleStruct.Authors.Aggregate((complete, cur) => complete + ", " + cur)
				            : @"N/A";
			ArticleNumber.Text = articleStruct.ArticleNumber;
			Publication.Text = articleStruct.Publication;
			SetColumnSpan(Title, 2);
			Controls.Add(Title);
				
			Controls.Add(By);
			Controls.Add(Authors);
			Controls.Add(ArticlePound);
			Controls.Add(ArticleNumber);
			Controls.Add(PubLabel);
			Controls.Add(Publication);

			Authors.Width = Authors.Parent.Width;
			Authors.Height = Authors.Parent.Height;
			Title.Width = Authors.Parent.Width;
			Title.Height = Authors.Parent.Height;
			return true;
		}

		public void Clear()
		{
			Controls.Clear();
		}
	}
}
