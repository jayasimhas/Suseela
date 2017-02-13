var container = $('#scrollbar-container')[0],
    content = $('#content')[0],
    scroll = $('#scrollbar')[0];

content.addEventListener('scroll', function(e) {
  scroll.style.height = container.clientHeight * content.clientHeight / content.scrollHeight + "px";
  scroll.style.top = container.clientHeight * content.scrollTop / content.scrollHeight + "px";
});
var event = new Event('scroll');

window.addEventListener('resize', content.dispatchEvent.bind(content, event));
content.dispatchEvent(event);

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