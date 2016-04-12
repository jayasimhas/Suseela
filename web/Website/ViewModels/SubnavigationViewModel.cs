using System.Collections.Generic;
using System.Linq;
using Glass.Mapper;
using Glass.Mapper.Sc;
using Informa.Library.Presentation;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels
{
    public class SubnavigationViewModel : GlassViewModel<IGeneral_Content_Page>
    {
        public ISitecoreContext SitecoreContext;
        public SubnavigationViewModel(ISitecoreContext sitecoreContext)
        {
            SitecoreContext = sitecoreContext;
        }
        
        public IEnumerable<I___BasePage> SubnavigationItems => GlassModel.Subnavigation_Items.Select(a => SitecoreContext.GetItem<I___BasePage>(a._Id));
    }
}