using System.Collections.Generic;
using InformaSitecoreWord.Config;

namespace InformaSitecoreWord
{
    internal class Constants
    {
		//Comment to change the DLL
        public static string PAN_JP = "PAN_JP";
        public static string PAN_CP = "PAN_CP";
        public static string PAN_WP = "PAN_WP";
        public static string PAN_US = "PAN_US";
        public static string PINK_DAILY = "PinkDaily";
        public static string SILVER = "Silver";
        public static string GOLD = "Gold";
        public static string PAM = "PAM";
        public static string TAN = "Tan";
        public static string ROSE = "Rose";
        public static string GRAY = "Gray";
        public static string PINK = "Pink";
        public static string THE_SILVER_SHEET = "The Silver Sheet";
        public static string THE_PINKSHEET_DAILY = "The Pinksheet Daily";
        public static string PHARMASIA_NEWS = "Pharmasia News";
        public static string US_STORY = "US Story";
        public static string WORLD_PRESS = "World Press";
        public static string CHINESE_PRESS = "Chinese Press";
        public static string JAPANESE_PRESS = "Japanese Press";
        public static string THE_GOLD_SHEET = "The Gold Sheet";
        public static string PHARMACEUTICAL_MONTHLY_APPROVALS = "Pharmaceutical Monthly Approvals";
        public static string THE_TAN_SHEET = "The Tan Sheet";
        public static string THE_ROSE_SHEET = "The Rose Sheet";
        public static string THE_GRAY_SHEET = "The Gray Sheet";
        public static string THE_PINK_SHEET = "The Pink Sheet";
        public static string ARTICLE_NUMBER = "Article Number";
        public static string TOP_STORY = "Top Story";
        public static string PARENT_ARTICLE = "Parent Article";
        public static string PUBLICATION_DATE = "Publication Date";
        public static string ARTICLE_TYPE = "Article Type";
        public static string PUBLICATION = "Publication";
        public static string SIDEBAR = "Sidebar";
        public static string EDITOR_NOTES = "Editor Notes";
        public static string VOLUME = "Volume";
        public static string ISSUE = "Issue";
        public static string TITLE_STYLE = "titleStyle";
        public static string HEALTHNEWS_DAILY = "HealthNews Daily";

        public static string DocumentPassword = "DocumentPassword";
        public static string ArticleID = "ArticleID";
		public static string PluginName = "PluginName";
		public static string InformaPluginName = "Informa Insight";
		public static string ArticleNumber = "ArticleNumber";
        public static string Editor = "Editor";
        public static string HYPERLINK_STYLE = "hyperlinkStyle";
        public static string Entities = "Entities";
        public static string Industries = "Industries";
        public static string TableauPrefix = "[T#:";
        public static string TableauTokenFormat = "[T#{0}:{1}]";
        public static string TableauTokenRegex = @"\[C#(.*?)\]";

        public static string PublicationGuid = "PublicationGuid";
        public static string VerticalGuid = "VerticalGuid";
        public static string TAXONOMY_GUID = "{E8A37C2D-FFE3-42D4-B38E-164584743832}";
        public static string INDUSTRY_GUID = "{2DF7E486-3062-4CA7-8ED3-AC19FAE466AE}";
        public static string SUBJECT_GUID = "{A43ABF01-C3F5-4ECD-8590-81686F3130DB}";
        public static string REGION_GUID = "{7233A087-0966-4336-85BD-9139B348B822}";
        public static string THERAPEUTIC_CATEGORY_GUID = "{9CB108F7-2F2C-49F1-8C52-A0F3068510BD}";
        public static string MARKET_SEGMENT_GUID = "{6141F0C9-975B-4655-8285-820A1B4E7F93}";

        public static string WordVersionNumber = "WordVersionNumber";
        public static string TitleMaxCharacters = "TitleMaxCharacters";
        public static readonly string PluginVersionNumber = "PluginVersionNumber";

        public static readonly string ConnectionUnavailable = "Sitecore server cannot be contacted.";
        public static readonly string LockedOutUser = "You have made too many login attempts. Please contact support at: {0}.";
        public static readonly string FailedLogin = "Your login attempt was not successful. Please try again.";
        public static readonly string InvalidPassword = "Your login attempt was not successful. Please try again. You have {0} more login attempts remaining before you are locked out.";
        public static string EDITOR_ENVIRONMENT_VALUE = "Production";
        public static string EDITOR_ENVIRONMENT_SERVERURL = ApplicationConfig.GetPropertyValue(EDITOR_ENVIRONMENT_VALUE);
        public static string EDITOR_ENVIRONMENT_LOGINURL = ApplicationConfig.GetPropertyValue(EDITOR_ENVIRONMENT_VALUE);
        public static string EDITOR_ENVIRONMENT_FORGOTPASSWORDLINK = ApplicationConfig.GetPropertyValue(EDITOR_ENVIRONMENT_VALUE + "ForgotPasswordLink");
        public static string DOCUMENT_NOT_LINKED = @"Document Not Linked";
        public static string MESSAGEBOX_TITLE = "Informa";
        public static string SESSIONTIMEOUTERRORMESSAGE = "Your session has timed out, please login again in order to continue";

        public static List<EditorEnvironment> EDITOR_ENVIRONMENT = new List<EditorEnvironment> {
            new EditorEnvironment{Name="Development" },
            new EditorEnvironment{Name="QA" },
            new EditorEnvironment{Name="INT" },
            ////new EditorEnvironment{Name="Stage" },
            ////new EditorEnvironment{Name="Production" },
        };
    }
    public class EditorEnvironment
    {
        public string Name { get; set; }
    }
}
