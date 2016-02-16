using Autofac;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Glass.Autofac.Util;
using Jabberwocky.Glass.Models;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
using System.Linq;

namespace Informa.Library.Content.Assets
{
	public class CreatePageAssetsCommand : Command
	{
		protected ISitecoreService SitecoreService { get { return AutofacConfig.ServiceLocator.Resolve<ISitecoreService>(); } }

		public override void Execute(CommandContext context)
		{
			var rawItem = GetItem(context);
			var item = GetPage(rawItem);

			if (item == null)
			{
				return;
			}

			using (new DatabaseSwitcher(rawItem.Database))
			{
				var assetsItem = SitecoreService.Create<IPage_Assets, I___BasePage>(item, "Assets");

				assetsItem.Style = "color: darkgray; font-style: italic";

				SitecoreService.Save(assetsItem);
			}
		}

		public override CommandState QueryState(CommandContext context)
		{
			var rawItem = GetItem(context);
			var item = GetPage(rawItem);

			if (item == null)
			{
				return CommandState.Hidden;
			}

			if (item._ChildrenWithInferType.Count() == 0 || !item._ChildrenWithInferType.Any(i => i is IPage_Assets))
			{
				return CommandState.Enabled;
			}

			return CommandState.Disabled;
		}

		public virtual I___BasePage GetPage(Item rawItem)
		{
			if (rawItem == null)
			{
				return null;
			}

			var item = SitecoreService.Cast<IGlassBase>(rawItem, inferType: true);

			return item is I___BasePage ? (I___BasePage)item : null;
		}

		public Item GetItem(CommandContext context)
		{
			Assert.ArgumentNotNull(context, "context");

			return context.Items[0];
		}
	}
}
