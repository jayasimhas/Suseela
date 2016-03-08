using System.Collections.Generic;
using System.Linq;
using Informa.Library.Globalization;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System.Web;
using System.Web.Mvc;
using Glass.Mapper;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Configuration.Attributes;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Glass.Models;
using Sitecore.Common;
using Sitecore.Data.Items;

namespace Informa.Web.Areas.Account.ViewModels.Manangement
{
	public class AccountNavViewModel : GlassViewModel<I___BasePage>
	{
        public ISitecoreContext SitecoreContext;
        public AccountNavViewModel(ISitecoreContext sitecoreContext)
        {
            SitecoreContext = sitecoreContext;
        }

	    public IEnumerable<I___BasePage> NavigationItems => SitecoreContext
                    .GetCurrentItem<Item>()
                    .Parent
                    .Children
	                .ToList()
                    .Select(a => a.GlassCast<I___BasePage>());

	    public string CssClass(I___BasePage p)
	    {
            return (SitecoreContext.GetCurrentItem<I___BasePage>()._Id.Equals(p._Id)) 
                ? "is-active" 
                : string.Empty;
        }

	    public IEnumerable<I___BasePage> Breadcrumbs
	    {
            get { 
                var pages = new List<I___BasePage>();
                I___BasePage current = SitecoreContext.GetCurrentItem<I___BasePage>();
                while (current._Parent != null && !current._TemplateId.ToID().Equals(ISite_RootConstants.TemplateId))
                {
                    pages.Add(current);
                    current = SitecoreContext.GetItem<I___BasePage>(current._Parent._Id);
                }
                pages.Reverse();
                return pages;
            }
        } 
	}
}
