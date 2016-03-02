using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PluginModels;
using StaffStruct = PluginModels.StaffStruct;

namespace InformaSitecoreWord.UI
{
	/// <summary>
	/// ListBox for selected staff members to allow the "remove"
	/// image to the left of the staff members' names.
	/// </summary>
	public class EasyRemoveListView : ListView
	{
		public List<StaffStruct> Selected = new List<StaffStruct>();
		public List<StaffStruct> Unremovable = new List<StaffStruct>();
		public bool DisableEdit;
		private readonly Dictionary<ListViewItem, StaffStruct> _staffItemDictionary = new Dictionary<ListViewItem, StaffStruct>();
		private readonly Dictionary<ListViewItem, StaffStruct> _unremovableStaffItemDictionary = new Dictionary<ListViewItem, StaffStruct>();

		public EasyRemoveListView()
		{
			View = View.Details;
			Columns.Add("Staff", Width);
			HeaderStyle = ColumnHeaderStyle.None;
			DisableEdit = false;
			MouseMove +=
				delegate(object sender, MouseEventArgs e)
				{
					if (!DisableEdit)
					{
						ListViewItem item = HitTest(e.X, e.Y).Item;
						if (item != null)
						{
							if (HitTest(e.X, e.Y).Location == ListViewHitTestLocations.Image && item.ImageIndex > -1)
							{
								Cursor.Current = Cursors.Hand;
							}
							else
							{
								Cursor.Current = Cursors.Default;
							}
						}
						else
						{
							Cursor.Current = Cursors.Default;
						}
					}
				};
			MouseDown +=
				delegate(object sender, MouseEventArgs e)
				{
					if (!DisableEdit)
					{
						ListViewItem item = HitTest(e.X, e.Y).Item;
						if (item != null)
						{
							if (HitTest(e.X, e.Y).Location == ListViewHitTestLocations.Image && item.ImageIndex > -1)
							{
								RemoveItem(item);
							}
						}
					}
				};
			SmallImageList = new ImageList();
			SmallImageList.Images.Add(Properties.Resources.remove);
		}

		protected void RemoveItem(ListViewItem item)
		{
			StaffStruct remove;
			_staffItemDictionary.TryGetValue(item, out remove);
			Items.Remove(item);
			Selected.Remove(remove);
			_staffItemDictionary.Remove(item);
		}

		protected void RemoveAllRegular()
		{
			foreach(var staff in _staffItemDictionary.Select(t => t.Key).ToList())
			{
				Items.Remove(staff);
				_staffItemDictionary.Remove(staff);
			}
		}

		public void PopulateRegular(List<StaffStruct> selected)
		{
			Selected = selected;
			RemoveAllRegular();
			foreach (StaffStruct staff in Selected)
			{
				ListViewItem item;
				if (SmallImageList.Images.Count > 0)
				{
					item = new ListViewItem(staff.Name, 0);
				}
				else
				{
					item = new ListViewItem(staff.Name);
				}
				Items.Add(item);
				_staffItemDictionary.Add(item, staff);
			}
		}

		protected void AddUnremovableStaff(StaffStruct unremovable)
		{
			var item = new ListViewItem(unremovable.Name);

			Items.Add(item);

			_unremovableStaffItemDictionary.Add(item, unremovable);
		}

		/// <summary>
		/// Resets 
		/// </summary>
		/// <param name="newUnremovable"></param>
		public void ResetUnremovableStaff(List<StaffStruct> newUnremovable)
		{
			foreach (KeyValuePair<ListViewItem, StaffStruct> x in _unremovableStaffItemDictionary)
			{
				x.Key.Remove();
			}

			_unremovableStaffItemDictionary.Clear();

			foreach (StaffStruct unremovable in newUnremovable)
			{
				if (_staffItemDictionary.Values.Contains(unremovable))
				{
					var pair = _staffItemDictionary.Where(s => s.Value == unremovable).FirstOrDefault();

					//remove the image
					pair.Key.ImageIndex = -1;

					_staffItemDictionary.Remove(pair.Key);
				}
				else
				{
					AddUnremovableStaff(unremovable);
				}
			}
		}

		public void Add(StaffStruct selected)
		{
			if (Selected.Any(r => r.ID == selected.ID) || Unremovable.Any(r => r.ID == selected.ID))
			{
				return;
			}

			ListViewItem item;

			item = SmallImageList.Images.Count > 0 
				? new ListViewItem(selected.Name, 0) 
				: new ListViewItem(selected.Name);
			Items.Add(item);
			_staffItemDictionary.Add(item, selected);
			Selected.Add(selected);
		}

		public void Reset()
		{
			Items.Clear();
			Selected.Clear();
			Unremovable.Clear();
		}
	}
}
