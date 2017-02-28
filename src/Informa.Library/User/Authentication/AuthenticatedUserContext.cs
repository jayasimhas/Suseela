using Informa.Library.Services.Global;
using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Web;
using System.Web.Security;

namespace Informa.Library.User.Authentication
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class AuthenticatedUserContext : IAuthenticatedUserContext
	{
		protected readonly ISitecoreUserContext SitecoreUserContext;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IUserSession UserSession;

        public AuthenticatedUserContext(ISitecoreUserContext sitecoreUserContext, IGlobalSitecoreService globalService, ISiteRootContext siteRootContext, IUserSession userSession)
		{
			SitecoreUserContext = sitecoreUserContext;
            GlobalService = globalService;
            SiteRootContext = siteRootContext;
            UserSession = userSession;
        }

		public IAuthenticatedUser User
		{
			get
			{
                if(!IsAuthenticated) { return null; }

				var sitecoreUser = SitecoreUserContext.User;

                //Although sitecoreUser.Profile.Email gets filled and saved when creating the user, after the browser gets closed and reopened the Email becomes NULL.
                //So had to find another way to retrieve it when extranet
                string email = sitecoreUser.Profile.Email;
                if (string.IsNullOrEmpty(email) && sitecoreUser.IsAuthenticated && sitecoreUser.Domain.Name == "extranet")
                {
                    sitecoreUser.RuntimeSettings.IsVirtual = true;
                    sitecoreUser.Profile.Email = sitecoreUser.LocalName;
                    
                }

                

                return new AuthenticatedUser
				{
					Email = sitecoreUser.LocalName,
					Name = sitecoreUser.Profile.Name,
					Username = sitecoreUser.LocalName,
                    AccessToken = sitecoreUser.Profile.Comment
				};
			}
		}

		public bool IsAuthenticated => AuthenticatedUserCheck();

        private bool AuthenticatedUserCheck()
        {
            return SitecoreUserContext.User.IsAuthenticated;
            //if (!SitecoreUserContext.User.IsAuthenticated) return false;
            //var sitecoreUser = SitecoreUserContext.User;
            //string loggedInVertical = sitecoreUser.Profile.GetCustomProperty("vertical");
            ////The below code tries to read the user session to find the vertical. 
            //if (string.IsNullOrEmpty(loggedInVertical))
            //{
            //    var vertical_Session = UserSession.Get<string>("user_vertical");
            //    if(vertical_Session != null && !string.IsNullOrEmpty(vertical_Session.Value))
            //    {
            //        loggedInVertical = vertical_Session.Value;
            //        sitecoreUser.Profile.SetCustomProperty("vertical", loggedInVertical);
            //        if(sitecoreUser.Domain.Name == "extranet")
            //            sitecoreUser.Profile.Save();
            //    }
            //}
            //if (!string.IsNullOrEmpty(loggedInVertical))
            //{
            //    var curVertical = GlobalService.GetVerticalRootAncestor(Sitecore.Context.Item.ID.ToGuid())?._Name;
            //    if (!loggedInVertical.Contains(curVertical))
            //        return false;
            //}
            //return true;
        }
                  
	}                                      
}
