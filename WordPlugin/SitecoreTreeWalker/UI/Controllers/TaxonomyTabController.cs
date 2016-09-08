using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls;
using PluginModels;
using ArticleStruct = PluginModels.ArticleStruct;

namespace InformaSitecoreWord.UI.Controllers
{
    public class TaxonomyTabController : ITabController
    {
        protected List<TreeNode> AllNodes = new List<TreeNode>();
        protected Dictionary<Guid, TreeNode> GuidNodeDictionary = new Dictionary<Guid, TreeNode>();
        protected Dictionary<TreeNode, Guid> NodeGuidDictionary = new Dictionary<TreeNode, Guid>();

        public TextBox Keywords;
        public LinkLabel ViewTreeLink;
        public LinkLabel ViewSearchLink;
        /// <summary>
        /// Control displaying the results of the search
        /// </summary>
        public ListView Results;
        public TreeView ResultsTree;
        public ListView Selected;
		public HDirectoryStruct Hierarchy;
        public Guid TaxonomyGuid;
        public MenuSelectorItem MenuItem;

        public Control Up;
        public Control Down;

        public bool TreeFlag;
        public bool HasChanged;


        public static int GrayPlusImageIndex = 1;
        public static int PlusImageIndex; //=0

		public List<TaxonomyStruct> CompleteList;
		public List<TaxonomyStruct> ResultsList;
		public List<TaxonomyStruct> SelectedList = new List<TaxonomyStruct>();
        //use SelectedList to get a list of taxonomies the user has selected

        public ImageList SelectedImageList = new ImageList();
        public ImageList ResultImageList = new ImageList();

        /// <summary>
        /// Class that handles the complexities of having a hierarchical view of taxonomies.
        /// </summary>
        /// <param name="keywords">Keywords TextBox whose contents is the basis for the search</param>
        /// <param name="viewTreeLink">"View Tree" LinkLabel</param>
        /// <param name="viewSearchLink">"View Search Results" Link Label</param>
        /// <param name="results">ListView containing the results of a search</param>
        /// <param name="resultsTree">TreeView containing the heirarchy of interested taxonomies</param>
        /// <param name="selected">ListView containing selected taxonomies</param>
        /// <param name="completeList">Complete list of interested taxonomies. Should be populated by initial crawl of all relevant taxonomies</param>
        /// <param name="hierarchy">Tree of all interested taxonomies. Should be populated by initial crawl of all relevant taxonomies</param>
        /// <param name="up"></param>
        /// <param name="down"></param>
        public TaxonomyTabController(TextBox keywords, LinkLabel viewTreeLink, LinkLabel viewSearchLink, ListView results,
            TreeView resultsTree, ListView selected, Control up, Control down)
        {
            ViewSearchLink = viewSearchLink;
            Keywords = keywords;
            ViewTreeLink = viewTreeLink;
            Results = results;
            ResultsTree = resultsTree;
            Selected = selected;

            Up = up;
            Down = down;

            try
            {
                SelectedImageList.Images.Add(Properties.Resources.remove);
                ResultImageList.Images.Add(Properties.Resources.add);
                ResultImageList.Images.Add(Properties.Resources.selected);
            }
            catch (ArgumentNullException anx)
            {
                Globals.SitecoreAddin.LogException("TaxonomyTabController.ctor: Added image is null!", anx);
            }
            catch (ArgumentException ax)
            {
                Globals.SitecoreAddin.LogException("TaxonomyTabController.ctor: Added image is not a bitmap!", ax);
            }

            InitializeUIComponents();
            AddEventHandlers();
        }

		public void InitializeSitecoreValues(List<TaxonomyStruct> completeList, HDirectoryStruct hierarchy)
        {
            CompleteList = completeList;
            Hierarchy = hierarchy;
            PopulateResultsTree();
            PopulateSelected();
        }

        public void ResetFields()
        {
            PopulateResultsTree();
            ViewTree();
            Selected.Items.Clear();
            SelectedList.Clear();
            RefreshResultsTreeImages();
            MenuItem.SetIndicatorNumber("");
            MenuItem.SetIndicatorIcon(Properties.Resources.redx);
        }

        public void LinkToMenuItem(MenuSelectorItem menuItem)
        {
            MenuItem = menuItem;
        }

		public TaxonomyStruct[] GetSelected()
        {
			return SelectedList.Select(t => new TaxonomyStruct { ID = t.ID, Name = t.Name, Section = t.Section }).ToArray();
        }

        private void AddEventHandlers()
        {
            ViewTreeLink.LinkClicked +=
                delegate
                    {
                        PopulateResultsTree();
                        ViewTree();
                    };
            ViewSearchLink.LinkClicked += delegate { ViewSearchResults(); };
            Results.MouseDown +=
                delegate (object sender, MouseEventArgs e)
                {
                    ListViewItem item = Results.HitTest(e.X, e.Y).Item;
                    if (item != null)
                    {
                        if (Results.HitTest(e.X, e.Y).SubItem == item.SubItems[1])
                        //if mouse click on "view in tree"
                        {
                            TreeNode node;
                            GuidNodeDictionary.TryGetValue(new Guid(item.SubItems[2].Text), out node);
                            ViewTree();
                            ExpandTreeTo(node);
                        }
                    }
                };
            Results.ItemSelectionChanged +=
                delegate (object sender, ListViewItemSelectionChangedEventArgs e)
                    {
                        foreach (ListViewItem i in Results.Items)
                        {
                            if (i != e.Item)
                            {
                                i.BackColor = Color.Empty;
                                i.Selected = false;
                            }
                        }
                        e.Item.BackColor = Color.Yellow;
                    };
            Results.DoubleClick +=
                delegate
                    {
                        AddSelectedResultsItemToSelected();
                    };
            Results.KeyDown +=
                delegate (object sender, KeyEventArgs e)
                    {
                        if (e.KeyCode == Keys.Enter)
                        {
                            AddSelectedResultsItemToSelected();
                        }
                    };
            Results.MouseMove +=
                delegate (object sender, MouseEventArgs e)
                    {
                        ListViewItem item = Results.HitTest(e.X, e.Y).Item;
                        Cursor.Current = item != null ? Cursors.Hand : Cursors.Default;
                    };
            Selected.DoubleClick +=
                delegate
                {
                    RemoveSelectedItemFromSelected();
                };
            Selected.KeyDown +=
                delegate (object sender, KeyEventArgs e)
                {
                    //var clickPos = Selected.PointToClient(Control.MousePosition);
                    if (e.KeyCode == Keys.Enter && Selected.SelectedItems.Count > 0)
                    {
                        RemoveSelectedItemFromSelected();
                    }
                };
            Selected.MouseMove +=
                delegate (object sender, MouseEventArgs e)
                    {
                        ListViewItem item = Selected.HitTest(e.X, e.Y).Item;
                        Cursor.Current = item != null ? Cursors.Hand : Cursors.Default;
                    };
            Keywords.KeyDown +=
                delegate (object sender, KeyEventArgs e)
                {
                    if (e.KeyCode == Keys.Up)
                    {
                        if (Results.SelectedItems.Count != 0)
                        {
                            int index = Results.SelectedIndices[0];
                            if (index > 0)
                            {
                                Results.BeginUpdate();
                                Results.Items[index - 1].Selected = true;
                            }
                        }
                    }
                    if (e.KeyCode == Keys.Down)
                    {
                        if (Results.SelectedItems.Count != 0)
                        {
                            int index = Results.SelectedIndices[0];
                            if (index < Results.Items.Count - 1)
                            {
                                Results.BeginUpdate();
                                Results.Items[index + 1].Selected = true;
                            }
                        }
                    }
                    if (e.KeyCode == Keys.Enter)
                    {
                        if (Results.SelectedItems.Count == 0)
                        {
                            PopulateResults();
                            ViewSearchResults();
                        }
                        else
                        {
                            string name = Results.SelectedItems[0].Text;
                            var guid = new Guid(Results.SelectedItems[0].SubItems[2].Text);
							var ts = new TaxonomyStruct { ID = guid, Name = name };
                            if (!AddToSelected(ts))
                            {
                                RemoveFromSelected(ts);
                            }
                            Keywords.Text = "";
                        }
                        e.SuppressKeyPress = true;
                    }
                    Results.EndUpdate();
                };
            Keywords.TextChanged +=
                delegate
                    {
                        if (Keywords.TextLength > 1)
                        {
                            PopulateResults();
                            ViewSearchResults();
                            if (Results.Items.Count > 0)
                            {
                                Results.Items[0].BackColor = Color.Yellow;
                                Results.Items[0].Selected = true;
                            }
                        }
                    };
            ResultsTree.NodeMouseDoubleClick +=
                delegate
                    {
                        AddSelectedResultsTreeNodeToSelected();
                    };
            ResultsTree.KeyDown +=
                delegate (object sender, KeyEventArgs e)
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        AddSelectedResultsTreeNodeToSelected();
                        e.SuppressKeyPress = true;
                    }
                };
            ResultsTree.MouseMove +=
                delegate (object sender, MouseEventArgs e)
                    {
                        if (ResultsTree.HitTest(e.X, e.Y).Location == TreeViewHitTestLocations.Label)
                        {
                            Cursor.Current = Cursors.Hand;
                        }
                    };

            Down.Click +=
                delegate
                    {
                        var currentIndex = Selected.SelectedItems[0].Index;
                        var item = Selected.Items[currentIndex];
                        if (currentIndex < Selected.Items.Count - 1)
                        {
                            Selected.Items.RemoveAt(currentIndex);
                            Selected.Items.Insert(currentIndex + 1, item);
							TaxonomyStruct currentItem = SelectedList.FirstOrDefault(i => i.ID.ToString() == Selected.SelectedItems[0].SubItems[1].Text);
							SelectedList.Remove(currentItem);
							SelectedList.Insert(currentIndex + 1, currentItem);
                        }
                    };

            Up.Click +=
                delegate
                    {
                        var currentIndex = Selected.SelectedItems[0].Index;
                        var item = Selected.Items[currentIndex];
                        if (currentIndex > 0)
                        {
                            Selected.Items.RemoveAt(currentIndex);
                            Selected.Items.Insert(currentIndex - 1, item);
							TaxonomyStruct currentItem = SelectedList.FirstOrDefault(i => i.ID.ToString() == Selected.SelectedItems[0].SubItems[1].Text);
							SelectedList.Remove(currentItem);
							SelectedList.Insert(currentIndex - 1, currentItem);
                        }
                    };

            Up.MouseMove +=
                delegate
                {
                    Cursor.Current = Cursors.Hand;
                };
            Down.MouseMove +=
                delegate
                {
                    Cursor.Current = Cursors.Hand;
                };
        }

        public void AddSelectedResultsTreeNodeToSelected()
        {
            TreeNode node = ResultsTree.SelectedNode;
            if (node == null) return;
            if (node.Parent == null) return;//TMehyar - 2016-02-23: This is to prevent the users from selecting folders
            string name = node.Text;
            Guid id;
            NodeGuidDictionary.TryGetValue(node, out id);
			var ts = new TaxonomyStruct { Name = name, ID = id };
            if (ResultsTree.SelectedNode.Parent != null)
            {
                ts.Section = ResultsTree.SelectedNode.Parent.FullPath;
            }
            AddToSelected(ts);
        }

        public void AddSelectedResultsItemToSelected()
        {
            if (Results.SelectedItems.Count > 0)
            {
                ListViewItem item = Results.SelectedItems[0];
                var id = new Guid(item.SubItems[2].Text);
				var taxonomy = new TaxonomyStruct { Name = item.Text, ID = id };
                AddToSelected(taxonomy);
            }

        }

        public void RemoveSelectedItemFromSelected()
        {
            if (Selected.SelectedItems.Count > 0)
            {
                ListViewItem item = Selected.SelectedItems[0];
                string name = item.Text;
                var id = new Guid(item.SubItems[1].Text);
				var taxonomy = new TaxonomyStruct { Name = name, ID = id };
                RemoveFromSelected(taxonomy);
            }

        }

        /// <summary>
        /// Sets the selected taxonomies and populates the field of selected taxonomies.
        /// </summary>
        /// <param name="selected">The selected taxonomies</param>
		public void SetSelected(List<TaxonomyStruct> selected)
        {
            SelectedList = selected;
            PopulateSelected();
            RefreshResultsTreeImages();
            RefreshResultsImages();
        }
        private void InitializeUIComponents()
        {
            Results.View = View.Details;
            Results.Columns.Add("Taxonomy", 400, HorizontalAlignment.Left);
            Results.Columns.Add("View in tree", 70, HorizontalAlignment.Right);
            Results.HeaderStyle = ColumnHeaderStyle.None;

            Selected.View = View.Details;
            Selected.Columns.Add("Taxonomy", Results.Width);
            Selected.HeaderStyle = ColumnHeaderStyle.None;

            Results.SmallImageList = ResultImageList;
            Selected.SmallImageList = SelectedImageList;
            ResultsTree.ImageList = ResultImageList;

            ViewTree();
        }

        /// <summary>
        /// Populates Results ListView based on a search using the contents of the Keywords TextBox
        /// </summary>
        public void PopulateResults()
        {
            Results.Items.Clear();
            ResultsList = GetResults(Keywords.Text);

            if (ResultsList.Count == 0)
            {
                var item = new ListViewItem("No matches for \'" + Keywords.Text + "\'");
                item.Font = new Font(item.Font, FontStyle.Italic);
                item.BackColor = Color.Gray;
                item.SubItems.Add("");
                item.SubItems.Add("");
                Results.Items.Add(item);
                return;
            }

			foreach (TaxonomyStruct result in ResultsList)
            {
                var item = new ListViewItem(result.Name, 0) { UseItemStyleForSubItems = false };
                item.SubItems.Add("view in tree");
                item.SubItems[1].Font = new Font(item.SubItems[1].Font, FontStyle.Underline);
                item.SubItems[1].ForeColor = Color.Blue;
                item.SubItems.Add(result.ID.ToString());
                Results.Items.Add(item);
            }
            RefreshResultsImages();
        }

        /// <summary>
        /// Updates the Icon Label to indicate the number of selected taxonomies
        /// </summary>
        public void UpdateIcon()
        {
            if (MenuItem != null)
            {
                MenuItem.SetIndicatorIcon(SelectedList.Count == 0 ? Properties.Resources.blankred : Properties.Resources.blankgreen);
                MenuItem.Refresh();
                String iconStr = SelectedList.Count.ToString();
                MenuItem.SetIndicatorNumber(iconStr);
                MenuItem.HasChanged = (HasChanged);
                MenuItem.UpdateBackground();
            }
        }

        /// <summary>
        /// Runs a search on the CompleteList of taxonomies
        /// </summary>
        /// <param name="text">Search keyword</param>
        /// <returns>A list of taxonomies which matches the search term</returns>
		public List<TaxonomyStruct> GetResults(String text)
        {
            string lcKey = text.ToLower();

            List<TaxonomyStruct> list = CompleteList.Where(t => t.Section != null && t.Name.ToLower().Contains(lcKey)).ToList();
            list.Sort(new TaxonomyComparer(text));

            return list;
        }

        /// <summary>
        /// Populates ResultsTree TreeView of the complete taxonomy
        /// </summary>
        public void PopulateResultsTree()
        {
            ResultsTree.Nodes.Clear();
            TreeNode node = RecursiveLoadHierarchy(Hierarchy);

            foreach (TreeNode child in node.Nodes)
            {
                ResultsTree.Nodes.Add(child);
            }
            RefreshResultsTreeImages();
        }

        /// <summary>
        /// Populates ResultsTree TreeView with only on particular root
        /// </summary>
        /// <param name="node">The root which will populate the ResultsTree</param>
        public void PopulateResultsTree(TreeNode node)
        {
            ResultsTree.Nodes.Clear();
            ResultsTree.Nodes.Add(node);
            RefreshResultsTreeImages();
        }

        /// <summary>
        /// Refresh "icons" in ResultsTree TreeView to correct icon to indicate available actions (able to add or not).
        /// </summary>
        public void RefreshResultsTreeImages()
        {
            ResultsTree.BeginUpdate();
            foreach (TreeNode node in AllNodes)
            {
                if (IsSelected(node))
                {
                    node.ImageIndex = GrayPlusImageIndex;
                    node.SelectedImageIndex = GrayPlusImageIndex;
                }
                else
                {
                    node.ImageIndex = PlusImageIndex;
                    node.SelectedImageIndex = PlusImageIndex;
                }
            }
            ResultsTree.EndUpdate();
        }

        /// <summary>
        /// Populate Selected ListView with all selected taxonomies
        /// </summary>
        public void PopulateSelected()
        {
            Selected.Items.Clear();
			foreach (TaxonomyStruct selected in SelectedList)
            {
                var item = new ListViewItem();
                if (!string.IsNullOrEmpty(selected.Section) && selected.Section != "Taxonomy")
                {
                    item.Text = selected.Section.Replace("\\", "/") + "/";
                }
                item.Text = item.Text + selected.Name;
                item.ImageIndex = 0;
                item.SubItems.Add(selected.ID.ToString());
                Selected.Items.Add(item);
            }

            UpdateIcon();
        }

        /// <summary>
        /// "De-select" particular taxonomy
        /// </summary>
        /// <param name="taxonomy">Taxonomy to "de-select"</param>
		public void RemoveFromSelected(TaxonomyStruct taxonomy)
        {
            HasChanged = true;
			foreach (TaxonomyStruct ts in SelectedList)
            {
                if (ts.ID.Equals(taxonomy.ID))
                {
                    SelectedList.Remove(ts);
                    break;
                }
            }
            PopulateSelected();
            RefreshResultsImages();
            RefreshResultsTreeImages();
        }

        /// <summary>
        /// "Selects" taxonomy
        /// </summary>
        /// <param name="taxonomy">Taxonomy to "select"</param>
        /// <returns>True if addition successful. Otherwise, false (e.g., taxonomy already selected).</returns>
		public bool AddToSelected(TaxonomyStruct taxonomy)
        {
            HasChanged = true;
            bool added = false;
            if (!IsSelected(taxonomy))
            {
                SelectedList.Add(taxonomy);
                added = true;
                PopulateSelected();
            }
            RefreshResultsImages();
            RefreshResultsTreeImages();
            return added;
        }

        /// <summary>
        /// Refresh "icons" in Results ListView to correct icon to indicate available actions (able to add or not).
        /// </summary>
        public void RefreshResultsImages()
        {
            foreach (ListViewItem item in Results.Items)
            {
                var guid = new Guid(item.SubItems[2].Text);
                item.ImageIndex = IsSelected(guid) ? GrayPlusImageIndex : PlusImageIndex;
            }
        }

        /// <summary>
        /// Hides ResultsTree TreeView and shows Results ListView
        /// </summary>
        public void ViewSearchResults()
        {
            Results.Visible = true;
            ResultsTree.Visible = false;
            ViewTreeLink.Visible = true;
            ViewSearchLink.Visible = false;
        }

        /// <summary>
        /// Hides Results ListView and shows ResultsTree TreeView
        /// </summary>
        public void ViewTree()
        {
            Results.Visible = false;
            ResultsTree.Visible = true;
            ResultsTree.Focus();
            ViewTreeLink.Visible = false;
            ViewSearchLink.Visible = true;
        }

        /// <summary>
        /// Indicates if Taxonomy represented by inputted TreeNode is selected.
        /// </summary>
        /// <param name="tn"></param>
        /// <returns>True if taxonomy represented by TreeNode tn is selected; otherwise, false.</returns>
        public bool IsSelected(TreeNode tn)
        {
            Guid id;
            NodeGuidDictionary.TryGetValue(tn, out id);
            return IsSelected(id);
        }

        /// <summary>
        /// Indicates if Taxonomy represented by inputted TaxonomyStruct is selected.
        /// </summary>
        /// <param name="ts"></param>
        /// <returns>True if taxonomy ts is selected; otherwise, false</returns>
		public bool IsSelected(TaxonomyStruct ts)
        {
            return IsSelected(ts.ID);
        }

        /// <summary>
        /// Indicates if taxonomy with inputted ID is selected
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if taxonomy with inputted id is selected; otherwise, false.</returns>
        public bool IsSelected(Guid id)
        {
            return SelectedList.Any(tsl => tsl.ID == id);
        }

        /// <summary>
        /// Sets the root node of the inputted TreeNode to be the only root node in the
        /// ResultsTree TreeView. Then selects the inputted TreeNode so that the tree is
        /// expanded to inputted node.
        /// </summary>
        /// <param name="node"></param>
        public void ExpandTreeTo(TreeNode node)
        {
            TreeNode parent = GetParentIndustry(node);
            PopulateResultsTree(parent);
            parent.Collapse(false);
            ResultsTree.SelectedNode = node;
        }

        /// <summary>
        /// Gets the root taxonomy of inputted taxonomy TreeNode
        /// </summary>
        /// <param name="node">TreeNode to find root taxonomy of</param>
        /// <returns>The root taxonomy TreeNode of inputted taxonomy TreeNode</returns>
        public TreeNode GetParentIndustry(TreeNode node)
        {
            TreeNode current = node;
            if (current != null)
            {
                while (current.Parent != null)
                {
                    current = current.Parent;
                    if (current.Parent == null)
                    {
                        return current;
                    }
                }
                return current;
            }
            return null;
        }

		protected TreeNode RecursiveLoadHierarchy(HDirectoryStruct hds)
        {
            var node = new TreeNode(hds.Name);
            AllNodes.Add(node);
            if (!GuidNodeDictionary.ContainsKey(hds.ID))
            {
                GuidNodeDictionary.Add(hds.ID, node);
            }
            if (!NodeGuidDictionary.ContainsKey(node))
            {
                NodeGuidDictionary.Add(node, hds.ID);
            }
            foreach (var child in hds.Children)
            {
                node.Nodes.Add(RecursiveLoadHierarchy(child));
            }
            return node;
        }

        /// <summary>
        /// Indicates if list of selected taxonomies has changed.
        /// </summary>
        /// <returns>True if a new taxonomy has been selected or removed; otherwise, false.</returns>
        bool ITabController.HasChanged()
        {
            return HasChanged;
        }

		public void UpdateFields(List<TaxonomyStruct> selected)
        {
			List<TaxonomyStruct> temp = selected.Select(t =>
				new TaxonomyStruct
                {
                    ID = t.ID,
                    Name = t.Name,
                    Section = t.Section
                }).ToList();
            SetSelected(temp);
        }
    }
}
