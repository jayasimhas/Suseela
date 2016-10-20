$.fn.sortElements = (function(){
	var sort = [].sort;
	return function(comparator, getSortable) {
		getSortable = getSortable || function(){return this;};
		var placements = this.map(function(){
			var sortElement = getSortable.call(this),
				parentNode = sortElement.parentNode,
				nextSibling = parentNode.insertBefore(document.createTextNode(''), sortElement.nextSibling);
			return function() {
				if (parentNode === this) {
					throw new Error(
						"You can't sort elements if any one is a descendant of another."
					);
				}
				parentNode.insertBefore(this, nextSibling);
				parentNode.removeChild(nextSibling);
			};
		});
		return sort.call(this, comparator).each(function(i){
			placements[i].call(getSortable.call(this));
		});
	};
})();
	
$(function(){
	$('#mySubscriptionTab').tablesorter(); 
	$('#mypurchases').tablesorter(); 
	
	table=$("table")   
	$('#publicationtd, #subjecttd, #expDatetd').each(function(){
	  var th = $(this),
		  thIndex = th.index(),
		  inverse = false;
		  th.click(function() {
			  $(".parent").each(function(i,node){
				var child=$(this).nextUntil('.parent');
				$(this).data("child-node",child);   
				}); 
			
			if(th.attr('id') !== 'subjecttd'){
			  table.find('tr.parent td').filter(function(){
				  return $(this).index() === thIndex;
			  }).sortElements(function(a, b){
					  return $.text([a]) > $.text([b]) ?
						  inverse ? -1 : 1
						  : inverse ? 1 : -1;
				  }, function(){
					  return this.parentNode;
				  });
				  setTimeout( function() {
				  $(".parent").each(function(i,node){
					 var child=$(this).data("child-node");
					 $(this).after(child);
					});
				  inverse = !inverse;
				  }, 2);
				}
				else{
					var tchild = $('.child');
					for(var i=0; i<tchild.length; i++){
						var cls = $(tchild[i]).attr('class').split(' ')[1]; 
						$('.'+cls).find('td').filter(function(){
						  return $(this).index() === thIndex;
					  }).sortElements(function(a, b){
							  return $.text([a]) > $.text([b]) ?
								  inverse ? -1 : 1
								  : inverse ? 1 : -1;
						  }, function(){
							  return this.parentNode;
						  });
					} 
						  
					setTimeout( function() {
					  $(".child").each(function(i,node){
						 var child=$(this).data("child-node");
						 $(this).after(child);
						});
					  inverse = !inverse;
					  }, 2);
				}
				
			  
		  });
	  });

	/******** accordian script **********/
	$('.accordian', '.parent').click(function(){
		var $this = $(this), parentrow = $this.closest('.parent'), parentCls = parentrow.attr('class'), getrowCls = parentCls.replace(/parent /ig, '').replace('hidden-xs', '').replace('active', '').replace(/ /ig, ''), setchildCls = getrowCls.substr(1);
		if(!$this.hasClass('active')){
			parentrow.find('.accordian').addClass('active');
			$('.child.'+ setchildCls).addClass('hiderow');
		}
		else{
			parentrow.find('.accordian').removeClass('active');
			$('.child.'+ setchildCls).removeClass('hiderow');
		}
	});
	
	$('.maintitle').click(function(){
		var $this = $(this), tableMrow = $this.closest('.tableMrow'), subjectsPan = tableMrow.find('.subjects');
		if(!$this.hasClass('active')){
			subjectsPan.removeClass('hiderow');
			$this.addClass('active'); 
		} 
		else{
			subjectsPan.addClass('hiderow');
			$this.removeClass('active');
		}
	});
  });