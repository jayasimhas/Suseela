using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InformaSitecoreWord.Config;
using InformaSitecoreWord.Util;

namespace InformaSitecoreWord.UI
{
    public partial class AdvancedSettings : Form
    {
        private readonly UserCredentialReader _reader = UserCredentialReader.GetReader();

        public AdvancedSettings()
        {
            InitializeComponent();
        }

        private void AdvancedSettings_Load(object sender, EventArgs e)
        {
            uxEnvironmentPicker.DataSource = Constants.EDITOR_ENVIRONMENT.ToList();
            uxEnvironmentPicker.DisplayMember = "Name";
            uxEnvironmentPicker.ValueMember = "Name";

            if (_reader.HasEditorEnvironment())
            {
                uxEnvironmentPicker.SelectedValue = _reader.GetEditorEnvironment();
            }
            else
            {
                uxEnvironmentPicker.SelectedValue = Constants.EDITOR_ENVIRONMENT_VALUE;
            }
        }

        private void AdvancedSettingCancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AdvancedSettingOkayBtn_Click(object sender, EventArgs e)
        {
            Constants.EDITOR_ENVIRONMENT_VALUE = uxEnvironmentPicker.SelectedValue.ToString();
            string serverURL = ApplicationConfig.GetPropertyValue(Constants.EDITOR_ENVIRONMENT_VALUE);
            string loginURL = ApplicationConfig.GetPropertyValue(Constants.EDITOR_ENVIRONMENT_VALUE);
            string forgotPasswordLink = ApplicationConfig.GetPropertyValue(Constants.EDITOR_ENVIRONMENT_VALUE + "ForgotPasswordLink");
            _reader.ClearEditorEnvironment();
            _reader.WriteEditorEnvironment(Constants.EDITOR_ENVIRONMENT_VALUE, serverURL, loginURL, forgotPasswordLink);
            this.Close();
        }
    }
}
