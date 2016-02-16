using System;
using System.Reflection;
using System.Web.Mvc;

namespace Informa.Library.CustomSitecore.Mvc
{
    public class CustomValidateFormHandlerAttribute : ActionMethodSelectorAttribute
    {
        protected internal const string FormHandlerControllerHiddenInput = "fhController";
        protected internal const string FormHandlerActionHiddenInput = "fhAction";

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            string str1 = controllerContext.HttpContext.Request.Form[FormHandlerControllerHiddenInput];
            string str2 = controllerContext.HttpContext.Request.Form[FormHandlerActionHiddenInput];
            string controllerContextName = this.GetControllerContextName(controllerContext);
            return !string.IsNullOrWhiteSpace(str1) && !string.IsNullOrWhiteSpace(str2) && str1 == controllerContextName && methodInfo.Name == str2;
        }

        private string GetControllerContextName(ControllerContext controllerContext)
        {
            string name = controllerContext.Controller.GetType().Name;
            int length = name.IndexOf("Controller", StringComparison.InvariantCulture);
            if (length > 0)
                return name.Substring(0, length);
            return name;
        }
    }
}