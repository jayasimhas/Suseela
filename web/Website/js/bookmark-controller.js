function bookmarkController() {

    // * * *
    //  Article bookmarking logic goes here
    // * * *
    this.toggle = function(e) {

        var svg = $(e).find('use')[0];
        var currentIcon = svg.getAttributeNS('http://www.w3.org/1999/xlink', 'href').split('#')[1];

    	if(currentIcon === 'bookmark') {
    		svg.setAttributeNS('http://www.w3.org/1999/xlink', 'href', '/dist/img/svg-sprite.svg#bookmarked');
    	} else {
    		svg.setAttributeNS('http://www.w3.org/1999/xlink', 'href', '/dist/img/svg-sprite.svg#bookmark');
    	}
    }
};

export default bookmarkController;
