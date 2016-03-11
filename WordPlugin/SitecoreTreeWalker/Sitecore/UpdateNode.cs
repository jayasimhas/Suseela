using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace InformaSitecoreWord.Sitecore
{
	public class UpdateNode
	{
		public UpdateNode(SitecorePath sitecorePath, TreeNode node, UserControl control)
		{
			Node = node;
			ControlReference = control;
			SitecorePath = sitecorePath;
		}

		private SitecorePath SitecorePath { get; set; }
		private TreeNode Node { get; set; }
		private UserControl ControlReference { get; set; }

		public void Update()
		{
			try
			{
				var currentCursor = ControlReference.Cursor;
				UpdateCursor(Cursors.WaitCursor);
				UpdateNodeColor(Color.Gray, true);
				System.Threading.Thread.Sleep(200);
				UpdateNestedNode();
				UpdateCursor(currentCursor);
			}
			catch(System.Web.Services.Protocols.SoapException e)
			{
			    Globals.SitecoreAddin.Alert("Window has gotten out of sync with Sitecore! " +
			                                "Please refresh tab.");
			    Globals.SitecoreAddin.LogException("Updating nested node:", e);
			}
		}

		private delegate void UpdateNestedNodeDel();
		private void UpdateNestedNode()
		{
			try
			{
				if (ControlReference.InvokeRequired)
				{
					ControlReference.Invoke(new UpdateNestedNodeDel(UpdateNestedNode));
				}
				else
				{
					SitecorePath.GetDecendents();
					AddTreeNode(SitecorePath, Node.Nodes);
					UpdateNodeColor(Color.Black, false);
				}
			}
			catch (System.Net.WebException e)
			{
				Globals.SitecoreAddin.LogException("Updating nested node:", e);
			}
			catch (System.Web.Services.Protocols.SoapException e)
			{
				Globals.SitecoreAddin.LogException("Updating nested node:", e);
			}
			
		}

		private delegate void UpdateCursorDel(Cursor cursor);
		private void UpdateCursor(Cursor cursor)
		{
			if (ControlReference.InvokeRequired)
			{
				ControlReference.Invoke(
					new UpdateCursorDel(UpdateCursor),
					new object[]{cursor});
			}
			else
			{
				ControlReference.Cursor = cursor;
			}
		}

		private delegate void UpdateNodeColorDel(Color Color, bool evalNested);
		private void UpdateNodeColor(Color color, bool evalNested)
		{
			if (evalNested && SitecorePath.NestedNodesAdded) return;
			if (ControlReference.InvokeRequired)
			{
				ControlReference.Invoke(
					new UpdateNodeColorDel(UpdateNodeColor),
					new object[]{color, evalNested});
			}
			else
			{
				Node.ForeColor = color;
			}
		}

		public static void AddTreeNode(SitecorePath sitecorePath, TreeNodeCollection collection)
		{
			if (sitecorePath.NestedNodesAdded) return;
			foreach (var decendent in sitecorePath.Descendents)
			{
				collection.Add(decendent.GetNode());
			}
			sitecorePath.NestedNodesAdded = true;
		}

	}
}
