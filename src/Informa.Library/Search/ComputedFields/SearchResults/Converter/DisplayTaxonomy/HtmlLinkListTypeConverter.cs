using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.Script.Serialization;

namespace Informa.Library.Search.ComputedFields.SearchResults.Converter.DisplayTaxonomy
{
	public class HtmlLinkListTypeConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(HtmlLinkList))
			{
				return true;
			}

			return base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return true;
			}

			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			var stringValue = value as string;
			if (string.IsNullOrEmpty(stringValue))
			{
				return new HtmlLinkList();
			}

			return new JavaScriptSerializer().Deserialize<HtmlLinkList>(stringValue);

		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return new JavaScriptSerializer().Serialize(value);
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}