using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Authentication
{
   public interface IverticalLogin
    {
     string curVertical { get; set; }
     void CreateLoginCookie(string userName, string userToken);
     void DeleteLoginCookie();
     string GetVerticalCookieName();
     int GetCookieTimeOut();
     string GetLoginCookieSubdomain();
     
    }
}
