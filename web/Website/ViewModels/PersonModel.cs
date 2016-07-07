using System.Runtime.CompilerServices;
using Autofac;
using Glass.Mapper.Sc.Fields;
using Informa.Library.Authors;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Util;

namespace Informa.Web.ViewModels
{
    public class PersonModel : IPersonModel
    {
        private readonly I___Person _author;

        public PersonModel(I___Person author)
        {
            _author = author;
        }

        #region Implementation of IPersonModel                       

        public string Name => $"{_author.First_Name} {_author.Last_Name}";
        public string Twitter => _author.Twitter;
        public string Email_Address => _author.Email_Address;
        public Image Image => _author.Image;

        public string AuthorLink
        {
            get
            {
                using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
                {
                    var authorClient = scope.Resolve<IAuthorService>();
                    return authorClient.ConvertUrlNameToLink(authorClient.GetUrlName(_author._Id));
                }
            }
        }

        public bool InActive => _author.Inactive;
        #endregion
    }
}