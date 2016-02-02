using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels
{
    public class PersonModel : GlassViewModel<I___Person>, IPersonModel
    {
        private I___Person _author;
        public PersonModel(I___Person author)
        {
            _author = author;
        }
        

        #region Implementation of IPersonModel                       

        public string Name => _author.First_Name + " " + _author.Last_Name;
        public string Twitter => _author.Twitter;
        public string Email_Address => _author.Email_Address;
        public string Image => _author.Image?.Src;

        #endregion
    }
}