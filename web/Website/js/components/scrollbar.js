if($('.scrollbar') && $('.scrollbar').length){
	$('.scrollbar').each(function() {
		var container = $(this).parents('.rolling-stream, .table-responsive').find('.scrollbar-container')[0],
			content = $(this).parents('.rolling-stream, .table-responsive').find('.content, .wrap-merge')[0],
			scroll = $(this).parents('.rolling-stream, .table-responsive').find('.scrollbar')[0];

		scroll.style.height = container.clientHeight * content.clientHeight / content.scrollHeight + "px";
		  	scroll.style.top = container.clientHeight * content.scrollTop / content.scrollHeight + "px";
		  	
		content.addEventListener('scroll', function(e) {
		  scroll.style.height = container.clientHeight * content.clientHeight / content.scrollHeight + "px";
		  scroll.style.top = container.clientHeight * content.scrollTop / content.scrollHeight + "px";
		});
		// var event = new Event('scroll');

		window.addEventListener('resize', function(e){
			scroll.style.height = container.clientHeight * content.clientHeight / content.scrollHeight + "px";
		  	scroll.style.top = container.clientHeight * content.scrollTop / content.scrollHeight + "px";
		});
		// content.dispatchEvent(event);

		scroll.addEventListener('mousedown', function(start){
		  start.preventDefault();
		  var y = scroll.offsetTop;
		  var onMove = function(end){
			var delta = end.pageY - start.pageY;
			scroll.style.top = Math.min(container.clientHeight - scroll.clientHeight, Math.max(0, y + delta)) + 'px';
			content.scrollTop = (content.scrollHeight * scroll.offsetTop / container.clientHeight);
		  };
		  document.addEventListener('mousemove', onMove);
		  document.addEventListener('mouseup', function(){
			document.removeEventListener('mousemove', onMove);
		  });
		});
	});
}

//Horizontal Scroll Bar
if($('#scrollbar-horizantal').length > 0) {
	var container = $('#scrollbar-horizantal')[0],
		content = $('.wrap-merge')[0],
		scroll = $('#scrollbar')[0];

	scroll.style.width = ( $('.merge-acquistion').width() - $('.wrap-merge').width() ) + 'px';

	$(window).on('resize', function() {
		scroll.style.width = ( $('.merge-acquistion').width() - $('.wrap-merge').width() ) + 'px';
	})
	$('.wrap-merge').on('scroll', function() {
		scroll.style.left = (($(this).find('.table').offset().left * -1) + 14) + 'px';
	});
}