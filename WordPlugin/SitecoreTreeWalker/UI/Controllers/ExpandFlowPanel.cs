using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SitecoreTreeWalker.UI.Controllers
{
    /// <summary>
    /// This class is in charge of tying together a Control with a FlowLayoutPanel so that the Control toggles
    /// the visibility of the FlowLayoutPanel. This class's use in the summary tab 
    /// </summary>
    public class ExpandFlowPanel
    {
        public FlowLayoutPanel Expansion;
        public Control Expander;
        public Image Expanded;
        public Image Collapsed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expander">Control when clicked will "expand" the Expansion FlowLayoutPanel</param>
        /// <param name="expansion">Control that will expand and collapse</param>
        /// <param name="expanded">Backgound image for Expander when Expansion is expanded</param>
        /// <param name="collapsed">Backgound image for Expander when Expansion is collapsed</param>
        public ExpandFlowPanel(Control expander, FlowLayoutPanel expansion, Image expanded, Image collapsed)
        {
            Expander = expander;
            Expansion = expansion;
            Expanded = expanded;
            Collapsed = collapsed;

            Expansion.FlowDirection = FlowDirection.TopDown;

            Expansion.Visible = false;
            RefreshExpander();

            Expander.MouseClick +=
                delegate
                    {
                        Expansion.Visible = !Expansion.Visible;
                        RefreshExpander();
                    };

            Expander.MouseMove +=
                delegate
                    {
                        Cursor.Current = Cursors.Hand;
                    };
        	Expansion.AutoSize = true;
        }

        /// <summary>
        /// Sets the data to Expansion by adding each string as a label to the FlowLayoutPanel.
        /// </summary>
        /// <param name="data">List to add to Expansion</param>
        public void SetData(List<string> data)
        {
            Expansion.Controls.Clear();
            foreach(string d in data)
            {
                var l = new Label {Text = d};
                var p = new Padding(50, 0, 0, 0);
                l.Padding = p;
                l.AutoSize = true;
                Expansion.Controls.Add(l);
            }

            if(data.Count == 0)
            {
                var l = new Label();
                l.Font = new Font(l.Font, FontStyle.Italic);
                l.Text = @"No taxonomies were selected";
                l.AutoSize = true;
                Expansion.Controls.Add(l);
            }
        }

        /// <summary>
        /// Set correct image to Expander dependent on Expansion's state (expanded or collapsed).
        /// </summary>
        public void RefreshExpander()
        {
            Expander.BackgroundImage = Expansion.Visible ? Expanded : Collapsed;
            Expander.Refresh();
        }

        /// <summary>
        /// Expand the Expansion
        /// </summary>
        public void Expand()
        {
            Expansion.Visible = true;
            Expander.BackgroundImage = Expanded;
        }

        /// <summary>
        /// Collapse the Expansion
        /// </summary>
        public void Collapse()
        {
            Expansion.Visible = false;
            Expander.BackgroundImage = Collapsed;
        }
    }
}
