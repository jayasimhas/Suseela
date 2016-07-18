using System;
using System.IO;
using System.Xml.Serialization;
using Informa.Library.Utilities.Extensions;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Core.Caching;

namespace Informa.Library.Utilities.Parsers
{
    public interface ICachedXmlParser
    {
        T ParseContent<T>(string xmlContent, string cacheKey, TimeSpan? timeSpan = null) where T : class;
    }

    [AutowireService]
    public class CachedXmlParser : ICachedXmlParser
    {
        private readonly IDependencies _dependencies;
        private readonly TimeSpan _defaultTimeSpan = new TimeSpan(0, 5, 0);

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            ICacheProvider CacheProvider { get; set; }
        }
        public CachedXmlParser(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public T ParseContent<T>(string xmlContent, string cacheKey, TimeSpan? timeSpan = null) where T : class
        {
            var finalTimeSpan = timeSpan ?? _defaultTimeSpan;
            return _dependencies.CacheProvider
                .GetFromCache($"CachedXmlParser:ParseContent:{cacheKey}", finalTimeSpan,
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