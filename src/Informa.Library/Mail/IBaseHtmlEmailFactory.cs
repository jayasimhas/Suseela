using System.Collections.Generic;

namespace Informa.Library.Mail
{
	public interface IBaseHtmlEmailFactory
	{
		IEmail Create();
        IEmail Create(Dictionary<string, string> replacements);
        string GetValue(string value, string defaultValue = null);
        string GetMediaURL(string mediaId);
    }
}
