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
                OmitXmlDeclaration = true
            };
            using (var writer = XmlWriter.Create(builder, settings))
            {
                DocumentParser.Parse(source.Body, writer);
            }

            return builder.ToString();
        }
    }
}
