using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PluginModels;
using SitecoreTreeWalker.Properties;
using SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls;
using SitecoreTreeWalker.Util;

namespace SitecoreTreeWalker.UI
{
	class SelectedRelatedArticles : TableLayoutPanel
	{
		public List<ArticlePreviewInfo> LocalRelatedArticles = new List<ArticlePreviewInfo>();
		public List<ArticlePreviewInfo> SitecoreRelatedArticles = new List<ArticlePreviewInfo>();
		public List<ArticlePreviewInfo> LocalReferencedArticles = new List<ArticlePreviewInfo>();
		public List<ArticlePreviewInfo> SitecoreReferencedArticles = new List<ArticlePreviewInfo>();
		public MenuSelectorItem MenuItem;
		public bool HasChanged;

		public SelectedRelatedArticles()
		{
			Padding = new Padding(0,0,SystemInformation.VerticalScrollBarWidth,0);
		}

		public Label CreateRemoveImageLabel(ArticlePreviewInfo article)
		{
			var label = new Label
			       	{
			       		Width = 20,
			       		Height = 20,
						Image = Properties.Resources.remove,
						ImageAlign = ContentAlignment.MiddleCenter
			       	};
			
			return label;
		}

		public void UpdateMenuItem()
		{
			MenuItem.SetIndicatorNumber(GetCount().ToString());
			MenuItem.SetIndicatorIcon(GetCount() > 0 
				? Properties.Resources.blankgreen 
				: Properties.Resources.blankred);
			MenuItem.Refresh();
		}

		private void ConfigureRemoveLabel(Label label, ArticlePreviewInfo article)
		{
			label.MouseMove += delegate
			{
				Cursor.Current = Cursors.Hand;
			};
			label.Click += delegate
			{
				if(!SitecoreRelatedArticles.Remove(article))
				{
					LocalRelatedArticles.Remove(article);
				}
				HasChanged = true;
				MenuItem.HasChanged = true;
				UpdateMenuItem();
				PopulateTable();
			};
			ConfigureLabel(label, article);
		}

		private void ConfigureViewLabel(Label label, ArticlePreviewInfo article)
		{
			label.MouseMove += delegate
			{
				Cursor.Current = Cursors.Hand;
			};
			ConfigureLabel(label, article);
		}

		public Label CreateFieldLabel(string text)
		{
			var field = new Label
			{
				Text = text,
				AutoSize = false,
				AutoEllipsis = true
			};
			return field;
		}

		public Label CreateDateLabel(DateTime dateTime)
		{
			if (dateTime > DateTime.MinValue)
			{
				string strdate = dateTime.Month + "-" + dateTime.Day + "-" + dateTime.Year;
				if (dateTime.CompareTo(DateTime.Today) > 0)
				{
					return new Label
					{
						Text = strdate,
						//ForeColor = Color.Red
					};
				}
				return new Label
				{
					Text = strdate,
				};
			}
			else
			{
				return new Label
				{
					Text = Resources.SelectedRelatedArticles_CreateDateLabel_Date_not_Set,
				};
			}
		}

		public Label CreateHeaderLabel(string text)
		{
			var header = new Label
				{
					Text = text,
					Font = new Font(Font, FontStyle.Bold)
				};
			return header;
		}

		public Label CreateViewLabel(ArticlePreviewInfo article)
		{
			var view = new Label
			           	{
			           		Text = @"View",
			           		Font = new Font(Font, FontStyle.Underline),
			           		ForeColor = Color.Blue
			           	};
			view.MouseClick += delegate
			                   	{
									   //Process.Start(PreviewLinkUpdater.GetPreviewURL(article.PreviewUrl).ToString());
									   Process.Start(article.PreviewUrl);
			                   	};
			return view;
		}

		public void ConfigureLabel(Label label, ArticlePreviewInfo article = null)
		{
			//label.Tag = article.ArticleNumber;
			label.Width = label.Parent.Width;
			label.Height = 15;
		}

		public void AddToRelated(ArticlePreviewInfo preview)
		{
			
			if(LocalRelatedArticles.Select(t => t.ArticleNumber).Contains(preview.ArticleNumber)
				|| LocalReferencedArticles.Select(t => t.ArticleNumber).Contains(preview.ArticleNumber)
				|| HasArticle(GetActiveSitecoreRelatedArticles(), preview))
			{
				MessageBox.Show(@"The selected article is already a Related Article or Referenced Article.", @"Informa");
				//foreach(Control control in Controls)
				//{
				//	if (control.Tag == null || control.Font.Strikeout) continue;
				//	control.BackColor = 
				//		control.Tag.ToString() == preview.ArticleNumber 
				//		? Color.Yellow 
				//		: Color.Transparent;
				//}
				return;
			}
			LocalRelatedArticles.Add(preview);
			//UpdateMenuItem();
			PopulateTable();
		}

		public int GetCount()
		{
			return GetActiveSitecoreRelatedArticles().Count() + LocalRelatedArticles.Count() + LocalReferencedArticles.Count();
		}

		/// <summary>
		/// Related items in Sitecore that will still remain there (since they were not referenced inline).
		/// Items referenced inline (locally) indicates that the respective item will lose its "Related" status
		/// and be "Referenced Inline" instead.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ArticlePreviewInfo> GetActiveSitecoreRelatedArticles()
		{
			return SitecoreRelatedArticles.Where(sra => !HasArticle(LocalReferencedArticles, sra));
		}

		public void PopulateTable()
		{
			RowStyles.Clear();
			Controls.Clear();
			if (LocalReferencedArticles.Count + SitecoreReferencedArticles.Count > 0)
			{
				Label ReferencedArticlesHeader = CreateHeaderLabel(@"Referenced Articles: ");
				SetColumnSpan(ReferencedArticlesHeader, ColumnCount);
				RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
				Controls.Add(ReferencedArticlesHeader);
				ConfigureLabel(ReferencedArticlesHeader);
				foreach (ArticlePreviewInfo article in LocalReferencedArticles)
				{
					bool bold = !SitecoreReferencedArticles.Select(a => a.ArticleNumber).Contains(article.ArticleNumber);
					AddRow(article, false, bold, false);
				}
				foreach (ArticlePreviewInfo article in SitecoreReferencedArticles.Where(a => !LocalReferencedArticles.Select(b=>b.ArticleNumber).Contains(a.ArticleNumber)))
				{
					bool italic = !LocalReferencedArticles.Select(a => a.ArticleNumber).Contains(article.ArticleNumber);
					AddRow(article, italic, false, false);
				} 
			}

			if (LocalRelatedArticles.Count + SitecoreRelatedArticles.Count > 0)
			{
				Label RelatedArticlesHeader = CreateHeaderLabel(@"Related Articles: ");
				SetColumnSpan(RelatedArticlesHeader, ColumnCount);
				RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
				Controls.Add(RelatedArticlesHeader);
			}
			foreach (ArticlePreviewInfo article in SitecoreRelatedArticles)
			{
				if(LocalReferencedArticles.Select(a => a.ArticleNumber).Contains(article.ArticleNumber))
				{ //sitecore related article already referenced inline locally, so strikeout
					AddRow(article, true, false, false);
				}
				else
				{
					AddRow(article);
				}
			} 
			foreach (ArticlePreviewInfo article in LocalRelatedArticles)
			{
				if(!HasArticle(SitecoreRelatedArticles, article))
				{
					AddRow(article, false, true);
				}
			}
			if (IsDifferentFromSitecore())
			{
				MenuItem.HasChanged = true;
				MenuItem.UpdateBackground();
				MenuItem.Refresh(); 
			}
			UpdateMenuItem();
		}

		public bool IsDifferentFromSitecore()
		{
			return (LocalRelatedArticles.Count > 0
			        || !HasSameMembers(LocalReferencedArticles, SitecoreReferencedArticles) || HasChanged);
		}

		private static bool HasSameMembers(List<ArticlePreviewInfo> articles1, List<ArticlePreviewInfo> articles2)
		{
			if(articles1.Count != articles2.Count)
			{
				return false;
			}
			List<string> numbers1 = articles1.Select(a => a.ArticleNumber).ToList();
			List<string> numbers2 = articles1.Select(a => a.ArticleNumber).ToList();
			return numbers1.All(numbers2.Contains);
		}

		private static bool HasArticle(IEnumerable<ArticlePreviewInfo> articles, ArticlePreviewInfo article)
		{
			return articles.Select(a => a.ArticleNumber).Contains(article.ArticleNumber);
		}

		public void AddRow(ArticlePreviewInfo article, bool italic = false, bool bold = false, bool removable = true)
		{
			//RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			Label image;
			Label articleNumber = CreateFieldLabel(article.ArticleNumber);
			Label title = CreateFieldLabel(article.Title);
			Label publication = CreateFieldLabel(article.Publication);
			Label date = CreateDateLabel(article.Date);
			Label view = CreateViewLabel(article);


			if (removable)
			{
				image = CreateRemoveImageLabel(article);
				Controls.Add(image);
				ConfigureRemoveLabel(image, article);
			}
			else
			{
				image = new Label();
				Controls.Add(image);
			}

			if (italic)
			{
				title.Font = new Font(image.Font, FontStyle.Strikeout|FontStyle.Italic);
			}
			
			if (bold)
			{
				title.Font = new Font(image.Font, FontStyle.Bold);
			}

			
			Controls.Add(articleNumber);
			Controls.Add(title);
			Controls.Add(publication);
			Controls.Add(date);
			Controls.Add(view);
			title.Tag = article.ArticleNumber;
			
			ConfigureLabel(articleNumber, article);
			ConfigureLabel(title, article);
			ConfigureLabel(publication, article);
			ConfigureLabel(date, article);
			ConfigureViewLabel(view, article);
		}

		public IEnumerable<Guid> GetInlineReferences()
		{
			return LocalReferencedArticles.Select(a => a.Guid);
		}

		public IEnumerable<Guid> GetRelatedArticles()
		{
			var total = new List<Guid>();
			total.AddRange(GetActiveSitecoreRelatedArticles().Select(a => a.Guid));
			total.AddRange(LocalRelatedArticles.Select(a=>a.Guid));
			return total;
		}
	}
}
