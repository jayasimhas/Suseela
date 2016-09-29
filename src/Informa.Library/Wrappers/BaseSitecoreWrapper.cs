using System;
using Informa.Library.Utilities.References;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Informa.Library.Wrappers
{
    public class BaseSitecoreWrapper
    {
        private Database _coreDb;
        protected Database CoreDb => _coreDb ?? (_coreDb = Sitecore.Configuration.Factory.GetDatabase(Constants.CoreDb));

        private Database _masterDb;
        protected Database MasterDb => _masterDb ?? (_masterDb = Sitecore.Configuration.Factory.GetDatabase(Constants.MasterDb));

        private Database _web;
        protected Database WebDb => _web ?? (_web = Sitecore.Configuration.Factory.GetDatabase(Constants.WebDb));

        protected Database GetDatabase(string databaseName)
        {
            switch (databaseName)
            {
                case Constants.CoreDb:
                    return CoreDb;
                case Constants.MasterDb:
                    return MasterDb;
                case Constants.WebDb:
                default:
                    return WebDb;
            }
        }

        protected Item GetItem(Guid itemId, string databaseName = null)
        {
            var database = databaseName == null
                ? Sitecore.Context.Database
                : GetDatabase(databaseName);

            return database.GetItem(new ID(itemId));
        }
    }
}