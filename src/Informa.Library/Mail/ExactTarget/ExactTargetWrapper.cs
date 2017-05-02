using System;
using System.Collections.Specialized;
using System.Linq;
using FuelSDK;
using Informa.Library.Utilities.References;
using Informa.Library.Utilities.Settings;
using Jabberwocky.Autofac.Attributes;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Emails;
using Sitecore.Data.Items;
using Sitecore.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;

namespace Informa.Library.Mail.ExactTarget
{
    public interface IExactTargetWrapper
    {
        ExactTargetResponse CreateEmail(ET_Email etEmail, Item currentSiteRoot);
        ExactTargetResponse UpdateEmail(ET_Email etEmail, Item currentSiteRoot);
        //string EtFrontEndUrl { get; }
    }

    [AutowireService]
    public class ExactTargetWrapper : IExactTargetWrapper
    {
        private readonly IDependencies _dependencies;

        [AutowireService(true)]
        public interface IDependencies
        {
            ISiteSettings SiteSettings { get; }

        }

        public ExactTargetWrapper(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        private ET_Client CreateClient(Item currentSiteRoot)
        {
            try
            {
                var exactTargetConfigID = currentSiteRoot.Fields[ISite_ConfigConstants.Exact_Target_ConfigFieldName].Value;
                if (!string.IsNullOrEmpty(exactTargetConfigID))
                {
                    var ETConfigItem = currentSiteRoot.Database.GetItem(exactTargetConfigID);
                    string clientId = string.Empty;
                    string clientSecret = string.Empty;
                    string sandbox = string.Empty;
                    if (ETConfigItem != null)
                    {
                        clientId = ETConfigItem.Fields[IExactTarget_ConfigurationConstants.Client_IdFieldName].Value;
                        clientSecret = ETConfigItem.Fields[IExactTarget_ConfigurationConstants.Secret_KeyFieldName].Value;
                        sandbox = ETConfigItem.Fields[IExactTarget_ConfigurationConstants.Is_SandboxFieldName].Value.ToLower();
                    }
                   
                    return
                            new ET_Client(new NameValueCollection
                            {
                    {nameof(clientId), clientId},
                    {nameof(clientSecret), clientSecret},
                    {nameof(sandbox), sandbox}
                            });
                }
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Error("Exact target field values empty", currentSiteRoot.ID);
            }
            return null;

        }

        public ExactTargetResponse CreateEmail(ET_Email etEmail, Item currentSiteRoot)
        {
            var client = CreateClient(currentSiteRoot);
            etEmail.AuthStub = client;
            PostReturn response = null;
            try { response = etEmail.Post(); }
            catch
            {
                return new ExactTargetResponse { ExactTargetEmailId = etEmail.ID, Message = "Failed to create email. Invalid/empty exact target config" };
            }

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

        public ExactTargetResponse UpdateEmail(ET_Email etEmail, Item currentSiteRoot)
        {
            var client = CreateClient(currentSiteRoot);
            etEmail.AuthStub = client;
            PatchReturn response = null;
            try { response = etEmail.Patch(); }
            catch (Exception)
            {
                return new ExactTargetResponse { ExactTargetEmailId = etEmail.ID, Message = "Failed to update email. Invalid/Empty exact target config" };
            }

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