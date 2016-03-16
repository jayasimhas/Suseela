using System.ComponentModel;

namespace Informa.Library.Newsletter
{
	public enum NewsletterType
	{
        [Description("SCRIP Intelligence")]
		Scrip
	}

    public static class EnumExtensions
    {
        public static string ToDescriptionString(this NewsletterType val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
