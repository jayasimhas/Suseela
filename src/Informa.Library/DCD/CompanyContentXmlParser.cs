using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Informa.Library.Utilities.Extensions;
using Informa.Models.DCD;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Core.Caching;

namespace Informa.Library.DCD
{
    public interface ICompanyContentXmlParser
    {
        CompanyContent ParseContent(string xmlContent, string recordNumber);
        string GetParentCompany(CompanyContent content);
    }

    [AutowireService]
    public class CompanyContentXmlParser : ICompanyContentXmlParser
    {
        private readonly IDependencies _dependencies;
        private readonly TimeSpan _timeSpan = new TimeSpan(0, 5, 0);

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            ICacheProvider CacheProvider { get; set; }
        }

        public CompanyContentXmlParser(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public CompanyContent ParseContent(string xmlContent, string recordNumber)
        {
            return _dependencies.CacheProvider
                .GetFromCache($"CompanyContentXmlParser:GetParsedConteny:{recordNumber}", _timeSpan,
                    () => ParseContentImplementation(xmlContent));
        }

        private static CompanyContent ParseContentImplementation(string xmlContent)
        {
            if (!xmlContent.HasContent()) { return null; }

            var serializer = new XmlSerializer(typeof(CompanyContent));
            using (var xmlReader = new StringReader(xmlContent))
            {
                return serializer.Deserialize(xmlReader) as CompanyContent;
            }
        }

        public string GetParentCompany(CompanyContent content)
        {
            return content.ParentsAndDivisions.CompanyPaths.FirstOrDefault()?.Path;
        }
    }
}