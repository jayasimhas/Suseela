function StoryTitle(editor,value,selectedtext,selectedtag)
{
	 var defaultBlockSets = [
              ["article-interview__question", "div"],
              ["bold", "strong"],
              ["article-paragraph", "p"],
              ["nomargin", "p"],
              ["article-interview__answer", "div"],
              ["article-pullquote", "blockquote"],
              ["article-list--ul", "ul"],
              ["article-list--ol", "ol"],
              ["quick-facts__header", "h3"],
			  ["quick-facts__list--ul", "ul"],
			  ["quick-facts__list--ol", "ol"],
			  ["quick-facts__source", "span"],
			  ["quick-facts", "p"],
			  ["article-exhibit__header", "span"],
			  ["article-exhibit__title", "span"],
			  ["article-exhibit__caption", "span"],  
			  ["article-exhibit__source", "span"],
			  ["companyname","p"],
			  ["header colored","td"],
			  ["tablesubhead","td"],
			  ["subheadalt","td"],
			  ["tablestorytextalt","td"],
			  ["storytitle_new","p"],
			  ["executivesummary","p"],
			  ["h2","h2"],
			  ["h3","h3"]];
			  
	  var block = defaultBlockSets.find(function (element) { return element[0] == value; })			
	  if(value=="clearstyle")
		{		 
		/* var oValue = editor.get_html(true); */ //get the HTML content	
        	var data=selectedtag.parentNode;
			var Elem = "";
			
			if(selectedtag.tagName == "UL" || selectedtag.tagName == "OL") {
				var children = selectedtag.childNodes;
				
				for(var i = 0; i < children.length; i++) {
					Elem += '<p>' + children[i].innerText + '</p>'
				}
				selectedtag.remove();
			} 
			else if(selectedtag.tagName=="LI")
			{
				selectedtag.remove();
				Elem = '<p>' + selectedtext + '</p>';
			}
			else{
				selectedtag.remove();
				Elem = '<p>' + selectedtext + '</p>';
			}
			
			/* if(data.tagName!="BODY")
			{
				var data1=data.parentNode;
				if(data1.tagName!="BODY")
				{
					data1.remove();
				}
				data.remove();
			} */
		   //var contentElement = document.createElement('p');
		   //contentElement.innerHTML=selectedtext;
		   //editor.pasteHtml(contentElement.outerHTML);
		   
		   editor.pasteHtml(Elem);
	  }
 else if (block) {
	 var tag = block[1]
	  var contentElement = document.createElement(tag);	 
	  if(value=="h2" || value=="h3")
	  {
		  contentElement.innerHTML = selectedtext;
		  editor.pasteHtml(contentElement.outerHTML);
	  }
	  else if(value=="article-list--ul" || value=="article-list--ol")
	  {
		  contentElement.setAttribute('class',block[0]);
		  var liTag=document.createElement("li");
		  liTag.innerHTML=selectedtext;
		  contentElement.append(liTag);
		  editor.pasteHtml(contentElement.outerHTML);
	  }	
	else if(value=="quick-facts__header" || value=="quick-facts__source")
	{	
		var entirehtml=editor.get_html(true);
		var parsedHtml=jQuery.parseHTML(entirehtml);
		
		contentElement.setAttribute('class',block[0]);
		contentElement.innerHTML=selectedtext;
		if(entirehtml.indexOf('<div class="quick-facts">') == -1) {
		
		 var qfdiv=document.createElement("div");
		 qfdiv.setAttribute('class','quick-facts');
		 qfdiv.append(contentElement);
		 //if(selectedtag.nodeName!='BODY'){selectedtag.outerHTML.replace(selectedtext, '');}
		  editor.pasteHtml(qfdiv.outerHTML);
		} else {
			var UpdatedHTML = AppendToQuickFacts(parsedHtml, contentElement, selectedtag, entirehtml, selectedtext);			
			editor.set_html(UpdatedHTML);
		}
	}
	  else if(value=="quick-facts")
		{			
		var entirehtml=editor.get_html(true);
		var parsedHtml=jQuery.parseHTML(entirehtml);		
			contentElement.innerHTML=selectedtext;
			contentElement.setAttribute('class','testing');
			
		if(entirehtml.indexOf('<div class="quick-facts">') == -1) {
		 var qfdiv=document.createElement("div");
		 qfdiv.setAttribute('class','quick-facts');
		 qfdiv.append(contentElement);
		 if(selectedtag.nodeName!='BODY'){selectedtag.remove();}
		  editor.pasteHtml(qfdiv.outerHTML);
		}else{
			var UpdatedHTML = AppendToQuickFacts(parsedHtml, contentElement, selectedtag, entirehtml, selectedtext);			
			editor.set_html(UpdatedHTML);
		}
	}
		else if(value=="quick-facts__list--ul" || value=="quick-facts__list--ol")
		{
			var entirehtml=editor.get_html(true);
			var parsedHtml=jQuery.parseHTML(entirehtml);
			
			 var qfdiv=document.createElement("div");
		 qfdiv.setAttribute('class','quick-facts');
			 contentElement.setAttribute('class',block[0]);
			  var liTag=document.createElement("li");
			  liTag.innerHTML=selectedtext;
			  contentElement.append(liTag);
			  
			  if(entirehtml.indexOf('<div class="quick-facts">') == -1) {
			  qfdiv.append(contentElement);
			  editor.pasteHtml(qfdiv.outerHTML);
			} else {
				var UpdatedHTML = AppendToQuickFacts(parsedHtml, contentElement, selectedtag, entirehtml, selectedtext);
				editor.set_html(UpdatedHTML);
			}
		}		
		
		else if(value=="article-exhibit__header" || value=="article-exhibit__title" || value=="article-exhibit__caption"|| value=="article-exhibit__source")
		{
			var sec=document.createElement("section");
			sec.setAttribute('class','article-exhibit');
			 contentElement.setAttribute('class',block[0]);	
			 contentElement.innerHTML=selectedtext;
			 sec.append(contentElement);
			 //if(selectedtag.nodeName!='BODY'){selectedtag.remove();}
			 editor.pasteHtml(sec.outerHTML);
		}	
		else if(value=='header colored')
		{
			if(selectedtag.localName=="td"|| selectedtag.localName=="tr")
			{
		       selectedtag.setAttribute('class','header colored');		      
			}
			else
				alert('Please select only table cell');
		}
		else if(value=="tablesubhead")
		{
			selectedtag.setAttribute('class','highlight');					
		}
		else if(value=="subheadalt")
		{
			selectedtag.setAttribute('class','cell colored');			
		}
		else if(value=="tablestorytextalt")
		{
			selectedtag.setAttribute('class','cell coloredAlt');			
		}
	  else
	  {
	  contentElement.setAttribute('class',block[0]);	 
      contentElement.innerHTML = selectedtext;
	 editor.pasteHtml(contentElement.outerHTML);	
	  }
 }	 
 
}

function ApplyStyleWithoutClass(value)
{
	 var defaultBlockSets = [             
              ["bold", "strong"],
              ["<h2>", "h2"],
              ["<h3>", "h3"]];
	var block = defaultBlockSets.find(function (element) { return element[0] == value; })		
	 if (block) {
	 var tag = block[1];
	  var contentElement = document.createElement(tag);	   
      contentElement.innerHTML = selectedtext;
	 editor.pasteHtml(contentElement.outerHTML);
	 }
}
function AppendToQuickFacts(parsedHtml, contentElement, selectedtag, entirehtml, selectedtext) {
	var temp="";
	var Str ="";
	var OldValue = "";
	var ParsedNewElement = "";
	for(var i = 0; i < parsedHtml.length; i++) {
		if(parsedHtml[i].className == 'quick-facts') {	
			var old=parsedHtml[i].outerHTML;			
			parsedHtml[i].append(contentElement);
			
			ParsedNewElement = parsedHtml[i].outerHTML.replace(selectedtag.outerHTML, '');
			//var newhtml = parsedHtml[i].outerHTML.replace(selectedtag.outerHTML, '');
			//selectedtag.outerHTML.replace(selectedtext, '');
			temp = entirehtml.replace(selectedtag.outerHTML, '');
			OldValue = old.replace(selectedtag.outerHTML, '');
			Str = temp.replace(OldValue,ParsedNewElement);			
		}
	}
	return Str;
}