using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.Globalization;
using Informa.Library.Salesforce;
using Informa.Library.Subscription;
using Informa.Library.User.Authentication;

namespace Informa.Web.ViewModels
{
    public interface IIndividualRenewalMessageViewModel
    {
        string DismissText { get; }

        bool Display { get; }

        string Id { get; }

        string Message { get; }

        string RenewURL { get; }

        string RenewURLText { get; }
    }
}
