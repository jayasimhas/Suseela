using Informa.Library.User.Authentication;
using Jabberwocky.Autofac.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Entitlement {
    public interface IStrategicEntitlementContext {
        bool AuthenticatedUserHasStrategicEntitlements();
    }

    [AutowireService]
    public class StrategicEntitlementContext : IStrategicEntitlementContext {

        private IDependencies _ { get; set; }

        [AutowireService(true)]
        public interface IDependencies {
            IGetUserEntitlements UserEntitlements { get; set; }
            IAuthenticatedUserContext UserContext { get; set; }
            IUserIpAddressContext IpContext { get; set; }
        }

        public StrategicEntitlementContext(IDependencies dependencies) {
            _ = dependencies;
        }

        public bool AuthenticatedUserHasStrategicEntitlements() {

            IList<IEntitlement> entitlements = (_.UserContext.IsAuthenticated)
                ? _.UserEntitlements.GetEntitlements(_.UserContext.User.Email, _.IpContext.IpAddress.ToString())
                : new List<IEntitlement>();

            return entitlements.Any(e 
                => e.ProductCode.Equals("SLTR") 
                || e.ProductCode.Equals("SLDX") 
                || e.ProductCode.Equals("SLRX"));
        }
    }
}
