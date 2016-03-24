using Informa.Library.Globalization;
using Jabberwocky.Glass.Autofac.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Subscription
{
	[AutowireService(LifetimeScope.Default)]
	public class SubscriptionProductKeyContext : ISubscriptionProductKeyContext
	{
		protected readonly ITextTranslator TextTranslator;

		public SubscriptionProductKeyContext(
			ITextTranslator textTranslator)
		{
			TextTranslator = textTranslator;
		}

		public string ProductKey => TextTranslator.Translate("Subscriptions.ProductTypeKey");
	}
}
