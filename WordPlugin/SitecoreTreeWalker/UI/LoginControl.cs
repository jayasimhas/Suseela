using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using InformaSitecoreWord.Config;
using System.Deployment;
using System.Deployment.Application;
using System.Reflection;
using InformaSitecoreWord.Sitecore;
using InformaSitecoreWord.User;
using InformaSitecoreWord.Util;
using PluginModels;

namespace InformaSitecoreWord.UI
{
    public partial class LoginControl : UserControl
    {
        private readonly SitecoreUser _user = SitecoreUser.GetUser();
        public Control ToReveal;
        private readonly UserCredentialReader _reader = UserCredentialReader.GetReader();
        //Format of PASSWORD_FILE is
        //[USERNAME]
        //[PASSWORD]

        public LoginControl()
        {
            InitializeComponent();
        }

        public void FocusUsernameField()
        {
            uxUsername.Focus();
        }

        public void Reveal(Control toReveal)
        {
            ToReveal = toReveal;
        }

        /// <summary>
        /// Shows login display and hides all other controls.
        /// </summary>
        public void ShowLogin()
        {
            if (!SitecoreClient.IsAvailable())
            {
                ShowError(Constants.ConnectionUnavailable);
            }
            else
            {
                HideError();
            }

            PopulateFields();
            Visible = true;
            if (ToReveal != null) ToReveal.Visible = false;
        }

        protected void ShowError(string errorMessage)
        {
            uxErrorMessage.Text = errorMessage;
            uxErrorMessage.Visible = true;
            uxConnectionError.Visible = true;

            // start location height = 136
            // start height = 44

            if (errorMessage.Length > 40)
            { //resizing the panel to fit
                int rows = errorMessage.Length / 40;
                var offset = rows * 15;
                uxConnectionError.Location = new Point(uxConnectionError.Location.X, 136 - offset);
                uxConnectionError.Size = new Size(uxConnectionError.Size.Width, 44 + offset);
            }
            else
            {
                uxConnectionError.Location = new Point(uxConnectionError.Location.X, 136);
                uxConnectionError.Size = new Size(uxConnectionError.Size.Width, 44);
            }
        }

        protected void HideError()
        {
            uxErrorMessage.Text = "";
            uxErrorMessage.Visible = false;
            uxConnectionError.Visible = false;

            uxConnectionError.Location = new Point(uxConnectionError.Location.X, 136);
            uxConnectionError.Size = new Size(uxConnectionError.Size.Width, 44);
        }

        /// <summary>
        /// Hides login display and shows the Article Details controls.
        /// </summary>
        public void HideLogin()
        {
            uxUsername.Clear();
            uxPassword.Clear();

            Visible = false;
            if (ToReveal != null) ToReveal.Visible = true;
        }

        /// <summary>
        /// Logs out currently logged-in user and displays login prompt.
        /// </summary>
        /// <returns>True upon successful logout. False if fails (e.g. no logged-in user to logout)</returns>
        public bool Logout()
        {
            Globals.SitecoreAddin.Log("LoginControl.Logout: Trying to log out...");
            if (_user.Logout())
            {
                Globals.SitecoreAddin.Log("LoginControl.Logout: Logout successful.");
                ShowLogin();
                return true;
            }
            Globals.SitecoreAddin.Log("LoginControl.Logout: Logout failed, most likely because no user was logged in.");
            return false;
        }

        /// <summary>
        /// This function will set the environment for the user as per the Advance settings.
        /// </summary>
        private void SetUserEnvironment()
        {
            if (_reader.HasEditorEnvironment())
            {
                Constants.EDITOR_ENVIRONMENT_VALUE = _reader.GetEditorEnvironment();
                Constants.EDITOR_ENVIRONMENT_SERVERURL = _reader.GetEditorEnvironmentServerUrl();
                Constants.EDITOR_ENVIRONMENT_LOGINURL = _reader.GetEditorEnvironmentLoginUrl();
                Constants.EDITOR_ENVIRONMENT_FORGOTPASSWORDLINK = _reader.GetEditorEnvironmentForgotPasswordLink();
            }

        }

        /// <summary>
        /// Tries to login using user credentials entered in username and password fields. If successful, 
        /// regular Article Details controls will be visible and the login prompt becomes invisible.
        /// </summary>
        private void TryToLogin()
        {
            try
            {
                SetUserEnvironment();
                Globals.SitecoreAddin.Log("LoginControl.TryToLogin: Attempting to login...");
                var userStatus = Authenticate();
                if (userStatus.LoginSuccessful)
                {


                    Globals.SitecoreAddin.Log("LoginControl.TryToLogin: Login successful, updating the UI...");
                    SuspendLayout();
                    if (uxRememberPassword.Checked)
                    {
                        Globals.SitecoreAddin.Log("LoginControl.TryToLogin: Saving the username and password...");
                        _reader.Write(uxUsername.Text, uxPassword.Text);
                    }
                    else
                    {
                        //IIPP-330 - Fixing hidding of both the fields
                        _reader.Clear();
                        Globals.SitecoreAddin.Log("LoginControl.TryToLogin: Saving the username...");
                        //_reader.Write(uxUsername.Text);
                    }
                    HideError();
                    HideLogin();
                    if (ToReveal != null) { ToReveal.Visible = true; }
                    ResumeLayout();
                }
                else if (userStatus.LockedOut)
                {
                    Globals.SitecoreAddin.Log("LoginControl.TryToLogin: User is locked out.");
                    try
                    {
                        if (string.IsNullOrEmpty(Globals.SitecoreAddin.SupportEmailAddress))
                            Globals.SitecoreAddin.SupportEmailAddress = SitecoreClient.GetContactEmail();
                    }
                    catch (Exception ex)
                    {
                        Globals.SitecoreAddin.SupportEmailAddress = string.Empty;
                        Globals.SitecoreAddin.LogException("Unable to retrieve email for support!", ex);
                    }

                    ShowError(string.Format(Constants.LockedOutUser, Globals.SitecoreAddin.SupportEmailAddress));
                    uxPassword.Clear();
                    uxPassword.Focus();
                }
                else if (userStatus.LoginAttemptsRemaining > 0)
                { //don't want to show a message where it says "you have 0 attempts remaining", that would be dumb.
                    Globals.SitecoreAddin.Log("LoginControl.TryToLogin: Credentials were invalid.");
                    ShowError(string.Format(Constants.InvalidPassword, userStatus.LoginAttemptsRemaining.ToString()));
                    uxPassword.Clear();
                    uxPassword.Focus();
                }
                else
                { // all else fails, show a generic failure
                    Globals.SitecoreAddin.Log("LoginControl.TryToLogin: Credentials were invalid.");
                    ShowError(Constants.FailedLogin);
                    uxPassword.Clear();
                    uxPassword.Focus();
                }
            }
            catch (System.Net.WebException wex)
            {
                Globals.SitecoreAddin.LogException("LoginControl.TryToLogin: Error logging in!", wex);
                Globals.SitecoreAddin.AlertConnectionFailure();
            }
        }

        /// <summary>
        /// Authenticates user credentials entered the username and password fields.
        /// </summary>
        /// <returns>True if authentication successful. Otherwise, false.</returns>
        private UserStatusStruct Authenticate()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                return _user.Authenticate(uxUsername.Text, uxPassword.Text);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void PopulateFields()
        {
            uxRememberPassword.Checked = (uxPassword.Text = _reader.GetPassword()) != null;
            if (uxRememberPassword.Checked)
            {
                uxUsername.Text = _reader.GetUsername();
            }
        }

        public Version AssemblyVersion => ApplicationDeployment.CurrentDeployment.CurrentVersion;

        private void LoginControl_Load(object sender, EventArgs e)
        {
            try
            {
                string version = $"{AssemblyVersion.Major}.{AssemblyVersion.Minor}.{AssemblyVersion.Build}.{AssemblyVersion.Revision}";
                uxVersionNumber.Text = version;
            }
            catch { }
            PopulateFields();
        }

        private void uxUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                uxLoginButton.PerformClick();
            }
        }

        private void uxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                uxLoginButton.PerformClick();
            }
        }

        private void uxLoginButton_Click(object sender, EventArgs e)
        {
            TryToLogin();
        }

        private void uxRememberPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                uxLoginButton.PerformClick();
            }
        }

        private void uxForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SetUserEnvironment();
            var forgotPasswordLink = Constants.EDITOR_ENVIRONMENT_FORGOTPASSWORDLINK;

            if (!forgotPasswordLink.IsNullOrEmpty())
            {
                Process.Start(forgotPasswordLink);
            }
        }

        private void AdvancedSettingsLbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var advancedSettingModal = new AdvancedSettings();
            advancedSettingModal.StartPosition = FormStartPosition.CenterParent;
            advancedSettingModal.ShowDialog();
        }
    }
}
