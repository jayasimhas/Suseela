using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Sitecore;

namespace Informa.Library.CustomSitecore.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SitecoreValidateAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
    {
        private string _salt;

        public SitecoreValidateAntiForgeryTokenAttribute()
            : this(System.Web.Helpers.AntiForgery.Validate)
        {
        }

        internal SitecoreValidateAntiForgeryTokenAttribute(Action validateAction)
        {
            Debug.Assert(validateAction != null);
            ValidateAction = validateAction;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly",
            MessageId = "AdditionalDataProvider", Justification = "API name.")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly",
            MessageId = "AntiForgeryConfig", Justification = "API name.")]
        [Obsolete(
            "The 'Salt' property is deprecated. To specify custom data to be embedded within the token, use the static AntiForgeryConfig.AdditionalDataProvider property.",
            true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Salt
        {
            get { return _salt; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    throw new NotSupportedException(
                        "The 'Salt' property is deprecated. To specify custom data to be embedded within the token, use the static AntiForgeryConfig.AdditionalDataProvider property.");
                }
                _salt = value;
            }
        }

        internal Action ValidateAction { get; }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (Context.PageMode.IsNormal)
            {
                ValidateAction();
            }
        }
    }
}