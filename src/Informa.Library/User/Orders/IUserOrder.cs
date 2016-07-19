using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.Subscription;
using Informa.Library.User.Authentication;

namespace Informa.Library.User.Orders {
    public interface IUserOrder {
        ICreateUserOrderResult CreateUserOrder(IAuthenticatedUser user, IEnumerable<ISubscription> subscriptions);
    }
}
