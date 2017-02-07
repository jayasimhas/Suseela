using Informa.Library.Salesforce.User.Entitlement;

namespace Informa.Library.Salesforce.V2.User.Entitlement
{
    public class SalesforceEntitlmentFactoryV2 : ISalesforceEntitlmentFactoryV2
    {
        public SalesforceEntitlement Create(UserEntitlement entitlement)
        {
            var archiveLimitedDays = entitlement.Code != null ? ArchiveCodeToDays(entitlement.Code) : 0;
            var archiveLimited = archiveLimitedDays != -1;

            return new SalesforceEntitlement
            {
                ArchiveCode = entitlement.Code,
                ArchiveLimited = archiveLimited,
                ArchiveLimitedDays = archiveLimitedDays,
                ProductCode = string.IsNullOrWhiteSpace(entitlement.Code) ? string.Empty : entitlement.Code,
                ProductType = entitlement.ProductType,
                Type = entitlement.Type,
                StartDate = entitlement.StartDate,
                SalesEndDate = entitlement.SalesEndDate,
                AccessEndDate = entitlement.AccessEndDate,
                ProductName = entitlement.ProductName,
                Description = entitlement.Description,
                Name = entitlement.Name
            };
        }

        public int ArchiveCodeToDays(string archiveCode)
        {
            switch (archiveCode.ToLower())
            {
                case "full":
                case "n/a":
                    return -1;
                case "555":
                    return 540;
                default:
                    var days = 0;
                    int.TryParse(archiveCode, out days);

                    return days;
            }
        }
    }
}
