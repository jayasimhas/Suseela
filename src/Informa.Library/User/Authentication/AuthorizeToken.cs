using Autofac;
using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Util;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net;
using System.Web;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Web.Http;

namespace Informa.Library.Utilities.WebApi.Filters
{
    public class AuthorizeToken : AuthorizeAttribute
    {

        private IAuthenticatedFormToken _iAuthenticatedFormToken;


        public override void OnAuthorization(HttpActionContext actionContext)
        {

            using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
            {
                _iAuthenticatedFormToken = scope.Resolve<IAuthenticatedFormToken>();
            }

            string authToken = HttpContext.Current.Request.Form["AuthToken"].ToString();

            if (authToken != _iAuthenticatedFormToken.currentToken)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            base.OnAuthorization(actionContext);
        }





    }
}
