using System;
using System.IO;
using System.Xml.Serialization;
using Informa.Library.Utilities.Extensions;
using Informa.Models.DCD;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Core.Caching;

namespace Informa.Library.DCD
{
    public interface IDcdXmlParser
    {
        T ParseContent<T>(string xmlContent, string recordNumber) where T : class;
    }

    [AutowireService]
    public class DcdXmlParser : IDcdXmlParser
    {
        private readonly IDependencies _dependencies;
        private readonly TimeSpan _timeSpan = new TimeSpan(0, 5, 0);

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            ICacheProvider CacheProvider { get; set; }
        }

        public DcdXmlParser(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public T ParseContent<T>(string xmlContent, string recordNumber) where T : class
        {
            return _dependencies.CacheProvider
                .GetFromCache($"DcdXmlParser:ParseContent:{recordNumber}", _timeSpan,
                    () => ParseContentImplementation<T>(xmlContent));
        }

        private static T ParseContentImplementation<T>(string xmlContent) where T : class
        {
            if (!xmlContent.HasContent()) { return default(T); }

            var serializer = new XmlSerializer(typeof(T));
            using (var xmlReader = new StringReader(xmlContent))
            {
                return serializer.Deserialize(xmlReader) as T;
            }
        }
    }
}