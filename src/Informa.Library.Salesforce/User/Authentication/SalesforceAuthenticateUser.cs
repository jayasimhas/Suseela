using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.Salesforce.User.Profile;
using Informa.Library.User;
using Informa.Library.User.Authentication;
using System.Linq;

namespace Informa.Library.Salesforce.User.Authentication
{
    public class SalesforceAuthenticateUser : ISalesforceAuthenticateUser
    {
        protected readonly ISalesforceServiceContext Service;
        protected readonly ISalesforceFindUserProfile FindUserProfile;



        public SalesforceAuthenticateUser(
            ISalesforceServiceContext service,
            ISalesforceFindUserProfile findUserProfile)
        {
            Service = service;
            FindUserProfile = findUserProfile;

        }

        public IAuthenticateUserResult Authenticate(string username, string password)
        {

            AuthenticateUserResultState state = AuthenticateUserResultState.Failure;

            //------------------------------
            SFv2.LoginResult loginResult = null;

            try
            {
                SFv2.LoginScopeHeader loginScopeHeader = new SFv2.LoginScopeHeader();
                loginScopeHeader.organizationId = "00D3E0000008rEm"; // TODO: read from config

                SFv2.SoapClient client = new SFv2.SoapClient("Soap");
                loginResult = client.login(loginScopeHeader, username, password);

                string userNameFromSalesForce = string.Empty;
                string sessionId = string.Empty;
                string serverUrl = string.Empty;
                string email = string.Empty;
                string userFullName = string.Empty;
                string contactId = string.Empty;

                if (loginResult != null)
                {
                    userNameFromSalesForce = loginResult.userInfo.userName;
                    sessionId = loginResult.sessionId;
                    serverUrl = loginResult.serverUrl;
                    userFullName = loginResult.userInfo.userFullName;
                    contactId = loginResult.userInfo.userEmail;

                    state = AuthenticateUserResultState.Success;
                }
                
                return new SalesforceAuthenticateUserResult
                {
                    State = state,
                    User = new SalesforceAuthenticatedUser
                    {
                        Username = userNameFromSalesForce,
                        Email = username,
                        Name = userFullName,
                        //AccountId = userAccount.accounts != null ? userAccount.accounts.Select(x => x.accountId).ToList() : null,
                        ContactId = contactId,

                        SalesForceSessionId = sessionId,
                        SalesForceURL = serverUrl
                    }
                };
            }
            catch (System.Exception ex)
            {
                string s = ex.ToString();

                return new SalesforceAuthenticateUserResult
                {
                    State = AuthenticateUserResultState.Failure

                };
            }
            
            //------------------------------

            //if(string.IsNullOrEmpty(username))
            //    return ErrorResult;

            //          var loginResponse = Service.Execute(s => s.login(username, password));

            //	if (!loginResponse.IsSuccess())
            //	{
            //		return ErrorResult;
            //	}

            //	var profile = FindUserProfile.Find(username);

            //	if (profile == null)
            //	{
            //		return ErrorResult;
            //	}


            //         var state = loginResponse.isTempPasswordSpecified && loginResponse.isTempPassword.Value ? AuthenticateUserResultState.TemporaryPassword : AuthenticateUserResultState.Success;
            //	var userAccount = Service.Execute(s => s.queryAccountByUsername(username));
            //	return new SalesforceAuthenticateUserResult
            //	{
            //		State = state,
            //		User = new SalesforceAuthenticatedUser
            //		{
            //			Username = username,
            //			Email = username,
            //			Name = string.Format("{0} {1}", profile.FirstName, profile.LastName),
            //			AccountId = userAccount.accounts != null ? userAccount.accounts.Select(x => x.accountId).ToList() : null,
            //			ContactId = loginResponse.contactId
            //		}
            //	};


            //}

            //public SalesforceAuthenticateUserResult ErrorResult => new SalesforceAuthenticateUserResult
            //{
            //	State = AuthenticateUserResultState.Failure
            //};
        }
    }
}
