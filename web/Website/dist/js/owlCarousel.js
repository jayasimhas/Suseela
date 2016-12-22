(function e(t,n,r){function s(o,u){if(!n[o]){if(!t[o]){var a=typeof require=="function"&&require;if(!u&&a)return a(o,!0);if(i)return i(o,!0);var f=new Error("Cannot find module '"+o+"'");throw f.code="MODULE_NOT_FOUND",f}var l=n[o]={exports:{}};t[o][0].call(l.exports,function(e){var n=t[o][1][e];return s(n?n:e)},l,l.exports,e,t,n,r)}return n[o].exports}var i=typeof require=="function"&&require;for(var o=0;o<r.length;o++)s(r[o]);return s})({1:[function(require,module,exports){
/**
 * @name Owl Carousel - code name Phenix
 * @author Bartosz Wojciechowski
 * @release 2014
 * Licensed under MIT
 * 
 * @version 2.0.0-beta.1.8
 * @versionNotes Not compatibile with Owl Carousel <2.0.0
 */

/*

{0,0}
 )_)
 ""

To do:

* Lazy Load Icon
* prevent animationend bubling
* itemsScaleUp 
* Test Zepto

Callback events list:

onInitBefore
onInitAfter
onResponsiveBefore
onResponsiveAfter
onTransitionStart
onTransitionEnd
onTouchStart
onTouchEnd
onChangeState
onLazyLoaded
onVideoPlay
onVideoStop

Custom events list:

next.owl
prev.owl
goTo.owl
jumpTo.owl
addItem.owl
removeItem.owl
refresh.owl
play.owl
stop.owl
stopVideo.owl

*/

'use strict';

;(function ($, window, document, undefined) {

	var defaults = {
		items: 3,
		loop: false,
		center: false,

		mouseDrag: true,
		touchDrag: true,
		pullDrag: true,
		freeDrag: false,

		margin: 0,
		stagePadding: 0,

		merge: false,
		mergeFit: true,
		autoWidth: false,
		autoHeight: false,

		startPosition: 0,
		URLhashListener: false,

		nav: false,
		navRewind: true,
		navText: ['prev', 'next'],
		slideBy: 1,
		dots: true,
		dotsEach: false,
		dotData: false,

		lazyLoad: false,
		lazyContent: false,

		autoplay: false,
		autoplayTimeout: 5000,
		autoplayHoverPause: false,

		smartSpeed: 250,
		fluidSpeed: false,
		autoplaySpeed: false,
		navSpeed: false,
		dotsSpeed: false,
		dragEndSpeed: false,

		responsive: {},
		responsiveRefreshRate: 200,
		responsiveBaseElement: window,
		responsiveClass: false,

		video: false,
		videoHeight: false,
		videoWidth: false,

		animateOut: false,
		animateIn: false,

		fallbackEasing: 'swing',

		callbacks: false,
		info: false,

		nestedItemSelector: false,
		itemElement: 'div',
		stageElement: 'div',

		//Classes and Names
		themeClass: 'owl-theme',
		baseClass: 'owl-carousel',
		itemClass: 'owl-item',
		centerClass: 'center',
		activeClass: 'active',
		navContainerClass: 'owl-nav',
		navClass: ['owl-prev', 'owl-next'],
		controlsClass: 'owl-controls',
		dotClass: 'owl-dot',
		dotsClass: 'owl-dots',
		autoHeightClass: 'owl-height'

	};

	// Reference to DOM elements
	// Those with $ sign are jQuery objects

	var dom = {
		el: null, // main element
		$el: null, // jQuery main element
		stage: null, // stage
		$stage: null, // jQuery stage
		oStage: null, // outer stage
		$oStage: null, // $ outer stage
		$items: null, // all items, clones and originals included
		$oItems: null, // original items
		$cItems: null, // cloned items only
		$cc: null,
		$navPrev: null,
		$navNext: null,
		$page: null,
		$nav: null,
		$content: null
	};

	/**
  * Variables
  * @since 2.0.0
  */

	// Only for development process

	// Widths

	var width = {
		el: 0,
		stage: 0,
		item: 0,
		prevWindow: 0,
		cloneLast: 0
	};

	// Numbers

	var num = {
		items: 0,
		oItems: 0,
		cItems: 0,
		active: 0,
		merged: [],
		nav: [],
		allPages: 0
	};

	// Positions

	var pos = {
		start: 0,
		max: 0,
		maxValue: 0,
		prev: 0,
		current: 0,
		currentAbs: 0,
		currentPage: 0,
		stage: 0,
		items: [],
		lsCurrent: 0
	};

	// Drag/Touches

	var drag = {
		start: 0,
		startX: 0,
		startY: 0,
		current: 0,
		currentX: 0,
		currentY: 0,
		offsetX: 0,
		offsetY: 0,
		distance: null,
		startTime: 0,
		endTime: 0,
		updatedX: 0,
		targetEl: null
	};

	// Speeds

	var speed = {
		onDragEnd: 300,
		nav: 300,
		css2speed: 0

	};

	// States

	var state = {
		isTouch: false,
		isScrolling: false,
		isSwiping: false,
		direction: false,
		inMotion: false,
		autoplay: false,
		lazyContent: false
	};

	// Event functions references

	var e = {
		_onDragStart: null,
		_onDragMove: null,
		_onDragEnd: null,
		_transitionEnd: null,
		_resizer: null,
		_responsiveCall: null,
		_goToLoop: null,
		_checkVisibile: null,
		_autoplay: null,
		_pause: null,
		_play: null,
		_stop: null
	};

	function Owl(element, options) {

		// add basic Owl information to dom element

		element.owlCarousel = {
			'name': 'Owl Carousel',
			'author': 'Bartosz Wojciechowski',
			'version': '2.0.0-beta.1.8',
			'released': '03.05.2014'
		};

		// Attach variables to object
		// Only for development process

		this.options = $.extend({}, defaults, options);
		this._options = $.extend({}, defaults, options);
		this.dom = $.extend({}, dom);
		this.width = $.extend({}, width);
		this.num = $.extend({}, num);
		this.pos = $.extend({}, pos);
		this.drag = $.extend({}, drag);
		this.speed = $.extend({}, speed);
		this.state = $.extend({}, state);
		this.e = $.extend({}, e);

		this.dom.el = element;
		this.dom.$el = $(element);
		this.init();
	}

	/**
  * init
  * @since 2.0.0
  */

	Owl.prototype.init = function () {

		this.fireCallback('onInitBefore');

		//Add base class
		if (!this.dom.$el.hasClass(this.options.baseClass)) {
			this.dom.$el.addClass(this.options.baseClass);
		}

		//Add theme class
		if (!this.dom.$el.hasClass(this.options.themeClass)) {
			this.dom.$el.addClass(this.options.themeClass);
		}

		//Add theme class
		if (this.options.rtl) {
			this.dom.$el.addClass('owl-rtl');
		}

		// Check support
		this.browserSupport();

		// Sort responsive items in array
		this.sortOptions();

		// Update options.items on given size
		this.setResponsiveOptions();

		if (this.options.autoWidth && this.state.imagesLoaded !== true) {
			var imgs = this.dom.$el.find('img');
			if (imgs.length) {
				this.preloadAutoWidthImages(imgs);
				return false;
			}
		}

		// Get and store window width
		// iOS safari likes to trigger unnecessary resize event
		this.width.prevWindow = this.windowWidth();

		// create stage object
		this.createStage();

		// Append local content
		this.fetchContent();

		// attach generic events
		this.eventsCall();

		// attach custom control events
		this.addCustomEvents();

		// attach generic events
		this.internalEvents();

		this.dom.$el.addClass('owl-loading');
		this.refresh(true);
		this.dom.$el.removeClass('owl-loading').addClass('owl-loaded');
		this.fireCallback('onInitAfter');
	};

	/**
  * sortOptions
  * @desc Sort responsive sizes 
  * @since 2.0.0
  */

	Owl.prototype.sortOptions = function () {

		var resOpt = this.options.responsive;
		this.responsiveSorted = {};
		var keys = [],
		    i,
		    j,
		    k;
		for (i in resOpt) {
			keys.push(i);
		}

		keys = keys.sort(function (a, b) {
			return a - b;
		});

		for (j = 0; j < keys.length; j++) {
			k = keys[j];
			this.responsiveSorted[k] = resOpt[k];
		}
	};

	/**
  * setResponsiveOptions
  * @since 2.0.0
  */

	Owl.prototype.setResponsiveOptions = function () {
		if (this.options.responsive === false) {
			return false;
		}

		var width = this.windowWidth();
		var resOpt = this.options.responsive;
		var i, j, k, minWidth;

		// overwrite non resposnive options
		for (k in this._options) {
			if (k !== 'responsive') {
				this.options[k] = this._options[k];
			}
		}

		// find responsive width
		for (i in this.responsiveSorted) {
			if (i <= width) {
				minWidth = i;
				// set responsive options
				for (j in this.responsiveSorted[minWidth]) {
					this.options[j] = this.responsiveSorted[minWidth][j];
				}
			}
		}
		this.num.breakpoint = minWidth;

		// Responsive Class
		if (this.options.responsiveClass) {
			this.dom.$el.attr('class', function (i, c) {
				return c.replace(/\b owl-responsive-\S+/g, '');
			}).addClass('owl-responsive-' + minWidth);
		}
	};

	/**
  * optionsLogic
  * @desc Update option logic if necessery
  * @since 2.0.0
  */

	Owl.prototype.optionsLogic = function () {
		// Toggle Center class
		this.dom.$el.toggleClass('owl-center', this.options.center);

		// Scroll per - 'page' option will scroll per visible items number
		// You can set this to any other number below visible items.
		if (this.options.slideBy && this.options.slideBy === 'page') {
			this.options.slideBy = this.options.items;
		} else if (this.options.slideBy > this.options.items) {
			this.options.slideBy = this.options.items;
		}

		// if items number is less than in body
		if (this.options.loop && this.num.oItems < this.options.items) {
			this.options.loop = false;
		}

		if (this.num.oItems <= this.options.items) {
			this.options.navRewind = false;
		}

		if (this.options.autoWidth) {
			this.options.stagePadding = false;
			this.options.dotsEach = 1;
			this.options.merge = false;
		}
		if (this.state.lazyContent) {
			this.options.loop = false;
			this.options.merge = false;
			this.options.dots = false;
			this.options.freeDrag = false;
			this.options.lazyContent = true;
		}

		if ((this.options.animateIn || this.options.animateOut) && this.options.items === 1 && this.support3d) {
			this.state.animate = true;
		} else {
			this.state.animate = false;
		}
	};

	/**
  * createStage
  * @desc Create stage and Outer-stage elements
  * @since 2.0.0
  */

	Owl.prototype.createStage = function () {
		var oStage = document.createElement('div');
		var stage = document.createElement(this.options.stageElement);

		oStage.className = 'owl-stage-outer';
		stage.className = 'owl-stage';

		oStage.appendChild(stage);
		this.dom.el.appendChild(oStage);

		this.dom.oStage = oStage;
		this.dom.$oStage = $(oStage);
		this.dom.stage = stage;
		this.dom.$stage = $(stage);

		oStage = null;
		stage = null;
	};

	/**
  * createItem
  * @desc Create item container
  * @since 2.0.0
  */

	Owl.prototype.createItem = function () {
		var item = document.createElement(this.options.itemElement);
		item.className = this.options.itemClass;
		return item;
	};

	/**
  * fetchContent
  * @since 2.0.0
  */

	Owl.prototype.fetchContent = function (extContent) {
		if (extContent) {
			this.dom.$content = extContent instanceof jQuery ? extContent : $(extContent);
		} else if (this.options.nestedItemSelector) {
			this.dom.$content = this.dom.$el.find('.' + this.options.nestedItemSelector).not('.owl-stage-outer');
		} else {
			this.dom.$content = this.dom.$el.children().not('.owl-stage-outer');
		}
		// content length
		this.num.oItems = this.dom.$content.length;

		// init Structure
		if (this.num.oItems !== 0) {
			this.initStructure();
		}
	};

	/**
  * initStructure
  * @param [refresh] - if refresh and not lazyContent then dont create normal structure
  * @since 2.0.0
  */

	Owl.prototype.initStructure = function () {

		// lazyContent needs at least 3*items

		if (this.options.lazyContent && this.num.oItems >= this.options.items * 3) {
			this.state.lazyContent = true;
		} else {
			this.state.lazyContent = false;
		}

		if (this.state.lazyContent) {

			// start position
			this.pos.currentAbs = this.options.items;

			//remove lazy content from DOM
			this.dom.$content.remove();
		} else {
			// create normal structure
			this.createNormalStructure();
		}
	};

	/**
  * createNormalStructure
  * @desc Create normal structure for small/mid weight content
  * @since 2.0.0
  */

	Owl.prototype.createNormalStructure = function () {
		for (var i = 0; i < this.num.oItems; i++) {
			// fill 'owl-item' with content
			var item = this.fillItem(this.dom.$content, i);
			// append into stage
			this.dom.$stage.append(item);
		}
		this.dom.$content = null;
	};

	/**
  * createCustomStructure
  * @since 2.0.0
  */

	Owl.prototype.createCustomStructure = function (howManyItems) {
		for (var i = 0; i < howManyItems; i++) {
			var emptyItem = this.createItem();
			var item = $(emptyItem);

			this.setData(item, false);
			this.dom.$stage.append(item);
		}
	};

	/**
  * createLazyContentStructure
  * @desc Create lazyContent structure for large content and better mobile experience
  * @since 2.0.0
  */

	Owl.prototype.createLazyContentStructure = function (refresh) {
		if (!this.state.lazyContent) {
			return false;
		}

		// prevent recreate - to do
		if (refresh && this.dom.$stage.children().length === this.options.items * 3) {
			return false;
		}
		// remove items from stage
		this.dom.$stage.empty();

		// create custom structure
		this.createCustomStructure(3 * this.options.items);
	};

	/**
  * fillItem
  * @desc Fill empty item container with provided content
  * @since 2.0.0
  * @param [content] - string/$dom - passed owl-item
  * @param [i] - index in jquery object
  * return $ new object
  */

	Owl.prototype.fillItem = function (content, i) {
		var emptyItem = this.createItem();
		var c = content[i] || content;
		// set item data
		var traversed = this.traversContent(c);
		this.setData(emptyItem, false, traversed);
		return $(emptyItem).append(c);
	};

	/**
  * traversContent
  * @since 2.0.0
  * @param [c] - content
  * return object
  */

	Owl.prototype.traversContent = function (c) {
		var $c = $(c),
		    dotValue,
		    hashValue;
		if (this.options.dotData) {
			dotValue = $c.find('[data-dot]').andSelf().data('dot');
		}
		// update URL hash
		if (this.options.URLhashListener) {
			hashValue = $c.find('[data-hash]').andSelf().data('hash');
		}
		return {
			dot: dotValue || false,
			hash: hashValue || false
		};
	};

	/**
  * setData
  * @desc Set item jQuery Data 
  * @since 2.0.0
  * @param [item] - dom - passed owl-item
  * @param [cloneObj] - $dom - passed clone item
  */

	Owl.prototype.setData = function (item, cloneObj, traversed) {
		var dot, hash;
		if (traversed) {
			dot = traversed.dot;
			hash = traversed.hash;
		}
		var itemData = {
			index: false,
			indexAbs: false,
			posLeft: false,
			clone: false,
			active: false,
			loaded: false,
			lazyLoad: false,
			current: false,
			width: false,
			center: false,
			page: false,
			hasVideo: false,
			playVideo: false,
			dot: dot,
			hash: hash
		};

		// copy itemData to cloned item

		if (cloneObj) {
			itemData = $.extend({}, itemData, cloneObj.data('owl-item'));
		}

		$(item).data('owl-item', itemData);
	};

	/**
  * updateLocalContent
  * @since 2.0.0
  */

	Owl.prototype.updateLocalContent = function () {
		this.dom.$oItems = this.dom.$stage.find('.' + this.options.itemClass).filter(function () {
			return $(this).data('owl-item').clone === false;
		});

		this.num.oItems = this.dom.$oItems.length;
		//update index on original items

		for (var k = 0; k < this.num.oItems; k++) {
			var item = this.dom.$oItems.eq(k);
			item.data('owl-item').index = k;
		}
	};

	/**
  * checkVideoLinks
  * @desc Check if for any videos links
  * @since 2.0.0
  */

	Owl.prototype.checkVideoLinks = function () {
		if (!this.options.video) {
			return false;
		}
		var videoEl, item;

		for (var i = 0; i < this.num.items; i++) {

			item = this.dom.$items.eq(i);
			if (item.data('owl-item').hasVideo) {
				continue;
			}

			videoEl = item.find('.owl-video');
			if (videoEl.length) {
				this.state.hasVideos = true;
				this.dom.$items.eq(i).data('owl-item').hasVideo = true;
				videoEl.css('display', 'none');
				this.getVideoInfo(videoEl, item);
			}
		}
	};

	/**
  * getVideoInfo
  * @desc Get Video ID and Type (YouTube/Vimeo only)
  * @since 2.0.0
  */

	Owl.prototype.getVideoInfo = function (videoEl, item) {

		var info,
		    type,
		    id,
		    vimeoId = videoEl.data('vimeo-id'),
		    youTubeId = videoEl.data('youtube-id'),
		    width = videoEl.data('width') || this.options.videoWidth,
		    height = videoEl.data('height') || this.options.videoHeight,
		    url = videoEl.attr('href');

		if (vimeoId) {
			type = 'vimeo';
			id = vimeoId;
		} else if (youTubeId) {
			type = 'youtube';
			id = youTubeId;
		} else if (url) {
			id = url.match(/(http:|https:|)\/\/(player.|www.)?(vimeo\.com|youtu(be\.com|\.be|be\.googleapis\.com))\/(video\/|embed\/|watch\?v=|v\/)?([A-Za-z0-9._%-]*)(\&\S+)?/);

			if (id[3].indexOf('youtu') > -1) {
				type = 'youtube';
			} else if (id[3].indexOf('vimeo') > -1) {
				type = 'vimeo';
			}
			id = id[6];
		} else {
			throw new Error('Missing video link.');
		}

		item.data('owl-item').videoType = type;
		item.data('owl-item').videoId = id;
		item.data('owl-item').videoWidth = width;
		item.data('owl-item').videoHeight = height;

		info = {
			type: type,
			id: id
		};

		// Check dimensions
		var dimensions = width && height ? 'style="width:' + width + 'px;height:' + height + 'px;"' : '';

		// wrap video content into owl-video-wrapper div
		videoEl.wrap('<div class="owl-video-wrapper"' + dimensions + '></div>');

		this.createVideoTn(videoEl, info);
	};

	/**
  * createVideoTn
  * @desc Create Video Thumbnail
  * @since 2.0.0
  */

	Owl.prototype.createVideoTn = function (videoEl, info) {

		var tnLink, icon, height;
		var customTn = videoEl.find('img');
		var srcType = 'src';
		var lazyClass = '';
		var that = this;

		if (this.options.lazyLoad) {
			srcType = 'data-src';
			lazyClass = 'owl-lazy';
		}

		// Custom thumbnail

		if (customTn.length) {
			addThumbnail(customTn.attr(srcType));
			customTn.remove();
			return false;
		}

		function addThumbnail(tnPath) {
			icon = '<div class="owl-video-play-icon"></div>';

			if (that.options.lazyLoad) {
				tnLink = '<div class="owl-video-tn ' + lazyClass + '" ' + srcType + '="' + tnPath + '"></div>';
			} else {
				tnLink = '<div class="owl-video-tn" style="opacity:1;background-image:url(' + tnPath + ')"></div>';
			}
			videoEl.after(tnLink);
			videoEl.after(icon);
		}

		if (info.type === 'youtube') {
			var path = "http://img.youtube.com/vi/" + info.id + "/hqdefault.jpg";
			addThumbnail(path);
		} else if (info.type === 'vimeo') {
			$.ajax({
				type: 'GET',
				url: 'http://vimeo.com/api/v2/video/' + info.id + '.json',
				jsonp: 'callback',
				dataType: 'jsonp',
				success: function success(data) {
					var path = data[0].thumbnail_large;
					addThumbnail(path);
					if (that.options.loop) {
						that.updateItemState();
					}
				}
			});
		}
	};

	/**
  * stopVideo
  * @since 2.0.0
  */

	Owl.prototype.stopVideo = function () {
		this.fireCallback('onVideoStop');
		var item = this.dom.$items.eq(this.state.videoPlayIndex);
		item.find('.owl-video-frame').remove();
		item.removeClass('owl-video-playing');
		this.state.videoPlay = false;
	};

	/**
  * playVideo
  * @since 2.0.0
  */

	Owl.prototype.playVideo = function (ev) {
		this.fireCallback('onVideoPlay');

		if (this.state.videoPlay) {
			this.stopVideo();
		}
		var videoLink,
		    videoWrap,
		    target = $(ev.target || ev.srcElement),
		    item = target.closest('.' + this.options.itemClass);

		var videoType = item.data('owl-item').videoType,
		    id = item.data('owl-item').videoId,
		    width = item.data('owl-item').videoWidth || Math.floor(item.data('owl-item').width - this.options.margin),
		    height = item.data('owl-item').videoHeight || this.dom.$stage.height();

		if (videoType === 'youtube') {
			videoLink = "<iframe width=\"" + width + "\" height=\"" + height + "\" src=\"http://www.youtube.com/embed/" + id + "?autoplay=1&v=" + id + "\" frameborder=\"0\" allowfullscreen></iframe>";
		} else if (videoType === 'vimeo') {
			videoLink = '<iframe src="http://player.vimeo.com/video/' + id + '?autoplay=1" width="' + width + '" height="' + height + '" frameborder="0" webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>';
		}

		item.addClass('owl-video-playing');
		this.state.videoPlay = true;
		this.state.videoPlayIndex = item.data('owl-item').indexAbs;

		videoWrap = $('<div style="height:' + height + 'px; width:' + width + 'px" class="owl-video-frame">' + videoLink + '</div>');
		target.after(videoWrap);
	};

	/**
  * loopClone
  * @desc Make a clones for infinity loop
  * @since 2.0.0
  */

	Owl.prototype.loopClone = function () {
		if (!this.options.loop || this.state.lazyContent || this.num.oItems < this.options.items) {
			return false;
		}

		var firstClone,
		    lastClone,
		    i,
		    num = this.options.items,
		    lastNum = this.num.oItems - 1;

		// if neighbour margin then add one more duplicat
		if (this.options.stagePadding && this.options.items === 1) {
			num += 1;
		}
		this.num.cItems = num * 2;

		for (i = 0; i < num; i++) {
			// Clone item
			var first = this.dom.$oItems.eq(i).clone(true, true);
			var last = this.dom.$oItems.eq(lastNum - i).clone(true, true);
			firstClone = $(first[0]).addClass('cloned');
			lastClone = $(last[0]).addClass('cloned');

			// set clone data
			// Somehow data has reference to same data id in cash

			this.setData(firstClone[0], first);
			this.setData(lastClone[0], last);

			firstClone.data('owl-item').clone = true;
			lastClone.data('owl-item').clone = true;

			this.dom.$stage.append(firstClone);
			this.dom.$stage.prepend(lastClone);

			firstClone = lastClone = null;
		}

		this.dom.$cItems = this.dom.$stage.find('.' + this.options.itemClass).filter(function () {
			return $(this).data('owl-item').clone === true;
		});
	};

	/**
  * reClone
  * @desc Update Cloned elements
  * @since 2.0.0
  */

	Owl.prototype.reClone = function () {
		// remove cloned items
		if (this.dom.$cItems !== null) {
			// && (this.num.oItems !== 0 && this.num.oItems <= this.options.items)){
			this.dom.$cItems.remove();
			this.dom.$cItems = null;
			this.num.cItems = 0;
		}

		if (!this.options.loop) {
			return;
		}
		// generete new elements
		this.loopClone();
	};

	/**
  * calculate
  * @desc Update item index data
  * @since 2.0.0
  */

	Owl.prototype.calculate = function () {

		var i,
		    j,
		    k,
		    dist,
		    posLeft = 0,
		    fullWidth = 0;

		// element width minus neighbour
		this.width.el = this.dom.$el.width() - this.options.stagePadding * 2;

		//to check
		this.width.view = this.dom.$el.width();

		// calculate width minus addition margins
		var elMinusMargin = this.width.el - this.options.margin * (this.options.items === 1 ? 0 : this.options.items - 1);

		// calculate element width and item width
		this.width.el = this.width.el + this.options.margin;
		this.width.item = (elMinusMargin / this.options.items + this.options.margin).toFixed(3);

		this.dom.$items = this.dom.$stage.find('.owl-item');
		this.num.items = this.dom.$items.length;

		//change to autoWidths
		if (this.options.autoWidth) {
			this.dom.$items.css('width', '');
		}

		// Set grid array
		this.pos.items = [];
		this.num.merged = [];
		this.num.nav = [];

		// item distances
		if (this.options.rtl) {
			dist = this.options.center ? -(this.width.el / 2) : 0;
		} else {
			dist = this.options.center ? this.width.el / 2 : 0;
		}

		this.width.mergeStage = 0;

		// Calculate items positions
		for (i = 0; i < this.num.items; i++) {

			// check merged items

			if (this.options.merge) {
				var mergeNumber = this.dom.$items.eq(i).find('[data-merge]').attr('data-merge') || 1;
				if (this.options.mergeFit && mergeNumber > this.options.items) {
					mergeNumber = this.options.items;
				}
				this.num.merged.push(parseInt(mergeNumber));
				this.width.mergeStage += this.width.item * this.num.merged[i];
			} else {
				this.num.merged.push(1);
			}

			// Array based on merged items used by dots and navigation
			if (this.options.loop) {
				if (i >= this.num.cItems / 2 && i < this.num.cItems / 2 + this.num.oItems) {
					this.num.nav.push(this.num.merged[i]);
				}
			} else {
				this.num.nav.push(this.num.merged[i]);
			}

			var iWidth = this.width.item * this.num.merged[i];

			// autoWidth item size
			if (this.options.autoWidth) {
				iWidth = this.dom.$items.eq(i).width() + this.options.margin;
				if (this.options.rtl) {
					this.dom.$items[i].style.marginLeft = this.options.margin + 'px';
				} else {
					this.dom.$items[i].style.marginRight = this.options.margin + 'px';
				}
			}
			// push item position into array
			this.pos.items.push(dist);

			// update item data
			this.dom.$items.eq(i).data('owl-item').posLeft = posLeft;
			this.dom.$items.eq(i).data('owl-item').width = iWidth;

			// dist starts from middle of stage if center
			// posLeft always starts from 0
			if (this.options.rtl) {
				dist += iWidth;
				posLeft += iWidth;
			} else {
				dist -= iWidth;
				posLeft -= iWidth;
			}

			fullWidth -= Math.abs(iWidth);

			// update position if center
			if (this.options.center) {
				this.pos.items[i] = !this.options.rtl ? this.pos.items[i] - iWidth / 2 : this.pos.items[i] + iWidth / 2;
			}
		}

		if (this.options.autoWidth) {
			this.width.stage = this.options.center ? Math.abs(fullWidth) : Math.abs(dist);
		} else {
			this.width.stage = Math.abs(fullWidth);
		}

		//update indexAbs on all items
		var allItems = this.num.oItems + this.num.cItems;

		for (j = 0; j < allItems; j++) {
			this.dom.$items.eq(j).data('owl-item').indexAbs = j;
		}

		// Set Min and Max
		this.setMinMax();

		// Recalculate grid
		this.setSizes();
	};

	/**
  * setMinMax
  * @since 2.0.0
  */

	Owl.prototype.setMinMax = function () {

		// set Min
		var minimum = this.dom.$oItems.eq(0).data('owl-item').indexAbs;
		var i;
		this.pos.min = 0;
		this.pos.minValue = this.pos.items[minimum];

		// set max position
		if (!this.options.loop) {
			this.pos.max = this.num.oItems - 1;
		}

		if (this.options.loop) {
			this.pos.max = this.num.oItems + this.options.items;
		}

		if (!this.options.loop && !this.options.center) {
			this.pos.max = this.num.oItems - this.options.items;
		}

		if (this.options.loop && this.options.center) {
			this.pos.max = this.num.oItems + this.options.items;
		}

		//set max value
		this.pos.maxValue = this.pos.items[this.pos.max];

		//Max for autoWidth content
		if (!this.options.loop && !this.options.center && this.options.autoWidth || this.options.merge && !this.options.center) {
			var revert = this.options.rtl ? 1 : -1;
			for (i = 0; i < this.pos.items.length; i++) {
				if (this.pos.items[i] * revert < this.width.stage - this.width.el) {
					this.pos.max = i + 1;
				}
			}
			this.pos.maxValue = this.options.rtl ? this.width.stage - this.width.el : -(this.width.stage - this.width.el);
			this.pos.items[this.pos.max] = this.pos.maxValue;
		}

		// Set loop boundries
		if (this.options.center) {
			this.pos.loop = this.pos.items[0] - this.pos.items[this.num.oItems];
		} else {
			this.pos.loop = -this.pos.items[this.num.oItems];
		}

		//if is less items
		if (this.num.oItems < this.options.items && !this.options.center) {
			this.pos.max = 0;
			this.pos.maxValue = this.pos.items[0];
		}
	};

	/**
  * setSizes
  * @desc Set sizes on elements (from collectData function)
  * @since 2.0.0
  */

	Owl.prototype.setSizes = function () {

		// show neighbours
		if (this.options.stagePadding !== false) {
			this.dom.oStage.style.paddingLeft = this.options.stagePadding + 'px';
			this.dom.oStage.style.paddingRight = this.options.stagePadding + 'px';
		}

		// CRAZY FIX!!! Doublecheck this!
		//if(this.width.stagePrev > this.width.stage){
		if (this.options.rtl) {
			window.setTimeout((function () {
				this.dom.stage.style.width = this.width.stage + 'px';
			}).bind(this), 0);
		} else {
			this.dom.stage.style.width = this.width.stage + 'px';
		}

		for (var i = 0; i < this.num.items; i++) {

			// Set items width
			if (!this.options.autoWidth) {
				this.dom.$items[i].style.width = this.width.item - this.options.margin + 'px';
			}
			// add margin
			if (this.options.rtl) {
				this.dom.$items[i].style.marginLeft = this.options.margin + 'px';
			} else {
				this.dom.$items[i].style.marginRight = this.options.margin + 'px';
			}

			if (this.num.merged[i] !== 1 && !this.options.autoWidth) {
				this.dom.$items[i].style.width = this.width.item * this.num.merged[i] - this.options.margin + 'px';
			}
		}

		// save prev stage size
		this.width.stagePrev = this.width.stage;
	};

	/**
  * responsive
  * @desc Responsive function update all data by calling refresh() 
  * @since 2.0.0
  */

	Owl.prototype.responsive = function () {

		if (!this.num.oItems) {
			return false;
		}
		// If El width hasnt change then stop responsive
		var elChanged = this.isElWidthChanged();
		if (!elChanged) {
			return false;
		}

		// if Vimeo Fullscreen mode
		var fullscreenElement = document.fullscreenElement || document.mozFullScreenElement || document.webkitFullscreenElement;
		if (fullscreenElement) {
			if ($(fullscreenElement.parentNode).hasClass('owl-video-frame')) {
				this.setSpeed(0);
				this.state.isFullScreen = true;
			}
		}

		if (fullscreenElement && this.state.isFullScreen && this.state.videoPlay) {
			return false;
		}

		// Comming back from fullscreen
		if (this.state.isFullScreen) {
			this.state.isFullScreen = false;
			return false;
		}

		// check full screen mode and window orientation
		if (this.state.videoPlay) {
			if (this.state.orientation !== window.orientation) {
				this.state.orientation = window.orientation;
				return false;
			}
		}

		this.fireCallback('onResponsiveBefore');
		this.state.responsive = true;
		this.refresh();
		this.state.responsive = false;
		this.fireCallback('onResponsiveAfter');
	};

	/**
  * refresh
  * @desc Refresh method is basically collection of functions that are responsible for Owl responsive functionality
  * @since 2.0.0
  */

	Owl.prototype.refresh = function (init) {

		if (this.state.videoPlay) {
			this.stopVideo();
		}

		// Update Options for given width
		this.setResponsiveOptions();

		//set lazy structure
		this.createLazyContentStructure(true);

		// update info about local content
		this.updateLocalContent();

		// udpate options
		this.optionsLogic();

		// if no items then stop
		if (this.num.oItems === 0) {
			if (this.dom.$page !== null) {
				this.dom.$page.hide();
			}
			return false;
		}

		// Hide and Show methods helps here to set a proper widths.
		// This prevents Scrollbar to be calculated in stage width
		this.dom.$stage.addClass('owl-refresh');

		// Remove clones and generate new ones
		this.reClone();

		// calculate
		this.calculate();

		//aaaand show.
		this.dom.$stage.removeClass('owl-refresh');

		// to do
		// lazyContent last position on refresh
		if (this.state.lazyContent) {
			this.pos.currentAbs = this.options.items;
		}

		this.initPosition(init);

		// jump to last position
		if (!this.state.lazyContent && !init) {
			this.jumpTo(this.pos.current, false); // fix that
		}

		//Check for videos ( YouTube and Vimeo currently supported)
		this.checkVideoLinks();

		this.updateItemState();

		// Update controls
		this.rebuildDots();

		this.updateControls();

		// update drag events
		//this.updateEvents();

		// update autoplay
		this.autoplay();

		this.autoHeight();

		this.state.orientation = window.orientation;

		this.watchVisibility();
	};

	/**
  * updateItemState
  * @desc Update information about current state of items (visibile, hidden, active, etc.)
  * @since 2.0.0
  */

	Owl.prototype.updateItemState = function (update) {

		if (!this.state.lazyContent) {
			this.updateActiveItems();
		} else {
			this.updateLazyContent(update);
		}

		if (this.options.center) {
			this.dom.$items.eq(this.pos.currentAbs).addClass(this.options.centerClass).data('owl-item').center = true;
		}

		if (this.options.lazyLoad) {
			this.lazyLoad();
		}
	};

	/**
  * updateActiveItems
  * @since 2.0.0
  */

	Owl.prototype.updateActiveItems = function () {
		var i, j, item, ipos, iwidth, wpos, stage, outsideView, foundCurrent, stageX, view;
		// clear states
		for (i = 0; i < this.num.items; i++) {
			this.dom.$items.eq(i).data('owl-item').active = false;
			this.dom.$items.eq(i).data('owl-item').current = false;
			this.dom.$items.eq(i).removeClass(this.options.activeClass).removeClass(this.options.centerClass);
		}

		this.num.active = 0;
		stageX = this.pos.stage;
		view = this.options.rtl ? this.width.view : -this.width.view;

		for (j = 0; j < this.num.items; j++) {

			item = this.dom.$items.eq(j);
			ipos = item.data('owl-item').posLeft;
			iwidth = item.data('owl-item').width;
			outsideView = this.options.rtl ? ipos + iwidth : ipos - iwidth;

			if (this.op(ipos, '<=', stageX) && this.op(ipos, '>', stageX + view) || this.op(outsideView, '<', stageX) && this.op(outsideView, '>', stageX + view)) {

				this.num.active++;

				if (this.options.freeDrag && !foundCurrent) {
					foundCurrent = true;
					this.pos.current = item.data('owl-item').index;
					this.pos.currentAbs = item.data('owl-item').indexAbs;
				}

				item.data('owl-item').active = true;
				item.data('owl-item').current = true;
				item.addClass(this.options.activeClass);

				if (!this.options.lazyLoad) {
					item.data('owl-item').loaded = true;
				}
				if (this.options.loop && (this.options.lazyLoad || this.options.center)) {
					this.updateClonedItemsState(item.data('owl-item').index);
				}
			}
		}
	};

	/**
  * updateClonedItemsState
  * @desc Set current state on sibilings items for lazyLoad and center
  * @since 2.0.0
  */

	Owl.prototype.updateClonedItemsState = function (activeIndex) {

		//find cloned center
		var center, $el, i;
		if (this.options.center) {
			center = this.dom.$items.eq(this.pos.currentAbs).data('owl-item').index;
		}

		for (i = 0; i < this.num.items; i++) {
			$el = this.dom.$items.eq(i);
			if ($el.data('owl-item').index === activeIndex) {
				$el.data('owl-item').current = true;
				if ($el.data('owl-item').index === center) {
					$el.addClass(this.options.centerClass);
				}
			}
		}
	};

	/**
  * updateLazyPosition
  * @desc Set current state on sibilings items for lazyLoad and center
  * @since 2.0.0
  */

	Owl.prototype.updateLazyPosition = function () {
		var jumpTo = this.pos.goToLazyContent || 0;

		this.pos.lcMovedBy = Math.abs(this.options.items - this.pos.currentAbs);

		if (this.options.items < this.pos.currentAbs) {
			this.pos.lcCurrent += this.pos.currentAbs - this.options.items;
			this.state.lcDirection = 'right';
		} else if (this.options.items > this.pos.currentAbs) {
			this.pos.lcCurrent -= this.options.items - this.pos.currentAbs;
			this.state.lcDirection = 'left';
		}

		this.pos.lcCurrent = jumpTo !== 0 ? jumpTo : this.pos.lcCurrent;

		if (this.pos.lcCurrent >= this.dom.$content.length) {
			this.pos.lcCurrent = this.pos.lcCurrent - this.dom.$content.length;
		} else if (this.pos.lcCurrent < -this.dom.$content.length + 1) {
			this.pos.lcCurrent = this.pos.lcCurrent + this.dom.$content.length;
		}

		if (this.options.startPosition > 0) {
			this.pos.lcCurrent = this.options.startPosition;
			this._options.startPosition = this.options.startPosition = 0;
		}

		this.pos.lcCurrentAbs = this.pos.lcCurrent < 0 ? this.pos.lcCurrent + this.dom.$content.length : this.pos.lcCurrent;
	};

	/**
  * updateLazyContent
  * @param [update] - boolean - update call by content manipulations
  * @since 2.0.0
  */

	Owl.prototype.updateLazyContent = function (update) {

		if (this.pos.lcCurrent === undefined) {
			this.pos.lcCurrent = 0;
			this.pos.current = this.pos.currentAbs = this.options.items;
		}

		if (!update) {
			this.updateLazyPosition();
		}
		var i, j, item, contentPos, content, freshItem, freshData;

		if (this.state.lcDirection !== false) {
			for (i = 0; i < this.pos.lcMovedBy; i++) {

				if (this.state.lcDirection === 'right') {
					item = this.dom.$stage.find('.owl-item').eq(0); //.appendTo(this.dom.$stage);
					item.appendTo(this.dom.$stage);
				}
				if (this.state.lcDirection === 'left') {
					item = this.dom.$stage.find('.owl-item').eq(-1);
					item.prependTo(this.dom.$stage);
				}
				item.data('owl-item').active = false;
			}
		}

		// recollect
		this.dom.$items = this.dom.$stage.find('.owl-item');

		for (j = 0; j < this.num.items; j++) {
			// to do
			this.dom.$items.eq(j).removeClass(this.options.centerClass);

			// get Content
			contentPos = this.pos.lcCurrent + j - this.options.items; // + this.options.startPosition;

			if (contentPos >= this.dom.$content.length) {
				contentPos = contentPos - this.dom.$content.length;
			}
			if (contentPos < -this.dom.$content.length) {
				contentPos = contentPos + this.dom.$content.length;
			}

			content = this.dom.$content.eq(contentPos);
			freshItem = this.dom.$items.eq(j);
			freshData = freshItem.data('owl-item');

			if (freshData.active === false || this.pos.goToLazyContent !== 0 || update === true) {

				freshItem.empty();
				freshItem.append(content.clone(true, true));
				freshData.active = true;
				freshData.current = true;
				if (!this.options.lazyLoad) {
					freshData.loaded = true;
				} else {
					freshData.loaded = false;
				}
			}
		}

		this.pos.goToLazyContent = 0;
		this.pos.current = this.pos.currentAbs = this.options.items;
		this.setSpeed(0);
		this.animStage(this.pos.items[this.options.items]);
	};

	/**
  * eventsCall
  * @desc Save internal event references and add event based functions like transitionEnd,responsive etc.
  * @since 2.0.0
  */

	Owl.prototype.eventsCall = function () {
		// Save events references
		this.e._onDragStart = (function (e) {
			this.onDragStart(e);
		}).bind(this);
		this.e._onDragMove = (function (e) {
			this.onDragMove(e);
		}).bind(this);
		this.e._onDragEnd = (function (e) {
			this.onDragEnd(e);
		}).bind(this);
		this.e._transitionEnd = (function (e) {
			this.transitionEnd(e);
		}).bind(this);
		this.e._resizer = (function () {
			this.responsiveTimer();
		}).bind(this);
		this.e._responsiveCall = (function () {
			this.responsive();
		}).bind(this);
		this.e._preventClick = (function (e) {
			this.preventClick(e);
		}).bind(this);
		this.e._goToHash = (function () {
			this.goToHash();
		}).bind(this);
		this.e._goToPage = (function (e) {
			this.goToPage(e);
		}).bind(this);
		this.e._ap = (function () {
			this.autoplay();
		}).bind(this);
		this.e._play = (function () {
			this.play();
		}).bind(this);
		this.e._pause = (function () {
			this.pause();
		}).bind(this);
		this.e._playVideo = (function (e) {
			this.playVideo(e);
		}).bind(this);

		this.e._navNext = (function (e) {
			if ($(e.target).hasClass('disabled')) {
				return false;
			}
			e.preventDefault();
			this.next();
		}).bind(this);

		this.e._navPrev = (function (e) {
			if ($(e.target).hasClass('disabled')) {
				return false;
			}
			e.preventDefault();
			this.prev();
		}).bind(this);
	};

	/**
  * responsiveTimer
  * @desc Check Window resize event with 200ms delay / this.options.responsiveRefreshRate
  * @since 2.0.0
  */

	Owl.prototype.responsiveTimer = function () {
		if (this.windowWidth() === this.width.prevWindow) {
			return false;
		}
		window.clearInterval(this.e._autoplay);
		window.clearTimeout(this.resizeTimer);
		this.resizeTimer = window.setTimeout(this.e._responsiveCall, this.options.responsiveRefreshRate);
		this.width.prevWindow = this.windowWidth();
	};

	/**
  * internalEvents
  * @desc Checks for touch/mouse drag options and add necessery event handlers.
  * @since 2.0.0
  */

	Owl.prototype.internalEvents = function () {
		var isTouch = isTouchSupport();
		var isTouchIE = isTouchSupportIE();

		if (isTouch && !isTouchIE) {
			this.dragType = ['touchstart', 'touchmove', 'touchend', 'touchcancel'];
		} else if (isTouch && isTouchIE) {
			this.dragType = ['MSPointerDown', 'MSPointerMove', 'MSPointerUp', 'MSPointerCancel'];
		} else {
			this.dragType = ['mousedown', 'mousemove', 'mouseup'];
		}

		if ((isTouch || isTouchIE) && this.options.touchDrag) {
			//touch cancel event
			this.on(document, this.dragType[3], this.e._onDragEnd);
		} else {
			// firefox startdrag fix - addeventlistener doesnt work here :/
			this.dom.$stage.on('dragstart', function () {
				return false;
			});

			if (this.options.mouseDrag) {
				//disable text select
				this.dom.stage.onselectstart = function () {
					return false;
				};
			} else {
				// enable text select
				this.dom.$el.addClass('owl-text-select-on');
			}
		}

		// Video Play Button event delegation
		this.dom.$stage.on(this.dragType[2], '.owl-video-play-icon', this.e._playVideo);

		if (this.options.URLhashListener) {
			this.on(window, 'hashchange', this.e._goToHash, false);
		}

		if (this.options.autoplayHoverPause) {
			var that = this;
			this.dom.$stage.on('mouseover', this.e._pause);
			this.dom.$stage.on('mouseleave', this.e._ap);
		}

		// Catch transitionEnd event
		if (this.transitionEndVendor) {
			this.on(this.dom.stage, this.transitionEndVendor, this.e._transitionEnd, false);
		}

		// Responsive
		if (this.options.responsive !== false) {
			this.on(window, 'resize', this.e._resizer, false);
		}

		this.updateEvents();
	};

	/**
  * updateEvents
  * @since 2.0.0
  */

	Owl.prototype.updateEvents = function () {

		if (this.options.touchDrag && (this.dragType[0] === 'touchstart' || this.dragType[0] === 'MSPointerDown')) {
			this.on(this.dom.stage, this.dragType[0], this.e._onDragStart, false);
		} else if (this.options.mouseDrag && this.dragType[0] === 'mousedown') {
			this.on(this.dom.stage, this.dragType[0], this.e._onDragStart, false);
		} else {
			this.off(this.dom.stage, this.dragType[0], this.e._onDragStart);
		}
	};

	/**
  * onDragStart
  * @desc touchstart/mousedown event
  * @since 2.0.0
  */

	Owl.prototype.onDragStart = function (event) {
		var ev = event.originalEvent || event || window.event;
		// prevent right click
		if (ev.which === 3) {
			return false;
		}

		if (this.dragType[0] === 'mousedown') {
			this.dom.$stage.addClass('owl-grab');
		}

		this.fireCallback('onTouchStart');
		this.drag.startTime = new Date().getTime();
		this.setSpeed(0);
		this.state.isTouch = true;
		this.state.isScrolling = false;
		this.state.isSwiping = false;
		this.drag.distance = 0;

		// if is 'touchstart'
		var isTouchEvent = ev.type === 'touchstart';
		var pageX = isTouchEvent ? event.targetTouches[0].pageX : ev.pageX || ev.clientX;
		var pageY = isTouchEvent ? event.targetTouches[0].pageY : ev.pageY || ev.clientY;

		//get stage position left
		this.drag.offsetX = this.dom.$stage.position().left - this.options.stagePadding;
		this.drag.offsetY = this.dom.$stage.position().top;

		if (this.options.rtl) {
			this.drag.offsetX = this.dom.$stage.position().left + this.width.stage - this.width.el + this.options.margin;
		}

		//catch position // ie to fix
		if (this.state.inMotion && this.support3d) {
			var animatedPos = this.getTransformProperty();
			this.drag.offsetX = animatedPos;
			this.animStage(animatedPos);
		} else if (this.state.inMotion && !this.support3d) {
			this.state.inMotion = false;
			return false;
		}

		this.drag.startX = pageX - this.drag.offsetX;
		this.drag.startY = pageY - this.drag.offsetY;

		this.drag.start = pageX - this.drag.startX;
		this.drag.targetEl = ev.target || ev.srcElement;
		this.drag.updatedX = this.drag.start;

		// to do/check
		//prevent links and images dragging;
		//this.drag.targetEl.draggable = false;

		this.on(document, this.dragType[1], this.e._onDragMove, false);
		this.on(document, this.dragType[2], this.e._onDragEnd, false);
	};

	/**
  * onDragMove
  * @desc touchmove/mousemove event
  * @since 2.0.0
  */

	Owl.prototype.onDragMove = function (event) {
		if (!this.state.isTouch) {
			return;
		}

		if (this.state.isScrolling) {
			return;
		}

		var neighbourItemWidth = 0;
		var ev = event.originalEvent || event || window.event;

		// if is 'touchstart'
		var isTouchEvent = ev.type == 'touchmove';
		var pageX = isTouchEvent ? ev.targetTouches[0].pageX : ev.pageX || ev.clientX;
		var pageY = isTouchEvent ? ev.targetTouches[0].pageY : ev.pageY || ev.clientY;

		// Drag Direction
		this.drag.currentX = pageX - this.drag.startX;
		this.drag.currentY = pageY - this.drag.startY;
		this.drag.distance = this.drag.currentX - this.drag.offsetX;

		// Check move direction
		if (this.drag.distance < 0) {
			this.state.direction = this.options.rtl ? 'right' : 'left';
		} else if (this.drag.distance > 0) {
			this.state.direction = this.options.rtl ? 'left' : 'right';
		}
		// Loop
		if (this.options.loop) {
			if (this.op(this.drag.currentX, '>', this.pos.minValue) && this.state.direction === 'right') {
				this.drag.currentX -= this.pos.loop;
			} else if (this.op(this.drag.currentX, '<', this.pos.maxValue) && this.state.direction === 'left') {
				this.drag.currentX += this.pos.loop;
			}
		} else {
			// pull
			var minValue = this.options.rtl ? this.pos.maxValue : this.pos.minValue;
			var maxValue = this.options.rtl ? this.pos.minValue : this.pos.maxValue;
			var pull = this.options.pullDrag ? this.drag.distance / 5 : 0;
			this.drag.currentX = Math.max(Math.min(this.drag.currentX, minValue + pull), maxValue + pull);
		}

		// Lock browser if swiping horizontal

		if (this.drag.distance > 8 || this.drag.distance < -8) {
			if (ev.preventDefault !== undefined) {
				ev.preventDefault();
			} else {
				ev.returnValue = false;
			}
			this.state.isSwiping = true;
		}

		this.drag.updatedX = this.drag.currentX;

		// Lock Owl if scrolling
		if ((this.drag.currentY > 16 || this.drag.currentY < -16) && this.state.isSwiping === false) {
			this.state.isScrolling = true;
			this.drag.updatedX = this.drag.start;
		}

		this.animStage(this.drag.updatedX);
	};

	/**
  * onDragEnd 
  * @desc touchend/mouseup event
  * @since 2.0.0
  */

	Owl.prototype.onDragEnd = function (event) {
		if (!this.state.isTouch) {
			return;
		}
		if (this.dragType[0] === 'mousedown') {
			this.dom.$stage.removeClass('owl-grab');
		}

		this.fireCallback('onTouchEnd');

		//prevent links and images dragging;
		//this.drag.targetEl.draggable = true;

		//remove drag event listeners

		this.state.isTouch = false;
		this.state.isScrolling = false;
		this.state.isSwiping = false;

		//to check
		if (this.drag.distance === 0 && this.state.inMotion !== true) {
			this.state.inMotion = false;
			return false;
		}

		// prevent clicks while scrolling

		this.drag.endTime = new Date().getTime();
		var compareTimes = this.drag.endTime - this.drag.startTime;
		var distanceAbs = Math.abs(this.drag.distance);

		//to test
		if (distanceAbs > 3 || compareTimes > 300) {
			this.removeClick(this.drag.targetEl);
		}

		var closest = this.closest(this.drag.updatedX);

		this.setSpeed(this.options.dragEndSpeed, false, true);
		this.animStage(this.pos.items[closest]);

		//if pullDrag is off then fire transitionEnd event manually when stick to border
		if (!this.options.pullDrag && this.drag.updatedX === this.pos.items[closest]) {
			this.transitionEnd();
		}

		this.drag.distance = 0;

		this.off(document, this.dragType[1], this.e._onDragMove);
		this.off(document, this.dragType[2], this.e._onDragEnd);
	};

	/**
  * removeClick
  * @desc Attach preventClick function to disable link while swipping
  * @since 2.0.0
  * @param [target] - clicked dom element
  */

	Owl.prototype.removeClick = function (target) {
		this.drag.targetEl = target;
		this.on(target, 'click', this.e._preventClick, false);
	};

	/**
  * preventClick
  * @desc Add preventDefault for any link and then remove removeClick event hanlder
  * @since 2.0.0
  */

	Owl.prototype.preventClick = function (ev) {
		if (ev.preventDefault) {
			ev.preventDefault();
		} else {
			ev.returnValue = false;
		}
		if (ev.stopPropagation) {
			ev.stopPropagation();
		}
		this.off(this.drag.targetEl, 'click', this.e._preventClick, false);
	};

	/**
  * getTransformProperty
  * @desc catch stage position while animate (only css3)
  * @since 2.0.0
  */

	Owl.prototype.getTransformProperty = function () {
		var transform = window.getComputedStyle(this.dom.stage, null).getPropertyValue(this.vendorName + 'transform');
		//var transform = this.dom.$stage.css(this.vendorName + 'transform')
		transform = transform.replace(/matrix(3d)?\(|\)/g, '').split(',');
		var matrix3d = transform.length === 16;

		return matrix3d !== true ? transform[4] : transform[12];
	};

	/**
  * closest
  * @desc Get closest item after touchend/mouseup
  * @since 2.0.0
  * @param [x] - curent position in pixels
  * return position in pixels
  */

	Owl.prototype.closest = function (x) {
		var newX = 0,
		    pull = 30;

		if (!this.options.freeDrag) {
			// Check closest item
			for (var i = 0; i < this.num.items; i++) {
				if (x > this.pos.items[i] - pull && x < this.pos.items[i] + pull) {
					newX = i;
				} else if (this.op(x, '<', this.pos.items[i]) && this.op(x, '>', this.pos.items[i + 1 || this.pos.items[i] - this.width.el])) {
					newX = this.state.direction === 'left' ? i + 1 : i;
				}
			}
		}
		//non loop boundries
		if (!this.options.loop) {
			if (this.op(x, '>', this.pos.minValue)) {
				newX = x = this.pos.min;
			} else if (this.op(x, '<', this.pos.maxValue)) {
				newX = x = this.pos.max;
			}
		}

		if (!this.options.freeDrag) {
			// set positions
			this.pos.currentAbs = newX;
			this.pos.current = this.dom.$items.eq(newX).data('owl-item').index;
		} else {
			this.updateItemState();
			return x;
		}

		return newX;
	};

	/**
  * animStage
  * @desc animate stage position (both css3/css2) and perform onChange functions/events
  * @since 2.0.0
  * @param [x] - curent position in pixels
  */

	Owl.prototype.animStage = function (pos) {

		// if speed is 0 the set inMotion to false
		if (this.speed.current !== 0 && this.pos.currentAbs !== this.pos.min) {
			this.fireCallback('onTransitionStart');
			this.state.inMotion = true;
		}

		var posX = this.pos.stage = pos,
		    style = this.dom.stage.style;

		if (this.support3d) {
			var translate = 'translate3d(' + posX + 'px' + ',0px, 0px)';
			style[this.transformVendor] = translate;
		} else if (this.state.isTouch) {
			style.left = posX + 'px';
		} else {
			this.dom.$stage.animate({ left: posX }, this.speed.css2speed, this.options.fallbackEasing, (function () {
				if (this.state.inMotion) {
					this.transitionEnd();
				}
			}).bind(this));
		}

		this.onChange();
	};

	/**
  * updatePosition
  * @desc Update current positions
  * @since 2.0.0
  * @param [pos] - number - new position
  */

	Owl.prototype.updatePosition = function (pos) {

		// if no items then stop
		if (this.num.oItems === 0) {
			return false;
		}
		// to do
		//if(pos > this.num.items){pos = 0;}
		if (pos === undefined) {
			return false;
		}

		//pos - new current position
		var nextPos = pos;
		this.pos.prev = this.pos.currentAbs;

		if (this.state.revert) {
			this.pos.current = this.dom.$items.eq(nextPos).data('owl-item').index;
			this.pos.currentAbs = nextPos;
			return;
		}

		if (!this.options.loop) {
			if (this.options.navRewind) {
				nextPos = nextPos > this.pos.max ? this.pos.min : nextPos < 0 ? this.pos.max : nextPos;
			} else {
				nextPos = nextPos > this.pos.max ? this.pos.max : nextPos <= 0 ? 0 : nextPos;
			}
		} else {
			nextPos = nextPos >= this.num.oItems ? this.num.oItems - 1 : nextPos;
		}

		this.pos.current = this.dom.$oItems.eq(nextPos).data('owl-item').index;
		this.pos.currentAbs = this.dom.$oItems.eq(nextPos).data('owl-item').indexAbs;
	};

	/**
  * setSpeed
  * @since 2.0.0
  * @param [speed] - number
  * @param [pos] - number - next position - use this param to calculate smartSpeed
  * @param [drag] - boolean - if drag is true then smart speed is disabled
  * return speed
  */

	Owl.prototype.setSpeed = function (speed, pos, drag) {
		var s = speed,
		    nextPos = pos;

		if (s === false && s !== 0 && drag !== true || s === undefined) {

			//Double check this
			// var nextPx = this.pos.items[nextPos];
			// var currPx = this.pos.stage
			// var diff = Math.abs(nextPx-currPx);
			// var s = diff/1
			// if(s>1000){
			// 	s = 1000;
			// }

			var diff = Math.abs(nextPos - this.pos.prev);
			diff = diff === 0 ? 1 : diff;
			if (diff > 6) {
				diff = 6;
			}
			s = diff * this.options.smartSpeed;
		}

		if (s === false && drag === true) {
			s = this.options.smartSpeed;
		}

		if (s === 0) {
			s = 0;
		}

		if (this.support3d) {
			var style = this.dom.stage.style;
			style.webkitTransitionDuration = style.MsTransitionDuration = style.msTransitionDuration = style.MozTransitionDuration = style.OTransitionDuration = style.transitionDuration = s / 1000 + 's';
		} else {
			this.speed.css2speed = s;
		}
		this.speed.current = s;
		return s;
	};

	/**
  * jumpTo
  * @since 2.0.0
  * @param [pos] - number - next position - use this param to calculate smartSpeed
  * @param [update] - boolean - if drag is true then smart speed is disabled
  */

	Owl.prototype.jumpTo = function (pos, update) {
		if (this.state.lazyContent) {
			this.pos.goToLazyContent = pos;
		}
		this.updatePosition(pos);
		this.setSpeed(0);
		this.animStage(this.pos.items[this.pos.currentAbs]);
		if (update !== true) {
			this.updateItemState();
		}
	};

	/**
  * goTo
  * @since 2.0.0
  * @param [pos] - number
  * @param [speed] - speed in ms
  * @param [speed] - speed in ms
  */

	Owl.prototype.goTo = function (pos, speed) {
		if (this.state.lazyContent && this.state.inMotion) {
			return false;
		}

		this.updatePosition(pos);

		if (this.state.animate) {
			speed = 0;
		}
		this.setSpeed(speed, this.pos.currentAbs);

		if (this.state.animate) {
			this.animate();
		}
		this.animStage(this.pos.items[this.pos.currentAbs]);
	};

	/**
  * next
  * @since 2.0.0
  */

	Owl.prototype.next = function (optionalSpeed) {
		var s = optionalSpeed || this.options.navSpeed;
		if (this.options.loop && !this.state.lazyContent) {
			this.goToLoop(this.options.slideBy, s);
		} else {
			this.goTo(this.pos.current + this.options.slideBy, s);
		}
	};

	/**
  * prev
  * @since 2.0.0
  */

	Owl.prototype.prev = function (optionalSpeed) {
		var s = optionalSpeed || this.options.navSpeed;
		if (this.options.loop && !this.state.lazyContent) {
			this.goToLoop(-this.options.slideBy, s);
		} else {
			this.goTo(this.pos.current - this.options.slideBy, s);
		}
	};

	/**
  * goToLoop
  * @desc Go to given position if loop is enabled - used only internal
  * @since 2.0.0
  * @param [distance] - number -how far to go
  * @param [speed] - number - speed in ms
  */

	Owl.prototype.goToLoop = function (distance, speed) {

		var revert = this.pos.currentAbs,
		    prevPosition = this.pos.currentAbs,
		    newPosition = this.pos.currentAbs + distance,
		    direction = prevPosition - newPosition < 0 ? true : false;

		this.state.revert = true;

		if (newPosition < 1 && direction === false) {

			this.state.bypass = true;
			revert = this.num.items - (this.options.items - prevPosition) - this.options.items;
			this.jumpTo(revert, true);
		} else if (newPosition >= this.num.items - this.options.items && direction === true) {

			this.state.bypass = true;
			revert = prevPosition - this.num.oItems;
			this.jumpTo(revert, true);
		}
		window.clearTimeout(this.e._goToLoop);
		this.e._goToLoop = window.setTimeout((function () {
			this.state.bypass = false;
			this.goTo(revert + distance, speed);
			this.state.revert = false;
		}).bind(this), 30);
	};

	/**
  * initPosition
  * @since 2.0.0
  */

	Owl.prototype.initPosition = function (init) {

		if (!this.dom.$oItems || !init || this.state.lazyContent) {
			return false;
		}
		var pos = this.options.startPosition;

		if (this.options.startPosition === 'URLHash') {
			pos = this.options.startPosition = this.hashPosition();
		} else if (typeof this.options.startPosition !== Number && !this.options.center) {
			this.options.startPosition = 0;
		}
		this.dom.oStage.scrollLeft = 0;
		this.jumpTo(pos, true);
	};

	/**
  * goToHash
  * @since 2.0.0
  */

	Owl.prototype.goToHash = function () {
		var pos = this.hashPosition();
		if (pos === false) {
			pos = 0;
		}
		this.dom.oStage.scrollLeft = 0;
		this.goTo(pos, this.options.navSpeed);
	};

	/**
  * hashPosition
  * @desc Find hash in URL then look into items to find contained ID
  * @since 2.0.0
  * return hashPos - number of item
  */

	Owl.prototype.hashPosition = function () {
		var hash = window.location.hash.substring(1),
		    hashPos;
		if (hash === "") {
			return false;
		}

		for (var i = 0; i < this.num.oItems; i++) {
			if (hash === this.dom.$oItems.eq(i).data('owl-item').hash) {
				hashPos = i;
			}
		}
		return hashPos;
	};

	/**
  * Autoplay
  * @since 2.0.0
  */

	Owl.prototype.autoplay = function () {
		if (this.options.autoplay && !this.state.videoPlay) {
			window.clearInterval(this.e._autoplay);
			this.e._autoplay = window.setInterval(this.e._play, this.options.autoplayTimeout);
		} else {
			window.clearInterval(this.e._autoplay);
			this.state.autoplay = false;
		}
	};

	/**
  * play
  * @param [timeout] - Integrer
  * @param [speed] - Integrer
  * @since 2.0.0
  */

	Owl.prototype.play = function (timeout, speed) {

		// if tab is inactive - doesnt work in <IE10
		if (document.hidden === true) {
			return false;
		}

		// overwrite default options (custom options are always priority)
		if (!this.options.autoplay) {
			this._options.autoplay = this.options.autoplay = true;
			this._options.autoplayTimeout = this.options.autoplayTimeout = timeout || this.options.autoplayTimeout || 4000;
			this._options.autoplaySpeed = speed || this.options.autoplaySpeed;
		}

		if (this.options.autoplay === false || this.state.isTouch || this.state.isScrolling || this.state.isSwiping || this.state.inMotion) {
			window.clearInterval(this.e._autoplay);
			return false;
		}

		if (!this.options.loop && this.pos.current >= this.pos.max) {
			window.clearInterval(this.e._autoplay);
			this.goTo(0);
		} else {
			this.next(this.options.autoplaySpeed);
		}
		this.state.autoplay = true;
	};

	/**
  * stop
  * @since 2.0.0
  */

	Owl.prototype.stop = function () {
		this._options.autoplay = this.options.autoplay = false;
		this.state.autoplay = false;
		window.clearInterval(this.e._autoplay);
	};

	Owl.prototype.pause = function () {
		window.clearInterval(this.e._autoplay);
	};

	/**
  * transitionEnd
  * @desc event used by css3 animation end and $.animate callback like transitionEnd,responsive etc.
  * @since 2.0.0
  */

	Owl.prototype.transitionEnd = function (event) {

		// if css2 animation then event object is undefined
		if (event !== undefined) {
			event.stopPropagation();

			// Catch only owl-stage transitionEnd event
			var eventTarget = event.target || event.srcElement || event.originalTarget;
			if (eventTarget !== this.dom.stage) {
				return false;
			}
		}

		this.state.inMotion = false;
		this.updateItemState();
		this.autoplay();
		this.fireCallback('onTransitionEnd');
	};

	/**
  * isElWidthChanged
  * @desc Check if element width has changed
  * @since 2.0.0
  */

	Owl.prototype.isElWidthChanged = function () {
		var newElWidth = this.dom.$el.width() - this.options.stagePadding,
		    //to check
		prevElWidth = this.width.el + this.options.margin;
		return newElWidth !== prevElWidth;
	};

	/**
  * windowWidth
  * @desc Get Window/responsiveBaseElement width
  * @since 2.0.0
  */

	Owl.prototype.windowWidth = function () {
		if (this.options.responsiveBaseElement !== window) {
			this.width.window = $(this.options.responsiveBaseElement).width();
		} else if (window.innerWidth) {
			this.width.window = window.innerWidth;
		} else if (document.documentElement && document.documentElement.clientWidth) {
			this.width.window = document.documentElement.clientWidth;
		}
		return this.width.window;
	};

	/**
  * Controls
  * @desc Calls controls container, navigation and dots creator
  * @since 2.0.0
  */

	Owl.prototype.controls = function () {
		var cc = document.createElement('div');
		cc.className = this.options.controlsClass;
		this.dom.$el.append(cc);
		this.dom.$cc = $(cc);
	};

	/**
  * updateControls 
  * @since 2.0.0
  */

	Owl.prototype.updateControls = function () {

		if (this.dom.$cc === null && (this.options.nav || this.options.dots)) {
			this.controls();
		}

		if (this.dom.$nav === null && this.options.nav) {
			this.createNavigation(this.dom.$cc[0]);
		}

		if (this.dom.$page === null && this.options.dots) {
			this.createDots(this.dom.$cc[0]);
		}

		if (this.dom.$nav !== null) {
			if (this.options.nav) {
				this.dom.$nav.show();
				this.updateNavigation();
			} else {
				this.dom.$nav.hide();
			}
		}

		if (this.dom.$page !== null) {
			if (this.options.dots) {
				this.dom.$page.show();
				this.updateDots();
			} else {
				this.dom.$page.hide();
			}
		}
	};

	/**
  * createNavigation
  * @since 2.0.0
  * @param [cc] - dom element - Controls Container
  */

	Owl.prototype.createNavigation = function (cc) {

		// Create nav container
		var nav = document.createElement('div');
		nav.className = this.options.navContainerClass;
		cc.appendChild(nav);

		// Create left and right buttons
		var navPrev = document.createElement('div'),
		    navNext = document.createElement('div');

		navPrev.className = this.options.navClass[0];
		navNext.className = this.options.navClass[1];

		nav.appendChild(navPrev);
		nav.appendChild(navNext);

		this.dom.$nav = $(nav);
		this.dom.$navPrev = $(navPrev).html(this.options.navText[0]);
		this.dom.$navNext = $(navNext).html(this.options.navText[1]);

		// add events to do
		//this.on(navPrev, this.dragType[2], this.e._navPrev, false);
		//this.on(navNext, this.dragType[2], this.e._navNext, false);

		//FF fix?
		this.dom.$nav.on(this.dragType[2], '.' + this.options.navClass[0], this.e._navPrev);
		this.dom.$nav.on(this.dragType[2], '.' + this.options.navClass[1], this.e._navNext);
	};

	/**
  * createNavigation
  * @since 2.0.0
  * @param [cc] - dom element - Controls Container
  */

	Owl.prototype.createDots = function (cc) {

		// Create dots container
		var page = document.createElement('div');
		page.className = this.options.dotsClass;
		cc.appendChild(page);

		// save reference
		this.dom.$page = $(page);

		// add events
		//this.on(page, this.dragType[2], this.e._goToPage, false);

		// FF fix? To test!
		var that = this;
		this.dom.$page.on(this.dragType[2], '.' + this.options.dotClass, goToPage);

		function goToPage(e) {
			e.preventDefault();
			var page = $(this).data('page');
			that.goTo(page, that.options.dotsSpeed);
		}
		// build dots
		this.rebuildDots();
	};

	/**
  * goToPage
  * @desc Event used by dots
  * @since 2.0.0
  */

	// Owl.prototype.goToPage = function(e){
	// 	console.log(e.taget);
	// 	var page = $(e.currentTarget).data('page')
	// 	this.goTo(page,this.options.dotsSpeed);
	// 	return false;
	// };

	/**
  * rebuildDots
  * @since 2.0.0
  */

	Owl.prototype.rebuildDots = function () {
		if (this.dom.$page === null) {
			return false;
		}
		var each,
		    dot,
		    span,
		    counter = 0,
		    last = 0,
		    i,
		    page = 0,
		    roundPages = 0;

		each = this.options.dotsEach || this.options.items;

		// display full dots if center
		if (this.options.center || this.options.dotData) {
			each = 1;
		}

		// clear dots
		this.dom.$page.html('');

		for (i = 0; i < this.num.nav.length; i++) {

			if (counter >= each || counter === 0) {

				dot = document.createElement('div');
				dot.className = this.options.dotClass;
				span = document.createElement('span');
				dot.appendChild(span);
				var $dot = $(dot);

				if (this.options.dotData) {
					$dot.html(this.dom.$oItems.eq(i).data('owl-item').dot);
				}

				$dot.data('page', page);
				$dot.data('goToPage', roundPages);

				this.dom.$page.append(dot);

				counter = 0;
				roundPages++;
			}

			this.dom.$oItems.eq(i).data('owl-item').page = roundPages - 1;

			//add merged items
			counter += this.num.nav[i];
			page++;
		}
		// find rest of dots
		if (!this.options.loop && !this.options.center) {
			for (var j = this.num.nav.length - 1; j >= 0; j--) {
				last += this.num.nav[j];
				this.dom.$oItems.eq(j).data('owl-item').page = roundPages - 1;
				if (last >= each) {
					break;
				}
			}
		}

		this.num.allPages = roundPages - 1;
	};

	/**
  * updateDots
  * @since 2.0.0
  */

	Owl.prototype.updateDots = function () {
		var dots = this.dom.$page.children();
		var itemIndex = this.dom.$oItems.eq(this.pos.current).data('owl-item').page;

		for (var i = 0; i < dots.length; i++) {
			var dotPage = dots.eq(i).data('goToPage');

			if (dotPage === itemIndex) {
				this.pos.currentPage = i;
				dots.eq(i).addClass('active');
			} else {
				dots.eq(i).removeClass('active');
			}
		}
	};

	/**
  * updateNavigation
  * @since 2.0.0
  */

	Owl.prototype.updateNavigation = function () {

		var isNav = this.options.nav;

		this.dom.$navNext.toggleClass('disabled', !isNav);
		this.dom.$navPrev.toggleClass('disabled', !isNav);

		if (!this.options.loop && isNav && !this.options.navRewind) {

			if (this.pos.current <= 0) {
				this.dom.$navPrev.addClass('disabled');
			}
			if (this.pos.current >= this.pos.max) {
				this.dom.$navNext.addClass('disabled');
			}
		}
	};

	Owl.prototype.insertContent = function (content) {
		this.dom.$stage.empty();
		this.fetchContent(content);
		this.refresh();
	};

	/**
  * addItem - Add an item
  * @since 2.0.0
  * @param [content] - dom element / string '<div>content</div>'
  * @param [pos] - number - position
  */

	Owl.prototype.addItem = function (content, pos) {
		pos = pos || 0;

		if (this.state.lazyContent) {
			this.dom.$content = this.dom.$content.add($(content));
			this.updateItemState(true);
		} else {
			// wrap content
			var item = this.fillItem(content);
			// if carousel is empty then append item
			if (this.dom.$oItems.length === 0) {
				this.dom.$stage.append(item);
			} else {
				// append item
				var it = this.dom.$oItems.eq(pos);
				if (pos !== -1) {
					it.before(item);
				} else {
					it.after(item);
				}
			}
			// update and calculate carousel
			this.refresh();
		}
	};

	/**
  * removeItem - Remove an Item
  * @since 2.0.0
  * @param [pos] - number - position
  */

	Owl.prototype.removeItem = function (pos) {
		if (this.state.lazyContent) {
			this.dom.$content.splice(pos, 1);
			this.updateItemState(true);
		} else {
			this.dom.$oItems.eq(pos).remove();
			this.refresh();
		}
	};

	/**
  * addCustomEvents
  * @desc Add custom events by jQuery .on method
  * @since 2.0.0
  */

	Owl.prototype.addCustomEvents = function () {

		this.e.next = (function (e, s) {
			this.next(s);
		}).bind(this);
		this.e.prev = (function (e, s) {
			this.prev(s);
		}).bind(this);
		this.e.goTo = (function (e, p, s) {
			this.goTo(p, s);
		}).bind(this);
		this.e.jumpTo = (function (e, p) {
			this.jumpTo(p);
		}).bind(this);
		this.e.addItem = (function (e, c, p) {
			this.addItem(c, p);
		}).bind(this);
		this.e.removeItem = (function (e, p) {
			this.removeItem(p);
		}).bind(this);
		this.e.refresh = (function (e) {
			this.refresh();
		}).bind(this);
		this.e.destroy = (function (e) {
			this.destroy();
		}).bind(this);
		this.e.autoHeight = (function (e) {
			this.autoHeight(true);
		}).bind(this);
		this.e.stop = (function () {
			this.stop();
		}).bind(this);
		this.e.play = (function (e, t, s) {
			this.play(t, s);
		}).bind(this);
		this.e.insertContent = (function (e, d) {
			this.insertContent(d);
		}).bind(this);

		this.dom.$el.on('next.owl', this.e.next);
		this.dom.$el.on('prev.owl', this.e.prev);
		this.dom.$el.on('goTo.owl', this.e.goTo);
		this.dom.$el.on('jumpTo.owl', this.e.jumpTo);
		this.dom.$el.on('addItem.owl', this.e.addItem);
		this.dom.$el.on('removeItem.owl', this.e.removeItem);
		this.dom.$el.on('destroy.owl', this.e.destroy);
		this.dom.$el.on('refresh.owl', this.e.refresh);
		this.dom.$el.on('autoHeight.owl', this.e.autoHeight);
		this.dom.$el.on('play.owl', this.e.play);
		this.dom.$el.on('stop.owl', this.e.stop);
		this.dom.$el.on('stopVideo.owl', this.e.stop);
		this.dom.$el.on('insertContent.owl', this.e.insertContent);
	};

	/**
  * on
  * @desc On method for adding internal events
  * @since 2.0.0
  */

	Owl.prototype.on = function (element, event, listener, capture) {

		if (element.addEventListener) {
			element.addEventListener(event, listener, capture);
		} else if (element.attachEvent) {
			element.attachEvent('on' + event, listener);
		}
	};

	/**
  * off
  * @desc Off method for removing internal events
  * @since 2.0.0
  */

	Owl.prototype.off = function (element, event, listener, capture) {
		if (element.removeEventListener) {
			element.removeEventListener(event, listener, capture);
		} else if (element.detachEvent) {
			element.detachEvent('on' + event, listener);
		}
	};

	/**
  * fireCallback
  * @since 2.0.0
  * @param event - string - event name
  * @param data - object - additional options - to do
  */

	Owl.prototype.fireCallback = function (event, data) {
		if (!this.options.callbacks) {
			return;
		}

		if (this.dom.el.dispatchEvent) {

			// dispatch event
			var evt = document.createEvent('CustomEvent');

			//evt.initEvent(event, false, true );
			evt.initCustomEvent(event, true, true, data);
			return this.dom.el.dispatchEvent(evt);
		} else if (!this.dom.el.dispatchEvent) {

			//	There is no clean solution for custom events name in <=IE8
			//	But if you know better way, please let me know :)
			return this.dom.$el.trigger(event);
		}
	};

	/**
  * watchVisibility
  * @desc check if el is visible - handy if Owl is inside hidden content (tabs etc.)
  * @since 2.0.0
  */

	Owl.prototype.watchVisibility = function () {

		// test on zepto
		if (!isElVisible(this.dom.el)) {
			this.dom.$el.addClass('owl-hidden');
			window.clearInterval(this.e._checkVisibile);
			this.e._checkVisibile = window.setInterval(checkVisible.bind(this), 500);
		}

		function isElVisible(el) {
			return el.offsetWidth > 0 && el.offsetHeight > 0;
		}

		function checkVisible() {
			if (isElVisible(this.dom.el)) {
				this.dom.$el.removeClass('owl-hidden');
				this.refresh();
				window.clearInterval(this.e._checkVisibile);
			}
		}
	};

	/**
  * onChange
  * @since 2.0.0
  */

	Owl.prototype.onChange = function () {

		if (!this.state.isTouch && !this.state.bypass && !this.state.responsive) {

			if (this.options.nav || this.options.dots) {
				this.updateControls();
			}
			this.autoHeight();

			this.fireCallback('onChangeState');
		}

		if (!this.state.isTouch && !this.state.bypass) {
			// set Status to do
			this.storeInfo();

			// stopVideo
			if (this.state.videoPlay) {
				this.stopVideo();
			}
		}
	};

	/**
  * storeInfo
  * store basic information about current states
  * @since 2.0.0
  */

	Owl.prototype.storeInfo = function () {
		var currentPosition = this.state.lazyContent ? this.pos.lcCurrentAbs || 0 : this.pos.current;
		var allItems = this.state.lazyContent ? this.dom.$content.length - 1 : this.num.oItems;

		this.info = {
			items: this.options.items,
			allItems: allItems,
			currentPosition: currentPosition,
			currentPage: this.pos.currentPage,
			allPages: this.num.allPages,
			autoplay: this.state.autoplay,
			windowWidth: this.width.window,
			elWidth: this.width.el,
			breakpoint: this.num.breakpoint
		};

		if (typeof this.options.info === 'function') {
			this.options.info.apply(this, [this.info, this.dom.el]);
		}
	};

	/**
  * autoHeight
  * @since 2.0.0
  */

	Owl.prototype.autoHeight = function (callback) {
		if (this.options.autoHeight !== true && callback !== true) {
			return false;
		}
		if (!this.dom.$oStage.hasClass(this.options.autoHeightClass)) {
			this.dom.$oStage.addClass(this.options.autoHeightClass);
		}

		var loaded = this.dom.$items.eq(this.pos.currentAbs);
		var stage = this.dom.$oStage;
		var iterations = 0;

		var isLoaded = window.setInterval(function () {
			iterations += 1;
			if (loaded.data('owl-item').loaded) {
				stage.height(loaded.height() + 'px');
				clearInterval(isLoaded);
			} else if (iterations === 500) {
				clearInterval(isLoaded);
			}
		}, 100);
	};

	/**
  * preloadAutoWidthImages
  * @desc still to test
  * @since 2.0.0
  */

	Owl.prototype.preloadAutoWidthImages = function (imgs) {
		var loaded = 0;
		var that = this;
		imgs.each(function (i, el) {
			var $el = $(el);
			var img = new Image();

			img.onload = function () {
				loaded++;
				$el.attr('src', img.src);
				$el.css('opacity', 1);
				if (loaded >= imgs.length) {
					that.state.imagesLoaded = true;
					that.init();
				}
			};

			img.src = $el.attr('src') || $el.attr('data-src') || $el.attr('data-src-retina');;
		});
	};

	/**
  * lazyLoad
  * @desc lazyLoad images
  * @since 2.0.0
  */

	Owl.prototype.lazyLoad = function () {
		var attr = isRetina() ? 'data-src-retina' : 'data-src';
		var src, img, i;

		for (i = 0; i < this.num.items; i++) {
			var $item = this.dom.$items.eq(i);

			if ($item.data('owl-item').current === true && $item.data('owl-item').loaded === false) {
				img = $item.find('.owl-lazy');
				src = img.attr(attr);
				src = src || img.attr('data-src');
				if (src) {
					img.css('opacity', '0');
					this.preload(img, $item);
				}
			}
		}
	};

	/**
  * preload
  * @since 2.0.0
  */

	Owl.prototype.preload = function (images, $item) {
		var that = this; // fix this later

		images.each(function (i, el) {
			var $el = $(el);
			var img = new Image();

			img.onload = function () {

				$item.data('owl-item').loaded = true;
				if ($el.is('img')) {
					$el.attr('src', img.src);
				} else {
					$el.css('background-image', 'url(' + img.src + ')');
				}

				$el.css('opacity', 1);
				that.fireCallback('onLazyLoaded');
			};
			img.src = $el.attr('data-src') || $el.attr('data-src-retina');
		});
	};

	/**
  * animate
  * @since 2.0.0
  */

	Owl.prototype.animate = function () {

		var prevItem = this.dom.$items.eq(this.pos.prev),
		    prevPos = Math.abs(prevItem.data('owl-item').width) * this.pos.prev,
		    currentItem = this.dom.$items.eq(this.pos.currentAbs),
		    currentPos = Math.abs(currentItem.data('owl-item').width) * this.pos.currentAbs;

		if (this.pos.currentAbs === this.pos.prev) {
			return false;
		}

		var pos = currentPos - prevPos;
		var tIn = this.options.animateIn;
		var tOut = this.options.animateOut;
		var that = this;

		removeStyles = function () {
			$(this).css({
				"left": ""
			}).removeClass('animated owl-animated-out owl-animated-in').removeClass(tIn).removeClass(tOut);

			that.transitionEnd();
		};

		if (tOut) {
			prevItem.css({
				"left": pos + "px"
			}).addClass('animated owl-animated-out ' + tOut).one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', removeStyles);
		}

		if (tIn) {
			currentItem.addClass('animated owl-animated-in ' + tIn).one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', removeStyles);
		}
	};

	/**
  * destroy
  * @desc Remove Owl structure and events :(
  * @since 2.0.0
  */

	Owl.prototype.destroy = function () {

		window.clearInterval(this.e._autoplay);

		if (this.dom.$el.hasClass(this.options.themeClass)) {
			this.dom.$el.removeClass(this.options.themeClass);
		}

		if (this.options.responsive !== false) {
			this.off(window, 'resize', this.e._resizer);
		}

		if (this.transitionEndVendor) {
			this.off(this.dom.stage, this.transitionEndVendor, this.e._transitionEnd);
		}

		if (this.options.mouseDrag || this.options.touchDrag) {
			this.off(this.dom.stage, this.dragType[0], this.e._onDragStart);
			if (this.options.mouseDrag) {
				this.off(document, this.dragType[3], this.e._onDragStart);
			}
			if (this.options.mouseDrag) {
				this.dom.$stage.off('dragstart', function () {
					return false;
				});
				this.dom.stage.onselectstart = function () {};
			}
		}

		if (this.options.URLhashListener) {
			this.off(window, 'hashchange', this.e._goToHash);
		}

		this.dom.$el.off('next.owl', this.e.next);
		this.dom.$el.off('prev.owl', this.e.prev);
		this.dom.$el.off('goTo.owl', this.e.goTo);
		this.dom.$el.off('jumpTo.owl', this.e.jumpTo);
		this.dom.$el.off('addItem.owl', this.e.addItem);
		this.dom.$el.off('removeItem.owl', this.e.removeItem);
		this.dom.$el.off('refresh.owl', this.e.refresh);
		this.dom.$el.off('autoHeight.owl', this.e.autoHeight);
		this.dom.$el.off('play.owl', this.e.play);
		this.dom.$el.off('stop.owl', this.e.stop);
		this.dom.$el.off('stopVideo.owl', this.e.stop);
		this.dom.$stage.off('click', this.e._playVideo);

		if (this.dom.$cc !== null) {
			this.dom.$cc.remove();
		}
		if (this.dom.$cItems !== null) {
			this.dom.$cItems.remove();
		}
		this.e = null;
		this.dom.$el.data('owlCarousel', null);
		delete this.dom.el.owlCarousel;

		this.dom.$stage.unwrap();
		this.dom.$items.unwrap();
		this.dom.$items.contents().unwrap();
		this.dom = null;
	};

	/**
  * Opertators 
  * @desc Used to calculate RTL
  * @param [a] - Number - left side
  * @param [o] - String - operator 
  * @param [b] - Number - right side
  * @since 2.0.0
  */

	Owl.prototype.op = function (a, o, b) {
		var rtl = this.options.rtl;
		switch (o) {
			case '<':
				return rtl ? a > b : a < b;
			case '>':
				return rtl ? a < b : a > b;
			case '>=':
				return rtl ? a <= b : a >= b;
			case '<=':
				return rtl ? a >= b : a <= b;
			default:
				break;
		}
	};

	/**
  * Opertators 
  * @desc Used to calculate RTL
  * @since 2.0.0
  */

	Owl.prototype.browserSupport = function () {
		this.support3d = isPerspective();

		if (this.support3d) {
			this.transformVendor = isTransform();

			// take transitionend event name by detecting transition
			var endVendors = ['transitionend', 'webkitTransitionEnd', 'transitionend', 'oTransitionEnd'];
			this.transitionEndVendor = endVendors[isTransition()];

			// take vendor name from transform name
			this.vendorName = this.transformVendor.replace(/Transform/i, '');
			this.vendorName = this.vendorName !== '' ? '-' + this.vendorName.toLowerCase() + '-' : '';
		}

		this.state.orientation = window.orientation;
	};

	// Pivate methods

	// CSS detection;
	function isStyleSupported(array) {
		var p,
		    s,
		    fake = document.createElement('div'),
		    list = array;
		for (p in list) {
			s = list[p];
			if (typeof fake.style[s] !== 'undefined') {
				fake = null;
				return [s, p];
			}
		}
		return [false];
	}

	function isTransition() {
		return isStyleSupported(['transition', 'WebkitTransition', 'MozTransition', 'OTransition'])[1];
	}

	function isTransform() {
		return isStyleSupported(['transform', 'WebkitTransform', 'MozTransform', 'OTransform', 'msTransform'])[0];
	}

	function isPerspective() {
		return isStyleSupported(['perspective', 'webkitPerspective', 'MozPerspective', 'OPerspective', 'MsPerspective'])[0];
	}

	function isTouchSupport() {
		return 'ontouchstart' in window || !!navigator.msMaxTouchPoints;
	}

	function isTouchSupportIE() {
		return window.navigator.msPointerEnabled;
	}

	function isRetina() {
		return window.devicePixelRatio > 1;
	}

	$.fn.owlCarousel = function (options) {
		return this.each(function () {
			if (!$(this).data('owlCarousel')) {
				$(this).data('owlCarousel', new Owl(this, options));
			}
		});
	};
})(window.Zepto || window.jQuery, window, document);

//https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Function/bind
//The bind() method creates a new function that, when called, has its this keyword set to the provided value, with a given sequence of arguments preceding any provided when the new function is called.

if (!Function.prototype.bind) {
	Function.prototype.bind = function (oThis) {
		if (typeof this !== 'function') {
			// closest thing possible to the ECMAScript 5 internal IsCallable function
			throw new TypeError('Function.prototype.bind - what is trying to be bound is not callable');
		}

		var aArgs = Array.prototype.slice.call(arguments, 1),
		    fToBind = this,
		    fNOP = function fNOP() {},
		    fBound = function fBound() {
			return fToBind.apply(this instanceof fNOP && oThis ? this : oThis, aArgs.concat(Array.prototype.slice.call(arguments)));
		};
		fNOP.prototype = this.prototype;
		fBound.prototype = new fNOP();
		return fBound;
	};
}

},{}]},{},[1])

//# sourceMappingURL=data:application/json;charset:utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIm5vZGVfbW9kdWxlcy9icm93c2VyaWZ5L25vZGVfbW9kdWxlcy9icm93c2VyLXBhY2svX3ByZWx1ZGUuanMiLCJEOi9JbnNpZ2h0c19Qcm9kdWN0L3dlYi9XZWJzaXRlL2pzL293bENhcm91c2VsLmpzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBOzs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7OztBQ3NEQSxDQUFDLENBQUMsVUFBVyxDQUFDLEVBQUUsTUFBTSxFQUFFLFFBQVEsRUFBRSxTQUFTLEVBQUc7O0FBRTdDLEtBQUksUUFBUSxHQUFHO0FBQ2QsT0FBSyxFQUFLLENBQUM7QUFDWCxNQUFJLEVBQUssS0FBSztBQUNkLFFBQU0sRUFBSyxLQUFLOztBQUVoQixXQUFTLEVBQUksSUFBSTtBQUNqQixXQUFTLEVBQUksSUFBSTtBQUNqQixVQUFRLEVBQUssSUFBSTtBQUNqQixVQUFRLEVBQUksS0FBSzs7QUFFakIsUUFBTSxFQUFLLENBQUM7QUFDWixjQUFZLEVBQUcsQ0FBQzs7QUFFaEIsT0FBSyxFQUFLLEtBQUs7QUFDZixVQUFRLEVBQUksSUFBSTtBQUNoQixXQUFTLEVBQUksS0FBSztBQUNsQixZQUFVLEVBQUksS0FBSzs7QUFFbkIsZUFBYSxFQUFHLENBQUM7QUFDakIsaUJBQWUsRUFBRSxLQUFLOztBQUV0QixLQUFHLEVBQU0sS0FBSztBQUNkLFdBQVMsRUFBSSxJQUFJO0FBQ2pCLFNBQU8sRUFBSyxDQUFDLE1BQU0sRUFBQyxNQUFNLENBQUM7QUFDM0IsU0FBTyxFQUFJLENBQUM7QUFDWixNQUFJLEVBQU0sSUFBSTtBQUNkLFVBQVEsRUFBSSxLQUFLO0FBQ2pCLFNBQU8sRUFBSSxLQUFLOztBQUVoQixVQUFRLEVBQUksS0FBSztBQUNqQixhQUFXLEVBQUcsS0FBSzs7QUFFbkIsVUFBUSxFQUFJLEtBQUs7QUFDakIsaUJBQWUsRUFBRSxJQUFJO0FBQ3JCLG9CQUFrQixFQUFFLEtBQUs7O0FB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QyxDQUFDO0FBQzVDLFFBQUksQ0FBQyxLQUFLLENBQUMsVUFBVSxJQUFJLElBQUksQ0FBQyxLQUFLLENBQUMsSUFBSSxHQUFHLElBQUksQ0FBQyxHQUFHLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxDQUFDO0lBQzlELE1BQU07QUFDTixRQUFJLENBQUMsR0FBRyxDQUFDLE1BQU0sQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUM7SUFDeEI7OztBQUdELE9BQUcsSUFBSSxDQUFDLE9BQU8sQ0FBQyxJQUFJLEVBQUM7QUFDcEIsUUFBRyxDQUFDLElBQUUsSUFBSSxDQUFDLEdBQUcsQ0FBQyxNQUFNLEdBQUMsQ0FBQyxJQUFJLENBQUMsR0FBQyxJQUFJLENBQUMsR0FBRyxDQUFDLE1BQU0sR0FBQyxDQUFDLEdBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxNQUFNLEVBQUM7QUFDOUQsU0FBSSxDQUFDLEdBQUcsQ0FBQyxHQUFHLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxHQUFHLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUM7S0FDdEM7SUFDRCxNQUFNO0FBQ04sUUFBSSxDQUFDLEdBQUcsQ0FBQyxHQUFHLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxHQUFHLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUM7SUFDdEM7O0FBRUQsT0FBSSxNQUFNLEdBQUcsSUFBSSxDQUFDLEtBQUssQ0FBQyxJQUFJLEdBQUcsSUFBSSxDQUFDLEdBQUcsQ0FBQyxNQUFNLENBQUMsQ0FBQyxDQUFDLENBQUM7OztBQUdsRCxPQUFHLElBQUksQ0FBQyxPQUFPLENBQUMsU0FBUyxFQUFDO0FBQ3pCLFVBQU0sR0FBRyxJQUFJLENBQUMsR0FBRyxDQUFDLE1BQU0sQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDLENBQUMsS0FBSyxFQUFFLEdBQUcsSUFBSSxDQUFDLE9BQU8sQ0FBQyxNQUFNLENBQUM7QUFDN0QsUUFBRyxJQUFJLENBQUMsT0FBTyxDQUFDLEdBQUcsRUFBQztBQUNuQixTQUFJLENB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QUFDLEdBQUcsR0FBRyxDQUFDLENBQUM7QUFDakIsTUFBSSxDQUFDLEdBQUcsQ0FBQyxRQUFRLEdBQUcsSUFBSSxDQUFDLEdBQUcsQ0FBQyxLQUFLLENBQUMsT0FBTyxDQUFDLENBQUM7OztBQUc1QyxNQUFHLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBQyxJQUFJLEVBQUM7QUFDckIsT0FBSSxDQUFDLEdBQUcsQ0FBQyxHQUFHLEdBQUcsSUFBSSxDQUFDLEdBQUcsQ0FBQyxNQUFNLEdBQUMsQ0FBQyxDQUFDO0dBQ2pDOztBQUVELE1BQUcsSUFBSSxDQUFDLE9BQU8sQ0FBQyxJQUFJLEVBQUM7QUFDcEIsT0FBSSxDQUFDLEdBQUcsQ0FBQyxHQUFHLEdBQUcsSUFBSSxDQUFDLEdBQUcsQ0FBQyxNQUFNLEdBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBQyxLQUFLLENBQUM7R0FDbEQ7O0FBRUQsTUFBRyxDQUFDLElBQUksQ0FBQyxPQUFPLENBQUMsSUFBSSxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBQyxNQUFNLEVBQUM7QUFDN0MsT0FBSSxDQUFDLEdBQUcsQ0FBQyxHQUFHLEdBQUcsSUFBSSxDQUFDLEdBQUcsQ0FBQyxNQUFNLEdBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBQyxLQUFLLENBQUM7R0FDbEQ7O0FBRUQsTUFBRyxJQUFJLENBQUMsT0FBTyxDQUFDLElBQUksSUFBSSxJQUFJLENBQUMsT0FBTyxDQUFDLE1BQU0sRUFBQztBQUMzQyxPQUFJLENBQUMsR0FBRyxDQUFDLEdBQUcsR0FBRyxJQUFJLENBQUMsR0FBRyxDQUFDLE1BQU0sR0FBQyxJQUFJLENBQUMsT0FBTyxDQUFDLEtBQUssQ0FBQztHQUNsRDs7O0FBR0QsTUFBSSxDQUFDLEdBQUcsQ0FBQyxRQUFRLEdBQUcsSUFBSSxDQUFDLEdBQUcsQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxHQUFHLENBQUMsQ0FBQzs7O0FBR2pELE1BQUcsQUFBQyxDQUFDLElBQUksQ0FBQyxPQUFPLENBQUMsSUFBSSxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBQyxNQUFNLElBQUksSUFBSSxDQUFDLE9BQU8sQ0FBQyxTQUFTLElBQU0sSUFBSSxDQUFDLE9BQU8sQ0FBQyxLQUFLLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFDLE1BQU0sQUFBQyxFQUFFO0FBQzFILE9BQUksTUFBTSxHQUFHLElBQUksQ0FBQyxPQUFPLENBQUMsR0FBRyxHQUFHLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQztBQUN2QyxRQUFLLENBQUMsR0FBRyxDQUFDLEVBQUUsQ0FBQyxHQUFHLElBQUksQ0FBQyxHQUFHLENBQUMsS0FBSyxDQUFDLE1BQU0sRUFBRSxDQUFDLEVBQUUsRUFBRTtBQUMzQyxRQUFJLEFBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxLQUFLLENBQUMsQ0FBQyxDQUFDLEdBQUcsTUFBTSxHQUFJLElBQUksQ0FBQyxLQUFLLENBQUMsS0FBSyxHQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsRUFBRSxFQUFFO0FBQ2xFLFNBQUksQ0FBQyxHQUFHLENBQUMsR0FBRyxHQUFHLENBQUMsR0FBQyxDQUFDLENBQUM7S0FDbkI7SUFDRDtBQUNELE9BQUksQ0FBQyxHQUFHLENBQUMsUUFBUSxHQUFHLElBQUksQ0FBQyxPQUFPLENBQUMsR0FBRyxHQUFHLElBQUksQ0FBQyxLQUFLLENBQUMsS0FBSyxHQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsRUFBRSxHQUFHLEVBQUUsSUFBSSxDQUFDLEtBQUssQ0FBQyxLQUFLLEdBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxFQUFFLENBQUEsQUFBQyxDQUFDO0FBQzFHLE9BQUksQ0FBQyxHQUFHLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQyxHQUFHLENBQUMsR0FBRyxDQUFDLEdBQUcsSUFBSSxDQUFDLEdBQUcsQ0FBQyxRQUFRLENBQUM7R0FDakQ7OztBQUdELE1BQUcsSUFBSSxDQUFDLE9BQU8sQ0FBQyxNQUFNLEVBQUM7QUFDdEIsT0FBSSxDQUFDLEdBQUcsQ0FBQyxJQUFJLEdBQUcsSUFBSSxDQUFDLEdBQUcsQ0FBQyxL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RyxJQUFJLENBQUMsS0FBSyxDQUFDLFdBQVcsS0FBSyxLQUFLLEVBQUM7QUFDbkMsUUFBSSxDQUFDLEdBQUcsQ0FBQyxFQUFFLENBQUMsR0FBQyxJQUFJLENBQUMsR0FBRyxDQUFDLFNBQVMsRUFBRSxDQUFDLEVBQUUsRUFBQzs7QUFFcEMsUUFBRyxJQUFJLENBQUMsS0FBSyxDQUFDLFdBQVcsS0FBSyxPQUFPLEVBQUM7QUFDckMsU0FBSSxHQUFHLElBQUksQ0FBQyxHQUFHLENBQUMsTUFBTSxDQUFDLElBQUksQ0FBQyxXQUFXLENBQUMsQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDLENBQUM7QUFDL0MsU0FBSSxDQUFDLFFBQVEsQ0FBQyxJQUFJLENBQUMsR0FBRyxDQUFDLE1BQU0sQ0FBQyxDQUFDO0tBQy9CO0FBQ0QsUUFBRyxJQUFJLENBQUMsS0FBSyxDQUFDLFdBQVcsS0FBSyxNQUFNLEVBQUM7QUFDcEMsU0FBSSxHQUFHLElBQUksQ0FBQyxHQUFHLENBQUMsTUFBTSxDQUFDLElBQUksQ0FBQyxXQUFXLENBQUMsQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQztBQUNoRCxTQUFJLENBQUMsU0FBUyxDQUFDLElBQUksQ0FBQyxHQUFHLENBQUMsTUFBTSxDQUFDLENBQUM7S0FDaEM7QUFDRCxRQUFJLENBQUMsSUFBSSxDQUFDLFVBQVUsQ0FBQyxDQUFDLE1BQU0sR0FBRyxLQUFLLENBQUM7SUFDckM7R0FDRDs7O0FBR0QsTUFBSSxDQUFDLEdBQUcsQ0FBQyxNQUFNLEdBQUcsSUFBSSxDQUFDLEdBQUcsQ0FBQyxNQUFNLENBQUMsSUFBSSxDQUFDLFdBQVcsQ0FBQyxDQUFDOztBQUVwRCxPQUFJLENBQUMsR0FBRyxDQUFDLEVBQUUsQ0FBQyxHQUFDLElBQUksQ0FBQyxHQUFHLENBQUMsS0FBSyxFQUFFLENBQUMsRUFBRSxFQUFDOztBQUVoQyxPQUFJLENBQUMsR0FBRyxDQUFDLE1BQU0sQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDLENBQUMsV0FBVyxDQUFDLElBQUksQ0FBQyxPQUFPLENB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LFFBQUcsQ0FBQyxHQUFHLElBQUksQ0FBQyxHQUFHLENBQUMsS0FBSyxDQUFDLENBQUMsQ0FBQyxHQUFDLElBQUksSUFBSSxDQUFDLEdBQUcsSUFBSSxDQUFDLEdBQUcsQ0FBQyxLQUFLLENBQUMsQ0FBQyxDQUFDLEdBQUMsSUFBSSxFQUFDO0FBQzNELFNBQUksR0FBRyxDQUFDLENBQUM7S0FDVCxNQUFLLElBQUcsSUFBSSxDQUFDLEVBQUUsQ0FBQyxDQUFDLEVBQUMsR0FBRyxFQUFDLElBQUksQ0FBQyxHQUFHLENBQUMsS0FBSyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksSUFBSSxDQUFDLEVBQUUsQ0FBQyxDQUFDLEVBQUMsR0FBRyxFQUFDLElBQUksQ0FBQyxHQUFHLENBQUMsS0FBSyxDQUFDLENBQUMsR0FBQyxDQUFDLElBQUksSUFBSSxDQUFDLEdBQUcsQ0FBQyxLQUFLLENBQUMsQ0FBQyxDQUFDLEdBQUcsSUFBSSxDQUFDLEtBQUssQ0FBQyxFQUFFLENBQUMsQ0FBQyxFQUFFO0FBQ3JILFNBQUksR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDLFNBQVMsS0FBSyxNQUFNLEdBQUcsQ0FBQyxHQUFDLENBQUMsR0FBRyxDQUFDLENBQUM7S0FDakQ7SUFDRDtHQUNEOztBQUVELE1BQUcsQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFDLElBQUksRUFBQztBQUNyQixPQUFHLElBQUksQ0FBQyxFQUFFLENBQUMsQ0FBQyxFQUFDLEdBQUcsRUFBQyxJQUFJLENBQUMsR0FBRyxDQUFDLFFBQVEsQ0FBQyxFQUFDO0FBQ25DLFFBQUksR0FBRyxDQUFDLEdBQUcsSUFBSSxDQUFDLEdBQUcsQ0FBQyxHQUFHLENBQUM7SUFDeEIsTUFBTSxJQUFHLElBQUksQ0FBQyxFQUFFLENBQUMsQ0FBQyxFQUFDLEdBQUcsRUFBQyxJQUFJLENBQUMsR0FBRyxDQUFDLFFBQVEsQ0FBQyxFQUFDO0FBQzFDLFFBQUksR0FBRyxDQUFDLEdBQUcsSUFBSSxDQUFDLEdBQUcsQ0FBQyxHQUFHLENBQUM7SUFDeEI7R0FDRDs7QUFFRCxNQUFH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QUFDLENBQUM7QUFDekQsTUFBSSxDQUFDLENBQUMsQ0FBQyxJQUFJLEdBQUcsQ0FBQSxVQUFTLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxPQUFJLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDO0dBQUksQ0FBQSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQztBQUN6RCxNQUFJLENBQUMsQ0FBQyxDQUFDLElBQUksR0FBRyxDQUFBLFVBQVMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxPQUFJLENBQUMsSUFBSSxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQztHQUFHLENBQUEsQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLENBQUM7QUFDNUQsTUFBSSxDQUFDLENBQUMsQ0FBQyxNQUFNLEdBQUcsQ0FBQSxVQUFTLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxPQUFJLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxDQUFDO0dBQUcsQ0FBQSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQztBQUM1RCxNQUFJLENBQUMsQ0FBQyxDQUFDLE9BQU8sR0FBRyxDQUFBLFVBQVMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxPQUFJLENBQUMsT0FBTyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQztHQUFFLENBQUEsQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLENBQUM7QUFDakUsTUFBSSxDQUFDLENBQUMsQ0FBQyxVQUFVLEdBQUcsQ0FBQSxVQUFTLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxPQUFJLENBQUMsVUFBVSxDQUFDLENBQUMsQ0FBQyxDQUFDO0dBQUMsQ0FBQSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQztBQUNsRSxNQUFJLENBQUMsQ0FBQyxDQUFDLE9BQU8sR0FBRyxDQUFBLFVBQVMsQ0FBQyxFQUFDO0FBQUMsT0FBSSxDQUFDLE9BQU8sRUFBRSxDQUFDO0dBQUcsQ0FBQSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQztBQUMzRCxNQUFJLENBQUMsQ0FBQyxDQUFDLE9BQU8sR0FBRyxDQUFBLFVBQVMsQ0FBQyxFQUFDO0FBQUMsT0FBSSxDQUFDLE9BQU8sRUFBRSxDQUFDO0dBQUcsQ0FBQSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQztBQUMzRCxNQUFJLENBQUMsQ0FBQyxDQUFDLFVBQVUsR0FBRyxDQUFBLFVBQVMsQ0FBQyxFQUFDO0FBQUMsT0FBSSxDQUFDLFVBQVUsQ0FBQyxJQUFJLENBQUMsQ0FBQztHQUFDLENBQUEsQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLENBQUM7QUFDbkUsTUFBSSxDQUFDLENBQUMsQ0FBQyxJQUFJLEdBQUcsQ0FBQSxZQUFVO0FBQUMsT0FBSSxDQUFDLElBQUksRUFBRSxDQUFDO0dBQUssQ0FBQSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQztBQUN0RCxNQUFJLENBQUMsQ0FBQyxDQUFDLElBQUksR0FBRyxDQUFBLFVBQVMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxPQUFJLENBQUMsSUFBSSxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQztHQUFHLENBQUEsQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLENBQUM7QUFDNUQsTUFBSSxDQUFDLENBQUMsQ0FBQyxhQUFhLEdBQUcsQ0FBQSxVQUFTLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxPQUFJLENBQUMsYUFBYSxDQUFDLENBQUMsQ0FBQyxDQUFDO0dBQUUsQ0FBQSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQzs7QUFFekUsTUFBSSxDQUFDLEdBQUcsQ0FBQyxHQUFHLENBQUMsRUFBRSxDQUFDLFVBQVUsRUFBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDO0FBQ3hDLE1BQUksQ0FBQyxHQUFHLENBQUMsR0FBRyxDQUFDLEVBQUUsQ0FBQyxVQUFVLEVBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQztBQUN4QyxNQUFJLENBQUMsR0FBRyxD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QUFLLENBQUMsSUFBSSxDQUFDLFNBQVMsQ0FBQyxDQUFDLENBQUMsQ0FBQztHQUN4SCxDQUFDO0FBQ0gsTUFBSSxDQUFDLFNBQVMsR0FBRyxJQUFJLENBQUMsU0FBUyxDQUFDO0FBQ2hDLFFBQU0sQ0FBQyxTQUFTLEdBQUcsSUFBSSxJQUFJLEVBQUUsQ0FBQztBQUM5QixTQUFPLE1BQU0sQ0FBQztFQUNaLENBQUM7Q0FDSCIsImZpbGUiOiJnZW5lcmF0ZWQuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlc0NvbnRlbnQiOlsiKGZ1bmN0aW9uIGUodCxuLHIpe2Z1bmN0aW9uIHMobyx1KXtpZighbltvXSl7aWYoIXRbb10pe3ZhciBhPXR5cGVvZiByZXF1aXJlPT1cImZ1bmN0aW9uXCImJnJlcXVpcmU7aWYoIXUmJmEpcmV0dXJuIGEobywhMCk7aWYoaSlyZXR1cm4gaShvLCEwKTt2YXIgZj1uZXcgRXJyb3IoXCJDYW5ub3QgZmluZCBtb2R1bGUgJ1wiK28rXCInXCIpO3Rocm93IGYuY29kZT1cIk1PRFVMRV9OT1RfRk9VTkRcIixmfXZhciBsPW5bb109e2V4cG9ydHM6e319O3Rbb11bMF0uY2FsbChsLmV4cG9ydHMsZnVuY3Rpb24oZSl7dmFyIG49dFtvXVsxXVtlXTtyZXR1cm4gcyhuP246ZSl9LGwsbC5leHBvcnRzLGUsdCxuLHIpfXJldHVybiBuW29dLmV4cG9ydHN9dmFyIGk9dHlwZW9mIHJlcXVpcmU9PVwiZnVuY3Rpb25cIiYmcmVxdWlyZTtmb3IodmFyIG89MDtvPHIubGVuZ3RoO28rKylzKHJbb10pO3JldHVybiBzfSkiLCIvKipcclxuICogQG5hbWUgT3dsIENhcm91c2VsIC0gY29kZSBuYW1lIFBoZW5peFxyXG4gKiBAYXV0aG9yIEJhcnRvc3ogV29qY2llY2hvd3NraVxyXG4gKiBAcmVsZWFzZSAyMDE0XHJcbiAqIExpY2Vuc2VkIHVuZGVyIE1JVFxyXG4gKiBcclxuICogQHZlcnNpb24gMi4wLjAtYmV0YS4xLjhcclxuICogQHZlcnNpb25Ob3RlcyBOb3QgY29tcGF0aWJpbGUgd2l0aCBPd2wgQ2Fyb3VzZWwgPDIuMC4wXHJcbiAqL1xyXG5cclxuLypcclxuXHJcbnswLDB9XHJcbiApXylcclxuIFwiXCJcclxuXHJcblRvIGRvOlxyXG5cclxuKiBMYXp5IExvYWQgSWNvblxyXG4qIHByZXZlbnQgYW5pbWF0aW9uZW5kIGJ1YmxpbmdcclxuKiBpdGVtc1NjYWxlVXAgXHJcbiogVGVzdCBaZXB0b1xyXG5cclxuQ2FsbGJhY2sgZXZlbnRzIGxpc3Q6XHJcblxyXG5vbkluaXRCZWZvcmVcclxub25Jbml0QWZ0ZXJcclxub25SZXNwb25zaXZlQmVmb3JlXHJcbm9uUmVzcG9uc2l2ZUFmdGVyXHJcbm9uVHJhbnNpdGlvblN0YXJ0XHJcbm9uVHJhbnNpdGlvbkVuZFxyXG5vblRvdWNoU3RhcnRcclxub25Ub3VjaEVuZFxyXG5vbkNoYW5nZVN0YXRlXHJcbm9uTGF6eUxvYWRlZFxyXG5vblZpZGVvUGxheVxyXG5vblZpZGVvU3RvcFxyXG5cclxuQ3VzdG9tIGV2ZW50cyBsaXN0OlxyXG5cclxubmV4dC5vd2xcclxucHJldi5vd2xcclxuZ29Uby5vd2xcclxuanVtcFRvLm93bFxyXG5hZGRJdGVtLm93bFxyXG5yZW1vdmVJdGVtLm93bFxyXG5yZWZyZXNoLm93bFxyXG5wbGF5Lm93bFxyXG5zdG9wLm93bFxyXG5zdG9wVmlkZW8ub3dsXHJcblxyXG4qL1xyXG5cclxuXHJcbjsoZnVuY3Rpb24gKCAkLCB3aW5kb3csIGRvY3VtZW50LCB1bmRlZmluZWQgKSB7XHJcblxyXG5cdHZhciBkZWZhdWx0cyA9IHtcclxuXHRcdGl0ZW1zOlx0XHRcdFx0MyxcclxuXHRcdGxvb3A6XHRcdFx0XHRmYWxzZSxcclxuXHRcdGNlbnRlcjpcdFx0XHRcdGZhbHNlLFxyXG5cclxuXHRcdG1vdXNlRHJhZzpcdFx0XHR0cnVlLFxyXG5cdFx0dG91Y2hEcmFnOlx0XHRcdHRydWUsXHJcblx0XHRwdWxsRHJhZzogXHRcdFx0dHJ1ZSxcclxuXHRcdGZyZWVEcmFnOlx0XHRcdGZhbHNlLFxyXG5cclxuXHRcdG1hcmdpbjpcdFx0XHRcdDAsXHJcblx0XHRzdGFnZVBhZGRpbmc6XHRcdDAsXHJcblxyXG5cdFx0bWVyZ2U6XHRcdFx0XHRmYWxzZSxcclxuXHRcdG1lcmdlRml0Olx0XHRcdHRydWUsXHJcblx0XHRhdXRvV2lkdGg6XHRcdFx0ZmFsc2UsXHJcblx0XHRhdXRvSGVpZ2h0Olx0XHRcdGZhbHNlLFxyXG5cclxuXHRcdHN0YXJ0UG9zaXRpb246XHRcdDAsXHJcblx0XHRVUkxoYXNoTGlzdGVuZXI6XHRmYWxzZSxcclxuXHJcblx0XHRuYXY6IFx0XHRcdFx0ZmFsc2UsXHJcblx0XHRuYXZSZXdpbmQ6XHRcdFx0dHJ1ZSxcclxuXHRcdG5hdlRleHQ6IFx0XHRcdFsncHJldicsJ25leHQnXSxcclxuXHRcdHNsaWRlQnk6XHRcdFx0MSxcclxuXHRcdGRvdHM6IFx0XHRcdFx0dHJ1ZSxcclxuXHRcdGRvdHNFYWNoOlx0XHRcdGZhbHNlLFxyXG5cdFx0ZG90RGF0YTpcdFx0XHRmYWxzZSxcclxuXHJcblx0XHRsYXp5TG9hZDpcdFx0XHRmYWxzZSxcclxuXHRcdGxhenlDb250ZW50Olx0XHRmYWxzZSxcclxuXHJcblx0XHRhdXRvcGxheTpcdFx0XHRmYWxzZSxcclxuXHRcdGF1dG9wbGF5VGltZW91dDpcdDUwMDAsXHJcblx0XHRhdXRvcGxheUhvdmVyUGF1c2U6XHRmYWxzZSxcclxuXHJcblx0XHRzbWFydFNwZWVkOlx0XHRcdDI1MCxcclxuXHRcdGZsdWlkU3BlZWQ6XHRcdFx0ZmFsc2UsXHJcblx0XHRhdXRvcGxheVNwZWVkOlx0XHRmYWxzZSxcclxuXHRcdG5hdlNwZWVkOlx0XHRcdGZhbHNlLFxyXG5cdFx0ZG90c1NwZWVkOlx0XHRcdGZhbHNlLFxyXG5cdFx0ZHJhZ0VuZFNwZWVkOlx0XHRmYWxzZSxcclxuXHRcdFxyXG5cdFx0cmVzcG9uc2l2ZTogXHRcdHt9LFxyXG5cdFx0cmVzcG9uc2l2ZVJlZnJlc2hSYXRlIDogMjAwLFxyXG5cdFx0cmVzcG9uc2l2ZUJhc2VFbGVtZW50OiB3aW5kb3csXHJcblx0XHRyZXNwb25zaXZlQ2xhc3M6XHRmYWxzZSxcclxuXHJcblx0XHR2aWRlbzpcdFx0XHRcdGZhbHNlLFxyXG5cdFx0dmlkZW9IZWlnaHQ6XHRcdGZhbHNlLFxyXG5cdFx0dmlkZW9XaWR0aDpcdFx0XHRmYWxzZSxcclxuXHJcblx0XHRhbmltYXRlT3V0Olx0XHRcdGZhbHNlLFxyXG5cdFx0YW5pbWF0ZUluOlx0XHRcdGZhbHNlLFxyXG5cclxuXHRcdGZhbGxiYWNrRWFzaW5nOlx0XHQnc3dpbmcnLFxyXG5cclxuXHRcdGNhbGxiYWNrczpcdFx0XHRmYWxzZSxcclxuXHRcdGluZm86IFx0XHRcdFx0ZmFsc2UsXHJcblxyXG5cdFx0bmVzdGVkSXRlbVNlbGVjdG9yOlx0ZmFsc2UsXHJcblx0XHRpdGVtRWxlbWVudDpcdFx0J2RpdicsXHJcblx0XHRzdGFnZUVsZW1lbnQ6XHRcdCdkaXYnLFxyXG5cclxuXHRcdC8vQ2xhc3NlcyBhbmQgTmFtZXNcclxuXHRcdHRoZW1lQ2xhc3M6IFx0XHQnb3dsLXRoZW1lJyxcclxuXHRcdGJhc2VDbGFzczpcdFx0XHQnb3dsLWNhcm91c2VsJyxcclxuXHRcdGl0ZW1DbGFzczpcdFx0XHQnb3dsLWl0ZW0nLFxyXG5cdFx0Y2VudGVyQ2xhc3M6XHRcdCdjZW50ZXInLFxyXG5cdFx0YWN0aXZlQ2xhc3M6IFx0XHQnYWN0aXZlJyxcclxuXHRcdG5hdkNvbnRhaW5lckNsYXNzOlx0J293bC1uYXYnLFxyXG5cdFx0bmF2Q2xhc3M6XHRcdFx0Wydvd2wtcHJldicsJ293bC1uZXh0J10sXHJcblx0XHRjb250cm9sc0NsYXNzOlx0XHQnb3dsLWNvbnRyb2xzJyxcclxuXHRcdGRvdENsYXNzOiBcdFx0XHQnb3dsLWRvdCcsXHJcblx0XHRkb3RzQ2xhc3M6XHRcdFx0J293bC1kb3RzJyxcclxuXHRcdGF1dG9IZWlnaHRDbGFzczpcdCdvd2wtaGVpZ2h0J1xyXG5cclxuXHR9O1xyXG5cclxuXHQvLyBSZWZlcmVuY2UgdG8gRE9NIGVsZW1lbnRzXHJcblx0Ly8gVGhvc2Ugd2l0aCAkIHNpZ24gYXJlIGpRdWVyeSBvYmplY3RzXHJcblxyXG5cdHZhciBkb20gPSB7XHJcblx0XHRlbDpcdFx0XHRudWxsLFx0Ly8gbWFpbiBlbGVtZW50IFxyXG5cdFx0JGVsOlx0XHRudWxsLFx0Ly8galF1ZXJ5IG1haW4gZWxlbWVudCBcclxuXHRcdHN0YWdlOlx0XHRudWxsLFx0Ly8gc3RhZ2VcclxuXHRcdCRzdGFnZTpcdFx0bnVsbCxcdC8vIGpRdWVyeSBzdGFnZVxyXG5cdFx0b1N0YWdlOlx0XHRudWxsLFx0Ly8gb3V0ZXIgc3RhZ2VcclxuXHRcdCRvU3RhZ2U6XHRudWxsLFx0Ly8gJCBvdXRlciBzdGFnZVxyXG5cdFx0JGl0ZW1zOlx0XHRudWxsLFx0Ly8gYWxsIGl0ZW1zLCBjbG9uZXMgYW5kIG9yaWdpbmFscyBpbmNsdWRlZCBcclxuXHRcdCRvSXRlbXM6XHRudWxsLFx0Ly8gb3JpZ2luYWwgaXRlbXNcclxuXHRcdCRjSXRlbXM6XHRudWxsLFx0Ly8gY2xvbmVkIGl0ZW1zIG9ubHlcclxuXHRcdCRjYzpcdFx0bnVsbCxcclxuXHRcdCRuYXZQcmV2Olx0bnVsbCxcclxuXHRcdCRuYXZOZXh0Olx0bnVsbCxcclxuXHRcdCRwYWdlOlx0XHRudWxsLFxyXG5cdFx0JG5hdjpcdFx0bnVsbCxcclxuXHRcdCRjb250ZW50Olx0bnVsbFxyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIFZhcmlhYmxlc1xyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqL1xyXG5cclxuXHQvLyBPbmx5IGZvciBkZXZlbG9wbWVudCBwcm9jZXNzXHJcblxyXG5cdC8vIFdpZHRoc1xyXG5cclxuXHR2YXIgd2lkdGggPSB7XHJcblx0XHRlbDpcdFx0XHQwLFxyXG5cdFx0c3RhZ2U6XHRcdDAsXHJcblx0XHRpdGVtOlx0XHQwLFxyXG5cdFx0cHJldldpbmRvdzpcdDAsXHJcblx0XHRjbG9uZUxhc3Q6ICAwXHJcblx0fTtcclxuXHJcblx0Ly8gTnVtYmVyc1xyXG5cclxuXHR2YXIgbnVtID0ge1xyXG5cdFx0aXRlbXM6XHRcdFx0XHQwLFxyXG5cdFx0b0l0ZW1zOiBcdFx0XHQwLFxyXG5cdFx0Y0l0ZW1zOlx0XHRcdFx0MCxcclxuXHRcdGFjdGl2ZTpcdFx0XHRcdDAsXHJcblx0XHRtZXJnZWQ6XHRcdFx0XHRbXSxcclxuXHRcdG5hdjpcdFx0XHRcdFtdLFxyXG5cdFx0YWxsUGFnZXM6XHRcdFx0MFxyXG5cdH07XHJcblxyXG5cdC8vIFBvc2l0aW9uc1xyXG5cclxuXHR2YXIgcG9zID0ge1xyXG5cdFx0c3RhcnQ6XHRcdDAsXHJcblx0XHRtYXg6XHRcdDAsXHJcblx0XHRtYXhWYWx1ZTpcdDAsXHJcblx0XHRwcmV2Olx0XHQwLFxyXG5cdFx0Y3VycmVudDpcdDAsXHJcblx0XHRjdXJyZW50QWJzOlx0MCxcclxuXHRcdGN1cnJlbnRQYWdlOjAsXHJcblx0XHRzdGFnZTpcdFx0MCxcclxuXHRcdGl0ZW1zOlx0XHRbXSxcclxuXHRcdGxzQ3VycmVudDpcdDBcclxuXHR9O1xyXG5cclxuXHQvLyBEcmFnL1RvdWNoZXNcclxuXHJcblx0dmFyIGRyYWcgPSB7XHJcblx0XHRzdGFydDpcdFx0MCxcclxuXHRcdHN0YXJ0WDpcdFx0MCxcclxuXHRcdHN0YXJ0WTpcdFx0MCxcclxuXHRcdGN1cnJlbnQ6XHQwLFxyXG5cdFx0Y3VycmVudFg6XHQwLFxyXG5cdFx0Y3VycmVudFk6XHQwLFxyXG5cdFx0b2Zmc2V0WDpcdDAsXHJcblx0XHRvZmZzZXRZOlx0MCxcclxuXHRcdGRpc3RhbmNlOlx0bnVsbCxcclxuXHRcdHN0YXJ0VGltZTpcdDAsXHJcblx0XHRlbmRUaW1lOlx0MCxcclxuXHRcdHVwZGF0ZWRYOlx0MCxcclxuXHRcdHRhcmdldEVsOlx0bnVsbFxyXG5cdH07XHJcblxyXG5cdC8vIFNwZWVkc1xyXG5cclxuXHR2YXIgc3BlZWQgPSB7XHJcblx0XHRvbkRyYWdFbmQ6IFx0MzAwLFxyXG5cdFx0bmF2Olx0XHQzMDAsXHJcblx0XHRjc3Myc3BlZWQ6XHQwXHJcblxyXG5cdH07XHJcblxyXG5cdC8vIFN0YXRlc1xyXG5cclxuXHR2YXIgc3RhdGUgPSB7XHJcblx0XHRpc1RvdWNoOlx0XHRmYWxzZSxcclxuXHRcdGlzU2Nyb2xsaW5nOlx0ZmFsc2UsXHJcblx0XHRpc1N3aXBpbmc6XHRcdGZhbHNlLFxyXG5cdFx0ZGlyZWN0aW9uOlx0XHRmYWxzZSxcclxuXHRcdGluTW90aW9uOlx0XHRmYWxzZSxcclxuXHRcdGF1dG9wbGF5Olx0XHRmYWxzZSxcclxuXHRcdGxhenlDb250ZW50Olx0ZmFsc2VcclxuXHR9O1xyXG5cclxuXHQvLyBFdmVudCBmdW5jdGlvbnMgcmVmZXJlbmNlc1xyXG5cclxuXHR2YXIgZSA9IHtcclxuXHRcdF9vbkRyYWdTdGFydDpcdG51bGwsXHJcblx0XHRfb25EcmFnTW92ZTpcdG51bGwsXHJcblx0XHRfb25EcmFnRW5kOlx0XHRudWxsLFxyXG5cdFx0X3RyYW5zaXRpb25FbmQ6IG51bGwsXHJcblx0XHRfcmVzaXplcjpcdFx0bnVsbCxcclxuXHRcdF9yZXNwb25zaXZlQ2FsbDpudWxsLFxyXG5cdFx0X2dvVG9Mb29wOlx0XHRudWxsLFxyXG5cdFx0X2NoZWNrVmlzaWJpbGU6IG51bGwsXHJcblx0XHRfYXV0b3BsYXk6XHRcdG51bGwsXHJcblx0XHRfcGF1c2U6XHRcdFx0bnVsbCxcclxuXHRcdF9wbGF5Olx0XHRcdG51bGwsXHJcblx0XHRfc3RvcDpcdFx0XHRudWxsXHJcblx0fTtcclxuXHJcblx0ZnVuY3Rpb24gT3dsKCBlbGVtZW50LCBvcHRpb25zICkge1xyXG5cclxuXHRcdC8vIGFkZCBiYXNpYyBPd2wgaW5mb3JtYXRpb24gdG8gZG9tIGVsZW1lbnRcclxuXHJcblx0XHRlbGVtZW50Lm93bENhcm91c2VsID0ge1xyXG5cdFx0XHQnbmFtZSc6XHRcdCdPd2wgQ2Fyb3VzZWwnLFxyXG5cdFx0XHQnYXV0aG9yJzpcdCdCYXJ0b3N6IFdvamNpZWNob3dza2knLFxyXG5cdFx0XHQndmVyc2lvbic6XHQnMi4wLjAtYmV0YS4xLjgnLFxyXG5cdFx0XHQncmVsZWFzZWQnOlx0JzAzLjA1LjIwMTQnXHJcblx0XHR9O1xyXG5cclxuXHRcdC8vIEF0dGFjaCB2YXJpYWJsZXMgdG8gb2JqZWN0XHJcblx0XHQvLyBPbmx5IGZvciBkZXZlbG9wbWVudCBwcm9jZXNzXHJcblxyXG5cdFx0dGhpcy5vcHRpb25zID0gXHRcdCQuZXh0ZW5kKCB7fSwgZGVmYXVsdHMsIG9wdGlvbnMpO1xyXG5cdFx0dGhpcy5fb3B0aW9ucyA9XHRcdCQuZXh0ZW5kKCB7fSwgZGVmYXVsdHMsIG9wdGlvbnMpO1xyXG5cdFx0dGhpcy5kb20gPVx0XHRcdCQuZXh0ZW5kKCB7fSwgZG9tKTtcclxuXHRcdHRoaXMud2lkdGggPVx0XHQkLmV4dGVuZCgge30sIHdpZHRoKTtcclxuXHRcdHRoaXMubnVtID1cdFx0XHQkLmV4dGVuZCgge30sIG51bSk7XHJcblx0XHR0aGlzLnBvcyA9XHRcdFx0JC5leHRlbmQoIHt9LCBwb3MpO1xyXG5cdFx0dGhpcy5kcmFnID1cdFx0XHQkLmV4dGVuZCgge30sIGRyYWcpO1xyXG5cdFx0dGhpcy5zcGVlZCA9XHRcdCQuZXh0ZW5kKCB7fSwgc3BlZWQpO1xyXG5cdFx0dGhpcy5zdGF0ZSA9XHRcdCQuZXh0ZW5kKCB7fSwgc3RhdGUpO1xyXG5cdFx0dGhpcy5lID1cdFx0XHQkLmV4dGVuZCgge30sIGUpO1xyXG5cclxuXHRcdHRoaXMuZG9tLmVsID1cdFx0ZWxlbWVudDtcclxuXHRcdHRoaXMuZG9tLiRlbCA9XHRcdCQoZWxlbWVudCk7XHJcblx0XHR0aGlzLmluaXQoKTtcclxuXHR9XHJcblxyXG5cdC8qKlxyXG5cdCAqIGluaXRcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS5pbml0ID0gZnVuY3Rpb24oKXtcclxuXHJcblx0XHR0aGlzLmZpcmVDYWxsYmFjaygnb25Jbml0QmVmb3JlJyk7XHJcblxyXG5cdFx0Ly9BZGQgYmFzZSBjbGFzc1xyXG5cdFx0aWYoIXRoaXMuZG9tLiRlbC5oYXNDbGFzcyh0aGlzLm9wdGlvbnMuYmFzZUNsYXNzKSl7XHJcblx0XHRcdHRoaXMuZG9tLiRlbC5hZGRDbGFzcyh0aGlzLm9wdGlvbnMuYmFzZUNsYXNzKTtcclxuXHRcdH1cclxuXHJcblx0XHQvL0FkZCB0aGVtZSBjbGFzc1xyXG5cdFx0aWYoIXRoaXMuZG9tLiRlbC5oYXNDbGFzcyh0aGlzLm9wdGlvbnMudGhlbWVDbGFzcykpe1xyXG5cdFx0XHR0aGlzLmRvbS4kZWwuYWRkQ2xhc3ModGhpcy5vcHRpb25zLnRoZW1lQ2xhc3MpO1xyXG5cdFx0fVxyXG5cclxuXHRcdC8vQWRkIHRoZW1lIGNsYXNzXHJcblx0XHRpZih0aGlzLm9wdGlvbnMucnRsKXtcclxuXHRcdFx0dGhpcy5kb20uJGVsLmFkZENsYXNzKCdvd2wtcnRsJyk7XHJcblx0XHR9XHJcblxyXG5cdFx0Ly8gQ2hlY2sgc3VwcG9ydFxyXG5cdFx0dGhpcy5icm93c2VyU3VwcG9ydCgpO1xyXG5cclxuXHRcdC8vIFNvcnQgcmVzcG9uc2l2ZSBpdGVtcyBpbiBhcnJheVxyXG5cdFx0dGhpcy5zb3J0T3B0aW9ucygpO1xyXG5cclxuXHRcdC8vIFVwZGF0ZSBvcHRpb25zLml0ZW1zIG9uIGdpdmVuIHNpemVcclxuXHRcdHRoaXMuc2V0UmVzcG9uc2l2ZU9wdGlvbnMoKTtcclxuXHJcblx0XHRpZih0aGlzLm9wdGlvbnMuYXV0b1dpZHRoICYmIHRoaXMuc3RhdGUuaW1hZ2VzTG9hZGVkICE9PSB0cnVlKXtcclxuXHRcdFx0dmFyIGltZ3MgPSB0aGlzLmRvbS4kZWwuZmluZCgnaW1nJyk7XHJcblx0XHRcdGlmKGltZ3MubGVuZ3RoKXtcclxuXHRcdFx0XHR0aGlzLnByZWxvYWRBdXRvV2lkdGhJbWFnZXMoaW1ncyk7XHJcblx0XHRcdFx0cmV0dXJuIGZhbHNlO1xyXG5cdFx0XHR9XHJcblx0XHR9XHJcblxyXG5cdFx0Ly8gR2V0IGFuZCBzdG9yZSB3aW5kb3cgd2lkdGhcclxuXHRcdC8vIGlPUyBzYWZhcmkgbGlrZXMgdG8gdHJpZ2dlciB1bm5lY2Vzc2FyeSByZXNpemUgZXZlbnRcclxuXHRcdHRoaXMud2lkdGgucHJldldpbmRvdyA9IHRoaXMud2luZG93V2lkdGgoKTtcclxuXHJcblx0XHQvLyBjcmVhdGUgc3RhZ2Ugb2JqZWN0XHJcblx0XHR0aGlzLmNyZWF0ZVN0YWdlKCk7XHJcblxyXG5cdFx0Ly8gQXBwZW5kIGxvY2FsIGNvbnRlbnQgXHJcblx0XHR0aGlzLmZldGNoQ29udGVudCgpO1xyXG5cclxuXHRcdC8vIGF0dGFjaCBnZW5lcmljIGV2ZW50cyBcclxuXHRcdHRoaXMuZXZlbnRzQ2FsbCgpO1xyXG5cclxuXHRcdC8vIGF0dGFjaCBjdXN0b20gY29udHJvbCBldmVudHNcclxuXHRcdHRoaXMuYWRkQ3VzdG9tRXZlbnRzKCk7XHJcblxyXG5cdFx0Ly8gYXR0YWNoIGdlbmVyaWMgZXZlbnRzIFxyXG5cdFx0dGhpcy5pbnRlcm5hbEV2ZW50cygpO1xyXG5cclxuXHRcdHRoaXMuZG9tLiRlbC5hZGRDbGFzcygnb3dsLWxvYWRpbmcnKTtcclxuXHRcdHRoaXMucmVmcmVzaCh0cnVlKTtcclxuXHRcdHRoaXMuZG9tLiRlbC5yZW1vdmVDbGFzcygnb3dsLWxvYWRpbmcnKS5hZGRDbGFzcygnb3dsLWxvYWRlZCcpO1xyXG5cdFx0dGhpcy5maXJlQ2FsbGJhY2soJ29uSW5pdEFmdGVyJyk7XHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICogc29ydE9wdGlvbnNcclxuXHQgKiBAZGVzYyBTb3J0IHJlc3BvbnNpdmUgc2l6ZXMgXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUuc29ydE9wdGlvbnMgPSBmdW5jdGlvbigpe1xyXG5cclxuXHRcdHZhciByZXNPcHQgPSB0aGlzLm9wdGlvbnMucmVzcG9uc2l2ZTtcclxuXHRcdHRoaXMucmVzcG9uc2l2ZVNvcnRlZCA9IHt9O1xyXG5cdFx0dmFyIGtleXMgPSBbXSxcclxuXHRcdGksIGosIGs7XHJcblx0XHRmb3IgKGkgaW4gcmVzT3B0KXtcclxuXHRcdFx0a2V5cy5wdXNoKGkpO1xyXG5cdFx0fVxyXG5cclxuXHRcdGtleXMgPSBrZXlzLnNvcnQoZnVuY3Rpb24gKGEsIGIpIHtyZXR1cm4gYSAtIGI7fSk7XHJcblxyXG5cdFx0Zm9yIChqID0gMDsgaiA8IGtleXMubGVuZ3RoOyBqKyspe1xyXG5cdFx0XHRrID0ga2V5c1tqXTtcclxuXHRcdFx0dGhpcy5yZXNwb25zaXZlU29ydGVkW2tdID0gcmVzT3B0W2tdO1xyXG5cdFx0fVxyXG5cclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiBzZXRSZXNwb25zaXZlT3B0aW9uc1xyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqL1xyXG5cclxuXHRPd2wucHJvdG90eXBlLnNldFJlc3BvbnNpdmVPcHRpb25zID0gZnVuY3Rpb24oKXtcclxuXHRcdGlmKHRoaXMub3B0aW9ucy5yZXNwb25zaXZlID09PSBmYWxzZSl7cmV0dXJuIGZhbHNlO31cclxuXHJcblx0XHR2YXIgd2lkdGggPSB0aGlzLndpbmRvd1dpZHRoKCk7XHJcblx0XHR2YXIgcmVzT3B0ID0gdGhpcy5vcHRpb25zLnJlc3BvbnNpdmU7XHJcblx0XHR2YXIgaSxqLGssIG1pbldpZHRoO1xyXG5cclxuXHRcdC8vIG92ZXJ3cml0ZSBub24gcmVzcG9zbml2ZSBvcHRpb25zXHJcblx0XHRmb3IoayBpbiB0aGlzLl9vcHRpb25zKXtcclxuXHRcdFx0aWYoayAhPT0gJ3Jlc3BvbnNpdmUnKXtcclxuXHRcdFx0XHR0aGlzLm9wdGlvbnNba10gPSB0aGlzLl9vcHRpb25zW2tdO1xyXG5cdFx0XHR9XHJcblx0XHR9XHJcblxyXG5cdFx0Ly8gZmluZCByZXNwb25zaXZlIHdpZHRoXHJcblx0XHRmb3IgKGkgaW4gdGhpcy5yZXNwb25zaXZlU29ydGVkKXtcclxuXHRcdFx0aWYoaTw9IHdpZHRoKXtcclxuXHRcdFx0XHRtaW5XaWR0aCA9IGk7XHJcblx0XHRcdFx0Ly8gc2V0IHJlc3BvbnNpdmUgb3B0aW9uc1xyXG5cdFx0XHRcdGZvcihqIGluIHRoaXMucmVzcG9uc2l2ZVNvcnRlZFttaW5XaWR0aF0pe1xyXG5cdFx0XHRcdFx0dGhpcy5vcHRpb25zW2pdID0gdGhpcy5yZXNwb25zaXZlU29ydGVkW21pbldpZHRoXVtqXTtcclxuXHRcdFx0XHR9XHJcblx0XHRcdFx0XHJcblx0XHRcdH1cclxuXHRcdH1cclxuXHRcdHRoaXMubnVtLmJyZWFrcG9pbnQgPSBtaW5XaWR0aDtcclxuXHJcblx0XHQvLyBSZXNwb25zaXZlIENsYXNzXHJcblx0XHRpZih0aGlzLm9wdGlvbnMucmVzcG9uc2l2ZUNsYXNzKXtcclxuXHRcdFx0dGhpcy5kb20uJGVsLmF0dHIoJ2NsYXNzJyxcclxuXHRcdFx0XHRmdW5jdGlvbihpLCBjKXtcclxuXHRcdFx0XHRyZXR1cm4gYy5yZXBsYWNlKC9cXGIgb3dsLXJlc3BvbnNpdmUtXFxTKy9nLCAnJyk7XHJcblx0XHRcdH0pLmFkZENsYXNzKCdvd2wtcmVzcG9uc2l2ZS0nK21pbldpZHRoKTtcclxuXHRcdH1cclxuXHJcblxyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIG9wdGlvbnNMb2dpY1xyXG5cdCAqIEBkZXNjIFVwZGF0ZSBvcHRpb24gbG9naWMgaWYgbmVjZXNzZXJ5XHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUub3B0aW9uc0xvZ2ljID0gZnVuY3Rpb24oKXtcclxuXHRcdC8vIFRvZ2dsZSBDZW50ZXIgY2xhc3NcclxuXHRcdHRoaXMuZG9tLiRlbC50b2dnbGVDbGFzcygnb3dsLWNlbnRlcicsdGhpcy5vcHRpb25zLmNlbnRlcik7XHJcblxyXG5cdFx0Ly8gU2Nyb2xsIHBlciAtICdwYWdlJyBvcHRpb24gd2lsbCBzY3JvbGwgcGVyIHZpc2libGUgaXRlbXMgbnVtYmVyXHJcblx0XHQvLyBZb3UgY2FuIHNldCB0aGlzIHRvIGFueSBvdGhlciBudW1iZXIgYmVsb3cgdmlzaWJsZSBpdGVtcy5cclxuXHRcdGlmKHRoaXMub3B0aW9ucy5zbGlkZUJ5ICYmIHRoaXMub3B0aW9ucy5zbGlkZUJ5ID09PSAncGFnZScpe1xyXG5cdFx0XHR0aGlzLm9wdGlvbnMuc2xpZGVCeSA9IHRoaXMub3B0aW9ucy5pdGVtcztcclxuXHRcdH0gZWxzZSBpZih0aGlzLm9wdGlvbnMuc2xpZGVCeSA+IHRoaXMub3B0aW9ucy5pdGVtcyl7XHJcblx0XHRcdHRoaXMub3B0aW9ucy5zbGlkZUJ5ID0gdGhpcy5vcHRpb25zLml0ZW1zO1xyXG5cdFx0fVxyXG5cclxuXHRcdC8vIGlmIGl0ZW1zIG51bWJlciBpcyBsZXNzIHRoYW4gaW4gYm9keVxyXG5cdFx0aWYodGhpcy5vcHRpb25zLmxvb3AgJiYgdGhpcy5udW0ub0l0ZW1zIDwgdGhpcy5vcHRpb25zLml0ZW1zKXtcclxuXHRcdFx0dGhpcy5vcHRpb25zLmxvb3AgPSBmYWxzZTtcclxuXHRcdH1cclxuXHJcblx0XHRpZih0aGlzLm51bS5vSXRlbXMgPD0gdGhpcy5vcHRpb25zLml0ZW1zKXtcclxuXHRcdFx0dGhpcy5vcHRpb25zLm5hdlJld2luZCA9IGZhbHNlO1xyXG5cdFx0fVxyXG5cclxuXHRcdGlmKHRoaXMub3B0aW9ucy5hdXRvV2lkdGgpe1xyXG5cdFx0XHR0aGlzLm9wdGlvbnMuc3RhZ2VQYWRkaW5nID0gZmFsc2U7XHJcblx0XHRcdHRoaXMub3B0aW9ucy5kb3RzRWFjaCA9IDE7XHJcblx0XHRcdHRoaXMub3B0aW9ucy5tZXJnZSA9IGZhbHNlO1xyXG5cdFx0fVxyXG5cdFx0aWYodGhpcy5zdGF0ZS5sYXp5Q29udGVudCl7XHJcblx0XHRcdHRoaXMub3B0aW9ucy5sb29wID0gZmFsc2U7XHJcblx0XHRcdHRoaXMub3B0aW9ucy5tZXJnZSA9IGZhbHNlO1xyXG5cdFx0XHR0aGlzLm9wdGlvbnMuZG90cyA9IGZhbHNlO1xyXG5cdFx0XHR0aGlzLm9wdGlvbnMuZnJlZURyYWcgPSBmYWxzZTtcclxuXHRcdFx0dGhpcy5vcHRpb25zLmxhenlDb250ZW50ID0gdHJ1ZTtcclxuXHRcdH1cclxuXHJcblx0XHRpZigodGhpcy5vcHRpb25zLmFuaW1hdGVJbiB8fCB0aGlzLm9wdGlvbnMuYW5pbWF0ZU91dCkgJiYgdGhpcy5vcHRpb25zLml0ZW1zID09PSAxICYmIHRoaXMuc3VwcG9ydDNkKXtcclxuXHRcdFx0dGhpcy5zdGF0ZS5hbmltYXRlID0gdHJ1ZTtcclxuXHRcdH0gZWxzZSB7dGhpcy5zdGF0ZS5hbmltYXRlID0gZmFsc2U7fVxyXG5cclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiBjcmVhdGVTdGFnZVxyXG5cdCAqIEBkZXNjIENyZWF0ZSBzdGFnZSBhbmQgT3V0ZXItc3RhZ2UgZWxlbWVudHNcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS5jcmVhdGVTdGFnZSA9IGZ1bmN0aW9uKCl7XHJcblx0XHR2YXIgb1N0YWdlID0gZG9jdW1lbnQuY3JlYXRlRWxlbWVudCgnZGl2Jyk7XHJcblx0XHR2YXIgc3RhZ2UgPSBkb2N1bWVudC5jcmVhdGVFbGVtZW50KHRoaXMub3B0aW9ucy5zdGFnZUVsZW1lbnQpO1xyXG5cclxuXHRcdG9TdGFnZS5jbGFzc05hbWUgPSAnb3dsLXN0YWdlLW91dGVyJztcclxuXHRcdHN0YWdlLmNsYXNzTmFtZSA9ICdvd2wtc3RhZ2UnO1xyXG5cclxuXHRcdG9TdGFnZS5hcHBlbmRDaGlsZChzdGFnZSk7XHJcblx0XHR0aGlzLmRvbS5lbC5hcHBlbmRDaGlsZChvU3RhZ2UpO1xyXG5cclxuXHRcdHRoaXMuZG9tLm9TdGFnZSA9IG9TdGFnZTtcclxuXHRcdHRoaXMuZG9tLiRvU3RhZ2UgPSAkKG9TdGFnZSk7XHJcblx0XHR0aGlzLmRvbS5zdGFnZSA9IHN0YWdlO1xyXG5cdFx0dGhpcy5kb20uJHN0YWdlID0gJChzdGFnZSk7XHJcblxyXG5cdFx0b1N0YWdlID0gbnVsbDtcclxuXHRcdHN0YWdlID0gbnVsbDtcclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiBjcmVhdGVJdGVtXHJcblx0ICogQGRlc2MgQ3JlYXRlIGl0ZW0gY29udGFpbmVyXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUuY3JlYXRlSXRlbSA9IGZ1bmN0aW9uKCl7XHJcblx0XHR2YXIgaXRlbSA9IGRvY3VtZW50LmNyZWF0ZUVsZW1lbnQodGhpcy5vcHRpb25zLml0ZW1FbGVtZW50KTtcclxuXHRcdGl0ZW0uY2xhc3NOYW1lID0gdGhpcy5vcHRpb25zLml0ZW1DbGFzcztcclxuXHRcdHJldHVybiBpdGVtO1xyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIGZldGNoQ29udGVudFxyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqL1xyXG5cclxuXHRPd2wucHJvdG90eXBlLmZldGNoQ29udGVudCA9IGZ1bmN0aW9uKGV4dENvbnRlbnQpe1xyXG5cdFx0aWYoZXh0Q29udGVudCl7XHJcblx0XHRcdHRoaXMuZG9tLiRjb250ZW50ID0gKGV4dENvbnRlbnQgaW5zdGFuY2VvZiBqUXVlcnkpID8gZXh0Q29udGVudCA6ICQoZXh0Q29udGVudCk7XHJcblx0XHR9XHJcblx0XHRlbHNlIGlmKHRoaXMub3B0aW9ucy5uZXN0ZWRJdGVtU2VsZWN0b3Ipe1xyXG5cdFx0XHR0aGlzLmRvbS4kY29udGVudD0gdGhpcy5kb20uJGVsLmZpbmQoJy4nK3RoaXMub3B0aW9ucy5uZXN0ZWRJdGVtU2VsZWN0b3IpLm5vdCgnLm93bC1zdGFnZS1vdXRlcicpO1xyXG5cdFx0fSBcclxuXHRcdGVsc2Uge1xyXG5cdFx0XHR0aGlzLmRvbS4kY29udGVudD0gdGhpcy5kb20uJGVsLmNoaWxkcmVuKCkubm90KCcub3dsLXN0YWdlLW91dGVyJyk7XHJcblx0XHR9XHJcblx0XHQvLyBjb250ZW50IGxlbmd0aFxyXG5cdFx0dGhpcy5udW0ub0l0ZW1zID0gdGhpcy5kb20uJGNvbnRlbnQubGVuZ3RoO1xyXG5cclxuXHRcdC8vIGluaXQgU3RydWN0dXJlXHJcblx0XHRpZih0aGlzLm51bS5vSXRlbXMgIT09IDApe1xyXG5cdFx0XHR0aGlzLmluaXRTdHJ1Y3R1cmUoKTtcclxuXHRcdH1cclxuXHR9O1xyXG5cclxuXHJcblx0LyoqXHJcblx0ICogaW5pdFN0cnVjdHVyZVxyXG5cdCAqIEBwYXJhbSBbcmVmcmVzaF0gLSBpZiByZWZyZXNoIGFuZCBub3QgbGF6eUNvbnRlbnQgdGhlbiBkb250IGNyZWF0ZSBub3JtYWwgc3RydWN0dXJlXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUuaW5pdFN0cnVjdHVyZSA9IGZ1bmN0aW9uKCl7XHJcblxyXG5cdFx0Ly8gbGF6eUNvbnRlbnQgbmVlZHMgYXQgbGVhc3QgMyppdGVtcyBcclxuXHJcblx0XHRpZih0aGlzLm9wdGlvbnMubGF6eUNvbnRlbnQgJiYgdGhpcy5udW0ub0l0ZW1zID49IHRoaXMub3B0aW9ucy5pdGVtcyozKXtcclxuXHRcdFx0dGhpcy5zdGF0ZS5sYXp5Q29udGVudCA9IHRydWU7XHJcblx0XHR9IGVsc2Uge1xyXG5cdFx0XHR0aGlzLnN0YXRlLmxhenlDb250ZW50ID0gZmFsc2U7XHJcblx0XHR9XHJcblxyXG5cdFx0aWYodGhpcy5zdGF0ZS5sYXp5Q29udGVudCl7XHJcblxyXG5cdFx0XHQvLyBzdGFydCBwb3NpdGlvblxyXG5cdFx0XHR0aGlzLnBvcy5jdXJyZW50QWJzID0gdGhpcy5vcHRpb25zLml0ZW1zO1xyXG5cclxuXHRcdFx0Ly9yZW1vdmUgbGF6eSBjb250ZW50IGZyb20gRE9NXHJcblx0XHRcdHRoaXMuZG9tLiRjb250ZW50LnJlbW92ZSgpO1xyXG5cclxuXHRcdH0gZWxzZSB7XHJcblx0XHRcdC8vIGNyZWF0ZSBub3JtYWwgc3RydWN0dXJlXHJcblx0XHRcdHRoaXMuY3JlYXRlTm9ybWFsU3RydWN0dXJlKCk7XHJcblx0XHR9XHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICogY3JlYXRlTm9ybWFsU3RydWN0dXJlXHJcblx0ICogQGRlc2MgQ3JlYXRlIG5vcm1hbCBzdHJ1Y3R1cmUgZm9yIHNtYWxsL21pZCB3ZWlnaHQgY29udGVudFxyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqL1xyXG5cclxuXHRPd2wucHJvdG90eXBlLmNyZWF0ZU5vcm1hbFN0cnVjdHVyZSA9IGZ1bmN0aW9uKCl7XHJcblx0XHRmb3IodmFyIGkgPSAwOyBpIDwgdGhpcy5udW0ub0l0ZW1zOyBpKyspe1xyXG5cdFx0XHQvLyBmaWxsICdvd2wtaXRlbScgd2l0aCBjb250ZW50IFxyXG5cdFx0XHR2YXIgaXRlbSA9IHRoaXMuZmlsbEl0ZW0odGhpcy5kb20uJGNvbnRlbnQsaSk7XHJcblx0XHRcdC8vIGFwcGVuZCBpbnRvIHN0YWdlIFxyXG5cdFx0XHR0aGlzLmRvbS4kc3RhZ2UuYXBwZW5kKGl0ZW0pO1xyXG5cdFx0fVxyXG5cdFx0dGhpcy5kb20uJGNvbnRlbnQgPSBudWxsO1xyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIGNyZWF0ZUN1c3RvbVN0cnVjdHVyZVxyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqL1xyXG5cclxuXHRPd2wucHJvdG90eXBlLmNyZWF0ZUN1c3RvbVN0cnVjdHVyZSA9IGZ1bmN0aW9uKGhvd01hbnlJdGVtcyl7XHJcblx0XHRmb3IodmFyIGkgPSAwOyBpIDwgaG93TWFueUl0ZW1zOyBpKyspe1xyXG5cdFx0XHR2YXIgZW1wdHlJdGVtID0gdGhpcy5jcmVhdGVJdGVtKCk7XHJcblx0XHRcdHZhciBpdGVtID0gJChlbXB0eUl0ZW0pO1xyXG5cclxuXHRcdFx0dGhpcy5zZXREYXRhKGl0ZW0sZmFsc2UpO1xyXG5cdFx0XHR0aGlzLmRvbS4kc3RhZ2UuYXBwZW5kKGl0ZW0pO1xyXG5cdFx0fVxyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIGNyZWF0ZUxhenlDb250ZW50U3RydWN0dXJlXHJcblx0ICogQGRlc2MgQ3JlYXRlIGxhenlDb250ZW50IHN0cnVjdHVyZSBmb3IgbGFyZ2UgY29udGVudCBhbmQgYmV0dGVyIG1vYmlsZSBleHBlcmllbmNlXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUuY3JlYXRlTGF6eUNvbnRlbnRTdHJ1Y3R1cmUgPSBmdW5jdGlvbihyZWZyZXNoKXtcclxuXHRcdGlmKCF0aGlzLnN0YXRlLmxhenlDb250ZW50KXtyZXR1cm4gZmFsc2U7fVxyXG5cclxuXHRcdC8vIHByZXZlbnQgcmVjcmVhdGUgLSB0byBkb1xyXG5cdFx0aWYocmVmcmVzaCAmJiB0aGlzLmRvbS4kc3RhZ2UuY2hpbGRyZW4oKS5sZW5ndGggPT09IHRoaXMub3B0aW9ucy5pdGVtcyozKXtcclxuXHRcdFx0cmV0dXJuIGZhbHNlO1xyXG5cdFx0fVxyXG5cdFx0Ly8gcmVtb3ZlIGl0ZW1zIGZyb20gc3RhZ2VcclxuXHRcdHRoaXMuZG9tLiRzdGFnZS5lbXB0eSgpO1xyXG5cclxuXHRcdC8vIGNyZWF0ZSBjdXN0b20gc3RydWN0dXJlXHJcblx0XHR0aGlzLmNyZWF0ZUN1c3RvbVN0cnVjdHVyZSgzKnRoaXMub3B0aW9ucy5pdGVtcyk7XHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICogZmlsbEl0ZW1cclxuXHQgKiBAZGVzYyBGaWxsIGVtcHR5IGl0ZW0gY29udGFpbmVyIHdpdGggcHJvdmlkZWQgY29udGVudFxyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqIEBwYXJhbSBbY29udGVudF0gLSBzdHJpbmcvJGRvbSAtIHBhc3NlZCBvd2wtaXRlbVxyXG5cdCAqIEBwYXJhbSBbaV0gLSBpbmRleCBpbiBqcXVlcnkgb2JqZWN0XHJcblx0ICogcmV0dXJuICQgbmV3IG9iamVjdFxyXG5cdCAqL1xyXG5cclxuXHRPd2wucHJvdG90eXBlLmZpbGxJdGVtID0gZnVuY3Rpb24oY29udGVudCxpKXtcclxuXHRcdHZhciBlbXB0eUl0ZW0gPSB0aGlzLmNyZWF0ZUl0ZW0oKTtcclxuXHRcdHZhciBjID0gY29udGVudFtpXSB8fCBjb250ZW50O1xyXG5cdFx0Ly8gc2V0IGl0ZW0gZGF0YSBcclxuXHRcdHZhciB0cmF2ZXJzZWQgPSB0aGlzLnRyYXZlcnNDb250ZW50KGMpO1xyXG5cdFx0dGhpcy5zZXREYXRhKGVtcHR5SXRlbSxmYWxzZSx0cmF2ZXJzZWQpO1xyXG5cdFx0cmV0dXJuICQoZW1wdHlJdGVtKS5hcHBlbmQoYyk7XHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICogdHJhdmVyc0NvbnRlbnRcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKiBAcGFyYW0gW2NdIC0gY29udGVudFxyXG5cdCAqIHJldHVybiBvYmplY3RcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS50cmF2ZXJzQ29udGVudCA9IGZ1bmN0aW9uKGMpe1xyXG5cdFx0dmFyICRjID0gJChjKSwgZG90VmFsdWUsIGhhc2hWYWx1ZTtcclxuXHRcdGlmKHRoaXMub3B0aW9ucy5kb3REYXRhKXtcclxuXHRcdFx0ZG90VmFsdWUgPSAkYy5maW5kKCdbZGF0YS1kb3RdJykuYW5kU2VsZigpLmRhdGEoJ2RvdCcpO1xyXG5cdFx0fVxyXG5cdFx0Ly8gdXBkYXRlIFVSTCBoYXNoXHJcblx0XHRpZih0aGlzLm9wdGlvbnMuVVJMaGFzaExpc3RlbmVyKXtcclxuXHRcdFx0aGFzaFZhbHVlID0gJGMuZmluZCgnW2RhdGEtaGFzaF0nKS5hbmRTZWxmKCkuZGF0YSgnaGFzaCcpO1xyXG5cdFx0fVxyXG5cdFx0cmV0dXJuIHtcclxuXHRcdFx0ZG90IDogZG90VmFsdWUgfHwgZmFsc2UsXHJcblx0XHRcdGhhc2ggOiBoYXNoVmFsdWUgIHx8IGZhbHNlXHJcblx0XHR9O1xyXG5cdH07XHJcblxyXG5cclxuXHQvKipcclxuXHQgKiBzZXREYXRhXHJcblx0ICogQGRlc2MgU2V0IGl0ZW0galF1ZXJ5IERhdGEgXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICogQHBhcmFtIFtpdGVtXSAtIGRvbSAtIHBhc3NlZCBvd2wtaXRlbVxyXG5cdCAqIEBwYXJhbSBbY2xvbmVPYmpdIC0gJGRvbSAtIHBhc3NlZCBjbG9uZSBpdGVtXHJcblx0ICovXHJcblxyXG5cclxuXHRPd2wucHJvdG90eXBlLnNldERhdGEgPSBmdW5jdGlvbihpdGVtLGNsb25lT2JqLHRyYXZlcnNlZCl7XHJcblx0XHR2YXIgZG90LGhhc2g7XHJcblx0XHRpZih0cmF2ZXJzZWQpe1xyXG5cdFx0XHRkb3QgPSB0cmF2ZXJzZWQuZG90O1xyXG5cdFx0XHRoYXNoID0gdHJhdmVyc2VkLmhhc2g7XHJcblx0XHR9XHJcblx0XHR2YXIgaXRlbURhdGEgPSB7XHJcblx0XHRcdGluZGV4Olx0XHRmYWxzZSxcclxuXHRcdFx0aW5kZXhBYnM6XHRmYWxzZSxcclxuXHRcdFx0cG9zTGVmdDpcdGZhbHNlLFxyXG5cdFx0XHRjbG9uZTpcdFx0ZmFsc2UsXHJcblx0XHRcdGFjdGl2ZTpcdFx0ZmFsc2UsXHJcblx0XHRcdGxvYWRlZDpcdFx0ZmFsc2UsXHJcblx0XHRcdGxhenlMb2FkOlx0ZmFsc2UsXHJcblx0XHRcdGN1cnJlbnQ6XHRmYWxzZSxcclxuXHRcdFx0d2lkdGg6XHRcdGZhbHNlLFxyXG5cdFx0XHRjZW50ZXI6XHRcdGZhbHNlLFxyXG5cdFx0XHRwYWdlOlx0XHRmYWxzZSxcclxuXHRcdFx0aGFzVmlkZW86XHRmYWxzZSxcclxuXHRcdFx0cGxheVZpZGVvOlx0ZmFsc2UsXHJcblx0XHRcdGRvdDpcdFx0ZG90LFxyXG5cdFx0XHRoYXNoOlx0XHRoYXNoXHJcblx0XHR9O1xyXG5cclxuXHRcdC8vIGNvcHkgaXRlbURhdGEgdG8gY2xvbmVkIGl0ZW0gXHJcblxyXG5cdFx0aWYoY2xvbmVPYmope1xyXG5cdFx0XHRpdGVtRGF0YSA9ICQuZXh0ZW5kKHt9LCBpdGVtRGF0YSwgY2xvbmVPYmouZGF0YSgnb3dsLWl0ZW0nKSk7XHJcblx0XHR9XHJcblxyXG5cdFx0JChpdGVtKS5kYXRhKCdvd2wtaXRlbScsIGl0ZW1EYXRhKTtcclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiB1cGRhdGVMb2NhbENvbnRlbnRcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS51cGRhdGVMb2NhbENvbnRlbnQgPSBmdW5jdGlvbigpe1xyXG5cdFx0dGhpcy5kb20uJG9JdGVtcyA9IHRoaXMuZG9tLiRzdGFnZS5maW5kKCcuJyt0aGlzLm9wdGlvbnMuaXRlbUNsYXNzKS5maWx0ZXIoZnVuY3Rpb24oKXtcclxuXHRcdFx0cmV0dXJuICQodGhpcykuZGF0YSgnb3dsLWl0ZW0nKS5jbG9uZSA9PT0gZmFsc2U7XHJcblx0XHR9KTtcclxuXHJcblx0XHR0aGlzLm51bS5vSXRlbXMgPSB0aGlzLmRvbS4kb0l0ZW1zLmxlbmd0aDtcclxuXHRcdC8vdXBkYXRlIGluZGV4IG9uIG9yaWdpbmFsIGl0ZW1zXHJcblxyXG5cdFx0Zm9yKHZhciBrID0gMDsgazx0aGlzLm51bS5vSXRlbXM7IGsrKyl7XHJcblx0XHRcdHZhciBpdGVtID0gdGhpcy5kb20uJG9JdGVtcy5lcShrKTtcclxuXHRcdFx0aXRlbS5kYXRhKCdvd2wtaXRlbScpLmluZGV4ID0gaztcclxuXHRcdH1cclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiBjaGVja1ZpZGVvTGlua3NcclxuXHQgKiBAZGVzYyBDaGVjayBpZiBmb3IgYW55IHZpZGVvcyBsaW5rc1xyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqL1xyXG5cclxuXHRPd2wucHJvdG90eXBlLmNoZWNrVmlkZW9MaW5rcyA9IGZ1bmN0aW9uKCl7XHJcblx0XHRpZighdGhpcy5vcHRpb25zLnZpZGVvKXtyZXR1cm4gZmFsc2U7fVxyXG5cdFx0dmFyIHZpZGVvRWwsaXRlbTtcclxuXHJcblx0XHRmb3IodmFyIGkgPSAwOyBpPHRoaXMubnVtLml0ZW1zOyBpKyspe1xyXG5cclxuXHRcdFx0aXRlbSA9IHRoaXMuZG9tLiRpdGVtcy5lcShpKTtcclxuXHRcdFx0aWYoaXRlbS5kYXRhKCdvd2wtaXRlbScpLmhhc1ZpZGVvKXtcclxuXHRcdFx0XHRjb250aW51ZTtcclxuXHRcdFx0fVxyXG5cclxuXHRcdFx0dmlkZW9FbCA9IGl0ZW0uZmluZCgnLm93bC12aWRlbycpO1xyXG5cdFx0XHRpZih2aWRlb0VsLmxlbmd0aCl7XHJcblx0XHRcdFx0dGhpcy5zdGF0ZS5oYXNWaWRlb3MgPSB0cnVlO1xyXG5cdFx0XHRcdHRoaXMuZG9tLiRpdGVtcy5lcShpKS5kYXRhKCdvd2wtaXRlbScpLmhhc1ZpZGVvID0gdHJ1ZTtcclxuXHRcdFx0XHR2aWRlb0VsLmNzcygnZGlzcGxheScsJ25vbmUnKTtcclxuXHRcdFx0XHR0aGlzLmdldFZpZGVvSW5mbyh2aWRlb0VsLGl0ZW0pO1xyXG5cdFx0XHR9XHJcblx0XHR9XHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICogZ2V0VmlkZW9JbmZvXHJcblx0ICogQGRlc2MgR2V0IFZpZGVvIElEIGFuZCBUeXBlIChZb3VUdWJlL1ZpbWVvIG9ubHkpXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUuZ2V0VmlkZW9JbmZvID0gZnVuY3Rpb24odmlkZW9FbCxpdGVtKXtcclxuXHJcblx0XHR2YXIgaW5mbywgdHlwZSwgaWQsXHJcblx0XHRcdHZpbWVvSWQgPSB2aWRlb0VsLmRhdGEoJ3ZpbWVvLWlkJyksXHJcblx0XHRcdHlvdVR1YmVJZCA9IHZpZGVvRWwuZGF0YSgneW91dHViZS1pZCcpLFxyXG5cdFx0XHR3aWR0aCA9IHZpZGVvRWwuZGF0YSgnd2lkdGgnKSB8fCB0aGlzLm9wdGlvbnMudmlkZW9XaWR0aCxcclxuXHRcdFx0aGVpZ2h0ID0gdmlkZW9FbC5kYXRhKCdoZWlnaHQnKSB8fCB0aGlzLm9wdGlvbnMudmlkZW9IZWlnaHQsXHJcblx0XHRcdHVybCA9IHZpZGVvRWwuYXR0cignaHJlZicpO1xyXG5cclxuXHRcdGlmKHZpbWVvSWQpe1xyXG5cdFx0XHR0eXBlID0gJ3ZpbWVvJztcclxuXHRcdFx0aWQgPSB2aW1lb0lkO1xyXG5cdFx0fSBlbHNlIGlmKHlvdVR1YmVJZCl7XHJcblx0XHRcdHR5cGUgPSAneW91dHViZSc7XHJcblx0XHRcdGlkID0geW91VHViZUlkO1xyXG5cdFx0fSBlbHNlIGlmKHVybCl7XHJcblx0XHRcdGlkID0gdXJsLm1hdGNoKC8oaHR0cDp8aHR0cHM6fClcXC9cXC8ocGxheWVyLnx3d3cuKT8odmltZW9cXC5jb218eW91dHUoYmVcXC5jb218XFwuYmV8YmVcXC5nb29nbGVhcGlzXFwuY29tKSlcXC8odmlkZW9cXC98ZW1iZWRcXC98d2F0Y2hcXD92PXx2XFwvKT8oW0EtWmEtejAtOS5fJS1dKikoXFwmXFxTKyk/Lyk7XHJcblx0XHRcdFxyXG5cdFx0XHRpZiAoaWRbM10uaW5kZXhPZigneW91dHUnKSA+IC0xKSB7XHJcblx0XHRcdFx0dHlwZSA9ICd5b3V0dWJlJztcclxuXHRcdFx0fSBlbHNlIGlmIChpZFszXS5pbmRleE9mKCd2aW1lbycpID4gLTEpIHtcclxuXHRcdFx0XHR0eXBlID0gJ3ZpbWVvJztcclxuXHRcdFx0fVxyXG5cdFx0XHRpZCA9IGlkWzZdO1xyXG5cdFx0fSBlbHNlIHtcclxuXHRcdFx0dGhyb3cgbmV3IEVycm9yKCdNaXNzaW5nIHZpZGVvIGxpbmsuJyk7XHJcblx0XHR9XHJcblxyXG5cdFx0aXRlbS5kYXRhKCdvd2wtaXRlbScpLnZpZGVvVHlwZSA9IHR5cGU7XHJcblx0XHRpdGVtLmRhdGEoJ293bC1pdGVtJykudmlkZW9JZCA9IGlkO1xyXG5cdFx0aXRlbS5kYXRhKCdvd2wtaXRlbScpLnZpZGVvV2lkdGggPSB3aWR0aDtcclxuXHRcdGl0ZW0uZGF0YSgnb3dsLWl0ZW0nKS52aWRlb0hlaWdodCA9IGhlaWdodDtcclxuXHJcblx0XHRpbmZvID0ge1xyXG5cdFx0XHR0eXBlOiB0eXBlLFxyXG5cdFx0XHRpZDogaWRcclxuXHRcdH07XHJcblx0XHRcclxuXHRcdC8vIENoZWNrIGRpbWVuc2lvbnNcclxuXHRcdHZhciBkaW1lbnNpb25zID0gd2lkdGggJiYgaGVpZ2h0ID8gJ3N0eWxlPVwid2lkdGg6Jyt3aWR0aCsncHg7aGVpZ2h0OicraGVpZ2h0KydweDtcIicgOiAnJztcclxuXHJcblx0XHQvLyB3cmFwIHZpZGVvIGNvbnRlbnQgaW50byBvd2wtdmlkZW8td3JhcHBlciBkaXZcclxuXHRcdHZpZGVvRWwud3JhcCgnPGRpdiBjbGFzcz1cIm93bC12aWRlby13cmFwcGVyXCInK2RpbWVuc2lvbnMrJz48L2Rpdj4nKTtcclxuXHJcblx0XHR0aGlzLmNyZWF0ZVZpZGVvVG4odmlkZW9FbCxpbmZvKTtcclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiBjcmVhdGVWaWRlb1RuXHJcblx0ICogQGRlc2MgQ3JlYXRlIFZpZGVvIFRodW1ibmFpbFxyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqL1xyXG5cclxuXHRPd2wucHJvdG90eXBlLmNyZWF0ZVZpZGVvVG4gPSBmdW5jdGlvbih2aWRlb0VsLGluZm8pe1xyXG5cclxuXHRcdHZhciB0bkxpbmssaWNvbixoZWlnaHQ7XHJcblx0XHR2YXIgY3VzdG9tVG4gPSB2aWRlb0VsLmZpbmQoJ2ltZycpO1xyXG5cdFx0dmFyIHNyY1R5cGUgPSAnc3JjJztcclxuXHRcdHZhciBsYXp5Q2xhc3MgPSAnJztcclxuXHRcdHZhciB0aGF0ID0gdGhpcztcclxuXHJcblx0XHRpZih0aGlzLm9wdGlvbnMubGF6eUxvYWQpe1xyXG5cdFx0XHRzcmNUeXBlID0gJ2RhdGEtc3JjJztcclxuXHRcdFx0bGF6eUNsYXNzID0gJ293bC1sYXp5JztcclxuXHRcdH1cclxuXHJcblx0XHQvLyBDdXN0b20gdGh1bWJuYWlsXHJcblxyXG5cdFx0aWYoY3VzdG9tVG4ubGVuZ3RoKXtcclxuXHRcdFx0YWRkVGh1bWJuYWlsKGN1c3RvbVRuLmF0dHIoc3JjVHlwZSkpO1xyXG5cdFx0XHRjdXN0b21Ubi5yZW1vdmUoKTtcclxuXHRcdFx0cmV0dXJuIGZhbHNlO1xyXG5cdFx0fVxyXG5cdFx0XHJcblx0XHRmdW5jdGlvbiBhZGRUaHVtYm5haWwodG5QYXRoKXtcclxuXHRcdFx0aWNvbiA9ICc8ZGl2IGNsYXNzPVwib3dsLXZpZGVvLXBsYXktaWNvblwiPjwvZGl2Pic7XHJcblxyXG5cdFx0XHRpZih0aGF0Lm9wdGlvbnMubGF6eUxvYWQpe1xyXG5cdFx0XHRcdHRuTGluayA9ICc8ZGl2IGNsYXNzPVwib3dsLXZpZGVvLXRuICcrIGxhenlDbGFzcyArJ1wiICcrIHNyY1R5cGUgKyc9XCInKyB0blBhdGggKydcIj48L2Rpdj4nO1xyXG5cdFx0XHR9IGVsc2V7XHJcblx0XHRcdFx0dG5MaW5rID0gJzxkaXYgY2xhc3M9XCJvd2wtdmlkZW8tdG5cIiBzdHlsZT1cIm9wYWNpdHk6MTtiYWNrZ3JvdW5kLWltYWdlOnVybCgnICsgdG5QYXRoICsgJylcIj48L2Rpdj4nO1xyXG5cdFx0XHR9XHJcblx0XHRcdHZpZGVvRWwuYWZ0ZXIodG5MaW5rKTtcclxuXHRcdFx0dmlkZW9FbC5hZnRlcihpY29uKTtcclxuXHRcdH1cclxuXHJcblx0XHRpZihpbmZvLnR5cGUgPT09ICd5b3V0dWJlJyl7XHJcblx0XHRcdHZhciBwYXRoID0gXCJodHRwOi8vaW1nLnlvdXR1YmUuY29tL3ZpL1wiKyBpbmZvLmlkICtcIi9ocWRlZmF1bHQuanBnXCI7XHJcblx0XHRcdGFkZFRodW1ibmFpbChwYXRoKTtcclxuXHRcdH0gZWxzZVxyXG5cdFx0aWYoaW5mby50eXBlID09PSAndmltZW8nKXtcclxuXHRcdFx0JC5hamF4KHtcclxuXHRcdFx0XHR0eXBlOidHRVQnLFxyXG5cdFx0XHRcdHVybDogJ2h0dHA6Ly92aW1lby5jb20vYXBpL3YyL3ZpZGVvLycgKyBpbmZvLmlkICsgJy5qc29uJyxcclxuXHRcdFx0XHRqc29ucDogJ2NhbGxiYWNrJyxcclxuXHRcdFx0XHRkYXRhVHlwZTogJ2pzb25wJyxcclxuXHRcdFx0XHRzdWNjZXNzOiBmdW5jdGlvbihkYXRhKXtcclxuXHRcdFx0XHRcdHZhciBwYXRoID0gZGF0YVswXS50aHVtYm5haWxfbGFyZ2U7XHJcblx0XHRcdFx0XHRhZGRUaHVtYm5haWwocGF0aCk7XHJcblx0XHRcdFx0XHRpZih0aGF0Lm9wdGlvbnMubG9vcCl7XHJcblx0XHRcdFx0XHRcdHRoYXQudXBkYXRlSXRlbVN0YXRlKCk7XHJcblx0XHRcdFx0XHR9XHJcblx0XHRcdFx0fVxyXG5cdFx0XHR9KTtcclxuXHRcdH1cclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiBzdG9wVmlkZW9cclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS5zdG9wVmlkZW8gPSBmdW5jdGlvbigpe1xyXG5cdFx0dGhpcy5maXJlQ2FsbGJhY2soJ29uVmlkZW9TdG9wJyk7XHJcblx0XHR2YXIgaXRlbSA9IHRoaXMuZG9tLiRpdGVtcy5lcSh0aGlzLnN0YXRlLnZpZGVvUGxheUluZGV4KTtcclxuXHRcdGl0ZW0uZmluZCgnLm93bC12aWRlby1mcmFtZScpLnJlbW92ZSgpO1xyXG5cdFx0aXRlbS5yZW1vdmVDbGFzcygnb3dsLXZpZGVvLXBsYXlpbmcnKTtcclxuXHRcdHRoaXMuc3RhdGUudmlkZW9QbGF5ID0gZmFsc2U7XHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICogcGxheVZpZGVvXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUucGxheVZpZGVvID0gZnVuY3Rpb24oZXYpe1xyXG5cdFx0dGhpcy5maXJlQ2FsbGJhY2soJ29uVmlkZW9QbGF5Jyk7XHJcblxyXG5cdFx0aWYodGhpcy5zdGF0ZS52aWRlb1BsYXkpe1xyXG5cdFx0XHR0aGlzLnN0b3BWaWRlbygpO1xyXG5cdFx0fVxyXG5cdFx0dmFyIHZpZGVvTGluayx2aWRlb1dyYXAsXHJcblx0XHRcdHRhcmdldCA9ICQoZXYudGFyZ2V0IHx8IGV2LnNyY0VsZW1lbnQpLFxyXG5cdFx0XHRpdGVtID0gdGFyZ2V0LmNsb3Nlc3QoJy4nK3RoaXMub3B0aW9ucy5pdGVtQ2xhc3MpO1xyXG5cclxuXHRcdHZhciB2aWRlb1R5cGUgPSBpdGVtLmRhdGEoJ293bC1pdGVtJykudmlkZW9UeXBlLFxyXG5cdFx0XHRpZCA9IGl0ZW0uZGF0YSgnb3dsLWl0ZW0nKS52aWRlb0lkLFxyXG5cdFx0XHR3aWR0aCA9IGl0ZW0uZGF0YSgnb3dsLWl0ZW0nKS52aWRlb1dpZHRoIHx8IE1hdGguZmxvb3IoaXRlbS5kYXRhKCdvd2wtaXRlbScpLndpZHRoIC0gdGhpcy5vcHRpb25zLm1hcmdpbiksXHJcblx0XHRcdGhlaWdodCA9IGl0ZW0uZGF0YSgnb3dsLWl0ZW0nKS52aWRlb0hlaWdodCB8fCB0aGlzLmRvbS4kc3RhZ2UuaGVpZ2h0KCk7XHJcblxyXG5cdFx0aWYodmlkZW9UeXBlID09PSAneW91dHViZScpe1xyXG5cdFx0XHR2aWRlb0xpbmsgPSBcIjxpZnJhbWUgd2lkdGg9XFxcIlwiKyB3aWR0aCArXCJcXFwiIGhlaWdodD1cXFwiXCIrIGhlaWdodCArXCJcXFwiIHNyYz1cXFwiaHR0cDovL3d3dy55b3V0dWJlLmNvbS9lbWJlZC9cIiArIGlkICsgXCI/YXV0b3BsYXk9MSZ2PVwiICsgaWQgKyBcIlxcXCIgZnJhbWVib3JkZXI9XFxcIjBcXFwiIGFsbG93ZnVsbHNjcmVlbj48L2lmcmFtZT5cIjtcclxuXHRcdH0gZWxzZSBpZih2aWRlb1R5cGUgPT09ICd2aW1lbycpe1xyXG5cdFx0XHR2aWRlb0xpbmsgPSAnPGlmcmFtZSBzcmM9XCJodHRwOi8vcGxheWVyLnZpbWVvLmNvbS92aWRlby8nKyBpZCArJz9hdXRvcGxheT0xXCIgd2lkdGg9XCInKyB3aWR0aCArJ1wiIGhlaWdodD1cIicrIGhlaWdodCArJ1wiIGZyYW1lYm9yZGVyPVwiMFwiIHdlYmtpdGFsbG93ZnVsbHNjcmVlbiBtb3phbGxvd2Z1bGxzY3JlZW4gYWxsb3dmdWxsc2NyZWVuPjwvaWZyYW1lPic7XHJcblx0XHR9XHJcblx0XHRcclxuXHRcdGl0ZW0uYWRkQ2xhc3MoJ293bC12aWRlby1wbGF5aW5nJyk7XHJcblx0XHR0aGlzLnN0YXRlLnZpZGVvUGxheSA9IHRydWU7XHJcblx0XHR0aGlzLnN0YXRlLnZpZGVvUGxheUluZGV4ID0gaXRlbS5kYXRhKCdvd2wtaXRlbScpLmluZGV4QWJzO1xyXG5cclxuXHRcdHZpZGVvV3JhcCA9ICQoJzxkaXYgc3R5bGU9XCJoZWlnaHQ6JysgaGVpZ2h0ICsncHg7IHdpZHRoOicrIHdpZHRoICsncHhcIiBjbGFzcz1cIm93bC12aWRlby1mcmFtZVwiPicgKyB2aWRlb0xpbmsgKyAnPC9kaXY+Jyk7XHJcblx0XHR0YXJnZXQuYWZ0ZXIodmlkZW9XcmFwKTtcclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiBsb29wQ2xvbmVcclxuXHQgKiBAZGVzYyBNYWtlIGEgY2xvbmVzIGZvciBpbmZpbml0eSBsb29wXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUubG9vcENsb25lID0gZnVuY3Rpb24oKXtcclxuXHRcdGlmKCF0aGlzLm9wdGlvbnMubG9vcCB8fCB0aGlzLnN0YXRlLmxhenlDb250ZW50IHx8IHRoaXMubnVtLm9JdGVtcyA8IHRoaXMub3B0aW9ucy5pdGVtcyl7cmV0dXJuIGZhbHNlO31cclxuXHJcblx0XHR2YXIgZmlyc3RDbG9uZSxcdGxhc3RDbG9uZSwgaSxcclxuXHRcdFx0bnVtXHQ9XHRcdHRoaXMub3B0aW9ucy5pdGVtcywgXHJcblx0XHRcdGxhc3ROdW0gPVx0dGhpcy5udW0ub0l0ZW1zLTE7XHJcblxyXG5cdFx0Ly8gaWYgbmVpZ2hib3VyIG1hcmdpbiB0aGVuIGFkZCBvbmUgbW9yZSBkdXBsaWNhdFxyXG5cdFx0aWYodGhpcy5vcHRpb25zLnN0YWdlUGFkZGluZyAmJiB0aGlzLm9wdGlvbnMuaXRlbXMgPT09IDEpe1xyXG5cdFx0XHRudW0rPTE7XHJcblx0XHR9XHJcblx0XHR0aGlzLm51bS5jSXRlbXMgPSBudW0gKiAyO1xyXG5cclxuXHRcdGZvcihpID0gMDsgaSA8IG51bTsgaSsrKXtcclxuXHRcdFx0Ly8gQ2xvbmUgaXRlbSBcclxuXHRcdFx0dmFyIGZpcnN0ID1cdFx0dGhpcy5kb20uJG9JdGVtcy5lcShpKS5jbG9uZSh0cnVlLHRydWUpO1xyXG5cdFx0XHR2YXIgbGFzdCA9XHRcdHRoaXMuZG9tLiRvSXRlbXMuZXEobGFzdE51bS1pKS5jbG9uZSh0cnVlLHRydWUpO1xyXG5cdFx0XHRmaXJzdENsb25lID0gXHQkKGZpcnN0WzBdKS5hZGRDbGFzcygnY2xvbmVkJyk7XHJcblx0XHRcdGxhc3RDbG9uZSA9IFx0JChsYXN0WzBdKS5hZGRDbGFzcygnY2xvbmVkJyk7XHJcblxyXG5cdFx0XHQvLyBzZXQgY2xvbmUgZGF0YSBcclxuXHRcdFx0Ly8gU29tZWhvdyBkYXRhIGhhcyByZWZlcmVuY2UgdG8gc2FtZSBkYXRhIGlkIGluIGNhc2ggXHJcblxyXG5cdFx0XHR0aGlzLnNldERhdGEoZmlyc3RDbG9uZVswXSxmaXJzdCk7XHJcblx0XHRcdHRoaXMuc2V0RGF0YShsYXN0Q2xvbmVbMF0sbGFzdCk7XHJcblxyXG5cdFx0XHRmaXJzdENsb25lLmRhdGEoJ293bC1pdGVtJykuY2xvbmUgPSB0cnVlO1xyXG5cdFx0XHRsYXN0Q2xvbmUuZGF0YSgnb3dsLWl0ZW0nKS5jbG9uZSA9IHRydWU7XHJcblxyXG5cdFx0XHR0aGlzLmRvbS4kc3RhZ2UuYXBwZW5kKGZpcnN0Q2xvbmUpO1xyXG5cdFx0XHR0aGlzLmRvbS4kc3RhZ2UucHJlcGVuZChsYXN0Q2xvbmUpO1xyXG5cclxuXHRcdFx0Zmlyc3RDbG9uZSA9IGxhc3RDbG9uZSA9IG51bGw7XHJcblx0XHR9XHJcblxyXG5cdFx0dGhpcy5kb20uJGNJdGVtcyA9IHRoaXMuZG9tLiRzdGFnZS5maW5kKCcuJyt0aGlzLm9wdGlvbnMuaXRlbUNsYXNzKS5maWx0ZXIoZnVuY3Rpb24oKXtcclxuXHRcdFx0cmV0dXJuICQodGhpcykuZGF0YSgnb3dsLWl0ZW0nKS5jbG9uZSA9PT0gdHJ1ZTtcclxuXHRcdH0pO1xyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIHJlQ2xvbmVcclxuXHQgKiBAZGVzYyBVcGRhdGUgQ2xvbmVkIGVsZW1lbnRzXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUucmVDbG9uZSA9IGZ1bmN0aW9uKCl7XHJcblx0XHQvLyByZW1vdmUgY2xvbmVkIGl0ZW1zIFxyXG5cdFx0aWYodGhpcy5kb20uJGNJdGVtcyAhPT0gbnVsbCl7IC8vICYmICh0aGlzLm51bS5vSXRlbXMgIT09IDAgJiYgdGhpcy5udW0ub0l0ZW1zIDw9IHRoaXMub3B0aW9ucy5pdGVtcykpe1xyXG5cdFx0XHR0aGlzLmRvbS4kY0l0ZW1zLnJlbW92ZSgpO1xyXG5cdFx0XHR0aGlzLmRvbS4kY0l0ZW1zID0gbnVsbDtcclxuXHRcdFx0dGhpcy5udW0uY0l0ZW1zID0gMDtcclxuXHRcdH1cclxuXHJcblx0XHRpZighdGhpcy5vcHRpb25zLmxvb3Ape1xyXG5cdFx0XHRyZXR1cm47XHJcblx0XHR9XHJcblx0XHQvLyBnZW5lcmV0ZSBuZXcgZWxlbWVudHMgXHJcblx0XHR0aGlzLmxvb3BDbG9uZSgpO1xyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIGNhbGN1bGF0ZVxyXG5cdCAqIEBkZXNjIFVwZGF0ZSBpdGVtIGluZGV4IGRhdGFcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS5jYWxjdWxhdGUgPSBmdW5jdGlvbigpe1xyXG5cclxuXHRcdHZhciBpLGosayxkaXN0LHBvc0xlZnQ9MCxmdWxsV2lkdGg9MDtcclxuXHJcblx0XHQvLyBlbGVtZW50IHdpZHRoIG1pbnVzIG5laWdoYm91ciBcclxuXHRcdHRoaXMud2lkdGguZWwgPSB0aGlzLmRvbS4kZWwud2lkdGgoKSAtICh0aGlzLm9wdGlvbnMuc3RhZ2VQYWRkaW5nKjIpO1xyXG5cclxuXHRcdC8vdG8gY2hlY2tcclxuXHRcdHRoaXMud2lkdGgudmlldyA9IHRoaXMuZG9tLiRlbC53aWR0aCgpO1xyXG5cclxuXHRcdC8vIGNhbGN1bGF0ZSB3aWR0aCBtaW51cyBhZGRpdGlvbiBtYXJnaW5zIFxyXG5cdFx0dmFyIGVsTWludXNNYXJnaW4gPSB0aGlzLndpZHRoLmVsIC0gKHRoaXMub3B0aW9ucy5tYXJnaW4gKiAodGhpcy5vcHRpb25zLml0ZW1zID09PSAxID8gMCA6IHRoaXMub3B0aW9ucy5pdGVtcyAtMSkpO1xyXG5cclxuXHRcdC8vIGNhbGN1bGF0ZSBlbGVtZW50IHdpZHRoIGFuZCBpdGVtIHdpZHRoIFxyXG5cdFx0dGhpcy53aWR0aC5lbCA9ICBcdHRoaXMud2lkdGguZWwgKyB0aGlzLm9wdGlvbnMubWFyZ2luO1xyXG5cdFx0dGhpcy53aWR0aC5pdGVtID0gXHQoKGVsTWludXNNYXJnaW4gLyB0aGlzLm9wdGlvbnMuaXRlbXMpICsgdGhpcy5vcHRpb25zLm1hcmdpbikudG9GaXhlZCgzKTtcclxuXHJcblx0XHR0aGlzLmRvbS4kaXRlbXMgPSBcdHRoaXMuZG9tLiRzdGFnZS5maW5kKCcub3dsLWl0ZW0nKTtcclxuXHRcdHRoaXMubnVtLml0ZW1zID0gXHR0aGlzLmRvbS4kaXRlbXMubGVuZ3RoO1xyXG5cclxuXHRcdC8vY2hhbmdlIHRvIGF1dG9XaWR0aHNcclxuXHRcdGlmKHRoaXMub3B0aW9ucy5hdXRvV2lkdGgpe1xyXG5cdFx0XHR0aGlzLmRvbS4kaXRlbXMuY3NzKCd3aWR0aCcsJycpO1xyXG5cdFx0fVxyXG5cclxuXHRcdC8vIFNldCBncmlkIGFycmF5IFxyXG5cdFx0dGhpcy5wb3MuaXRlbXMgPSBcdFtdO1xyXG5cdFx0dGhpcy5udW0ubWVyZ2VkID0gXHRbXTtcclxuXHRcdHRoaXMubnVtLm5hdiA9IFx0XHRbXTtcclxuXHJcblx0XHQvLyBpdGVtIGRpc3RhbmNlc1xyXG5cdFx0aWYodGhpcy5vcHRpb25zLnJ0bCl7XHJcblx0XHRcdGRpc3QgPSB0aGlzLm9wdGlvbnMuY2VudGVyID8gLSgodGhpcy53aWR0aC5lbCkvMikgOiAwO1xyXG5cdFx0fSBlbHNlIHtcclxuXHRcdFx0ZGlzdCA9IHRoaXMub3B0aW9ucy5jZW50ZXIgPyAodGhpcy53aWR0aC5lbCkvMiA6IDA7XHJcblx0XHR9XHJcblx0XHRcclxuXHRcdHRoaXMud2lkdGgubWVyZ2VTdGFnZSA9IDA7XHJcblxyXG5cdFx0Ly8gQ2FsY3VsYXRlIGl0ZW1zIHBvc2l0aW9uc1xyXG5cdFx0Zm9yKGkgPSAwOyBpPHRoaXMubnVtLml0ZW1zOyBpKyspe1xyXG5cclxuXHRcdFx0Ly8gY2hlY2sgbWVyZ2VkIGl0ZW1zXHJcblxyXG5cdFx0XHRpZih0aGlzLm9wdGlvbnMubWVyZ2Upe1xyXG5cdFx0XHRcdHZhciBtZXJnZU51bWJlciA9IHRoaXMuZG9tLiRpdGVtcy5lcShpKS5maW5kKCdbZGF0YS1tZXJnZV0nKS5hdHRyKCdkYXRhLW1lcmdlJykgfHwgMTtcclxuXHRcdFx0XHRpZih0aGlzLm9wdGlvbnMubWVyZ2VGaXQgJiYgbWVyZ2VOdW1iZXIgPiB0aGlzLm9wdGlvbnMuaXRlbXMpe1xyXG5cdFx0XHRcdFx0bWVyZ2VOdW1iZXIgPSB0aGlzLm9wdGlvbnMuaXRlbXM7XHJcblx0XHRcdFx0fVxyXG5cdFx0XHRcdHRoaXMubnVtLm1lcmdlZC5wdXNoKHBhcnNlSW50KG1lcmdlTnVtYmVyKSk7XHJcblx0XHRcdFx0dGhpcy53aWR0aC5tZXJnZVN0YWdlICs9IHRoaXMud2lkdGguaXRlbSAqIHRoaXMubnVtLm1lcmdlZFtpXTtcclxuXHRcdFx0fSBlbHNlIHtcclxuXHRcdFx0XHR0aGlzLm51bS5tZXJnZWQucHVzaCgxKTtcclxuXHRcdFx0fVxyXG5cclxuXHRcdFx0Ly8gQXJyYXkgYmFzZWQgb24gbWVyZ2VkIGl0ZW1zIHVzZWQgYnkgZG90cyBhbmQgbmF2aWdhdGlvblxyXG5cdFx0XHRpZih0aGlzLm9wdGlvbnMubG9vcCl7XHJcblx0XHRcdFx0aWYoaT49dGhpcy5udW0uY0l0ZW1zLzIgJiYgaTx0aGlzLm51bS5jSXRlbXMvMit0aGlzLm51bS5vSXRlbXMpe1xyXG5cdFx0XHRcdFx0dGhpcy5udW0ubmF2LnB1c2godGhpcy5udW0ubWVyZ2VkW2ldKTtcclxuXHRcdFx0XHR9XHJcblx0XHRcdH0gZWxzZSB7XHJcblx0XHRcdFx0dGhpcy5udW0ubmF2LnB1c2godGhpcy5udW0ubWVyZ2VkW2ldKTtcclxuXHRcdFx0fVxyXG5cclxuXHRcdFx0dmFyIGlXaWR0aCA9IHRoaXMud2lkdGguaXRlbSAqIHRoaXMubnVtLm1lcmdlZFtpXTtcclxuXHJcblx0XHRcdC8vIGF1dG9XaWR0aCBpdGVtIHNpemVcclxuXHRcdFx0aWYodGhpcy5vcHRpb25zLmF1dG9XaWR0aCl7XHJcblx0XHRcdFx0aVdpZHRoID0gdGhpcy5kb20uJGl0ZW1zLmVxKGkpLndpZHRoKCkgKyB0aGlzLm9wdGlvbnMubWFyZ2luO1xyXG5cdFx0XHRcdGlmKHRoaXMub3B0aW9ucy5ydGwpe1xyXG5cdFx0XHRcdFx0dGhpcy5kb20uJGl0ZW1zW2ldLnN0eWxlLm1hcmdpbkxlZnQgPSB0aGlzLm9wdGlvbnMubWFyZ2luICsgJ3B4JztcclxuXHRcdFx0XHR9IGVsc2Uge1xyXG5cdFx0XHRcdFx0dGhpcy5kb20uJGl0ZW1zW2ldLnN0eWxlLm1hcmdpblJpZ2h0ID0gdGhpcy5vcHRpb25zLm1hcmdpbiArICdweCc7XHJcblx0XHRcdFx0fVxyXG5cdFx0XHRcdFxyXG5cdFx0XHR9XHJcblx0XHRcdC8vIHB1c2ggaXRlbSBwb3NpdGlvbiBpbnRvIGFycmF5XHJcblx0XHRcdHRoaXMucG9zLml0ZW1zLnB1c2goZGlzdCk7XHJcblxyXG5cdFx0XHQvLyB1cGRhdGUgaXRlbSBkYXRhXHJcblx0XHRcdHRoaXMuZG9tLiRpdGVtcy5lcShpKS5kYXRhKCdvd2wtaXRlbScpLnBvc0xlZnQgPSBwb3NMZWZ0O1xyXG5cdFx0XHR0aGlzLmRvbS4kaXRlbXMuZXEoaSkuZGF0YSgnb3dsLWl0ZW0nKS53aWR0aCA9IGlXaWR0aDtcclxuXHJcblx0XHRcdC8vIGRpc3Qgc3RhcnRzIGZyb20gbWlkZGxlIG9mIHN0YWdlIGlmIGNlbnRlclxyXG5cdFx0XHQvLyBwb3NMZWZ0IGFsd2F5cyBzdGFydHMgZnJvbSAwXHJcblx0XHRcdGlmKHRoaXMub3B0aW9ucy5ydGwpe1xyXG5cdFx0XHRcdGRpc3QgKz0gaVdpZHRoO1xyXG5cdFx0XHRcdHBvc0xlZnQgKz0gaVdpZHRoO1xyXG5cdFx0XHR9IGVsc2V7XHJcblx0XHRcdFx0ZGlzdCAtPSBpV2lkdGg7XHJcblx0XHRcdFx0cG9zTGVmdCAtPSBpV2lkdGg7XHJcblx0XHRcdH1cclxuXHJcblx0XHRcdGZ1bGxXaWR0aCAtPSBNYXRoLmFicyhpV2lkdGgpO1xyXG5cclxuXHRcdFx0Ly8gdXBkYXRlIHBvc2l0aW9uIGlmIGNlbnRlclxyXG5cdFx0XHRpZih0aGlzLm9wdGlvbnMuY2VudGVyKXtcclxuXHRcdFx0XHR0aGlzLnBvcy5pdGVtc1tpXSA9ICF0aGlzLm9wdGlvbnMucnRsID8gdGhpcy5wb3MuaXRlbXNbaV0gLSAoaVdpZHRoLzIpIDogdGhpcy5wb3MuaXRlbXNbaV0gKyAoaVdpZHRoLzIpO1xyXG5cdFx0XHR9XHJcblx0XHR9XHJcblxyXG5cdFx0aWYodGhpcy5vcHRpb25zLmF1dG9XaWR0aCl7XHJcblx0XHRcdHRoaXMud2lkdGguc3RhZ2UgPSB0aGlzLm9wdGlvbnMuY2VudGVyID8gTWF0aC5hYnMoZnVsbFdpZHRoKSA6IE1hdGguYWJzKGRpc3QpO1xyXG5cdFx0fSBlbHNlIHtcclxuXHRcdFx0dGhpcy53aWR0aC5zdGFnZSA9IE1hdGguYWJzKGZ1bGxXaWR0aCk7XHJcblx0XHR9XHJcblxyXG5cdFx0Ly91cGRhdGUgaW5kZXhBYnMgb24gYWxsIGl0ZW1zIFxyXG5cdFx0dmFyIGFsbEl0ZW1zID0gdGhpcy5udW0ub0l0ZW1zICsgdGhpcy5udW0uY0l0ZW1zO1xyXG5cclxuXHRcdGZvcihqID0gMDsgajwgYWxsSXRlbXM7IGorKyl7XHJcblx0XHRcdHRoaXMuZG9tLiRpdGVtcy5lcShqKS5kYXRhKCdvd2wtaXRlbScpLmluZGV4QWJzID0gajtcclxuXHRcdH1cclxuXHJcblx0XHQvLyBTZXQgTWluIGFuZCBNYXhcclxuXHRcdHRoaXMuc2V0TWluTWF4KCk7XHJcblxyXG5cdFx0Ly8gUmVjYWxjdWxhdGUgZ3JpZCBcclxuXHRcdHRoaXMuc2V0U2l6ZXMoKTtcclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiBzZXRNaW5NYXhcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS5zZXRNaW5NYXggPSBmdW5jdGlvbigpe1xyXG5cclxuXHRcdC8vIHNldCBNaW5cclxuXHRcdHZhciBtaW5pbXVtID0gdGhpcy5kb20uJG9JdGVtcy5lcSgwKS5kYXRhKCdvd2wtaXRlbScpLmluZGV4QWJzO1xyXG5cdFx0dmFyIGk7XHJcblx0XHR0aGlzLnBvcy5taW4gPSAwO1xyXG5cdFx0dGhpcy5wb3MubWluVmFsdWUgPSB0aGlzLnBvcy5pdGVtc1ttaW5pbXVtXTtcclxuXHJcblx0XHQvLyBzZXQgbWF4IHBvc2l0aW9uXHJcblx0XHRpZighdGhpcy5vcHRpb25zLmxvb3Ape1xyXG5cdFx0XHR0aGlzLnBvcy5tYXggPSB0aGlzLm51bS5vSXRlbXMtMTtcclxuXHRcdH1cclxuXHJcblx0XHRpZih0aGlzLm9wdGlvbnMubG9vcCl7XHJcblx0XHRcdHRoaXMucG9zLm1heCA9IHRoaXMubnVtLm9JdGVtcyt0aGlzLm9wdGlvbnMuaXRlbXM7XHJcblx0XHR9XHJcblxyXG5cdFx0aWYoIXRoaXMub3B0aW9ucy5sb29wICYmICF0aGlzLm9wdGlvbnMuY2VudGVyKXtcclxuXHRcdFx0dGhpcy5wb3MubWF4ID0gdGhpcy5udW0ub0l0ZW1zLXRoaXMub3B0aW9ucy5pdGVtcztcclxuXHRcdH1cclxuXHJcblx0XHRpZih0aGlzLm9wdGlvbnMubG9vcCAmJiB0aGlzLm9wdGlvbnMuY2VudGVyKXtcclxuXHRcdFx0dGhpcy5wb3MubWF4ID0gdGhpcy5udW0ub0l0ZW1zK3RoaXMub3B0aW9ucy5pdGVtcztcclxuXHRcdH1cclxuXHJcblx0XHQvL3NldCBtYXggdmFsdWVcclxuXHRcdHRoaXMucG9zLm1heFZhbHVlID0gdGhpcy5wb3MuaXRlbXNbdGhpcy5wb3MubWF4XTtcclxuXHJcblx0XHQvL01heCBmb3IgYXV0b1dpZHRoIGNvbnRlbnQgXHJcblx0XHRpZigoIXRoaXMub3B0aW9ucy5sb29wICYmICF0aGlzLm9wdGlvbnMuY2VudGVyICYmIHRoaXMub3B0aW9ucy5hdXRvV2lkdGgpIHx8ICh0aGlzLm9wdGlvbnMubWVyZ2UgJiYgIXRoaXMub3B0aW9ucy5jZW50ZXIpICl7XHJcblx0XHRcdHZhciByZXZlcnQgPSB0aGlzLm9wdGlvbnMucnRsID8gMSA6IC0xO1xyXG5cdFx0XHRmb3IgKGkgPSAwOyBpIDwgdGhpcy5wb3MuaXRlbXMubGVuZ3RoOyBpKyspIHtcclxuXHRcdFx0XHRpZiggKHRoaXMucG9zLml0ZW1zW2ldICogcmV2ZXJ0KSA8IHRoaXMud2lkdGguc3RhZ2UtdGhpcy53aWR0aC5lbCApe1xyXG5cdFx0XHRcdFx0dGhpcy5wb3MubWF4ID0gaSsxO1xyXG5cdFx0XHRcdH1cclxuXHRcdFx0fVxyXG5cdFx0XHR0aGlzLnBvcy5tYXhWYWx1ZSA9IHRoaXMub3B0aW9ucy5ydGwgPyB0aGlzLndpZHRoLnN0YWdlLXRoaXMud2lkdGguZWwgOiAtKHRoaXMud2lkdGguc3RhZ2UtdGhpcy53aWR0aC5lbCk7XHJcblx0XHRcdHRoaXMucG9zLml0ZW1zW3RoaXMucG9zLm1heF0gPSB0aGlzLnBvcy5tYXhWYWx1ZTtcclxuXHRcdH1cclxuXHJcblx0XHQvLyBTZXQgbG9vcCBib3VuZHJpZXNcclxuXHRcdGlmKHRoaXMub3B0aW9ucy5jZW50ZXIpe1xyXG5cdFx0XHR0aGlzLnBvcy5sb29wID0gdGhpcy5wb3MuaXRlbXNbMF0tdGhpcy5wb3MuaXRlbXNbdGhpcy5udW0ub0l0ZW1zXTtcclxuXHRcdH0gZWxzZSB7XHJcblx0XHRcdHRoaXMucG9zLmxvb3AgPSAtdGhpcy5wb3MuaXRlbXNbdGhpcy5udW0ub0l0ZW1zXTtcclxuXHRcdH1cclxuXHJcblx0XHQvL2lmIGlzIGxlc3MgaXRlbXNcclxuXHRcdGlmKHRoaXMubnVtLm9JdGVtcyA8IHRoaXMub3B0aW9ucy5pdGVtcyAmJiAhdGhpcy5vcHRpb25zLmNlbnRlcil7XHJcblx0XHRcdHRoaXMucG9zLm1heCA9IDA7XHJcblx0XHRcdHRoaXMucG9zLm1heFZhbHVlID0gdGhpcy5wb3MuaXRlbXNbMF07XHJcblx0XHR9XHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICogc2V0U2l6ZXNcclxuXHQgKiBAZGVzYyBTZXQgc2l6ZXMgb24gZWxlbWVudHMgKGZyb20gY29sbGVjdERhdGEgZnVuY3Rpb24pXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUuc2V0U2l6ZXMgPSBmdW5jdGlvbigpe1xyXG5cclxuXHRcdC8vIHNob3cgbmVpZ2hib3VycyBcclxuXHRcdGlmKHRoaXMub3B0aW9ucy5zdGFnZVBhZGRpbmcgIT09IGZhbHNlKXtcclxuXHRcdFx0dGhpcy5kb20ub1N0YWdlLnN0eWxlLnBhZGRpbmdMZWZ0ID0gXHR0aGlzLm9wdGlvbnMuc3RhZ2VQYWRkaW5nICsgJ3B4JztcclxuXHRcdFx0dGhpcy5kb20ub1N0YWdlLnN0eWxlLnBhZGRpbmdSaWdodCA9IFx0dGhpcy5vcHRpb25zLnN0YWdlUGFkZGluZyArICdweCc7XHJcblx0XHR9XHJcblxyXG5cdFx0Ly8gQ1JBWlkgRklYISEhIERvdWJsZWNoZWNrIHRoaXMhXHJcblx0XHQvL2lmKHRoaXMud2lkdGguc3RhZ2VQcmV2ID4gdGhpcy53aWR0aC5zdGFnZSl7XHJcblx0XHRpZih0aGlzLm9wdGlvbnMucnRsKXtcclxuXHRcdFx0d2luZG93LnNldFRpbWVvdXQoZnVuY3Rpb24oKXtcclxuXHRcdFx0XHR0aGlzLmRvbS5zdGFnZS5zdHlsZS53aWR0aCA9IHRoaXMud2lkdGguc3RhZ2UgKyAncHgnO1xyXG5cdFx0XHR9LmJpbmQodGhpcyksMCk7XHJcblx0XHR9IGVsc2V7XHJcblx0XHRcdHRoaXMuZG9tLnN0YWdlLnN0eWxlLndpZHRoID0gdGhpcy53aWR0aC5zdGFnZSArICdweCc7XHJcblx0XHR9XHJcblxyXG5cdFx0Zm9yKHZhciBpPTA7IGk8dGhpcy5udW0uaXRlbXM7IGkrKyl7XHJcblxyXG5cdFx0XHQvLyBTZXQgaXRlbXMgd2lkdGhcclxuXHRcdFx0aWYoIXRoaXMub3B0aW9ucy5hdXRvV2lkdGgpe1xyXG5cdFx0XHRcdHRoaXMuZG9tLiRpdGVtc1tpXS5zdHlsZS53aWR0aCA9IHRoaXMud2lkdGguaXRlbSAtICh0aGlzLm9wdGlvbnMubWFyZ2luKSArICdweCc7XHJcblx0XHRcdH1cclxuXHRcdFx0Ly8gYWRkIG1hcmdpblxyXG5cdFx0XHRpZih0aGlzLm9wdGlvbnMucnRsKXtcclxuXHRcdFx0XHR0aGlzLmRvbS4kaXRlbXNbaV0uc3R5bGUubWFyZ2luTGVmdCA9IHRoaXMub3B0aW9ucy5tYXJnaW4gKyAncHgnO1xyXG5cdFx0XHR9IGVsc2Uge1xyXG5cdFx0XHRcdHRoaXMuZG9tLiRpdGVtc1tpXS5zdHlsZS5tYXJnaW5SaWdodCA9IHRoaXMub3B0aW9ucy5tYXJnaW4gKyAncHgnO1xyXG5cdFx0XHR9XHJcblx0XHRcdFxyXG5cdFx0XHRpZih0aGlzLm51bS5tZXJnZWRbaV0gIT09IDEgJiYgIXRoaXMub3B0aW9ucy5hdXRvV2lkdGgpe1xyXG5cdFx0XHRcdHRoaXMuZG9tLiRpdGVtc1tpXS5zdHlsZS53aWR0aCA9ICh0aGlzLndpZHRoLml0ZW0gKiB0aGlzLm51bS5tZXJnZWRbaV0pIC0gKHRoaXMub3B0aW9ucy5tYXJnaW4pICsgJ3B4JztcclxuXHRcdFx0fVxyXG5cdFx0fVxyXG5cclxuXHRcdC8vIHNhdmUgcHJldiBzdGFnZSBzaXplIFxyXG5cdFx0dGhpcy53aWR0aC5zdGFnZVByZXYgPSB0aGlzLndpZHRoLnN0YWdlO1xyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIHJlc3BvbnNpdmVcclxuXHQgKiBAZGVzYyBSZXNwb25zaXZlIGZ1bmN0aW9uIHVwZGF0ZSBhbGwgZGF0YSBieSBjYWxsaW5nIHJlZnJlc2goKSBcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS5yZXNwb25zaXZlID0gZnVuY3Rpb24oKXtcclxuXHJcblx0XHRpZighdGhpcy5udW0ub0l0ZW1zKXtyZXR1cm4gZmFsc2U7fVxyXG5cdFx0Ly8gSWYgRWwgd2lkdGggaGFzbnQgY2hhbmdlIHRoZW4gc3RvcCByZXNwb25zaXZlIFxyXG5cdFx0dmFyIGVsQ2hhbmdlZCA9IHRoaXMuaXNFbFdpZHRoQ2hhbmdlZCgpO1xyXG5cdFx0aWYoIWVsQ2hhbmdlZCl7cmV0dXJuIGZhbHNlO31cclxuXHJcblx0XHQvLyBpZiBWaW1lbyBGdWxsc2NyZWVuIG1vZGVcclxuXHRcdHZhciBmdWxsc2NyZWVuRWxlbWVudCA9IGRvY3VtZW50LmZ1bGxzY3JlZW5FbGVtZW50IHx8IGRvY3VtZW50Lm1vekZ1bGxTY3JlZW5FbGVtZW50IHx8IGRvY3VtZW50LndlYmtpdEZ1bGxzY3JlZW5FbGVtZW50O1xyXG5cdFx0aWYoZnVsbHNjcmVlbkVsZW1lbnQpe1xyXG5cdFx0XHRpZigkKGZ1bGxzY3JlZW5FbGVtZW50LnBhcmVudE5vZGUpLmhhc0NsYXNzKCdvd2wtdmlkZW8tZnJhbWUnKSl7XHJcblx0XHRcdFx0dGhpcy5zZXRTcGVlZCgwKTtcclxuXHRcdFx0XHR0aGlzLnN0YXRlLmlzRnVsbFNjcmVlbiA9IHRydWU7XHJcblx0XHRcdH1cclxuXHRcdH1cclxuXHJcblx0XHRpZihmdWxsc2NyZWVuRWxlbWVudCAmJiB0aGlzLnN0YXRlLmlzRnVsbFNjcmVlbiAmJiB0aGlzLnN0YXRlLnZpZGVvUGxheSl7XHJcblx0XHRcdHJldHVybiBmYWxzZTtcclxuXHRcdH1cclxuXHJcblx0XHQvLyBDb21taW5nIGJhY2sgZnJvbSBmdWxsc2NyZWVuXHJcblx0XHRpZih0aGlzLnN0YXRlLmlzRnVsbFNjcmVlbil7XHJcblx0XHRcdHRoaXMuc3RhdGUuaXNGdWxsU2NyZWVuID0gZmFsc2U7XHJcblx0XHRcdHJldHVybiBmYWxzZTtcclxuXHRcdH1cclxuXHJcblx0XHQvLyBjaGVjayBmdWxsIHNjcmVlbiBtb2RlIGFuZCB3aW5kb3cgb3JpZW50YXRpb25cclxuXHRcdGlmICh0aGlzLnN0YXRlLnZpZGVvUGxheSkge1xyXG5cdFx0XHRpZih0aGlzLnN0YXRlLm9yaWVudGF0aW9uICE9PSB3aW5kb3cub3JpZW50YXRpb24pe1xyXG5cdFx0XHRcdHRoaXMuc3RhdGUub3JpZW50YXRpb24gPSB3aW5kb3cub3JpZW50YXRpb247XHJcblx0XHRcdFx0cmV0dXJuIGZhbHNlO1xyXG5cdFx0XHR9XHJcblx0XHR9XHJcblxyXG5cdFx0dGhpcy5maXJlQ2FsbGJhY2soJ29uUmVzcG9uc2l2ZUJlZm9yZScpO1xyXG5cdFx0dGhpcy5zdGF0ZS5yZXNwb25zaXZlID0gdHJ1ZTtcclxuXHRcdHRoaXMucmVmcmVzaCgpO1xyXG5cdFx0dGhpcy5zdGF0ZS5yZXNwb25zaXZlID0gZmFsc2U7XHJcblx0XHR0aGlzLmZpcmVDYWxsYmFjaygnb25SZXNwb25zaXZlQWZ0ZXInKTtcclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiByZWZyZXNoXHJcblx0ICogQGRlc2MgUmVmcmVzaCBtZXRob2QgaXMgYmFzaWNhbGx5IGNvbGxlY3Rpb24gb2YgZnVuY3Rpb25zIHRoYXQgYXJlIHJlc3BvbnNpYmxlIGZvciBPd2wgcmVzcG9uc2l2ZSBmdW5jdGlvbmFsaXR5XHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUucmVmcmVzaCA9IGZ1bmN0aW9uKGluaXQpe1xyXG5cclxuXHRcdGlmKHRoaXMuc3RhdGUudmlkZW9QbGF5KXtcclxuXHRcdFx0dGhpcy5zdG9wVmlkZW8oKTtcclxuXHRcdH1cclxuXHJcblx0XHQvLyBVcGRhdGUgT3B0aW9ucyBmb3IgZ2l2ZW4gd2lkdGhcclxuXHRcdHRoaXMuc2V0UmVzcG9uc2l2ZU9wdGlvbnMoKTtcclxuXHJcblx0XHQvL3NldCBsYXp5IHN0cnVjdHVyZVxyXG5cdFx0dGhpcy5jcmVhdGVMYXp5Q29udGVudFN0cnVjdHVyZSh0cnVlKTtcclxuXHJcblx0XHQvLyB1cGRhdGUgaW5mbyBhYm91dCBsb2NhbCBjb250ZW50XHJcblx0XHR0aGlzLnVwZGF0ZUxvY2FsQ29udGVudCgpO1xyXG5cclxuXHRcdC8vIHVkcGF0ZSBvcHRpb25zXHJcblx0XHR0aGlzLm9wdGlvbnNMb2dpYygpO1xyXG5cclxuXHRcdC8vIGlmIG5vIGl0ZW1zIHRoZW4gc3RvcCBcclxuXHRcdGlmKHRoaXMubnVtLm9JdGVtcyA9PT0gMCl7XHJcblx0XHRcdGlmKHRoaXMuZG9tLiRwYWdlICE9PSBudWxsKXtcclxuXHRcdFx0XHR0aGlzLmRvbS4kcGFnZS5oaWRlKCk7XHJcblx0XHRcdH1cclxuXHRcdFx0cmV0dXJuIGZhbHNlO1xyXG5cdFx0fVxyXG5cclxuXHRcdC8vIEhpZGUgYW5kIFNob3cgbWV0aG9kcyBoZWxwcyBoZXJlIHRvIHNldCBhIHByb3BlciB3aWR0aHMuXHJcblx0XHQvLyBUaGlzIHByZXZlbnRzIFNjcm9sbGJhciB0byBiZSBjYWxjdWxhdGVkIGluIHN0YWdlIHdpZHRoXHJcblx0XHR0aGlzLmRvbS4kc3RhZ2UuYWRkQ2xhc3MoJ293bC1yZWZyZXNoJyk7XHJcblx0XHRcclxuXHRcdC8vIFJlbW92ZSBjbG9uZXMgYW5kIGdlbmVyYXRlIG5ldyBvbmVzXHJcblx0XHR0aGlzLnJlQ2xvbmUoKTtcclxuXHJcblx0XHQvLyBjYWxjdWxhdGUgXHJcblx0XHR0aGlzLmNhbGN1bGF0ZSgpO1xyXG5cclxuXHRcdC8vYWFhYW5kIHNob3cuXHJcblx0XHR0aGlzLmRvbS4kc3RhZ2UucmVtb3ZlQ2xhc3MoJ293bC1yZWZyZXNoJyk7XHJcblxyXG5cdFx0Ly8gdG8gZG9cclxuXHRcdC8vIGxhenlDb250ZW50IGxhc3QgcG9zaXRpb24gb24gcmVmcmVzaFxyXG5cdFx0aWYodGhpcy5zdGF0ZS5sYXp5Q29udGVudCl7XHJcblx0XHRcdHRoaXMucG9zLmN1cnJlbnRBYnMgPSB0aGlzLm9wdGlvbnMuaXRlbXM7XHJcblx0XHR9XHJcblxyXG5cdFx0dGhpcy5pbml0UG9zaXRpb24oaW5pdCk7XHJcblxyXG5cdFx0Ly8ganVtcCB0byBsYXN0IHBvc2l0aW9uIFxyXG5cdFx0aWYoIXRoaXMuc3RhdGUubGF6eUNvbnRlbnQgJiYgIWluaXQpe1xyXG5cdFx0XHR0aGlzLmp1bXBUbyh0aGlzLnBvcy5jdXJyZW50LGZhbHNlKTsgLy8gZml4IHRoYXQgXHJcblx0XHR9XHJcblxyXG5cdFx0Ly9DaGVjayBmb3IgdmlkZW9zICggWW91VHViZSBhbmQgVmltZW8gY3VycmVudGx5IHN1cHBvcnRlZClcclxuXHRcdHRoaXMuY2hlY2tWaWRlb0xpbmtzKCk7XHJcblxyXG5cdFx0dGhpcy51cGRhdGVJdGVtU3RhdGUoKTtcclxuXHJcblx0XHQvLyBVcGRhdGUgY29udHJvbHNcclxuXHRcdHRoaXMucmVidWlsZERvdHMoKTtcclxuXHJcblx0XHR0aGlzLnVwZGF0ZUNvbnRyb2xzKCk7XHJcblxyXG5cdFx0Ly8gdXBkYXRlIGRyYWcgZXZlbnRzXHJcblx0XHQvL3RoaXMudXBkYXRlRXZlbnRzKCk7XHJcblxyXG5cdFx0Ly8gdXBkYXRlIGF1dG9wbGF5XHJcblx0XHR0aGlzLmF1dG9wbGF5KCk7XHJcblxyXG5cdFx0dGhpcy5hdXRvSGVpZ2h0KCk7XHJcblxyXG5cdFx0dGhpcy5zdGF0ZS5vcmllbnRhdGlvbiA9IHdpbmRvdy5vcmllbnRhdGlvbjtcclxuXHJcblx0XHR0aGlzLndhdGNoVmlzaWJpbGl0eSgpO1xyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIHVwZGF0ZUl0ZW1TdGF0ZVxyXG5cdCAqIEBkZXNjIFVwZGF0ZSBpbmZvcm1hdGlvbiBhYm91dCBjdXJyZW50IHN0YXRlIG9mIGl0ZW1zICh2aXNpYmlsZSwgaGlkZGVuLCBhY3RpdmUsIGV0Yy4pXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUudXBkYXRlSXRlbVN0YXRlID0gZnVuY3Rpb24odXBkYXRlKXtcclxuXHJcblx0XHRpZighdGhpcy5zdGF0ZS5sYXp5Q29udGVudCl7XHJcblx0XHRcdHRoaXMudXBkYXRlQWN0aXZlSXRlbXMoKTtcclxuXHRcdH0gZWxzZSB7XHJcblx0XHRcdHRoaXMudXBkYXRlTGF6eUNvbnRlbnQodXBkYXRlKTtcclxuXHRcdH1cclxuXHJcblx0XHRpZih0aGlzLm9wdGlvbnMuY2VudGVyKXtcclxuXHRcdFx0dGhpcy5kb20uJGl0ZW1zLmVxKHRoaXMucG9zLmN1cnJlbnRBYnMpXHJcblx0XHRcdC5hZGRDbGFzcyh0aGlzLm9wdGlvbnMuY2VudGVyQ2xhc3MpXHJcblx0XHRcdC5kYXRhKCdvd2wtaXRlbScpLmNlbnRlciA9IHRydWU7XHJcblx0XHR9XHJcblxyXG5cdFx0aWYodGhpcy5vcHRpb25zLmxhenlMb2FkKXtcclxuXHRcdFx0dGhpcy5sYXp5TG9hZCgpO1xyXG5cdFx0fVxyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIHVwZGF0ZUFjdGl2ZUl0ZW1zXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cclxuXHRPd2wucHJvdG90eXBlLnVwZGF0ZUFjdGl2ZUl0ZW1zID0gZnVuY3Rpb24oKXtcclxuXHRcdHZhciBpLGosaXRlbSxpcG9zLGl3aWR0aCx3cG9zLHN0YWdlLG91dHNpZGVWaWV3LGZvdW5kQ3VycmVudCxzdGFnZVgsdmlldztcclxuXHRcdC8vIGNsZWFyIHN0YXRlc1xyXG5cdFx0Zm9yKGkgPSAwOyBpPHRoaXMubnVtLml0ZW1zOyBpKyspe1xyXG5cdFx0XHR0aGlzLmRvbS4kaXRlbXMuZXEoaSkuZGF0YSgnb3dsLWl0ZW0nKS5hY3RpdmUgPSBmYWxzZTtcclxuXHRcdFx0dGhpcy5kb20uJGl0ZW1zLmVxKGkpLmRhdGEoJ293bC1pdGVtJykuY3VycmVudCA9IGZhbHNlO1xyXG5cdFx0XHR0aGlzLmRvbS4kaXRlbXMuZXEoaSkucmVtb3ZlQ2xhc3ModGhpcy5vcHRpb25zLmFjdGl2ZUNsYXNzKS5yZW1vdmVDbGFzcyh0aGlzLm9wdGlvbnMuY2VudGVyQ2xhc3MpO1xyXG5cdFx0fVxyXG5cclxuXHRcdHRoaXMubnVtLmFjdGl2ZSA9IDA7XHJcblx0XHRzdGFnZVggPSB0aGlzLnBvcy5zdGFnZTtcclxuXHRcdHZpZXcgPSB0aGlzLm9wdGlvbnMucnRsID8gdGhpcy53aWR0aC52aWV3IDogLXRoaXMud2lkdGgudmlldztcclxuXHJcblx0XHRmb3IoaiA9IDA7IGo8dGhpcy5udW0uaXRlbXM7IGorKyl7XHJcblxyXG5cdFx0XHRcdGl0ZW0gPSB0aGlzLmRvbS4kaXRlbXMuZXEoaik7XHJcblx0XHRcdFx0aXBvcyA9IGl0ZW0uZGF0YSgnb3dsLWl0ZW0nKS5wb3NMZWZ0O1xyXG5cdFx0XHRcdGl3aWR0aCA9IGl0ZW0uZGF0YSgnb3dsLWl0ZW0nKS53aWR0aDtcclxuXHRcdFx0XHRvdXRzaWRlVmlldyA9IHRoaXMub3B0aW9ucy5ydGwgPyBpcG9zICsgaXdpZHRoIDogaXBvcyAtIGl3aWR0aDtcclxuXHJcblx0XHRcdGlmKCAodGhpcy5vcChpcG9zLCc8PScsc3RhZ2VYKSAmJiAodGhpcy5vcChpcG9zLCc+JyxzdGFnZVggKyB2aWV3KSkpIHx8IFxyXG5cdFx0XHRcdCh0aGlzLm9wKG91dHNpZGVWaWV3LCc8JyxzdGFnZVgpICYmIHRoaXMub3Aob3V0c2lkZVZpZXcsJz4nLHN0YWdlWCArIHZpZXcpKSBcclxuXHRcdFx0XHQpe1xyXG5cclxuXHRcdFx0XHR0aGlzLm51bS5hY3RpdmUrKztcclxuXHJcblx0XHRcdFx0aWYodGhpcy5vcHRpb25zLmZyZWVEcmFnICYmICFmb3VuZEN1cnJlbnQpe1xyXG5cdFx0XHRcdFx0Zm91bmRDdXJyZW50ID0gdHJ1ZTtcclxuXHRcdFx0XHRcdHRoaXMucG9zLmN1cnJlbnQgPSBpdGVtLmRhdGEoJ293bC1pdGVtJykuaW5kZXg7XHJcblx0XHRcdFx0XHR0aGlzLnBvcy5jdXJyZW50QWJzID0gaXRlbS5kYXRhKCdvd2wtaXRlbScpLmluZGV4QWJzO1xyXG5cdFx0XHRcdH1cclxuXHJcblx0XHRcdFx0aXRlbS5kYXRhKCdvd2wtaXRlbScpLmFjdGl2ZSA9IHRydWU7XHJcblx0XHRcdFx0aXRlbS5kYXRhKCdvd2wtaXRlbScpLmN1cnJlbnQgPSB0cnVlO1xyXG5cdFx0XHRcdGl0ZW0uYWRkQ2xhc3ModGhpcy5vcHRpb25zLmFjdGl2ZUNsYXNzKTtcclxuXHJcblx0XHRcdFx0aWYoIXRoaXMub3B0aW9ucy5sYXp5TG9hZCl7XHJcblx0XHRcdFx0XHRpdGVtLmRhdGEoJ293bC1pdGVtJykubG9hZGVkID0gdHJ1ZTtcclxuXHRcdFx0XHR9XHJcblx0XHRcdFx0aWYodGhpcy5vcHRpb25zLmxvb3AgJiYgKHRoaXMub3B0aW9ucy5sYXp5TG9hZCB8fCB0aGlzLm9wdGlvbnMuY2VudGVyKSl7XHJcblx0XHRcdFx0XHR0aGlzLnVwZGF0ZUNsb25lZEl0ZW1zU3RhdGUoaXRlbS5kYXRhKCdvd2wtaXRlbScpLmluZGV4KTtcclxuXHRcdFx0XHR9XHJcblx0XHRcdH1cclxuXHRcdH1cclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiB1cGRhdGVDbG9uZWRJdGVtc1N0YXRlXHJcblx0ICogQGRlc2MgU2V0IGN1cnJlbnQgc3RhdGUgb24gc2liaWxpbmdzIGl0ZW1zIGZvciBsYXp5TG9hZCBhbmQgY2VudGVyXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUudXBkYXRlQ2xvbmVkSXRlbXNTdGF0ZSA9IGZ1bmN0aW9uKGFjdGl2ZUluZGV4KXtcclxuXHJcblx0XHQvL2ZpbmQgY2xvbmVkIGNlbnRlclxyXG5cdFx0dmFyIGNlbnRlciwgJGVsLGk7XHJcblx0XHRpZih0aGlzLm9wdGlvbnMuY2VudGVyKXtcclxuXHRcdFx0Y2VudGVyID0gdGhpcy5kb20uJGl0ZW1zLmVxKHRoaXMucG9zLmN1cnJlbnRBYnMpLmRhdGEoJ293bC1pdGVtJykuaW5kZXg7XHJcblx0XHR9XHJcblxyXG5cdFx0Zm9yKGkgPSAwOyBpPHRoaXMubnVtLml0ZW1zOyBpKyspe1xyXG5cdFx0XHQkZWwgPSB0aGlzLmRvbS4kaXRlbXMuZXEoaSk7XHJcblx0XHRcdGlmKCAkZWwuZGF0YSgnb3dsLWl0ZW0nKS5pbmRleCA9PT0gYWN0aXZlSW5kZXggKXtcclxuXHRcdFx0XHQkZWwuZGF0YSgnb3dsLWl0ZW0nKS5jdXJyZW50ID0gdHJ1ZTtcclxuXHRcdFx0XHRpZigkZWwuZGF0YSgnb3dsLWl0ZW0nKS5pbmRleCA9PT0gY2VudGVyICl7XHJcblx0XHRcdFx0XHQkZWwuYWRkQ2xhc3ModGhpcy5vcHRpb25zLmNlbnRlckNsYXNzKTtcclxuXHRcdFx0XHR9XHJcblx0XHRcdH1cclxuXHRcdH1cclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiB1cGRhdGVMYXp5UG9zaXRpb25cclxuXHQgKiBAZGVzYyBTZXQgY3VycmVudCBzdGF0ZSBvbiBzaWJpbGluZ3MgaXRlbXMgZm9yIGxhenlMb2FkIGFuZCBjZW50ZXJcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS51cGRhdGVMYXp5UG9zaXRpb24gPSBmdW5jdGlvbigpe1xyXG5cdFx0dmFyIGp1bXBUbyA9IHRoaXMucG9zLmdvVG9MYXp5Q29udGVudCB8fCAwO1xyXG5cclxuXHRcdHRoaXMucG9zLmxjTW92ZWRCeSA9IE1hdGguYWJzKHRoaXMub3B0aW9ucy5pdGVtcyAtIHRoaXMucG9zLmN1cnJlbnRBYnMpO1xyXG5cclxuXHRcdGlmKHRoaXMub3B0aW9ucy5pdGVtcyA8IHRoaXMucG9zLmN1cnJlbnRBYnMgKXtcclxuXHRcdFx0dGhpcy5wb3MubGNDdXJyZW50ICs9IHRoaXMucG9zLmN1cnJlbnRBYnMgLSB0aGlzLm9wdGlvbnMuaXRlbXM7XHJcblx0XHRcdHRoaXMuc3RhdGUubGNEaXJlY3Rpb24gPSAncmlnaHQnO1xyXG5cdFx0fSBlbHNlIGlmKHRoaXMub3B0aW9ucy5pdGVtcyA+IHRoaXMucG9zLmN1cnJlbnRBYnMgKXtcclxuXHRcdFx0dGhpcy5wb3MubGNDdXJyZW50IC09IHRoaXMub3B0aW9ucy5pdGVtcyAtIHRoaXMucG9zLmN1cnJlbnRBYnM7XHJcblx0XHRcdHRoaXMuc3RhdGUubGNEaXJlY3Rpb24gPSAnbGVmdCc7XHJcblx0XHR9XHJcblxyXG5cdFx0dGhpcy5wb3MubGNDdXJyZW50ID0ganVtcFRvICE9PSAwID8ganVtcFRvIDogdGhpcy5wb3MubGNDdXJyZW50O1xyXG5cclxuXHRcdGlmKHRoaXMucG9zLmxjQ3VycmVudCA+PSB0aGlzLmRvbS4kY29udGVudC5sZW5ndGgpe1xyXG5cdFx0XHR0aGlzLnBvcy5sY0N1cnJlbnQgPSB0aGlzLnBvcy5sY0N1cnJlbnQtdGhpcy5kb20uJGNvbnRlbnQubGVuZ3RoO1xyXG5cdFx0fSBlbHNlIGlmKHRoaXMucG9zLmxjQ3VycmVudCA8IC10aGlzLmRvbS4kY29udGVudC5sZW5ndGgrMSl7XHJcblx0XHRcdHRoaXMucG9zLmxjQ3VycmVudCA9IHRoaXMucG9zLmxjQ3VycmVudCt0aGlzLmRvbS4kY29udGVudC5sZW5ndGg7XHJcblx0XHR9XHJcblxyXG5cdFx0aWYodGhpcy5vcHRpb25zLnN0YXJ0UG9zaXRpb24+MCl7XHJcblx0XHRcdHRoaXMucG9zLmxjQ3VycmVudCA9IHRoaXMub3B0aW9ucy5zdGFydFBvc2l0aW9uO1xyXG5cdFx0XHR0aGlzLl9vcHRpb25zLnN0YXJ0UG9zaXRpb24gPSB0aGlzLm9wdGlvbnMuc3RhcnRQb3NpdGlvbiA9IDA7XHJcblx0XHR9XHJcblxyXG5cdFx0dGhpcy5wb3MubGNDdXJyZW50QWJzID0gdGhpcy5wb3MubGNDdXJyZW50IDwgMCA/IHRoaXMucG9zLmxjQ3VycmVudCt0aGlzLmRvbS4kY29udGVudC5sZW5ndGggOiB0aGlzLnBvcy5sY0N1cnJlbnQ7XHJcblxyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIHVwZGF0ZUxhenlDb250ZW50XHJcblx0ICogQHBhcmFtIFt1cGRhdGVdIC0gYm9vbGVhbiAtIHVwZGF0ZSBjYWxsIGJ5IGNvbnRlbnQgbWFuaXB1bGF0aW9uc1xyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqL1xyXG5cclxuXHRPd2wucHJvdG90eXBlLnVwZGF0ZUxhenlDb250ZW50ID0gZnVuY3Rpb24odXBkYXRlKXtcclxuXHJcblx0XHRpZih0aGlzLnBvcy5sY0N1cnJlbnQgPT09IHVuZGVmaW5lZCl7XHJcblx0XHRcdHRoaXMucG9zLmxjQ3VycmVudCA9IDA7XHJcblx0XHRcdHRoaXMucG9zLmN1cnJlbnQgPSB0aGlzLnBvcy5jdXJyZW50QWJzID0gdGhpcy5vcHRpb25zLml0ZW1zO1xyXG5cdFx0fVxyXG5cclxuXHRcdGlmKCF1cGRhdGUpe1xyXG5cdFx0XHR0aGlzLnVwZGF0ZUxhenlQb3NpdGlvbigpO1xyXG5cdFx0fVxyXG5cdFx0dmFyIGksaixpdGVtLGNvbnRlbnRQb3MsY29udGVudCxmcmVzaEl0ZW0sZnJlc2hEYXRhO1xyXG5cclxuXHRcdGlmKHRoaXMuc3RhdGUubGNEaXJlY3Rpb24gIT09IGZhbHNlKXtcclxuXHRcdFx0Zm9yKGkgPSAwOyBpPHRoaXMucG9zLmxjTW92ZWRCeTsgaSsrKXtcclxuXHJcblx0XHRcdFx0aWYodGhpcy5zdGF0ZS5sY0RpcmVjdGlvbiA9PT0gJ3JpZ2h0Jyl7XHJcblx0XHRcdFx0XHRpdGVtID0gdGhpcy5kb20uJHN0YWdlLmZpbmQoJy5vd2wtaXRlbScpLmVxKDApOyAvLy5hcHBlbmRUbyh0aGlzLmRvbS4kc3RhZ2UpO1xyXG5cdFx0XHRcdFx0aXRlbS5hcHBlbmRUbyh0aGlzLmRvbS4kc3RhZ2UpO1xyXG5cdFx0XHRcdH1cclxuXHRcdFx0XHRpZih0aGlzLnN0YXRlLmxjRGlyZWN0aW9uID09PSAnbGVmdCcpe1xyXG5cdFx0XHRcdFx0aXRlbSA9IHRoaXMuZG9tLiRzdGFnZS5maW5kKCcub3dsLWl0ZW0nKS5lcSgtMSk7XHJcblx0XHRcdFx0XHRpdGVtLnByZXBlbmRUbyh0aGlzLmRvbS4kc3RhZ2UpO1xyXG5cdFx0XHRcdH1cclxuXHRcdFx0XHRpdGVtLmRhdGEoJ293bC1pdGVtJykuYWN0aXZlID0gZmFsc2U7XHJcblx0XHRcdH1cclxuXHRcdH1cclxuXHJcblx0XHQvLyByZWNvbGxlY3QgXHJcblx0XHR0aGlzLmRvbS4kaXRlbXMgPSB0aGlzLmRvbS4kc3RhZ2UuZmluZCgnLm93bC1pdGVtJyk7XHJcblxyXG5cdFx0Zm9yKGogPSAwOyBqPHRoaXMubnVtLml0ZW1zOyBqKyspe1xyXG5cdFx0XHQvLyB0byBkb1xyXG5cdFx0XHR0aGlzLmRvbS4kaXRlbXMuZXEoaikucmVtb3ZlQ2xhc3ModGhpcy5vcHRpb25zLmNlbnRlckNsYXNzKTtcclxuXHJcblx0XHRcdC8vIGdldCBDb250ZW50IFxyXG5cdFx0XHRjb250ZW50UG9zID0gdGhpcy5wb3MubGNDdXJyZW50ICsgaiAtIHRoaXMub3B0aW9ucy5pdGVtczsvLyArIHRoaXMub3B0aW9ucy5zdGFydFBvc2l0aW9uO1xyXG5cclxuXHRcdFx0aWYoY29udGVudFBvcyA+PSB0aGlzLmRvbS4kY29udGVudC5sZW5ndGgpe1xyXG5cdFx0XHRcdGNvbnRlbnRQb3MgPSBjb250ZW50UG9zIC0gdGhpcy5kb20uJGNvbnRlbnQubGVuZ3RoO1xyXG5cdFx0XHR9XHJcblx0XHRcdGlmKGNvbnRlbnRQb3MgPCAtdGhpcy5kb20uJGNvbnRlbnQubGVuZ3RoKXtcclxuXHRcdFx0XHRjb250ZW50UG9zID0gY29udGVudFBvcyArIHRoaXMuZG9tLiRjb250ZW50Lmxlbmd0aDtcclxuXHRcdFx0fVxyXG5cclxuXHRcdFx0Y29udGVudCA9IHRoaXMuZG9tLiRjb250ZW50LmVxKGNvbnRlbnRQb3MpO1xyXG5cdFx0XHRmcmVzaEl0ZW0gPSB0aGlzLmRvbS4kaXRlbXMuZXEoaik7XHJcblx0XHRcdGZyZXNoRGF0YSA9IGZyZXNoSXRlbS5kYXRhKCdvd2wtaXRlbScpO1xyXG5cclxuXHRcdFx0aWYoZnJlc2hEYXRhLmFjdGl2ZSA9PT0gZmFsc2UgfHwgdGhpcy5wb3MuZ29Ub0xhenlDb250ZW50ICE9PSAwIHx8IHVwZGF0ZSA9PT0gdHJ1ZSl7XHJcblxyXG5cdFx0XHRcdGZyZXNoSXRlbS5lbXB0eSgpO1xyXG5cdFx0XHRcdGZyZXNoSXRlbS5hcHBlbmQoY29udGVudC5jbG9uZSh0cnVlLHRydWUpKTtcclxuXHRcdFx0XHRmcmVzaERhdGEuYWN0aXZlID0gdHJ1ZTtcclxuXHRcdFx0XHRmcmVzaERhdGEuY3VycmVudCA9IHRydWU7XHJcblx0XHRcdFx0aWYoIXRoaXMub3B0aW9ucy5sYXp5TG9hZCl7XHJcblx0XHRcdFx0XHRmcmVzaERhdGEubG9hZGVkID0gdHJ1ZTtcclxuXHRcdFx0XHR9IGVsc2Uge1xyXG5cdFx0XHRcdFx0ZnJlc2hEYXRhLmxvYWRlZCA9IGZhbHNlO1xyXG5cdFx0XHRcdH1cclxuXHRcdFx0fVxyXG5cdFx0fVxyXG5cclxuXHRcdHRoaXMucG9zLmdvVG9MYXp5Q29udGVudCA9IDA7XHJcblx0XHR0aGlzLnBvcy5jdXJyZW50ID0gdGhpcy5wb3MuY3VycmVudEFicyA9IHRoaXMub3B0aW9ucy5pdGVtcztcclxuXHRcdHRoaXMuc2V0U3BlZWQoMCk7XHJcblx0XHR0aGlzLmFuaW1TdGFnZSh0aGlzLnBvcy5pdGVtc1t0aGlzLm9wdGlvbnMuaXRlbXNdKTtcclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiBldmVudHNDYWxsXHJcblx0ICogQGRlc2MgU2F2ZSBpbnRlcm5hbCBldmVudCByZWZlcmVuY2VzIGFuZCBhZGQgZXZlbnQgYmFzZWQgZnVuY3Rpb25zIGxpa2UgdHJhbnNpdGlvbkVuZCxyZXNwb25zaXZlIGV0Yy5cclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS5ldmVudHNDYWxsID0gZnVuY3Rpb24oKXtcclxuXHRcdC8vIFNhdmUgZXZlbnRzIHJlZmVyZW5jZXMgXHJcblx0XHR0aGlzLmUuX29uRHJhZ1N0YXJ0ID1cdGZ1bmN0aW9uKGUpe3RoaXMub25EcmFnU3RhcnQoZSk7XHRcdH0uYmluZCh0aGlzKTtcclxuXHRcdHRoaXMuZS5fb25EcmFnTW92ZSA9XHRmdW5jdGlvbihlKXt0aGlzLm9uRHJhZ01vdmUoZSk7XHRcdFx0fS5iaW5kKHRoaXMpO1xyXG5cdFx0dGhpcy5lLl9vbkRyYWdFbmQgPVx0XHRmdW5jdGlvbihlKXt0aGlzLm9uRHJhZ0VuZChlKTtcdFx0XHR9LmJpbmQodGhpcyk7XHJcblx0XHR0aGlzLmUuX3RyYW5zaXRpb25FbmQgPVx0ZnVuY3Rpb24oZSl7dGhpcy50cmFuc2l0aW9uRW5kKGUpO1x0XHR9LmJpbmQodGhpcyk7XHJcblx0XHR0aGlzLmUuX3Jlc2l6ZXIgPVx0XHRmdW5jdGlvbigpe3RoaXMucmVzcG9uc2l2ZVRpbWVyKCk7XHRcdH0uYmluZCh0aGlzKTtcclxuXHRcdHRoaXMuZS5fcmVzcG9uc2l2ZUNhbGwgPWZ1bmN0aW9uKCl7dGhpcy5yZXNwb25zaXZlKCk7XHRcdFx0fS5iaW5kKHRoaXMpO1xyXG5cdFx0dGhpcy5lLl9wcmV2ZW50Q2xpY2sgPVx0ZnVuY3Rpb24oZSl7dGhpcy5wcmV2ZW50Q2xpY2soZSk7XHRcdH0uYmluZCh0aGlzKTtcclxuXHRcdHRoaXMuZS5fZ29Ub0hhc2ggPVx0XHRmdW5jdGlvbigpe3RoaXMuZ29Ub0hhc2goKTtcdFx0XHRcdH0uYmluZCh0aGlzKTtcclxuXHRcdHRoaXMuZS5fZ29Ub1BhZ2UgPVx0XHRmdW5jdGlvbihlKXt0aGlzLmdvVG9QYWdlKGUpO1x0XHRcdH0uYmluZCh0aGlzKTtcclxuXHRcdHRoaXMuZS5fYXAgPSBcdFx0XHRmdW5jdGlvbigpe3RoaXMuYXV0b3BsYXkoKTtcdFx0XHRcdH0uYmluZCh0aGlzKTtcclxuXHRcdHRoaXMuZS5fcGxheSA9IFx0XHRcdGZ1bmN0aW9uKCl7dGhpcy5wbGF5KCk7XHRcdFx0XHRcdH0uYmluZCh0aGlzKTtcclxuXHRcdHRoaXMuZS5fcGF1c2UgPSBcdFx0ZnVuY3Rpb24oKXt0aGlzLnBhdXNlKCk7XHRcdFx0XHR9LmJpbmQodGhpcyk7XHJcblx0XHR0aGlzLmUuX3BsYXlWaWRlbyA9IFx0ZnVuY3Rpb24oZSl7dGhpcy5wbGF5VmlkZW8oZSk7XHRcdFx0fS5iaW5kKHRoaXMpO1xyXG5cclxuXHRcdHRoaXMuZS5fbmF2TmV4dCA9IGZ1bmN0aW9uKGUpe1xyXG5cdFx0XHRpZigkKGUudGFyZ2V0KS5oYXNDbGFzcygnZGlzYWJsZWQnKSl7cmV0dXJuIGZhbHNlO31cclxuXHRcdFx0ZS5wcmV2ZW50RGVmYXVsdCgpO1xyXG5cdFx0XHR0aGlzLm5leHQoKTtcdFx0XHRcdFxyXG5cdFx0fS5iaW5kKHRoaXMpO1xyXG5cclxuXHRcdHRoaXMuZS5fbmF2UHJldiA9IGZ1bmN0aW9uKGUpe1xyXG5cdFx0XHRpZigkKGUudGFyZ2V0KS5oYXNDbGFzcygnZGlzYWJsZWQnKSl7cmV0dXJuIGZhbHNlO31cclxuXHRcdFx0ZS5wcmV2ZW50RGVmYXVsdCgpO1xyXG5cdFx0XHR0aGlzLnByZXYoKTtcclxuXHRcdH0uYmluZCh0aGlzKTtcclxuXHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICogcmVzcG9uc2l2ZVRpbWVyXHJcblx0ICogQGRlc2MgQ2hlY2sgV2luZG93IHJlc2l6ZSBldmVudCB3aXRoIDIwMG1zIGRlbGF5IC8gdGhpcy5vcHRpb25zLnJlc3BvbnNpdmVSZWZyZXNoUmF0ZVxyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqL1xyXG5cclxuXHRPd2wucHJvdG90eXBlLnJlc3BvbnNpdmVUaW1lciA9IGZ1bmN0aW9uKCl7XHJcblx0XHRpZih0aGlzLndpbmRvd1dpZHRoKCkgPT09IHRoaXMud2lkdGgucHJldldpbmRvdyl7XHJcblx0XHRcdHJldHVybiBmYWxzZTtcclxuXHRcdH1cclxuXHRcdHdpbmRvdy5jbGVhckludGVydmFsKHRoaXMuZS5fYXV0b3BsYXkpO1xyXG5cdFx0d2luZG93LmNsZWFyVGltZW91dCh0aGlzLnJlc2l6ZVRpbWVyKTtcclxuXHRcdHRoaXMucmVzaXplVGltZXIgPSB3aW5kb3cuc2V0VGltZW91dCh0aGlzLmUuX3Jlc3BvbnNpdmVDYWxsLCB0aGlzLm9wdGlvbnMucmVzcG9uc2l2ZVJlZnJlc2hSYXRlKTtcclxuXHRcdHRoaXMud2lkdGgucHJldldpbmRvdyA9IHRoaXMud2luZG93V2lkdGgoKTtcclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiBpbnRlcm5hbEV2ZW50c1xyXG5cdCAqIEBkZXNjIENoZWNrcyBmb3IgdG91Y2gvbW91c2UgZHJhZyBvcHRpb25zIGFuZCBhZGQgbmVjZXNzZXJ5IGV2ZW50IGhhbmRsZXJzLlxyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqL1xyXG5cclxuXHRPd2wucHJvdG90eXBlLmludGVybmFsRXZlbnRzID0gZnVuY3Rpb24oKXtcclxuXHRcdHZhciBpc1RvdWNoID0gaXNUb3VjaFN1cHBvcnQoKTtcclxuXHRcdHZhciBpc1RvdWNoSUUgPSBpc1RvdWNoU3VwcG9ydElFKCk7XHJcblxyXG5cdFx0aWYoaXNUb3VjaCAmJiAhaXNUb3VjaElFKXtcclxuXHRcdFx0dGhpcy5kcmFnVHlwZSA9IFsndG91Y2hzdGFydCcsJ3RvdWNobW92ZScsJ3RvdWNoZW5kJywndG91Y2hjYW5jZWwnXTtcclxuXHRcdH0gZWxzZSBpZihpc1RvdWNoICYmIGlzVG91Y2hJRSl7XHJcblx0XHRcdHRoaXMuZHJhZ1R5cGUgPSBbJ01TUG9pbnRlckRvd24nLCdNU1BvaW50ZXJNb3ZlJywnTVNQb2ludGVyVXAnLCdNU1BvaW50ZXJDYW5jZWwnXTtcclxuXHRcdH0gZWxzZSB7XHJcblx0XHRcdHRoaXMuZHJhZ1R5cGUgPSBbJ21vdXNlZG93bicsJ21vdXNlbW92ZScsJ21vdXNldXAnXTtcclxuXHRcdH1cclxuXHJcblx0XHRpZiggKGlzVG91Y2ggfHwgaXNUb3VjaElFKSAmJiB0aGlzLm9wdGlvbnMudG91Y2hEcmFnKXtcclxuXHRcdFx0Ly90b3VjaCBjYW5jZWwgZXZlbnQgXHJcblx0XHRcdHRoaXMub24oZG9jdW1lbnQsIHRoaXMuZHJhZ1R5cGVbM10sIHRoaXMuZS5fb25EcmFnRW5kKTtcclxuXHJcblx0XHR9IGVsc2Uge1xyXG5cdFx0XHQvLyBmaXJlZm94IHN0YXJ0ZHJhZyBmaXggLSBhZGRldmVudGxpc3RlbmVyIGRvZXNudCB3b3JrIGhlcmUgOi9cclxuXHRcdFx0dGhpcy5kb20uJHN0YWdlLm9uKCdkcmFnc3RhcnQnLCBmdW5jdGlvbigpIHtyZXR1cm4gZmFsc2U7fSk7XHJcblxyXG5cdFx0XHRpZih0aGlzLm9wdGlvbnMubW91c2VEcmFnKXtcclxuXHRcdFx0XHQvL2Rpc2FibGUgdGV4dCBzZWxlY3RcclxuXHRcdFx0XHR0aGlzLmRvbS5zdGFnZS5vbnNlbGVjdHN0YXJ0ID0gZnVuY3Rpb24oKXtyZXR1cm4gZmFsc2U7fTtcclxuXHRcdFx0fSBlbHNlIHtcclxuXHRcdFx0XHQvLyBlbmFibGUgdGV4dCBzZWxlY3RcclxuXHRcdFx0XHR0aGlzLmRvbS4kZWwuYWRkQ2xhc3MoJ293bC10ZXh0LXNlbGVjdC1vbicpO1xyXG5cdFx0XHR9XHJcblx0XHR9XHJcblxyXG5cdFx0Ly8gVmlkZW8gUGxheSBCdXR0b24gZXZlbnQgZGVsZWdhdGlvblxyXG5cdFx0dGhpcy5kb20uJHN0YWdlLm9uKHRoaXMuZHJhZ1R5cGVbMl0sICcub3dsLXZpZGVvLXBsYXktaWNvbicsIHRoaXMuZS5fcGxheVZpZGVvKTtcclxuXHJcblx0XHRpZih0aGlzLm9wdGlvbnMuVVJMaGFzaExpc3RlbmVyKXtcclxuXHRcdFx0dGhpcy5vbih3aW5kb3csICdoYXNoY2hhbmdlJywgdGhpcy5lLl9nb1RvSGFzaCwgZmFsc2UpO1xyXG5cdFx0fVxyXG5cclxuXHRcdGlmKHRoaXMub3B0aW9ucy5hdXRvcGxheUhvdmVyUGF1c2Upe1xyXG5cdFx0XHR2YXIgdGhhdCA9IHRoaXM7XHJcblx0XHRcdHRoaXMuZG9tLiRzdGFnZS5vbignbW91c2VvdmVyJywgdGhpcy5lLl9wYXVzZSApO1xyXG5cdFx0XHR0aGlzLmRvbS4kc3RhZ2Uub24oJ21vdXNlbGVhdmUnLCB0aGlzLmUuX2FwICk7XHJcblx0XHR9XHJcblxyXG5cdFx0Ly8gQ2F0Y2ggdHJhbnNpdGlvbkVuZCBldmVudFxyXG5cdFx0aWYodGhpcy50cmFuc2l0aW9uRW5kVmVuZG9yKXtcclxuXHRcdFx0dGhpcy5vbih0aGlzLmRvbS5zdGFnZSwgdGhpcy50cmFuc2l0aW9uRW5kVmVuZG9yLCB0aGlzLmUuX3RyYW5zaXRpb25FbmQsIGZhbHNlKTtcclxuXHRcdH1cclxuXHRcdFxyXG5cdFx0Ly8gUmVzcG9uc2l2ZVxyXG5cdFx0aWYodGhpcy5vcHRpb25zLnJlc3BvbnNpdmUgIT09IGZhbHNlKXtcclxuXHRcdFx0dGhpcy5vbih3aW5kb3csICdyZXNpemUnLCB0aGlzLmUuX3Jlc2l6ZXIsIGZhbHNlKTtcclxuXHRcdH1cclxuXHJcblx0XHR0aGlzLnVwZGF0ZUV2ZW50cygpO1xyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIHVwZGF0ZUV2ZW50c1xyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqL1xyXG5cclxuXHRPd2wucHJvdG90eXBlLnVwZGF0ZUV2ZW50cyA9IGZ1bmN0aW9uKCl7XHJcblxyXG5cdFx0aWYodGhpcy5vcHRpb25zLnRvdWNoRHJhZyAmJiAodGhpcy5kcmFnVHlwZVswXSA9PT0gJ3RvdWNoc3RhcnQnIHx8IHRoaXMuZHJhZ1R5cGVbMF0gPT09ICdNU1BvaW50ZXJEb3duJykpe1xyXG5cdFx0XHR0aGlzLm9uKHRoaXMuZG9tLnN0YWdlLCB0aGlzLmRyYWdUeXBlWzBdLCB0aGlzLmUuX29uRHJhZ1N0YXJ0LGZhbHNlKTtcclxuXHRcdH0gZWxzZSBpZih0aGlzLm9wdGlvbnMubW91c2VEcmFnICYmIHRoaXMuZHJhZ1R5cGVbMF0gPT09ICdtb3VzZWRvd24nKXtcclxuXHRcdFx0dGhpcy5vbih0aGlzLmRvbS5zdGFnZSwgdGhpcy5kcmFnVHlwZVswXSwgdGhpcy5lLl9vbkRyYWdTdGFydCxmYWxzZSk7XHJcblxyXG5cdFx0fSBlbHNlIHtcclxuXHRcdFx0dGhpcy5vZmYodGhpcy5kb20uc3RhZ2UsIHRoaXMuZHJhZ1R5cGVbMF0sIHRoaXMuZS5fb25EcmFnU3RhcnQpO1xyXG5cdFx0fVxyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIG9uRHJhZ1N0YXJ0XHJcblx0ICogQGRlc2MgdG91Y2hzdGFydC9tb3VzZWRvd24gZXZlbnRcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS5vbkRyYWdTdGFydCA9IGZ1bmN0aW9uKGV2ZW50KXtcclxuXHRcdHZhciBldiA9IGV2ZW50Lm9yaWdpbmFsRXZlbnQgfHwgZXZlbnQgfHwgd2luZG93LmV2ZW50O1xyXG5cdFx0Ly8gcHJldmVudCByaWdodCBjbGlja1xyXG5cdFx0aWYgKGV2LndoaWNoID09PSAzKSB7IFxyXG5cdFx0XHRyZXR1cm4gZmFsc2U7XHJcblx0XHR9XHJcblxyXG5cdFx0aWYodGhpcy5kcmFnVHlwZVswXSA9PT0gJ21vdXNlZG93bicpe1xyXG5cdFx0XHR0aGlzLmRvbS4kc3RhZ2UuYWRkQ2xhc3MoJ293bC1ncmFiJyk7XHJcblx0XHR9XHJcblxyXG5cdFx0dGhpcy5maXJlQ2FsbGJhY2soJ29uVG91Y2hTdGFydCcpO1xyXG5cdFx0dGhpcy5kcmFnLnN0YXJ0VGltZSA9IG5ldyBEYXRlKCkuZ2V0VGltZSgpO1xyXG5cdFx0dGhpcy5zZXRTcGVlZCgwKTtcclxuXHRcdHRoaXMuc3RhdGUuaXNUb3VjaCA9IHRydWU7XHJcblx0XHR0aGlzLnN0YXRlLmlzU2Nyb2xsaW5nID0gZmFsc2U7XHJcblx0XHR0aGlzLnN0YXRlLmlzU3dpcGluZyA9IGZhbHNlO1xyXG5cdFx0dGhpcy5kcmFnLmRpc3RhbmNlID0gMDtcclxuXHJcblx0XHQvLyBpZiBpcyAndG91Y2hzdGFydCdcclxuXHRcdHZhciBpc1RvdWNoRXZlbnQgPSBldi50eXBlID09PSAndG91Y2hzdGFydCc7XHJcblx0XHR2YXIgcGFnZVggPSBpc1RvdWNoRXZlbnQgPyBldmVudC50YXJnZXRUb3VjaGVzWzBdLnBhZ2VYIDogKGV2LnBhZ2VYIHx8IGV2LmNsaWVudFgpO1xyXG5cdFx0dmFyIHBhZ2VZID0gaXNUb3VjaEV2ZW50ID8gZXZlbnQudGFyZ2V0VG91Y2hlc1swXS5wYWdlWSA6IChldi5wYWdlWSB8fCBldi5jbGllbnRZKTtcclxuXHJcblx0XHQvL2dldCBzdGFnZSBwb3NpdGlvbiBsZWZ0XHJcblx0XHR0aGlzLmRyYWcub2Zmc2V0WCA9IHRoaXMuZG9tLiRzdGFnZS5wb3NpdGlvbigpLmxlZnQgLSB0aGlzLm9wdGlvbnMuc3RhZ2VQYWRkaW5nO1xyXG5cdFx0dGhpcy5kcmFnLm9mZnNldFkgPSB0aGlzLmRvbS4kc3RhZ2UucG9zaXRpb24oKS50b3A7XHJcblxyXG5cdFx0aWYodGhpcy5vcHRpb25zLnJ0bCl7XHJcblx0XHRcdHRoaXMuZHJhZy5vZmZzZXRYID0gdGhpcy5kb20uJHN0YWdlLnBvc2l0aW9uKCkubGVmdCArIHRoaXMud2lkdGguc3RhZ2UgLSB0aGlzLndpZHRoLmVsICsgdGhpcy5vcHRpb25zLm1hcmdpbjtcclxuXHRcdH1cclxuXHJcblx0XHQvL2NhdGNoIHBvc2l0aW9uIC8vIGllIHRvIGZpeFxyXG5cdFx0aWYodGhpcy5zdGF0ZS5pbk1vdGlvbiAmJiB0aGlzLnN1cHBvcnQzZCl7XHJcblx0XHRcdHZhciBhbmltYXRlZFBvcyA9IHRoaXMuZ2V0VHJhbnNmb3JtUHJvcGVydHkoKTtcclxuXHRcdFx0dGhpcy5kcmFnLm9mZnNldFggPSBhbmltYXRlZFBvcztcclxuXHRcdFx0dGhpcy5hbmltU3RhZ2UoYW5pbWF0ZWRQb3MpO1xyXG5cdFx0fSBlbHNlIGlmKHRoaXMuc3RhdGUuaW5Nb3Rpb24gJiYgIXRoaXMuc3VwcG9ydDNkICl7XHJcblx0XHRcdHRoaXMuc3RhdGUuaW5Nb3Rpb24gPSBmYWxzZTtcclxuXHRcdFx0cmV0dXJuIGZhbHNlO1xyXG5cdFx0fVxyXG5cclxuXHRcdHRoaXMuZHJhZy5zdGFydFggPSBwYWdlWCAtIHRoaXMuZHJhZy5vZmZzZXRYO1xyXG5cdFx0dGhpcy5kcmFnLnN0YXJ0WSA9IHBhZ2VZIC0gdGhpcy5kcmFnLm9mZnNldFk7XHJcblxyXG5cdFx0dGhpcy5kcmFnLnN0YXJ0ID0gcGFnZVggLSB0aGlzLmRyYWcuc3RhcnRYO1xyXG5cdFx0dGhpcy5kcmFnLnRhcmdldEVsID0gZXYudGFyZ2V0IHx8IGV2LnNyY0VsZW1lbnQ7XHJcblx0XHR0aGlzLmRyYWcudXBkYXRlZFggPSB0aGlzLmRyYWcuc3RhcnQ7XHJcblxyXG5cdFx0Ly8gdG8gZG8vY2hlY2tcclxuXHRcdC8vcHJldmVudCBsaW5rcyBhbmQgaW1hZ2VzIGRyYWdnaW5nO1xyXG5cdFx0Ly90aGlzLmRyYWcudGFyZ2V0RWwuZHJhZ2dhYmxlID0gZmFsc2U7XHJcblxyXG5cdFx0dGhpcy5vbihkb2N1bWVudCwgdGhpcy5kcmFnVHlwZVsxXSwgdGhpcy5lLl9vbkRyYWdNb3ZlLCBmYWxzZSk7XHJcblx0XHR0aGlzLm9uKGRvY3VtZW50LCB0aGlzLmRyYWdUeXBlWzJdLCB0aGlzLmUuX29uRHJhZ0VuZCwgZmFsc2UpO1xyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIG9uRHJhZ01vdmVcclxuXHQgKiBAZGVzYyB0b3VjaG1vdmUvbW91c2Vtb3ZlIGV2ZW50XHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUub25EcmFnTW92ZSA9IGZ1bmN0aW9uKGV2ZW50KXtcclxuXHRcdGlmICghdGhpcy5zdGF0ZS5pc1RvdWNoKXtcclxuXHRcdFx0cmV0dXJuO1xyXG5cdFx0fVxyXG5cclxuXHRcdGlmICh0aGlzLnN0YXRlLmlzU2Nyb2xsaW5nKXtcclxuXHRcdFx0cmV0dXJuO1xyXG5cdFx0fVxyXG5cclxuXHRcdHZhciBuZWlnaGJvdXJJdGVtV2lkdGg9MDtcclxuXHRcdHZhciBldiA9IGV2ZW50Lm9yaWdpbmFsRXZlbnQgfHwgZXZlbnQgfHwgd2luZG93LmV2ZW50O1xyXG5cclxuXHRcdC8vIGlmIGlzICd0b3VjaHN0YXJ0J1xyXG5cdFx0dmFyIGlzVG91Y2hFdmVudCA9IGV2LnR5cGUgPT0gJ3RvdWNobW92ZSc7XHJcblx0XHR2YXIgcGFnZVggPSBpc1RvdWNoRXZlbnQgPyBldi50YXJnZXRUb3VjaGVzWzBdLnBhZ2VYIDogKGV2LnBhZ2VYIHx8IGV2LmNsaWVudFgpO1xyXG5cdFx0dmFyIHBhZ2VZID0gaXNUb3VjaEV2ZW50ID8gZXYudGFyZ2V0VG91Y2hlc1swXS5wYWdlWSA6IChldi5wYWdlWSB8fCBldi5jbGllbnRZKTtcclxuXHJcblx0XHQvLyBEcmFnIERpcmVjdGlvbiBcclxuXHRcdHRoaXMuZHJhZy5jdXJyZW50WCA9IHBhZ2VYIC0gdGhpcy5kcmFnLnN0YXJ0WDtcclxuXHRcdHRoaXMuZHJhZy5jdXJyZW50WSA9IHBhZ2VZIC0gdGhpcy5kcmFnLnN0YXJ0WTtcclxuXHRcdHRoaXMuZHJhZy5kaXN0YW5jZSA9IHRoaXMuZHJhZy5jdXJyZW50WCAtIHRoaXMuZHJhZy5vZmZzZXRYO1xyXG5cclxuXHRcdC8vIENoZWNrIG1vdmUgZGlyZWN0aW9uIFxyXG5cdFx0aWYgKHRoaXMuZHJhZy5kaXN0YW5jZSA8IDApIHtcclxuXHRcdFx0dGhpcy5zdGF0ZS5kaXJlY3Rpb24gPSB0aGlzLm9wdGlvbnMucnRsID8gJ3JpZ2h0JyA6ICdsZWZ0JztcclxuXHRcdH0gZWxzZSBpZih0aGlzLmRyYWcuZGlzdGFuY2UgPiAwKXtcclxuXHRcdFx0dGhpcy5zdGF0ZS5kaXJlY3Rpb24gPSB0aGlzLm9wdGlvbnMucnRsID8gJ2xlZnQnIDogJ3JpZ2h0JztcclxuXHRcdH1cclxuXHRcdC8vIExvb3BcclxuXHRcdGlmKHRoaXMub3B0aW9ucy5sb29wKXtcclxuXHRcdFx0aWYodGhpcy5vcCh0aGlzLmRyYWcuY3VycmVudFgsICc+JywgdGhpcy5wb3MubWluVmFsdWUpICYmIHRoaXMuc3RhdGUuZGlyZWN0aW9uID09PSAncmlnaHQnICl7XHJcblx0XHRcdFx0dGhpcy5kcmFnLmN1cnJlbnRYIC09IHRoaXMucG9zLmxvb3A7XHJcblx0XHRcdH1lbHNlIGlmKHRoaXMub3AodGhpcy5kcmFnLmN1cnJlbnRYLCAnPCcsIHRoaXMucG9zLm1heFZhbHVlKSAmJiB0aGlzLnN0YXRlLmRpcmVjdGlvbiA9PT0gJ2xlZnQnICl7XHJcblx0XHRcdFx0dGhpcy5kcmFnLmN1cnJlbnRYICs9IHRoaXMucG9zLmxvb3A7XHJcblx0XHRcdH1cclxuXHRcdH0gZWxzZSB7XHJcblx0XHRcdC8vIHB1bGxcclxuXHRcdFx0dmFyIG1pblZhbHVlID0gdGhpcy5vcHRpb25zLnJ0bCA/IHRoaXMucG9zLm1heFZhbHVlIDogdGhpcy5wb3MubWluVmFsdWU7XHJcblx0XHRcdHZhciBtYXhWYWx1ZSA9IHRoaXMub3B0aW9ucy5ydGwgPyB0aGlzLnBvcy5taW5WYWx1ZSA6IHRoaXMucG9zLm1heFZhbHVlO1xyXG5cdFx0XHR2YXIgcHVsbCA9IHRoaXMub3B0aW9ucy5wdWxsRHJhZyA/IHRoaXMuZHJhZy5kaXN0YW5jZSAvIDUgOiAwO1xyXG5cdFx0XHR0aGlzLmRyYWcuY3VycmVudFggPSBNYXRoLm1heChNYXRoLm1pbih0aGlzLmRyYWcuY3VycmVudFgsIG1pblZhbHVlICsgcHVsbCksIG1heFZhbHVlICsgcHVsbCk7XHJcblx0XHR9XHJcblxyXG5cclxuXHJcblx0XHQvLyBMb2NrIGJyb3dzZXIgaWYgc3dpcGluZyBob3Jpem9udGFsXHJcblxyXG5cdFx0aWYgKCh0aGlzLmRyYWcuZGlzdGFuY2UgPiA4IHx8IHRoaXMuZHJhZy5kaXN0YW5jZSA8IC04KSkge1xyXG5cdFx0XHRpZiAoZXYucHJldmVudERlZmF1bHQgIT09IHVuZGVmaW5lZCkge1xyXG5cdFx0XHRcdGV2LnByZXZlbnREZWZhdWx0KCk7XHJcblx0XHRcdH0gZWxzZSB7XHJcblx0XHRcdFx0ZXYucmV0dXJuVmFsdWUgPSBmYWxzZTtcclxuXHRcdFx0fVxyXG5cdFx0XHR0aGlzLnN0YXRlLmlzU3dpcGluZyA9IHRydWU7XHJcblx0XHR9XHJcblxyXG5cdFx0dGhpcy5kcmFnLnVwZGF0ZWRYID0gdGhpcy5kcmFnLmN1cnJlbnRYO1xyXG5cclxuXHRcdC8vIExvY2sgT3dsIGlmIHNjcm9sbGluZyBcclxuXHRcdGlmICgodGhpcy5kcmFnLmN1cnJlbnRZID4gMTYgfHwgdGhpcy5kcmFnLmN1cnJlbnRZIDwgLTE2KSAmJiB0aGlzLnN0YXRlLmlzU3dpcGluZyA9PT0gZmFsc2UpIHtcclxuXHRcdFx0IHRoaXMuc3RhdGUuaXNTY3JvbGxpbmcgPSB0cnVlO1xyXG5cdFx0XHQgdGhpcy5kcmFnLnVwZGF0ZWRYID0gdGhpcy5kcmFnLnN0YXJ0O1xyXG5cdFx0fVxyXG5cclxuXHRcdHRoaXMuYW5pbVN0YWdlKHRoaXMuZHJhZy51cGRhdGVkWCk7XHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICogb25EcmFnRW5kIFxyXG5cdCAqIEBkZXNjIHRvdWNoZW5kL21vdXNldXAgZXZlbnRcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS5vbkRyYWdFbmQgPSBmdW5jdGlvbihldmVudCl7XHJcblx0XHRpZiAoIXRoaXMuc3RhdGUuaXNUb3VjaCl7XHJcblx0XHRcdHJldHVybjtcclxuXHRcdH1cclxuXHRcdGlmKHRoaXMuZHJhZ1R5cGVbMF0gPT09ICdtb3VzZWRvd24nKXtcclxuXHRcdFx0dGhpcy5kb20uJHN0YWdlLnJlbW92ZUNsYXNzKCdvd2wtZ3JhYicpO1xyXG5cdFx0fVxyXG5cclxuXHRcdHRoaXMuZmlyZUNhbGxiYWNrKCdvblRvdWNoRW5kJyk7XHJcblxyXG5cdFx0Ly9wcmV2ZW50IGxpbmtzIGFuZCBpbWFnZXMgZHJhZ2dpbmc7XHJcblx0XHQvL3RoaXMuZHJhZy50YXJnZXRFbC5kcmFnZ2FibGUgPSB0cnVlO1xyXG5cclxuXHRcdC8vcmVtb3ZlIGRyYWcgZXZlbnQgbGlzdGVuZXJzXHJcblxyXG5cdFx0dGhpcy5zdGF0ZS5pc1RvdWNoID0gZmFsc2U7XHJcblx0XHR0aGlzLnN0YXRlLmlzU2Nyb2xsaW5nID0gZmFsc2U7XHJcblx0XHR0aGlzLnN0YXRlLmlzU3dpcGluZyA9IGZhbHNlO1xyXG5cclxuXHRcdC8vdG8gY2hlY2tcclxuXHRcdGlmKHRoaXMuZHJhZy5kaXN0YW5jZSA9PT0gMCAmJiB0aGlzLnN0YXRlLmluTW90aW9uICE9PSB0cnVlKXtcclxuXHRcdFx0dGhpcy5zdGF0ZS5pbk1vdGlvbiA9IGZhbHNlO1xyXG5cdFx0XHRyZXR1cm4gZmFsc2U7XHJcblx0XHR9XHJcblxyXG5cdFx0Ly8gcHJldmVudCBjbGlja3Mgd2hpbGUgc2Nyb2xsaW5nXHJcblxyXG5cdFx0dGhpcy5kcmFnLmVuZFRpbWUgPSBuZXcgRGF0ZSgpLmdldFRpbWUoKTtcclxuXHRcdHZhciBjb21wYXJlVGltZXMgPSB0aGlzLmRyYWcuZW5kVGltZSAtIHRoaXMuZHJhZy5zdGFydFRpbWU7XHJcblx0XHR2YXIgZGlzdGFuY2VBYnMgPSBNYXRoLmFicyh0aGlzLmRyYWcuZGlzdGFuY2UpO1xyXG5cclxuXHRcdC8vdG8gdGVzdFxyXG5cdFx0aWYoZGlzdGFuY2VBYnMgPiAzIHx8IGNvbXBhcmVUaW1lcyA+IDMwMCl7XHJcblx0XHRcdHRoaXMucmVtb3ZlQ2xpY2sodGhpcy5kcmFnLnRhcmdldEVsKTtcclxuXHRcdH1cclxuXHJcblx0XHR2YXIgY2xvc2VzdCA9IHRoaXMuY2xvc2VzdCh0aGlzLmRyYWcudXBkYXRlZFgpO1xyXG5cclxuXHRcdHRoaXMuc2V0U3BlZWQodGhpcy5vcHRpb25zLmRyYWdFbmRTcGVlZCwgZmFsc2UsIHRydWUpO1xyXG5cdFx0dGhpcy5hbmltU3RhZ2UodGhpcy5wb3MuaXRlbXNbY2xvc2VzdF0pO1xyXG5cdFx0XHJcblx0XHQvL2lmIHB1bGxEcmFnIGlzIG9mZiB0aGVuIGZpcmUgdHJhbnNpdGlvbkVuZCBldmVudCBtYW51YWxseSB3aGVuIHN0aWNrIHRvIGJvcmRlclxyXG5cdFx0aWYoIXRoaXMub3B0aW9ucy5wdWxsRHJhZyAmJiB0aGlzLmRyYWcudXBkYXRlZFggPT09IHRoaXMucG9zLml0ZW1zW2Nsb3Nlc3RdKXtcclxuXHRcdFx0dGhpcy50cmFuc2l0aW9uRW5kKCk7XHJcblx0XHR9XHJcblxyXG5cdFx0dGhpcy5kcmFnLmRpc3RhbmNlID0gMDtcclxuXHJcblx0XHR0aGlzLm9mZihkb2N1bWVudCwgdGhpcy5kcmFnVHlwZVsxXSwgdGhpcy5lLl9vbkRyYWdNb3ZlKTtcclxuXHRcdHRoaXMub2ZmKGRvY3VtZW50LCB0aGlzLmRyYWdUeXBlWzJdLCB0aGlzLmUuX29uRHJhZ0VuZCk7XHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICogcmVtb3ZlQ2xpY2tcclxuXHQgKiBAZGVzYyBBdHRhY2ggcHJldmVudENsaWNrIGZ1bmN0aW9uIHRvIGRpc2FibGUgbGluayB3aGlsZSBzd2lwcGluZ1xyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqIEBwYXJhbSBbdGFyZ2V0XSAtIGNsaWNrZWQgZG9tIGVsZW1lbnRcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS5yZW1vdmVDbGljayA9IGZ1bmN0aW9uKHRhcmdldCl7XHJcblx0XHR0aGlzLmRyYWcudGFyZ2V0RWwgPSB0YXJnZXQ7XHJcblx0XHR0aGlzLm9uKHRhcmdldCwnY2xpY2snLCB0aGlzLmUuX3ByZXZlbnRDbGljaywgZmFsc2UpO1xyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIHByZXZlbnRDbGlja1xyXG5cdCAqIEBkZXNjIEFkZCBwcmV2ZW50RGVmYXVsdCBmb3IgYW55IGxpbmsgYW5kIHRoZW4gcmVtb3ZlIHJlbW92ZUNsaWNrIGV2ZW50IGhhbmxkZXJcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS5wcmV2ZW50Q2xpY2sgPSBmdW5jdGlvbihldil7XHJcblx0XHRpZihldi5wcmV2ZW50RGVmYXVsdCkge1xyXG5cdFx0XHRldi5wcmV2ZW50RGVmYXVsdCgpO1xyXG5cdFx0fWVsc2Uge1xyXG5cdFx0XHRldi5yZXR1cm5WYWx1ZSA9IGZhbHNlO1xyXG5cdFx0fVxyXG5cdFx0aWYoZXYuc3RvcFByb3BhZ2F0aW9uKXtcclxuXHRcdFx0ZXYuc3RvcFByb3BhZ2F0aW9uKCk7XHJcblx0XHR9XHJcblx0XHR0aGlzLm9mZih0aGlzLmRyYWcudGFyZ2V0RWwsJ2NsaWNrJyx0aGlzLmUuX3ByZXZlbnRDbGljayxmYWxzZSk7XHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICogZ2V0VHJhbnNmb3JtUHJvcGVydHlcclxuXHQgKiBAZGVzYyBjYXRjaCBzdGFnZSBwb3NpdGlvbiB3aGlsZSBhbmltYXRlIChvbmx5IGNzczMpXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUuZ2V0VHJhbnNmb3JtUHJvcGVydHkgPSBmdW5jdGlvbigpe1xyXG5cdFx0dmFyIHRyYW5zZm9ybSA9IHdpbmRvdy5nZXRDb21wdXRlZFN0eWxlKHRoaXMuZG9tLnN0YWdlLCBudWxsKS5nZXRQcm9wZXJ0eVZhbHVlKHRoaXMudmVuZG9yTmFtZSArICd0cmFuc2Zvcm0nKTtcclxuXHRcdC8vdmFyIHRyYW5zZm9ybSA9IHRoaXMuZG9tLiRzdGFnZS5jc3ModGhpcy52ZW5kb3JOYW1lICsgJ3RyYW5zZm9ybScpXHJcblx0XHR0cmFuc2Zvcm0gPSB0cmFuc2Zvcm0ucmVwbGFjZSgvbWF0cml4KDNkKT9cXCh8XFwpL2csICcnKS5zcGxpdCgnLCcpO1xyXG5cdFx0dmFyIG1hdHJpeDNkID0gdHJhbnNmb3JtLmxlbmd0aCA9PT0gMTY7XHJcblxyXG5cdFx0cmV0dXJuIG1hdHJpeDNkICE9PSB0cnVlID8gdHJhbnNmb3JtWzRdIDogdHJhbnNmb3JtWzEyXTtcclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiBjbG9zZXN0XHJcblx0ICogQGRlc2MgR2V0IGNsb3Nlc3QgaXRlbSBhZnRlciB0b3VjaGVuZC9tb3VzZXVwXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICogQHBhcmFtIFt4XSAtIGN1cmVudCBwb3NpdGlvbiBpbiBwaXhlbHNcclxuXHQgKiByZXR1cm4gcG9zaXRpb24gaW4gcGl4ZWxzXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUuY2xvc2VzdCA9IGZ1bmN0aW9uKHgpe1xyXG5cdFx0dmFyIG5ld1ggPSAwLFxyXG5cdFx0XHRwdWxsID0gMzA7XHJcblxyXG5cdFx0aWYoIXRoaXMub3B0aW9ucy5mcmVlRHJhZyl7XHJcblx0XHRcdC8vIENoZWNrIGNsb3Nlc3QgaXRlbVxyXG5cdFx0XHRmb3IodmFyIGkgPSAwOyBpPCB0aGlzLm51bS5pdGVtczsgaSsrKXtcclxuXHRcdFx0XHRpZih4ID4gdGhpcy5wb3MuaXRlbXNbaV0tcHVsbCAmJiB4IDwgdGhpcy5wb3MuaXRlbXNbaV0rcHVsbCl7XHJcblx0XHRcdFx0XHRuZXdYID0gaTtcclxuXHRcdFx0XHR9ZWxzZSBpZih0aGlzLm9wKHgsJzwnLHRoaXMucG9zLml0ZW1zW2ldKSAmJiB0aGlzLm9wKHgsJz4nLHRoaXMucG9zLml0ZW1zW2krMSB8fCB0aGlzLnBvcy5pdGVtc1tpXSAtIHRoaXMud2lkdGguZWxdKSApe1xyXG5cdFx0XHRcdFx0bmV3WCA9IHRoaXMuc3RhdGUuZGlyZWN0aW9uID09PSAnbGVmdCcgPyBpKzEgOiBpO1xyXG5cdFx0XHRcdH1cclxuXHRcdFx0fVxyXG5cdFx0fVxyXG5cdFx0Ly9ub24gbG9vcCBib3VuZHJpZXNcclxuXHRcdGlmKCF0aGlzLm9wdGlvbnMubG9vcCl7XHJcblx0XHRcdGlmKHRoaXMub3AoeCwnPicsdGhpcy5wb3MubWluVmFsdWUpKXtcclxuXHRcdFx0XHRuZXdYID0geCA9IHRoaXMucG9zLm1pbjtcclxuXHRcdFx0fSBlbHNlIGlmKHRoaXMub3AoeCwnPCcsdGhpcy5wb3MubWF4VmFsdWUpKXtcclxuXHRcdFx0XHRuZXdYID0geCA9IHRoaXMucG9zLm1heDtcclxuXHRcdFx0fVxyXG5cdFx0fVxyXG5cclxuXHRcdGlmKCF0aGlzLm9wdGlvbnMuZnJlZURyYWcpe1xyXG5cdFx0XHQvLyBzZXQgcG9zaXRpb25zXHJcblx0XHRcdHRoaXMucG9zLmN1cnJlbnRBYnMgPSBuZXdYO1xyXG5cdFx0XHR0aGlzLnBvcy5jdXJyZW50ID0gdGhpcy5kb20uJGl0ZW1zLmVxKG5ld1gpLmRhdGEoJ293bC1pdGVtJykuaW5kZXg7XHJcblx0XHR9IGVsc2Uge1xyXG5cdFx0XHR0aGlzLnVwZGF0ZUl0ZW1TdGF0ZSgpO1xyXG5cdFx0XHRyZXR1cm4geDtcclxuXHRcdH1cclxuXHJcblx0XHRyZXR1cm4gbmV3WDtcclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiBhbmltU3RhZ2VcclxuXHQgKiBAZGVzYyBhbmltYXRlIHN0YWdlIHBvc2l0aW9uIChib3RoIGNzczMvY3NzMikgYW5kIHBlcmZvcm0gb25DaGFuZ2UgZnVuY3Rpb25zL2V2ZW50c1xyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqIEBwYXJhbSBbeF0gLSBjdXJlbnQgcG9zaXRpb24gaW4gcGl4ZWxzXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUuYW5pbVN0YWdlID0gZnVuY3Rpb24ocG9zKXtcclxuXHJcblx0XHQvLyBpZiBzcGVlZCBpcyAwIHRoZSBzZXQgaW5Nb3Rpb24gdG8gZmFsc2VcclxuXHRcdGlmKHRoaXMuc3BlZWQuY3VycmVudCAhPT0gMCAmJiB0aGlzLnBvcy5jdXJyZW50QWJzICE9PSB0aGlzLnBvcy5taW4pe1xyXG5cdFx0XHR0aGlzLmZpcmVDYWxsYmFjaygnb25UcmFuc2l0aW9uU3RhcnQnKTtcclxuXHRcdFx0dGhpcy5zdGF0ZS5pbk1vdGlvbiA9IHRydWU7XHJcblx0XHR9XHJcblxyXG5cdFx0dmFyIHBvc1ggPSB0aGlzLnBvcy5zdGFnZSA9IHBvcyxcclxuXHRcdFx0c3R5bGUgPSB0aGlzLmRvbS5zdGFnZS5zdHlsZTtcclxuXHJcblx0XHRpZih0aGlzLnN1cHBvcnQzZCl7XHJcblx0XHRcdHZhciB0cmFuc2xhdGUgPSAndHJhbnNsYXRlM2QoJyArIHBvc1ggKyAncHgnKycsMHB4LCAwcHgpJztcclxuXHRcdFx0c3R5bGVbdGhpcy50cmFuc2Zvcm1WZW5kb3JdID0gdHJhbnNsYXRlO1xyXG5cdFx0fSBlbHNlIGlmKHRoaXMuc3RhdGUuaXNUb3VjaCl7XHJcblx0XHRcdHN0eWxlLmxlZnQgPSBwb3NYKydweCc7XHJcblx0XHR9IGVsc2Uge1xyXG5cdFx0XHR0aGlzLmRvbS4kc3RhZ2UuYW5pbWF0ZSh7bGVmdDogcG9zWH0sdGhpcy5zcGVlZC5jc3Myc3BlZWQsIHRoaXMub3B0aW9ucy5mYWxsYmFja0Vhc2luZywgZnVuY3Rpb24oKXtcclxuXHRcdFx0XHRpZih0aGlzLnN0YXRlLmluTW90aW9uKXtcclxuXHRcdFx0XHRcdHRoaXMudHJhbnNpdGlvbkVuZCgpO1xyXG5cdFx0XHRcdH1cclxuXHRcdFx0fS5iaW5kKHRoaXMpKTtcclxuXHRcdH1cclxuXHJcblx0XHR0aGlzLm9uQ2hhbmdlKCk7XHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICogdXBkYXRlUG9zaXRpb25cclxuXHQgKiBAZGVzYyBVcGRhdGUgY3VycmVudCBwb3NpdGlvbnNcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKiBAcGFyYW0gW3Bvc10gLSBudW1iZXIgLSBuZXcgcG9zaXRpb25cclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS51cGRhdGVQb3NpdGlvbiA9IGZ1bmN0aW9uKHBvcyl7XHJcblxyXG5cdFx0Ly8gaWYgbm8gaXRlbXMgdGhlbiBzdG9wIFxyXG5cdFx0aWYodGhpcy5udW0ub0l0ZW1zID09PSAwKXtyZXR1cm4gZmFsc2U7fVxyXG5cdFx0Ly8gdG8gZG9cclxuXHRcdC8vaWYocG9zID4gdGhpcy5udW0uaXRlbXMpe3BvcyA9IDA7fVxyXG5cdFx0aWYocG9zID09PSB1bmRlZmluZWQpe3JldHVybiBmYWxzZTt9XHJcblxyXG5cdFx0Ly9wb3MgLSBuZXcgY3VycmVudCBwb3NpdGlvblxyXG5cdFx0dmFyIG5leHRQb3MgPSBwb3M7XHJcblx0XHR0aGlzLnBvcy5wcmV2ID0gdGhpcy5wb3MuY3VycmVudEFicztcclxuXHJcblx0XHRpZih0aGlzLnN0YXRlLnJldmVydCl7XHJcblx0XHRcdHRoaXMucG9zLmN1cnJlbnQgPSB0aGlzLmRvbS4kaXRlbXMuZXEobmV4dFBvcykuZGF0YSgnb3dsLWl0ZW0nKS5pbmRleDtcclxuXHRcdFx0dGhpcy5wb3MuY3VycmVudEFicyA9IG5leHRQb3M7XHJcblx0XHRcdHJldHVybjtcclxuXHRcdH1cclxuXHJcblx0XHRpZighdGhpcy5vcHRpb25zLmxvb3Ape1xyXG5cdFx0XHRpZih0aGlzLm9wdGlvbnMubmF2UmV3aW5kKXtcclxuXHRcdFx0XHRuZXh0UG9zID0gbmV4dFBvcyA+IHRoaXMucG9zLm1heCA/IHRoaXMucG9zLm1pbiA6IChuZXh0UG9zIDwgMCA/IHRoaXMucG9zLm1heCA6IG5leHRQb3MpO1xyXG5cdFx0XHR9IGVsc2Uge1xyXG5cdFx0XHRcdG5leHRQb3MgPSBuZXh0UG9zID4gdGhpcy5wb3MubWF4ID8gdGhpcy5wb3MubWF4IDogKG5leHRQb3MgPD0gMCA/IDAgOiBuZXh0UG9zKTtcclxuXHRcdFx0fVxyXG5cdFx0fSBlbHNlIHtcclxuXHRcdFx0bmV4dFBvcyA9IG5leHRQb3MgPj0gdGhpcy5udW0ub0l0ZW1zID8gdGhpcy5udW0ub0l0ZW1zLTEgOiBuZXh0UG9zO1xyXG5cdFx0fVxyXG5cclxuXHRcdHRoaXMucG9zLmN1cnJlbnQgPSB0aGlzLmRvbS4kb0l0ZW1zLmVxKG5leHRQb3MpLmRhdGEoJ293bC1pdGVtJykuaW5kZXg7XHJcblx0XHR0aGlzLnBvcy5jdXJyZW50QWJzID0gdGhpcy5kb20uJG9JdGVtcy5lcShuZXh0UG9zKS5kYXRhKCdvd2wtaXRlbScpLmluZGV4QWJzO1xyXG5cclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiBzZXRTcGVlZFxyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqIEBwYXJhbSBbc3BlZWRdIC0gbnVtYmVyXHJcblx0ICogQHBhcmFtIFtwb3NdIC0gbnVtYmVyIC0gbmV4dCBwb3NpdGlvbiAtIHVzZSB0aGlzIHBhcmFtIHRvIGNhbGN1bGF0ZSBzbWFydFNwZWVkXHJcblx0ICogQHBhcmFtIFtkcmFnXSAtIGJvb2xlYW4gLSBpZiBkcmFnIGlzIHRydWUgdGhlbiBzbWFydCBzcGVlZCBpcyBkaXNhYmxlZFxyXG5cdCAqIHJldHVybiBzcGVlZFxyXG5cdCAqL1xyXG5cclxuXHRPd2wucHJvdG90eXBlLnNldFNwZWVkID0gZnVuY3Rpb24oc3BlZWQscG9zLGRyYWcpIHtcclxuXHRcdHZhciBzID0gc3BlZWQsXHJcblx0XHRcdG5leHRQb3MgPSBwb3M7XHJcblxyXG5cdFx0aWYoKHMgPT09IGZhbHNlICYmIHMgIT09IDAgJiYgZHJhZyAhPT0gdHJ1ZSkgfHwgcyA9PT0gdW5kZWZpbmVkKXtcclxuXHJcblx0XHRcdC8vRG91YmxlIGNoZWNrIHRoaXNcclxuXHRcdFx0Ly8gdmFyIG5leHRQeCA9IHRoaXMucG9zLml0ZW1zW25leHRQb3NdO1xyXG5cdFx0XHQvLyB2YXIgY3VyclB4ID0gdGhpcy5wb3Muc3RhZ2UgXHJcblx0XHRcdC8vIHZhciBkaWZmID0gTWF0aC5hYnMobmV4dFB4LWN1cnJQeCk7XHJcblx0XHRcdC8vIHZhciBzID0gZGlmZi8xXHJcblx0XHRcdC8vIGlmKHM+MTAwMCl7XHJcblx0XHRcdC8vIFx0cyA9IDEwMDA7XHJcblx0XHRcdC8vIH1cclxuXHRcdFx0XHJcblx0XHRcdHZhciBkaWZmID0gTWF0aC5hYnMobmV4dFBvcyAtIHRoaXMucG9zLnByZXYpO1xyXG5cdFx0XHRkaWZmID0gZGlmZiA9PT0gMCA/IDEgOiBkaWZmO1xyXG5cdFx0XHRpZihkaWZmPjYpe2RpZmYgPSA2O31cclxuXHRcdFx0cyA9IGRpZmYgKiB0aGlzLm9wdGlvbnMuc21hcnRTcGVlZDtcclxuXHRcdH1cclxuXHJcblx0XHRpZihzID09PSBmYWxzZSAmJiBkcmFnID09PSB0cnVlKXtcclxuXHRcdFx0cyA9IHRoaXMub3B0aW9ucy5zbWFydFNwZWVkO1xyXG5cdFx0fVxyXG5cclxuXHRcdGlmKHMgPT09IDApe3M9MDt9XHJcblxyXG5cdFx0aWYodGhpcy5zdXBwb3J0M2Qpe1xyXG5cdFx0XHR2YXIgc3R5bGUgPSB0aGlzLmRvbS5zdGFnZS5zdHlsZTtcclxuXHRcdFx0c3R5bGUud2Via2l0VHJhbnNpdGlvbkR1cmF0aW9uID0gc3R5bGUuTXNUcmFuc2l0aW9uRHVyYXRpb24gPSBzdHlsZS5tc1RyYW5zaXRpb25EdXJhdGlvbiA9IHN0eWxlLk1velRyYW5zaXRpb25EdXJhdGlvbiA9IHN0eWxlLk9UcmFuc2l0aW9uRHVyYXRpb24gPSBzdHlsZS50cmFuc2l0aW9uRHVyYXRpb24gPSAocyAvIDEwMDApICsgJ3MnO1xyXG5cdFx0fSBlbHNle1xyXG5cdFx0XHR0aGlzLnNwZWVkLmNzczJzcGVlZCA9IHM7XHJcblx0XHR9XHJcblx0XHR0aGlzLnNwZWVkLmN1cnJlbnQgPSBzO1xyXG5cdFx0cmV0dXJuIHM7XHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICoganVtcFRvXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICogQHBhcmFtIFtwb3NdIC0gbnVtYmVyIC0gbmV4dCBwb3NpdGlvbiAtIHVzZSB0aGlzIHBhcmFtIHRvIGNhbGN1bGF0ZSBzbWFydFNwZWVkXHJcblx0ICogQHBhcmFtIFt1cGRhdGVdIC0gYm9vbGVhbiAtIGlmIGRyYWcgaXMgdHJ1ZSB0aGVuIHNtYXJ0IHNwZWVkIGlzIGRpc2FibGVkXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUuanVtcFRvID0gZnVuY3Rpb24ocG9zLHVwZGF0ZSl7XHJcblx0XHRpZih0aGlzLnN0YXRlLmxhenlDb250ZW50KXtcclxuXHRcdFx0dGhpcy5wb3MuZ29Ub0xhenlDb250ZW50ID0gcG9zO1xyXG5cdFx0fVxyXG5cdFx0dGhpcy51cGRhdGVQb3NpdGlvbihwb3MpO1xyXG5cdFx0dGhpcy5zZXRTcGVlZCgwKTtcclxuXHRcdHRoaXMuYW5pbVN0YWdlKHRoaXMucG9zLml0ZW1zW3RoaXMucG9zLmN1cnJlbnRBYnNdKTtcclxuXHRcdGlmKHVwZGF0ZSAhPT0gdHJ1ZSl7XHJcblx0XHRcdHRoaXMudXBkYXRlSXRlbVN0YXRlKCk7XHJcblx0XHR9XHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICogZ29Ub1xyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqIEBwYXJhbSBbcG9zXSAtIG51bWJlclxyXG5cdCAqIEBwYXJhbSBbc3BlZWRdIC0gc3BlZWQgaW4gbXNcclxuXHQgKiBAcGFyYW0gW3NwZWVkXSAtIHNwZWVkIGluIG1zXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUuZ29UbyA9IGZ1bmN0aW9uKHBvcyxzcGVlZCl7XHJcblx0XHRpZih0aGlzLnN0YXRlLmxhenlDb250ZW50ICYmIHRoaXMuc3RhdGUuaW5Nb3Rpb24pe1xyXG5cdFx0XHRyZXR1cm4gZmFsc2U7XHJcblx0XHR9XHJcblxyXG5cdFx0dGhpcy51cGRhdGVQb3NpdGlvbihwb3MpO1xyXG5cclxuXHRcdGlmKHRoaXMuc3RhdGUuYW5pbWF0ZSl7c3BlZWQgPSAwO31cclxuXHRcdHRoaXMuc2V0U3BlZWQoc3BlZWQsdGhpcy5wb3MuY3VycmVudEFicyk7XHJcblxyXG5cdFx0aWYodGhpcy5zdGF0ZS5hbmltYXRlKXt0aGlzLmFuaW1hdGUoKTt9XHJcblx0XHR0aGlzLmFuaW1TdGFnZSh0aGlzLnBvcy5pdGVtc1t0aGlzLnBvcy5jdXJyZW50QWJzXSk7XHJcblx0XHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICogbmV4dFxyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqL1xyXG5cclxuXHRPd2wucHJvdG90eXBlLm5leHQgPSBmdW5jdGlvbihvcHRpb25hbFNwZWVkKXtcclxuXHRcdHZhciBzID0gb3B0aW9uYWxTcGVlZCB8fCB0aGlzLm9wdGlvbnMubmF2U3BlZWQ7XHJcblx0XHRpZih0aGlzLm9wdGlvbnMubG9vcCAmJiAhdGhpcy5zdGF0ZS5sYXp5Q29udGVudCl7XHJcblx0XHRcdHRoaXMuZ29Ub0xvb3AodGhpcy5vcHRpb25zLnNsaWRlQnksIHMpO1xyXG5cdFx0fWVsc2V7XHJcblx0XHRcdHRoaXMuZ29Ubyh0aGlzLnBvcy5jdXJyZW50ICsgdGhpcy5vcHRpb25zLnNsaWRlQnksIHMpO1xyXG5cdFx0fVxyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIHByZXZcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS5wcmV2ID0gZnVuY3Rpb24ob3B0aW9uYWxTcGVlZCl7XHJcblx0XHR2YXIgcyA9IG9wdGlvbmFsU3BlZWQgfHwgdGhpcy5vcHRpb25zLm5hdlNwZWVkO1xyXG5cdFx0aWYodGhpcy5vcHRpb25zLmxvb3AgJiYgIXRoaXMuc3RhdGUubGF6eUNvbnRlbnQpe1xyXG5cdFx0XHR0aGlzLmdvVG9Mb29wKC10aGlzLm9wdGlvbnMuc2xpZGVCeSwgcyk7XHJcblx0XHR9ZWxzZXtcclxuXHRcdFx0dGhpcy5nb1RvKHRoaXMucG9zLmN1cnJlbnQtdGhpcy5vcHRpb25zLnNsaWRlQnksIHMpO1xyXG5cdFx0fVxyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIGdvVG9Mb29wXHJcblx0ICogQGRlc2MgR28gdG8gZ2l2ZW4gcG9zaXRpb24gaWYgbG9vcCBpcyBlbmFibGVkIC0gdXNlZCBvbmx5IGludGVybmFsXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICogQHBhcmFtIFtkaXN0YW5jZV0gLSBudW1iZXIgLWhvdyBmYXIgdG8gZ29cclxuXHQgKiBAcGFyYW0gW3NwZWVkXSAtIG51bWJlciAtIHNwZWVkIGluIG1zXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUuZ29Ub0xvb3AgPSBmdW5jdGlvbihkaXN0YW5jZSxzcGVlZCl7XHJcblxyXG5cdFx0dmFyIHJldmVydCA9IHRoaXMucG9zLmN1cnJlbnRBYnMsXHJcblx0XHRcdHByZXZQb3NpdGlvbiA9IHRoaXMucG9zLmN1cnJlbnRBYnMsXHJcblx0XHRcdG5ld1Bvc2l0aW9uID0gdGhpcy5wb3MuY3VycmVudEFicyArIGRpc3RhbmNlLFxyXG5cdFx0XHRkaXJlY3Rpb24gPSBwcmV2UG9zaXRpb24gLSBuZXdQb3NpdGlvbiA8IDAgPyB0cnVlIDogZmFsc2U7XHJcblxyXG5cdFx0dGhpcy5zdGF0ZS5yZXZlcnQgPSB0cnVlO1xyXG5cclxuXHRcdGlmKG5ld1Bvc2l0aW9uIDwgMSAmJiBkaXJlY3Rpb24gPT09IGZhbHNlKXtcclxuXHJcblx0XHRcdHRoaXMuc3RhdGUuYnlwYXNzID0gdHJ1ZTtcclxuXHRcdFx0cmV2ZXJ0ID0gdGhpcy5udW0uaXRlbXMgLSAodGhpcy5vcHRpb25zLml0ZW1zLXByZXZQb3NpdGlvbikgLSB0aGlzLm9wdGlvbnMuaXRlbXM7XHJcblx0XHRcdHRoaXMuanVtcFRvKHJldmVydCx0cnVlKTtcclxuXHJcblx0XHR9IGVsc2UgaWYobmV3UG9zaXRpb24gPj0gdGhpcy5udW0uaXRlbXMgLSB0aGlzLm9wdGlvbnMuaXRlbXMgJiYgZGlyZWN0aW9uID09PSB0cnVlICl7XHJcblxyXG5cdFx0XHR0aGlzLnN0YXRlLmJ5cGFzcyA9IHRydWU7XHJcblx0XHRcdHJldmVydCA9IHByZXZQb3NpdGlvbiAtIHRoaXMubnVtLm9JdGVtcztcclxuXHRcdFx0dGhpcy5qdW1wVG8ocmV2ZXJ0LHRydWUpO1xyXG5cclxuXHRcdH1cclxuXHRcdHdpbmRvdy5jbGVhclRpbWVvdXQodGhpcy5lLl9nb1RvTG9vcCk7XHJcblx0XHR0aGlzLmUuX2dvVG9Mb29wID0gd2luZG93LnNldFRpbWVvdXQoZnVuY3Rpb24oKXtcclxuXHRcdFx0dGhpcy5zdGF0ZS5ieXBhc3MgPSBmYWxzZTtcclxuXHRcdFx0dGhpcy5nb1RvKHJldmVydCArIGRpc3RhbmNlLCBzcGVlZCk7XHJcblx0XHRcdHRoaXMuc3RhdGUucmV2ZXJ0ID0gZmFsc2U7XHJcblxyXG5cdFx0fS5iaW5kKHRoaXMpLCAzMCk7XHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICogaW5pdFBvc2l0aW9uXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUuaW5pdFBvc2l0aW9uID0gZnVuY3Rpb24oaW5pdCl7XHJcblxyXG5cdFx0aWYoICF0aGlzLmRvbS4kb0l0ZW1zIHx8ICFpbml0IHx8IHRoaXMuc3RhdGUubGF6eUNvbnRlbnQgKXtyZXR1cm4gZmFsc2U7fVxyXG5cdFx0dmFyIHBvcyA9IHRoaXMub3B0aW9ucy5zdGFydFBvc2l0aW9uO1xyXG5cclxuXHRcdGlmKHRoaXMub3B0aW9ucy5zdGFydFBvc2l0aW9uID09PSAnVVJMSGFzaCcpe1xyXG5cdFx0XHRwb3MgPSB0aGlzLm9wdGlvbnMuc3RhcnRQb3NpdGlvbiA9IHRoaXMuaGFzaFBvc2l0aW9uKCk7XHJcblx0XHR9IGVsc2UgaWYodHlwZW9mIHRoaXMub3B0aW9ucy5zdGFydFBvc2l0aW9uICE9PSBOdW1iZXIgJiYgIXRoaXMub3B0aW9ucy5jZW50ZXIpe1xyXG5cdFx0XHR0aGlzLm9wdGlvbnMuc3RhcnRQb3NpdGlvbiA9IDA7XHJcblx0XHR9XHJcblx0XHR0aGlzLmRvbS5vU3RhZ2Uuc2Nyb2xsTGVmdCA9IDA7XHJcblx0XHR0aGlzLmp1bXBUbyhwb3MsdHJ1ZSk7XHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICogZ29Ub0hhc2hcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS5nb1RvSGFzaCA9IGZ1bmN0aW9uKCl7XHJcblx0XHR2YXIgcG9zID0gdGhpcy5oYXNoUG9zaXRpb24oKTtcclxuXHRcdGlmKHBvcyA9PT0gZmFsc2Upe1xyXG5cdFx0XHRwb3MgPSAwO1xyXG5cdFx0fVxyXG5cdFx0dGhpcy5kb20ub1N0YWdlLnNjcm9sbExlZnQgPSAwO1xyXG5cdFx0dGhpcy5nb1RvKHBvcyx0aGlzLm9wdGlvbnMubmF2U3BlZWQpO1xyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIGhhc2hQb3NpdGlvblxyXG5cdCAqIEBkZXNjIEZpbmQgaGFzaCBpbiBVUkwgdGhlbiBsb29rIGludG8gaXRlbXMgdG8gZmluZCBjb250YWluZWQgSURcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKiByZXR1cm4gaGFzaFBvcyAtIG51bWJlciBvZiBpdGVtXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUuaGFzaFBvc2l0aW9uID0gZnVuY3Rpb24oKXtcclxuXHRcdHZhciBoYXNoID0gd2luZG93LmxvY2F0aW9uLmhhc2guc3Vic3RyaW5nKDEpLFxyXG5cdFx0XHRoYXNoUG9zO1xyXG5cdFx0aWYoaGFzaCA9PT0gXCJcIil7cmV0dXJuIGZhbHNlO31cclxuXHJcblx0XHRmb3IodmFyIGk9MDtpPHRoaXMubnVtLm9JdGVtczsgaSsrKXtcclxuXHRcdFx0aWYoaGFzaCA9PT0gdGhpcy5kb20uJG9JdGVtcy5lcShpKS5kYXRhKCdvd2wtaXRlbScpLmhhc2gpe1xyXG5cdFx0XHRcdGhhc2hQb3MgPSBpO1xyXG5cdFx0XHR9XHJcblx0XHR9XHJcblx0XHRyZXR1cm4gaGFzaFBvcztcclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiBBdXRvcGxheVxyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqL1xyXG5cclxuXHRPd2wucHJvdG90eXBlLmF1dG9wbGF5ID0gZnVuY3Rpb24oKXtcclxuXHRcdGlmKHRoaXMub3B0aW9ucy5hdXRvcGxheSAmJiAhdGhpcy5zdGF0ZS52aWRlb1BsYXkpe1xyXG5cdFx0XHR3aW5kb3cuY2xlYXJJbnRlcnZhbCh0aGlzLmUuX2F1dG9wbGF5KTtcclxuXHRcdFx0dGhpcy5lLl9hdXRvcGxheSA9IHdpbmRvdy5zZXRJbnRlcnZhbCh0aGlzLmUuX3BsYXksIHRoaXMub3B0aW9ucy5hdXRvcGxheVRpbWVvdXQpO1xyXG5cdFx0fSBlbHNlIHtcclxuXHRcdFx0d2luZG93LmNsZWFySW50ZXJ2YWwodGhpcy5lLl9hdXRvcGxheSk7XHJcblx0XHRcdHRoaXMuc3RhdGUuYXV0b3BsYXk9ZmFsc2U7XHJcblx0XHR9XHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICogcGxheVxyXG5cdCAqIEBwYXJhbSBbdGltZW91dF0gLSBJbnRlZ3JlclxyXG5cdCAqIEBwYXJhbSBbc3BlZWRdIC0gSW50ZWdyZXJcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS5wbGF5ID0gZnVuY3Rpb24odGltZW91dCwgc3BlZWQpe1xyXG5cclxuXHRcdC8vIGlmIHRhYiBpcyBpbmFjdGl2ZSAtIGRvZXNudCB3b3JrIGluIDxJRTEwXHJcblx0XHRpZihkb2N1bWVudC5oaWRkZW4gPT09IHRydWUpe3JldHVybiBmYWxzZTt9XHJcblxyXG5cdFx0Ly8gb3ZlcndyaXRlIGRlZmF1bHQgb3B0aW9ucyAoY3VzdG9tIG9wdGlvbnMgYXJlIGFsd2F5cyBwcmlvcml0eSlcclxuXHRcdGlmKCF0aGlzLm9wdGlvbnMuYXV0b3BsYXkpe1xyXG5cdFx0XHR0aGlzLl9vcHRpb25zLmF1dG9wbGF5ID0gdGhpcy5vcHRpb25zLmF1dG9wbGF5ID0gdHJ1ZTtcclxuXHRcdFx0dGhpcy5fb3B0aW9ucy5hdXRvcGxheVRpbWVvdXQgPSB0aGlzLm9wdGlvbnMuYXV0b3BsYXlUaW1lb3V0ID0gdGltZW91dCB8fCB0aGlzLm9wdGlvbnMuYXV0b3BsYXlUaW1lb3V0IHx8IDQwMDA7XHJcblx0XHRcdHRoaXMuX29wdGlvbnMuYXV0b3BsYXlTcGVlZCA9IHNwZWVkIHx8IHRoaXMub3B0aW9ucy5hdXRvcGxheVNwZWVkO1xyXG5cdFx0fVxyXG5cclxuXHRcdGlmKHRoaXMub3B0aW9ucy5hdXRvcGxheSA9PT0gZmFsc2UgfHwgdGhpcy5zdGF0ZS5pc1RvdWNoIHx8IHRoaXMuc3RhdGUuaXNTY3JvbGxpbmcgfHwgdGhpcy5zdGF0ZS5pc1N3aXBpbmcgfHwgdGhpcy5zdGF0ZS5pbk1vdGlvbil7XHJcblx0XHRcdHdpbmRvdy5jbGVhckludGVydmFsKHRoaXMuZS5fYXV0b3BsYXkpO1xyXG5cdFx0XHRyZXR1cm4gZmFsc2U7XHJcblx0XHR9XHJcblxyXG5cdFx0aWYoIXRoaXMub3B0aW9ucy5sb29wICYmIHRoaXMucG9zLmN1cnJlbnQgPj0gdGhpcy5wb3MubWF4KXtcclxuXHRcdFx0d2luZG93LmNsZWFySW50ZXJ2YWwodGhpcy5lLl9hdXRvcGxheSk7XHJcblx0XHRcdHRoaXMuZ29UbygwKTtcclxuXHRcdH0gZWxzZSB7XHJcblx0XHRcdHRoaXMubmV4dCh0aGlzLm9wdGlvbnMuYXV0b3BsYXlTcGVlZCk7XHJcblx0XHR9XHJcblx0XHR0aGlzLnN0YXRlLmF1dG9wbGF5PXRydWU7XHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICogc3RvcFxyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqL1xyXG5cclxuXHRPd2wucHJvdG90eXBlLnN0b3AgPSBmdW5jdGlvbigpe1xyXG5cdFx0dGhpcy5fb3B0aW9ucy5hdXRvcGxheSA9IHRoaXMub3B0aW9ucy5hdXRvcGxheSA9IGZhbHNlO1xyXG5cdFx0dGhpcy5zdGF0ZS5hdXRvcGxheSA9IGZhbHNlO1xyXG5cdFx0d2luZG93LmNsZWFySW50ZXJ2YWwodGhpcy5lLl9hdXRvcGxheSk7XHJcblx0fTtcclxuXHJcblx0T3dsLnByb3RvdHlwZS5wYXVzZSA9IGZ1bmN0aW9uKCl7XHJcblx0XHR3aW5kb3cuY2xlYXJJbnRlcnZhbCh0aGlzLmUuX2F1dG9wbGF5KTtcclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiB0cmFuc2l0aW9uRW5kXHJcblx0ICogQGRlc2MgZXZlbnQgdXNlZCBieSBjc3MzIGFuaW1hdGlvbiBlbmQgYW5kICQuYW5pbWF0ZSBjYWxsYmFjayBsaWtlIHRyYW5zaXRpb25FbmQscmVzcG9uc2l2ZSBldGMuXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUudHJhbnNpdGlvbkVuZCA9IGZ1bmN0aW9uKGV2ZW50KXtcclxuXHJcblx0XHQvLyBpZiBjc3MyIGFuaW1hdGlvbiB0aGVuIGV2ZW50IG9iamVjdCBpcyB1bmRlZmluZWQgXHJcblx0XHRpZihldmVudCAhPT0gdW5kZWZpbmVkKXtcclxuXHRcdFx0ZXZlbnQuc3RvcFByb3BhZ2F0aW9uKCk7XHJcblxyXG5cdFx0XHQvLyBDYXRjaCBvbmx5IG93bC1zdGFnZSB0cmFuc2l0aW9uRW5kIGV2ZW50XHJcblx0XHRcdHZhciBldmVudFRhcmdldCA9IGV2ZW50LnRhcmdldCB8fCBldmVudC5zcmNFbGVtZW50IHx8IGV2ZW50Lm9yaWdpbmFsVGFyZ2V0O1xyXG5cdFx0XHRpZihldmVudFRhcmdldCAhPT0gdGhpcy5kb20uc3RhZ2UpeyBcclxuXHRcdFx0XHRyZXR1cm4gZmFsc2U7XHJcblx0XHRcdH1cclxuXHRcdH1cclxuXHJcblx0XHR0aGlzLnN0YXRlLmluTW90aW9uID0gZmFsc2U7XHJcblx0XHR0aGlzLnVwZGF0ZUl0ZW1TdGF0ZSgpO1xyXG5cdFx0dGhpcy5hdXRvcGxheSgpO1xyXG5cdFx0dGhpcy5maXJlQ2FsbGJhY2soJ29uVHJhbnNpdGlvbkVuZCcpO1xyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIGlzRWxXaWR0aENoYW5nZWRcclxuXHQgKiBAZGVzYyBDaGVjayBpZiBlbGVtZW50IHdpZHRoIGhhcyBjaGFuZ2VkXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUuaXNFbFdpZHRoQ2hhbmdlZCA9IGZ1bmN0aW9uKCl7XHJcblx0XHR2YXIgbmV3RWxXaWR0aCA9IFx0dGhpcy5kb20uJGVsLndpZHRoKCkgLSB0aGlzLm9wdGlvbnMuc3RhZ2VQYWRkaW5nLC8vdG8gY2hlY2tcclxuXHRcdFx0cHJldkVsV2lkdGggPSBcdHRoaXMud2lkdGguZWwgKyB0aGlzLm9wdGlvbnMubWFyZ2luO1xyXG5cdFx0cmV0dXJuIG5ld0VsV2lkdGggIT09IHByZXZFbFdpZHRoO1xyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIHdpbmRvd1dpZHRoXHJcblx0ICogQGRlc2MgR2V0IFdpbmRvdy9yZXNwb25zaXZlQmFzZUVsZW1lbnQgd2lkdGhcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS53aW5kb3dXaWR0aCA9IGZ1bmN0aW9uKCkge1xyXG5cdFx0aWYodGhpcy5vcHRpb25zLnJlc3BvbnNpdmVCYXNlRWxlbWVudCAhPT0gd2luZG93KXtcclxuXHRcdFx0dGhpcy53aWR0aC53aW5kb3cgPSAgJCh0aGlzLm9wdGlvbnMucmVzcG9uc2l2ZUJhc2VFbGVtZW50KS53aWR0aCgpO1xyXG5cdFx0fSBlbHNlIGlmICh3aW5kb3cuaW5uZXJXaWR0aCl7XHJcblx0XHRcdHRoaXMud2lkdGgud2luZG93ID0gd2luZG93LmlubmVyV2lkdGg7XHJcblx0XHR9IGVsc2UgaWYgKGRvY3VtZW50LmRvY3VtZW50RWxlbWVudCAmJiBkb2N1bWVudC5kb2N1bWVudEVsZW1lbnQuY2xpZW50V2lkdGgpe1xyXG5cdFx0XHR0aGlzLndpZHRoLndpbmRvdyA9IGRvY3VtZW50LmRvY3VtZW50RWxlbWVudC5jbGllbnRXaWR0aDtcclxuXHRcdH1cclxuXHRcdHJldHVybiB0aGlzLndpZHRoLndpbmRvdztcclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiBDb250cm9sc1xyXG5cdCAqIEBkZXNjIENhbGxzIGNvbnRyb2xzIGNvbnRhaW5lciwgbmF2aWdhdGlvbiBhbmQgZG90cyBjcmVhdG9yXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUuY29udHJvbHMgPSBmdW5jdGlvbigpe1xyXG5cdFx0dmFyIGNjID0gZG9jdW1lbnQuY3JlYXRlRWxlbWVudCgnZGl2Jyk7XHJcblx0XHRjYy5jbGFzc05hbWUgPSB0aGlzLm9wdGlvbnMuY29udHJvbHNDbGFzcztcclxuXHRcdHRoaXMuZG9tLiRlbC5hcHBlbmQoY2MpO1xyXG5cdFx0dGhpcy5kb20uJGNjID0gJChjYyk7XHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICogdXBkYXRlQ29udHJvbHMgXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUudXBkYXRlQ29udHJvbHMgPSBmdW5jdGlvbigpe1xyXG5cclxuXHRcdGlmKHRoaXMuZG9tLiRjYyA9PT0gbnVsbCAmJiAodGhpcy5vcHRpb25zLm5hdiB8fCB0aGlzLm9wdGlvbnMuZG90cykpe1xyXG5cdFx0XHR0aGlzLmNvbnRyb2xzKCk7XHJcblx0XHR9XHJcblxyXG5cdFx0aWYodGhpcy5kb20uJG5hdiA9PT0gbnVsbCAmJiB0aGlzLm9wdGlvbnMubmF2KXtcclxuXHRcdFx0dGhpcy5jcmVhdGVOYXZpZ2F0aW9uKHRoaXMuZG9tLiRjY1swXSk7XHJcblx0XHR9XHJcblx0XHRcclxuXHRcdGlmKHRoaXMuZG9tLiRwYWdlID09PSBudWxsICYmIHRoaXMub3B0aW9ucy5kb3RzKXtcclxuXHRcdFx0dGhpcy5jcmVhdGVEb3RzKHRoaXMuZG9tLiRjY1swXSk7XHJcblx0XHR9XHJcblxyXG5cdFx0aWYodGhpcy5kb20uJG5hdiAhPT0gbnVsbCl7XHJcblx0XHRcdGlmKHRoaXMub3B0aW9ucy5uYXYpe1xyXG5cdFx0XHRcdHRoaXMuZG9tLiRuYXYuc2hvdygpO1xyXG5cdFx0XHRcdHRoaXMudXBkYXRlTmF2aWdhdGlvbigpO1xyXG5cdFx0XHR9IGVsc2Uge1xyXG5cdFx0XHRcdHRoaXMuZG9tLiRuYXYuaGlkZSgpO1xyXG5cdFx0XHR9XHJcblx0XHR9XHJcblxyXG5cdFx0aWYodGhpcy5kb20uJHBhZ2UgIT09IG51bGwpe1xyXG5cdFx0XHRpZih0aGlzLm9wdGlvbnMuZG90cyl7XHJcblx0XHRcdFx0dGhpcy5kb20uJHBhZ2Uuc2hvdygpO1xyXG5cdFx0XHRcdHRoaXMudXBkYXRlRG90cygpO1xyXG5cdFx0XHR9IGVsc2Uge1xyXG5cdFx0XHRcdHRoaXMuZG9tLiRwYWdlLmhpZGUoKTtcclxuXHRcdFx0fVxyXG5cdFx0fVxyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIGNyZWF0ZU5hdmlnYXRpb25cclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKiBAcGFyYW0gW2NjXSAtIGRvbSBlbGVtZW50IC0gQ29udHJvbHMgQ29udGFpbmVyXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUuY3JlYXRlTmF2aWdhdGlvbiA9IGZ1bmN0aW9uKGNjKXtcclxuXHJcblx0XHQvLyBDcmVhdGUgbmF2IGNvbnRhaW5lclxyXG5cdFx0dmFyIG5hdiA9IGRvY3VtZW50LmNyZWF0ZUVsZW1lbnQoJ2RpdicpO1xyXG5cdFx0bmF2LmNsYXNzTmFtZSA9IHRoaXMub3B0aW9ucy5uYXZDb250YWluZXJDbGFzcztcclxuXHRcdGNjLmFwcGVuZENoaWxkKG5hdik7XHJcblxyXG5cdFx0Ly8gQ3JlYXRlIGxlZnQgYW5kIHJpZ2h0IGJ1dHRvbnNcclxuXHRcdHZhciBuYXZQcmV2ID0gZG9jdW1lbnQuY3JlYXRlRWxlbWVudCgnZGl2JyksXHJcblx0XHRcdG5hdk5leHQgPSBkb2N1bWVudC5jcmVhdGVFbGVtZW50KCdkaXYnKTtcclxuXHJcblx0XHRuYXZQcmV2LmNsYXNzTmFtZSA9IHRoaXMub3B0aW9ucy5uYXZDbGFzc1swXTtcclxuXHRcdG5hdk5leHQuY2xhc3NOYW1lID0gdGhpcy5vcHRpb25zLm5hdkNsYXNzWzFdO1xyXG5cclxuXHRcdG5hdi5hcHBlbmRDaGlsZChuYXZQcmV2KTtcclxuXHRcdG5hdi5hcHBlbmRDaGlsZChuYXZOZXh0KTtcclxuXHJcblx0XHR0aGlzLmRvbS4kbmF2ID0gJChuYXYpO1xyXG5cdFx0dGhpcy5kb20uJG5hdlByZXYgPSAkKG5hdlByZXYpLmh0bWwodGhpcy5vcHRpb25zLm5hdlRleHRbMF0pO1xyXG5cdFx0dGhpcy5kb20uJG5hdk5leHQgPSAkKG5hdk5leHQpLmh0bWwodGhpcy5vcHRpb25zLm5hdlRleHRbMV0pO1xyXG5cclxuXHRcdC8vIGFkZCBldmVudHMgdG8gZG9cclxuXHRcdC8vdGhpcy5vbihuYXZQcmV2LCB0aGlzLmRyYWdUeXBlWzJdLCB0aGlzLmUuX25hdlByZXYsIGZhbHNlKTtcclxuXHRcdC8vdGhpcy5vbihuYXZOZXh0LCB0aGlzLmRyYWdUeXBlWzJdLCB0aGlzLmUuX25hdk5leHQsIGZhbHNlKTtcclxuXHJcblx0XHQvL0ZGIGZpeD9cclxuXHRcdHRoaXMuZG9tLiRuYXYub24odGhpcy5kcmFnVHlwZVsyXSwgJy4nK3RoaXMub3B0aW9ucy5uYXZDbGFzc1swXSwgdGhpcy5lLl9uYXZQcmV2KTtcclxuXHRcdHRoaXMuZG9tLiRuYXYub24odGhpcy5kcmFnVHlwZVsyXSwgJy4nK3RoaXMub3B0aW9ucy5uYXZDbGFzc1sxXSwgdGhpcy5lLl9uYXZOZXh0KTtcclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiBjcmVhdGVOYXZpZ2F0aW9uXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICogQHBhcmFtIFtjY10gLSBkb20gZWxlbWVudCAtIENvbnRyb2xzIENvbnRhaW5lclxyXG5cdCAqL1xyXG5cclxuXHRPd2wucHJvdG90eXBlLmNyZWF0ZURvdHMgPSBmdW5jdGlvbihjYyl7XHJcblxyXG5cdFx0Ly8gQ3JlYXRlIGRvdHMgY29udGFpbmVyXHJcblx0XHR2YXIgcGFnZSA9IGRvY3VtZW50LmNyZWF0ZUVsZW1lbnQoJ2RpdicpO1xyXG5cdFx0cGFnZS5jbGFzc05hbWUgPSB0aGlzLm9wdGlvbnMuZG90c0NsYXNzO1xyXG5cdFx0Y2MuYXBwZW5kQ2hpbGQocGFnZSk7XHJcblxyXG5cdFx0Ly8gc2F2ZSByZWZlcmVuY2VcclxuXHRcdHRoaXMuZG9tLiRwYWdlID0gJChwYWdlKTtcclxuXHJcblx0XHQvLyBhZGQgZXZlbnRzXHJcblx0XHQvL3RoaXMub24ocGFnZSwgdGhpcy5kcmFnVHlwZVsyXSwgdGhpcy5lLl9nb1RvUGFnZSwgZmFsc2UpO1xyXG5cclxuXHRcdC8vIEZGIGZpeD8gVG8gdGVzdCFcclxuXHRcdHZhciB0aGF0ID0gdGhpcztcclxuXHRcdHRoaXMuZG9tLiRwYWdlLm9uKHRoaXMuZHJhZ1R5cGVbMl0sICcuJyt0aGlzLm9wdGlvbnMuZG90Q2xhc3MsIGdvVG9QYWdlKTtcclxuXHJcblx0XHRmdW5jdGlvbiBnb1RvUGFnZShlKXtcclxuXHRcdFx0ZS5wcmV2ZW50RGVmYXVsdCgpO1xyXG5cdFx0XHR2YXIgcGFnZSA9ICQodGhpcykuZGF0YSgncGFnZScpO1xyXG5cdFx0XHR0aGF0LmdvVG8ocGFnZSx0aGF0Lm9wdGlvbnMuZG90c1NwZWVkKTtcclxuXHRcdH1cclxuXHRcdC8vIGJ1aWxkIGRvdHNcclxuXHRcdHRoaXMucmVidWlsZERvdHMoKTtcclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiBnb1RvUGFnZVxyXG5cdCAqIEBkZXNjIEV2ZW50IHVzZWQgYnkgZG90c1xyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqL1xyXG5cclxuXHQvLyBPd2wucHJvdG90eXBlLmdvVG9QYWdlID0gZnVuY3Rpb24oZSl7XHJcblx0Ly8gXHRjb25zb2xlLmxvZyhlLnRhZ2V0KTtcclxuXHQvLyBcdHZhciBwYWdlID0gJChlLmN1cnJlbnRUYXJnZXQpLmRhdGEoJ3BhZ2UnKVxyXG5cdC8vIFx0dGhpcy5nb1RvKHBhZ2UsdGhpcy5vcHRpb25zLmRvdHNTcGVlZCk7XHJcblx0Ly8gXHRyZXR1cm4gZmFsc2U7XHJcblx0Ly8gfTtcclxuXHJcblx0LyoqXHJcblx0ICogcmVidWlsZERvdHNcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS5yZWJ1aWxkRG90cyA9IGZ1bmN0aW9uKCl7XHJcblx0XHRpZih0aGlzLmRvbS4kcGFnZSA9PT0gbnVsbCl7cmV0dXJuIGZhbHNlO31cclxuXHRcdHZhciBlYWNoLCBkb3QsIHNwYW4sIGNvdW50ZXIgPSAwLCBsYXN0ID0gMCwgaSwgcGFnZT0wLCByb3VuZFBhZ2VzID0gMDtcclxuXHJcblx0XHRlYWNoID0gdGhpcy5vcHRpb25zLmRvdHNFYWNoIHx8IHRoaXMub3B0aW9ucy5pdGVtcztcclxuXHJcblx0XHQvLyBkaXNwbGF5IGZ1bGwgZG90cyBpZiBjZW50ZXJcclxuXHRcdGlmKHRoaXMub3B0aW9ucy5jZW50ZXIgfHwgdGhpcy5vcHRpb25zLmRvdERhdGEpe1xyXG5cdFx0XHRlYWNoID0gMTtcclxuXHRcdH1cclxuXHJcblx0XHQvLyBjbGVhciBkb3RzXHJcblx0XHR0aGlzLmRvbS4kcGFnZS5odG1sKCcnKTtcclxuXHJcblx0XHRmb3IoaSA9IDA7IGkgPCB0aGlzLm51bS5uYXYubGVuZ3RoOyBpKyspe1xyXG5cclxuXHRcdFx0aWYoY291bnRlciA+PSBlYWNoIHx8IGNvdW50ZXIgPT09IDApe1xyXG5cclxuXHRcdFx0XHRkb3QgPSBkb2N1bWVudC5jcmVhdGVFbGVtZW50KCdkaXYnKTtcclxuXHRcdFx0XHRkb3QuY2xhc3NOYW1lID0gdGhpcy5vcHRpb25zLmRvdENsYXNzO1xyXG5cdFx0XHRcdHNwYW4gPSBkb2N1bWVudC5jcmVhdGVFbGVtZW50KCdzcGFuJyk7XHJcblx0XHRcdFx0ZG90LmFwcGVuZENoaWxkKHNwYW4pO1xyXG5cdFx0XHRcdHZhciAkZG90ID0gJChkb3QpO1xyXG5cclxuXHRcdFx0XHRpZih0aGlzLm9wdGlvbnMuZG90RGF0YSl7XHJcblx0XHRcdFx0XHQkZG90Lmh0bWwodGhpcy5kb20uJG9JdGVtcy5lcShpKS5kYXRhKCdvd2wtaXRlbScpLmRvdCk7XHJcblx0XHRcdFx0fVxyXG5cclxuXHRcdFx0XHQkZG90LmRhdGEoJ3BhZ2UnLHBhZ2UpO1xyXG5cdFx0XHRcdCRkb3QuZGF0YSgnZ29Ub1BhZ2UnLHJvdW5kUGFnZXMpO1xyXG5cclxuXHRcdFx0XHR0aGlzLmRvbS4kcGFnZS5hcHBlbmQoZG90KTtcclxuXHJcblx0XHRcdFx0Y291bnRlciA9IDA7XHJcblx0XHRcdFx0cm91bmRQYWdlcysrO1xyXG5cdFx0XHR9XHJcblxyXG5cdFx0XHR0aGlzLmRvbS4kb0l0ZW1zLmVxKGkpLmRhdGEoJ293bC1pdGVtJykucGFnZSA9IHJvdW5kUGFnZXMtMTtcclxuXHJcblx0XHRcdC8vYWRkIG1lcmdlZCBpdGVtc1xyXG5cdFx0XHRjb3VudGVyICs9IHRoaXMubnVtLm5hdltpXTtcclxuXHRcdFx0cGFnZSsrO1xyXG5cdFx0fVxyXG5cdFx0Ly8gZmluZCByZXN0IG9mIGRvdHNcclxuXHRcdGlmKCF0aGlzLm9wdGlvbnMubG9vcCAmJiAhdGhpcy5vcHRpb25zLmNlbnRlcil7XHJcblx0XHRcdGZvcih2YXIgaiA9IHRoaXMubnVtLm5hdi5sZW5ndGgtMTsgaiA+PSAwOyBqLS0pe1xyXG5cdFx0XHRcdGxhc3QgKz0gdGhpcy5udW0ubmF2W2pdO1xyXG5cdFx0XHRcdHRoaXMuZG9tLiRvSXRlbXMuZXEoaikuZGF0YSgnb3dsLWl0ZW0nKS5wYWdlID0gcm91bmRQYWdlcy0xO1xyXG5cdFx0XHRcdGlmKGxhc3QgPj0gZWFjaCl7XHJcblx0XHRcdFx0XHRicmVhaztcclxuXHRcdFx0XHR9XHJcblx0XHRcdH1cclxuXHRcdH1cclxuXHJcblx0XHR0aGlzLm51bS5hbGxQYWdlcyA9IHJvdW5kUGFnZXMtMTtcclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiB1cGRhdGVEb3RzXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUudXBkYXRlRG90cyA9IGZ1bmN0aW9uKCl7XHJcblx0XHR2YXIgZG90cyA9IHRoaXMuZG9tLiRwYWdlLmNoaWxkcmVuKCk7XHJcblx0XHR2YXIgaXRlbUluZGV4ID0gdGhpcy5kb20uJG9JdGVtcy5lcSh0aGlzLnBvcy5jdXJyZW50KS5kYXRhKCdvd2wtaXRlbScpLnBhZ2U7XHJcblx0XHRcclxuXHRcdGZvcih2YXIgaSA9IDA7IGkgPCBkb3RzLmxlbmd0aDsgaSsrKXtcclxuXHRcdFx0dmFyIGRvdFBhZ2UgPSBkb3RzLmVxKGkpLmRhdGEoJ2dvVG9QYWdlJyk7XHJcblxyXG5cdFx0XHRpZihkb3RQYWdlPT09aXRlbUluZGV4KXtcclxuXHRcdFx0XHR0aGlzLnBvcy5jdXJyZW50UGFnZSA9IGk7XHJcblx0XHRcdFx0ZG90cy5lcShpKS5hZGRDbGFzcygnYWN0aXZlJyk7XHJcblx0XHRcdH1lbHNle1xyXG5cdFx0XHRcdGRvdHMuZXEoaSkucmVtb3ZlQ2xhc3MoJ2FjdGl2ZScpO1xyXG5cdFx0XHR9XHJcblx0XHR9XHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICogdXBkYXRlTmF2aWdhdGlvblxyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqL1xyXG5cclxuXHRPd2wucHJvdG90eXBlLnVwZGF0ZU5hdmlnYXRpb24gPSBmdW5jdGlvbigpe1xyXG5cclxuXHRcdHZhciBpc05hdiA9IHRoaXMub3B0aW9ucy5uYXY7XHJcblxyXG5cdFx0dGhpcy5kb20uJG5hdk5leHQudG9nZ2xlQ2xhc3MoJ2Rpc2FibGVkJywhaXNOYXYpO1xyXG5cdFx0dGhpcy5kb20uJG5hdlByZXYudG9nZ2xlQ2xhc3MoJ2Rpc2FibGVkJywhaXNOYXYpO1xyXG5cclxuXHRcdGlmKCF0aGlzLm9wdGlvbnMubG9vcCAmJiBpc05hdiAmJiAhdGhpcy5vcHRpb25zLm5hdlJld2luZCl7XHJcblxyXG5cdFx0XHRpZih0aGlzLnBvcy5jdXJyZW50IDw9IDApe1xyXG5cdFx0XHRcdHRoaXMuZG9tLiRuYXZQcmV2LmFkZENsYXNzKCdkaXNhYmxlZCcpO1xyXG5cdFx0XHR9IFxyXG5cdFx0XHRpZih0aGlzLnBvcy5jdXJyZW50ID49IHRoaXMucG9zLm1heCl7XHJcblx0XHRcdFx0dGhpcy5kb20uJG5hdk5leHQuYWRkQ2xhc3MoJ2Rpc2FibGVkJyk7XHJcblx0XHRcdH1cclxuXHRcdH1cclxuXHR9O1xyXG5cclxuXHRPd2wucHJvdG90eXBlLmluc2VydENvbnRlbnQgPSBmdW5jdGlvbihjb250ZW50KXtcclxuXHRcdHRoaXMuZG9tLiRzdGFnZS5lbXB0eSgpO1xyXG5cdFx0dGhpcy5mZXRjaENvbnRlbnQoY29udGVudCk7XHJcblx0XHR0aGlzLnJlZnJlc2goKTtcclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiBhZGRJdGVtIC0gQWRkIGFuIGl0ZW1cclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKiBAcGFyYW0gW2NvbnRlbnRdIC0gZG9tIGVsZW1lbnQgLyBzdHJpbmcgJzxkaXY+Y29udGVudDwvZGl2PidcclxuXHQgKiBAcGFyYW0gW3Bvc10gLSBudW1iZXIgLSBwb3NpdGlvblxyXG5cdCAqL1xyXG5cclxuXHRPd2wucHJvdG90eXBlLmFkZEl0ZW0gPSBmdW5jdGlvbihjb250ZW50LHBvcyl7XHJcblx0XHRwb3MgPSBwb3MgfHwgMDtcclxuXHJcblx0XHRpZih0aGlzLnN0YXRlLmxhenlDb250ZW50KXtcclxuXHRcdFx0dGhpcy5kb20uJGNvbnRlbnQgPSB0aGlzLmRvbS4kY29udGVudC5hZGQoJChjb250ZW50KSk7XHJcblx0XHRcdHRoaXMudXBkYXRlSXRlbVN0YXRlKHRydWUpO1xyXG5cdFx0fSBlbHNlIHtcclxuXHRcdFx0Ly8gd3JhcCBjb250ZW50XHJcblx0XHRcdHZhciBpdGVtID0gdGhpcy5maWxsSXRlbShjb250ZW50KTtcclxuXHRcdFx0Ly8gaWYgY2Fyb3VzZWwgaXMgZW1wdHkgdGhlbiBhcHBlbmQgaXRlbVxyXG5cdFx0XHRpZih0aGlzLmRvbS4kb0l0ZW1zLmxlbmd0aCA9PT0gMCl7XHJcblx0XHRcdFx0dGhpcy5kb20uJHN0YWdlLmFwcGVuZChpdGVtKTtcclxuXHRcdFx0fSBlbHNlIHtcclxuXHRcdFx0XHQvLyBhcHBlbmQgaXRlbVxyXG5cdFx0XHRcdHZhciBpdCA9IHRoaXMuZG9tLiRvSXRlbXMuZXEocG9zKTtcclxuXHRcdFx0XHRpZihwb3MgIT09IC0xKXtpdC5iZWZvcmUoaXRlbSk7fSBlbHNlIHtpdC5hZnRlcihpdGVtKTt9XHJcblx0XHRcdH1cclxuXHRcdFx0Ly8gdXBkYXRlIGFuZCBjYWxjdWxhdGUgY2Fyb3VzZWxcclxuXHRcdFx0dGhpcy5yZWZyZXNoKCk7XHJcblx0XHR9XHJcblxyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIHJlbW92ZUl0ZW0gLSBSZW1vdmUgYW4gSXRlbVxyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqIEBwYXJhbSBbcG9zXSAtIG51bWJlciAtIHBvc2l0aW9uXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUucmVtb3ZlSXRlbSA9IGZ1bmN0aW9uKHBvcyl7XHJcblx0XHRpZih0aGlzLnN0YXRlLmxhenlDb250ZW50KXtcclxuXHRcdFx0dGhpcy5kb20uJGNvbnRlbnQuc3BsaWNlKHBvcywxKTtcclxuXHRcdFx0dGhpcy51cGRhdGVJdGVtU3RhdGUodHJ1ZSk7XHJcblx0XHR9IGVsc2Uge1xyXG5cdFx0XHR0aGlzLmRvbS4kb0l0ZW1zLmVxKHBvcykucmVtb3ZlKCk7XHJcblx0XHRcdHRoaXMucmVmcmVzaCgpO1xyXG5cdFx0fVxyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIGFkZEN1c3RvbUV2ZW50c1xyXG5cdCAqIEBkZXNjIEFkZCBjdXN0b20gZXZlbnRzIGJ5IGpRdWVyeSAub24gbWV0aG9kXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUuYWRkQ3VzdG9tRXZlbnRzID0gZnVuY3Rpb24oKXtcclxuXHJcblx0XHR0aGlzLmUubmV4dCA9IGZ1bmN0aW9uKGUscyl7dGhpcy5uZXh0KHMpO1x0XHRcdH0uYmluZCh0aGlzKTtcclxuXHRcdHRoaXMuZS5wcmV2ID0gZnVuY3Rpb24oZSxzKXt0aGlzLnByZXYocyk7XHRcdFx0fS5iaW5kKHRoaXMpO1xyXG5cdFx0dGhpcy5lLmdvVG8gPSBmdW5jdGlvbihlLHAscyl7dGhpcy5nb1RvKHAscyk7XHRcdH0uYmluZCh0aGlzKTtcclxuXHRcdHRoaXMuZS5qdW1wVG8gPSBmdW5jdGlvbihlLHApe3RoaXMuanVtcFRvKHApO1x0XHR9LmJpbmQodGhpcyk7XHJcblx0XHR0aGlzLmUuYWRkSXRlbSA9IGZ1bmN0aW9uKGUsYyxwKXt0aGlzLmFkZEl0ZW0oYyxwKTtcdH0uYmluZCh0aGlzKTtcclxuXHRcdHRoaXMuZS5yZW1vdmVJdGVtID0gZnVuY3Rpb24oZSxwKXt0aGlzLnJlbW92ZUl0ZW0ocCk7fS5iaW5kKHRoaXMpO1xyXG5cdFx0dGhpcy5lLnJlZnJlc2ggPSBmdW5jdGlvbihlKXt0aGlzLnJlZnJlc2goKTtcdFx0fS5iaW5kKHRoaXMpO1xyXG5cdFx0dGhpcy5lLmRlc3Ryb3kgPSBmdW5jdGlvbihlKXt0aGlzLmRlc3Ryb3koKTtcdFx0fS5iaW5kKHRoaXMpO1xyXG5cdFx0dGhpcy5lLmF1dG9IZWlnaHQgPSBmdW5jdGlvbihlKXt0aGlzLmF1dG9IZWlnaHQodHJ1ZSk7fS5iaW5kKHRoaXMpO1xyXG5cdFx0dGhpcy5lLnN0b3AgPSBmdW5jdGlvbigpe3RoaXMuc3RvcCgpO1x0XHRcdFx0fS5iaW5kKHRoaXMpO1xyXG5cdFx0dGhpcy5lLnBsYXkgPSBmdW5jdGlvbihlLHQscyl7dGhpcy5wbGF5KHQscyk7XHRcdH0uYmluZCh0aGlzKTtcclxuXHRcdHRoaXMuZS5pbnNlcnRDb250ZW50ID0gZnVuY3Rpb24oZSxkKXt0aGlzLmluc2VydENvbnRlbnQoZCk7XHR9LmJpbmQodGhpcyk7XHJcblxyXG5cdFx0dGhpcy5kb20uJGVsLm9uKCduZXh0Lm93bCcsdGhpcy5lLm5leHQpO1xyXG5cdFx0dGhpcy5kb20uJGVsLm9uKCdwcmV2Lm93bCcsdGhpcy5lLnByZXYpO1xyXG5cdFx0dGhpcy5kb20uJGVsLm9uKCdnb1RvLm93bCcsdGhpcy5lLmdvVG8pO1xyXG5cdFx0dGhpcy5kb20uJGVsLm9uKCdqdW1wVG8ub3dsJyx0aGlzLmUuanVtcFRvKTtcclxuXHRcdHRoaXMuZG9tLiRlbC5vbignYWRkSXRlbS5vd2wnLHRoaXMuZS5hZGRJdGVtKTtcclxuXHRcdHRoaXMuZG9tLiRlbC5vbigncmVtb3ZlSXRlbS5vd2wnLHRoaXMuZS5yZW1vdmVJdGVtKTtcclxuXHRcdHRoaXMuZG9tLiRlbC5vbignZGVzdHJveS5vd2wnLHRoaXMuZS5kZXN0cm95KTtcclxuXHRcdHRoaXMuZG9tLiRlbC5vbigncmVmcmVzaC5vd2wnLHRoaXMuZS5yZWZyZXNoKTtcclxuXHRcdHRoaXMuZG9tLiRlbC5vbignYXV0b0hlaWdodC5vd2wnLHRoaXMuZS5hdXRvSGVpZ2h0KTtcclxuXHRcdHRoaXMuZG9tLiRlbC5vbigncGxheS5vd2wnLHRoaXMuZS5wbGF5KTtcclxuXHRcdHRoaXMuZG9tLiRlbC5vbignc3RvcC5vd2wnLHRoaXMuZS5zdG9wKTtcclxuXHRcdHRoaXMuZG9tLiRlbC5vbignc3RvcFZpZGVvLm93bCcsdGhpcy5lLnN0b3ApO1xyXG5cdFx0dGhpcy5kb20uJGVsLm9uKCdpbnNlcnRDb250ZW50Lm93bCcsdGhpcy5lLmluc2VydENvbnRlbnQpO1xyXG5cdFxyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIG9uXHJcblx0ICogQGRlc2MgT24gbWV0aG9kIGZvciBhZGRpbmcgaW50ZXJuYWwgZXZlbnRzXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUub24gPSBmdW5jdGlvbiAoZWxlbWVudCwgZXZlbnQsIGxpc3RlbmVyLCBjYXB0dXJlKSB7XHJcblxyXG5cdFx0aWYgKGVsZW1lbnQuYWRkRXZlbnRMaXN0ZW5lcikge1xyXG5cdFx0XHRlbGVtZW50LmFkZEV2ZW50TGlzdGVuZXIoZXZlbnQsIGxpc3RlbmVyLCBjYXB0dXJlKTtcclxuXHRcdH1cclxuXHRcdGVsc2UgaWYgKGVsZW1lbnQuYXR0YWNoRXZlbnQpIHtcclxuXHRcdFx0ZWxlbWVudC5hdHRhY2hFdmVudCgnb24nICsgZXZlbnQsIGxpc3RlbmVyKTtcclxuXHRcdH1cclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiBvZmZcclxuXHQgKiBAZGVzYyBPZmYgbWV0aG9kIGZvciByZW1vdmluZyBpbnRlcm5hbCBldmVudHNcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS5vZmYgPSBmdW5jdGlvbiAoZWxlbWVudCwgZXZlbnQsIGxpc3RlbmVyLCBjYXB0dXJlKSB7XHJcblx0XHRpZiAoZWxlbWVudC5yZW1vdmVFdmVudExpc3RlbmVyKSB7XHJcblx0XHRcdGVsZW1lbnQucmVtb3ZlRXZlbnRMaXN0ZW5lcihldmVudCwgbGlzdGVuZXIsIGNhcHR1cmUpO1xyXG5cdFx0fVxyXG5cdFx0ZWxzZSBpZiAoZWxlbWVudC5kZXRhY2hFdmVudCkge1xyXG5cdFx0XHRlbGVtZW50LmRldGFjaEV2ZW50KCdvbicgKyBldmVudCwgbGlzdGVuZXIpO1xyXG5cdFx0fVxyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIGZpcmVDYWxsYmFja1xyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqIEBwYXJhbSBldmVudCAtIHN0cmluZyAtIGV2ZW50IG5hbWVcclxuXHQgKiBAcGFyYW0gZGF0YSAtIG9iamVjdCAtIGFkZGl0aW9uYWwgb3B0aW9ucyAtIHRvIGRvXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUuZmlyZUNhbGxiYWNrID0gZnVuY3Rpb24oZXZlbnQsIGRhdGEpe1xyXG5cdFx0aWYoIXRoaXMub3B0aW9ucy5jYWxsYmFja3Mpe3JldHVybjt9XHJcblxyXG5cdFx0aWYodGhpcy5kb20uZWwuZGlzcGF0Y2hFdmVudCl7XHJcblxyXG5cdFx0XHQvLyBkaXNwYXRjaCBldmVudFxyXG5cdFx0XHR2YXIgZXZ0ID0gZG9jdW1lbnQuY3JlYXRlRXZlbnQoJ0N1c3RvbUV2ZW50Jyk7XHJcblxyXG5cdFx0XHQvL2V2dC5pbml0RXZlbnQoZXZlbnQsIGZhbHNlLCB0cnVlICk7XHJcblx0XHRcdGV2dC5pbml0Q3VzdG9tRXZlbnQoZXZlbnQsIHRydWUsIHRydWUsIGRhdGEpO1xyXG5cdFx0XHRyZXR1cm4gdGhpcy5kb20uZWwuZGlzcGF0Y2hFdmVudChldnQpO1xyXG5cclxuXHRcdH0gZWxzZSBpZiAoIXRoaXMuZG9tLmVsLmRpc3BhdGNoRXZlbnQpe1xyXG5cclxuXHRcdFx0Ly9cdFRoZXJlIGlzIG5vIGNsZWFuIHNvbHV0aW9uIGZvciBjdXN0b20gZXZlbnRzIG5hbWUgaW4gPD1JRTggXHJcblx0XHRcdC8vXHRCdXQgaWYgeW91IGtub3cgYmV0dGVyIHdheSwgcGxlYXNlIGxldCBtZSBrbm93IDopIFxyXG5cdFx0XHRyZXR1cm4gdGhpcy5kb20uJGVsLnRyaWdnZXIoZXZlbnQpO1xyXG5cdFx0fVxyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIHdhdGNoVmlzaWJpbGl0eVxyXG5cdCAqIEBkZXNjIGNoZWNrIGlmIGVsIGlzIHZpc2libGUgLSBoYW5keSBpZiBPd2wgaXMgaW5zaWRlIGhpZGRlbiBjb250ZW50ICh0YWJzIGV0Yy4pXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUud2F0Y2hWaXNpYmlsaXR5ID0gZnVuY3Rpb24oKXtcclxuXHJcblx0XHQvLyB0ZXN0IG9uIHplcHRvXHJcblx0XHRpZighaXNFbFZpc2libGUodGhpcy5kb20uZWwpKSB7XHJcblx0XHRcdHRoaXMuZG9tLiRlbC5hZGRDbGFzcygnb3dsLWhpZGRlbicpO1xyXG5cdFx0XHR3aW5kb3cuY2xlYXJJbnRlcnZhbCh0aGlzLmUuX2NoZWNrVmlzaWJpbGUpO1xyXG5cdFx0XHR0aGlzLmUuX2NoZWNrVmlzaWJpbGUgPSB3aW5kb3cuc2V0SW50ZXJ2YWwoY2hlY2tWaXNpYmxlLmJpbmQodGhpcyksNTAwKTtcclxuXHRcdH1cclxuXHJcblx0XHRmdW5jdGlvbiBpc0VsVmlzaWJsZShlbCkge1xyXG5cdFx0ICAgIHJldHVybiBlbC5vZmZzZXRXaWR0aCA+IDAgJiYgZWwub2Zmc2V0SGVpZ2h0ID4gMDtcclxuXHRcdH1cclxuXHJcblx0XHRmdW5jdGlvbiBjaGVja1Zpc2libGUoKXtcclxuXHRcdFx0aWYgKGlzRWxWaXNpYmxlKHRoaXMuZG9tLmVsKSkge1xyXG5cdFx0XHRcdHRoaXMuZG9tLiRlbC5yZW1vdmVDbGFzcygnb3dsLWhpZGRlbicpO1xyXG5cdFx0XHRcdHRoaXMucmVmcmVzaCgpO1xyXG5cdFx0XHRcdHdpbmRvdy5jbGVhckludGVydmFsKHRoaXMuZS5fY2hlY2tWaXNpYmlsZSk7XHJcblx0XHRcdH1cclxuXHRcdH1cclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiBvbkNoYW5nZVxyXG5cdCAqIEBzaW5jZSAyLjAuMFxyXG5cdCAqL1xyXG5cclxuXHRPd2wucHJvdG90eXBlLm9uQ2hhbmdlID0gZnVuY3Rpb24oKXtcclxuXHJcblx0XHRpZighdGhpcy5zdGF0ZS5pc1RvdWNoICYmICF0aGlzLnN0YXRlLmJ5cGFzcyAmJiAhdGhpcy5zdGF0ZS5yZXNwb25zaXZlICl7XHJcblx0XHRcdFxyXG5cdFx0XHRpZiAodGhpcy5vcHRpb25zLm5hdiB8fCB0aGlzLm9wdGlvbnMuZG90cykge1xyXG5cdFx0XHRcdHRoaXMudXBkYXRlQ29udHJvbHMoKTtcclxuXHRcdFx0fVxyXG5cdFx0XHR0aGlzLmF1dG9IZWlnaHQoKTtcclxuXHJcblx0XHRcdHRoaXMuZmlyZUNhbGxiYWNrKCdvbkNoYW5nZVN0YXRlJyk7XHJcblx0XHR9XHJcblxyXG5cdFx0aWYoIXRoaXMuc3RhdGUuaXNUb3VjaCAmJiAhdGhpcy5zdGF0ZS5ieXBhc3Mpe1xyXG5cdFx0XHQvLyBzZXQgU3RhdHVzIHRvIGRvXHJcblx0XHRcdHRoaXMuc3RvcmVJbmZvKCk7XHJcblxyXG5cdFx0XHQvLyBzdG9wVmlkZW8gXHJcblx0XHRcdGlmKHRoaXMuc3RhdGUudmlkZW9QbGF5KXtcclxuXHRcdFx0XHR0aGlzLnN0b3BWaWRlbygpO1xyXG5cdFx0XHR9XHJcblx0XHR9XHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICogc3RvcmVJbmZvXHJcblx0ICogc3RvcmUgYmFzaWMgaW5mb3JtYXRpb24gYWJvdXQgY3VycmVudCBzdGF0ZXNcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS5zdG9yZUluZm8gPSBmdW5jdGlvbigpe1xyXG5cdFx0dmFyIGN1cnJlbnRQb3NpdGlvbiA9IHRoaXMuc3RhdGUubGF6eUNvbnRlbnQgPyB0aGlzLnBvcy5sY0N1cnJlbnRBYnMgfHwgMCA6IHRoaXMucG9zLmN1cnJlbnQ7XHJcblx0XHR2YXIgYWxsSXRlbXMgPSB0aGlzLnN0YXRlLmxhenlDb250ZW50ID8gdGhpcy5kb20uJGNvbnRlbnQubGVuZ3RoLTEgOiB0aGlzLm51bS5vSXRlbXM7XHJcblx0XHRcclxuXHRcdHRoaXMuaW5mbyA9IHtcdFxyXG5cdFx0XHRpdGVtczogXHRcdFx0dGhpcy5vcHRpb25zLml0ZW1zLFxyXG5cdFx0XHRhbGxJdGVtczpcdFx0YWxsSXRlbXMsXHJcblx0XHRcdGN1cnJlbnRQb3NpdGlvbjpjdXJyZW50UG9zaXRpb24sXHJcblx0XHRcdGN1cnJlbnRQYWdlOlx0dGhpcy5wb3MuY3VycmVudFBhZ2UsXHJcblx0XHRcdGFsbFBhZ2VzOlx0XHR0aGlzLm51bS5hbGxQYWdlcyxcclxuXHRcdFx0YXV0b3BsYXk6XHRcdHRoaXMuc3RhdGUuYXV0b3BsYXksXHJcblx0XHRcdHdpbmRvd1dpZHRoOlx0dGhpcy53aWR0aC53aW5kb3csXHJcblx0XHRcdGVsV2lkdGg6XHRcdHRoaXMud2lkdGguZWwsXHJcblx0XHRcdGJyZWFrcG9pbnQ6XHRcdHRoaXMubnVtLmJyZWFrcG9pbnRcclxuXHRcdH07XHJcblxyXG5cdFx0aWYgKHR5cGVvZiB0aGlzLm9wdGlvbnMuaW5mbyA9PT0gJ2Z1bmN0aW9uJykge1xyXG5cdFx0XHR0aGlzLm9wdGlvbnMuaW5mby5hcHBseSh0aGlzLFt0aGlzLmluZm8sdGhpcy5kb20uZWxdKTtcclxuXHRcdH1cclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiBhdXRvSGVpZ2h0XHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUuYXV0b0hlaWdodCA9IGZ1bmN0aW9uKGNhbGxiYWNrKXtcclxuXHRcdCBpZih0aGlzLm9wdGlvbnMuYXV0b0hlaWdodCAhPT0gdHJ1ZSAmJiBjYWxsYmFjayAhPT0gdHJ1ZSl7XHJcblx0XHRcdHJldHVybiBmYWxzZTtcclxuXHRcdH1cclxuXHRcdGlmKCF0aGlzLmRvbS4kb1N0YWdlLmhhc0NsYXNzKHRoaXMub3B0aW9ucy5hdXRvSGVpZ2h0Q2xhc3MpKXtcclxuXHRcdFx0dGhpcy5kb20uJG9TdGFnZS5hZGRDbGFzcyh0aGlzLm9wdGlvbnMuYXV0b0hlaWdodENsYXNzKTtcclxuXHRcdH1cclxuXHJcblx0XHR2YXIgbG9hZGVkID0gdGhpcy5kb20uJGl0ZW1zLmVxKHRoaXMucG9zLmN1cnJlbnRBYnMpO1xyXG5cdFx0dmFyIHN0YWdlID0gdGhpcy5kb20uJG9TdGFnZTtcclxuXHRcdHZhciBpdGVyYXRpb25zID0gMDtcclxuXHJcblx0XHR2YXIgaXNMb2FkZWQgPSB3aW5kb3cuc2V0SW50ZXJ2YWwoZnVuY3Rpb24oKSB7XHJcblx0XHRcdGl0ZXJhdGlvbnMgKz0gMTtcclxuXHRcdFx0aWYobG9hZGVkLmRhdGEoJ293bC1pdGVtJykubG9hZGVkKXtcclxuXHRcdFx0XHRzdGFnZS5oZWlnaHQobG9hZGVkLmhlaWdodCgpICsgJ3B4Jyk7XHJcblx0XHRcdFx0Y2xlYXJJbnRlcnZhbChpc0xvYWRlZCk7XHJcblx0XHRcdH0gZWxzZSBpZihpdGVyYXRpb25zID09PSA1MDApe1xyXG5cdFx0XHRcdGNsZWFySW50ZXJ2YWwoaXNMb2FkZWQpO1xyXG5cdFx0XHR9XHJcblx0XHR9LCAxMDApO1xyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIHByZWxvYWRBdXRvV2lkdGhJbWFnZXNcclxuXHQgKiBAZGVzYyBzdGlsbCB0byB0ZXN0XHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUucHJlbG9hZEF1dG9XaWR0aEltYWdlcyA9IGZ1bmN0aW9uKGltZ3Mpe1xyXG5cdFx0dmFyIGxvYWRlZCA9IDA7XHJcblx0XHR2YXIgdGhhdCA9IHRoaXM7XHJcblx0XHRpbWdzLmVhY2goZnVuY3Rpb24oaSxlbCl7XHJcblx0XHRcdHZhciAkZWwgPSAkKGVsKTtcclxuXHRcdFx0dmFyIGltZyA9IG5ldyBJbWFnZSgpO1xyXG5cclxuXHRcdFx0aW1nLm9ubG9hZCA9IGZ1bmN0aW9uKCl7XHJcblx0XHRcdFx0bG9hZGVkKys7XHJcblx0XHRcdFx0JGVsLmF0dHIoJ3NyYycsaW1nLnNyYyk7XHJcblx0XHRcdFx0JGVsLmNzcygnb3BhY2l0eScsMSk7XHJcblx0XHRcdFx0aWYobG9hZGVkID49IGltZ3MubGVuZ3RoKXtcclxuXHRcdFx0XHRcdHRoYXQuc3RhdGUuaW1hZ2VzTG9hZGVkID0gdHJ1ZTtcclxuXHRcdFx0XHRcdHRoYXQuaW5pdCgpO1xyXG5cdFx0XHRcdH1cclxuXHRcdFx0fVxyXG5cclxuXHRcdFx0aW1nLnNyYyA9ICRlbC5hdHRyKCdzcmMnKSB8fCAgJGVsLmF0dHIoJ2RhdGEtc3JjJykgfHwgJGVsLmF0dHIoJ2RhdGEtc3JjLXJldGluYScpOztcclxuXHRcdH0pXHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICogbGF6eUxvYWRcclxuXHQgKiBAZGVzYyBsYXp5TG9hZCBpbWFnZXNcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS5sYXp5TG9hZCA9IGZ1bmN0aW9uKCl7XHJcblx0XHR2YXIgYXR0ciA9IGlzUmV0aW5hKCkgPyAnZGF0YS1zcmMtcmV0aW5hJyA6ICdkYXRhLXNyYyc7XHJcblx0XHR2YXIgc3JjLCBpbWcsaTtcclxuXHJcblx0XHRmb3IoaSA9IDA7IGkgPCB0aGlzLm51bS5pdGVtczsgaSsrKXtcclxuXHRcdFx0dmFyICRpdGVtID0gdGhpcy5kb20uJGl0ZW1zLmVxKGkpO1xyXG5cclxuXHRcdFx0aWYoICRpdGVtLmRhdGEoJ293bC1pdGVtJykuY3VycmVudCA9PT0gdHJ1ZSAmJiAkaXRlbS5kYXRhKCdvd2wtaXRlbScpLmxvYWRlZCA9PT0gZmFsc2Upe1xyXG5cdFx0XHRcdGltZyA9ICRpdGVtLmZpbmQoJy5vd2wtbGF6eScpO1xyXG5cdFx0XHRcdHNyYyA9IGltZy5hdHRyKGF0dHIpO1xyXG5cdFx0XHRcdHNyYyA9IHNyYyB8fCBpbWcuYXR0cignZGF0YS1zcmMnKTtcclxuXHRcdFx0XHRpZihzcmMpe1xyXG5cdFx0XHRcdFx0aW1nLmNzcygnb3BhY2l0eScsJzAnKTtcclxuXHRcdFx0XHRcdHRoaXMucHJlbG9hZChpbWcsJGl0ZW0pO1xyXG5cdFx0XHRcdH1cclxuXHRcdFx0fVxyXG5cdFx0fVxyXG5cdH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIHByZWxvYWRcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKi9cclxuXHJcblx0IE93bC5wcm90b3R5cGUucHJlbG9hZCA9IGZ1bmN0aW9uKGltYWdlcywkaXRlbSl7XHJcblx0XHR2YXIgdGhhdCA9IHRoaXM7IC8vIGZpeCB0aGlzIGxhdGVyXHJcblxyXG5cdFx0aW1hZ2VzLmVhY2goZnVuY3Rpb24oaSxlbCl7XHJcblx0XHRcdHZhciAkZWwgPSAkKGVsKTtcclxuXHRcdFx0dmFyIGltZyA9IG5ldyBJbWFnZSgpO1xyXG5cclxuXHRcdFx0aW1nLm9ubG9hZCA9IGZ1bmN0aW9uKCl7XHJcblxyXG5cdFx0XHRcdCRpdGVtLmRhdGEoJ293bC1pdGVtJykubG9hZGVkID0gdHJ1ZTtcclxuXHRcdFx0XHRpZigkZWwuaXMoJ2ltZycpKXtcclxuXHRcdFx0XHRcdCRlbC5hdHRyKCdzcmMnLGltZy5zcmMpO1xyXG5cdFx0XHRcdH1lbHNle1xyXG5cdFx0XHRcdFx0JGVsLmNzcygnYmFja2dyb3VuZC1pbWFnZScsJ3VybCgnICsgaW1nLnNyYyArICcpJyk7XHJcblx0XHRcdFx0fVxyXG5cdFx0XHRcdFxyXG5cdFx0XHRcdCRlbC5jc3MoJ29wYWNpdHknLDEpO1xyXG5cdFx0XHRcdHRoYXQuZmlyZUNhbGxiYWNrKCdvbkxhenlMb2FkZWQnKTtcclxuXHRcdFx0fTtcclxuXHRcdFx0aW1nLnNyYyA9ICRlbC5hdHRyKCdkYXRhLXNyYycpIHx8ICRlbC5hdHRyKCdkYXRhLXNyYy1yZXRpbmEnKTtcclxuXHRcdH0pO1xyXG5cdCB9O1xyXG5cclxuXHQvKipcclxuXHQgKiBhbmltYXRlXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdCBPd2wucHJvdG90eXBlLmFuaW1hdGUgPSBmdW5jdGlvbigpe1xyXG5cclxuXHRcdHZhciBwcmV2SXRlbSA9IHRoaXMuZG9tLiRpdGVtcy5lcSh0aGlzLnBvcy5wcmV2KSxcclxuXHRcdFx0cHJldlBvcyA9IE1hdGguYWJzKHByZXZJdGVtLmRhdGEoJ293bC1pdGVtJykud2lkdGgpICogdGhpcy5wb3MucHJldixcclxuXHRcdFx0Y3VycmVudEl0ZW0gPSB0aGlzLmRvbS4kaXRlbXMuZXEodGhpcy5wb3MuY3VycmVudEFicyksXHJcblx0XHRcdGN1cnJlbnRQb3MgPSBNYXRoLmFicyhjdXJyZW50SXRlbS5kYXRhKCdvd2wtaXRlbScpLndpZHRoKSAqIHRoaXMucG9zLmN1cnJlbnRBYnM7XHJcblxyXG5cdFx0aWYodGhpcy5wb3MuY3VycmVudEFicyA9PT0gdGhpcy5wb3MucHJldil7XHJcblx0XHRcdHJldHVybiBmYWxzZTtcclxuXHRcdH1cclxuXHJcblx0XHR2YXIgcG9zID0gY3VycmVudFBvcyAtIHByZXZQb3M7XHJcblx0XHR2YXIgdEluID0gdGhpcy5vcHRpb25zLmFuaW1hdGVJbjtcclxuXHRcdHZhciB0T3V0ID0gdGhpcy5vcHRpb25zLmFuaW1hdGVPdXQ7XHJcblx0XHR2YXIgdGhhdCA9IHRoaXM7XHJcblxyXG5cdFx0cmVtb3ZlU3R5bGVzID0gZnVuY3Rpb24oKXtcclxuXHRcdFx0JCh0aGlzKS5jc3Moe1xyXG4gICAgICAgICAgICAgICAgXCJsZWZ0XCIgOiBcIlwiXHJcbiAgICAgICAgICAgIH0pXHJcbiAgICAgICAgICAgIC5yZW1vdmVDbGFzcygnYW5pbWF0ZWQgb3dsLWFuaW1hdGVkLW91dCBvd2wtYW5pbWF0ZWQtaW4nKVxyXG4gICAgICAgICAgICAucmVtb3ZlQ2xhc3ModEluKVxyXG4gICAgICAgICAgICAucmVtb3ZlQ2xhc3ModE91dCk7XHJcblxyXG4gICAgICAgICAgICB0aGF0LnRyYW5zaXRpb25FbmQoKTtcclxuICAgICAgICB9O1xyXG5cclxuXHRcdGlmKHRPdXQpe1xyXG5cdFx0XHRwcmV2SXRlbVxyXG5cdFx0XHQuY3NzKHtcclxuXHRcdFx0XHRcImxlZnRcIiA6IHBvcyArIFwicHhcIlxyXG5cdFx0XHR9KVxyXG5cdFx0XHQuYWRkQ2xhc3MoJ2FuaW1hdGVkIG93bC1hbmltYXRlZC1vdXQgJyt0T3V0KVxyXG5cdFx0XHQub25lKCd3ZWJraXRBbmltYXRpb25FbmQgbW96QW5pbWF0aW9uRW5kIE1TQW5pbWF0aW9uRW5kIG9hbmltYXRpb25lbmQgYW5pbWF0aW9uZW5kJywgcmVtb3ZlU3R5bGVzKTtcclxuXHRcdH1cclxuXHJcblx0XHRpZih0SW4pe1xyXG5cdFx0XHRjdXJyZW50SXRlbVxyXG5cdFx0XHQuYWRkQ2xhc3MoJ2FuaW1hdGVkIG93bC1hbmltYXRlZC1pbiAnK3RJbilcclxuXHRcdFx0Lm9uZSgnd2Via2l0QW5pbWF0aW9uRW5kIG1vekFuaW1hdGlvbkVuZCBNU0FuaW1hdGlvbkVuZCBvYW5pbWF0aW9uZW5kIGFuaW1hdGlvbmVuZCcsIHJlbW92ZVN0eWxlcyk7XHJcblx0XHR9XHJcblx0IH07XHJcblxyXG5cdC8qKlxyXG5cdCAqIGRlc3Ryb3lcclxuXHQgKiBAZGVzYyBSZW1vdmUgT3dsIHN0cnVjdHVyZSBhbmQgZXZlbnRzIDooXHJcblx0ICogQHNpbmNlIDIuMC4wXHJcblx0ICovXHJcblxyXG5cdE93bC5wcm90b3R5cGUuZGVzdHJveSA9IGZ1bmN0aW9uKCl7XHJcblxyXG5cdFx0d2luZG93LmNsZWFySW50ZXJ2YWwodGhpcy5lLl9hdXRvcGxheSk7XHJcblxyXG5cdFx0aWYodGhpcy5kb20uJGVsLmhhc0NsYXNzKHRoaXMub3B0aW9ucy50aGVtZUNsYXNzKSl7XHJcblx0XHRcdHRoaXMuZG9tLiRlbC5yZW1vdmVDbGFzcyh0aGlzLm9wdGlvbnMudGhlbWVDbGFzcyk7XHJcblx0XHR9XHJcblxyXG5cdFx0aWYodGhpcy5vcHRpb25zLnJlc3BvbnNpdmUgIT09IGZhbHNlKXtcclxuXHRcdFx0dGhpcy5vZmYod2luZG93LCAncmVzaXplJywgdGhpcy5lLl9yZXNpemVyKTtcclxuXHRcdH1cclxuXHJcblx0XHRpZih0aGlzLnRyYW5zaXRpb25FbmRWZW5kb3Ipe1xyXG5cdFx0XHR0aGlzLm9mZih0aGlzLmRvbS5zdGFnZSwgdGhpcy50cmFuc2l0aW9uRW5kVmVuZG9yLCB0aGlzLmUuX3RyYW5zaXRpb25FbmQpO1xyXG5cdFx0fVxyXG5cclxuXHRcdGlmKHRoaXMub3B0aW9ucy5tb3VzZURyYWcgfHwgdGhpcy5vcHRpb25zLnRvdWNoRHJhZyl7XHJcblx0XHRcdHRoaXMub2ZmKHRoaXMuZG9tLnN0YWdlLCB0aGlzLmRyYWdUeXBlWzBdLCB0aGlzLmUuX29uRHJhZ1N0YXJ0KTtcclxuXHRcdFx0aWYodGhpcy5vcHRpb25zLm1vdXNlRHJhZyl7XHJcblx0XHRcdFx0dGhpcy5vZmYoZG9jdW1lbnQsIHRoaXMuZHJhZ1R5cGVbM10sIHRoaXMuZS5fb25EcmFnU3RhcnQpO1xyXG5cdFx0XHR9XHJcblx0XHRcdGlmKHRoaXMub3B0aW9ucy5tb3VzZURyYWcpe1xyXG5cdFx0XHRcdHRoaXMuZG9tLiRzdGFnZS5vZmYoJ2RyYWdzdGFydCcsIGZ1bmN0aW9uKCkge3JldHVybiBmYWxzZTt9KTtcclxuXHRcdFx0XHR0aGlzLmRvbS5zdGFnZS5vbnNlbGVjdHN0YXJ0ID0gZnVuY3Rpb24oKXt9O1xyXG5cdFx0XHR9XHJcblx0XHR9XHJcblxyXG5cdFx0aWYodGhpcy5vcHRpb25zLlVSTGhhc2hMaXN0ZW5lcil7XHJcblx0XHRcdHRoaXMub2ZmKHdpbmRvdywgJ2hhc2hjaGFuZ2UnLCB0aGlzLmUuX2dvVG9IYXNoKTtcclxuXHRcdH1cclxuXHJcblx0XHR0aGlzLmRvbS4kZWwub2ZmKCduZXh0Lm93bCcsdGhpcy5lLm5leHQpO1xyXG5cdFx0dGhpcy5kb20uJGVsLm9mZigncHJldi5vd2wnLHRoaXMuZS5wcmV2KTtcclxuXHRcdHRoaXMuZG9tLiRlbC5vZmYoJ2dvVG8ub3dsJyx0aGlzLmUuZ29Ubyk7XHJcblx0XHR0aGlzLmRvbS4kZWwub2ZmKCdqdW1wVG8ub3dsJyx0aGlzLmUuanVtcFRvKTtcclxuXHRcdHRoaXMuZG9tLiRlbC5vZmYoJ2FkZEl0ZW0ub3dsJyx0aGlzLmUuYWRkSXRlbSk7XHJcblx0XHR0aGlzLmRvbS4kZWwub2ZmKCdyZW1vdmVJdGVtLm93bCcsdGhpcy5lLnJlbW92ZUl0ZW0pO1xyXG5cdFx0dGhpcy5kb20uJGVsLm9mZigncmVmcmVzaC5vd2wnLHRoaXMuZS5yZWZyZXNoKTtcclxuXHRcdHRoaXMuZG9tLiRlbC5vZmYoJ2F1dG9IZWlnaHQub3dsJyx0aGlzLmUuYXV0b0hlaWdodCk7XHJcblx0XHR0aGlzLmRvbS4kZWwub2ZmKCdwbGF5Lm93bCcsdGhpcy5lLnBsYXkpO1xyXG5cdFx0dGhpcy5kb20uJGVsLm9mZignc3RvcC5vd2wnLHRoaXMuZS5zdG9wKTtcclxuXHRcdHRoaXMuZG9tLiRlbC5vZmYoJ3N0b3BWaWRlby5vd2wnLHRoaXMuZS5zdG9wKTtcclxuXHRcdHRoaXMuZG9tLiRzdGFnZS5vZmYoJ2NsaWNrJyx0aGlzLmUuX3BsYXlWaWRlbyk7XHJcblxyXG5cdFx0aWYodGhpcy5kb20uJGNjICE9PSBudWxsKXtcclxuXHRcdFx0dGhpcy5kb20uJGNjLnJlbW92ZSgpO1xyXG5cdFx0fVxyXG5cdFx0aWYodGhpcy5kb20uJGNJdGVtcyAhPT0gbnVsbCl7XHJcblx0XHRcdHRoaXMuZG9tLiRjSXRlbXMucmVtb3ZlKCk7XHJcblx0XHR9XHJcblx0XHR0aGlzLmUgPSBudWxsO1xyXG5cdFx0dGhpcy5kb20uJGVsLmRhdGEoJ293bENhcm91c2VsJyxudWxsKTtcclxuXHRcdGRlbGV0ZSB0aGlzLmRvbS5lbC5vd2xDYXJvdXNlbDtcclxuXHJcblx0XHR0aGlzLmRvbS4kc3RhZ2UudW53cmFwKCk7XHJcblx0XHR0aGlzLmRvbS4kaXRlbXMudW53cmFwKCk7XHJcblx0XHR0aGlzLmRvbS4kaXRlbXMuY29udGVudHMoKS51bndyYXAoKTtcclxuXHRcdHRoaXMuZG9tID0gbnVsbDtcclxuXHR9O1xyXG5cclxuXHQvKipcclxuXHQgKiBPcGVydGF0b3JzIFxyXG5cdCAqIEBkZXNjIFVzZWQgdG8gY2FsY3VsYXRlIFJUTFxyXG5cdCAqIEBwYXJhbSBbYV0gLSBOdW1iZXIgLSBsZWZ0IHNpZGVcclxuXHQgKiBAcGFyYW0gW29dIC0gU3RyaW5nIC0gb3BlcmF0b3IgXHJcblx0ICogQHBhcmFtIFtiXSAtIE51bWJlciAtIHJpZ2h0IHNpZGVcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS5vcCA9IGZ1bmN0aW9uKGEsbyxiKXtcclxuXHRcdHZhciBydGwgPSB0aGlzLm9wdGlvbnMucnRsO1xyXG5cdFx0c3dpdGNoKG8pIHtcclxuXHRcdFx0Y2FzZSAnPCc6XHJcblx0XHRcdFx0cmV0dXJuIHJ0bCA/IGEgPiBiIDogYSA8IGI7XHJcblx0XHRcdGNhc2UgJz4nOlxyXG5cdFx0XHRcdHJldHVybiBydGwgPyBhIDwgYiA6IGEgPiBiO1xyXG5cdFx0XHRjYXNlICc+PSc6XHJcblx0XHRcdFx0cmV0dXJuIHJ0bCA/IGEgPD0gYiA6IGEgPj0gYjtcclxuXHRcdFx0Y2FzZSAnPD0nOlxyXG5cdFx0XHRcdHJldHVybiBydGwgPyBhID49IGIgOiBhIDw9IGI7XHJcblx0XHRcdGRlZmF1bHQ6XHJcblx0XHRcdFx0YnJlYWs7XHJcblx0XHR9XHJcblx0fTtcclxuXHJcblx0LyoqXHJcblx0ICogT3BlcnRhdG9ycyBcclxuXHQgKiBAZGVzYyBVc2VkIHRvIGNhbGN1bGF0ZSBSVExcclxuXHQgKiBAc2luY2UgMi4wLjBcclxuXHQgKi9cclxuXHJcblx0T3dsLnByb3RvdHlwZS5icm93c2VyU3VwcG9ydCA9IGZ1bmN0aW9uKCl7XHJcblx0XHR0aGlzLnN1cHBvcnQzZCA9IGlzUGVyc3BlY3RpdmUoKTtcclxuXHJcblx0XHRpZih0aGlzLnN1cHBvcnQzZCl7XHJcblx0XHRcdHRoaXMudHJhbnNmb3JtVmVuZG9yID0gaXNUcmFuc2Zvcm0oKTtcclxuXHJcblx0XHRcdC8vIHRha2UgdHJhbnNpdGlvbmVuZCBldmVudCBuYW1lIGJ5IGRldGVjdGluZyB0cmFuc2l0aW9uXHJcblx0XHRcdHZhciBlbmRWZW5kb3JzID0gWyd0cmFuc2l0aW9uZW5kJywnd2Via2l0VHJhbnNpdGlvbkVuZCcsJ3RyYW5zaXRpb25lbmQnLCdvVHJhbnNpdGlvbkVuZCddO1xyXG5cdFx0XHR0aGlzLnRyYW5zaXRpb25FbmRWZW5kb3IgPSBlbmRWZW5kb3JzW2lzVHJhbnNpdGlvbigpXTtcclxuXHJcblx0XHRcdC8vIHRha2UgdmVuZG9yIG5hbWUgZnJvbSB0cmFuc2Zvcm0gbmFtZVxyXG5cdFx0XHR0aGlzLnZlbmRvck5hbWUgPSB0aGlzLnRyYW5zZm9ybVZlbmRvci5yZXBsYWNlKC9UcmFuc2Zvcm0vaSwnJyk7XHJcblx0XHRcdHRoaXMudmVuZG9yTmFtZSA9IHRoaXMudmVuZG9yTmFtZSAhPT0gJycgPyAnLScrdGhpcy52ZW5kb3JOYW1lLnRvTG93ZXJDYXNlKCkrJy0nIDogJyc7XHJcblx0XHR9XHJcblxyXG5cdFx0dGhpcy5zdGF0ZS5vcmllbnRhdGlvbiA9IHdpbmRvdy5vcmllbnRhdGlvbjtcclxuXHR9O1xyXG5cclxuXHQvLyBQaXZhdGUgbWV0aG9kcyBcclxuXHJcblx0Ly8gQ1NTIGRldGVjdGlvbjtcclxuXHRmdW5jdGlvbiBpc1N0eWxlU3VwcG9ydGVkKGFycmF5KXtcclxuXHRcdHZhciBwLHMsZmFrZSA9IGRvY3VtZW50LmNyZWF0ZUVsZW1lbnQoJ2RpdicpLGxpc3QgPSBhcnJheTtcclxuXHRcdGZvcihwIGluIGxpc3Qpe1xyXG5cdFx0XHRzID0gbGlzdFtwXTsgXHJcblx0XHRcdGlmKHR5cGVvZiBmYWtlLnN0eWxlW3NdICE9PSAndW5kZWZpbmVkJyl7XHJcblx0XHRcdFx0ZmFrZSA9IG51bGw7XHJcblx0XHRcdFx0cmV0dXJuIFtzLHBdO1xyXG5cdFx0XHR9XHJcblx0XHR9XHJcblx0XHRyZXR1cm4gW2ZhbHNlXTtcclxuXHR9XHJcblxyXG5cdGZ1bmN0aW9uIGlzVHJhbnNpdGlvbigpe1xyXG5cdFx0cmV0dXJuIGlzU3R5bGVTdXBwb3J0ZWQoWyd0cmFuc2l0aW9uJywnV2Via2l0VHJhbnNpdGlvbicsJ01velRyYW5zaXRpb24nLCdPVHJhbnNpdGlvbiddKVsxXTtcclxuXHR9XHJcbiBcclxuXHRmdW5jdGlvbiBpc1RyYW5zZm9ybSgpIHtcclxuXHRcdHJldHVybiBpc1N0eWxlU3VwcG9ydGVkKFsndHJhbnNmb3JtJywnV2Via2l0VHJhbnNmb3JtJywnTW96VHJhbnNmb3JtJywnT1RyYW5zZm9ybScsJ21zVHJhbnNmb3JtJ10pWzBdO1xyXG5cdH1cclxuXHJcblx0ZnVuY3Rpb24gaXNQZXJzcGVjdGl2ZSgpe1xyXG5cdFx0cmV0dXJuIGlzU3R5bGVTdXBwb3J0ZWQoWydwZXJzcGVjdGl2ZScsJ3dlYmtpdFBlcnNwZWN0aXZlJywnTW96UGVyc3BlY3RpdmUnLCdPUGVyc3BlY3RpdmUnLCdNc1BlcnNwZWN0aXZlJ10pWzBdO1xyXG5cdH1cclxuXHJcblx0ZnVuY3Rpb24gaXNUb3VjaFN1cHBvcnQoKXtcclxuXHRcdHJldHVybiAnb250b3VjaHN0YXJ0JyBpbiB3aW5kb3cgfHwgISEobmF2aWdhdG9yLm1zTWF4VG91Y2hQb2ludHMpO1xyXG5cdH1cclxuXHJcblx0ZnVuY3Rpb24gaXNUb3VjaFN1cHBvcnRJRSgpe1xyXG5cdFx0cmV0dXJuIHdpbmRvdy5uYXZpZ2F0b3IubXNQb2ludGVyRW5hYmxlZDtcclxuXHR9XHJcblxyXG5cdGZ1bmN0aW9uIGlzUmV0aW5hKCl7XHJcblx0XHRyZXR1cm4gd2luZG93LmRldmljZVBpeGVsUmF0aW8gPiAxO1xyXG5cdH1cclxuXHJcblx0JC5mbi5vd2xDYXJvdXNlbCA9IGZ1bmN0aW9uICggb3B0aW9ucyApIHtcclxuXHRcdHJldHVybiB0aGlzLmVhY2goZnVuY3Rpb24gKCkge1xyXG5cdFx0XHRpZiAoISQodGhpcykuZGF0YSgnb3dsQ2Fyb3VzZWwnKSkge1xyXG5cdFx0XHRcdCQodGhpcykuZGF0YSggJ293bENhcm91c2VsJyxcclxuXHRcdFx0XHRuZXcgT3dsKCB0aGlzLCBvcHRpb25zICkpO1xyXG5cdFx0XHR9XHJcblx0XHR9KTtcclxuXHJcblx0fTtcclxuXHJcbn0pKCB3aW5kb3cuWmVwdG8gfHwgd2luZG93LmpRdWVyeSwgd2luZG93LCAgZG9jdW1lbnQgKTtcclxuXHJcbi8vaHR0cHM6Ly9kZXZlbG9wZXIubW96aWxsYS5vcmcvZW4tVVMvZG9jcy9XZWIvSmF2YVNjcmlwdC9SZWZlcmVuY2UvR2xvYmFsX09iamVjdHMvRnVuY3Rpb24vYmluZFxyXG4vL1RoZSBiaW5kKCkgbWV0aG9kIGNyZWF0ZXMgYSBuZXcgZnVuY3Rpb24gdGhhdCwgd2hlbiBjYWxsZWQsIGhhcyBpdHMgdGhpcyBrZXl3b3JkIHNldCB0byB0aGUgcHJvdmlkZWQgdmFsdWUsIHdpdGggYSBnaXZlbiBzZXF1ZW5jZSBvZiBhcmd1bWVudHMgcHJlY2VkaW5nIGFueSBwcm92aWRlZCB3aGVuIHRoZSBuZXcgZnVuY3Rpb24gaXMgY2FsbGVkLlxyXG5cclxuaWYgKCFGdW5jdGlvbi5wcm90b3R5cGUuYmluZCkge1xyXG4gIEZ1bmN0aW9uLnByb3RvdHlwZS5iaW5kID0gZnVuY3Rpb24gKG9UaGlzKSB7XHJcblx0aWYgKHR5cGVvZiB0aGlzICE9PSAnZnVuY3Rpb24nKSB7XHJcblx0XHQvLyBjbG9zZXN0IHRoaW5nIHBvc3NpYmxlIHRvIHRoZSBFQ01BU2NyaXB0IDUgaW50ZXJuYWwgSXNDYWxsYWJsZSBmdW5jdGlvblxyXG5cdFx0dGhyb3cgbmV3IFR5cGVFcnJvcignRnVuY3Rpb24ucHJvdG90eXBlLmJpbmQgLSB3aGF0IGlzIHRyeWluZyB0byBiZSBib3VuZCBpcyBub3QgY2FsbGFibGUnKTtcclxuXHR9XHJcblxyXG5cdHZhciBhQXJncyA9IEFycmF5LnByb3RvdHlwZS5zbGljZS5jYWxsKGFyZ3VtZW50cywgMSksIFxyXG5cdFx0ZlRvQmluZCA9IHRoaXMsIFxyXG5cdFx0Zk5PUCA9IGZ1bmN0aW9uICgpIHt9LFxyXG5cdFx0ZkJvdW5kID0gZnVuY3Rpb24gKCkge1xyXG5cdFx0XHRyZXR1cm4gZlRvQmluZC5hcHBseSh0aGlzIGluc3RhbmNlb2YgZk5PUCAmJiBvVGhpcyA/IHRoaXMgOiBvVGhpcywgYUFyZ3MuY29uY2F0KEFycmF5LnByb3RvdHlwZS5zbGljZS5jYWxsKGFyZ3VtZW50cykpKTtcclxuXHRcdH07XHJcblx0Zk5PUC5wcm90b3R5cGUgPSB0aGlzLnByb3RvdHlwZTtcclxuXHRmQm91bmQucHJvdG90eXBlID0gbmV3IGZOT1AoKTtcclxuXHRyZXR1cm4gZkJvdW5kO1xyXG4gIH07XHJcbn0iXX0=