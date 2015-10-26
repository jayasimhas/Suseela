using Autofac;
using Jabberwocky.Glass.Autofac.Util;
using Jabberwocky.Glass.Factory;
using Jabberwocky.Glass.Models;

namespace Informa.Library.Services.Search.Fields.Base
{
	public abstract class BaseInterfaceFactoryComputedField<T> : BaseGlassComputedField<IGlassBase> where T : class
	{
		private readonly IGlassInterfaceFactory _interfaceFactory;

		public override string FieldName { get; set; }
		public override string ReturnType { get; set; }

		protected BaseInterfaceFactoryComputedField(IGlassInterfaceFactory interfaceFactory = null)
		{
			_interfaceFactory = interfaceFactory ?? AutofacConfig.ServiceLocator.Resolve<IGlassInterfaceFactory>();
		}

		public override object GetFieldValue(IGlassBase glassItem)
		{
			var model = _interfaceFactory.GetItem<T>(glassItem);

			return model == null
				? null
				: GetFieldValue(model);
		}

		public abstract object GetFieldValue(T model);
	}
}
