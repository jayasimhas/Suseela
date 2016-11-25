using System;

namespace Informa.Library.Search.SearchIndex
{
    public interface ISearchIndexNameService
    {
        string GetIndexName(Guid publicationGuid = default(Guid));
         
        string GetAutherIndexName();
    }
}
