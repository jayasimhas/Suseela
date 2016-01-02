using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SitecoreTreeWalker.User;

namespace SitecoreTreeWalker.UI.Controllers
{
    public class LoginController
    {
        public TextBox Username;
        public TextBox Password;
        public Button LoginButton;
        public CheckBox RememberMe;
        public Control Reveal;
        public List<Control> ToHide = new List<Control>();
        private readonly SitecoreUser _user = SitecoreUser.GetUser();
        public Button LogoutButton;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username">TextBox for username</param>
        /// <param name="password">TextBox for password</param>
        /// <param name="loginButton">Login button</param>
        /// <param name="rememberMe">Check box to remember credentials</param>
        /// <param name="reveal">Controls to reveal upon login</param>
        /// <param name="toHide">Controls to hide upon login</param>
        /// <param name="logoutButton">Logout button</param>
        public LoginController(TextBox username, TextBox password, Button loginButton, CheckBox rememberMe, 
            Control reveal, IEnumerable<Control> toHide, Button logoutButton)
        {
            Username = username;
            Password = password;
            LoginButton = loginButton;
            RememberMe = rememberMe;
            LogoutButton = logoutButton;

            ToHide.Add(Username);
            ToHide.Add(Password);
            ToHide.Add(LoginButton);
            ToHide.Add(RememberMe);
            Reveal = reveal;

            ToHide.AddRange(toHide);
            AddEventHandlers();
            if(_user.LoggedIn)
            {
                HideLogin();
            }
            else
            {
                ShowLogin();
            }
        }

        public void AddEventHandlers()
        {
            Password.KeyDown +=
                delegate(object sender, KeyEventArgs e)
                    {
                        if (e.KeyCode == Keys.Enter)
                        {
                            TryToLogin();
                        }
                    };

            LoginButton.Click +=
                delegate(object sender, EventArgs e)
                    {
                        TryToLogin();
                    };
            LogoutButton.Click +=
                delegate
                    {
                        Logout();
                    };
        }

        /// <summary>
        /// Tries to login using user credentials entered in username and password fields. If successful, 
        /// regular Article Details controls will be visible and the login prompt becomes invisible.
        /// </summary>
        private void TryToLogin()
        {
            if(Authenticate())
            {
                HideLogin();
                Reveal.Visible = true;
            }
        }

        /// <summary>
        /// Authenticates user credentials entered the username and password fields.
        /// </summary>
        /// <returns>True if authentication successful. Otherwise, false.</returns>
        private bool Authenticate()
        {
            return _user.Authenticate(Username.Text, Password.Text);
        }

        /// <summary>
        /// Hides login display and shows the Article Details controls.
        /// </summary>
        public void HideLogin()
        {
            ToHide.ForEach(c => c.Visible = false);
            Reveal.Visible = true;
        }

        /// <summary>
        /// Shows login display and hides all other controls.
        /// </summary>
        public void ShowLogin()
        {
            ToHide.ForEach(c => c.Visible = true);
            Reveal.Visible = false;
        }

        /// <summary>
        /// Logs out currently logged-in user and displays login prompt.
        /// </summary>
        /// <returns>True upon successful logout. False if fails (e.g. no logged-in user to logout)</returns>
        public bool Logout()
        {
            if(_user.Logout())
            {
                ShowLogin();
                return true;
            }
            return false;
        }
    }
}
