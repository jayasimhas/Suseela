using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Library.User.Profile;
using Jabberwocky.Glass.Autofac.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore.Security.Authentication;
using System;
using System.Configuration;
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
        protected readonly IUserProfileFactory UserProfileFactory;
        protected readonly ISitecoreVirtualUsernameFactory VirtualUsernameFactory;

        public AuthenticatedUserContext(ISitecoreUserContext sitecoreUserContext, IGlobalSitecoreService globalService, ISiteRootContext siteRootContext, IUserSession userSession, IUserProfileFactory userProfileFactory,
            ISitecoreVirtualUsernameFactory virtualUsernameFactory)
		{
			SitecoreUserContext = sitecoreUserContext;
            GlobalService = globalService;
            SiteRootContext = siteRootContext;
            UserSession = userSession;
            UserProfileFactory = userProfileFactory;
            VirtualUsernameFactory = virtualUsernameFactory;
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
            string domain = ConfigurationManager.AppSettings["EnableVerticalLogin"];
            if(domain != "true")
            {
                return SitecoreUserContext.User.IsAuthenticated;
            }
            var curVertical = GlobalService.GetVerticalRootAncestor(Sitecore.Context.Item.ID.ToGuid())?._Name;
            //Current vertical cookiename
            string cookieName = curVertical + "_LoggedInUser";
            if (SitecoreUserContext.User.IsAuthenticated && HttpContext.Current.Request.Cookies[cookieName] != null)
            return true;
          
            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                string username = HttpContext.Current.Request.Cookies[cookieName].Value;
                if (!string.IsNullOrEmpty(username))
                {
                    string uToken = username.Contains("|") ? username.Split('|')[1] : string.Empty;
                    IAuthenticatedUser user = new AuthenticatedUser() { Username = username ,AccessToken = uToken };
                    var sitecoreUsername = VirtualUsernameFactory.Create(user);
                    var sitecoreVirtualUser = AuthenticationManager.BuildVirtualUser(sitecoreUsername, true);
                    var userProfile = UserProfileFactory.Create(user);
                    if (userProfile != null)
                    {
                        sitecoreVirtualUser.Profile.Email = userProfile.Email;
                        sitecoreVirtualUser.Profile.Name = string.Format("{0} {1}", userProfile.FirstName, userProfile.LastName);
                    }

                    sitecoreVirtualUser.Profile.Comment = string.IsNullOrWhiteSpace(user.AccessToken) ? string.Empty : user.AccessToken;
                    sitecoreVirtualUser.Profile.Save();
                    var success = AuthenticationManager.Login(sitecoreVirtualUser.Name, true);
                    return true;
                }
                //if (success)
                //{
                //    LoginActions.Process(user);
                //}
            }
            return false;
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
