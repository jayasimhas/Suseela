function StoryTitle(editor,value,selectedtext,selectedtag)
{
	 var defaultBlockSets = [
              ["article-interview__question_new", "div"],
              ["bold", "strong"],
              ["article-paragraph_new", "p"],
              ["nomargin_new", "p"],
              ["article-interview__answer_new", "div"],
              ["article-pullquote_new", "blockquote"],
              ["article-list--ul_new", "ul"],
              ["article-list--ol_new", "ol"],
              ["quick-facts__header_new", "h3"],
			  ["quick-facts__list--ul_new", "ul"],
			  ["quick-facts__list--ol_new", "ol"],
			  ["quick-facts__source_new", "p"],
			  ["quick-facts", "p"],
			  ["article-exhibit__header_new", "p"],
			  ["article-exhibit__title_new", "p"],
			  ["article-exhibit__caption_new", "p"],  
			  ["article-exhibit__source_new", "p"],
			  ["companyname_new","p"],
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
	  else if(value=="article-list--ul_new" || value=="article-list--ol_new")
	  {
		  contentElement.setAttribute('class',block[0]);
		  var liTag=document.createElement("li");
		  liTag.innerHTML=selectedtext;
		  contentElement.append(liTag);
		  editor.pasteHtml(contentElement.outerHTML);
	  }	
	else if(value=="quick-facts__header_new" || value=="quick-facts__source_new")
	{	
		contentElement.setAttribute('class',block[0]);
		contentElement.innerHTML=selectedtext;
		 var qfdiv=document.createElement("div");
		 qfdiv.setAttribute('class','quick-facts_new');
		 qfdiv.append(contentElement);
		  editor.pasteHtml(qfdiv.outerHTML);
	}
	  else if(value=="quick-facts")
		{
			contentElement.innerHTML=selectedtext;
		 var qfdiv=document.createElement("div");
		 qfdiv.setAttribute('class','quick-facts_new');
		 qfdiv.append(contentElement);
		  editor.pasteHtml(qfdiv.outerHTML);
		}
		else if(value=="quick-facts__list--ul_new" || value=="quick-facts__list--ol_new")
		{
			 var qfdiv=document.createElement("div");
		 qfdiv.setAttribute('class','quick-facts_new');
			 contentElement.setAttribute('class',block[0]);
		  var liTag=document.createElement("li");
		  liTag.innerHTML=selectedtext;
		  contentElement.append(liTag);
		  qfdiv.append(contentElement);
		  editor.pasteHtml(qfdiv.outerHTML);
		}		
		else if(value=="article-exhibit__header_new" || value=="article-exhibit__title_new" || value=="article-exhibit__caption_new"|| value=="article-exhibit__source_new")
		{
			var sec=document.createElement("section");
			sec.setAttribute('class','article-exhibit');
			 contentElement.setAttribute('class',block[0]);	
			 contentElement.innerHTML=selectedtext;
			 sec.append(contentElement);
			 editor.pasteHtml(sec.outerHTML);
		}	
		else if(value=='header colored')
		{
			if(selectedtag.localName=="td"|| selectedtag.localName=="tr")
			{
		       selectedtag.setAttribute('class','headercolored_new');		      
			}
			else
				alert('Please select only table cell');
		}
		else if(value=="tablesubhead")
		{
			selectedtag.setAttribute('class','tablesubhead');					
		}
		else if(value=="subheadalt")
		{
			selectedtag.setAttribute('class','subheadalternate');			
		}
		else if(value=="tablestorytextalt")
		{
			selectedtag.setAttribute('class','tablestorytextalternate');			
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
