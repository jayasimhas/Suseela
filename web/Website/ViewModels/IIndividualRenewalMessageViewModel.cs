using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.ViewModels
{
    public interface IIndividualRenewalMessageViewModel
    {
        string Message { get; }
        string DismissText { get; }
        bool Display { get; }
        string RenewURL { get; }
        string RenewURLText { get;  }
        string Id { get; }
    }
}