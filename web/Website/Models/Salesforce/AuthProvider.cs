using Newtonsoft.Json;
using Salesforce.Common.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Web.Models.Salesforce
{
    public class AuthProvider
    {
        [Key]
        [Display(Name = "Auth. Provider ID")]
        [Createable(false), Updateable(false)]
        public string Id { get; set; }

        [Display(Name = "Created Date")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset CreatedDate { get; set; }

        [Display(Name = "Provider Type")]
        public string ProviderType { get; set; }

        [Display(Name = "Name")]
        [StringLength(32)]
        public string FriendlyName { get; set; }

        [Display(Name = "URL Suffix")]
        [StringLength(32)]
        public string DeveloperName { get; set; }

        [Display(Name = "Class ID")]
        public string RegistrationHandlerId { get; set; }

        [Display(Name = "User ID")]
        public string ExecutionUserId { get; set; }

        [Display(Name = "Consumer Key")]
        [StringLength(256)]
        public string ConsumerKey { get; set; }

        [Display(Name = "Consumer Secret")]
        [StringLength(100)]
        [Updateable(false)]
        public string ConsumerSecret { get; set; }

        [Display(Name = "Custom Error URL")]
        [StringLength(500)]
        public string ErrorUrl { get; set; }

        [Display(Name = "Authorize Endpoint URL")]
        [Url]
        public string AuthorizeUrl { get; set; }

        [Display(Name = "Token Endpoint URL")]
        [Url]
        public string TokenUrl { get; set; }

        [Display(Name = "User Info Endpoint URL")]
        [Url]
        public string UserInfoUrl { get; set; }

        [Display(Name = "Default Scopes")]
        [StringLength(256)]
        public string DefaultScopes { get; set; }

        [Display(Name = "Token Issuer")]
        [StringLength(1024)]
        public string IdTokenIssuer { get; set; }

        [Display(Name = "Send access token in header")]
        public bool OptionsSendAccessTokenInHeader { get; set; }

        [Display(Name = "Send client credentials in header")]
        public bool OptionsSendClientCredentialsInHeader { get; set; }

        [Display(Name = "Include identity organization's organization ID for third-party account linkage")]
        public bool OptionsIncludeOrgIdInId { get; set; }

        [Display(Name = "Icon URL")]
        [Url]
        public string IconUrl { get; set; }

    }
}
