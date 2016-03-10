using System;
using System.IO;
using AutoMapper;
using Informa.Library.Services.NlmExport.Models;
using Informa.Library.Services.NlmExport.Serialization;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Services.NlmExport
{
    [AutowireService]
    public class NlmExportService : INlmExportService
    {
        private readonly IDependencies _;

        public NlmExportService(IDependencies dependencies)
        {
            if (dependencies == null) throw new ArgumentNullException(nameof(dependencies));
            _ = dependencies;
        }

        public Stream ExportNlm(ArticleItem article)
        {
            var model = _.Mapper.Map<ArticleItem, NlmArticleModel>(article);
            var memStream = new MemoryStream();

            _.Serializer.Serialize(model, memStream);
            memStream.Seek(0, SeekOrigin.Begin);

            return memStream;
        }



        [AutowireService(true)]
        public interface IDependencies
        {
            INlmSerializer Serializer { get; }
            IMapper Mapper { get; }
        }
    }
}
