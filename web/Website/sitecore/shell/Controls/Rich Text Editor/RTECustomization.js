Telerik.Web.UI.Editor.CommandList["InsertMultimedia"] = function (commandName, editor, args) {

    var html = editor.getSelectionHtml();
    scEditor = editor;
	
	var val=args.value;
	if(val=="Iframe")
	{
    editor.showExternalDialog(
  "/sitecore/shell/default.aspx?xmlcontrol=RichText.InsertIframe&la=" + scLanguage + "&selectedText=" + escape(html),
  null, //argument
  500, //width
  600, //height
  scRTEManager, //callback
  null, // callback args
  "Insert Youtube IFrame",
  true, //modal
  Telerik.Web.UI.WindowBehaviors.Close, // behaviors
  false, //showStatusBar
  false //showTitleBar
 );
	}
	if(val=="Tableau")
	{
		///sitecore/shell/default.aspx?xmlcontrol=RichText.InsertArticles&la=" + scLanguage + "&selectedText=" + escape(html)
		editor.showExternalDialog(		
  "/sitecore/shell/default.aspx?xmlcontrol=RichText.InsertTableau&la=" + scLanguage + "&contextItem=" + scItemID,
  null, //argument
  800, //width
  600, //height
  scRTEManager, //callback
  null, // callback args
  "Insert Youtube IFrame",
  true, //modal
  Telerik.Web.UI.WindowBehaviors.Close, // behaviors
  false, //showStatusBar
  false //showTitleBar
 );
	}
};

Telerik.Web.UI.Editor.CommandList["MediaClass"] = function (commandName, editor, args) {

var html = editor.getSelectionHtml();
    scEditor = editor;
    var val=args.value;
    if(val=="SideBarArticle")
    {
    editor.showExternalDialog(
  "/sitecore/shell/default.aspx?xmlcontrol=RichText.InsertSidebarArticles&la=" + scLanguage + "&selectedText=" + escape(html)+ "&articletype=sidebar&itemid="+scItemID,
  null, //argument
  500, //width
  600, //height
  scRTEManager, //callback
  null, // callback args
  "Insert Youtube IFrame",
  true, //modal
  Telerik.Web.UI.WindowBehaviors.Close, // behaviors
  false, //showStatusBar
 false //showTitleBar
);
}
 if(val=="Referenced")
 {
    ///sitecore/shell/default.aspx?xmlcontrol=RichText.InsertArticles&la=" + scLanguage + "&selectedText=" + escape(html)
    editor.showExternalDialog(                         
  "/sitecore/shell/default.aspx?xmlcontrol=RichText.InsertSidebarArticles&la=" + scLanguage + "&selectedText=" + escape(html)+"&articletype=referenced&itemid="+scItemID,
  null, //argument
  800, //width
  600, //height
  scRTEManager, //callback
  null, // callback args
  "Insert Youtube IFrame",
  true, //modal
  Telerik.Web.UI.WindowBehaviors.Close, // behaviors
  false, //showStatusBar
  false //showTitleBar
);
}
                
 if(val=="Iframe")
{
    editor.showExternalDialog(
  "/sitecore/shell/default.aspx?xmlcontrol=RichText.InsertIframe&la=" + scLanguage + "&selectedText=" + escape(html),
  null, //argument
  600, //width
  600, //height
  scRTEManager, //callback
  null, // callback args
  "Insert Youtube IFrame",
  true, //modal
  Telerik.Web.UI.WindowBehaviors.Close, // behaviors
  false, //showStatusBar
  false //showTitleBar
);
}
};

function scRTEManager(sender, returnValue) {
    if (!returnValue) {
        return;
    }			
	scEditor.pasteHtml('<br/>');
	scEditor.pasteHtml(unescape(returnValue.Text), "DocumentManager");
}