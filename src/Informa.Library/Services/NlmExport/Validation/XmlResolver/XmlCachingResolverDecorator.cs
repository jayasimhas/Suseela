using System;
using System.Collections.Concurrent;
using System.IO;

namespace Informa.Library.Services.NlmExport.Validation.XmlResolver
{
    public class XmlCachingResolverDecorator : System.Xml.XmlResolver
    {
        private static readonly ConcurrentDictionary<Uri, object> EntityCache = new ConcurrentDictionary<Uri, object>();

        private readonly System.Xml.XmlResolver _innerResolver;

        public XmlCachingResolverDecorator(System.Xml.XmlResolver innerResolver)
        {
            if (innerResolver == null) throw new ArgumentNullException(nameof(innerResolver));
            _innerResolver = innerResolver;
        }

        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            var entity = EntityCache.GetOrAdd(absoluteUri,
                uri => GetEntityForCache(role, ofObjectToReturn, uri));

            var bytes = entity as byte[];
            if (bytes != null)
            {
                return new MemoryStream(bytes);
            }

            return entity;
        }

        private object GetEntityForCache(string role, Type ofObjectToReturn, Uri uri)
        {
            var entity = _innerResolver.GetEntity(uri, role, ofObjectToReturn);

            var fileStream = entity as FileStream;
            if (fileStream == null)
            {
                return entity;
            }

            using (var memStream = new MemoryStream())
            {
                using (fileStream)
                {
                    fileStream.CopyTo(memStream);
                }

                return memStream.ToArray();
            }
        }
    }
}
