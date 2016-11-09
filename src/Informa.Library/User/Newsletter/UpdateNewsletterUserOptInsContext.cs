using Informa.Library.User.Authentication;
using Jabberwocky.Autofac.Attributes;
using log4net;
using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
    [AutowireService]
    public class UpdateNewsletterUserOptInsContext : IUpdateNewsletterUserOptInsContext
    {
        protected readonly IUpdateNewsletterUserOptIns UpdateUserOptIns;
        protected readonly INewsletterUserOptInsContext OptInsContext;
        protected readonly IAuthenticatedUserContext UserContext;
        protected readonly ILog Logger;

        public UpdateNewsletterUserOptInsContext(
            IUpdateNewsletterUserOptIns updateUserOptIns,
            INewsletterUserOptInsContext optInsContext,
            IAuthenticatedUserContext userContext,
            ILog logger)
        {
            UpdateUserOptIns = updateUserOptIns;
            OptInsContext = optInsContext;
            UserContext = userContext;
            Logger = logger;
        }

        public bool Update(IEnumerable<INewsletterUserOptIn> optIns)
        {
            Logger.Error("UserContext.IsAuthenticated : " + UserContext.IsAuthenticated);
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
