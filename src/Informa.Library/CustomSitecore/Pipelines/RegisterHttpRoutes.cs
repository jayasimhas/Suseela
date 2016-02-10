using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Jabberwocky.Glass.Autofac.Pipelines.Processors;
using Jabberwocky.WebApi.Handlers;
using Newtonsoft.Json.Serialization;
using Sitecore.Pipelines;

namespace Informa.Library.CustomSitecore.Pipelines
{
	public class RegisterHttpRoutes : ProcessorBase<PipelineArgs>
	{
		protected override void Run(PipelineArgs pipelineArgs)
		{
			// Register Web API routes & formatters

			var configuration = GlobalConfiguration.Configuration;
			var routes = configuration.Routes;

			routes.MapHttpRoute("defaultApi", "api/{controller}/{id}",
				new
			{
				id = RouteParameter.Optional
			});

            routes.MapHttpRoute(
                "articleNumberApi", 
                "SC{articleNumber}",
                new { controller = "Article", action ="Get", prefix = "SC" },
                new { articleNumber = @"\d+" }
            );

            routes.MapHttpRoute(
                "articlePathApi",
                "articles/{year}/{month}/{day}/{title}",
                new { controller = "Article", action = "Get" }
            );
            
            routes.MapHttpRoute(
                "articleEScenicApi", 
                "{title}-{escenicID}",
                new { controller = "Article", action = "Get" },
                new { escenicID = @"\d+" }
            );

            var jsonFormatter = new JsonMediaTypeFormatter
			{
				SerializerSettings = { ContractResolver = new CamelCasePropertyNamesContractResolver()},
			};
			jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
			configuration.Formatters.Insert(0, jsonFormatter);

			// Add gzip/deflate compression
			configuration.MessageHandlers.Add(new CompressionHandler());

			configuration.EnsureInitialized();
		}
	}
}
