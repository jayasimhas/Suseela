using Informa.Library.User.Authentication;
using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
    [AutowireService]
    public class UpdateNewsletterUserOptInsContext : IUpdateNewsletterUserOptInsContext
    {
        protected readonly IUpdateNewsletterUserOptIns UpdateUserOptIns;
        protected readonly INewsletterUserOptInsContext OptInsContext;
        protected readonly IAuthenticatedUserContext UserContext;

        public UpdateNewsletterUserOptInsContext(
            IUpdateNewsletterUserOptIns updateUserOptIns,
            INewsletterUserOptInsContext optInsContext,
            IAuthenticatedUserContext userContext)
        {
            UpdateUserOptIns = updateUserOptIns;
            OptInsContext = optInsContext;
            UserContext = userContext;
        }

        public bool Update(IEnumerable<INewsletterUserOptIn> optIns)
        {
            if (!UserContext.IsAuthenticated)
            {
                return false;
            }

            var success = UpdateUserOptIns.Update(optIns, UserContext.User?.Username);

            if (success)
            {
                OptInsContext.Clear();
            }

            return success;
        }
    }
}
