using Autofac;
using Informa.Library.Publishing.Scheduled;
using Informa.Library.Publishing.Scheduled.History;

namespace Informa.Web.App_Start.Registrations
{
	public class ScheduledPublishingRegistrar
	{
		public static void RegisterDependencies(ContainerBuilder builder)
		{
			builder.RegisterType<Library.Publishing.Scheduled.MongoDB.ScheduledPublishDocumentFactory>().As<Library.Publishing.Scheduled.MongoDB.IScheduledPublishDocumentFactory>();
			builder.RegisterType<Library.Publishing.Scheduled.MongoDB.MongoDbScheduledPublishConfiguration>().As<Library.Publishing.Scheduled.MongoDB.IMongoDbScheduledPublishConfiguration>();
			builder.RegisterType<Library.Publishing.Scheduled.MongoDB.MongoDbScheduledPublishContext>().As<Library.Publishing.Scheduled.MongoDB.IMongoDbScheduledPublishContext>();
			builder.RegisterType<Library.Publishing.Scheduled.MongoDB.MongoDbInsertScheduledPublish>().As<Library.Publishing.Scheduled.MongoDB.IMongoDbInsertScheduledPublish>();
			builder.RegisterType<Library.Publishing.Scheduled.MongoDB.MongoDbFindOneScheduledPublish>().As<Library.Publishing.Scheduled.MongoDB.IMongoDbFindOneScheduledPublish>();
			builder.RegisterType<Library.Publishing.Scheduled.MongoDB.MongoDbUpsertScheduledPublish>().As<IUpsertScheduledPublish>();
			builder.RegisterType<Library.Publishing.Scheduled.MongoDB.MongoDbFindScheduledPublishes>().As<IFindScheduledPublishes>();
			builder.RegisterType<Library.Publishing.Scheduled.MongoDB.MongoDbDeleteVersionScheduledPublishes>().As<IDeleteVersionScheduledPublishes>();
			builder.RegisterType<Library.Publishing.Scheduled.MongoDB.MongoDbDeleteScheduledPublish>().As<IDeleteScheduledPublish>();
			builder.RegisterType<Library.Publishing.Scheduled.MongoDB.MongoDbDeleteItemScheduledPublishes>().As<IDeleteItemScheduledPublishes>();
			builder.RegisterType<Library.Publishing.Scheduled.MongoDB.MongoDbAllScheduledPublishes>().As<IAllScheduledPublishes>();

			//builder.RegisterType<Library.Publishing.Scheduled.Entity.EntityScheduledPublishContextFactory>().As<Library.Publishing.Scheduled.Entity.IEntityScheduledPublishContextFactory>();
			//builder.RegisterType<Library.Publishing.Scheduled.Entity.EntityScheduledPublishFactory>().As<Library.Publishing.Scheduled.Entity.IEntityScheduledPublishFactory>();
			//builder.RegisterType<Library.Publishing.Scheduled.Entity.EntityUpsertScheduledPublish>().As<IUpsertScheduledPublish>();
			//builder.RegisterType<Library.Publishing.Scheduled.Entity.EntityFindScheduledPublishes>().As<IFindScheduledPublishes>();
			//builder.RegisterType<Library.Publishing.Scheduled.Entity.EntityDeleteVersionScheduledPublishes>().As<IDeleteVersionScheduledPublishes>();
			//builder.RegisterType<Library.Publishing.Scheduled.Entity.EntityDeleteScheduledPublish>().As<IDeleteScheduledPublish>();
			//builder.RegisterType<Library.Publishing.Scheduled.Entity.EntityDeleteItemScheduledPublishes>().As<IDeleteItemScheduledPublishes>();
			//builder.RegisterType<Library.Publishing.Scheduled.Entity.EntityAllScheduledPublishes>().As<IAllScheduledPublishes>();

			//builder.RegisterType<AddHistoryProcessedScheduledPublishAction>().As<IAddHistoryProcessedScheduledPublishAction>();
			//builder.RegisterType<Library.Publishing.Scheduled.History.Entity.EntityScheduledPublishHistoryContextFactory>().As<Library.Publishing.Scheduled.History.Entity.IEntityScheduledPublishHistoryContextFactory>();
			//builder.RegisterType<Library.Publishing.Scheduled.History.Entity.EntityScheduledPublishFactory>().As<Library.Publishing.Scheduled.History.Entity.IEntityScheduledPublishFactory>();
			//builder.RegisterType<Library.Publishing.Scheduled.History.Entity.EntityAddScheduledPublishHistory>().As<IAddScheduledPublishHistory>();
			//builder.RegisterType<Library.Publishing.Scheduled.History.Entity.EntityFindScheduledPublishHistories>()
			//	.As<IFindItemScheduledPublishHistories>()
			//	.As<IFindItemLanguageScheduledPublishHistories>()
			//	.As<IFindItemVersionScheduledPublishHistories>();
		}
	}
}