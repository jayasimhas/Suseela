using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using SitecoreTreeWalker.SitecoreTree;
using System.Drawing;

namespace SitecoreTreeWalker.UI.TabController
{

    class IndustryTab : TaxonomyTab
    {
        public TextBox Keywords;
        public LinkLabel ViewTreeLink;
        public LinkLabel ViewSearchLink;
        public ListView Results;
        public TreeView ResultsTree;
        public ListView Selected;
        public HDirectoryStruct Hierarchy;
        public Guid TaxonomyGuid;
        public bool TreeFlag;

        public static int GrayPlusImageIndex = 1;
        public static int PlusImageIndex = 0;

        public List<TaxonomyStruct> CompleteList;
        public List<TaxonomyStruct> ResultsList;
        public List<TaxonomyStruct> SelectedList = new List<TaxonomyStruct>();
        //use SelectedList to get a list of taxonomies the user has selected

        private readonly SCTree _scTree;
        public ImageList SelectedImageList = new ImageList();
        public ImageList ResultImageList = new ImageList();


        public IndustryTab(TextBox keywords, LinkLabel viewTreeLink, LinkLabel viewSearchLink, ListView results,
            TreeView resultsTree, ListView selected, List<TaxonomyStruct> completeList, 
            HDirectoryStruct hierarchy)
        {
            ViewSearchLink = viewSearchLink;
            CompleteList = completeList;
            Keywords = keywords;
            ViewTreeLink = viewTreeLink;
            Results = results;
            ResultsTree = resultsTree;
            Selected = selected;
            Hierarchy = hierarchy;

            //TODO: make resource manager pull icons
            try
            {
                SelectedImageList.Images.Add(Properties.Resources.delete_icon);
                ResultImageList.Images.Add(Properties.Resources.plus_icon);
                ResultImageList.Images.Add(Properties.Resources.grayplus_icon);
            }
            catch (Exception)
            {
                //TODO: log cannot find files
            }

            InitializeUIComponents();
            AddEventHandlers();
        }

        private void AddEventHandlers()
        {
            ViewTreeLink.LinkClicked += delegate { ViewTree(); };
            ViewSearchLink.LinkClicked += delegate { ViewSearchResults(); };
            Results.MouseDown +=
                delegate(object sender, MouseEventArgs e)
                {
                    ListViewItem item = Results.HitTest(e.X, e.Y).Item;
                    if (item != null)
                    {
                        if (Results.HitTest(e.X, e.Y).Location == ListViewHitTestLocations.Image)
                        {
                            string name = item.Text;
                            var id = new Guid(item.SubItems[2].Text);
                            var taxonomy = new TaxonomyStruct { Name = name, ID = id };
                            AddToSelected(taxonomy);
                        }
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
                delegate(object sender, ListViewItemSelectionChangedEventArgs e)
                    {
                        foreach (ListViewItem i in Results.Items)
                        {
                            if (i != e.Item)
                            {
                                i.BackColor = Color.Empty;
                                i.Selected = false;
                            }
                        }

                        //e.Item.Selected = true;
                        //e.Item.BackColor = Color.Yellow;
                    };
            Results.MouseMove += 
                delegate(object sender, MouseEventArgs e)
                    {
                        ListViewItem item = Results.HitTest(e.X, e.Y).Item;
                        if (item != null)
                        {
                            if (Results.HitTest(e.X, e.Y).SubItem == item.SubItems[1]
                                || Results.HitTest(e.X, e.Y).Location == ListViewHitTestLocations.Image)
                            {
                                Cursor.Current = Cursors.Hand;
                            }
                        }
                    };

            Selected.MouseDown +=
                delegate(object sender, MouseEventArgs e)
                {
                    ListViewItem item = Selected.HitTest(e.X, e.Y).Item;
                    if (Selected.HitTest(e.X, e.Y).Location == ListViewHitTestLocations.Image)
                    {
                        string name = item.Text;
                        var id = new Guid(item.SubItems[1].Text);
                        var taxonomy = new TaxonomyStruct { Name = name, ID = id };
                        RemoveFromSelected(taxonomy);
                    }

                };
            Keywords.KeyDown +=
                delegate(object sender, KeyEventArgs e)
                {
                    if(e.KeyCode == Keys.Up)
                    {
                        if (Results.SelectedItems.Count != 0)
                        {
                            int index = Results.SelectedIndices[0];
                            if (index > 0)
                            {
                                Results.Items[index].Selected = false;
                                Results.Items[index].BackColor = Color.Empty;
                                Results.Items[index-1].Selected = true;
                                Results.Items[index-1].BackColor = Color.Yellow;
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
                                Results.Items[index].Selected = false;
                                Results.Items[index].BackColor = Color.Empty;
                                Results.Items[index + 1].Selected = true;
                                Results.Items[index + 1].BackColor = Color.Yellow;
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
                            Guid guid = new Guid(Results.SelectedItems[0].SubItems[2].Text);
                            TaxonomyStruct ts = new TaxonomyStruct{ID = guid, Name = name};
                            if(!AddToSelected(ts))
                            {
                                RemoveFromSelected(ts);
                            }
                            Keywords.Text = "";
                        }
                    }
                };
            Keywords.TextChanged +=
                delegate
                    {
                        if (Keywords.TextLength > 1)
                        {
                            PopulateResults();
                            ViewSearchResults();
                            if(Results.Items.Count > 0)
                            {
                                Results.Items[0].BackColor = Color.Yellow;
                                Results.Items[0].Selected = true;
                            }
                        }
                    };
            ResultsTree.MouseDown +=
                delegate(object sender, MouseEventArgs e)
                {
                    if (ResultsTree.HitTest(e.X, e.Y).Location == TreeViewHitTestLocations.Image)
                    {
                        TreeNode node = ResultsTree.HitTest(e.X, e.Y).Node;
                        string name = node.Text;
                        Guid id;
                        NodeGuidDictionary.TryGetValue(node, out id);
                        var ts = new TaxonomyStruct { Name = name, ID = id };
                        AddToSelected(ts);
                    }
                };
        }

        /// <summary>
        /// Sets the selected taxonomies and populates the field of selected taxonomies.
        /// </summary>
        /// <param name="selected">The selected taxonomies</param>
        public void SetSelected(List<TaxonomyStruct> selected)
        {
            SelectedList = selected;
            PopulateSelected();
        }

        private void InitializeUIComponents()
        {
            Results.View = View.Details;
            Results.Columns.Add("Taxonomy", 165, HorizontalAlignment.Left);
            Results.Columns.Add("View in tree", 70, HorizontalAlignment.Right);
            Results.HeaderStyle = ColumnHeaderStyle.None;
            
            Selected.View = View.Details;
            Selected.Columns.Add("Taxonomy", Results.Width);
            Selected.HeaderStyle = ColumnHeaderStyle.None;

            Results.SmallImageList = ResultImageList;
            Selected.SmallImageList = SelectedImageList;
            ResultsTree.ImageList = ResultImageList;

            PopulateResultsTree();

            TreeFlag = true;
            Results.Visible = false;
            ResultsTree.Visible = true;
        }

        public void PopulateResults()
        {
            Results.Items.Clear();
            ResultsList = GetResults(Keywords.Text);

            if(ResultsList.Count == 0)
            {
                var item = new ListViewItem("No matches for \'" + Keywords.Text + "\'");
                item.Font = new Font(item.Font, FontStyle.Italic);
                item.BackColor = Color.Gray;
                item.SubItems.Add("");
                item.SubItems.Add("");
                Results.Items.Add(item);
                return;
            }

            foreach(TaxonomyStruct result in ResultsList)
            {
                var item = new ListViewItem(result.Name, 0) {UseItemStyleForSubItems = false};
                item.SubItems.Add("view in tree");
                item.SubItems[1].Font = new Font(item.SubItems[1].Font, FontStyle.Underline);
                item.SubItems[1].ForeColor = Color.Blue;
                item.SubItems.Add(result.ID.ToString());
                Results.Items.Add(item);
            }
            RefreshResultsImages();
        }

        public List<TaxonomyStruct> GetResults(String text)
        {
            string lcKey = text.ToLower();

            List<TaxonomyStruct> list = CompleteList.Where(t => t.Name.ToLower().Contains(lcKey)).ToList();
            list.Sort(new TaxonomyComparer(text));

            return list;
        }

        public void PopulateResultsTree()
        {
            ResultsTree.Nodes.Clear();
            TreeNode node = RecursiveLoadHierarchy(Hierarchy);
            
            foreach(TreeNode child in node.Nodes)
            {
                ResultsTree.Nodes.Add(child);
            }
            RefreshResultsTreeImages();
        }

        public void PopulateResultsTree(TreeNode node)
        {
            ResultsTree.Nodes.Clear();
            ResultsTree.Nodes.Add(node);
            RefreshResultsTreeImages();
        }

        public void RefreshResultsTreeImages()
        {
            foreach(TreeNode node in AllNodes)
            {
                if (IsSelected(node))
                {
                    //TODO: figure out why image flickers to correct image then original image
                    node.ImageIndex = GrayPlusImageIndex;
                    node.SelectedImageIndex = GrayPlusImageIndex;
                }
                else
                {
                    node.ImageIndex = PlusImageIndex;
                    node.SelectedImageIndex = PlusImageIndex;
                }
            }
        }

        public void PopulateSelected()
        {
            Selected.Items.Clear();
            foreach(TaxonomyStruct selected in SelectedList)
            {
                var item = new ListViewItem(selected.Name, 0);
                item.SubItems.Add(selected.ID.ToString());
                Selected.Items.Add(item);
            }
        }

        public void RemoveFromSelected(TaxonomyStruct taxonomy)
        {
            foreach(TaxonomyStruct ts in SelectedList)
            {
                if(ts.ID.Equals(taxonomy.ID))
                {
                    SelectedList.Remove(ts);
                    break;
                }
            }
            PopulateSelected();
            RefreshResultsImages();
            RefreshResultsTreeImages();
        }

        public bool AddToSelected(TaxonomyStruct taxonomy)
        {
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

        public void RefreshResultsImages()
        {
            foreach (ListViewItem item in Results.Items)
            {
                var guid = new Guid(item.SubItems[2].Text);
                item.ImageIndex = IsSelected(guid) ? GrayPlusImageIndex : PlusImageIndex;
            }
        }

        public void ViewSearchResults()
        {
            Results.Visible = true;
            ResultsTree.Visible = false;
            ViewTreeLink.Visible = true;
            ViewSearchLink.Visible = false;
        }

        public void ViewTree()
        {
            Results.Visible = false;
            ResultsTree.Visible = true;
            ResultsTree.Focus();
            ViewTreeLink.Visible = false;
            ViewSearchLink.Visible = true;
        }

        public bool IsSelected(TreeNode tn)
        {
            Guid id;
            NodeGuidDictionary.TryGetValue(tn, out id);
            return IsSelected(id);
        }

        public bool IsSelected(TaxonomyStruct ts)
        {
            return IsSelected(ts.ID);
        }

        public bool IsSelected(Guid id)
        {
            foreach (TaxonomyStruct tsl in SelectedList)
            {
                if (tsl.ID == id)
                {
                    return true;
                }
            }
            return false;
        }

        public void ExpandTreeTo(TreeNode node)
        {
            PopulateResultsTree(GetParentIndustry(node));
            ResultsTree.SelectedNode = node;
        }

        public TreeNode GetParentIndustry(TreeNode node)
        {
            TreeNode current = node;
            if (current != null)
            {
                while (current.Parent != null)
                {
                    current = current.Parent;
                    if(current.Parent == null)
                    {
                        return current;
                    }
                }
                return current;
            }
            return null;
        }
    }
}
