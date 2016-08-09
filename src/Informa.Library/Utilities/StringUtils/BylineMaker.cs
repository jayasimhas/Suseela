using System.Collections.Generic;
using System.Linq;
using Informa.Library.Authors;
using Informa.Library.Globalization;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Utilities.StringUtils
{
    public interface IBylineMaker
    {
        string MakeByline(IEnumerable<I___Person> authors);
        string MakePrintByLine(IEnumerable<I___Person> authors);
    }

    [AutowireService]
    public class BylineMaker : IBylineMaker
    {
        private readonly IDependencies _dependencies;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            ITextTranslator TextTranslator { get; }
            IAuthorService AuthorClient { get; }
        }

        public BylineMaker(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public string MakeByline(IEnumerable<I___Person> authors)
        {
            if (authors == null) { return null; }

            var by = $"{_dependencies.TextTranslator.Translate("Article.By")} ";

            var names = new List<string>();
            foreach (var eachAuthor in authors)
            {
                if (eachAuthor.Inactive)
                {
                    names.Add($"{eachAuthor.First_Name} {eachAuthor.Last_Name}");
                }
                else
                {
                    names.Add($"<a href='{_dependencies.AuthorClient.ConvertUrlNameToLink(_dependencies.AuthorClient.GetUrlName(eachAuthor._Id))}'>{eachAuthor.First_Name} {eachAuthor.Last_Name}</a>".Trim());
                }
            }

            var result = $"{by}{names.JoinWithFinal(", ", "and")}";

            return result == by ? string.Empty : result;
        }

        public string MakePrintByLine(IEnumerable<I___Person> authors)
        {
            if (authors == null) { return string.Empty; }

            var formattedStrings =
                authors.Select(a => $"{a.First_Name} {a.Last_Name} {a.Email_Address}");

            return string.Join(" ", formattedStrings);
        }
    }
}