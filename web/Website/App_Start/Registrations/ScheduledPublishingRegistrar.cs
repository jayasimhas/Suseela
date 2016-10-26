using Autofac;
using Informa.Library.Publishing.Scheduled;
using Informa.Library.Publishing.Scheduled.History;
using Informa.Library.Publishing.Scheduled.History.Entity;

namespace Informa.Web.App_Start.Registrations
{
	public class ScheduledPublishingRegistrar
	{
		public static void RegisterDependencies(ContainerBuilder builder)
		{
            builder.RegisterType<Library.Publishing.Scheduled.Entity.EntityScheduledPublishContextFactory>().As<Library.Publishing.Scheduled.Entity.IEntityScheduledPublishContextFactory>();
            builder.RegisterType<Library.Publishing.Scheduled.Entity.EntityScheduledPublishFactory>().As<Library.Publishing.Scheduled.Entity.IEntityScheduledPublishFactory>();
            builder.RegisterType<Library.Publishing.Scheduled.Entity.EntityUpsertScheduledPublish>().As<IUpsertScheduledPublish>();
            builder.RegisterType<Library.Publishing.Scheduled.Entity.EntityFindScheduledPublishes>().As<IFindScheduledPublishes>();
            builder.RegisterType<Library.Publishing.Scheduled.Entity.EntityDeleteVersionScheduledPublishes>().As<IDeleteVersionScheduledPublishes>();
            builder.RegisterType<Library.Publishing.Scheduled.Entity.EntityDeleteScheduledPublish>().As<IDeleteScheduledPublish>();
            builder.RegisterType<Library.Publishing.Scheduled.Entity.EntityDeleteItemScheduledPublishes>().As<IDeleteItemScheduledPublishes>();
            builder.RegisterType<Library.Publishing.Scheduled.Entity.EntityAllScheduledPublishes>().As<IAllScheduledPublishes>();

			builder.RegisterType<AddHistoryProcessedScheduledPublishAction>()
				.As<IAddHistoryProcessedScheduledPublishAction>()
				.As<IProcessedScheduledPublishAction>();
            builder.RegisterType<EntityScheduledPublishHistoryContextFactory>().As<IEntityScheduledPublishHistoryContextFactory>();
            builder.RegisterType<EntityScheduledPublishFactory>().As<IEntityScheduledPublishFactory>();
            builder.RegisterType<EntityAddScheduledPublishHistory>().As<IAddScheduledPublishHistory>();
            builder.RegisterType<EntityFindScheduledPublishHistories>()
                .As<IFindItemScheduledPublishHistories>()
                .As<IFindItemLanguageScheduledPublishHistories>()
                .As<IFindItemVersionScheduledPublishHistories>();
        }
	}
}