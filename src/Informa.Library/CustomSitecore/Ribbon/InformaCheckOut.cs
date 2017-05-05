using Sitecore.Shell.Framework.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Web.UI.Sheer;
using System.Collections.Specialized;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Workflows;
using Sitecore.Data.Managers;
using Sitecore.Common;
using Sitecore.Sites;

namespace Informa.Library.CustomSitecore.Ribbon
{
    public class InformaCheckOut : Command
    {
        private readonly Context.ContextData _context;
        public bool Enabled
        {
            get
            {
                WorkflowContextState currentValue = Switcher<WorkflowContextState, WorkflowContextState>.CurrentValue;
                if (currentValue == WorkflowContextState.Default)
                {
                    SiteContext site = Sitecore.Context.Site;
                    return site != null && site.EnableWorkflow;
                }
                return currentValue != WorkflowContextState.Disabled;
            }
        }
        /// <summary>
		/// Executes the command in the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            if (context.Items.Length != 1)
            {
                return;
            }
            Item item = context.Items[0];
            System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection["id"] = item.ID.ToString();
            nameValueCollection["language"] = item.Language.ToString();
            nameValueCollection["version"] = item.Version.ToString();
            Context.ClientPage.Start(this, "Run", nameValueCollection);
        }
        /// <summary>
        /// Queries the state of the command.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The state of the command.</returns>
        public override CommandState QueryState(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            if (context.Items.Length != 1)
            {
                return CommandState.Hidden;
            }
            if (Context.IsAdministrator)
            {
                return CommandState.Enabled;
            }
            Item item = context.Items[0];
            if (item.Appearance.ReadOnly)
            {
                return CommandState.Disabled;
            }
            if (!item.Access.CanWrite())
            {
                return CommandState.Disabled;
            }
            if (item.Locking.HasLock())
            {
                return CommandState.Disabled;
            }
            if (!item.Access.CanWriteLanguage())
            {
                return CommandState.Disabled;
            }
            return base.QueryState(context);
        }
        /// <summary>
        /// Runs the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        protected void Run(ClientPipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            if (!SheerResponse.CheckModified())
            {
                return;
            }
            string itemPath = args.Parameters["id"];
            string name = args.Parameters["language"];
            string value = args.Parameters["version"];
            Item item = Client.GetItemNotNull(itemPath, Language.Parse(name), Sitecore.Data.Version.Parse(value));
            if (!item.Locking.IsLocked())
            {
                Log.Audit(this, "Start editing: {0}", new string[]
                {
                    AuditFormatter.FormatItem(item)
                });
                if (Context.User.IsAdministrator)
                {
                    item.Locking.Lock();
                }
                else
                {
                    item = StartEditing(item);
                }
                Context.ClientPage.SendMessage(this, string.Concat(new object[]
                {
                    "item:startediting(id=",
                    item.ID,
                    ",version=",
                    item.Version,
                    ",language=",
                    item.Language,
                    ")"
                }));
            }
        }
        public Item StartEditing(Item item)
        {
            Error.AssertObject(item, "item");
            if (!Settings.RequireLockBeforeEditing || Context.User.IsAdministrator)
            {
                return item;
            }
            if (Sitecore.Context.IsAdministrator)
            {
                return this.Lock(item);
            }
            if (StandardValuesManager.IsStandardValuesHolder(item))
            {
                return this.Lock(item);
            }
            if (!Context.Workflow.HasWorkflow(item) && !Context.Workflow.HasDefaultWorkflow(item))
            {
                return this.Lock(item);
            }
            if (!IsApproved(item, null))
            {
                return this.Lock(item);
            }
            return this.Lock(item); 
            //Item item2 = item.Versions.AddVersion();
            //if (item2 != null)
            //{
            //    return this.Lock(item2);
            //}
            
        }

        private Item Lock(Item item)
        {
            if (TemplateManager.IsFieldPartOfTemplate(FieldIDs.Lock, item) && !item.Locking.Lock())
            {
                return null;
            }
            return item;
        }

        public bool IsApproved(Item item, Database targetDatabase)
        {
            Error.AssertObject(item, "item");
            IWorkflow workflow = this.GetWorkflow(item);
            return workflow == null || workflow.IsApproved(item, targetDatabase);
        }

        public IWorkflow GetWorkflow(Item item)
        {
            Error.AssertObject(item, "item");
            if (this.Enabled)
            {
                IWorkflowProvider workflowProvider = item.Database.WorkflowProvider;
                if (workflowProvider != null)
                {
                    return workflowProvider.GetWorkflow(item);
                }
            }
            return null;
        }
    }
}
