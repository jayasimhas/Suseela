using System.Reflection;
using System.Web.Mvc;

namespace Informa.Library.CustomSitecore.Mvc
{
    public class CustomValidHttpPostAttribute : ActionMethodSelectorAttribute
    {
        private static readonly HttpPostAttribute InnerHttpPostAttribute = new HttpPostAttribute();
        private static readonly CustomValidateFormHandlerAttribute InnerFormHandlerAttribute = new CustomValidateFormHandlerAttribute();

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return InnerHttpPostAttribute.IsValidForRequest(controllerContext, methodInfo)
                   && InnerFormHandlerAttribute.IsValidForRequest(controllerContext, methodInfo);
        }
    }
}