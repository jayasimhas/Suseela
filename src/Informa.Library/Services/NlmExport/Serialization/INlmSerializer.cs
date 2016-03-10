using System.IO;
using Informa.Library.Services.NlmExport.Models;

namespace Informa.Library.Services.NlmExport.Serialization
{
    public interface INlmSerializer
    {
        void Serialize(NlmArticleModel model, Stream output);
    }
}
