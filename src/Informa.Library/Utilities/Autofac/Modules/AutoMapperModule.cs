using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Features.OwnedInstances;
using AutoMapper;
using Jabberwocky.Glass.Autofac.Util;
using Module = Autofac.Module;

namespace Informa.Library.Utilities.Autofac.Modules
{
    public class AutomapperModule : Module
    {
        private readonly Assembly[] _assemblies;

        public AutomapperModule(params string[] assemblies)
            : this(LoadAssemblies(assemblies).ToArray())
        {
        }

        public AutomapperModule(params Assembly[] assemblies)
        {
            _assemblies = assemblies ?? new Assembly[0];
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Register custom AutoMapper Profiles
            builder.RegisterAssemblyTypes(_assemblies).AssignableTo<Profile>()
                .As<Profile>().AsSelf();

            // Register custom ValueResolvers
            builder.RegisterAssemblyTypes(_assemblies).AssignableTo<IValueResolver>()
                .AsSelf();

            // Register AutoMapper configuration
            builder.Register(c => new MapperConfiguration(config =>
            {
                foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                {
                    config.AddProfile(profile);
                }

                config.ConstructServicesUsing(t =>
                {
                    var owned = typeof(Owned<>).MakeGenericType(t);

                    return owned.GetProperty("Value").GetValue(AutofacConfig.ServiceLocator.Resolve(owned));
                });
            }))
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            // Register AutoMapper Mapper
            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>().SingleInstance();
        }

        private static IEnumerable<Assembly> LoadAssemblies(IEnumerable<string> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                Assembly asm = null;

                try
                {
                    asm = Assembly.Load(assembly);
                }
                catch
                {
                    // Suppress assembly loading errors
                }

                if (asm != null)
                {
                    yield return asm;
                }
            }
        }
    }
}
