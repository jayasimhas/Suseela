using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Informa.Web.Areas.Account.Models;
using SitecoreTreeWalker.Sitecore;

namespace SitecoreTreeWalker.Util.CharacterStyles
{
	/// <summary>
	/// Fields are null until GetCharacterStyles() has been called to initialize values
	/// </summary>
	class CharacterStyleFactory
	{
		public static BoldStyle BoldStyle { get; private set; }
		public static HyperlinkStyle HyperlinkStyle { get; private set; }
		public static ItalicStyle ItalicStyle { get; private set; }
		public static StrikethroughStyle StrikethroughStyle { get; private set; }
		public static SubscriptStyle SubscriptStyle { get; private set; }
		public static SuperscriptStyle SuperscriptStyle { get; private set; }
		public static UnderlineStyle UnderlineStyle { get; private set; }
		private static List<CharacterStyle> CharacterStyles; 

		public static List<CharacterStyle> GetCharacterStyles()
		{
			if (CharacterStyles == null)
			{
				List<WordPluginModel.WordStyleStruct> styles = SitecoreGetter.GetCharacterStyles().ToList();
				var characterStyles = styles.ToDictionary(style => style.WordStyle, style => style.CssElement);
				string boldElement;
				string italicElement;
				string underlineElement;
				string strikethroughElement;
				string superscriptElement;
				string subscriptElement;
				characterStyles.TryGetValue("Bold", out boldElement);
				characterStyles.TryGetValue("Italic", out italicElement);
				characterStyles.TryGetValue("Underline", out underlineElement);
				characterStyles.TryGetValue("Strikethrough", out strikethroughElement);
				characterStyles.TryGetValue("Superscript", out superscriptElement);
				characterStyles.TryGetValue("Subscript", out subscriptElement);
				BoldStyle = new BoldStyle(boldElement);
				HyperlinkStyle = new HyperlinkStyle("a");
				ItalicStyle = new ItalicStyle(italicElement);
				StrikethroughStyle = new StrikethroughStyle(strikethroughElement);
				SubscriptStyle = new SubscriptStyle(subscriptElement);
				SuperscriptStyle = new SuperscriptStyle(superscriptElement);
				UnderlineStyle = new UnderlineStyle(underlineElement); 
				CharacterStyles = new List<CharacterStyle>
				                  	{
				                  		BoldStyle, ItalicStyle, StrikethroughStyle, 
										SubscriptStyle, SuperscriptStyle, UnderlineStyle
				                  	};
			}
			return CharacterStyles;
		} 
	}
}
