using System.Text;
using System.Xml;
using AutoMapper;
using Informa.Library.Services.NlmExport.Parser.Legacy;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Utilities.AutoMapper.Resolvers.Nlm.Body
{
    public class ArticleBodyResolver : BaseValueResolver<ArticleItem, string>
    {


        protected override string Resolve(ArticleItem source, ResolutionContext context)
        {
            var builder = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Fragment
            };
            using (var writer = XmlWriter.Create(builder, settings))
            {
                DocumentParser.Parse($"<body>{source.Body}</body>", writer);
            }

            var bodyXml = Helpers.ConvertSpecialCharacters(builder.ToString());
            bodyXml = bodyXml.Replace("&amp;#", "&#");

            return bodyXml;
        }
    }
}
