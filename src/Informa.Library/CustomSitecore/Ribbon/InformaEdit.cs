using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Security.AccessControl;
using Sitecore.Shell.Framework.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Informa.Library.CustomSitecore.Ribbon
{
    public class InformaEdit : Command
    {
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
            if (InformaEdit.CanCheckIn(item))
            {
                Sitecore.Context.ClientPage.SendMessage(this, "item:checkin");
                return;
            }
            Sitecore.Context.ClientPage.SendMessage(this, "item:Informacheckout");
        }
        /// <summary>
        /// Queries the state of the command.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override CommandState QueryState(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            if (context.Items.Length != 1)
            {
                return CommandState.Hidden;
            }
            Item item = context.Items[0];
            if (!base.HasField(item, FieldIDs.Workflow) || !base.HasField(item, FieldIDs.WorkflowState))
            {
                return CommandState.Hidden;
            }
            if (!InformaEdit.HasWriteAccess(item))
            {
                return CommandState.Disabled;
            }
            if (!item.Access.CanWriteLanguage())
            {
                return CommandState.Disabled;
            }
            if (InformaEdit.CanCheckIn(item))
            {
                return CommandState.Down;
            }
            if (InformaEdit.CanEdit(item))
            {
                return CommandState.Enabled;
            }
            return CommandState.Disabled;
        }
        /// <summary>
        /// Determines whether this instance can check in the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// 	<c>true</c> if this instance can check in the specified item; otherwise, <c>false</c>.
        /// </returns>
        protected static bool CanCheckIn(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            return (!item.Appearance.ReadOnly && item.Locking.HasLock()) || (Context.IsAdministrator && item.Locking.IsLocked());
        }
        /// <summary>
        /// Determines whether context user has write access to the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// 	<c>true</c> if context user has write access to the specified item; otherwise, <c>false</c>.
        /// </returns>
        protected static bool HasWriteAccess(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            return item.Access.CanWrite();
        }
        /// <summary>
        /// Determines whether this instance can edit the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// 	<c>true</c> if this instance can edit the specified item; otherwise, <c>false</c>.
        /// </returns>
        protected static bool CanEdit(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            return !item.Appearance.ReadOnly && item.State.CanEdit() && (!AuthorizationManager.IsAllowed(item, AccessRight.ItemWrite, Context.User) || !item.Locking.HasLock());
        }
    }
}

