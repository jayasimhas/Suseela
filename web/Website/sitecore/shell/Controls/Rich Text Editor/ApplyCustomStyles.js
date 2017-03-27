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
			  ["quick-facts__source", "p"],
			  ["quick-facts", "p"],
			  ["article-exhibit__header", "p"],
			  ["article-exhibit__title", "p"],
			  ["article-exhibit__caption", "p"],  
			  ["article-exhibit__source", "p"],
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
			else{
				Elem = '<p>' + selectedtext + '</p>';
			}
			
			if(data.tagName!="BODY")
			{
				var data1=data.parentNode;
				if(data1.tagName!="BODY")
				{
					data1.remove();
				}
				data.remove();
			}
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
		contentElement.setAttribute('class',block[0]);
		contentElement.innerHTML=selectedtext;
		 var qfdiv=document.createElement("div");
		 qfdiv.setAttribute('class','quick-facts');
		 qfdiv.append(contentElement);
		 if(selectedtag.nodeName!='BODY'){selectedtag.remove();}
		  editor.pasteHtml(qfdiv.outerHTML);
	}
	  else if(value=="quick-facts")
		{			
			contentElement.innerHTML=selectedtext;
		 var qfdiv=document.createElement("div");
		 qfdiv.setAttribute('class','quick-facts');
		 qfdiv.append(contentElement);
		 if(selectedtag.nodeName!='BODY'){selectedtag.remove();}
		  editor.pasteHtml(qfdiv.outerHTML);
		}
		else if(value=="quick-facts__list--ul" || value=="quick-facts__list--ol")
		{
			 var qfdiv=document.createElement("div");
		 qfdiv.setAttribute('class','quick-facts');
			 contentElement.setAttribute('class',block[0]);
		  var liTag=document.createElement("li");
		  liTag.innerHTML=selectedtext;
		  contentElement.append(liTag);
		  qfdiv.append(contentElement);
		  editor.pasteHtml(qfdiv.outerHTML);
		}		
		else if(value=="article-exhibit__header" || value=="article-exhibit__title" || value=="article-exhibit__caption"|| value=="article-exhibit__source")
		{
			var sec=document.createElement("section");
			sec.setAttribute('class','article-exhibit');
			 contentElement.setAttribute('class',block[0]);	
			 contentElement.innerHTML=selectedtext;
			 sec.append(contentElement);
			 if(selectedtag.nodeName!='BODY'){selectedtag.remove();}
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
