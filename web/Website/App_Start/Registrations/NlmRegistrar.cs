using System;
using Autofac;
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
        }
    }
}