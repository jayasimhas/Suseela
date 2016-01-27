using System;
using System.Linq.Expressions;
using Sitecore;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Rules;
using Velir.Search.Core.Util;

namespace Velir.Search.Core.Rules.Conditions
{
	public class DateFieldCondition<T> : FieldOperatorCondition<T, string> where T : RuleContext
	{
		protected DateTime ParsedDate
		{
			get
			{
				return this.ParseDate(this.Value);
			}
		}

		protected override bool Execute(T p_RuleContext)
		{
			bool flag = false;
			DateTime dateTime = this.ParseDate(p_RuleContext.Item[this.FieldName]);
			DateTime parsedDate = this.ParsedDate;
			switch (this.OperatorId)
			{
				case "{066602E2-ED1D-44C2-A698-7ED27FD3A2CC}":
					flag = dateTime == parsedDate;
					break;
				case "{B88CD556-082E-4385-BB76-E4D1B565F290}":
					flag = dateTime > parsedDate;
					break;
				case "{814EF7D0-1639-44FD-AEEF-735B5AC14425}":
					flag = dateTime >= parsedDate;
					break;
				case "{E362A3A4-E230-4A40-A7C4-FC42767E908F}":
					flag = dateTime < parsedDate;
					break;
				case "{2E1FC840-5919-4C66-8182-A33A1039EDBF}":
					flag = dateTime <= parsedDate;
					break;
				case "{3627ED99-F454-4B83-841A-A0194F0FB8B4}":
					flag = dateTime != parsedDate;
					break;
			}
			return flag;
		}

		protected override Expression GetTypedFieldValueExpression(ConditionContext p_Context)
		{
			return Expression.Property(p_Context.ParameterExpression, typeof(SearchResultItem), SearchResultItemUtil.GetPropertyName<SearchResultItem>(FieldName));
		}

		protected override Expression GetTypedValueExpression(ConditionContext p_Context)
		{
			return (Expression) Expression.Constant((object) this.ParsedDate);
		}

		private DateTime ParseDate(string p_Value)
		{
			DateTime dateTime = DateTime.MinValue;
			if (!string.IsNullOrEmpty(p_Value))
				dateTime = !DateUtil.IsIsoDate(p_Value) ? DateTime.Parse(p_Value) : DateUtil.IsoDateToDateTime(p_Value);
			return dateTime;
		}
	}
}