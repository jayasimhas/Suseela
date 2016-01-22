using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels
{
    public class AuthorModel : GlassViewModel<IAuthor>, IAuthorModel
    {
        private IAuthor _author;
        public AuthorModel(IAuthor author)
        {
            _author = author;
        }
        

        #region Implementation of IAuthorModel                       

        public string Name => _author.First_Name + " " + _author.Last_Name;
        public string Twitter => _author.Twitter;
        public string Email_Address => _author.Email_Address;
        public string Image => _author.Image?.Src;

        #endregion
    }
}