using Autofac;
using Glass.Mapper.Sc;
using Informa.Library.Search.Results;
using Velir.Search.Core.Managers;
using Velir.Search.Core.Page;

namespace Informa.Web.App_Start.Registrations
{
    public class SearchRegistrar
    {
        public static void RegisterDependencies(ContainerBuilder builder)
        {
            builder.Register(
                c =>
                    new SearchManager<InformaSearchResultItem>(
                        GetIndexName(c.Resolve<ISearchPageParser>(), c.Resolve<ISitecoreContext>()),
                        c.Resolve<ISitecoreContext>())).As<ISearchManager<InformaSearchResultItem>>();
        }

        private static string GetIndexName(ISearchPageParser parser, ISitecoreContext context)
        {
            if (parser?.ListingConfiguration != null)
            {
                return parser.ListingConfiguration.Index_Name;
            }

            return string.Format("informa_content_{0}_index", context.Database.Name);
        }
    }
}