using System.Net.Http;

namespace Informa.Library.User.Authentication
{
    public interface IAuthenticateUserV2
    {
        IAuthenticateUserResult Authenticate(string code, string grant_type, 
            string client_id, string client_secret, string redirect_uri);
    }
}
