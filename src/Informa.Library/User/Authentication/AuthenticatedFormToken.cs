using Jabberwocky.Glass.Autofac.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Authentication
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class AuthenticatedFormToken : IAuthenticatedFormToken
    {

        protected readonly IAuthenticatedUserContext _authenticatedUserContext;
        public AuthenticatedFormToken(IAuthenticatedUserContext authenticatedUserContext)
        {
            _authenticatedUserContext = authenticatedUserContext;
        }

        public string currentToken
        {
            get;
            set;
        }

        public string GenerateMD5Token()
        {
            if (_authenticatedUserContext.IsAuthenticated)
            {

                if ((_authenticatedUserContext.User.Email == null) || (_authenticatedUserContext.User.Email.Length == 0))
                {
                    return String.Empty;
                }

                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] textToHash = Encoding.Default.GetBytes(_authenticatedUserContext.User.Email + DateTime.Now.ToString());
                byte[] result = md5.ComputeHash(textToHash);

                string hashtoken = System.BitConverter.ToString(result);
                currentToken = hashtoken;
                return hashtoken;
            }

            return "";
        }
    }
}

