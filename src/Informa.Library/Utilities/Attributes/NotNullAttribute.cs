using System;
using System.Diagnostics;

namespace Informa.Library.Utilities.Attributes
{
    //Lovingly borrowed from Sitecore.Kernel
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Delegate, AllowMultiple = false, Inherited = true)]
    [Conditional("NOP")]
    public sealed class NotNullAttribute : Attribute
    {
    }
}
