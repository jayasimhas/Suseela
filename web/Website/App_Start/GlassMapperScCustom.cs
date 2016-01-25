using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;
using Glass.Mapper.IoC;
using Glass.Mapper.Maps;
using Glass.Mapper.Sc.IoC;
using Informa.Library.Site;
using Informa.Models;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using IDependencyResolver = Glass.Mapper.Sc.IoC.IDependencyResolver;

namespace Informa.Web.App_Start
{
    public static  class GlassMapperScCustom
    {
		public static IDependencyResolver CreateResolver(){
			var config = new Glass.Mapper.Sc.Config();

			var dependencyResolver = new DependencyResolver(config);
			// add any changes to the standard resolver here
			return dependencyResolver;
		}

		public static IConfigurationLoader[] GlassLoaders(){

            /* USE THIS AREA TO ADD FLUENT CONFIGURATION LOADERS
             * 
             * If you are using Attribute Configuration or automapping/on-demand mapping you don't need to do anything!
             * 
             */

            return new IConfigurationLoader[] { new AttributeConfigurationLoader("Informa.Models", "Jabberwocky.Glass", "Velir.Search.Models") };
        }
		public static void PostLoad(){
			//Remove the comments to activate CodeFist
			/* CODE FIRST START
            var dbs = Sitecore.Configuration.Factory.GetDatabases();
            foreach (var db in dbs)
            {
                var provider = db.GetDataProviders().FirstOrDefault(x => x is GlassDataProvider) as GlassDataProvider;
                if (provider != null)
                {
                    using (new SecurityDisabler())
                    {
                        provider.Initialise(db);
                    }
                }
            }
             * CODE FIRST END
             */
		}
		public static void AddMaps(IConfigFactory<IGlassMap> mapsConfigFactory)
        {
			// Add maps here
            //mapsConfigFactory.Add(() => new ListableConfig());
			//mapsConfigFactory.Add(() => new ArticleMap());
            //mapsConfigFactory.Add(() => new ScripConfig());
        }
    }
}
