using System;
using System.Linq.Expressions;
using Sitecore.Diagnostics;

namespace Velir.Search.Core.Rules
{
	public class ConditionContext
	{
		public ParameterExpression ParameterExpression { get; private set; }

		public Type ParameterType
		{
			get
			{
				return this.ParameterExpression.Type;
			}
		}

		public ConditionContext(ParameterExpression p_ParameterExpression)
		{
			Assert.ArgumentNotNull((object)p_ParameterExpression, "p_ParameterExpression");
			this.ParameterExpression = p_ParameterExpression;
		}
	}
}
