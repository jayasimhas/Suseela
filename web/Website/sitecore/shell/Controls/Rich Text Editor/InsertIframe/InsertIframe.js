function scClose(text) {
    var returnValue = {
        Text: text
    };
  
    getRadWindow().close(returnValue);
}
  
function GetDialogArguments() {
    return getRadWindow().ClientParameters;
}
  
function getRadWindow() {
    if (window.radWindow) {
        return window.radWindow;
    }
  
    if (window.frameElement && window.frameElement.radWindow) {
        return window.frameElement.radWindow;
    }
  
    return null;
}
  
var isRadWindow = true;
  
var radWindow = getRadWindow();
  
if (radWindow) {
    if (window.dialogArguments) {
        radWindow.Window = window;
    }
}
  
function scCancel() {
  
    getRadWindow().close();
}
  
function scCloseWebEdit(embedTag) {
    window.returnValue = embedTag;
    window.close();
}
  
if (window.focus && Prototype.Browser.Gecko) {
    window.focus();
}

function scOnClickEdit(text) {
	var radWindow = getRadWindow();
	if (radWindow) {
    if (window.dialogArguments) {
        radWindow.Window = window;
    }
}
   $.ajax({url: "http://informa1/sitecore/shell/default.aspx?xmlcontrol=RichText.InsertImageCustom&la=en", type:params,success: function(response, textStatus, jqXHR){
	   var responseText = response.responseText || '[]';
        var json = responseText.evalJSON();
        if (json.success) {
            alert("json.sucess")
            onSuccessfulLogin(form.j_username.value);
		}
	    alert('edit called');
        $("#edit").val(text);
    }});
}

function textValue(text)
{		
	var d=document.getElementsByName('Window')[0];
	alert(d.getElementById('#edit'));
}



