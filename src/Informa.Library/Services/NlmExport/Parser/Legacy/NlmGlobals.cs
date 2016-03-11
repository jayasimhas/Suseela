using Informa.Library.Services.NlmExport.Parser.Legacy.List;

namespace Informa.Library.Services.NlmExport.Parser.Legacy
{
    public static class NlmGlobals
    {
        public static class Output
        {
            public static string ListNodeName
            {
                get { return "list"; }
            }

            public static string ListTypeAttributeName
            {
                get { return "list-type"; }
            }

            public static string ListIdAttributeName
            {
                get { return "id"; }
            }

            public static string ListIdAttributeDefaultValue
            {
                get { return "L"; }
            }

            public static string UnorderedListBulletLabel
            {
                get { return "&bull; "; }
            }

            public static string ListItemNodeName
            {
                get { return "list-item"; }
            }

            public static string LabelNodeName
            {
                get { return "label"; }
            }

            public static string ParagraphNodeName
            {
                get { return "p"; }
            }

            public static string ParagraphContentTypeAttributeName
            {
                get { return "content-type"; }
            }

            public static string OrderedListType
            {
                get { return "order"; }
            }

            public static string OrderListContentType
            {
                get
                {
                    return "2.2 Story Number-List";
                }
            }

            public static ListItemNodeType OrderListNodeType
            {
                get { return ListItemNodeType.Numbered; }
            }

            public static string SideboxOrderListContentType
            {
                get { return "2.5 Quick facts Number List"; }
            }

            public static string SideboxUnorderedListContentType
            {
                get { return "2.5 Quick facts Bullet List"; }
            }

            public static string UnorderedListType
            {
                get { return "bullet"; }
            }

            public static string UnorderedListContentType
            {
                get { return "2.2 Story Bullet-List"; }
            }

            public static ListItemNodeType UnorderedListNodeType
            {
                get { return ListItemNodeType.Bullet; }
            }

            public static string TableCellNodeName
            {
                get { return "entry"; }
            }

            public static string TableCellColSepAttributeName
            {
                get { return "colsep"; }
            }

            public static string TableCellVerticalAlignAttributeName
            {
                get { return "valign"; }
            }

            public static string TableCellVerticalAlignDefaultValue
            {
                get { return "true"; }
            }

            public static string TableCellAlignAttributeName
            {
                get { return "align"; }
            }

            public static string TableCellAlignDefaultValue
            {
                get { return "align"; }
            }

            public static string TableCellRowSepAttributeName
            {
                get { return "rowsep"; }
            }

            public static string TableCellDefaultStyleClass
            {
                get { return "Chart-Cell"; }
            }

            public static string TableCellHeaderStyleClass
            {
                get { return "Chart-Header"; }
            }

            public static string TableCellSubHeaderStyleClass
            {
                get { return "Chart-subhead"; }
            }

            public static string TableCellStyledContentNodeName
            {
                get { return "styled-content"; }
            }

            public static string TableCellStyledContentStyleAttributeName
            {
                get { return "style"; }
            }
        }

        public static class Input
        {
            public static string ListItemHtmlTag
            {
                get { return "li"; }
            }
        }
    }
}
