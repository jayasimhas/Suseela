using System.Drawing;
using InformaSitecoreWord.Sitecore;
using InformaSitecoreWord.SitecoreTree;
using InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.Interfaces;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Windows.Forms;
using InformaSitecoreWord.UI.Controllers;
using InformaSitecoreWord.SitecoreTree;

namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
	public partial class GeneralTagsControl : ArticleDetailsPageUserControl
	{
		public List<GeneralTag> GeneralTags;
		/// <summary>
		/// List of the tags already assigned to the article item
		/// </summary>
		public List<Guid> OriginalTags;
		public Guid Publication;
		public List<TaxonomyStruct> SelectedTags = new List<TaxonomyStruct>();
		public bool TagCloudFull { get; private set; }
		protected bool _isLive;

		private ArticleDetailsPageSelector _myPageSelector;
		enum TaxonomyTypes
		{
			GeneralTag,
			Industries,
			Subjects,
			MarketSegments,
			TherapeuticCategories

		};
		private ArticleDetailsPageSelector MyPageSelector
		{
			get
			{
				if(_myPageSelector == null)
				{
					_myPageSelector = (ArticleDetailsPageSelector) this.Parent.Parent;
				}
				return _myPageSelector;
			}
		}

		public GeneralTagsControl()
		{
			InitializeComponent();
		}

		protected void SetGeneralTags(Guid publication)
		{
			Publication = publication;

			GeneralTags = SitecoreGetter.GetGeneralTags(publication);
			PopulateTagCloud();
		}

		public void PopulateTagCloud()
		{
			uxTagCloud.Controls.Clear();
			TagCloudFull = false;
			foreach(var tag in GeneralTags)
			{
				if (TagCloudFull) return;
				Label label = GetTagLabel(tag);
				uxTagCloud.Controls.Add(label);
			}
		}

		public void ResetFields()
		{
			uxSelectedTags.Controls.Clear();
			MenuItem.SetIndicatorNumber("0");
			MenuItem.SetIndicatorIcon(Properties.Resources.blankred);
			OriginalTags = new List<Guid>();
			SelectedTags = new List<TaxonomyStruct>();
		}

		protected Label GetTagLabel(GeneralTag tag)
		{
			double size;
			if (tag.Counter >= 0)
			{
				size = Math.Log(tag.Counter, 1.4); 
			}
			else
			{
				size = 8;
			}
			if(size > 20)
			{
				size = 20;
			}
			else if(size < 8)
			{
				size = 8;
			}
			var label = new Label
			            	{
			            		Text = tag.Title,
			            		Tag = tag,
			            		Font = new Font(new FontFamily("Segoe UI"), (float) size, FontStyle.Underline),
			            		AutoSize = true,
			            		ForeColor = Color.Blue,
			            		Anchor = AnchorStyles.None
			            	};
			label.MouseClick += delegate
			                    	{
										AddSelectedTag(tag);
			                    	};
			label.MouseMove += delegate
			                   	{
			                   		Cursor.Current = Cursors.Hand;
			                   	};
			return label;
		}

		public void UpdateFields(SitecoreTree.ArticleStruct articleDetails)
		{
			OriginalTags = new List<Guid>();
			SelectedTags = new List<TaxonomyStruct>();
			SetGeneralTags(articleDetails.Publication);
			uxSelectedTags.RowStyles.Clear();
			uxSelectedTags.Controls.Clear();
			if (articleDetails.GeneralTags == null) return;
			foreach(TaxonomyStruct g in articleDetails.GeneralTags)
			{
				OriginalTags.Add(g.ID);
				var target = GetGeneralTag(g.ID);
				if(target != null)
				{
					AddSelectedTag(target);
				}
			}
			UpdateMenuItem();
			_isLive = articleDetails.IsPublished;
			label1.Refresh();
		}

		public void UpdateMenuItem(bool hasChanged = false)
		{
			MenuItem.HasChanged = hasChanged;
			MenuItem.SetIndicatorNumber(SelectedTags.Count.ToString());
			MenuItem.SetIndicatorIcon(SelectedTags.Count > 0
				? Properties.Resources.blankgreen
				: Properties.Resources.blankred);
			MenuItem.UpdateBackground();
		}

		/// <summary>
		/// To add a new general tag that does not exist in Sitecore, 
		/// give the general tag an empty Guid
		/// </summary>
		/// <param name="tag"></param>
		/// <param name="color"></param>
		protected void AddSelectedTag(GeneralTag tag)
		{
			if (tag == null) return;
			if (GetSelected(tag) != null) return;

			string[] selectedTaxonomyParts = tag.Path.Split('/');
			selectedTaxonomyParts = selectedTaxonomyParts.SkipWhile(x => !x.Equals("Taxonomy")).ToArray();
			selectedTaxonomyParts = selectedTaxonomyParts.Skip(1).ToArray();
			string taxonomyRoot = selectedTaxonomyParts.First();
			switch(taxonomyRoot.ToLower())
			{
				case "general tags":
				case "create as new tag":
					//general tags case
					uxSelectedTags.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));

					uxSelectedTags.Controls.Add(CreateRemoveImage(tag));
					uxSelectedTags.Controls.Add(CreateSelectedLabel(tag));
					SelectedTags.Add(GetTaxonomy(tag));
					UpdateMenuItem(true);
					break;
				case "market segments":
					TaxonomyStruct marketItem = new TaxonomyStruct();
					marketItem.ID = tag.Guid;
					marketItem.Name = tag.Title;
					MyPageSelector.pageMarketSegmentsControl.TabController.AddToSelected(marketItem);
					break;

				case "industries":
					TaxonomyStruct industryItem = new TaxonomyStruct();
					industryItem.ID = tag.Guid;
					industryItem.Name = tag.Title;
					MyPageSelector.pageIndustriesControl.TabController.AddToSelected(industryItem);
					break;

				case "subjects":
					TaxonomyStruct subjectItem = new TaxonomyStruct();
					subjectItem.ID = tag.Guid;
					subjectItem.Name = tag.Title;
					MyPageSelector.pageSubjectsControl.TabController.AddToSelected(subjectItem);
					break;

				case "therapeutic categories":
					TaxonomyStruct tcItem = new TaxonomyStruct();
					tcItem.ID = tag.Guid;
					tcItem.Name = tag.Title;
					MyPageSelector.pageTherapeuticCategoriesControl.TabController.AddToSelected(tcItem);
					break;

				default:
					break;
			}
			
		}

		protected TaxonomyStruct GetTaxonomy(GeneralTag tag)
		{
			return new TaxonomyStruct
			       	{
			       		ID = tag.Guid,
			       		Name = tag.Title
			       	};
		}

		protected Label CreateSelectedLabel(GeneralTag tag)
		{
			Color color = GetColor(tag);
			var label = new Label
			            	{
			            		Text = tag.Title,
			            		Tag = tag,
			            		Width = 500,
			            		Font = new Font(new FontFamily("Segoe UI"), 9.5f),
			            		ForeColor = color,
			            		UseMnemonic = false
			            	};
			ConfigureSelectedLabel(label);
			return label;
		}

		protected Color GetColor(GeneralTag tag)
		{
			Color color;
			if(OriginalTags.Contains(tag.Guid))
			{//if the tag is already one of the tags assigned to the article
				color = Color.Black;
			}
			else if(tag.Guid == Guid.Empty)
			{//if it's a tag that has not yet existed
				color = Color.Blue;
			}
			else
			{
				color = Color.Green;
			}
			return color;
		}

		protected void ConfigureSelectedLabel(Control label)
		{
			label.MouseMove += delegate
			{
				Cursor.Current = Cursors.Hand;
			};

			label.MouseDoubleClick += SelectedLabelClick;
		}

		protected void ConfigureRemoveLabel(Control label)
		{
			label.MouseMove += delegate
			{
				Cursor.Current = Cursors.Hand;
			};

			label.MouseClick += SelectedLabelClick;
		}

		protected Label CreateRemoveImage(GeneralTag tag)
		{
			var label = new Label
			{
				Tag = tag,
			    Width = 20,
			    Height = 20,
			    Image = Properties.Resources.remove,
			    ImageAlign = ContentAlignment.MiddleCenter
			};

			ConfigureRemoveLabel(label);

			return label;
		}

		private void SelectedLabelClick(object sender, MouseEventArgs e)
		{
			var control = sender as Control;
			if (control == null) return;
			var target = control.Tag as GeneralTag;
			if(target == null) return;
			RemoveFromSelected(target);
		}

		public void PushSitecoreChanges()
		{
			foreach(Control control in uxSelectedTags.Controls)
			{
				control.ForeColor = Color.Black;
			}
		}

		protected void RemoveFromSelectedRecusive(GeneralTag tag)
		{
			var selected = GetSelected(tag);
			if (selected != null)
			{
				foreach(Control control in uxSelectedTags.Controls)
				{
					var target = control.Tag as GeneralTag;
					if(target != null && target == tag)
					{
						uxSelectedTags.Controls.Remove(control);
						RemoveFromSelectedRecusive(tag);
						break;
					}
				}
			}
			SelectedTags.Remove(selected);
		}

		protected void RemoveFromSelected(GeneralTag tag)
		{
			RemoveFromSelectedRecusive(tag);
			uxSelectedTags.RowStyles.RemoveAt(uxSelectedTags.RowStyles.Count - 1);
			//uxSelectedTags.RowCount--;
			UpdateMenuItem(true);
		}

		protected void PopulateSelected()
		{
			/*uxSelectedTags.Controls.Clear();
			uxSelectedTags.RowStyles.Clear();
			foreach(var tag in SelectedTags)
			{
				Guid temp = tag;
				var target = GeneralTags.FirstOrDefault(t => t.Guid == temp);
				uxSelectedTags.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
				uxSelectedTags.Controls.Add(CreateRemoveImage(target));
				uxSelectedTags.Controls.Add(CreateSelectedLabel(target));
			}*/
		}

		protected TaxonomyStruct GetSelected(GeneralTag tag)
		{
			/*if(tag.Guid == Guid.Empty)
			{ //if it's "new" tag, make sure the title is unique
				return
					(from Control control in uxSelectedTags.Controls select control.Tag as GeneralTag).Any(t => t.Title == tag.Title);
			}
			return  (from Control control in uxSelectedTags.Controls select control.Tag as GeneralTag).Any(current => current != null && current.Guid == tag.Guid);*/

			if (tag.Guid == Guid.Empty)
			{
				return SelectedTags.FirstOrDefault(t => t.Name.ToLower() == tag.Title.ToLower());
			}
			return SelectedTags.FirstOrDefault(t => t.ID == tag.Guid);
		}

		protected GeneralTag GetGeneralTag(Guid guid)
		{
			return GeneralTags.FirstOrDefault(tag => tag.Guid == guid);
		}

		private void GeneralTagsControl_Load(object sender, System.EventArgs e)
		{
			uxSuggestions.Visible = false;
			this.MouseClick += delegate
				{
					uxSuggestions.Visible = false;
				};
			AddHideSuggestionsListeners(Controls);
			uxSuggestions.MouseDoubleClick += new MouseEventHandler(uxSuggestions_SelectedIndexChanged);
		}

		protected void AddHideSuggestionsListeners(ControlCollection controls)
		{
			if(controls.Count == 0)
			{
				return;
			}
			foreach(Control control in controls)
			{
				if(control != uxKeyword && control != uxSuggestions)
				{
					control.MouseClick += delegate
																	{
																		uxSuggestions.Visible = false;
																	};
				}
				AddHideSuggestionsListeners(control.Controls);
			}
		}

		public const string CreateNewTagSuggestion = @"Create as new tag";
		public const string UNWANTED_PATH_PREFIX = "/sitecore/content/Home/Globals/Taxonomy/";
		protected List<string> GetSuggestions(string text)
		{
			//var suggestions = new List<string>();
			//get suggestions that are not already selected

			var suggestions = GeneralTags.Select(g => g.Path.Replace(UNWANTED_PATH_PREFIX, "")).Where(t => 
				t.ToLower().Contains(text.ToLower()) && !SelectedTags.Select(s => s.Name.ToLower()).Contains(t.ToLower())).ToList();
			suggestions.Sort(new GeneralTagTitleComparer(text));
			suggestions = suggestions.GetRange(0, Math.Min(5, suggestions.Count));
			if (!GeneralTagExists(text))
			{
				suggestions.Add(CreateNewTagSuggestion); 
			}
			return suggestions;
		}

		public bool GeneralTagExists(string title)
		{
			return GeneralTags.Any(tag => tag.Title.ToLower().Equals(title.ToLower()));
		}
		
		private void uxKeyword_TextChanged(object sender, EventArgs e)
		{
			var suggestions = GetSuggestions(uxKeyword.Text);
			if(uxKeyword.Text.Length > 0 && suggestions.Count > 0)
			{
				uxSuggestions.DataSource = suggestions;
				uxSuggestions.Visible = true;
			}
		}

		private void uxSuggestions_SelectedIndexChanged(object sender, EventArgs e)
		{
			GeneralTag tag;

			var title = (string)uxSuggestions.SelectedItem;
			if (title == CreateNewTagSuggestion)
			{
				tag = CreateNewTag();
			}
			else
			{
				string[] selectedValueParts = uxSuggestions.SelectedValue.ToString().Split('/');
				string selectedValue = selectedValueParts.Last();

				tag = GeneralTags.FirstOrDefault(t => t.Path.ToLower().Contains(uxSuggestions.SelectedValue.ToString().ToLower()));
			}
			
			AddSelectedTag(tag);
			uxSuggestions.Visible = false;
		}



		private GeneralTag CreateNewTag()
		{
			var title = (string)uxSuggestions.SelectedItem;
			GeneralTag tag;

				tag = new GeneralTag
				{
					Counter = 1,
					Guid = Guid.Empty,
					Title = uxKeyword.Text
				};
			return tag;
		}

		private void uxKeyword_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Down)
			{
				if (uxSuggestions.SelectedIndex < uxSuggestions.Items.Count - 1)
				{
					uxSuggestions.SelectedIndex++; 
				}
				e.SuppressKeyPress = true;
			}
			if(e.KeyCode == Keys.Up)
			{
				if (uxSuggestions.SelectedIndex > 0)
				{
					uxSuggestions.SelectedIndex--;
				}
				e.SuppressKeyPress = true;
			}
			if(e.KeyCode == Keys.Enter)
			{
				var title = (string)uxSuggestions.SelectedItem;
				GeneralTag tag;
				if (title == CreateNewTagSuggestion)
				{
					tag = CreateNewTag();
				}
				else
				{
					string[] selectedValueParts = uxSuggestions.SelectedValue.ToString().Split('/');
					string selectedValue = selectedValueParts.Last();
					tag = GeneralTags.FirstOrDefault(t => t.Title.ToLower().Equals(selectedValue.ToLower()));
				}
				AddSelectedTag(tag);
				uxKeyword.Clear();
				e.SuppressKeyPress = true;
			}
		}

		private void uxTagCloud_ControlAdded(object sender, ControlEventArgs e)
		{
			if(e.Control.Location.Y + e.Control.Height >= uxTagCloud.Height)
			{
				uxTagCloud.Controls.Remove(e.Control);
				TagCloudFull = true;
			}

		}

		private void label1_Paint(object sender, PaintEventArgs e)
		{
			if (_isLive)
			{
				e.Graphics.DrawImage(InformaSitecoreWord.Properties.Resources.live, 570, 1, 28, 28);
				e.Graphics.DrawString("Live!", new Font("SegoeUI", 18), Brushes.Green, 510, 1);
			}
		}
	}
}
