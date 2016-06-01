using System.Collections.Generic;
using System.Linq;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Jabberwocky.Glass.Models;

namespace Informa.Web.Areas.Account.ViewModels.Management
{
	public class AccountNavViewModel : GlassViewModel<I___BasePage>
	{
		private readonly IGlassBase _currentItem;
		public AccountNavViewModel(ISitecoreContext sitecoreContext)
		{
			_currentItem = sitecoreContext.GetCurrentItem<IGlassBase>();
			NavigationItems = _currentItem._Parent._ChildrenWithInferType.OfType<I___BasePage>();
		}

		public IEnumerable<I___BasePage> NavigationItems { get; set; }

		public string CssClass(I___BasePage p)
		{
			return _currentItem._Id.Equals(p._Id)
					? "is-active"
					: string.Empty;
		}
	}
}
