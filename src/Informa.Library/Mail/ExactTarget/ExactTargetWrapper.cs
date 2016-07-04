using System;
using System.Collections.Specialized;
using System.Linq;
using FuelSDK;
using Informa.Library.Utilities.References;
using Informa.Library.Utilities.Settings;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Mail.ExactTarget
{
    public interface IExactTargetWrapper
    {
        ExactTargetResponse CreateEmail(ET_Email etEmail);
        ExactTargetResponse UpdateEmail(ET_Email etEmail);
        string EtFrontEndUrl { get; }
    }

    [AutowireService]
    public class ExactTargetWrapper : IExactTargetWrapper
    {
        private readonly IDependencies _dependencies;

        private const string EtFrontEndUrlFormat = "https://mc{0}.exacttarget.com/cloud/#app/Email/C12/Default.aspx?entityID=0%23Content";

        [AutowireService(true)]
        public interface IDependencies
        {
            ISiteSettings SiteSettings { get; }
        }

        public ExactTargetWrapper(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        private bool IsSandbox => _dependencies.SiteSettings.GetSetting(Constants.SettingKeys.ExactTargetUseSandbox)
            .Equals("true", StringComparison.InvariantCultureIgnoreCase);

        public string EtFrontEndUrl => string.Format(
            EtFrontEndUrlFormat, (IsSandbox ? ".test" : string.Empty));


        private ET_Client CreateClient()
        {
            var clientId = _dependencies.SiteSettings.GetSetting(Constants.SettingKeys.ExactTargetClientId);
            var clientSecret = _dependencies.SiteSettings.GetSetting(Constants.SettingKeys.ExactTargetSecretKey);
            var sandbox = IsSandbox ? "true" : "false";

            return
                new ET_Client(new NameValueCollection
                {
                    {nameof(clientId), clientId}, 
                    {nameof(clientSecret), clientSecret},
                    {nameof(sandbox), sandbox}
                });
        }

        public ExactTargetResponse CreateEmail(ET_Email etEmail)
        {
            var client = CreateClient();
            etEmail.AuthStub = client;
            var response = etEmail.Post();
            var result = response.Results.FirstOrDefault();

            if (string.IsNullOrEmpty(response.Message))
            {
                response.Message = response.Status ? "Email Created" : "Failed to create email.";
            }

            return new ExactTargetResponse
            {
                Success = response.Status && result != null,
                ExactTargetEmailId = result?.NewID ?? 0,
                Message = response.Message
            };
        }

        public ExactTargetResponse UpdateEmail(ET_Email etEmail)
        {
            var client = CreateClient();
            etEmail.AuthStub = client;
            var response = etEmail.Patch();

            if (string.IsNullOrEmpty(response.Message))
            {
                response.Message = response.Status ? "Email Updated" : "Failed to update email.";
            }

            return new ExactTargetResponse
            {
                ExactTargetEmailId = etEmail.ID,
                Success = response.Status,
                Message = response.Message
            };
        }
    }

    public class ExactTargetResponse
    {
        public int ExactTargetEmailId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}