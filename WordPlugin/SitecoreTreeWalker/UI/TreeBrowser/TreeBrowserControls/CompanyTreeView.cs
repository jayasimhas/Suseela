using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Informa.Web.Areas.Account.Models;
using SitecoreTreeWalker.Sitecore;

namespace SitecoreTreeWalker.UI.TreeBrowser.TreeBrowserControls
{
    public partial class CompanyTreeView : UserControl
    {
        public delegate void CompanyDoubleClick(WordPluginModel.CompanyWrapper wrapper);

        public event CompanyDoubleClick CompanyDoubleClicked;

        public CompanyTreeView()
        {
            InitializeComponent();

            treeView1.HideSelection = false;
            noFlickerListView1.HideSelection = false;

            _unfiltered = new List<TreeNode>();
        }

        private IEnumerable<WordPluginModel.CompanyWrapper> _wrappers;

        private List<TreeNode> _unfiltered;

        private void LoadCompanies(IEnumerable<WordPluginModel.CompanyWrapper> wrappers)
        {
            PopulateTree(wrappers);
        }

        private void PopulateTree(IEnumerable<WordPluginModel.CompanyWrapper> wrappers)
        {
            PopulateTree(wrappers, null);
        }

        private void PopulateTree(IEnumerable<WordPluginModel.CompanyWrapper> wrappers, Dictionary<int, bool> filter)
        {
            treeView1.Nodes.Clear();

            List<TreeNode> nodesToAdd = new List<TreeNode>();
            foreach (var companyWrapper in wrappers)
            {
                if (filter != null && !filter.ContainsKey(companyWrapper.RecordID))
                {
                    continue;
                }

                TreeNode node = GetNode(companyWrapper, filter);
                nodesToAdd.Add(node);
            }

            TreeNode[] n = nodesToAdd.ToArray();

            _unfiltered.AddRange(n);

            treeView1.Nodes.AddRange(n);
        }

        private Dictionary<int, WordPluginModel.CompanyWrapper> _foundCompanies = new Dictionary<int, WordPluginModel.CompanyWrapper>();
        private Dictionary<char, List<WordPluginModel.CompanyWrapper>> _wrappersByLetter = new Dictionary<char, List<WordPluginModel.CompanyWrapper>>();

        private TreeNode GetNode(WordPluginModel.CompanyWrapper current, Dictionary<int, bool> filter)
        {
            if (!_foundCompanies.ContainsKey(current.RecordID))
            {
                _foundCompanies.Add(current.RecordID, current);

                char c = current.Title.ToLower()[0];

                List<WordPluginModel.CompanyWrapper> letterWrappers;
                if (!_wrappersByLetter.TryGetValue(c, out letterWrappers))
                {
                    letterWrappers = new List<WordPluginModel.CompanyWrapper>();
                    _wrappersByLetter.Add(c, letterWrappers);
                }

                letterWrappers.Add(current);
            }

            var node = new TreeNode(current.Title, GetChildNodes(current, filter).ToArray())
                           {
                               Tag = current.RecordID,
                               ImageIndex = 0
                           };
            node.Expand();

            return node;
        }

        private IEnumerable<TreeNode> GetChildNodes(WordPluginModel.CompanyWrapper current, Dictionary<int, bool> filter)
        {
            foreach (var child in current.RelatedCompanies)
            {
                if (filter != null && !filter.ContainsKey(child.RecordID))
                {
                    continue;
                }

                yield return GetNode(child, filter);
            }
        }

        public void Filter(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                tabControl1.SelectTab(tabTreeView);
                return;
            }

            tabControl1.SelectTab(tabResultsView);

            noFlickerListView1.Items.Clear();

            char c = text.ToLower()[0];

            List<WordPluginModel.CompanyWrapper> wrappersForLetter;
            if (_wrappersByLetter.TryGetValue(c, out wrappersForLetter))
            {
                Dictionary<int, bool> filter = Filter(text, wrappersForLetter);

                linkViewInTree.Visible = false;
                noFlickerListView1.Items.Clear();

                foreach (var b in filter)
                {
                    WordPluginModel.CompanyWrapper wrapper;
                    if (_foundCompanies.TryGetValue(b.Key, out wrapper))
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = wrapper.Title;
                        item.Tag = wrapper.RecordID;
                        noFlickerListView1.Items.Add(item);
                    }
                }
            }

        }

        private Dictionary<int, bool> Filter(string text, IEnumerable<WordPluginModel.CompanyWrapper> wrappers)
        {
            Dictionary<int, bool> allResults = new Dictionary<int, bool>();

            foreach (var companyWrapper in wrappers)
            {
                Dictionary<int, bool> currentResults = FindMatches(companyWrapper, text);

                foreach (var currentResult in currentResults)
                {
                    if (!allResults.ContainsKey(currentResult.Key))
                    {
                        allResults.Add(currentResult.Key, true);
                    }
                }
            }

            return allResults;
        }

        private Dictionary<int, bool> FindMatches(WordPluginModel.CompanyWrapper wrapper, string text)
        {
            Dictionary<int, bool> matches = new Dictionary<int, bool>();

            if (IsMatch(wrapper, text))
            {
                if (!matches.ContainsKey(wrapper.RecordID))
                {
                    matches.Add(wrapper.RecordID, true);
                }
            }

            return matches;
        }

        private bool IsMatch(WordPluginModel.CompanyWrapper wrapper, string text)
        {
            return wrapper.Title.StartsWith(text, StringComparison.InvariantCultureIgnoreCase);
        }

        public WordPluginModel.CompanyWrapper SelectedCompany
        {
            get
            {
                int? recordId = null;
                if (tabControl1.SelectedTab == tabTreeView)
                {
                    TreeNode node = treeView1.SelectedNode;

                    if (node == null)
                    {
                        return null;
                    }

                    recordId = node.Tag as int?;
                }
                else if (tabControl1.SelectedTab == tabResultsView)
                {
                    if (noFlickerListView1.SelectedItems.Count != 0)
                    {
                        ListViewItem firstSelected = noFlickerListView1.SelectedItems[0];

                        recordId = firstSelected.Tag as int?;
                    }
                }

                WordPluginModel.CompanyWrapper wrapper = null;
                if (recordId.HasValue)
                {
                    _foundCompanies.TryGetValue(recordId.Value, out wrapper);
                }

                return wrapper;
            }
        }

        public void SetImageList(ImageList images)
        {
            treeView1.ImageList = images;
            noFlickerListView1.SmallImageList = images;
        }

        public void SelectFirst()
        {
            if (tabControl1.SelectedTab == tabTreeView)
            {
                treeView1.SelectedNode = treeView1.TopNode;
                treeView1.SelectedNode.EnsureVisible();
            }
            else
            {
                noFlickerListView1.SelectedIndices.Clear();

                if (noFlickerListView1.Items.Count > 0)
                {
                    noFlickerListView1.SelectedIndices.Add(0);
                }
            }
        }

        public void MoveDown()
        {
            if (tabControl1.SelectedTab == tabTreeView)
            {
                TreeNode currentNode = treeView1.SelectedNode;

                if (currentNode == null)
                {
                    return;
                }

                TreeNode nextVisible = currentNode.NextVisibleNode;

                if (nextVisible != null)
                {
                    treeView1.SelectedNode = nextVisible;
                    treeView1.SelectedNode.EnsureVisible();
                }
            }
            else if (tabControl1.SelectedTab == tabResultsView)
            {
                if (noFlickerListView1.SelectedItems.Count != 0)
                {
                    int index = noFlickerListView1.SelectedIndices[0];
                    index = Math.Min(index + 1, noFlickerListView1.Items.Count - 1);

                    noFlickerListView1.SelectedIndices.Clear();
                    noFlickerListView1.SelectedIndices.Add(index);
                }
                else if (noFlickerListView1.Items.Count > 0)
                {
                    SelectFirst();
                }
            }
        }

        public void MoveUp()
        {
            if (tabControl1.SelectedTab == tabTreeView)
            {
                TreeNode currentNode = treeView1.SelectedNode;

                if (currentNode == null)
                {
                    return;
                }

                TreeNode prevVisible = currentNode.PrevVisibleNode;

                if (prevVisible != null)
                {
                    treeView1.SelectedNode = prevVisible;
                    treeView1.SelectedNode.EnsureVisible();
                }
            }
            else if (tabControl1.SelectedTab == tabResultsView)
            {
                if (noFlickerListView1.SelectedItems.Count != 0)
                {
                    int index = noFlickerListView1.SelectedIndices[0];
                    index = Math.Max(index - 1, 0);

                    noFlickerListView1.SelectedIndices.Clear();
                    noFlickerListView1.SelectedIndices.Add(index);
                }
                else if (noFlickerListView1.Items.Count > 0)
                {
                    SelectFirst();
                }
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            WordPluginModel.CompanyWrapper selectedCompany = SelectedCompany;

            if (selectedCompany != null && CompanyDoubleClicked != null)
            {
                CompanyDoubleClicked(selectedCompany);
            }
        }

        private void CompanyTreeView_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                _wrappers = SitecoreGetter.GetAllCompaniesWithRelated();

                LoadCompanies(_wrappers);
            }
        }

        private void noFlickerListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (noFlickerListView1.SelectedItems.Count != 0)
            {
                linkViewInTree.Visible = true;
                foreach (ListViewItem selectedItem in noFlickerListView1.SelectedItems)
                {
                    selectedItem.EnsureVisible();
                }
            }
            else
            {
                linkViewInTree.Visible = false;
            }
        }

        private void linkViewInTree_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (noFlickerListView1.SelectedItems.Count != 0)
            {
                ListViewItem selectedItem = noFlickerListView1.SelectedItems[0];

                int i = (int) selectedItem.Tag;

                SelectNodeInTree(i);
                tabControl1.SelectedTab = tabTreeView;
            }
        }

        private TreeNode FindCompany(int recordId)
        {
            foreach (TreeNode treeNode in treeView1.Nodes)
            {
                TreeNode wrapper = FindCompany(recordId, treeNode);

                if (wrapper != null)
                {
                    return wrapper;
                }

            }

            return null;
        }

        private TreeNode FindCompany(int recordId, TreeNode wrapper)
        {
            int id = (int) wrapper.Tag;
            if (id == recordId)
            {
                return wrapper;
            }

            foreach (TreeNode companyWrapper in wrapper.Nodes)
            {
                TreeNode w = FindCompany(recordId, companyWrapper);

                if (w != null)
                {
                    return w;
                }
            }

            return null;
        }

        private void SelectNodeInTree(int id)
        {
            TreeNode node = FindCompany(id);

            if (node != null)
            {
                treeView1.SelectedNode = node;
                node.EnsureVisible();
            }
            
            tabControl1.SelectedTab = tabTreeView;
        }

        private void noFlickerListView1_DoubleClick(object sender, EventArgs e)
        {
            WordPluginModel.CompanyWrapper selectedCompany = SelectedCompany;

            if (selectedCompany != null && CompanyDoubleClicked != null)
            {
                CompanyDoubleClicked(selectedCompany);
            }
        }
    }
}
