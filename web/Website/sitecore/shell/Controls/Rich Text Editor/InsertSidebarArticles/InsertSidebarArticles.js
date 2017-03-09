function scCloseAndUpdateReferencedArticles(text) {
    var returnValue = {
        Text: text
    };
    getRadWindow().close(returnValue);
		var selectedID=jQuery( "#SearchCombo option:selected" ).val();
	var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
	for (var i = 0; i < url.length; i++) {
var urlparam = url[i].split('=');
if (urlparam[0] == 'itemid') {
// alert(urlparam[1]);
}
}
	UpdateReferencedArticles(selectedID,urlparam[1]);
}

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

var AspectPreserver = Class.create({
  reload: function() {
    if ($("Width")) {
      $("Width").tainted = true;
    }
    this.retryCount = 0;
    this.hookEvents();
  },
  
  hookEvents: function() {
    if ((!$("Width") || $("Width").tainted) && this.retryCount < 10) {
      this.retryCount++;
      setTimeout(this.hookEvents.bind(this), 50);
      return;
    }
    else if (this.retryCount >= 10) {
      console.warn("retry limit exceeded, bailing out");
      return;
    }
    
    this._originalWidth = $F($("Width"));
    this._originalHeight = $F($("Height"));
    
    $("Width").observe("blur", this.onWidthChange.bind(this));
    $("Height").observe("blur", this.onHeightChange.bind(this));
  },
  
  onWidthChange: function() {
    var width = parseInt($F($("Width")));
    $("Height").value = Math.round(width / this._originalWidth * this._originalHeight);
  },
  
  onHeightChange: function() {
    var height = parseInt($F($("Height")));
    $("Width").value = Math.round(height / this._originalHeight * this._originalWidth);
  }
})

var scAspectPreserver = new AspectPreserver();

/* function getArticleNumberAndscClose() {
	var id=jQuery( "#SearchCombo option:selected" ).val();
	var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
	for (var i = 0; i < url.length; i++) {
var urlparam = url[i].split('=');
if (urlparam[0] == 'itemid') {
// alert(urlparam[1]);
}
}
	jQuery.ajax({
      type: "GET",
      url: "/api/SearchArticlesInRTE/test/SetReturnText", 
	  data: {'CurrentItemId' : id, 'ArticleType' :'sidebar', 'ItemId' : urlparam[1]},
      contentType: "application/json; charset=utf-8",
      dataType: 'json',
      success: function (a) {
		  alert(a);
	 var returnValue = {
         Text: a.d
     };
     // getRadWindow().close(returnValue);
	  },
	  error:function(a){
	  alert('error calling function');
	  }
	  });
} */


function UpdateReferencedArticles(selectedArticleID, currentItemId) {
	jQuery.ajax({
      type: "POST",
      url: "/api/SearchArticlesInRTE", 
	  data: JSON.stringify({'CurrentItemId':currentItemId, 'ItemId' : selectedArticleID}),
      contentType: "application/json; charset=utf-8",
      dataType: 'json',
      success: function (a) { 
		  
	alert('success');
	  },
	  error:function(a){
	  alert('error calling function');
	  }
	  });
}


/* function getArticleNumberForReferencedAndscClose() {
	var id=jQuery( "#SearchCombo option:selected" ).val();
	jQuery.ajax({
      type: "POST",
      url: "/Helpers/InsertArticlesInRTE.asmx/SetReturnText", 
	  data: "{'id' : '" + id + "','articleType' :'referenced'}",
      contentType: "application/json; charset=utf-8",
      dataType: 'json',
      success: function (a) {
	 var returnValue = {
         Text: a.d
     };
     getRadWindow().close(returnValue);
	  },
	  error:function(a){
	  alert('error calling function');
	  }
	  });
} */


	



