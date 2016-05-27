using System.Collections.Generic;
using System.Linq;
using Informa.Library.Globalization;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Utilities.StringUtils
{
	public interface IBylineMaker
	{
		string MakeByline(IEnumerable<I___Person> authors);
	}

	[AutowireService]
	public class BylineMaker : IBylineMaker
	{
		private readonly IDependencies _dependencies;

		[AutowireService(IsAggregateService = true)]
		public interface IDependencies
		{
			ITextTranslator TextTranslator { get; }
		}

		public BylineMaker(IDependencies dependencies)
		{
			_dependencies = dependencies;
		}

		public string MakeByline(IEnumerable<I___Person> authors)
		{
			if (authors == null) { return null; }

			var by = $"{_dependencies.TextTranslator.Translate("Article.By")} ";

			var names = authors.Select(auth => $"{auth.First_Name} {auth.Last_Name}".Trim());
			var result = $"{by}{names.JoinWithFinal(", ", "and")}";
			
			return result == by ? string.Empty : result;
		}
	}
}