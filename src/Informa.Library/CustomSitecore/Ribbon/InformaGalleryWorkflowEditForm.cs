using Sitecore.Shell.Applications.ContentManager.Galleries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Shell.Framework.CommandBuilders;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using Sitecore.Workflows;
using System.Web.UI;
using Sitecore;

namespace Informa.Library.CustomSitecore.Ribbon
{
    class InformaGalleryWorkflowEditForm : GalleryForm
    {
        /// <summary>The m_check in item.</summary>
		private Item checkInItem;
        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>The options.</value>
        protected Menu Options
        {
            get;
            set;
        }
        /// <summary>Handles the message.</summary>
        /// <param name="message">The message.</param>
        public override void HandleMessage(Message message)
        {
            Assert.ArgumentNotNull(message, "message");
            base.Invoke(message, true);
            message.CancelBubble = true;
            message.CancelDispatch = true;
        }
        /// <summary>Raises the load event.</summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> instance containing the event data.</param>
        /// <remarks>This method notifies the server control that it should perform actions common to each HTTP
        /// request for the page it is associated with, such as setting up a database query. At this
        /// stage in the page lifecycle, server controls in the hierarchy are created and initialized,
        /// view state is restored, and form controls reflect client-side data. Use the IsPostBack
        /// property to determine whether the page is being loaded in response to a client postback,
        /// or if it is being loaded and accessed for the first time.</remarks>
        protected override void OnLoad(System.EventArgs e)
        {
            Assert.ArgumentNotNull(e, "e");
            base.OnLoad(e);
            if (!Context.ClientPage.IsEvent)
            {
                Item itemFromQueryString = UIUtil.GetItemFromQueryString(Context.ContentDatabase);
                if (itemFromQueryString == null)
                {
                    return;
                }
                if (!InformaGalleryWorkflowEditForm.HasField(itemFromQueryString, FieldIDs.Workflow))
                {
                    return;
                }
                IWorkflow workflow;
                WorkflowState state;
                WorkflowCommand[] array;
                InformaGalleryWorkflowEditForm.GetCommands(itemFromQueryString, out workflow, out state, out array);
                bool flag = this.IsCommandEnabled("item:checkout", itemFromQueryString);
                bool flag2 = this.IsCommandEnabled("item:checkin", itemFromQueryString);
                bool flag3 = false;
                if (array != null)
                {
                    flag3 = InformaGalleryWorkflowEditForm.CanShowCommands(itemFromQueryString, array);
                }
                this.RenderText(workflow, state, itemFromQueryString);
                if ((workflow != null && flag3) || flag || flag2)
                {
                    if (flag)
                    {
                        this.RenderEdit();
                    }
                    if (!Settings.Workflows.Enabled)
                    {
                        return;
                    }
                    if (flag2)
                    {
                        this.RenderCheckIn(itemFromQueryString);
                    }
                    if (array != null)
                    {
                        this.RenderCommands(workflow, itemFromQueryString, array);
                    }
                }
            }
        }
        /// <summary>Determines whether this instance can show commands.</summary>
        /// <param name="item">The item.</param>
        /// <param name="commands">The commands.</param>
        /// <returns><c>true</c> if this instance [can show commands] the specified item; otherwise, <c>false</c>.</returns>
        private static bool CanShowCommands(Item item, WorkflowCommand[] commands)
        {
            Assert.ArgumentNotNull(item, "item");
            Assert.ArgumentNotNull(commands, "commands");
            return !item.Appearance.ReadOnly && commands.Length > 0 && (item.Locking.CanLock() || item.Locking.HasLock() || Context.IsAdministrator);
        }
        /// <summary>Gets the commands.</summary>
        /// <param name="item">The item.</param>
        /// <param name="workflow">The workflow.</param>
        /// <param name="state">The state.</param>
        /// <param name="commands">The commands.</param>
        private static void GetCommands(Item item, out IWorkflow workflow, out WorkflowState state, out WorkflowCommand[] commands)
        {
            Assert.ArgumentNotNull(item, "item");
            workflow = null;
            commands = null;
            state = null;
            IWorkflowProvider workflowProvider = Client.ContentDatabase.WorkflowProvider;
            if (workflowProvider == null || workflowProvider.GetWorkflows().Length <= 0)
            {
                return;
            }
            workflow = workflowProvider.GetWorkflow(item);
            if (workflow == null)
            {
                return;
            }
            state = workflow.GetState(item);
            if (state == null)
            {
                return;
            }
            commands = workflow.GetCommands(item);
            commands = WorkflowFilterer.FilterVisibleCommands(commands, item);
        }
        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="workflow">The workflow.</param>
        /// <param name="state">The state.</param>
        /// <returns>The text.</returns>
        private static string GetText(Item item, IWorkflow workflow, WorkflowState state)
        {
            string str = string.Empty;
            if (item.Locking.IsLocked())
            {
                if (item.Locking.HasLock())
                {
                    str = Translate.Text("The item is locked by <b>you</b>.");
                }
                else
                {
                    str = Translate.Text("The item is locked by <b>{0}</b>.", new object[]
                    {
                        item.Locking.GetOwnerWithoutDomain()
                    });
                }
                str = "<br/><br/>" + str;
            }
            if (workflow == null)
            {
                return Translate.Text("The item is currently not part of a workflow.") + str;
            }
            string @string = StringUtil.GetString(new string[]
            {
                workflow.Appearance.DisplayName,
                "?"
            });
            if (state == null)
            {
                return Translate.Text("The item is part of the <b>{0}</b> workflow,<br/>but has no state.", new object[]
                {
                    @string
                }) + str;
            }
            string string2 = StringUtil.GetString(new string[]
            {
                state.DisplayName,
                "?"
            });
            return Translate.Text("The item is in the <b>{0}</b> state<br/>in the <b>{1}</b> workflow.", new object[]
            {
                string2,
                @string
            }) + str;
        }
        /// <summary>Determines whether the specified item has field.</summary>
        /// <param name="item">The item.</param>
        /// <param name="fieldID">The field ID.</param>
        /// <returns><c>true</c> if the specified item has field; otherwise, <c>false</c>.</returns>
        private static bool HasField(Item item, ID fieldID)
        { 
            Assert.ArgumentNotNull(item, "item");
            Assert.ArgumentNotNull(fieldID, "fieldID");
            return TemplateManager.IsFieldPartOfTemplate(fieldID, item);
        }
        /// <summary>Gets the check in item.</summary>
        /// <returns>The check in item.</returns>
        private Item GetCheckInItem()
        {
            if (this.checkInItem == null)
            {
                this.checkInItem = Client.CoreDatabase.GetItem("/sitecore/system/Settings/Workflow/Check In");
            }
            return this.checkInItem;
        }
        /// <summary>Determines whether [is command enabled] [the specified command].</summary>
        /// <param name="command">The command.</param>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [is command enabled] [the specified command]; otherwise, <c>false</c>.</returns>
        private bool IsCommandEnabled(string command, Item item)
        {
            Assert.ArgumentNotNullOrEmpty(command, "command");
            Assert.ArgumentNotNull(item, "item");
            CommandState commandState = CommandManager.QueryState(command, item);
            return commandState == CommandState.Down || commandState == CommandState.Enabled;
        }
        /// <summary>Renders the check in.</summary>
        /// <param name="item">The item.</param>
        private void RenderCheckIn(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            string header = Translate.Text("Check In");
            string icon = "Office/16x16/check.png";
            Item item2 = this.GetCheckInItem();
            if (item2 != null)
            {
                header = item2["Phrase"];
                icon = item2.Appearance.Icon;
            }
            MenuItem menuItem = new MenuItem();
            this.Options.Controls.Add(menuItem);
            menuItem.Header = header;
            menuItem.Icon = icon;
            menuItem.Click = "item:checkin";
        }
        /// <summary>Renders the commands.</summary>
        /// <param name="workflow">The workflow.</param>
        /// <param name="item">The item.</param>
        /// <param name="commands">The commands.</param>
        private void RenderCommands(IWorkflow workflow, Item item, WorkflowCommand[] commands)
        {
            Assert.ArgumentNotNull(item, "item");
            Assert.ArgumentNotNull(commands, "commands");
            for (int i = 0; i < commands.Length; i++)
            {
                WorkflowCommand workflowCommand = commands[i];
                MenuItem menuItem = new MenuItem();
                this.Options.Controls.Add(menuItem);
                menuItem.Header = workflowCommand.DisplayName;
                menuItem.Icon = workflowCommand.Icon;
                menuItem.Click = new WorkflowCommandBuilder(item, workflow, workflowCommand).ToString();
            }
        }
        /// <summary>Renders the edit.</summary>
        private void RenderEdit()
        {
            MenuItem menuItem = new MenuItem();
            this.Options.Controls.Add(menuItem);
            menuItem.Header = "Edit";
            menuItem.Icon = "Office/24x24/edit_in_workflow.png";
            menuItem.Click = "item:Informacheckout";
        }
        /// <summary>
        /// Renders the text.
        /// </summary>
        /// <param name="workflow">The workflow.</param>
        /// <param name="state">The state.</param>
        /// <param name="item">The item.</param>
        private void RenderText(IWorkflow workflow, WorkflowState state, Item item)
        {
            MenuHeader menuHeader = new MenuHeader();
            this.Options.Controls.Add(menuHeader);
            menuHeader.Header = "Workflow";
            string text = InformaGalleryWorkflowEditForm.GetText(item, workflow, state);
            MenuPanel menuPanel = new MenuPanel();
            this.Options.Controls.Add(menuPanel);
            menuPanel.Controls.Add(new System.Web.UI.LiteralControl("<div class=\"scMenuItem\">" + text + "</div>"));
        }
    }
}

