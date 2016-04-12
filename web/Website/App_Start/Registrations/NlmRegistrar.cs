using System;
using Autofac;
using Informa.Library.Services.NlmExport;
using Informa.Library.Services.NlmExport.Logging;
using Informa.Library.Services.NlmExport.Validation.Config;
using Informa.Library.Utilities.References;

namespace Informa.Web.App_Start.Registrations
{
    public static class NlmRegistrar
    {
        public static void RegisterDependencies(ContainerBuilder builder)
        {
            // AppDomain should start in web/Website
            var dtdPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\" + Constants.DTDPath;

            builder.Register(c => new NlmValidationConfiguration(dtdPath)).AsSelf().SingleInstance();

            // Register logging decorator for NLM Export Service
            builder.RegisterType<NlmExportService>().Named<INlmExportService>("default");
            builder.RegisterType<NlmLoggingDecorator>().AsSelf();
            builder.RegisterDecorator<INlmExportService>((c, inner) => c.Resolve<NlmLoggingDecorator>(new TypedParameter(typeof(INlmExportService), inner)), "default").As<INlmExportService>();
        }
    }
}