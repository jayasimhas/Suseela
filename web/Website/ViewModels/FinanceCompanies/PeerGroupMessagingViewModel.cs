using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels.FinanceCompanies
{
    public class PeerGroupMessagingViewModel : GlassViewModel<I___BasePage>
    {

        protected readonly ITextTranslator TextTranslator;
        public ISitecoreContext SitecoreContext;

        public PeerGroupMessagingViewModel(ITextTranslator textTranslator, ISitecoreContext sitecoreContext)
        {
            TextTranslator = textTranslator;
            SitecoreContext = sitecoreContext;
        }
        /// <summary>
        /// Instruction Copy Title
        /// </summary>
        public string InstructionTitle => GlassModel?.Title;
        /// <summary>
        /// Instruction Text
        /// </summary>
        public string InstructionText => GlassModel?.Body;       
        /// <summary>
        /// Dictionary Item for Dont show message text
        /// </summary>
        public string MessageBoxHideText => TextTranslator.Translate("CompanyMessage.HideText");
        /// <summary>
        /// Dictionary Item for show message text
        /// </summary>
        public string MessageBoxShowText => TextTranslator.Translate("CompanyMessage.ShowText");
    }
}