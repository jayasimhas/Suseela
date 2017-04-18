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
  600, //width
  650, //height
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
  700, //width
  400, //height
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
  700, //width
  400, //height
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

Telerik.Web.UI.Editor.CommandList["ApplyStyles"] = function (commandName, editor, args) {
	var html = editor.getSelectionHtml();
	var selectedtag=editor.getSelectedElement(); 
	var val=args.value;
	scEditor = editor;
	if(val=='')
	{
		ApplyStyleWithoutClass(val);
	}	
	else
	StoryTitle(editor,val,html,selectedtag);
    
		   /* var tool = editor.getToolByName("FormatBlock");
			if (tool) {
				editor.pasteHtml("<div class="">"+html+"</div>");
				 // args.set_cancel(true);
			} */
	var Items = args._tool._items,
		Heading = "";
	for(var i = 0; i < Items.length; i++) {
		if(Items[i][0] == val) {
			Heading = Items[i][1];
			break;
		}
	};
	args._tool._element.childElements()[0].style.width = "165px";
	args._tool._element.childElements()[0].innerText = Heading;
};

function scRTEManager(sender, returnValue) {
    if (!returnValue) {
        return;
    }			
	scEditor.pasteHtml('<br/>');
	scEditor.pasteHtml(unescape(returnValue.Text), "DocumentManager");
}