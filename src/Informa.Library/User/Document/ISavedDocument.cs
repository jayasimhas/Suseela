using System;

namespace Informa.Library.User.Document
{
    public interface ISavedDocument
    {
        DateTime SaveDate { get; set; }
        string DocumentId { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string SalesforceId { get; set; }
    }
}
