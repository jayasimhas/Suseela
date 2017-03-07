(function e(t,n,r){function s(o,u){if(!n[o]){if(!t[o]){var a=typeof require=="function"&&require;if(!u&&a)return a(o,!0);if(i)return i(o,!0);var f=new Error("Cannot find module '"+o+"'");throw f.code="MODULE_NOT_FOUND",f}var l=n[o]={exports:{}};t[o][0].call(l.exports,function(e){var n=t[o][1][e];return s(n?n:e)},l,l.exports,e,t,n,r)}return n[o].exports}var i=typeof require=="function"&&require;for(var o=0;o<r.length;o++)s(r[o]);return s})({1:[function(require,module,exports){
'use strict';

var DragDropTouch,
    checkTouchType = true;
(function (DragDropTouch_1) {
    'use strict';
    /**
     * Object used to hold the data that is being dragged during drag and drop operations.
     *
     * It may hold one or more data items of different types. For more information about
     * drag and drop operations and data transfer objects, see
     * <a href="https://developer.mozilla.org/en-US/docs/Web/API/DataTransfer">HTML Drag and Drop API</a>.
     *
     * This object is created automatically by the @see:DragDropTouch singleton and is
     * accessible through the @see:dataTransfer property of all drag events.
     */
    var DataTransfer = (function () {
        function DataTransfer() {
            this._dropEffect = 'move';
            this._effectAllowed = 'all';
            this._data = {};
        }
        Object.defineProperty(DataTransfer.prototype, "dropEffect", {
            /**
             * Gets or sets the type of drag-and-drop operation currently selected.
             * The value must be 'none',  'copy',  'link', or 'move'.
             */
            get: function get() {
                return this._dropEffect;
            },
            set: function set(value) {
                this._dropEffect = value;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(DataTransfer.prototype, "effectAllowed", {
            /**
             * Gets or sets the types of operations that are possible.
             * Must be one of 'none', 'copy', 'copyLink', 'copyMove', 'link',
             * 'linkMove', 'move', 'all' or 'uninitialized'.
             */
            get: function get() {
                return this._effectAllowed;
            },
            set: function set(value) {
                this._effectAllowed = value;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(DataTransfer.prototype, "types", {
            /**
             * Gets an array of strings giving the formats that were set in the @see:dragstart event.
             */
            get: function get() {
                return Object.keys(this._data);
            },
            enumerable: true,
            configurable: true
        });
        /**
         * Removes the data associated with a given type.
         *
         * The type argument is optional. If the type is empty or not specified, the data
         * associated with all types is removed. If data for the specified type does not exist,
         * or the data transfer contains no data, this method will have no effect.
         *
         * @param type Type of data to remove.
         */
        DataTransfer.prototype.clearData = function (type) {
            if (type != null) {
                delete this._data[type];
            } else {
                this._data = null;
            }
        };
        /**
         * Retrieves the data for a given type, or an empty string if data for that type does
         * not exist or the data transfer contains no data.
         *
         * @param type Type of data to retrieve.
         */
        DataTransfer.prototype.getData = function (type) {
            return this._data[type] || '';
        };
        /**
         * Set the data for a given type.
         *
         * For a list of recommended drag types, please see
         * https://developer.mozilla.org/en-US/docs/Web/Guide/HTML/Recommended_Drag_Types.
         *
         * @param type Type of data to add.
         * @param value Data to add.
         */
        DataTransfer.prototype.setData = function (type, value) {
            this._data[type] = value;
        };
        /**
         * Set the image to be used for dragging if a custom one is desired.
         *
         * @param img An image element to use as the drag feedback image.
         * @param offsetX The horizontal offset within the image.
         * @param offsetY The vertical offset within the image.
         */
        DataTransfer.prototype.setDragImage = function (img, offsetX, offsetY) {
            var ddt = DragDropTouch._instance;
            ddt._imgCustom = img;
            ddt._imgOffset = { x: offsetX, y: offsetY };
        };
        return DataTransfer;
    })();
    DragDropTouch_1.DataTransfer = DataTransfer;
    /**
     * Defines a class that adds support for touch-based HTML5 drag/drop operations.
     *
     * The @see:DragDropTouch class listens to touch events and raises the
     * appropriate HTML5 drag/drop events as if the events had been caused
     * by mouse actions.
     *
     * The purpose of this class is to enable using existing, standard HTML5
     * drag/drop code on mobile devices running IOS or Android.
     *
     * To use, include the DragDropTouch.js file on the page. The class will
     * automatically start monitoring touch events and will raise the HTML5
     * drag drop events (dragstart, dragenter, dragleave, drop, dragend) which
     * should be handled by the application.
     *
     * For details and examples on HTML drag and drop, see
     * https://developer.mozilla.org/en-US/docs/Web/Guide/HTML/Drag_operations.
     */
    var DragDropTouch = (function () {
        /**
         * Initializes the single instance of the @see:DragDropTouch class.
         */
        function DragDropTouch() {
            this._lastClick = 0;
            // enforce singleton pattern
            if (DragDropTouch._instance) {
                throw 'DragDropTouch instance already created.';
            }
            // listen to touch events
            if ('ontouchstart' in document) {
                var d = document,
                    ts = this._touchstart.bind(this),
                    tm = this._touchmove.bind(this),
                    te = this._touchend.bind(this);
                d.addEventListener('touchstart', ts);
                d.addEventListener('touchmove', tm);
                d.addEventListener('touchend', te);
                d.addEventListener('touchcancel', te);
            }
        }
        /**
         * Gets a reference to the @see:DragDropTouch singleton.
         */
        DragDropTouch.getInstance = function () {
            return DragDropTouch._instance;
        };
        // ** event handlers
        DragDropTouch.prototype._touchstart = function (e) {
            var _this = this;
            if (this._shouldHandle(e)) {
                // raise double-click and prevent zooming
                if (Date.now() - this._lastClick < DragDropTouch._DBLCLICK) {
                    if (this._dispatchEvent(e, 'dblclick', e.target)) {
                        //e.preventDefault();
                        this._reset();
                        return;
                    }
                }
                // clear all variables
                this._reset();
                // get nearest draggable element
                var src = this._closestDraggable(e.target);
                if (src) {
                    // give caller a chance to handle the hover/move events
                    if (!this._dispatchEvent(e, 'mousemove', e.target) && !this._dispatchEvent(e, 'mousedown', e.target)) {
                        // get ready to start dragging
                        this._dragSource = src;
                        this._ptDown = this._getPoint(e);
                        this._lastTouch = e;
                        if (e.target.className == 'pull-left' || e.target.className == 'wd-15' || e.target.className == 'accordionImg' || e.target.className == 'accordionImg' || e.target.className == 'mobileMode' || e.target.className == 'mobileMode expanded' || e.target.className == 'mv') {
                            checkTouchType = true;
                            e.preventDefault();

                            if (e.target.className == 'accordionImg' || e.target.className == 'mobileMode' || e.target.className == 'mobileMode expanded') {
                                var $this = $(this),
                                    allPublications = $('#allPublicationsPan'),
                                    pPan = $this.closest('.publicationPan'),
                                    thead = pPan.find('thead'),
                                    tbody = pPan.find('tbody'),
                                    trs = tbody.find('tr'),
                                    disabledtrs = tbody.find('tr.disabled'),
                                    followlbl = thead.find('.followlbl'),
                                    followinglbl = thead.find('.followinglbl'),
                                    accStatusflwLbl = thead.find('.accordionStatus.flwLbl'),
                                    accStatusflwBtn = thead.find('.accordionStatus.flwBtn'),
                                    allpubpans = allPublications.find('.publicationPan'),
                                    pickTxt = thead.find('.pickTxt'),
                                    setFlag = true;

                                if ($this.hasClass('expanded')) {
                                    setFlag = false;
                                    tbody.addClass('tbodyhidden');
                                    //pPan.find('.smfollowingBtn').hide(); 
                                    accStatusflwLbl.removeClass('hideRow');
                                    accStatusflwBtn.addClass('hideRow');
                                    thead.find('.mtp').addClass('hideBtn');

                                    for (var i = 0; i < pickTxt.length; i++) {
                                        $(pickTxt[i]).closest('.accordionStatus').addClass('hideRow');
                                    }
                                    if (trs.length === disabledtrs.length) {
                                        followlbl.removeClass('hideBtn');
                                    } else {
                                        followinglbl.removeClass('hideBtn');
                                    }
                                    var position = $this.closest('.publicationPan').position();
                                    $(window).scrollTop(position.top - 40);
                                } else {
                                    allPublications.find('tbody').addClass('tbodyhidden');
                                    for (var i = 0; i < allpubpans.length; i++) {
                                        var eachPickTxt = $(allpubpans[i]).find('thead .pickTxt');
                                        for (var j = 0; j < eachPickTxt.length; j++) {
                                            $(eachPickTxt[j]).closest('.accordionStatus').addClass('hideRow');;
                                        }
                                    }
                                    thead.find('tr').removeClass('hidden');
                                    tbody.removeClass('tbodyhidden');
                                    pPan.find('.smfollowingBtn').show();
                                    for (var i = 0; i < pickTxt.length; i++) {
                                        $(pickTxt[i]).closest('.accordionStatus').removeClass('hideRow');
                                    }
                                    if (setFlag) {
                                        for (var i = 0; i < allpubpans.length; i++) {
                                            $(allpubpans[i]).find('.accordionStatus.flwLbl').removeClass('hideRow');
                                            $(allpubpans[i]).find('.accordionStatus.flwBtn').addClass('hideRow');
                                        }
                                    }
                                    accStatusflwLbl.addClass('hideRow');
                                    accStatusflwBtn.removeClass('hideRow');

                                    var position = $this.closest('.publicationPan').position();
                                    $(window).scrollTop(position.top - 40);

                                    for (var i = 0; i < allpubpans.length; i++) {
                                        var labelVal = $(allpubpans[i]).find('.firstrow .lableStatus').val();
                                        $('.' + labelVal, allpubpans[i]).removeClass('hideBtn');
                                    }
                                    thead.find('.mtp').addClass('hideBtn');
                                }
                            }
                        } else {
                            checkTouchType = false;
                            return false;
                        }

                        // show context menu if the user hasn't started dragging after a while
                        setTimeout(function () {
                            if (_this._dragSource == src && _this._img == null) {
                                if (_this._dispatchEvent(e, 'contextmenu', src)) {
                                    _this._reset();
                                }
                            }
                        }, DragDropTouch._CTXMENU);
                    }
                }
            }
        };
        DragDropTouch.prototype._touchmove = function (e) {
            if (checkTouchType) {
                if (this._shouldHandle(e)) {
                    // see if target wants to handle move
                    var target = this._getTarget(e);
                    if (this._dispatchEvent(e, 'mousemove', target)) {
                        this._lastTouch = e;
                        e.preventDefault();
                        return;
                    }
                    // start dragging
                    if (this._dragSource && !this._img) {
                        var delta = this._getDelta(e);
                        if (delta > DragDropTouch._THRESHOLD) {
                            this._dispatchEvent(e, 'dragstart', this._dragSource);
                            this._createImage(e);
                            this._dispatchEvent(e, 'dragenter', target);
                        }
                    }
                    // continue dragging
                    if (this._img) {
                        this._lastTouch = e;
                        e.preventDefault(); // prevent scrolling
                        if (target != this._lastTarget) {
                            this._dispatchEvent(this._lastTouch, 'dragleave', this._lastTarget);
                            this._dispatchEvent(e, 'dragenter', target);
                            this._lastTarget = target;
                        }
                        this._moveImage(e);
                        this._dispatchEvent(e, 'dragover', target);
                    }
                }
            }
        };
        DragDropTouch.prototype._touchend = function (e) {
            if (checkTouchType) {
                if (this._shouldHandle(e)) {
                    // see if target wants to handle up
                    if (this._dispatchEvent(this._lastTouch, 'mouseup', e.target)) {
                        e.preventDefault();
                        return;
                    }
                    // user clicked the element but didn't drag, so clear the source and simulate a click
                    if (!this._img) {
                        this._dragSource = null;
                        this._dispatchEvent(this._lastTouch, 'click', e.target);
                        this._lastClick = Date.now();
                    }
                    // finish dragging
                    this._destroyImage();
                    if (this._dragSource) {
                        if (e.type.indexOf('cancel') < 0) {
                            this._dispatchEvent(this._lastTouch, 'drop', this._lastTarget);
                        }
                        this._dispatchEvent(this._lastTouch, 'dragend', this._dragSource);
                        this._reset();
                    }
                }
            }
        };
        // ** utilities
        // ignore events that have been handled or that involve more than one touch
        DragDropTouch.prototype._shouldHandle = function (e) {
            return e && !e.defaultPrevented && e.touches && e.touches.length < 2;
        };
        // clear all members
        DragDropTouch.prototype._reset = function () {
            this._destroyImage();
            this._dragSource = null;
            this._lastTouch = null;
            this._lastTarget = null;
            this._ptDown = null;
            this._dataTransfer = new DataTransfer();
        };
        // get point for a touch event
        DragDropTouch.prototype._getPoint = function (e, page) {
            if (e && e.touches) {
                e = e.touches[0];
            }
            return { x: page ? e.pageX : e.clientX, y: page ? e.pageY : e.clientY };
        };
        // get distance between the current touch event and the first one
        DragDropTouch.prototype._getDelta = function (e) {
            var p = this._getPoint(e);
            return Math.abs(p.x - this._ptDown.x) + Math.abs(p.y - this._ptDown.y);
        };
        // get the element at a given touch event
        DragDropTouch.prototype._getTarget = function (e) {
            var pt = this._getPoint(e),
                el = document.elementFromPoint(pt.x, pt.y);
            while (el && getComputedStyle(el).pointerEvents == 'none') {
                el = el.parentElement;
            }
            return el;
        };
        // create drag image from source element
        DragDropTouch.prototype._createImage = function (e) {
            // just in case...
            if (this._img) {
                this._destroyImage();
            }
            // create drag image from custom element or drag source
            var src = this._imgCustom || this._dragSource;
            this._img = src.cloneNode(true);
            this._copyStyle(src, this._img);
            this._img.style.top = this._img.style.left = '-9999px';
            // if creating from drag source, apply offset and opacity
            if (!this._imgCustom) {
                var rc = src.getBoundingClientRect(),
                    pt = this._getPoint(e);
                this._imgOffset = { x: pt.x - rc.left, y: pt.y - rc.top };
                this._img.style.opacity = DragDropTouch._OPACITY.toString();
            }
            // add image to document
            this._moveImage(e);
            document.body.appendChild(this._img);
        };
        // dispose of drag image element
        DragDropTouch.prototype._destroyImage = function () {
            if (this._img && this._img.parentElement) {
                this._img.parentElement.removeChild(this._img);
            }
            this._img = null;
            this._imgCustom = null;
        };
        // move the drag image element
        DragDropTouch.prototype._moveImage = function (e) {
            var _this = this;
            requestAnimationFrame(function () {
                var pt = _this._getPoint(e, true),
                    s = _this._img.style;
                s.position = 'absolute';
                s.pointerEvents = 'none';
                s.zIndex = '999999';
                s.left = Math.round(pt.x - _this._imgOffset.x) + 'px';
                s.top = Math.round(pt.y - _this._imgOffset.y) + 'px';
            });
        };
        // copy properties from an object to another
        DragDropTouch.prototype._copyProps = function (dst, src, props) {
            for (var i = 0; i < props.length; i++) {
                var p = props[i];
                dst[p] = src[p];
            }
        };
        DragDropTouch.prototype._copyStyle = function (src, dst) {
            // remove potentially troublesome attributes
            DragDropTouch._rmvAtts.forEach(function (att) {
                dst.removeAttribute(att);
            });
            // copy canvas content
            if (src instanceof HTMLCanvasElement) {
                var cSrc = src,
                    cDst = dst;
                cDst.width = cSrc.width;
                cDst.height = cSrc.height;
                cDst.getContext('2d').drawImage(cSrc, 0, 0);
            }
            // copy style
            var cs = getComputedStyle(src);
            for (var i = 0; i < cs.length; i++) {
                var key = cs[i];
                dst.style[key] = cs[key];
            }
            dst.style.pointerEvents = 'none';
            // and repeat for all children
            for (var i = 0; i < src.children.length; i++) {
                this._copyStyle(src.children[i], dst.children[i]);
            }
        };
        DragDropTouch.prototype._dispatchEvent = function (e, type, target) {
            if (e && target) {
                var evt = document.createEvent('Event'),
                    t = e.touches ? e.touches[0] : e;
                evt.initEvent(type, true, true);
                evt.button = 0;
                evt.which = evt.buttons = 1;
                this._copyProps(evt, e, DragDropTouch._kbdProps);
                this._copyProps(evt, t, DragDropTouch._ptProps);
                evt.dataTransfer = this._dataTransfer;
                target.dispatchEvent(evt);
                return evt.defaultPrevented;
            }
            return false;
        };
        // gets an element's closest draggable ancestor
        DragDropTouch.prototype._closestDraggable = function (e) {
            for (; e; e = e.parentElement) {
                if (e.hasAttribute('draggable')) {
                    return e;
                }
            }
            return null;
        };
        /*private*/DragDropTouch._instance = new DragDropTouch(); // singleton
        // constants
        DragDropTouch._THRESHOLD = 5; // pixels to move before drag starts
        DragDropTouch._OPACITY = 0.5; // drag image opacity
        DragDropTouch._DBLCLICK = 500; // max ms between clicks in a double click
        DragDropTouch._CTXMENU = 900; // ms to hold before raising 'contextmenu' event
        // copy styles/attributes from drag source to drag image element
        DragDropTouch._rmvAtts = 'id,class,style,draggable'.split(',');
        // synthesize and dispatch an event
        // returns true if the event has been handled (e.preventDefault == true)
        DragDropTouch._kbdProps = 'altKey,ctrlKey,metaKey,shiftKey'.split(',');
        DragDropTouch._ptProps = 'pageX,pageY,clientX,clientY,screenX,screenY'.split(',');
        return DragDropTouch;
    })();
    DragDropTouch_1.DragDropTouch = DragDropTouch;
})(DragDropTouch || (DragDropTouch = {}));


},{}],2:[function(require,module,exports){
/**
 * if this popup is outside of it's parent, nudge it back in
 * @param  {element} popup: DOM elmenet of the popup to be placed
 * @param  {number} top: The top coordinate of where the popup should point
 * @param  {number} left: The left coordinate of where the popup should point
 * @param  {number} offset: an offet to be added to top/bottom or left/right
 * @param  {string} triangle: "top", "right", "bottom", or "left"
 * @param  {number} triangleSize: used to calculate the position
 * @param  {boolean} flipToContain: will flip the popup if it goes outside the parent container
 * @return {object} {
 *     realTop       : with no offset adjustment, the popup should go here, based on triangleSide
 *     realLeft      : ^^
 *     popupTop      : with adjustment when the popup butts up agains the parent
 *     popupLeft     : ^^
 *     overflow      : "top", "right", "bottom", "left".  Positive numbers are overflows
 *     triangleOffset: Amount the triangle needs to move to be on the dot, relative from 50%
 *     triangleSide  : Will be the same as passed in triangle, unless it fliped via flipToContain
 * }
 * use popupTop, popupLeft, and triangleOffset to position the popup
 */
"use strict";

Object.defineProperty(exports, "__esModule", {
    value: true
});
function calculatePopupOffsets(_ref) {
    var popup = _ref.popup;
    var top = _ref.top;
    var left = _ref.left;
    var _ref$offset = _ref.offset;
    var offset = _ref$offset === undefined ? 0 : _ref$offset;
    var _ref$triangle = _ref.triangle;
    var triangle = _ref$triangle === undefined ? "bottom" : _ref$triangle;
    var triangleSize = _ref.triangleSize;
    var _ref$flipToContain = _ref.flipToContain;
    var flipToContain = _ref$flipToContain === undefined ? false : _ref$flipToContain;

    // make a copy of this
    var triangleSide = triangle;

    // get the width and height of this popup from the DOM
    var width = popup.offsetWidth;
    var height = popup.offsetHeight;

    // get the width/height of the parent container div
    var parent = popup.parentNode;
    var parentWidth = parent.clientWidth;
    var parentHeight = parent.offsetHeight; // client height of body will only be the viewport height

    // common calculations
    var popupOnTop = top - height - triangleSize + offset;
    var popupOnBottom = top + triangleSize - offset;
    var popupOnLeft = left - width - triangleSize + offset;
    var popupOnRight = left + triangleSize - offset;

    // calculate where the top of the popup should be based on top/left
    var realTop = triangleSide === "bottom" ? popupOnTop : triangleSide === "top" ? popupOnBottom : top - height / 2; //  left or right

    var realLeft = triangleSide === "right" ? popupOnLeft : triangleSide === "left" ? popupOnRight : left - width / 2; // center

    // the amounts that this popup is outside of it's parent.
    var overflow = {
        top: -realTop,
        right: -(parentWidth - (realLeft + width)),
        bottom: -(parentHeight - (realTop + height)),
        left: -realLeft
    };

    // calculate where the popup should go
    // start with popupLeft as realLeft before nudging
    var popupTop = realTop;
    var popupLeft = realLeft;
    var triangleOffset = 0;

    // if there is an overflow on the right, adjust the popup and triangle position
    if (overflow.right > 0) {
        if (triangleSide === "top" || triangleSide === "bottom") {
            popupLeft = realLeft - overflow.right;
            triangleOffset = overflow.right;
        }

        // for left, flip the popup
        if (triangleSide === "left" && flipToContain) {
            triangleSide = "right";
            popupLeft = popupOnLeft;
        }
    }

    // if there is an overflow on the left, adjust the popup and triangle position
    if (overflow.left > 0) {
        if (triangleSide === "top" || triangleSide === "bottom") {
            popupLeft = realLeft + overflow.left;
            triangleOffset = -overflow.left;
        }

        // for right, flip the popup
        if (triangleSide === "right" && flipToContain) {
            triangleSide = "left";
            popupLeft = popupOnRight;
        }
    }

    // if there is an overflow on the bottom
    if (overflow.bottom > 0) {
        // for left/right, butt the popup against the bottom
        if (triangleSide === "left" || triangleSide === "right") {
            popupTop = realTop - overflow.bottom;
            triangleOffset = overflow.bottom;
        }
        // for top, flip the popup
        if (triangleSide === "top" && flipToContain) {
            triangleSide = "bottom";
            popupTop = popupOnTop;
        }
    }

    // if there is an overflow on the top
    if (overflow.top > 0) {

        if (triangleSide === "left" || triangleSide === "right") {
            popupTop = realTop + overflow.top;
            triangleOffset = -overflow.top;
        }

        // for bottom, flip the popup
        if (triangleSide === "bottom" && flipToContain) {
            triangleSide = "top";
            popupTop = popupOnBottom;
        }
    }

    // return all the measurements
    return {
        realTop: realTop, realLeft: realLeft, popupTop: popupTop, popupLeft: popupLeft, overflow: overflow, triangleOffset: triangleOffset, triangleSide: triangleSide
    };
}

exports["default"] = calculatePopupOffsets;
module.exports = exports["default"];

},{}],3:[function(require,module,exports){
"use strict";

(function e(t, n, r) {
	function s(o, u) {
		if (!n[o]) {
			if (!t[o]) {
				var a = typeof require == "function" && require;if (!u && a) return a(o, !0);if (i) return i(o, !0);var f = new Error("Cannot find module '" + o + "'");throw (f.code = "MODULE_NOT_FOUND", f);
			}var l = n[o] = { exports: {} };t[o][0].call(l.exports, function (e) {
				var n = t[o][1][e];return s(n ? n : e);
			}, l, l.exports, e, t, n, r);
		}return n[o].exports;
	}var i = typeof require == "function" && require;for (var o = 0; o < r.length; o++) s(r[o]);return s;
})({ 1: [function (require, module, exports) {
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
	}, {}] }, {}, [1]);

},{}],4:[function(require,module,exports){
//     Zepto.js
//     (c) 2010-2014 Thomas Fuchs
//     Zepto.js may be freely distributed under the MIT license.

// The following code is heavily inspired by jQuery's $.fn.data()

'use strict';

;(function ($) {
  var data = {},
      dataAttr = $.fn.data,
      camelize = $.camelCase,
      exp = $.expando = 'Zepto' + +new Date(),
      emptyArray = [];

  // Get value from node:
  // 1. first try key as given,
  // 2. then try camelized key,
  // 3. fall back to reading "data-*" attribute.
  function getData(node, name) {
    var id = node[exp],
        store = id && data[id];
    if (name === undefined) return store || setData(node);else {
      if (store) {
        if (name in store) return store[name];
        var camelName = camelize(name);
        if (camelName in store) return store[camelName];
      }
      return dataAttr.call($(node), name);
    }
  }

  // Store value under camelized key on node
  function setData(node, name, value) {
    var id = node[exp] || (node[exp] = ++$.uuid),
        store = data[id] || (data[id] = attributeData(node));
    if (name !== undefined) store[camelize(name)] = value;
    return store;
  }

  // Read all "data-*" attributes from a node
  function attributeData(node) {
    var store = {};
    $.each(node.attributes || emptyArray, function (i, attr) {
      if (attr.name.indexOf('data-') == 0) store[camelize(attr.name.replace('data-', ''))] = $.zepto.deserializeValue(attr.value);
    });
    return store;
  }

  $.fn.data = function (name, value) {
    return value === undefined ?
    // set multiple values via object
    $.isPlainObject(name) ? this.each(function (i, node) {
      $.each(name, function (key, value) {
        setData(node, key, value);
      });
    }) :
    // get value from first element
    this.length == 0 ? undefined : getData(this[0], name) :
    // set value on all elements
    this.each(function () {
      setData(this, name, value);
    });
  };

  $.fn.removeData = function (names) {
    if (typeof names == 'string') names = names.split(/\s+/);
    return this.each(function () {
      var id = this[exp],
          store = id && data[id];
      if (store) $.each(names || store, function (key) {
        delete store[names ? camelize(this) : key];
      });
    });
  };

  // Generate extended `remove` and `empty` functions
  ['remove', 'empty'].forEach(function (methodName) {
    var origFn = $.fn[methodName];
    $.fn[methodName] = function () {
      var elements = this.find('*');
      if (methodName === 'remove') elements = elements.add(this);
      elements.removeData();
      return origFn.call(this);
    };
  });
})(Zepto);

},{}],5:[function(require,module,exports){
'use strict';

$(document).ready(function () {
    if ($('.merge-acquistion').length > 0) {
        var FirstChartType = $('#chartdiv-deals').attr('data-type'),
            FirstChartColor = $('#chartdiv-deals').attr('data-color'),
            SecondChartType = $('#chartdiv-region').attr('data-type'),
            SecondChartColor = $('#chartdiv-region').attr('data-color'),
            ThirdChartType = $('#chartdiv-region2').attr('data-type'),
            ThirdChartColor = $('#chartdiv-region2').attr('data-color'),
            FourthChartType = $('#chartdiv-type').attr('data-type'),
            FourthChartColor = $('#chartdiv-type').attr('data-color');
        AmCharts.ready(function () {
            var chart = new AmCharts.AmSerialChart();
            chart.dataProvider = generateChartDataSizeRange();
            chart.categoryField = "range";
            var valueAxis1 = new AmCharts.ValueAxis();
            valueAxis1.integersOnly = true;
            chart.addValueAxis(valueAxis1);
            var graph = new AmCharts.AmGraph();
            graph.valueField = "count";
            graph.type = FirstChartType;
            graph.fillAlphas = 0.7;FourthChartType;
            graph.lineAlpha = 0.5;
            graph.balloonText = "[[title]]: [[count]]";
            graph.balloonColor = '#' + FirstChartColor;
            graph.fillColors = '#' + FirstChartColor;
            graph.lineColor = '#' + FirstChartColor;
            chart.addGraph(graph);
            chart.write('chartdiv-deals');
            var chart2 = new AmCharts.AmSerialChart();
            chart2.dataProvider = generateChartDataDealsByRegion();
            chart2.categoryField = "region";
            var valueAxis2 = new AmCharts.ValueAxis();
            valueAxis2.integersOnly = true;
            chart2.addValueAxis(valueAxis2);
            var graph2 = new AmCharts.AmGraph();
            graph2.valueField = "tally";
            graph2.type = SecondChartType;
            graph2.fillAlphas = 0.7;
            graph2.lineAlpha = 0.5;
            graph2.balloonText = "[[title]]: [[value]]";
            graph2.balloonColor = '#' + SecondChartColor;
            graph2.fillColors = '#' + SecondChartColor;
            graph2.lineColor = '#' + SecondChartColor;
            chart2.categoryAxis.labelRotation = 40;
            chart2.categoryAxis.autoGridCount = false;
            chart2.addGraph(graph2);
            chart2.write('chartdiv-region');
            var chart3 = new AmCharts.AmSerialChart();
            chart3.dataProvider = generateChartDataByRegion();
            chart3.categoryField = "region";
            var valueAxis3 = new AmCharts.ValueAxis();
            valueAxis3.integersOnly = true;
            chart3.addValueAxis(valueAxis3);
            var graph3 = new AmCharts.AmGraph();
            graph3.valueField = "count";
            graph3.type = ThirdChartType;
            graph3.fillAlphas = 0.7;
            graph3.lineAlpha = 0.5;
            graph3.balloonText = "[[title]]: [[value]]";
            graph3.balloonColor = '#' + ThirdChartColor;
            graph3.fillColors = '#' + ThirdChartColor;
            graph3.lineColor = '#' + ThirdChartColor;
            chart3.categoryAxis.labelRotation = 40;
            chart3.categoryAxis.autoGridCount = false;
            chart3.addGraph(graph3);
            chart3.write('chartdiv-region2');
            var chart4 = new AmCharts.AmSerialChart();
            chart4.dataProvider = generateChartData();
            chart4.validateData();
            chart4.categoryField = "type";
            var valueAxis4 = new AmCharts.ValueAxis();
            valueAxis4.integersOnly = true;
            chart4.addValueAxis(valueAxis4);
            var graph4 = new AmCharts.AmGraph();
            graph4.valueField = "count";
            graph4.type = FourthChartType;
            graph4.fillAlphas = 0.7;
            graph4.lineAlpha = 0.5;
            graph4.balloonText = "[[title]]: [[value]]";
            graph4.balloonColor = '#' + FourthChartColor;
            graph4.fillColors = '#' + FourthChartColor;
            graph4.lineColor = '#' + FourthChartColor;
            chart4.addGraph(graph4);
            chart4.write('chartdiv-type');
            var inputSet = $(".merge-acquistion th input, .merge-form-items input");
            inputSet.each(function () {
                if (!$(this).hasClass('range-field')) {
                    $(this).keyup(function () {
                        chart.dataProvider = generateChartDataSizeRange();
                        chart.validateData();
                        chart2.dataProvider = generateChartDataDealsByRegion();
                        chart2.validateData();
                        chart3.dataProvider = generateChartDataByRegion();
                        chart3.validateData();
                        chart4.dataProvider = generateChartData();
                        chart4.validateData();
                    });
                }
            });
            // $("#filterDropDown").change(function() {
            //     chart.dataProvider = generateChartDataSizeRange();
            //     chart.validateData();
            //     chart2.dataProvider = generateChartDataDealsByRegion();
            //     chart2.validateData();
            //     chart3.dataProvider = generateChartDataByRegion();
            //     chart3.validateData();
            //     chart4.dataProvider = generateChartData();
            //     chart4.validateData();
            // });
            $(".range-field.start").keyup(function () {
                chart.dataProvider = generateChartDataSizeRange();
                chart.validateData();
                chart2.dataProvider = generateChartDataDealsByRegion();
                chart2.validateData();
                chart3.dataProvider = generateChartDataByRegion();
                chart3.validateData();
                chart4.dataProvider = generateChartData();
                chart4.validateData();
            });
            $(".range-field.end").keyup(function () {
                chart.dataProvider = generateChartDataSizeRange();
                chart.validateData();
                chart2.dataProvider = generateChartDataDealsByRegion();
                chart2.validateData();
                chart3.dataProvider = generateChartDataByRegion();
                chart3.validateData();
                chart4.dataProvider = generateChartData();
                chart4.validateData();
            });
            $(".show-largest-btn").click(function () {
                setTimeout(function () {
                    chart.dataProvider = generateChartDataSizeRange();
                    chart.validateData();
                    chart2.dataProvider = generateChartDataDealsByRegion();
                    chart2.validateData();
                    chart3.dataProvider = generateChartDataByRegion();
                    chart3.validateData();
                    chart4.dataProvider = generateChartData();
                    chart4.validateData();
                }, 1000);
            });
            var defaultRegions = ["Africa", "Asia-Pacific", "Bermuda", "Europe", "Global", "Latin America", "London/UK", "North America"];
            var defaultTypes = ["non-life", "life", "international/reinsurance", "composite"];

            function generateChartDataSizeRange() {
                var chartDataType = [];
                var price = [];
                var i;
                $(".merge-acquistion td[deal='Price']").each(function () {
                    var tempPrice = $(this).text();
                    price.push(tempPrice);
                });
                var range = {
                    100: 0,
                    250: 0,
                    1000: 0
                };
                for (i = 0; i < price.length; i++) {
                    if (price[i] >= 100 && price[i] < 250) {
                        range[100] = range[100] + 1;
                    } else if (price[i] >= 250 && price[i] < 1000) {
                        range[250] = range[250] + 1;
                    } else if (price[i] >= 1000) {
                        range[1000] = range[1000] + 1;
                    }
                }
                var rangeAsArray = [];
                var rangeCountAsArray = [];
                for (var key in range) {
                    rangeAsArray.push(key);
                    rangeCountAsArray.push(range[key]);
                }
                for (i = 0; i < rangeAsArray.length; i++) {
                    rangeAsArray[i] = rangeAsArray[i] + "+";
                    var obj = {
                        range: rangeAsArray[i],
                        count: rangeCountAsArray[i],
                        title: rangeAsArray[i]
                    };
                    chartDataType.push(obj);
                }
                return chartDataType;
            }

            function generateChartDataDealsByRegion() {
                var chartDataType = [];
                var regions = [];
                $(".merge-acquistion td[deal='TargetLocation']").each(function () {
                    var tempRegion = $(this).text();
                    regions.push(tempRegion);
                });
                var price = [];
                $(".merge-acquistion td[deal='Price']").each(function () {
                    var tempPrice = $(this).text();
                    price.push(tempPrice);
                });
                var regionWithTotalPrice = {};
                var regionName = {};
                for (i = 0; i < regions.length; ++i) {
                    regionWithTotalPrice[regions[i]] = 0;
                    regionName[regions[i]] = regions[i];
                }
                for (i = 0; i < regions.length; ++i) {
                    if (price[i] != "-" && price[i] >= 100) {
                        regionWithTotalPrice[regions[i]] = regionWithTotalPrice[regions[i]] + parseFloat(price[i]);
                    }
                }
                var regionNameAsArray = [];
                for (var key in regionName) {
                    if (regionName.hasOwnProperty(key)) {
                        regionNameAsArray.push(regionName[key]);
                    }
                }
                regionNameAsArray.sort();
                var isNull = false;
                isNull = jQuery.isEmptyObject(price);
                if (!isNull) {
                    for (var i = 0; i < regionNameAsArray.length; i++) {
                        if (regionWithTotalPrice[regionNameAsArray[i]] != 0) {
                            var abbrRegionNameAsArray = abbreviate(regionNameAsArray[i]);
                            var obj = {
                                region: abbrRegionNameAsArray,
                                tally: regionWithTotalPrice[regionNameAsArray[i]],
                                title: regionNameAsArray[i]
                            };
                            chartDataType.push(obj);
                        }
                    }
                } else {
                    for (i = 0; i < defaultRegions.length; i++) {
                        var abbrRegionNameAsArray = abbreviate(defaultRegions[i]);
                        var obj = {
                            region: abbrRegionNameAsArray,
                            tally: 0,
                            title: defaultRegions[i]
                        };
                        chartDataType.push(obj);
                    }
                }
                if (chartDataType.length === 0) {
                    for (i = 0; i < defaultRegions.length; i++) {
                        var abbrRegionNameAsArray = abbreviate(defaultRegions[i]);
                        var obj = {
                            region: abbrRegionNameAsArray,
                            tally: 0,
                            title: defaultRegions[i]
                        };
                        chartDataType.push(obj);
                    }
                }
                return chartDataType;
            }

            function generateChartDataByRegion() {
                var chartDataType = [];
                var regions = [];
                $(".merge-acquistion td[deal='TargetLocation']").each(function () {
                    var tempRegion = $(this).text();
                    regions.push(tempRegion);
                });
                var price = [];
                $(".merge-acquistion td[deal='Price']").each(function () {
                    var tempPrice = $(this).text();
                    price.push(tempPrice);
                });
                var regionOver100m = [];
                for (var i = 0; i < regions.length; i++) {
                    if (price[i] != "-" && price[i] >= 100) {
                        regionOver100m.push(regions[i]);
                    }
                }
                var uniqueRegions = new stringSet();
                if (regionOver100m.length != 0) {
                    for (i = 0; i < regionOver100m.length; i++) {
                        uniqueRegions.add(regionOver100m[i]);
                    }
                }
                var orderedValueRegions = uniqueRegions.values();
                orderedValueRegions.sort();
                var cR = uniqueRegions.count();
                var isNull = false;
                isNull = jQuery.isEmptyObject(uniqueRegions.count());
                if (!isNull) {
                    for (i = 0; i < orderedValueRegions.length; i++) {
                        var abbrUniqueValueType = abbreviate(orderedValueRegions[i]);
                        var obj = {
                            region: abbrUniqueValueType,
                            count: cR[orderedValueRegions[i]],
                            title: orderedValueRegions[i]
                        };
                        chartDataType.push(obj);
                    }
                } else {
                    for (i = 0; i < defaultRegions.length; i++) {
                        var abbrRegionNameAsArray = abbreviate(defaultRegions[i]);
                        var obj = {
                            region: abbrRegionNameAsArray,
                            count: 0,
                            title: defaultRegions[i]
                        };
                        chartDataType.push(obj);
                    }
                }
                return chartDataType;
            }

            function generateChartData() {
                var chartDataType = [];
                var types = [];
                $(".merge-acquistion td[deal='TargetSector']").each(function () {
                    var tempType = $(this).text();
                    types.push(tempType);
                });
                var price = [];
                $(".merge-acquistion td[deal='Price']").each(function () {
                    var tempPrice = $(this).text();
                    price.push(tempPrice);
                });
                var typesOver100m = [];
                for (var i = 0; i < types.length; i++) {
                    if (price[i] != "-" && price[i] >= 100) {
                        typesOver100m.push(types[i]);
                    }
                }
                var uniqueTypes = new stringSet();
                if (typesOver100m.length != 0) {
                    for (i = 0; i < typesOver100m.length; i++) {
                        uniqueTypes.add(typesOver100m[i]);
                    }
                }
                var uniqueValueTypes = uniqueTypes.values();
                uniqueValueTypes.sort();
                uniqueValueTypes.reverse();
                var cT = uniqueTypes.count();
                var isNull = false;
                isNull = jQuery.isEmptyObject(uniqueTypes.count());
                if (!isNull) {
                    for (i = 0; i < uniqueValueTypes.length; i++) {
                        var abbrUniqueValueType = abbreviate(uniqueValueTypes[i]);
                        var obj = {
                            type: abbrUniqueValueType,
                            count: cT[uniqueValueTypes[i]],
                            title: uniqueValueTypes[i]
                        };
                        chartDataType.push(obj);
                    }
                } else {
                    for (i = 0; i < defaultTypes.length; i++) {
                        var abbrTypeNameAsArray = abbreviate(defaultTypes[i]);
                        var obj = {
                            type: abbrTypeNameAsArray,
                            count: 0,
                            title: defaultTypes[i]
                        };
                        chartDataType.push(obj);
                    }
                }
                return chartDataType;
            }

            function abbreviate(str) {
                if (str == "international/reinsurance") {
                    str = "int/re";
                }
                if (str == "composite") {
                    str = "com";
                }
                if (str == "Africa") {
                    str = "AF";
                }
                if (str == "Asia-Pacific") {
                    str = "APAC";
                }
                if (str == "Bermuda") {
                    str = "BM";
                }
                if (str == "Europe") {
                    str = "EU";
                }
                if (str == "Global") {
                    str = "G";
                }
                if (str == "Latin America") {
                    str = "LA";
                }
                if (str == "London/UK") {
                    str = "UK";
                }
                if (str == "North America") {
                    str = "NA";
                }
                return str;
            }

            function stringSet() {
                var setObj = {},
                    val = {};
                var objectCount = {};
                this.add = function (str) {
                    setObj[str] = val;
                    if (objectCount[str] == null || objectCount[str] == {}) {
                        objectCount[str] = 1;
                    } else {
                        var count = objectCount[str];
                        objectCount[str] = count + 1;
                    }
                };
                this.count = function () {
                    return objectCount;
                };
                this.contains = function (str) {
                    return setObj[str] === val;
                };
                this.remove = function (str) {
                    delete setObj[str];
                };
                this.values = function () {
                    var values = [];
                    for (var i in setObj) {
                        if (setObj[i] === val) {
                            values.push(i);
                        }
                    }
                    return values;
                };
            }
        });
    }
});

},{}],6:[function(require,module,exports){
'use strict';

$(function () {
	if ($('#compareChartVal') && $('#compareChartVal').val() == "true") {
		var GWPGraph = false;
		$("#GWP").on('click', function () {
			var $this = $(this),
			    eachChartData = $this.closest('.eachChartData'),
			    chartData = eachChartData.find('.chartData'),
			    arrow = eachChartData.find('.chartexpand');
			if (chartData.is(':visible')) {
				$this.removeClass('act');
				$this.find('.title').removeClass('act');
				chartData.addClass('hide');
				arrow.removeClass('active');
			} else {
				$this.addClass('act');
				$this.find('.title').addClass('act');
				chartData.removeClass('hide');
				arrow.addClass('active');
			}
			if (GWPGraph == false) {
				GWPcreateStockChart();
				GWPGraph = true;
			}
		});

		var NWPGraph = false;
		$("#NWP").on('click', function () {
			var $this = $(this),
			    eachChartData = $this.closest('.eachChartData'),
			    chartData = eachChartData.find('.chartData'),
			    arrow = eachChartData.find('.chartexpand');
			if (chartData.is(':visible')) {
				$this.removeClass('act');
				$this.find('.title').removeClass('act');
				chartData.addClass('hide');
				arrow.removeClass('active');
			} else {
				$this.addClass('act');
				$this.find('.title').addClass('act');
				chartData.removeClass('hide');
				arrow.addClass('active');
			}
			if (NWPGraph == false) {
				NWPcreateStockChart();
				NWPGraph = true;
			}
		});

		var URGraph = false;
		$("#UR").on('click', function () {
			var $this = $(this),
			    eachChartData = $this.closest('.eachChartData'),
			    chartData = eachChartData.find('.chartData'),
			    arrow = eachChartData.find('.chartexpand');
			if (chartData.is(':visible')) {
				$this.removeClass('act');
				$this.find('.title').removeClass('act');
				chartData.addClass('hide');
				arrow.removeClass('active');
			} else {
				$this.addClass('act');
				$this.find('.title').addClass('act');
				chartData.removeClass('hide');
				arrow.addClass('active');
			}
			if (URGraph == false) {
				URcreateStockChart();
				URGraph = true;
			}
		});

		var NPGraph = false;
		$("#NP").on('click', function () {
			var $this = $(this),
			    eachChartData = $this.closest('.eachChartData'),
			    chartData = eachChartData.find('.chartData'),
			    arrow = eachChartData.find('.chartexpand');
			if (chartData.is(':visible')) {
				$this.removeClass('act');
				$this.find('.title').removeClass('act');
				chartData.addClass('hide');
				arrow.removeClass('active');
			} else {
				$this.addClass('act');
				$this.find('.title').addClass('act');
				chartData.removeClass('hide');
				arrow.addClass('active');
			}
			if (NPGraph == false) {
				NPcreateStockChart();
				NPGraph = true;
			}
		});

		var SFGraph = false;
		$("#SF").on('click', function () {
			var $this = $(this),
			    eachChartData = $this.closest('.eachChartData'),
			    chartData = eachChartData.find('.chartData'),
			    arrow = eachChartData.find('.chartexpand');
			if (chartData.is(':visible')) {
				$this.removeClass('act');
				$this.find('.title').removeClass('act');
				chartData.addClass('hide');
				arrow.removeClass('active');
			} else {
				$this.addClass('act');
				$this.find('.title').addClass('act');
				chartData.removeClass('hide');
				arrow.addClass('active');
			}
			if (SFGraph == false) {
				SFcreateStockChart();
				SFGraph = true;
			}
		});

		var NWPNRGraph = false;
		$("#NWPNR").on('click', function () {
			var $this = $(this),
			    eachChartData = $this.closest('.eachChartData'),
			    chartData = eachChartData.find('.chartData'),
			    arrow = eachChartData.find('.chartexpand');
			if (chartData.is(':visible')) {
				$this.removeClass('act');
				$this.find('.title').removeClass('act');
				chartData.addClass('hide');
				arrow.removeClass('active');
			} else {
				$this.addClass('act');
				$this.find('.title').addClass('act');
				chartData.removeClass('hide');
				arrow.addClass('active');
			}
			if (NWPNRGraph == false) {
				NWPNRcreateStockChart();
				NWPNRGraph = true;
			}
		});

		var SFNRGraph = false;
		$("#SFNR").on('click', function () {
			var $this = $(this),
			    eachChartData = $this.closest('.eachChartData'),
			    chartData = eachChartData.find('.chartData'),
			    arrow = eachChartData.find('.chartexpand');
			if (chartData.is(':visible')) {
				$this.removeClass('act');
				$this.find('.title').removeClass('act');
				chartData.addClass('hide');
				arrow.removeClass('active');
			} else {
				$this.addClass('act');
				$this.find('.title').addClass('act');
				chartData.removeClass('hide');
				arrow.addClass('active');
			}
			if (SFNRGraph == false) {
				SFNRcreateStockChart();
				SFNRGraph = true;
			}
		});

		var NPSFRGraph = false;
		$("#NPSFR").on('click', function () {
			var $this = $(this),
			    eachChartData = $this.closest('.eachChartData'),
			    chartData = eachChartData.find('.chartData'),
			    arrow = eachChartData.find('.chartexpand');
			if (chartData.is(':visible')) {
				$this.removeClass('act');
				$this.find('.title').removeClass('act');
				chartData.addClass('hide');
				arrow.removeClass('active');
			} else {
				$this.addClass('act');
				$this.find('.title').addClass('act');
				chartData.removeClass('hide');
				arrow.addClass('active');
			}
			if (NPSFRGraph == false) {
				NPSFRcreateStockChart();
				NPSFRGraph = true;
			}
		});

		var geturl = window.location.href;
		if (geturl.indexOf('graphid=') !== -1) {
			var getId = geturl.split('graphid=')[1];
			$(window).scrollTop($('#' + getId).offset().top);
			$('#' + getId).find('.chartexpand').trigger('click');
		}
	}
});

// GWP STOCK CHART
function GWPcreateStockChart() {
	var dataSet = new Array();
	var dataSetList = new Array(),
	    matches,
	    d;
	var chart = new AmCharts.AmStockChart();
	chart.startDuration = 0;
	chart.startRadius = "100%";
	chart.pathToImages = "/skins/insurance-day/amcharts_2.7.6/images/";

	// DATASETS //////////////////////////////////////////
	// create data sets first

	for (var dataSetCtr = 0; dataSetCtr < chartDataSet.length; dataSetCtr++) {

		// KLUGE: translate given string e.g. 2006 - Q1 to date object
		for (var i = 0; i < dataVars['chartData' + (dataSetCtr + 1) + '_GWP'].length; i++) {
			//matches = window['chartData' + (dataSetCtr+1) + '_GWP'][i]['year'].match(/(\d\d\d\d).*Q(\d)/);
			//d = new Date();
			//d.setFullYear(matches[1], 0, 0);
			//d.setDate(d.getDate() + 90 * parseInt(matches[2], 10));
			matches = [dataVars['chartData' + (dataSetCtr + 1) + '_GWP'][i]['year'], dataVars['chartData' + (dataSetCtr + 1) + '_GWP'][i]['year'], '4'];
			d = new Date(matches[1], 11, 31, 0, 0, 0, 0);
			dataVars['chartData' + (dataSetCtr + 1) + '_GWP'][i]['year'] = d;
		}
		// KLUGE ends here

		dataSet[dataSetCtr] = new AmCharts.DataSet();
		dataSet[dataSetCtr].title = chartDataSet[dataSetCtr];

		dataSet[dataSetCtr].fieldMappings = [{
			fromField: "value",
			toField: "value"
		}, {
			fromField: "year",
			toField: "year"
		}];

		dataSet[dataSetCtr].dataProvider = dataVars['chartData' + (dataSetCtr + 1) + '_GWP'];
		dataSet[dataSetCtr].categoryField = "year";

		dataSetList.push(dataSet[dataSetCtr]);
	}
	chart.dataSets = dataSetList;
	chart.chartCursorSettings.zoomable = false;

	// PANELS ///////////////////////////////////////////

	// first stock panel
	var stockPanel1 = new AmCharts.StockPanel();
	stockPanel1.showCategoryAxis = true;
	stockPanel1.title = "Company";
	stockPanel1.percentHeight = 70;
	stockPanel1.startDuration = 0;
	stockPanel1.startRadius = "100%";

	// graph of first stock panel
	var graph1 = new AmCharts.StockGraph();
	graph1.valueField = "value";
	graph1.comparable = true;
	graph1.compareField = "value";
	graph1.startDuration = 0;
	graph1.startRadius = "100%";
	stockPanel1.addStockGraph(graph1);
	stockPanel1.recalculateToPercents = "never";

	// create stock legend
	stockPanel1.stockLegend = new AmCharts.StockLegend();
	stockPanel1.startDuration = 0;
	stockPanel1.startRadius = "100%";
	// second stock panel

	// set panels to the chart
	chart.panels = [stockPanel1];

	// OTHER SETTINGS ////////////////////////////////////
	var sbsettings = new AmCharts.ChartScrollbarSettings();
	sbsettings.graph = graph1;
	sbsettings.selectedBackgroundColor = "#888";
	sbsettings.selectedGraphFillColor = "#666";
	sbsettings.enabled = false;
	chart.chartScrollbarSettings = sbsettings;

	// DATA SET SELECTOR
	var dataSetSelector = new AmCharts.DataSetSelector();
	dataSetSelector.position = "right";
	chart.dataSetSelector = dataSetSelector;
	chart.write('GWP-CHART');
}

// NWP STOCK CHART
function NWPcreateStockChart() {
	var dataSet = new Array();
	var dataSetList = new Array();
	var chart = new AmCharts.AmStockChart(),
	    matches,
	    d;
	chart.startDuration = 0;
	chart.startRadius = "100%";

	chart.pathToImages = "/skins/insurance-day/amcharts_2.7.6/images/";

	// DATASETS //////////////////////////////////////////
	// create data sets first

	for (var dataSetCtr = 0; dataSetCtr < chartDataSet.length; dataSetCtr++) {

		// KLUGE: translate given string e.g. 2006 - Q1 to date object
		for (var i = 0; i < dataVars['chartData' + (dataSetCtr + 1) + '_NWP'].length; i++) {
			//matches = window['chartData' + (dataSetCtr+1) + '_NWP'][i]['year'].match(/(\d\d\d\d).*Q(\d)/);
			//d = new Date();
			//d.setFullYear(matches[1], 0, 0);
			//d.setDate(d.getDate() + 90 * parseInt(matches[2], 10));
			matches = [dataVars['chartData' + (dataSetCtr + 1) + '_NWP'][i]['year'], dataVars['chartData' + (dataSetCtr + 1) + '_NWP'][i]['year'], '4'];
			d = new Date(matches[1], 11, 31, 0, 0, 0, 0);
			dataVars['chartData' + (dataSetCtr + 1) + '_NWP'][i]['year'] = d;
		}
		// KLUGE ends here

		dataSet[dataSetCtr] = new AmCharts.DataSet();
		dataSet[dataSetCtr].title = chartDataSet[dataSetCtr];

		dataSet[dataSetCtr].fieldMappings = [{
			fromField: "value",
			toField: "value"
		}, {
			fromField: "year",
			toField: "year"
		}];

		dataSet[dataSetCtr].dataProvider = dataVars['chartData' + (dataSetCtr + 1) + '_NWP'];
		dataSet[dataSetCtr].categoryField = "year";

		dataSetList.push(dataSet[dataSetCtr]);
	}
	chart.dataSets = dataSetList;
	chart.chartCursorSettings.zoomable = false;

	// PANELS ///////////////////////////////////////////

	// first stock panel
	var stockPanel1 = new AmCharts.StockPanel();
	stockPanel1.showCategoryAxis = true;
	stockPanel1.title = "Company";
	stockPanel1.percentHeight = 70;
	stockPanel1.startDuration = 0;
	stockPanel1.startRadius = "100%";

	// graph of first stock panel
	var graph1 = new AmCharts.StockGraph();
	graph1.valueField = "value";
	graph1.comparable = true;
	graph1.compareField = "value";
	graph1.startDuration = 0;
	graph1.startRadius = "100%";
	stockPanel1.addStockGraph(graph1);
	stockPanel1.recalculateToPercents = "never";

	// create stock legend
	stockPanel1.stockLegend = new AmCharts.StockLegend();
	// second stock panel

	chart.panels = [stockPanel1];

	// OTHER SETTINGS ////////////////////////////////////
	var sbsettings = new AmCharts.ChartScrollbarSettings();
	sbsettings.graph = graph1;
	sbsettings.selectedBackgroundColor = "#888";
	sbsettings.selectedGraphFillColor = "#666";
	sbsettings.enabled = false;
	chart.chartScrollbarSettings = sbsettings;

	// DATA SET SELECTOR
	var dataSetSelector = new AmCharts.DataSetSelector();
	dataSetSelector.position = "right";
	chart.dataSetSelector = dataSetSelector;

	chart.write('NWP-CHART');
}

// UR STOCK CHART
function URcreateStockChart() {
	var dataSet = new Array();
	var dataSetList = new Array();
	var chart = new AmCharts.AmStockChart(),
	    matches,
	    d;
	chart.startDuration = 0;
	chart.startRadius = "100%";

	chart.pathToImages = "/skins/insurance-day/amcharts_2.7.6/images/";

	// DATASETS //////////////////////////////////////////
	// create data sets first

	for (var dataSetCtr = 0; dataSetCtr < chartDataSet.length; dataSetCtr++) {

		// KLUGE: translate given string e.g. 2006 - Q1 to date object
		for (i = 0; i < dataVars['chartData' + (dataSetCtr + 1) + '_UR'].length; i++) {
			//matches = window['chartData' + (dataSetCtr+1) + '_UR'][i]['year'].match(/(\d\d\d\d).*Q(\d)/);
			//d = new Date();
			//d.setFullYear(matches[1], 0, 0);
			//d.setDate(d.getDate() + 90 * parseInt(matches[2], 10));
			matches = [dataVars['chartData' + (dataSetCtr + 1) + '_UR'][i]['year'], dataVars['chartData' + (dataSetCtr + 1) + '_UR'][i]['year'], '4'];
			d = new Date(matches[1], 11, 31, 0, 0, 0, 0);
			dataVars['chartData' + (dataSetCtr + 1) + '_UR'][i]['year'] = d;
		}
		// KLUGE ends here

		dataSet[dataSetCtr] = new AmCharts.DataSet();
		dataSet[dataSetCtr].title = chartDataSet[dataSetCtr];

		dataSet[dataSetCtr].fieldMappings = [{
			fromField: "value",
			toField: "value"
		}, {
			fromField: "year",
			toField: "year"
		}];

		dataSet[dataSetCtr].dataProvider = dataVars['chartData' + (dataSetCtr + 1) + '_UR'];
		dataSet[dataSetCtr].categoryField = "year";

		dataSetList.push(dataSet[dataSetCtr]);
	}
	chart.dataSets = dataSetList;
	chart.chartCursorSettings.zoomable = false;

	// PANELS ///////////////////////////////////////////

	// first stock panel
	var stockPanel1 = new AmCharts.StockPanel();
	stockPanel1.showCategoryAxis = true;
	stockPanel1.title = "Company";
	stockPanel1.percentHeight = 70;
	stockPanel1.startDuration = 0;
	stockPanel1.startRadius = "100%";

	// graph of first stock panel
	var graph1 = new AmCharts.StockGraph();
	graph1.valueField = "value";
	graph1.comparable = true;
	graph1.compareField = "value";
	graph1.startDuration = 0;
	graph1.startRadius = "100%";
	stockPanel1.addStockGraph(graph1);
	stockPanel1.recalculateToPercents = "never";

	// create stock legend
	stockPanel1.stockLegend = new AmCharts.StockLegend();
	// second stock panel

	chart.panels = [stockPanel1];

	// OTHER SETTINGS ////////////////////////////////////
	var sbsettings = new AmCharts.ChartScrollbarSettings();
	sbsettings.graph = graph1;
	sbsettings.selectedBackgroundColor = "#888";
	sbsettings.selectedGraphFillColor = "#666";
	sbsettings.enabled = false;
	chart.chartScrollbarSettings = sbsettings;

	// DATA SET SELECTOR
	var dataSetSelector = new AmCharts.DataSetSelector();
	dataSetSelector.position = "right";
	chart.dataSetSelector = dataSetSelector;

	chart.write('UR-CHART');
}

// NP STOCK CHART
function NPcreateStockChart() {
	var dataSet = new Array();
	var dataSetList = new Array();
	var chart = new AmCharts.AmStockChart(),
	    matches,
	    d;
	chart.startDuration = 0;
	chart.startRadius = "100%";

	chart.pathToImages = "/skins/insurance-day/amcharts_2.7.6/images/";

	// DATASETS //////////////////////////////////////////
	// create data sets first

	for (var dataSetCtr = 0; dataSetCtr < chartDataSet.length; dataSetCtr++) {

		// KLUGE: translate given string e.g. 2006 - Q1 to date object
		for (i = 0; i < dataVars['chartData' + (dataSetCtr + 1) + '_NP'].length; i++) {
			//matches = window['chartData' + (dataSetCtr+1) + '_NP'][i]['year'].match(/(\d\d\d\d).*Q(\d)/);
			//d = new Date();
			//d.setFullYear(matches[1], 0, 0);
			//d.setDate(d.getDate() + 90 * parseInt(matches[2], 10));
			matches = [dataVars['chartData' + (dataSetCtr + 1) + '_NP'][i]['year'], dataVars['chartData' + (dataSetCtr + 1) + '_NP'][i]['year'], '4'];
			d = new Date(matches[1], 11, 31, 0, 0, 0, 0);
			dataVars['chartData' + (dataSetCtr + 1) + '_NP'][i]['year'] = d;
		}
		// KLUGE ends here

		dataSet[dataSetCtr] = new AmCharts.DataSet();
		dataSet[dataSetCtr].title = chartDataSet[dataSetCtr];

		dataSet[dataSetCtr].fieldMappings = [{
			fromField: "value",
			toField: "value"
		}, {
			fromField: "year",
			toField: "year"
		}];

		dataSet[dataSetCtr].dataProvider = dataVars['chartData' + (dataSetCtr + 1) + '_NP'];
		dataSet[dataSetCtr].categoryField = "year";

		dataSetList.push(dataSet[dataSetCtr]);
	}
	chart.dataSets = dataSetList;
	chart.chartCursorSettings.zoomable = false;

	// PANELS ///////////////////////////////////////////

	// first stock panel
	var stockPanel1 = new AmCharts.StockPanel();
	stockPanel1.showCategoryAxis = true;
	stockPanel1.title = "Company";
	stockPanel1.percentHeight = 70;
	stockPanel1.startDuration = 0;
	stockPanel1.startRadius = "100%";

	// graph of first stock panel
	var graph1 = new AmCharts.StockGraph();
	graph1.valueField = "value";
	graph1.comparable = true;
	graph1.compareField = "value";
	graph1.startDuration = 0;
	graph1.startRadius = "100%";
	stockPanel1.addStockGraph(graph1);
	stockPanel1.recalculateToPercents = "never";

	// create stock legend
	stockPanel1.stockLegend = new AmCharts.StockLegend();
	// second stock panel

	chart.panels = [stockPanel1];

	// OTHER SETTINGS ////////////////////////////////////
	var sbsettings = new AmCharts.ChartScrollbarSettings();
	sbsettings.graph = graph1;
	sbsettings.selectedBackgroundColor = "#888";
	sbsettings.selectedGraphFillColor = "#666";
	sbsettings.enabled = false;
	chart.chartScrollbarSettings = sbsettings;

	// DATA SET SELECTOR
	var dataSetSelector = new AmCharts.DataSetSelector();
	dataSetSelector.position = "right";
	chart.dataSetSelector = dataSetSelector;

	chart.write('NP-CHART');
}

// SF STOCK CHART
function SFcreateStockChart() {
	var dataSet = new Array();
	var dataSetList = new Array();
	var chart = new AmCharts.AmStockChart(),
	    matches,
	    d;
	chart.startDuration = 0;
	chart.startRadius = "100%";

	chart.pathToImages = "/skins/insurance-day/amcharts_2.7.6/images/";

	// DATASETS //////////////////////////////////////////
	// create data sets first

	for (var dataSetCtr = 0; dataSetCtr < chartDataSet.length; dataSetCtr++) {

		// KLUGE: translate given string e.g. 2006 - Q1 to date object
		for (i = 0; i < dataVars['chartData' + (dataSetCtr + 1) + '_SF'].length; i++) {
			//matches = window['chartData' + (dataSetCtr+1) + '_SF'][i]['year'].match(/(\d\d\d\d).*Q(\d)/);
			//d = new Date();
			//d.setFullYear(matches[1], 0, 0);
			//d.setDate(d.getDate() + 90 * parseInt(matches[2], 10));
			matches = [dataVars['chartData' + (dataSetCtr + 1) + '_SF'][i]['year'], dataVars['chartData' + (dataSetCtr + 1) + '_SF'][i]['year'], '4'];
			d = new Date(matches[1], 11, 31, 0, 0, 0, 0);
			dataVars['chartData' + (dataSetCtr + 1) + '_SF'][i]['year'] = d;
		}
		// KLUGE ends here

		dataSet[dataSetCtr] = new AmCharts.DataSet();
		dataSet[dataSetCtr].title = chartDataSet[dataSetCtr];

		dataSet[dataSetCtr].fieldMappings = [{
			fromField: "value",
			toField: "value"
		}, {
			fromField: "year",
			toField: "year"
		}];

		dataSet[dataSetCtr].dataProvider = dataVars['chartData' + (dataSetCtr + 1) + '_SF'];
		dataSet[dataSetCtr].categoryField = "year";

		dataSetList.push(dataSet[dataSetCtr]);
	}
	chart.dataSets = dataSetList;
	chart.chartCursorSettings.zoomable = false;

	// PANELS ///////////////////////////////////////////

	// first stock panel
	var stockPanel1 = new AmCharts.StockPanel();
	stockPanel1.showCategoryAxis = true;
	stockPanel1.title = "Company";
	stockPanel1.percentHeight = 70;
	stockPanel1.startDuration = 0;
	stockPanel1.startRadius = "100%";

	// graph of first stock panel
	var graph1 = new AmCharts.StockGraph();
	graph1.valueField = "value";
	graph1.comparable = true;
	graph1.compareField = "value";
	graph1.startDuration = 0;
	graph1.startRadius = "100%";
	stockPanel1.addStockGraph(graph1);
	stockPanel1.recalculateToPercents = "never";

	// create stock legend
	stockPanel1.stockLegend = new AmCharts.StockLegend();
	// second stock panel

	chart.panels = [stockPanel1];

	// OTHER SETTINGS ////////////////////////////////////
	var sbsettings = new AmCharts.ChartScrollbarSettings();
	sbsettings.graph = graph1;
	sbsettings.selectedBackgroundColor = "#888";
	sbsettings.selectedGraphFillColor = "#666";
	sbsettings.enabled = false;
	chart.chartScrollbarSettings = sbsettings;

	// DATA SET SELECTOR
	var dataSetSelector = new AmCharts.DataSetSelector();
	dataSetSelector.position = "right";
	chart.dataSetSelector = dataSetSelector;

	chart.write('SF-CHART');
}

// NWPNR STOCK CHART
function NWPNRcreateStockChart() {
	var dataSet = new Array();
	var dataSetList = new Array();
	var chart = new AmCharts.AmStockChart(),
	    matches,
	    d;
	chart.startDuration = 0;
	chart.startRadius = "100%";

	chart.pathToImages = "/skins/insurance-day/amcharts_2.7.6/images/";

	// DATASETS //////////////////////////////////////////
	// create data sets first

	for (var dataSetCtr = 0; dataSetCtr < chartDataSet.length; dataSetCtr++) {

		// KLUGE: translate given string e.g. 2006 - Q1 to date object
		for (i = 0; i < dataVars['chartData' + (dataSetCtr + 1) + '_NWPNR'].length; i++) {
			//matches = window['chartData' + (dataSetCtr+1) + '_NWPNR'][i]['year'].match(/(\d\d\d\d).*Q(\d)/);
			//d = new Date();
			//d.setFullYear(matches[1], 0, 0);
			//d.setDate(d.getDate() + 90 * parseInt(matches[2], 10));
			matches = [dataVars['chartData' + (dataSetCtr + 1) + '_NWPNR'][i]['year'], dataVars['chartData' + (dataSetCtr + 1) + '_NWPNR'][i]['year'], '4'];
			d = new Date(matches[1], 11, 31, 0, 0, 0, 0);
			dataVars['chartData' + (dataSetCtr + 1) + '_NWPNR'][i]['year'] = d;
		}
		// KLUGE ends here

		dataSet[dataSetCtr] = new AmCharts.DataSet();
		dataSet[dataSetCtr].title = chartDataSet[dataSetCtr];

		dataSet[dataSetCtr].fieldMappings = [{
			fromField: "value",
			toField: "value"
		}, {
			fromField: "year",
			toField: "year"
		}];

		dataSet[dataSetCtr].dataProvider = dataVars['chartData' + (dataSetCtr + 1) + '_NWPNR'];
		dataSet[dataSetCtr].categoryField = "year";

		dataSetList.push(dataSet[dataSetCtr]);
	}
	chart.dataSets = dataSetList;
	chart.chartCursorSettings.zoomable = false;

	// PANELS ///////////////////////////////////////////

	// first stock panel
	var stockPanel1 = new AmCharts.StockPanel();
	stockPanel1.showCategoryAxis = true;
	stockPanel1.title = "Company";
	stockPanel1.percentHeight = 70;
	stockPanel1.startDuration = 0;
	stockPanel1.startRadius = "100%";

	// graph of first stock panel
	var graph1 = new AmCharts.StockGraph();
	graph1.valueField = "value";
	graph1.comparable = true;
	graph1.compareField = "value";
	graph1.startDuration = 0;
	graph1.startRadius = "100%";
	stockPanel1.addStockGraph(graph1);
	stockPanel1.recalculateToPercents = "never";

	// create stock legend
	stockPanel1.stockLegend = new AmCharts.StockLegend();
	// second stock panel

	chart.panels = [stockPanel1];

	// OTHER SETTINGS ////////////////////////////////////
	var sbsettings = new AmCharts.ChartScrollbarSettings();
	sbsettings.graph = graph1;
	sbsettings.selectedBackgroundColor = "#888";
	sbsettings.selectedGraphFillColor = "#666";
	sbsettings.enabled = false;
	chart.chartScrollbarSettings = sbsettings;

	// DATA SET SELECTOR
	var dataSetSelector = new AmCharts.DataSetSelector();
	dataSetSelector.position = "right";
	chart.dataSetSelector = dataSetSelector;

	chart.write('NWPNR-CHART');
}

// SFNR STOCK CHART
function SFNRcreateStockChart() {
	var dataSet = new Array();
	var dataSetList = new Array();
	var chart = new AmCharts.AmStockChart(),
	    matches,
	    d;
	chart.startDuration = 0;
	chart.startRadius = "100%";

	chart.pathToImages = "/skins/insurance-day/amcharts_2.7.6/images/";

	// DATASETS //////////////////////////////////////////
	// create data sets first

	for (var dataSetCtr = 0; dataSetCtr < chartDataSet.length; dataSetCtr++) {

		// KLUGE: translate given string e.g. 2006 - Q1 to date object
		for (i = 0; i < dataVars['chartData' + (dataSetCtr + 1) + '_SFNR'].length; i++) {
			//matches = window['chartData' + (dataSetCtr+1) + '_SFNR'][i]['year'].match(/(\d\d\d\d).*Q(\d)/);
			//d = new Date();
			//d.setFullYear(matches[1], 0, 0);
			//d.setDate(d.getDate() + 90 * parseInt(matches[2], 10));
			matches = [dataVars['chartData' + (dataSetCtr + 1) + '_SFNR'][i]['year'], dataVars['chartData' + (dataSetCtr + 1) + '_SFNR'][i]['year'], '4'];
			d = new Date(matches[1], 11, 31, 0, 0, 0, 0);
			dataVars['chartData' + (dataSetCtr + 1) + '_SFNR'][i]['year'] = d;
		}
		// KLUGE ends here

		dataSet[dataSetCtr] = new AmCharts.DataSet();
		dataSet[dataSetCtr].title = chartDataSet[dataSetCtr];

		dataSet[dataSetCtr].fieldMappings = [{
			fromField: "value",
			toField: "value"
		}, {
			fromField: "year",
			toField: "year"
		}];

		dataSet[dataSetCtr].dataProvider = dataVars['chartData' + (dataSetCtr + 1) + '_SFNR'];
		dataSet[dataSetCtr].categoryField = "year";

		dataSetList.push(dataSet[dataSetCtr]);
	}
	chart.dataSets = dataSetList;
	chart.chartCursorSettings.zoomable = false;

	// PANELS ///////////////////////////////////////////

	// first stock panel
	var stockPanel1 = new AmCharts.StockPanel();
	stockPanel1.showCategoryAxis = true;
	stockPanel1.title = "Company";
	stockPanel1.percentHeight = 70;
	stockPanel1.startDuration = 0;
	stockPanel1.startRadius = "100%";

	// graph of first stock panel
	var graph1 = new AmCharts.StockGraph();
	graph1.valueField = "value";
	graph1.comparable = true;
	graph1.compareField = "value";
	graph1.startDuration = 0;
	graph1.startRadius = "100%";
	stockPanel1.addStockGraph(graph1);
	stockPanel1.recalculateToPercents = "never";

	// create stock legend
	stockPanel1.stockLegend = new AmCharts.StockLegend();
	// second stock panel

	chart.panels = [stockPanel1];

	// OTHER SETTINGS ////////////////////////////////////
	var sbsettings = new AmCharts.ChartScrollbarSettings();
	sbsettings.graph = graph1;
	sbsettings.selectedBackgroundColor = "#888";
	sbsettings.selectedGraphFillColor = "#666";
	sbsettings.enabled = false;
	chart.chartScrollbarSettings = sbsettings;

	// DATA SET SELECTOR
	var dataSetSelector = new AmCharts.DataSetSelector();
	dataSetSelector.position = "right";
	chart.dataSetSelector = dataSetSelector;

	chart.write('SFNR-CHART');
}

// NPSFR STOCK CHART
function NPSFRcreateStockChart() {
	var dataSet = new Array();
	var dataSetList = new Array();
	var chart = new AmCharts.AmStockChart(),
	    matches,
	    d;
	chart.startDuration = 0;
	chart.startRadius = "100%";

	chart.pathToImages = "/skins/insurance-day/amcharts_2.7.6/images/";

	// DATASETS //////////////////////////////////////////
	// create data sets first

	for (var dataSetCtr = 0; dataSetCtr < chartDataSet.length; dataSetCtr++) {

		// KLUGE: translate given string e.g. 2006 - Q1 to date object
		for (i = 0; i < dataVars['chartData' + (dataSetCtr + 1) + '_NPSFR'].length; i++) {
			//matches = window['chartData' + (dataSetCtr+1) + '_NPSFR'][i]['year'].match(/(\d\d\d\d).*Q(\d)/);
			//d = new Date();
			//d.setFullYear(matches[1], 0, 0);
			//d.setDate(d.getDate() + 90 * parseInt(matches[2], 10));
			matches = [dataVars['chartData' + (dataSetCtr + 1) + '_NPSFR'][i]['year'], dataVars['chartData' + (dataSetCtr + 1) + '_NPSFR'][i]['year'], '4'];
			d = new Date(matches[1], 11, 31, 0, 0, 0, 0);
			dataVars['chartData' + (dataSetCtr + 1) + '_NPSFR'][i]['year'] = d;
		}
		// KLUGE ends here

		dataSet[dataSetCtr] = new AmCharts.DataSet();
		dataSet[dataSetCtr].title = chartDataSet[dataSetCtr];

		dataSet[dataSetCtr].fieldMappings = [{
			fromField: "value",
			toField: "value"
		}, {
			fromField: "year",
			toField: "year"
		}];

		dataSet[dataSetCtr].dataProvider = dataVars['chartData' + (dataSetCtr + 1) + '_NPSFR'];
		dataSet[dataSetCtr].categoryField = "year";

		dataSetList.push(dataSet[dataSetCtr]);
	}
	chart.dataSets = dataSetList;
	chart.chartCursorSettings.zoomable = false;

	// PANELS ///////////////////////////////////////////

	// first stock panel
	var stockPanel1 = new AmCharts.StockPanel();
	stockPanel1.showCategoryAxis = true;
	stockPanel1.title = "Company";
	stockPanel1.percentHeight = 70;
	stockPanel1.startDuration = 0;
	stockPanel1.startRadius = "100%";

	// graph of first stock panel
	var graph1 = new AmCharts.StockGraph();
	graph1.valueField = "value";
	graph1.comparable = true;
	graph1.compareField = "value";
	graph1.startDuration = 0;
	graph1.startRadius = "100%";
	stockPanel1.addStockGraph(graph1);
	stockPanel1.recalculateToPercents = "never";

	// create stock legend
	stockPanel1.stockLegend = new AmCharts.StockLegend();
	// second stock panel

	chart.panels = [stockPanel1];

	// OTHER SETTINGS ////////////////////////////////////
	var sbsettings = new AmCharts.ChartScrollbarSettings();
	sbsettings.graph = graph1;
	sbsettings.selectedBackgroundColor = "#888";
	sbsettings.selectedGraphFillColor = "#666";
	sbsettings.enabled = false;
	chart.chartScrollbarSettings = sbsettings;

	// DATA SET SELECTOR
	var dataSetSelector = new AmCharts.DataSetSelector();
	dataSetSelector.position = "right";
	chart.dataSetSelector = dataSetSelector;

	chart.write('NPSFR-CHART');
}

},{}],7:[function(require,module,exports){
'use strict';

function ConvertToCSV(objArray) {
    var array = typeof objArray != 'object' ? JSON.parse(objArray) : objArray;
    var str = '';

    for (var i = 0; i < array.length; i++) {
        var line = '';
        var value = '';
        var year = '';
        for (var index in array[i]) {
            if (index == 'year') year = array[i][index];
            if (index == 'value') value = array[i][index];
        }
        line = year + ';' + value;
        str += line + '\n';
    }

    return str;
}

window.PrintGraph = function (chartData, divId, skinUrl) {

    if (chartData.length > 0) {

        /*if ($.browser.msie){
              var jsonObject = JSON.stringify(chartData);
              var csvData = ConvertToCSV(jsonObject);
              var chartColor = chartData[0].colour;
              var params =
            {
                bgcolor:"#E7E7E7",
                wmode:"opaque"
            };
              var chartSettings ="<settings><data_type>csv</data_type><background><color>E7E7E7</color></background><text_size>9</text_size><colors>"+chartColor+","+chartColor+"</colors><plot_area><margins><left>55</left><right>25</right><top>20</top><bottom>25</bottom></margins></plot_area><grid><category><alpha>13</alpha><dash_length>1</dash_length></category><value><alpha>13</alpha><dash_length>1</dash_length><approx_count>3</approx_count></value></grid><axes><category><width>1</width></category><value><width>1</width></value></axes><values><category><frequency>2</frequency></category><value><skip_first>0</skip_first></value></values><legend><enabled>0</enabled></legend><angle>0</angle><column><alpha>83</alpha><border_alpha>23</border_alpha><balloon_text>{category}:{value}</balloon_text></column><graphs><graphgid='0'><title>Stock</title><alpha>85</alpha><visible_in_legend>0</visible_in_legend></graph></graphs><labels><labellid='0'><y>18</y><align>center</align></label></labels></settings>";
              var flashVars =
            {
                path: skinUrl + "amcharts_2.7.6/flash/",
                  chart_data: csvData,
                  chart_settings: chartSettings
            };
              swfobject.embedSWF(skinUrl + "amcharts_2.7.6/flash/amcolumn.swf", divId, "220", "120", "8.0.0", skinUrl + "amcharts_2.7.6/flash/expressInstall.swf", flashVars, params);*/

        //} else{
        var chart1 = new AmCharts.AmSerialChart();
        /*
        // Remove " - Q1, - Q2" from the year:
        $.each(chartData, function(i, item) {
            if(item.year){
                var posQ = item.year.indexOf(" - ");
                if(posQ > 0){
                    item.year = item.year.substring(0,posQ);
                }
            } // Year is defined.
        });
        */

        chart1.dataProvider = chartData;
        chart1.categoryField = "year";

        chart1.marginTop = 20;
        chart1.marginBottom = 25;
        chart1.marginLeft = 55;
        //chart1.marginLeft = 45;
        chart1.marginRight = 25;
        chart1.height = '100%';
        chart1.width = '100%';
        //chart1.fontSize = 8;

        chart1.startDuration = 0;
        chart1.startRadius = "100%";

        var graph1 = new AmCharts.AmGraph();
        graph1.valueField = "value";

        graph1.lineColor = "#" + chartData[0].colour;
        graph1.fillColor = "#" + chartData[0].colour;
        graph1.balloonText = "[[category]]: [[value]]";
        graph1.type = "column";
        graph1.lineAlpha = 0;
        graph1.fillAlphas = 0.8;
        //graph1.lineThickness = 2;
        chart1.addGraph(graph1);

        var catAxis = chart1.categoryAxis;

        catAxis.gridPosition = "start";
        catAxis.autoGridCount = true;

        //catAxis.autoGridCount = false;
        //catAxis.gridCount = 5;
        //catAxis.labelFrequency = 1;

        // chart1.addTitle("Millions $", 8);

        chart1.write(divId);
        //}
    }
};

$(function () {
    if ($('#mycarousel1') && $('#mycarousel1').length) {
        $('#mycarousel1.owl-carousel').owlCarousel({
            loop: false,
            margin: 0,
            merge: true,
            nav: true,
            touchDrag: false,
            mouseDrag: false,
            slideBy: 4,
            navText: ["<svg class='sorting-arrows__arrow sorting-arrows__arrow--down right-arrow'><use xmlns:xlink='http://www.w3.org/1999/xlink' xlink:href='/dist/img/svg-sprite.svg#sorting-arrow-table'></use></svg>", "<svg class='sorting-arrows__arrow sorting-arrows__arrow--down left-arrow'><use xmlns:xlink='http://www.w3.org/1999/xlink' xlink:href='/dist/img/svg-sprite.svg#sorting-arrow-table'></use></svg>"],
            responsive: {
                0: {
                    items: 1
                },
                678: {
                    items: 1
                },
                320: {
                    items: 1
                },
                480: {
                    items: 1
                },
                1000: {
                    items: 1
                }
            }
        });
    }
});

},{}],8:[function(require,module,exports){
'use strict';

var articleSidebarAd, articleSidebarAdParent, lastActionFlagsBar, stickyFloor, sidebarIsTaller, rightRail;
$(document).ready(function () {
    articleSidebarAdParent = $('.article-right-rail section:last-child');
    articleSidebarAd = articleSidebarAdParent.find('.advertising');
    lastActionFlagsBar = $('.action-flags-bar:last-of-type');
    sidebarIsTaller = $('.article-right-rail').height() > $('.article-left-rail').height();
    if ($('.article-right-rail').length > 0) {
        rightRail = $('.article-right-rail').offset().left;
    }
});
$(window).on('scroll', function () {
    if (articleSidebarAdParent && articleSidebarAdParent.length && !sidebarIsTaller) {
        // pageYOffset instead of scrollY for IE / pre-Edge compatibility
        stickyFloor = lastActionFlagsBar.offset().top - window.pageYOffset - articleSidebarAd.height();
        if (articleSidebarAdParent.offset().top - window.pageYOffset <= 16) {
            articleSidebarAdParent.addClass('advertising--sticky');
            if ($('.article-right-rail').length > 0) {
                articleSidebarAdParent.find('.advertising').css('left', rightRail + 'px');
            }
        } else {
            articleSidebarAdParent.removeClass('advertising--sticky');
        }
        if (stickyFloor <= 40) {
            articleSidebarAd.css('top', stickyFloor - 40 + 'px');
        } else {
            articleSidebarAd.css('top', '');
        }
    }
});

},{}],9:[function(require,module,exports){
'use strict';

(function () {
    var RecomendedContent = {
        AjaxData: function AjaxData(url, type, data, SuccessCallback) {
            console.log(JSON.stringify(data));
            $.ajax({
                url: url,
                data: JSON.stringify(data),
                dataType: 'json',
                contentType: "application/json",
                type: type,
                cache: false,
                success: function success(data) {
                    //if (data.articles && typeof data.articles === "object" && data.articles.length >= 3) {
                    SuccessCallback(data);
                    //}
                }
            });
        },
        RecomendedTemplate: function RecomendedTemplate(data) {
            var Template = '';
            if (data.articles.length > 0) {
                for (var i = 0; i < 3; i++) {
                    if (data.articles[i].listableImage == null) {
                        Template += '<div class="section-article">' + '<span class="article-related-content__category"> ' + data.articles[i].listablePublication + ' </span>' + '<h5><a class="click-utag" href="' + data.articles[i].linkableUrl + '">' + data.articles[i].listableTitle + '</a></h5>' + '<time class="article-related-content__date">' + data.articles[i].listableDate + '</time>' + '</div>';
                    } else {
                        Template += '<div class="section-article">' + '<img class="article-related-content__img" src="' + data.articles[i].listableImage + '">' + '<span class="article-related-content__category"> ' + data.articles[i].listablePublication + ' </span>' + '<h5><a class="click-utag" href="' + data.articles[i].linkableUrl + '">' + data.articles[i].listableTitle + '</a></h5>' + '<time class="article-related-content__date">' + data.articles[i].listableDate + '</time>' + '</div>';
                    }
                }
            }

            Template += '</div>';

            $('.ContentRecomm-ReadNext').append(Template);
        },
        SuggestedTemplate: function SuggestedTemplate(data) {
            var Template = '';
            if (data.articles.length > 0) {
                for (var i = 0; i < 3; i++) {
                    if (data.articles[i].listableImage == null) {
                        Template += '<div class="contentRecomm-article">' + '<span class="article-related-content__category"> ' + data.articles[i].listablePublication + ' </span>' + '<h5><a class="click-utag" href="' + data.articles[i].linkableUrl + '">' + data.articles[i].listableTitle + '</a></h5>' + '<time class="article-related-content__date">' + data.articles[i].listableDate + '</time>' + '</div>';
                    } else {
                        Template += '<div class="contentRecomm-article">' + '<img class="article-related-content__img" src="' + data.articles[i].listableImage + '">' + '<span class="article-related-content__category"> ' + data.articles[i].listablePublication + ' </span>' + '<h5><a class="click-utag" href="' + data.articles[i].linkableUrl + '">' + data.articles[i].listableTitle + '</a></h5>' + '<time class="article-related-content__date">' + data.articles[i].listableDate + '</time>' + '</div>';
                    }
                }
            }

            Template += '</div>';

            $('.suggested-article').append(Template);
        },
        init: function init() {
            var self = this;
            if ($('.ContentRecomm-ReadNext').length > 0) {
                self.AjaxData('/api/articlesearch', 'POST', { 'TaxonomyIds': $('#hdnTaxonomyIds').val().split(","), 'PageNo': 1, 'PageSize': 4 }, self.RecomendedTemplate);
            }
            if ($('#hdnPreferanceIds').val()) {
                self.AjaxData('/api/articlesearch', 'POST', { 'TaxonomyIds': $('#hdnPreferanceIds').val().split(","), 'PageNo': 1, 'PageSize': 4 }, self.SuggestedTemplate);
            }
        }
    };

    RecomendedContent.init();
})();

},{}],10:[function(require,module,exports){
'use strict';

(function (argument) {
	// body...

	$('.header_salesforce_sign-in-out').on('click', function (e) {
		e.preventDefault();
		var IframeUrl = $(this).attr('data-logout-url');
		var RelocateUrl = $(this).attr('data-redirect-url');

		// $('#hiddenforms_logout form').each(function() {
		//           $(this)[0].submit();
		//       });

		$(document.body).append('<iframe width="1000" height="1000" src="' + IframeUrl + '" frameborder="0"></iframe>');
		// var openedWindow = window.open('https://ideqa-informabi.cs82.force.com/agribusiness/secur/logout.jsp', 'popup', 'width=0,height=0,scrollbars=no');
		// openedWindow.close();
		// localStorage.setItem('RelocateUrl', RelocateUrl);

		window.location.href = RelocateUrl;
		// window.location.href = RelocateUrl;
	});

	// $(document).ready()
})();

},{}],11:[function(require,module,exports){
'use strict';

$(function () {
	$('.availableGraphs').off('click').on('click', 'li a', function () {
		var $this = $(this),
		    id = $this.attr('id'),
		    getFocusId = id.split('-')[1];
		$(window).scrollTop($('#' + getFocusId).offset().top);
		if (!$('#' + getFocusId).find('.chartexpand').hasClass('active')) {
			$('#' + getFocusId).find('.chartexpand').trigger('click');
		}
	});

	var chartAccordionIDs = ["GWP", "NWP", "UR", "NP", "SF", "NWPNR", "SFNR", "NPSFR"];
	$('.expandAll', '.compareChart').click(function () {
		var eachChartData = $('.eachChartData');
		for (var i = 0; i < chartAccordionIDs.length; i++) {
			if (!$(eachChartData[i]).find('.chartexpand').hasClass('active')) {
				$(eachChartData[i]).find('.chartexpand').click();
			}
		}
		$('.chartexpand', '.compareChart').addClass('active');
	});

	$('.collapseAll', '.compareChart').click(function () {
		if ($('.graph-container') && $('.graph-container').length) {
			$('.graph-container', '.compareChart').addClass('hide');
		}
		$('.chartData', '.compareChart').addClass('hide');
		$('.chartexpand', '.compareChart').removeClass('active');
		$('.dataTitle', '.compareChart').removeClass('act');
		$('.title', '.compareChart').removeClass('act');
	});

	$('.chartexpand', '.eachChartData').click(function () {
		var $this = $(this),
		    eachChartData = $this.closest('.eachChartData'),
		    chartexpand = eachChartData.find('.chartexpand'),
		    graphCont = eachChartData.find('.graph-container');
		if (chartexpand.hasClass('active')) {
			$this.closest('.dataTitle').removeClass('act');
			$this.closest('.dataTitle').find('.title').removeClass('act');
			graphCont.addClass('hide');
			$this.removeClass('active');
		} else {
			$this.closest('.dataTitle').addClass('act');
			$this.closest('.dataTitle').find('.title').addClass('act');
			graphCont.removeClass('hide');
			$this.addClass('active');
		}
	});

	var geturl = window.location.href;
	if (geturl.indexOf('graphid=') !== -1) {
		var getId = geturl.split('graphid=')[1];
		$(window).scrollTop($('#' + getId).offset().top);
		$('#' + getId).find('.chartexpand').trigger('click');
	}

	$('.section-article-img').each(function (idx) {
		var $this = $(this),
		    sectionArt = $this.closest('.section-article'),
		    sectionImg = sectionArt.find('.section-article-img'),
		    sectionTxt = sectionArt.find('.section-article-text');
		if ($.trim($(sectionImg).html()) !== '') {
			//sectionTxt.css('display', 'inline');
		}
	});
});

},{}],12:[function(require,module,exports){
'use strict';

(function () {
	// body...
	'use strict';

	var ResponsiveCompareTable = {
		GetAjaxData: function GetAjaxData(data, id) {
			var self = this;
			if (data) {
				window.ResponsiveJSON = data;
				window.ResponsiveModalJSON = data;
				self.RenderCarousel(data, id);
				self.RenderModal(data, id);
				//}
			}
		},
		RenderModal: function RenderModal(data, id) {
			var ModalId = $(id).attr('data-modal'),
			    Parent = $('#' + ModalId),
			    HeaderData = data[0],
			    Header = "",
			    category = "";

			Parent.find('.table').empty();
			for (var key in HeaderData) {
				if (key !== "ID") {
					Header += "<div class='tableHead'><strong>" + key + "</strong><a href='#' class='sort-modal' type='ascending'><svg class='sorting-arrows__arrow sorting-arrows__arrow--down'><use xmlns:xlink='http://www.w3.org/1999/xlink' xlink:href='/dist/img/svg-sprite.svg#sorting-arrow-table'></use></svg></a><a href='#' class='sort-modal' type='descending'><svg class='sorting-arrows__arrow sorting-arrows__arrow--down'><use xmlns:xlink='http://www.w3.org/1999/xlink' xlink:href='/dist/img/svg-sprite.svg#sorting-arrow-table'></use></svg></a></div>";
				}
			}
			Parent.find('.table').append('<div class="tableRow">' + Header + '</div>');
			for (var key in data) {
				var Item = data[key],
				    Template = "";
				for (var val in Item) {
					var content = "";
					if (val !== "ID") {
						if (Array.isArray(Item[val])) {
							content = Item[val][0].value;
						} else {
							content = Item[val];
						}
						Template += "<div class='tableCell'>" + content + "</div>";
					}
				}
				Parent.find('.table').append('<div class="tableRow">' + Template + '</div>');
			}
		},
		RenderCarousel: function RenderCarousel(data, Parent) {

			Parent.find('.owl-carousel').remove();
			Parent.find('.states_heading').find('.RB16').remove();
			Parent.find('.states_heading').after('<div class="owl-wrapper"><div class="owl-carousel"></div></div>');
			var CreateList = window.jsonMappingData;

			for (var key in CreateList) {
				Parent.find('.owl-carousel').append('<div class="article" data-head="' + CreateList[key].Key + '"><div class="year_heading"><span>' + CreateList[key].Value + '</span><a href="#" class="sort" type="ascending"><svg class="sorting-arrows__arrow sorting-arrows__arrow--down"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#sorting-arrow-table"></use></svg></a><a href="#" class="sort" type="descending"><svg class="sorting-arrows__arrow sorting-arrows__arrow--down"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#sorting-arrow-table"></use></svg></a></div></div>');
			}
			var Items = Parent.find('.owl-carousel').find('.article');

			for (var i = 0; i < data.length; i++) {
				var Item = data[i],
				    index = i;
				Parent.find('.states_heading').append('<div class="RB16"><a href="' + data[i].CompanyPageUrl + '">' + data[i].Company + '</a></div>');
				for (var key in Item) {
					if (Array.isArray(Item[key])) {
						Parent.find('.article[data-head="' + key + '"]').append('<div  class="R16 TableRow' + index + '">' + Item[key][0].value + '</div>');
					}
				}
			}
			this.InitiateCarousel(Parent);
			this.HeightManagement(Parent);
		},
		HeightManagement: function HeightManagement(Parent) {
			var HeadingItems = Parent.find('.year_heading'),
			    MaxHeadingHeight = 0;

			Parent.find('.states_heading .RB16').each(function (key) {
				var Height = $(this).height(),
				    Item = [];

				$('.article').each(function () {
					var CurrentItem = $(this).find('.R16')[key];
					Item.push(CurrentItem);
				});

				for (var i = 0; i < Item.length; i++) {
					$(Item[i]).height(Height);
				}
			});
			// HeadingItems.height(0);
			HeadingItems.each(function () {
				var thisHeight = $(this).height();
				if (thisHeight > MaxHeadingHeight) {
					MaxHeadingHeight = thisHeight;
				}
			});
			$(HeadingItems).find('span').height(MaxHeadingHeight);
		},
		InitiateCarousel: function InitiateCarousel(Parent) {

			Parent.find('.owl-carousel').owlCarousel({
				loop: false,
				margin: 0,
				merge: true,
				nav: false,
				slideBy: 4,
				responsive: {
					0: {
						items: 4
					},
					678: {
						items: 2
					},
					320: {
						items: 2
					},
					480: {
						items: 2
					},
					768: {
						items: 2
					},
					1025: {
						items: 4
					}
				}
			});
		},
		ModalEvents: function ModalEvents() {
			$(document).on('click', 'a[data-toggle="modal-comparefinancialresults"]', function (e) {
				e.preventDefault();
				$('#modal-comparefinancialresults').show();
			});
			$(document).on('click', '#modal-comparefinancialresults .table_close', function (e) {
				e.preventDefault();
				$(this).parents('.ID-responsive-table-modal').hide();
			});
			$(document).on('click', '.ID-responsive-table-modal', function (e) {
				if ($(e.target).parents('.container').length > 0 || $(e.target).hasClass('.container')) {
					return false;
				} else {
					$(this).hide();
				}
			});
		},
		SortingModal: function SortingModal(id) {
			var self = this;

			$(document).on('click', '.sort-modal', function (e) {
				e.preventDefault();
				var MainData = window.ResponsiveJSON,
				    Index = $(this).parents('.tableHead').index(),
				    Items = [],
				    type = $(this).attr('type'),
				    Category = $(this).attr('category'),
				    ModalData = window.ResponsiveModalJSON,
				    UpdatedJson = [],
				    HeadingText = $(this).parents('.tableHead').find('strong').text();

				$('#modal-comparefinancialresults .tableRow').each(function () {
					if ($(this).find('.tableCell').length > 0) {
						var Text = $($(this).find('.tableCell')[Index]).text();
						if (HeadingText == 'Company') {
							Items.push(Text);
						} else {
							Items.push(parseFloat(Text));
						}
					}
				});

				if (HeadingText == 'Company') {
					if (type == "descending") {
						Items.sort().reverse();
					} else {
						Items.sort();
					}

					for (var key in Items) {
						for (var json in ModalData) {
							if (Items[key] == ModalData[json].Company) {
								UpdatedJson.push(ModalData[json]);
							}
						}
					}
				} else {
					if (type == "descending") {
						Items.sort(function (a, b) {
							return b - a;
						});
					} else {
						Items.sort(function (a, b) {
							return a - b;
						});
					}

					for (var key in Items) {
						for (var json in ModalData) {
							if (Items[key] == ModalData[json][HeadingText][0].value) {
								UpdatedJson.push(ModalData[json]);
							}
						}
					}
				}
				window.ResponsiveModalJSON = UpdatedJson;

				self.RenderModal(window.ResponsiveModalJSON, id);
				$('.sort-modal').removeClass('active');
				$($('#modal-comparefinancialresults .tableRow .tableHead')[Index]).find('.sort-modal[type=' + type + ']').addClass('active');
			});
		},
		SortingFunctionality: function SortingFunctionality(id) {
			//Sorting Functionality
			var self = this;
			$(document).on('click', '.year_heading .sort', function (e) {
				e.preventDefault();
				var Parent = $(this).parents('.article'),
				    Values = Parent.find('.R16'),
				    Content = Parent.attr('data-head'),
				    type = $(this).attr('type'),
				    category = $(this).attr('category'),
				    Items = [],
				    CarouselControl = $(this).parents('.ID-Responsive-Table').find('.owl-controls').find('.owl-dots'),
				    ControlIndex = CarouselControl.find('.active').index(),
				    CarouselStyles = $('#comparefinancialresults .owl-stage').attr('style'),
				    OwlItems = $('#comparefinancialresults .owl-stage').find('.owl-item'),
				    ClonedItems = [],
				    ActiveItems = [];

				OwlItems.each(function () {
					if ($(this).hasClass('cloned')) {
						ClonedItems.push($(this).index());
					}
					if ($(this).hasClass('active')) {
						ActiveItems.push($(this).index());
					}
				});
				$('.year_heading .sort').removeClass('active');
				$(this).addClass('active');

				if (category == 'companies') {
					var CompanyNames = $(this).parents('.states_heading').find('.RB16');

					CompanyNames.each(function () {
						Items.push($(this).text());
					});
					if (type == "descending") {
						Items.sort().reverse();
					} else {
						Items.sort();
					}
				} else {
					Values.each(function () {
						if ($(this).length > 0) {
							Items.push(parseFloat($(this).text()));
						}
					});
					if (type == "descending") {
						Items.sort(function (a, b) {
							return b - a;
						});
					} else {
						Items.sort(function (a, b) {
							return a - b;
						});
					}
				}

				self.RecreateObject(Content, Items, window.ResponsiveJSON, id, category);

				$('#comparefinancialresults .owl-stage').attr('style', CarouselStyles);
				$('#comparefinancialresults .owl-stage .owl-item').removeClass('cloned');
				$('#comparefinancialresults .owl-stage .owl-item').removeClass('active');
				for (var key in ClonedItems) {
					$($('#comparefinancialresults .owl-stage .owl-item')[ClonedItems[key]]).addClass('cloned');
				}

				for (var key in ActiveItems) {
					$($('#comparefinancialresults .owl-stage .owl-item')[ActiveItems[key]]).addClass('active');
				}

				$('#comparefinancialresults .owl-dot').removeClass('active');
				$($('#comparefinancialresults .owl-dot')[ControlIndex]).addClass('active');
				$('#comparefinancialresults .article[data-head="' + Content + '"] .sort[type="' + type + '"]').addClass('active');
			});
		},
		RecreateObject: function RecreateObject(Content, SortedItem, MainArray, id, category, modal) {
			var self = this,
			    RecreatedArray = [];
			// id.find('.RB16').remove();
			if (category === 'companies') {
				for (var i = 0; i < SortedItem.length; i++) {
					for (var key in MainArray) {
						var Name = MainArray[key].Company;
						if (Name == SortedItem[i]) {
							RecreatedArray.push(MainArray[key]);
						}
					}
				}
			} else {
				for (var i = 0; i < SortedItem.length; i++) {
					for (var key in MainArray) {
						var _Object = MainArray[key];
						if (_Object[Content][0].value == SortedItem[i]) {
							RecreatedArray.push(_Object);
						}
					}
				}
				for (var key in MainArray) {
					var _Object = MainArray[key];
					if (_Object[Content][0].value.length === 0) {
						RecreatedArray.push(_Object);
					}
				}
			}
			// $owl.trigger('destroy.owl.carousel');

			if (modal) {
				window.ResponsiveModalJSON = RecreatedArray;
				self.RenderModal(window.ResponsiveModalJSON, id);
			} else {
				window.ResponsiveJSON = RecreatedArray;
				self.RenderCarousel(window.ResponsiveJSON, id);
			}
		},
		init: function init(data, id) {
			var self = this;
			self.GetAjaxData(data, id);
			self.ModalEvents();
			self.SortingFunctionality(id);
			self.SortingModal(id);
			$(window).resize(function () {
				self.HeightManagement(id);
			});
		}
	};

	if ($('#comparefinancialresults').length > 0) {
		ResponsiveCompareTable.init(window.jsonFinancialResultForCompare, $('#comparefinancialresults'));
	}
})();

},{}],13:[function(require,module,exports){
'use strict';

(function () {
	// body...
	'use strict';

	var ResponsiveFinancialTable = {
		LastItem: null,
		FirstItem: null,
		RenderCarousel: function RenderCarousel(data, Parent) {
			Parent.find('.owl-carousel').remove();
			Parent.find('.states_heading').parent().append('<div class="owl-wrapper"><div class="owl-carousel"></div></div>');
			var self = this,
			    Header = data[0].Header,
			    Values = data[0].Values,
			    StatesHeading = Parent.find('.states_heading'),
			    Carousel = Parent.find('.owl-carousel');

			self.FirstItem = data[0].Header[1];
			self.LastItem = data[0].Header[data[0].Header.length - 1];
			Parent.find('.states_heading').empty();
			for (var key in Header) {
				if (key == 0) {
					StatesHeading.append('<div class="year_heading">' + Header[key] + '</div>');
				} else {
					Carousel.append('<div class="article"><div class="year_heading" data-head="' + Header[key] + '">' + Header[key] + '</div></div>');
				}
			}

			for (var key in Values) {
				var CurrentValue = Values[key];
				for (var item in CurrentValue) {
					if (item == 0) {
						StatesHeading.append('<div class="RB16">' + CurrentValue[item] + '</div>');
					} else {
						$(Parent.find('.article')[item - 1]).append('<div class="R16">' + CurrentValue[item] + '</div>');
					}
				}
			}
			self.InitateCarousel(Parent);
			self.HeightManagement(Parent);
			self.ChangeStateEvents(Parent);
		},
		ChangeStateEvents: function ChangeStateEvents(Parent) {
			var OwlNext = Parent.find('.owl-next'),
			    OwlPrevious = Parent.find('.owl-previous'),
			    self = this;

			$('.owl-prev').addClass('disabled');
			$(document).on('click', '.owl-prev, .owl-next', function () {
				setTimeout(function () {
					var ActiveElements = Parent.find('.owl-item.active .year_heading'),
					    ActiveElementsTexts = [];

					ActiveElements.each(function () {
						ActiveElementsTexts.push($(this).text().trim());
					});
					$('.owl-prev, .owl-next').removeClass('disabled');
					if (self.FirstItem.trim() == ActiveElementsTexts[0]) {
						$('.owl-prev').addClass('disabled');
					} else {
						$('.owl-prev').removeClass('disabled');
					}
					if (self.LastItem.trim() == ActiveElementsTexts[ActiveElementsTexts.length - 1]) {
						$('.owl-next').addClass('disabled');
					} else {
						$('.owl-next').removeClass('disabled');
					}
				}, 400);
			});
		},
		HeightManagement: function HeightManagement(Parent) {
			Parent.find('.states_heading .RB16').each(function (key) {
				var Height = $(this).height(),
				    Item = [];

				$('.article').each(function () {
					var CurrentItem = $(this).find('.R16')[key];
					Item.push(CurrentItem);
				});

				for (var i = 0; i < Item.length; i++) {
					$(Item[i]).height(Height);
				}
			});
		},
		RenderModal: function RenderModal(data, Parent) {
			var Header = data[0].Header,
			    Values = data[0].Values,
			    FinanceModal = $('#modal-financialresults'),
			    ModalTable = FinanceModal.find('.table');

			ModalTable.append('<div class="tableRow"></div>');
			for (var key in Header) {
				ModalTable.find('.tableRow').append('<div class="tableHead"><strong>' + Header[key] + '</strong></div>');
			}

			for (var key in Values) {
				ModalTable.append('<div class="tableRow"></div>');
				var CurrentValue = Values[key];
				for (var item in CurrentValue) {
					ModalTable.find('.tableRow:last-child').append('<div class="tableCell">' + CurrentValue[item] + '</div>');
				}
			}
		},
		InitateCarousel: function InitateCarousel(Parent) {
			Parent.find('.owl-carousel').owlCarousel({
				loop: true,
				merge: true,
				margin: 0,
				nav: false,
				onDragged: function onDragged() {
					var ActiveElements = Parent.find('.owl-item.active .year_heading'),
					    ActiveElementsTexts = [];

					ActiveElements.each(function () {
						ActiveElementsTexts.push($(this).text().trim());
					});
					$('.owl-prev, .owl-next').removeClass('disabled');
					if (self.FirstItem.trim() == ActiveElementsTexts[0]) {
						$('.owl-prev').addClass('disabled');
					} else {
						$('.owl-prev').removeClass('disabled');
					}
					if (self.LastItem.trim() == ActiveElementsTexts[ActiveElementsTexts.length - 1]) {
						$('.owl-next').addClass('disabled');
					} else {
						$('.owl-next').removeClass('disabled');
					}
				},
				slideBy: 1,
				responsive: {
					0: {
						items: 3
					},
					678: {
						items: 3
					},
					320: {
						items: 2
					},
					480: {
						items: 2
					},
					1000: {
						items: 5
					}
				}
			});
		},
		ModalEvents: function ModalEvents() {
			$(document).on('click', 'a[data-toggle="modal-financialresults"]', function (e) {
				e.preventDefault();
				$('#modal-financialresults').show();
			});
			$(document).on('click', '#modal-financialresults .table_close', function (e) {
				e.preventDefault();
				$(this).parents('.ID-responsive-table-modal').hide();
			});
			$(document).on('click', '.ID-responsive-table-modal', function (e) {
				if ($(e.target).parents('.container').length > 0 || $(e.target).hasClass('.container')) {
					return false;
				} else {
					$(this).hide();
				}
			});
		},
		RenderTable: function RenderTable(data, Parent) {
			Parent.find('.states_heading').parent().remove();
			Parent.append('<div class="table-wrapper"><div class="table"></div></div>');
			var Wrapper = $('#financialresults .table'),
			    Header = data[0].Header,
			    Values = data[0].Values;

			Wrapper.append('<div class="tableRow"></div>');

			for (var key in Header) {
				Wrapper.find('.tableRow:last-child').append('<div class="tableHead">' + Header[key] + '</div>');
			}

			for (var keyItem in Values) {
				var Items = Values[keyItem];
				Wrapper.append('<div class="tableRow"></div>');
				for (var item in Items) {
					Wrapper.find('.tableRow:last-child').append('<div class="tableCell">' + Items[item] + '</div>');
				}
			}
		},
		init: function init(data, Parent) {
			var self = this,
			    windowSize = $(document).width();

			if (windowSize > 736) {
				self.RenderTable(data, Parent);
			} else {
				self.RenderCarousel(data, Parent);
			}
			self.RenderModal(data, Parent);
			self.ModalEvents();
		}

	};

	if ($('#financialresults').length > 0) {
		ResponsiveFinancialTable.init(window.jsonResultFinancial, $('#financialresults'));
	}
})();

},{}],14:[function(require,module,exports){
"use strict";

(function (argument) {
	var MergeAcquistion = {
		CurrentArray: [],
		LargeValue: [],
		MonthNames: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
		HeadingNames: ['Month', 'Acquirer', 'Target', 'TargetSector', 'TargetLocation', 'Detail', 'Price'],
		RenderDesktopVersion: function RenderDesktopVersion(data, Parent) {
			Parent.find('tbody.visible-lg').remove();
			Parent.append('<tbody class="visible-lg"></tbody>');

			var Wrapper = Parent.find('tbody.visible-lg');

			for (var key in data) {
				Wrapper.append('<tr></tr>');
				Wrapper.find('tr:last-child').append('<td align="left" deal="Month" type="date" month="' + data[key].Month + '" class="R16 pad-10">' + this.MonthNames[data[key].Month - 1] + '</td>');
				Wrapper.find('tr:last-child').append('<td align="left" deal="Acquirer" type="text" class="R16 pad-10">' + data[key].Acquirer + '</td>');
				Wrapper.find('tr:last-child').append('<td align="left" deal="Target" type="text" class="R16 pad-10">' + data[key].Target + '</td>');
				Wrapper.find('tr:last-child').append('<td align="left" deal="TargetSector" type="text" class="R16 pad-10">' + data[key].TargetSector + '</td>');
				Wrapper.find('tr:last-child').append('<td align="left" deal="TargetLocation" type="text" class="R16 pad-10">' + data[key].TargetLocation + '</td>');
				Wrapper.find('tr:last-child').append('<td align="left" deal="Detail" type="text" class="R16 pad-10">' + data[key].Detail + '</td>');
				if (data[key].Price) {
					var Price = data[key].Price;
				} else {
					Price = '-';
				}
				Wrapper.find('tr:last-child').append('<td align="right" deal="Price" type="number" class="R16 pad-10">' + Price + '</td>');
			}
		},
		RenderMobileVersion: function RenderMobileVersion(data, Parent) {
			Parent.find('tbody.visible-xs').remove();
			Parent.append('<tbody class="visible-xs"></tbody>');

			var Wrapper = Parent.find('tbody.visible-xs'),
			    Headings = this.HeadingNames;

			for (var key in data) {
				Wrapper.append('<tr>' + '<td width="50%" class="boder-none"><hr class="m-0"/></td>' + '<td width="50%" class="boder-none"><hr class="m-0"/></td>' + '</tr>');
				for (var j in Headings) {
					if (Headings[j] == 'Month') {
						Wrapper.append('<tr>' + '<td class="pad-10" width="50%">' + Headings[j] + '</td>' + '<td class="pad-10" width="50%">' + this.MonthNames[data[key][Headings[j]] - 1] + '</td>' + '</tr>');
					} else if (Headings[j] == 'Price') {
						var Price = "";
						if (data[key][Headings[j]].length > 0) {
							Price = data[key][Headings[j]];
						} else {
							Price = '-';
						}
						Wrapper.append('<tr>' + '<td class="pad-10" width="50%">' + Headings[j] + '</td>' + '<td class="pad-10" width="50%">' + Price + '</td>' + '</tr>');
					} else {
						Wrapper.append('<tr>' + '<td class="pad-10" width="50%">' + Headings[j] + '</td>' + '<td class="pad-10" width="50%">' + data[key][Headings[j]] + '</td>' + '</tr>');
					}
				}
				Wrapper.append('<tr>' + '<td width="50%" class="boder-none"><hr/></td>' + '<td width="50%" class="boder-none"><hr/></td>' + '</tr>');
			}
		},
		SortingEvent: function SortingEvent(data, Parent) {
			var self = this;
			$(document).on('click', '.sorting-buttons a', function (e) {
				e.preventDefault();
				var Type = $(this).attr('type'),
				    Category = $(this).attr('category'),
				    Index = $(this).parents('th').index(),
				    SortingArray = [],
				    MyArray = self.CurrentArray,
				    Body = Parent.find('tbody.visible-lg'),
				    AppendToEndElements = [],
				    SortedElements = [],
				    SortingType = $(this).parents('.sorting-buttons').attr('deal');

				//Fetching Elements
				Body.find('tr').each(function (key) {
					var Text = "";
					if (Category === 'month') {
						SortingArray.push(parseInt($($(this).find('td')[Index]).attr('month')));
					} else if (Category === 'number') {
						var Num = $($(this).find('td')[Index]).text();
						if (Num != '-') {
							SortingArray.push(parseFloat(Num));
						} else {
							AppendToEndElements.push(self.CurrentArray[key]);
						}
					} else {
						if ($($(this).find('td')[Index]).text().includes('<a href=')) {
							Text = $($(this).find('td')[Index]).find('a').text();
						} else {
							Text = $($(this).find('td')[Index]).text();
						}
						SortingArray.push(Text);
					}
				});
				console.log(SortingArray);
				if (Category === 'month' || Category === 'number') {
					if (Type === 'ascending') {
						SortingArray.sort(function (a, b) {
							return a - b;
						});
					} else {
						SortingArray.sort(function (a, b) {
							return b - a;
						});
					}
				} else {
					if (Type === 'ascending') {
						SortingArray.sort();
					} else {
						SortingArray.sort().reverse();
					}
				}
				console.log(SortingArray);
				SortedElements = [];
				var CurrentItem = self.CurrentArray;
				for (var i in SortingArray) {
					for (var j in CurrentItem) {
						if (CurrentItem[j] != undefined) {
							if (SortingArray[i] == CurrentItem[j][SortingType]) {
								SortedElements.push(CurrentItem[j]);
								CurrentItem = CurrentItem.filter(function (item, index) {
									return CurrentItem[index] !== CurrentItem[j];
								});
							}
						}
					}
				}
				//if(AppendToEndElements.length > 0) {
				var NewArray = SortedElements.concat(AppendToEndElements);
				//}
				// var UniqueArray = [];
				// for(var k = 0; k < SortedElements.length; k++) {
				// 	if(!SortedElements.contains(SortedElements[k])) {
				// 		UniqueArray.push(SortedElements[k]);
				// 	}
				// }
				self.CurrentArray = [];
				self.CurrentArray = NewArray;
				self.RenderDesktopVersion(self.CurrentArray, $('.merge-acquistion'));
				self.RenderMobileVersion(self.CurrentArray, $('.merge-acquistion'));
			});
		},
		FilterEvent: function FilterEvent(data, Parent) {
			var InputValues = Parent.find('th').find('input'),
			    self = this;
			if ($(window).width() < 668) {
				InputValues = $('.merge-form-items input');
			}
			InputValues.on('keyup', function () {
				var textFieldValue = $(this).val(),
				    DealType = $(this).attr('deal'),
				    Index = $(this).parents('th').index(),
				    Body = Parent.find('tbody.visible-lg'),
				    regExp = new RegExp(textFieldValue, "i"),
				    ItemArray = [],
				    FilteredArray = [],
				    Obj = {},
				    StartField = Parent.find('.range-field')[0].value,
				    EndField = Parent.find('.range-field')[1].value;

				if ($(window).width() < 668) {
					Index = $(this).parents('.forms').index();
					StartField = $('.merge-form-items .range-field.start').val();
					EndField = $('.merge-form-items .range-field.end').val();
				}
				InputValues.each(function (key) {
					var DealType = $(this).attr('deal');
					if ($(this).val()) {
						if (DealType != 'Price') {
							Obj[DealType] = $(this).val();
						} else {
							var Start, End;
							if (StartField) {
								Start = parseFloat(StartField);
							} else {
								Start = 0;
							}
							if (EndField) {
								End = parseFloat(EndField);
							} else {
								End = null;
							}
							Obj[DealType] = {
								Start: Start,
								End: End
							};
							// Obj[DealType]['End'] =
						}
					}
				});

				// Body.find('tr').each(function(key) {
				// 	ItemArray.push($($(this).find('td')[Index]).text());
				// });

				//for(var i in ItemArray) {
				// if(regExp.test(ItemArray[i])) {
				// 	for(var j = 0; j < window.jsonMergeAcquistion.length; j++) {
				// 		if(ItemArray[i] == window.jsonMergeAcquistion[j][DealType]) {
				// 			FilteredArray.push(window.jsonMergeAcquistion[j]);
				// 		}
				// 	}
				// }
				//}
				if (Object.keys(Obj).length > 0) {
					for (var i in window.jsonMergeAcquistion) {
						var count = 0;
						for (var j in Obj) {
							var text = "";
							if (j == 'Price') {
								var Price = parseFloat(window.jsonMergeAcquistion[i][j]);

								if (Obj[j]['End'] != null) {
									if (Price >= Obj[j]['Start'] && Price <= Obj[j]['End']) {
										count++;
									}
								} else {
									if (Price >= Obj[j]['Start']) {
										count++;
									}
								}
							} else if (j == 'Month') {
								var MonthValue = window.jsonMergeAcquistion[i][j] - 1;
								if (self.MonthNames[MonthValue].toLowerCase().includes(Obj[j].toLowerCase())) {
									count++;
								}
							} else {
								if (window.jsonMergeAcquistion[i][j].includes('<a href=')) {
									text = $(window.jsonMergeAcquistion[i][j]).text();
								} else {
									text = window.jsonMergeAcquistion[i][j];
								}
								if (text.toLowerCase().includes(Obj[j].toLowerCase())) {
									count++;
								}
							}
						}

						if (count === Object.keys(Obj).length) {
							FilteredArray.push(window.jsonMergeAcquistion[i]);
						}
					}
				} else {
					FilteredArray = window.jsonMergeAcquistion;
				}
				self.CurrentArray = FilteredArray;
				self.RenderDesktopVersion(self.CurrentArray, $('.merge-acquistion'));
				self.RenderMobileVersion(self.CurrentArray, $('.merge-acquistion'));
			});
		},
		YearChange: function YearChange() {
			$(document).on('change', '.idYearSelect', function () {
				var Href = $(this).attr('data-href'),
				    value = $(this).find('.selectivity-single-selected-item').attr('data-item-id'),
				    newUrl = window.location.href.split('?')[0].concat("?year=" + value);

				window.location = newUrl;
			});
		},
		MobileEvent: function MobileEvent() {
			$(document).on('click', '.merge-acordian', function (e) {
				e.preventDefault();
				$(this).parents('.merges-form').toggleClass('open');
			});
		},
		showLargestEvent: function showLargestEvent() {
			var self = this;
			$(document).on('click', '.show-largest-btn', function (e) {
				e.preventDefault();
				var ThresholdValue = $(this).attr('data-large'),
				    Items = [],
				    PriceItem = [];

				if ($(this).hasClass('active')) {
					self.RenderDesktopVersion(self.CurrentArray, $('.merge-acquistion'));
					self.RenderMobileVersion(self.CurrentArray, $('.merge-acquistion'));
				} else {
					for (var i = 0; i < self.CurrentArray.length; i++) {
						if (self.CurrentArray[i].Price != '-') {
							if (parseFloat(self.CurrentArray[i].Price) > parseFloat(ThresholdValue)) {
								PriceItem.push(parseFloat(self.CurrentArray[i].Price));
							}
						}
					}
					PriceItem.sort(function (a, b) {
						return b - a;
					});
					for (var i = 0; i < PriceItem.length; i++) {
						for (var j = 0; j < self.CurrentArray.length; j++) {
							if (self.CurrentArray[j].Price == PriceItem[i]) {
								Items.push(self.CurrentArray[j]);
							}
						}
					}
					self.RenderDesktopVersion(Items, $('.merge-acquistion'));
					self.RenderMobileVersion(Items, $('.merge-acquistion'));
				}

				$(this).toggleClass('active');
			});
		},
		init: function init(data, Parent) {
			this.CurrentArray = data;
			this.RenderDesktopVersion(data, Parent);
			this.RenderMobileVersion(data, Parent);
			this.SortingEvent(data, Parent);
			this.FilterEvent(data, Parent);
			this.YearChange();
			this.MobileEvent();
			this.showLargestEvent();
		}
	};

	if ($('.merge-acquistion').length > 0) {
		MergeAcquistion.init(window.jsonMergeAcquistion, $('.merge-acquistion'));
	}
})();

},{}],15:[function(require,module,exports){
'use strict';

(function () {
	// body...
	'use strict';

	var ResponsiveFinancialTable = {
		RenderTable: function RenderTable(data, Parent) {
			var Tables = $('#quarterlyresults, #modal-quarterlyresults'),
			    QuaterlyDataHeader = data[0].QuaterlyDataHeader,
			    QuaterlyData = data[0].QuaterlyData,
			    QuaterlyResultHeader = data[0].QuaterlyResultHeader,
			    QuaterlyResult = data[0].QuaterlyResultData[0];

			$('#quarterlyresults').find('.states_heading').parent().remove();
			$('#quarterlyresults').append('<div class="table-wrapper"><div class="table"></div></div>');
			var Wrapper = $('#quarterlyresults .table, #modal-quarterlyresults .table');

			Wrapper.append('<div class="tableRow"></div>');
			for (var key in QuaterlyDataHeader) {
				Wrapper.find('.tableRow:last-child').append('<div class="tableHead">' + QuaterlyDataHeader[key] + '</div>');
			}

			for (var key in QuaterlyData) {
				Wrapper.append('<div class="tableRow"></div>');
				var Content = QuaterlyData[key];
				for (var item in Content) {
					Wrapper.find('.tableRow:last-child').append('<div class="tableCell">' + Content[item] + '</div>');
				}
			}

			Wrapper.append('<div class="tableRow"></div>');
			for (var key in QuaterlyResultHeader) {
				Wrapper.find('.tableRow:last-child').append('<div class="tableHead">' + QuaterlyResultHeader[key] + '</div>');
			}

			Wrapper.append('<div class="tableRow"></div>');
			for (var key in QuaterlyResult) {
				Wrapper.find('.tableRow:last-child').append('<div class="tableCell">' + QuaterlyResult[key] + '</div>');
			}
		},
		ModalEvents: function ModalEvents() {
			$(document).on('click', 'a[data-toggle="modal-quarterlyresults"]', function (e) {
				e.preventDefault();
				$('#modal-quarterlyresults').show();
			});
			$(document).on('click', '#modal-quarterlyresults .table_close', function (e) {
				e.preventDefault();
				$(this).parents('.ID-responsive-table-modal').hide();
			});
			$(document).on('click', '.ID-responsive-table-modal', function (e) {
				if ($(e.target).parents('.container').length > 0 || $(e.target).hasClass('.container')) {
					return false;
				} else {
					$(this).hide();
				}
			});
		},
		init: function init(data, Parent) {
			var self = this;
			self.RenderTable(data, Parent);
			self.ModalEvents();
		}

	};

	if ($('#quarterlyresults').length > 0) {
		ResponsiveFinancialTable.init(window.jsonResultQuarterly, $('#quarterlyresults'));
	}
})();

},{}],16:[function(require,module,exports){
'use strict';

(function () {
	// body...
	'use strict';

	var ResponsiveTable = {
		GetAjaxData: function GetAjaxData(data, id) {
			var self = this;
			if (data) {
				window.ResponsiveJSON = data;
				window.ResponsiveModalJSON = data;
				self.RenderCarousel(data, id);
				self.RenderModal(data, id);
				//}
			}
		},
		RenderModal: function RenderModal(data, id) {
			var ModalId = $(id).attr('data-modal'),
			    Parent = $('#' + ModalId),
			    HeaderData = data[0],
			    Header = "",
			    category = "";

			Parent.find('.table').empty();
			for (var key in HeaderData) {
				if (key !== "ID") {
					if (key !== "CompanyPageUrl") {
						Header += "<div class='tableHead'><strong>" + key + "</strong><a href='#' class='sort-modal' type='ascending'><svg class='sorting-arrows__arrow sorting-arrows__arrow--down'><use xmlns:xlink='http://www.w3.org/1999/xlink' xlink:href='/dist/img/svg-sprite.svg#sorting-arrow-table'></use></svg></a><a href='#' class='sort-modal' type='descending'><svg class='sorting-arrows__arrow sorting-arrows__arrow--down'><use xmlns:xlink='http://www.w3.org/1999/xlink' xlink:href='/dist/img/svg-sprite.svg#sorting-arrow-table'></use></svg></a></div>";
					}
				}
			}
			Parent.find('.table').append('<div class="tableRow">' + Header + '</div>');
			for (var key in data) {
				var Item = data[key],
				    Template = "";
				for (var val in Item) {
					var content = "";
					if (val !== "ID") {
						if (val !== "CompanyPageUrl") {
							if (Array.isArray(Item[val])) {
								content = Item[val][0].value;
							} else {
								content = Item[val];
							}
							Template += "<div class='tableCell'>" + content + "</div>";
						}
					}
				}
				Parent.find('.table').append('<div class="tableRow">' + Template + '</div>');
			}
		},
		RenderCarousel: function RenderCarousel(data, Parent) {

			Parent.find('.owl-carousel').remove();
			Parent.find('.states_heading').find('.RB16').remove();
			Parent.find('.states_heading').after('<div class="owl-wrapper"><div class="owl-carousel"></div></div>');
			var CreateList = window.jsonMappingData;

			for (var key in CreateList) {
				Parent.find('.owl-carousel').append('<div class="article" data-head="' + CreateList[key].Key + '"><div class="year_heading"><span>' + CreateList[key].Value + '</span><a href="#" class="sort" type="ascending"><svg class="sorting-arrows__arrow sorting-arrows__arrow--down"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#sorting-arrow-table"></use></svg></a><a href="#" class="sort" type="descending"><svg class="sorting-arrows__arrow sorting-arrows__arrow--down"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#sorting-arrow-table"></use></svg></a></div></div>');
			}
			var Items = Parent.find('.owl-carousel').find('.article');

			for (var i = 0; i < data.length; i++) {
				var Item = data[i],
				    index = i;
				Parent.find('.states_heading').append('<div class="RB16"><a href="' + data[i].CompanyPageUrl + '">' + data[i].Company + '</a></div>');
				for (var key in Item) {
					if (Array.isArray(Item[key])) {
						Parent.find('.article[data-head="' + key + '"]').append('<div  class="R16 TableRow' + index + '">' + Item[key][0].value + '</div>');
					}
				}
			}
			this.InitiateCarousel(Parent);
			this.HeightManagement(Parent);
		},
		HeightManagement: function HeightManagement(Parent) {
			var HeadingItems = Parent.find('.year_heading'),
			    MaxHeadingHeight = 0;

			Parent.find('.states_heading .RB16').each(function (key) {
				var Height = $(this).height(),
				    Item = [];

				$('.article').each(function () {
					var CurrentItem = $(this).find('.R16')[key];
					Item.push(CurrentItem);
				});

				for (var i = 0; i < Item.length; i++) {
					$(Item[i]).height(Height);
				}
			});
			// HeadingItems.height(0);
			HeadingItems.each(function () {
				var thisHeight = $(this).height();
				if (thisHeight > MaxHeadingHeight) {
					MaxHeadingHeight = thisHeight;
				}
			});
			$(HeadingItems).find('span').height(MaxHeadingHeight);
		},
		InitiateCarousel: function InitiateCarousel(Parent) {

			Parent.find('.owl-carousel').owlCarousel({
				loop: false,
				margin: 0,
				merge: true,
				nav: false,
				slideBy: 4,
				responsive: {
					0: {
						items: 4
					},
					678: {
						items: 2
					},
					320: {
						items: 2
					},
					480: {
						items: 2
					},
					1000: {
						items: 4
					}
				}
			});
		},
		ModalEvents: function ModalEvents() {
			$(document).on('click', 'a[data-toggle="modal-annualresults"]', function (e) {
				e.preventDefault();
				$('#modal-annualresults').show();
			});
			$(document).on('click', '#modal-annualresults .table_close', function (e) {
				e.preventDefault();
				$(this).parents('.ID-responsive-table-modal').hide();
			});
			$(document).on('click', '.ID-responsive-table-modal', function (e) {
				if ($(e.target).parents('.container').length > 0 || $(e.target).hasClass('.container')) {
					return false;
				} else {
					$(this).hide();
				}
			});
		},
		SortingModal: function SortingModal(id) {
			var self = this;

			$(document).on('click', '.sort-modal', function (e) {
				e.preventDefault();
				var MainData = window.ResponsiveJSON,
				    Index = $(this).parents('.tableHead').index(),
				    Items = [],
				    type = $(this).attr('type'),
				    Category = $(this).attr('category'),
				    ModalData = window.ResponsiveModalJSON,
				    UpdatedJson = [],
				    HeadingText = $(this).parents('.tableHead').find('strong').text();

				$('#modal-annualresults .tableRow').each(function () {
					if ($(this).find('.tableCell').length > 0) {
						var Text = $($(this).find('.tableCell')[Index]).text();
						if (HeadingText == 'Company') {
							Items.push(Text);
						} else {
							Items.push(parseFloat(Text));
						}
					}
				});

				if (HeadingText == 'Company') {
					if (type == "descending") {
						Items.sort().reverse();
					} else {
						Items.sort();
					}

					for (var key in Items) {
						for (var json in ModalData) {
							if (Items[key] == ModalData[json].Company) {
								UpdatedJson.push(ModalData[json]);
							}
						}
					}
				} else {
					if (type == "descending") {
						Items.sort(function (a, b) {
							return b - a;
						});
					} else {
						Items.sort(function (a, b) {
							return a - b;
						});
					}

					for (var key in Items) {
						for (var json in ModalData) {
							if (Items[key] == ModalData[json][HeadingText][0].value) {
								UpdatedJson.push(ModalData[json]);
							}
						}
					}
				}
				window.ResponsiveModalJSON = UpdatedJson;

				self.RenderModal(window.ResponsiveModalJSON, id);
				$('.sort-modal').removeClass('active');
				$($('#modal-annualresults .tableRow .tableHead')[Index]).find('.sort-modal[type=' + type + ']').addClass('active');
			});
		},
		SortingFunctionality: function SortingFunctionality(id) {
			//Sorting Functionality
			var self = this;
			$(document).on('click', '.year_heading .sort', function (e) {
				e.preventDefault();
				var Parent = $(this).parents('.article'),
				    Values = Parent.find('.R16'),
				    Content = Parent.attr('data-head'),
				    type = $(this).attr('type'),
				    category = $(this).attr('category'),
				    Items = [],
				    CarouselControl = $(this).parents('.ID-Responsive-Table').find('.owl-controls').find('.owl-dots'),
				    ControlIndex = CarouselControl.find('.active').index(),
				    CarouselStyles = $('#annualresults .owl-stage').attr('style'),
				    OwlItems = $('#annualresults .owl-stage').find('.owl-item'),
				    ClonedItems = [],
				    ActiveItems = [];

				OwlItems.each(function () {
					if ($(this).hasClass('cloned')) {
						ClonedItems.push($(this).index());
					}
					if ($(this).hasClass('active')) {
						ActiveItems.push($(this).index());
					}
				});
				$('.year_heading .sort').removeClass('active');
				$(this).addClass('active');

				if (category == 'companies') {
					var CompanyNames = $(this).parents('.states_heading').find('.RB16');

					CompanyNames.each(function () {
						Items.push($(this).text());
					});
					if (type == "descending") {
						Items.sort().reverse();
					} else {
						Items.sort();
					}
				} else {
					Values.each(function () {
						if ($(this).length > 0) {
							Items.push(parseFloat($(this).text()));
						}
					});
					if (type == "descending") {
						Items.sort(function (a, b) {
							return b - a;
						});
					} else {
						Items.sort(function (a, b) {
							return a - b;
						});
					}
				}

				self.RecreateObject(Content, Items, window.ResponsiveJSON, id, category);

				$('#annualresults .owl-stage').attr('style', CarouselStyles);
				$('#annualresults .owl-stage .owl-item').removeClass('cloned');
				$('#annualresults .owl-stage .owl-item').removeClass('active');
				for (var key in ClonedItems) {
					$($('#annualresults .owl-stage .owl-item')[ClonedItems[key]]).addClass('cloned');
				}

				for (var key in ActiveItems) {
					$($('#annualresults .owl-stage .owl-item')[ActiveItems[key]]).addClass('active');
				}

				$('#annualresults .owl-dot').removeClass('active');
				$($('#annualresults .owl-dot')[ControlIndex]).addClass('active');
				$('#annualresults .article[data-head="' + Content + '"] .sort[type="' + type + '"]').addClass('active');
			});
		},
		RecreateObject: function RecreateObject(Content, SortedItem, MainArray, id, category, modal) {
			var self = this,
			    RecreatedArray = [];
			// id.find('.RB16').remove();
			if (category === 'companies') {
				for (var i = 0; i < SortedItem.length; i++) {
					for (var key in MainArray) {
						var Name = MainArray[key].Company;
						if (Name == SortedItem[i]) {
							RecreatedArray.push(MainArray[key]);
						}
					}
				}
			} else {
				for (var i = 0; i < SortedItem.length; i++) {
					for (var key in MainArray) {
						var _Object = MainArray[key];
						if (_Object[Content][0].value == SortedItem[i]) {
							RecreatedArray.push(_Object);
						}
					}
				}
				for (var key in MainArray) {
					var _Object = MainArray[key];
					if (_Object[Content][0].value.length === 0) {
						RecreatedArray.push(_Object);
					}
				}
			}
			// $owl.trigger('destroy.owl.carousel');

			if (modal) {
				window.ResponsiveModalJSON = RecreatedArray;
				self.RenderModal(window.ResponsiveModalJSON, id);
			} else {
				window.ResponsiveJSON = RecreatedArray;
				self.RenderCarousel(window.ResponsiveJSON, id);
			}
		},
		init: function init(data, id) {
			var self = this;
			self.GetAjaxData(data, id);
			self.ModalEvents();
			self.SortingFunctionality(id);
			self.SortingModal(id);
		}
	};

	if ($('#annualresults').length > 0) {
		ResponsiveTable.init(window.jsonResultAnnual, $('#annualresults'));
	}
})();

$(document).on('mouseenter', '.ID-Responsive-Table .R16, .ID-Responsive-Table .RB16', function () {
	var Index = $(this).index();
	$($('.states_heading .RB16')[Index - 1]).addClass('activate-hover');
	$('.owl-item').each(function () {
		$($(this).find('.R16')[Index - 1]).addClass('activate-hover');
	});
});

$(document).on('mouseleave', '.ID-Responsive-Table .R16, .ID-Responsive-Table .RB16', function () {
	$('.R16, .RB16').removeClass('activate-hover');
});

},{}],17:[function(require,module,exports){
'use strict';

(function (argument) {
	var LatestCasuality = {
		RenderLinks: function RenderLinks(data, Parent) {
			var latestcasualties = data[0].latestcasualties,
			    Html = "";

			for (var key in latestcasualties) {
				Html += '<li class="article-topics__li"><a href="' + $('#CasualtyDetailPageUrl').val() + '?incidentId=' + latestcasualties[key].IncidentId + '"><strong>' + latestcasualties[key].title + '</strong> - ' + latestcasualties[key].date + ' </a></li>';
			}
			Parent.find('ul').append(Html);
			//<li class="article-topics__li"><a href="#"><strong>MCC Shanghai</strong> - 01.01.2017</a></li>
		},
		init: function init(data, parent) {
			this.RenderLinks(data, parent);
		}
	};

	if ($('.lloyd-related-links').length > 0) {
		LatestCasuality.init(window.jsonLatestCasualties, $('.lloyd-related-links'));
	}
})();

},{}],18:[function(require,module,exports){
'use strict';

(function () {
	var CasualityDetail = {
		RenderTable: function RenderTable(data, Parent) {
			var Data = data[0].casualtyDetail;

			Parent.append('<thead class="table_head">' + '<tr>' + '<th colspan="2" class="p-10">' + Data.Heading + '</th>' + '</tr>' + '</thead>');
			Parent.append('<tbody></tbody>');
			var Body = Parent.find('tbody');
			for (var key in Data) {
				if (key != 'Heading') {
					if (Array.isArray(Data[key])) {
						if (key == 'Messages') {
							var StrMsg = "",
							    Messages = Data[key];
							for (var i in Messages) {
								StrMsg += "<p><strong>" + Messages[i].date + "</strong>" + Messages[i].message + "</p>";
							}
							Body.append('<tr>' + '<td class="R16">' + key + '</td>' + '<td class="R16">' + StrMsg + '</td>' + '</tr>');
						}
					} else {
						Body.append('<tr>' + '<td class="R16">' + key + '</td>' + '<td class="R16">' + Data[key] + '</td>' + '</tr>');
					}
				}
			}
		},
		init: function init(data, Parent) {
			this.RenderTable(data, Parent);
		}
	};

	$(document).ready(function () {
		if ($('#casualty-detail-table').length > 0) {
			CasualityDetail.init(window.jsonCasualtyDetailData, $('#casualty-detail-table'));
		}
	});
})();

},{}],19:[function(require,module,exports){
'use strict';

(function () {
	var CasualityListing = {
		HeaderLinks: [],
		JumpToArray: [],
		DesktopVersion: function DesktopVersion(data, Parent) {
			//Header
			Parent.append('<thead class="table_head"></thead>');

			var Header = Parent.find('thead.table_head'),
			    HeaderItems = this.HeaderLinks,
			    self = this;
			self.JumpToArray = [];
			Header.append('<tr class="visible-lg"></tr>');

			for (var headItem in HeaderItems) {
				Header.find('tr').append('<th class="p-10">' + HeaderItems[headItem] + '</th>');
			}
			//Body
			Parent.append('<tbody class="visible-lg"></tbody>');

			var Wrapper = Parent.find('tbody.visible-lg');
			for (var key in data) {
				//Appending Heading
				$('#jumpTo').append('<option value="' + data[key].casualtytitle + '">' + data[key].casualtytitle + '</option>');
				self.JumpToArray.push(data[key].casualtytitle);
				Wrapper.append('<tr data-jump="' + data[key].casualtytitle + '"><td colspan="2" class="graybg RB18 p-10"> ' + data[key].casualtytitle + '</td><td colspan="1" align="right" class="graybg RB18 p-10"><a href="javascript: void(0);" class="top"><span class="arrow"></span>Top</a></td></tr>');

				//Appending Body
				var CasualityData = data[key].casualtyData;
				for (var item in CasualityData) {
					Wrapper.append('<tr><td class="RB16 pad-10"><a href="' + $('#casualtyDetailUrl').val() + '?incidentId=' + CasualityData[item].incidentId + '">' + CasualityData[item].Title + '</a></td><td class="R16 pad-10">' + CasualityData[item]["Date of Incident"] + '</td><td class="R16 pad-10">' + CasualityData[item]["Area"] + '</td></tr>');
				}
			}
		},
		MobileVersion: function MobileVersion(data, Parent) {
			Parent.append('<tbody class="visible-sm"></tbody>');

			var Wrapper = Parent.find('tbody.visible-sm');
			for (var key in data) {
				Wrapper.append('<tr data-jump="' + data[key].casualtytitle + '"><td class="graybg RB18 pad-full-10">' + data[key].casualtytitle + '</td><td align="right" class="graybg RB18 p-10"><a class="top" href="javascript: void(0);"><span class="arrow"></span>Top</a></td></tr>');

				var HeaderItems = this.HeaderLinks;
				var CasualData = data[key].casualtyData;

				for (var key in CasualData) {
					for (var i in HeaderItems) {
						if (HeaderItems[i] == 'Title') {
							Wrapper.append('<tr>' + '<td class="pad-10 R21_GrayColor border-right">' + HeaderItems[i] + '</td>' + '<td class="pad-10 R21_GrayColor"><a href="' + $('#casualtyDetailUrl').val() + '?incidentId=' + CasualData[key].incidentId + '">' + CasualData[key][HeaderItems[i]] + '</a></td>' + '</tr>');
						} else {
							Wrapper.append('<tr>' + '<td class="pad-10 R21_GrayColor border-right">' + HeaderItems[i] + '</td>' + '<td class="pad-10 R21_GrayColor">' + CasualData[key][HeaderItems[i]] + '</td>' + '</tr>');
						}
					}
					Wrapper.append('<tr>' + '<td><hr /></td>' + '<td><hr /></td>' + '</tr>');
				}
			}
		},
		RenderTable: function RenderTable(data, Parent) {
			Parent.empty();
			this.DesktopVersion(data, Parent);
			this.MobileVersion(data, Parent);
			$('table').on('click', 'a.top', function () {
				var $this = $(this),
				    table = $this.closest('table'),
				    tablePos = table.offset().top;
				if (window.matchMedia("(max-width: 400px)").matches) {
					$(window).scrollTop(tablePos - 40);
				} else {
					$(window).scrollTop(tablePos);
				}
			});
		},
		FindHeaderLinks: function FindHeaderLinks(data) {
			for (var key in data) {
				var CasualityData = data[key].casualtyData;
				for (var item in CasualityData) {
					var List = CasualityData[item];
					for (var list in List) {
						if (list != "incidentId") {
							this.HeaderLinks.push(list);
						}
					}
					break;
				}
				break;
			}
		},
		ChangeReport: function ChangeReport() {
			var self = this;
			$(document).on('change', '#relDate', function () {
				var Value = $(this).find('.selectivity-single-selected-item').attr('data-item-id');
				if (window.jsonCasualtyListing[0][Value] != undefined) {
					self.RenderTable(window.jsonCasualtyListing[0][Value], $('#casualty-listing-table'));
					$('#casualty-listing-table').show();
				} else {
					$('#casualty-listing-table').hide();
				}

				if ($('.jumpToSection #jumpTo')) {
					$('.jumpToSection #jumpTo').remove();
				}
				$('.jumpToSection').append('<select name="jumpTo" id="jumpTo" class="common-field inline"></select>');

				for (var i = 0; i < self.JumpToArray.length; i++) {
					$('#jumpTo').append('<option value="' + self.JumpToArray[i] + '">' + self.JumpToArray[i] + '</option>');
				}
				$('#jumpTo').selectivity({
					showSearchInputInDropdown: false
				});

				$(".selectivity-input .selectivity-single-select").each(function () {
					$(this).append('<span class="selectivity-arrow"><svg class="alert__icon"><use xlink:href="/dist/img/svg-sprite.svg#sort-down-arrow"></use></svg></span>');
				});
			});
			$(document).on('change', '#jumpTo', function () {
				var Value = $(this).find('.selectivity-single-selected-item').attr('data-item-id');
				var Top = $('#casualty-listing-table tr[data-jump=' + Value + ']').offset().top;

				window.scrollTo(0, Top);
			});
		},
		init: function init(data, Parent) {
			var FirstValue = $('#relDate').val(),
			    CurrentObj = data[0][FirstValue];

			this.FindHeaderLinks(CurrentObj);
			this.RenderTable(CurrentObj, Parent);
			this.ChangeReport();
		}
	};

	$(document).ready(function () {
		if ($('#casualty-listing-table').length > 0) {
			CasualityListing.init(window.jsonCasualtyListing, $('#casualty-listing-table'));
		}
	});
})();

},{}],20:[function(require,module,exports){
"use strict";

(function () {
	// body...
	'use strict';

	var cockettBunker = {
		RenderTable: function RenderTable(data, Parent) {
			var self = this,
			    TableStr = "";
			Parent.empty();

			for (var key in data[0]) {
				TableStr += self.RenderSingleTable(data[0][key]);
			}

			Parent.append(TableStr);
		},
		RenderSingleTable: function RenderSingleTable(Data) {
			console.log(Data);
			var HeadingStr = "",
			    SubHeadingStr = "",
			    TbodyStr = "",
			    Heading = Data[0];

			for (var key in Heading) {
				if (key.split("|").length === 1) {
					HeadingStr += "<th class='pad-full-10' colspan='1'>" + "<div class='text-center'>&nbsp;</div>" + "<div class='text-center'></div>" + "</th>";
				} else {
					HeadingStr += "<th class='pad-full-10' colspan='1'>" + "<div class='text-center'>" + key.split("|")[0] + "</div>" + "<div class='text-center'>" + key.split("|")[1] + "</div>" + "</th>";
				}
			}

			for (var i = 0; i < Data.length; i++) {
				var EachValue = Data[i],
				    Td = "",
				    align = "right";
				for (var key in EachValue) {
					if (key == "Header") {
						align = "left";
					}
					Td += '<td colspan="1" class="pad-full-10" align="' + align + '">' + EachValue[key] + '</td>';
				}
				TbodyStr += '<tr>' + Td + '</tr>';
			}

			var Table = '<table class="table">' + '<thead class="table_head">' + '<tr>' + HeadingStr + '</tr>' + '</thead>' + '<tbody>' + TbodyStr + '</tbody>' + '</table>';

			return Table;
		},
		init: function init(data, id) {
			var self = this;
			self.RenderTable(data, id);
		}
	};

	if ($('#cockettBunker').length > 0) {
		cockettBunker.init(window.jsonCockettBunker, $('#cockettBunker'));
	}
})();

},{}],21:[function(require,module,exports){
'use strict';

(function () {
	var dryCargoBulkFixtures = {
		table: '',
		recreateObj: {},
		renderDate: function renderDate() {
			var options = '';
			$.each(dateObj[0], function (key, val) {
				$.each(val, function (idx, value) {
					options += '<option value="' + value + '">' + value + '</option>';
				});
			});

			$('#selectDay').html(options);
		},
		renderTable: function renderTable() {
			var tableStr = '';
			tableStr += this.loadDescView();
			tableStr += this.loadMobileView();

			$('#dryCargoBulkFixtures table').append(tableStr);

			$('table').on('click', 'a.top', function () {
				var $this = $(this),
				    table = $this.closest('table'),
				    tablePos = table.offset().top;
				if (window.matchMedia("(max-width: 400px)").matches) {
					$(window).scrollTop(tablePos - 40);
				} else {
					$(window).scrollTop(tablePos);
				}
			});
		},
		loadDescView: function loadDescView() {
			var self = this,
			    getArr;
			$.each(tableObj[0], function (key, val) {
				self.table += '<table class="table">';
				self.table += '<thead class="table_head">';
				self.table += '<tr><th colspan="6" class="pad-full-10">' + key + '</th></tr>';
				self.table += '<tr class="visible-lg">';
				var tableHead = val[0];
				for (var prop in tableHead) {
					self.table += '<th class="pad-10">' + prop + '</th>';
				}
				self.table += '</tr>';
				self.table += '</thead>';
				self.table += '</table>';
			});
			$('#dryCargoBulkFixtures').html(self.table);

			$.each(tableObj[0], function (idx, val) {
				getArr = val;
			});
			for (var i = 0; i < getArr.length; i++) {
				if (self.recreateObj[getArr[i]['fixtureType']] == undefined) {
					self.recreateObj[getArr[i]['fixtureType']] = [];
				}
			}
			for (var prop in self.recreateObj) {
				for (var i = 0; i < getArr.length; i++) {
					if (prop == getArr[i]['fixtureType']) {
						self.recreateObj[prop].push(getArr[i]);
					}
				}
			}
			var tbody = '<tbody class="visible-lg">';
			$.each(self.recreateObj, function (key, val) {
				tbody += '<tr><td colspan="2" class="graybg RB18 p-10">' + key + '</td><td align="right" class="graybg RB18 p-10"><a href="javascript: void(0);" class="top"><span class="arrow"></span>Top</a></td></tr>';
				$.each(val, function (idx, value) {
					tbody += '<tr>';
					$.each(value, function (k, v) {
						tbody += '<td class="R16 pad-10">' + v + '</td>';
					});
					tbody += '</tr>';
				});
			});
			tbody += '</tbody>';

			return tbody;
		},
		loadMobileView: function loadMobileView() {
			var tbody = '<tbody class="visible-sm">';
			$.each(this.recreateObj, function (key, val) {
				tbody += '<tr><td class="graybg RB18 p-10">' + key + '</td><td colspan="1" align="right" class="graybg RB18 p-10"><a href="javascript: void(0);" class="top"><span class="arrow"></span>Top</a></td></tr>';
				$.each(val, function (idx, value) {
					$.each(value, function (k, v) {
						tbody += '<tr><td class="pad-10 R21_GrayColor border-right">' + k + '</td><td class="pad-10 R21_GrayColor">' + v + '</td></tr>';
					});
					tbody += '<tr><td><hr /></td><td><hr /></td></tr>';
				});
			});
			tbody += '</tbody>';

			return tbody;
		},
		init: function init() {
			this.renderDate();
			this.renderTable();
		}
	};

	$(document).ready(function () {
		if ($('#dryCargoBulkFixtures').length > 0) {
			dryCargoBulkFixtures.init();
		}
	});
})();

},{}],22:[function(require,module,exports){
'use strict';

(function () {
	var dryCargoIcap = {
		table: '',
		renderTable: function renderTable() {
			var self = this;
			//console.log(tableObj);
			$.each(tableObj, function (datekey, date) {

				$.each(date, function (key, value) {
					//console.log(key);
					self.table += '<table class="table descView">';
					self.table += '<thead class="table_head">';
					self.table += '<tr><th colspan="6" class="pad-full-10">' + key + '</th></tr>';
					self.table += '<tr class="visible-lg">';
					//console.log(value[0]);
					var tableHead = value[0];
					for (var key in tableHead) {
						self.table += '<th class="pad-10">' + key + '</th>';
					}
					self.table += '</tr>';
					self.table += '</thead>';
					self.table += '<tbody class="visible-lg">';
					self.table += '<tr>';
					$.each(value, function (objData, objVal) {
						//console.log(objVal);						
						$.each(objVal, function (responseKey, responseVal) {
							self.table += '<td class="R16 pad-10">' + responseVal + '</td>';
						});
						self.table += '</tr>';
					});
					self.table += '</tbody>';
					self.table += '</table>';
					$('#dryCargoIcap').html(self.table);
				});
			});
		},
		init: function init() {
			this.renderTable();
		}
	};

	$(document).ready(function () {
		if ($('#dryCargoIcap').length > 0) {
			dryCargoIcap.init();
		}
	});
})();

},{}],23:[function(require,module,exports){
'use strict';

(function () {
	var dryCargo = {
		table: '',
		renderTable: function renderTable() {
			var self = this;
			//console.log(tableObj);
			$.each(tableObj, function (datekey, date) {

				$.each(date, function (key, value) {
					//console.log(key);
					self.table += '<table class="table descView">';
					self.table += '<thead class="table_head">';
					self.table += '<tr><th colspan="6" class="pad-full-10">' + key + '</th></tr>';
					self.table += '<tr class="visible-lg">';
					//console.log(value[0]);
					var tableHead = value[0];
					for (var key in tableHead) {
						self.table += '<th class="pad-10">' + key + '</th>';
					}
					self.table += '</tr>';
					self.table += '</thead>';
					self.table += '<tbody class="visible-lg">';
					self.table += '<tr>';
					$.each(value, function (objData, objVal) {
						//console.log(objVal);						
						$.each(objVal, function (responseKey, responseVal) {
							self.table += '<td class="R16 pad-10">' + responseVal + '</td>';
						});
						self.table += '</tr>';
					});
					self.table += '</tbody>';
					self.table += '</table>';
					$('#dryCargo').html(self.table);
				});
			});
		},
		renderMobile: function renderMobile() {
			var self = this;
			$.each(tableObj, function (datekey, date) {

				$.each(date, function (key, value) {
					//console.log(key);
					self.table += '<table class="table mobView">';
					self.table += '<thead class="table_head">';
					self.table += '<tr><th colspan="8" class="pad-full-10">' + key + '</th></tr>';
					self.table += '</thead>';
					self.table += '<tbody class="visible-sm">';

					$.each(value, function (objData, objVal) {
						//console.log(objVal);						
						$.each(objVal, function (responseKey, responseVal) {
							self.table += '<tr>';
							self.table += '<td class="pad-10 mobleftCol">' + responseKey + '</td>';
							self.table += '<td class="pad-10 mobrigCol">' + responseVal + '</td>';
							self.table += '</tr>';
						});
					});
					self.table += '</tbody>';
					self.table += '</table>';
					$('#dryCargo').html(self.table);
				});
			});
		},
		init: function init() {
			this.renderTable();
			this.renderMobile();
		}
	};

	$(document).ready(function () {
		if ($('#dryCargo').length > 0) {
			dryCargo.init();
		}
	});
})();

},{}],24:[function(require,module,exports){
'use strict';

(function () {
	var marketData = {
		renderTable: function renderTable(data, renderId) {
			this.loadMobileView(data, renderId[0]);
			this.loadDescView(data, renderId[1]);
			this.initiateCarousel(renderId[0]);
			this.setColHeight(renderId);
		},
		loadMobileView: function loadMobileView(tableData, id) {
			var headObj = tableData[0],
			    indx = 0,
			    titflag = true;
			$.each(headObj, function (key, val) {
				if (titflag) {
					titflag = false;
					$('.states_heading').append('<div class="title" data-title="' + key + '"><span>' + key + '</span></div>');
				} else {
					$(id).append('<div class="item titleHead" data-title="' + key + '"><div class="title">' + key + '</div></div>');
				}
			});

			$.each(tableData, function (idx, val) {
				var oddCls = idx % 2 !== 0 ? 'oddCls' : '';
				$.each(val, function (k, v) {
					var cls = v.split(' ')[1].indexOf('-') !== -1 ? 'fall' : 'rise';

					if (indx >= 1) {
						$('div[data-title="' + k + '"]').append('<div class="R16 leftbord ' + oddCls + '"><span class="numData">' + v.split(' ')[0] + '</span><span class="' + cls + '">' + v.split(' ')[1] + '</span></div>');
					} else {
						$('.states_heading').append('<div class="R16 leftbord ' + oddCls + '">' + v + '</div>');
					}
					indx++;
				});
				indx = 0;
			});
		},
		loadDescView: function loadDescView(tableData, id) {
			var thead = tableData[0],
			    descStr = '<thead class="table_head">',
			    index = 0,
			    indx = 0;
			descStr += "<tr>";
			for (var prop in thead) {
				descStr += "<th class='title'><div class='pad'>" + prop + "</div></th>";
			}
			descStr += "</tr>";
			descStr += "</thead>";

			$.each(tableData, function (idx, val) {
				index++;
				var oddCls = index % 2 == 0 ? 'oddCls' : '';
				descStr += "<tr class='" + oddCls + "'>";
				for (var prop in val) {
					var cls = val[prop].split(' ')[1].indexOf('-') !== -1 ? 'fall' : 'rise';
					if (indx >= 1) {
						descStr += "<td class='R16 pad-10'><div class='pad'><span class='numData'>" + val[prop].split(' ')[0] + '</span><span class="' + cls + '">' + val[prop].split(' ')[1] + "</span></div></td>";
					} else {
						descStr += "<td class='R16 pad-10'><div class='pad'>" + val[prop] + "</div></td>";
					}
					indx++;
				}
				indx = 0;
				descStr += "</tr>";
			});
			$(id).html(descStr);
		},
		initiateCarousel: function initiateCarousel(id) {
			$(id).owlCarousel({
				loop: true,
				autoPlay: false,
				nav: true,
				navContainer: '#customNav',
				dotsContainer: '#customDots',
				slideBy: 1,
				responsive: {
					0: {
						items: 4
					},
					678: {
						items: 3
					},
					320: {
						items: 1
					},
					480: {
						items: 2
					},
					1000: {
						items: 6
					}
				}
			});
		},
		setColHeight: function setColHeight(parentId) {
			parentId.closest('#main-carousel').find('.states_heading .R16').each(function (idx) {
				var $this = $(this),
				    colHeight = $this.height();
				$('.titleHead').each(function () {
					$($(this).find('.R16')[idx]).height(colHeight);
				});
			});
		},
		init: function init(data, renderId) {
			var self = this;
			self.renderTable(data, renderId);
			$(window).resize(function () {
				self.setColHeight(renderId);
			});
		}
	};

	$(document).ready(function () {
		if ($('#market-data').length > 0) {
			marketData.init(window.jsonBalticIndices, $('.marketDataTable'));
		}
	});
})();

},{}],25:[function(require,module,exports){
'use strict';

(function () {
	var marketImarex = {
		table: '',
		renderDate: function renderDate() {
			var options = '';
			$.each(dateObj[0], function (key, val) {
				$.each(val, function (idx, value) {
					options += '<option value="' + value.Value + '">' + value.Text + '</option>';
				});
			});

			$('#selectWeek').html(options);
		},
		renderTable: function renderTable() {
			this.loadDescView();
			//this.loadMobileView();
		},
		loadDescView: function loadDescView() {
			var self = this;
			$.each(tableObj[0], function (key, val) {
				self.table += '<table class="table">';
				self.table += '<thead class="table_head">';
				self.table += '<tr><th colspan="6" class="pad-full-10">' + key + '</th></tr>';
				self.table += '<tr class="blueBg">';
				var tableHead = val[0];
				for (var prop in tableHead) {
					if (prop != "Header") {
						self.table += '<th class="pad-10">' + prop + '</th>';
					} else {
						self.table += '<th class="pad-10"></th>';
					}
				}
				self.table += '</tr>';
				self.table += '</thead>';

				self.table += '<tbody>';

				$.each(val, function (idx, value) {
					self.table += '<tr>';
					$.each(value, function (k, v) {
						self.table += '<td class="R16 pad-10">' + v + '</td>';
					});
					self.table += '</tr>';
				});
				self.table += '</tbody>';
				self.table += '</table>';
			});
			$('#marketImarex').html(self.table);
		},
		loadMobileView: function loadMobileView() {
			var tbody = '<tbody class="visible-sm">';
			$.each(this.recreateObj, function (key, val) {
				tbody += '<tr><td class="graybg RB18 p-10">' + key + '</td><td colspan="1" align="right" class="graybg RB18 p-10"><a href="javascript: void(0);" class="top"><span class="arrow"></span>Top</a></td></tr>';
				$.each(val, function (idx, value) {
					$.each(value, function (k, v) {
						tbody += '<tr><td class="pad-10 R21_GrayColor border-right">' + k + '</td><td class="pad-10 R21_GrayColor">' + v + '</td></tr>';
					});
					tbody += '<tr><td><hr /></td><td><hr /></td></tr>';
				});
			});
			tbody += '</tbody>';

			$('#marketImarex table').append(tbody);
		},
		init: function init() {
			this.renderDate();
			this.renderTable();
		}
	};

	$(document).ready(function () {
		if ($('#marketImarex').length > 0) {
			marketImarex.init();
		}
	});
})();

},{}],26:[function(require,module,exports){
"use strict";

(function () {
	// body...
	'use strict';

	var marketDataDryCargoSsyAtl = {
		RenderTable: function RenderTable(data, Parent) {
			var self = this,
			    TableStr = "";
			Parent.empty();

			for (var key in data[0]) {
				TableStr += self.RenderSingleTable(data[0][key]);
			}

			Parent.append(TableStr);
		},
		RenderSingleTable: function RenderSingleTable(Data) {
			console.log(Data);
			var HeadingStr = "",
			    SubHeadingStr = "",
			    TbodyStr = "",
			    Heading = Data[0];

			for (var key in Heading) {
				if (key.split("|").length === 1) {
					HeadingStr += "<th class='pad-full-10' colspan='1'>" + "<div class='text-center'>&nbsp;</div>" + "<div class='text-center'>" + key.split("|")[0] + "</div>" + "</th>";
				} else {
					HeadingStr += "<th class='pad-full-10' colspan='1'>" + "<div class='text-center'>" + key.split("|")[0] + "</div>" + "<div class='text-center'>" + key.split("|")[1] + "</div>" + "</th>";
				}
			}

			for (var i = 0; i < Data.length; i++) {
				var EachValue = Data[i],
				    Td = "",
				    align = "right";
				for (var key in EachValue) {
					if (key == "Route") {
						align = "left";
					}
					Td += '<td colspan="1" class="pad-full-10" align="' + align + '">' + EachValue[key] + '</td>';
				}
				TbodyStr += '<tr>' + Td + '</tr>';
			}

			var Table = '<table class="table">' + '<thead class="table_head">' + '<tr>' + HeadingStr + '</tr>' + '</thead>' + '<tbody>' + TbodyStr + '</tbody>' + '</table>';

			return Table;
		},
		RenderCarousel: function RenderCarousel(data, Parent) {
			var self = this,
			    CarouselStr = "";
			Parent.empty();

			//Heading
			for (var key in data[0]) {
				CarouselStr += self.RenderSingleCarousel(key, data[0][key]);
			}

			Parent.append(CarouselStr);
			self.InitCarousel(Parent);
		},
		getFixedData: function getFixedData(key, Data) {
			var FixedStr = "";
			for (var i = 0; i < Data.length; i++) {
				FixedStr += '<div class="R16">' + Data[i][key] + '</div>';
			}
			return FixedStr;
		},
		getData: function getData(key, Data) {
			var ArticleStr = "";
			for (var i = 0; i < Data.length; i++) {
				// ArticleStr += Data[i][]
				ArticleStr += '<div class="R16">' + Data[i][key] + '</div>';
			}
			return ArticleStr;
		},
		InitCarousel: function InitCarousel(Parent) {
			Parent.find('.owl-carousel').owlCarousel({
				loop: false,
				margin: 0,
				merge: true,
				nav: false,
				slideBy: 1,
				responsive: {
					0: {
						items: 1
					},
					678: {
						items: 1
					},
					320: {
						items: 1
					},
					480: {
						items: 1
					}
				}
			});
		},
		RenderSingleCarousel: function RenderSingleCarousel(Name, Data) {
			var FixedPart = "",
			    CarouselPart = "",
			    Heading = Data[0],
			    self = this,
			    i = 0;
			for (var key in Heading) {
				if (key != 'Route') {
					CarouselPart += '<div class="article">' + '<div class="year_heading">' + key + '</div>' + self.getData(key, Data) + '</div>';
				} else {
					FixedPart += '<div class="year_heading">' + key + '</div>' + self.getFixedData(key, Data);
				}
			}

			var Carousel = '<div class="table_head pad-10 clearfix">' + '<span>&nbsp;</span>' + '</div>' + '<div class="clearfix" style="margin-bottom: 1rem; border:1px solid #d1d3d4;">' + '<div class="states_heading">' + FixedPart + '</div>' + '<div class="owl-wrapper">' + '<div class="owl-carousel">' + CarouselPart + '</div>' + '</div>' + '</div>';

			return Carousel;
		},
		init: function init(data, id) {
			var self = this;
			if ($(window).width() > 668) self.RenderTable(data, id);else self.RenderCarousel(data, id);
		}
	};

	if ($('#marketDataDryCargo').length > 0) {
		marketDataDryCargoSsyAtl.init(window.jsonMarketdataDryCargo, $('#marketDataDryCargo'));
	}
})();

},{}],27:[function(require,module,exports){
"use strict";

(function () {
	// body...
	'use strict';

	var shipCoalExport = {
		RenderTable: function RenderTable(data, Parent) {
			var self = this,
			    TableStr = "";
			Parent.empty();

			TableStr = self.RenderSingleTable(data);

			Parent.append(TableStr);
		},
		RenderSingleTable: function RenderSingleTable(Data) {
			console.log(Data);
			var HeadingStr = "",
			    SubHeadingStr = "",
			    TbodyStr = "",
			    Heading = Data[0];

			for (var key in Heading) {
				if (Array.isArray(Heading[key])) {
					HeadingStr += '<th align="left" colspan="2" class="pad-10 main-heading">' + key + '</th>';
					var SubHeading = Heading[key][0];
					for (var k in SubHeading) {
						SubHeadingStr += '<th colspan="1">' + k + '</th>';
					}
				} else {
					HeadingStr += '<th align="left" colspan="1" class="pad-10 main-heading">' + key + '</th>';
					SubHeadingStr += '<th colspan="1"></th>';
				}
			}

			for (var i = 0; i < Data.length; i++) {
				var EachValue = Data[i],
				    Td = "";
				for (var key in EachValue) {
					if (Array.isArray(EachValue[key])) {
						var Values = EachValue[key][0];
						for (var k in Values) {
							Td += '<td colspan="1" class="pad-10" align="right">' + Values[k] + '</td>';
						}
					} else {
						Td += '<td colspan="1" class="pad-10">' + EachValue[key] + '</td>';
					}
				}
				TbodyStr += '<tr>' + Td + '</tr>';
			}

			var Table = '<table class="table theme-table">' + '<thead>' + '<tr>' + HeadingStr + '</tr>' + '<tr>' + SubHeadingStr + '</tr>' + '</thead>' + '<tbody>' + TbodyStr + '</tbody>' + '</table>';

			return Table;
		},
		init: function init(data, id) {
			var self = this;
			self.RenderTable(data, id);
		}
	};

	if ($('#shipCoalExport').length > 0) {
		shipCoalExport.init(window.jsonShipCoalExport, $('#shipCoalExport'));
	}
})();

},{}],28:[function(require,module,exports){
"use strict";

(function () {
	// body...
	'use strict';

	var shipContainerShip = {
		RenderTable: function RenderTable(data, Parent) {
			var self = this,
			    TableStr = "";
			Parent.empty();

			//Heading
			for (var key in data) {
				TableStr += self.RenderSingleTable(key, data[key]);
			}

			Parent.append(TableStr);
		},
		RenderSingleTable: function RenderSingleTable(heading, Data) {
			console.log(Data);
			var SubHeadingStr = "",
			    SubSubHeadingStr = "",
			    TbodyStr = "",
			    SubHeading = Data[0];

			for (var key in SubHeading) {
				SubHeadingStr += '<th class="pad-10" colspan="2">' + key + '</th>';
				if (Array.isArray(SubHeading[key])) {
					var SubHeadingArray = SubHeading[key][0];
					for (var sub in SubHeadingArray) {
						SubSubHeadingStr += '<th class="pad-full-10" colspan="1">' + sub + '</th>';
					}
				} else {
					SubSubHeadingStr += '<th class="pad-full-10" colspan="2"></th>';
				}
			}

			for (var i = 0; i < Data.length; i++) {
				var Body = "",
				    Values = Data[i];

				for (var key in Values) {
					if (Array.isArray(Values[key])) {
						var items = Values[key][0];
						for (var k in items) {
							Body += '<td class="pad-full-10" align="right">' + items[k] + '</td>';
						}
					} else {
						Body += '<td class="pad-full-10" align="left" colspan="2">' + Values[key] + '</td>';
					}
				}

				TbodyStr += '<tr>' + Body + '</tr>';
			}

			var Table = '<table class="table">' + '<thead class="table_head">' + '<tr>' + '<th align="left" colspan="14" class="pad-10 main-heading">' + heading + '</th>' + '</tr>' + '<tr class="visible-lg">' + SubHeadingStr + '</tr>' + '<tr class="visible-lg">' + SubSubHeadingStr + '</tr>' + '</thead>' + '<tbody>' + TbodyStr + '</tbody>' + '</table>';

			return Table;
		},
		RenderCarousel: function RenderCarousel(data, Parent) {
			var self = this,
			    CarouselStr = "";
			Parent.empty();

			//Heading
			for (var key in data) {
				CarouselStr += self.RenderSingleCarousel(key, data[key]);
			}

			Parent.append(CarouselStr);
			self.InitCarousel(Parent);
		},
		InitCarousel: function InitCarousel(Parent) {
			Parent.find('.owl-carousel').owlCarousel({
				loop: false,
				margin: 0,
				merge: true,
				nav: false,
				slideBy: 1,
				responsive: {
					0: {
						items: 1
					},
					678: {
						items: 1
					},
					320: {
						items: 1
					},
					480: {
						items: 1
					}
				}
			});
		},
		getData: function getData(key, Data) {
			var ArticleStr = "";
			for (var i = 0; i < Data.length; i++) {
				// ArticleStr += Data[i][]
				var ArticleContent = Data[i][key][0];
				var Str = "";
				for (var k in ArticleContent) {
					Str += '<span class="sub-item">' + ArticleContent[k] + '</span>';
				}
				ArticleStr += '<div class="R16">' + Str + '</div>';
			}
			return ArticleStr;
		},
		getKeyChild: function getKeyChild(key, Object) {
			var ChildStr = "";
			for (var i in Object[0]) {
				ChildStr += "<span class='sub-item'>" + i + "</span>";
			}
			return ChildStr;
		},
		getFixedData: function getFixedData(key, Data) {
			var FixedStr = "";
			for (var i = 0; i < Data.length; i++) {
				FixedStr += '<div class="R16">' + Data[i][key] + '</div>';
			}
			return FixedStr;
		},
		RenderSingleCarousel: function RenderSingleCarousel(Name, Data) {
			var FixedPart = "",
			    CarouselPart = "",
			    Heading = Data[0],
			    self = this;
			for (var key in Heading) {
				if (Array.isArray(Heading[key])) {
					CarouselPart += '<div class="article">' + '<div class="year_heading">' + '<div>' + key + '</div>' + self.getKeyChild(key, Heading[key]) + '</div>' + self.getData(key, Data) + '</div>';
				} else {
					FixedPart += '<div class="year_heading">' + key + '</div>' + self.getFixedData(key, Data);
				}
			}

			var Carousel = '<div class="table_head pad-10 clearfix">' + '<span class="RB16">' + Name + '</span>' + '</div>' + '<div class="clearfix" style="margin-bottom: 1rem; border:1px solid #d1d3d4">' + '<div class="states_heading">' + FixedPart + '</div>' + '<div class="owl-wrapper">' + '<div class="owl-carousel">' + CarouselPart + '</div>' + '</div>' + '</div>';

			return Carousel;
		},
		init: function init(data, id) {
			var self = this;
			self.RenderTable(data[0], id);
		}
	};

	if ($('#shipContainerShip').length > 0) {
		shipContainerShip.init(window.jsonShipContainerShip, $('#shipContainerShip'));
	}
})();

},{}],29:[function(require,module,exports){
"use strict";

(function () {
	// body...
	'use strict';

	var shipRoro = {
		RenderTable: function RenderTable(data, Parent) {
			var self = this,
			    TableStr = "";
			Parent.empty();

			for (var key in data[0]) {
				TableStr += self.RenderSingleTable(data[0][key]);
			}

			Parent.append(TableStr);
		},
		RenderSingleTable: function RenderSingleTable(Data) {
			console.log(Data);
			var HeadingStr = "",
			    SubHeadingStr = "",
			    TbodyStr = "",
			    Heading = Data[0];

			for (var key in Heading) {
				HeadingStr += "<th colspan='1'>" + '<div class="pad-full-10">' + key + '</div>' + "</th>";
			}

			for (var i = 0; i < Data.length; i++) {
				var EachValue = Data[i],
				    Td = "",
				    align = "right";
				for (var key in EachValue) {
					Td += '<td colspan="1" class="pad-full-10" align="left">' + EachValue[key] + '</td>';
				}
				TbodyStr += '<tr>' + Td + '</tr>';
			}

			var Table = '<table class="table">' + '<thead class="table_head">' + '<tr>' + HeadingStr + '</tr>' + '</thead>' + '<tbody>' + TbodyStr + '</tbody>' + '</table>';

			return Table;
		},
		init: function init(data, id) {
			var self = this;
			self.RenderTable(data, id);
		}
	};

	if ($('#shipRoro').length > 0) {
		shipRoro.init(window.jsonshipRoro, $('#shipRoro'));
	}
})();

},{}],30:[function(require,module,exports){
"use strict";

(function () {
	// body...
	'use strict';

	var shipVehicle = {
		RenderTable: function RenderTable(data, Parent) {
			var self = this,
			    TableStr = "";
			Parent.empty();

			//Heading
			for (var key in data) {
				TableStr += self.RenderSingleTable(key, data[key]);
			}

			Parent.append(TableStr);
		},
		RenderSingleTable: function RenderSingleTable(heading, Data) {
			console.log(Data);
			var SubHeadingStr = "",
			    SubSubHeadingStr = "",
			    TbodyStr = "",
			    SubHeading = Data[0];

			for (var key in SubHeading) {
				SubHeadingStr += '<th class="pad-10" colspan="2">' + key + '</th>';
				if (Array.isArray(SubHeading[key])) {
					var SubHeadingArray = SubHeading[key][0];
					for (var sub in SubHeadingArray) {
						SubSubHeadingStr += '<th class="pad-10" colspan="1">' + sub + '</th>';
					}
				} else {
					SubSubHeadingStr += '<th class="pad-10" colspan="2"></th>';
				}
			}

			for (var i = 0; i < Data.length; i++) {
				var Body = "",
				    Values = Data[i];

				for (var key in Values) {
					if (Array.isArray(Values[key])) {
						var items = Values[key][0];
						for (var k in items) {
							Body += '<td class="pad-10" align="right">' + items[k] + '</td>';
						}
					} else {
						Body += '<td class="pad-10" align="right" colspan="2">' + Values[key] + '</td>';
					}
				}

				TbodyStr += '<tr>' + Body + '</tr>';
			}

			var Table = '<table class="table">' + '<thead class="table_head">' + '<tr>' + '<th align="left" colspan="14" class="pad-10 main-heading">' + heading + '</th>' + '</tr>' + '<tr class="visible-lg">' + SubHeadingStr + '</tr>' + '<tr class="visible-lg">' + SubSubHeadingStr + '</tr>' + '</thead>' + '<tbody>' + TbodyStr + '</tbody>' + '</table>';

			return Table;
		},
		RenderCarousel: function RenderCarousel(data, Parent) {
			var self = this,
			    CarouselStr = "";
			Parent.empty();

			//Heading
			for (var key in data) {
				CarouselStr += self.RenderSingleCarousel(key, data[key]);
			}

			Parent.append(CarouselStr);
			self.InitCarousel(Parent);
		},
		InitCarousel: function InitCarousel(Parent) {
			Parent.find('.owl-carousel').owlCarousel({
				loop: false,
				margin: 0,
				merge: true,
				nav: false,
				slideBy: 1,
				responsive: {
					0: {
						items: 1
					},
					678: {
						items: 1
					},
					320: {
						items: 1
					},
					480: {
						items: 1
					}
				}
			});
		},
		getData: function getData(key, Data) {
			var ArticleStr = "";
			for (var i = 0; i < Data.length; i++) {
				// ArticleStr += Data[i][]
				var ArticleContent = Data[i][key][0];
				var Str = "";
				for (var k in ArticleContent) {
					Str += '<span class="sub-item">' + ArticleContent[k] + '</span>';
				}
				ArticleStr += '<div class="R16">' + Str + '</div>';
			}
			return ArticleStr;
		},
		getKeyChild: function getKeyChild(key, Object) {
			var ChildStr = "";
			for (var i in Object[0]) {
				ChildStr += "<span class='sub-item'>" + i + "</span>";
			}
			return ChildStr;
		},
		getFixedData: function getFixedData(key, Data) {
			var FixedStr = "";
			for (var i = 0; i < Data.length; i++) {
				FixedStr += '<div class="R16">' + Data[i][key] + '</div>';
			}
			return FixedStr;
		},
		RenderSingleCarousel: function RenderSingleCarousel(Name, Data) {
			var FixedPart = "",
			    CarouselPart = "",
			    Heading = Data[0],
			    self = this;
			for (var key in Heading) {
				if (Array.isArray(Heading[key])) {
					CarouselPart += '<div class="article">' + '<div class="year_heading">' + '<div>' + key + '</div>' + self.getKeyChild(key, Heading[key]) + '</div>' + self.getData(key, Data) + '</div>';
				} else {
					FixedPart += '<div class="year_heading">' + key + '</div>' + self.getFixedData(key, Data);
				}
			}

			var Carousel = '<div class="table_head pad-10 clearfix">' + '<span class="RB16">' + Name + '</span>' + '</div>' + '<div class="clearfix" style="margin-bottom: 1rem; border:1px solid #d1d3d4">' + '<div class="states_heading">' + FixedPart + '</div>' + '<div class="owl-wrapper">' + '<div class="owl-carousel">' + CarouselPart + '</div>' + '</div>' + '</div>';

			return Carousel;
		},
		init: function init(data, id) {
			var self = this;
			if ($(window).width() > 667) self.RenderTable(data[0], id);else self.RenderCarousel(data[0], id);
		}
	};

	if ($('#shipVehicle').length > 0) {
		shipVehicle.init(window.jsonShipVehicle, $('#shipVehicle'));
	}
})();

},{}],31:[function(require,module,exports){
'use strict';

(function () {
	var tankerFixtures = {
		renderDateData: function renderDateData(data) {
			if (data[0]['SelectDate'] !== undefined) {
				$('#selectDay').html(this.loadDropdownData(data[0]['SelectDate']));
			}
		},
		loadDropdownData: function loadDropdownData(options) {
			var optionStr = '';
			$.each(options, function (idx, val) {
				if (idx == 0) {
					optionStr += '<option value="' + val.Value + '" selected="selected">' + val.Text + '</option>';
				} else {
					optionStr += '<option value="' + val.Value + '">' + val.Text + '</option>';
				}
			});
			return optionStr;
		},
		renderTable: function renderTable(tableData) {
			var self = this,
			    loadDateVal = $('#selectDay option').val();

			self.callAjaxFn(loadDateVal);
			$(document).on('change', '#selectDay', function () {
				var selectDateVal = $('#selectDay option').val();
				self.callAjaxFn(selectDateVal);
			});
		},
		callAjaxFn: function callAjaxFn(seldateVal) {
			var self = this;
			$.ajax({
				url: '/Download/JsonDataFromFeed/ReadJsonMarketFixture/',
				data: { 'dateVal': seldateVal, 'feedUrl': $('#TankerFixHiddenVal').val() },
				dataType: 'json',
				type: 'GET',
				success: function success(searchData) {
					self.sendHTTPRequest(searchData);
				},
				error: function error(err) {
					console.log(err);
				}
			});
		},
		sendHTTPRequest: function sendHTTPRequest(searchData) {
			var self = this,
			    loadHead = true,
			    tableStr = '';

			tableStr += self.loadDesktopView(searchData);
			tableStr += self.loadMobileView(searchData);

			$('#tankerFixtures').html(tableStr);
		},
		loadDesktopView: function loadDesktopView(tableData) {
			var tableStr = '';
			for (var i = 0; i < tableData.length; i++) {
				var theadFlag = true,
				    tbodyFlag = true,
				    tbodyFlagend = false,
				    idx = 0;
				tableStr += '<table class="table descView"><thead class="table_head">';
				for (var prop in tableData[i]) {
					tableStr += '<tr>';
					tableStr += '<th colspan="8" class="pad-full-10">' + prop + '</th>';
					tableStr += '</tr>';
					for (var j = 0; j < tableData[i][prop].length; j++) {
						idx++;
						if (tableData[i][prop].length == idx - 1) tbodyFlagend = true;
						var eachObj = tableData[i][prop];

						if (theadFlag) {
							theadFlag = false;
							tableStr += '<tr class="visible-lg">';
							for (var p in eachObj[j]) {
								tableStr += '<th class="pad-10">' + p + '</th>';
							}
							tableStr += '</thead>';
						}
						if (tbodyFlag) {
							tbodyFlag = false;
							tableStr += '<tbody class="visible-lg">';
						}
						tableStr += '<tr>';
						for (var p in eachObj[j]) {
							tableStr += '<td class="R16 pad-10">' + eachObj[j][p] + '</td>';
						}
						tableStr += '</tr>';
						if (tbodyFlagend) {
							tbodyFlagend = false;
							tableStr += '</tbody>';
						}
					}
				}
				tableStr += '</table>';
			}
			return tableStr;
		},
		loadMobileView: function loadMobileView(tableData) {
			var mobileStr = '',
			    dataIdx = 0;
			$.each(tableData, function (index, value) {
				mobileStr += '<table class="table mobView">';
				$.each(value, function (key, val) {
					mobileStr += '<thead class="table_head">';
					mobileStr += '<tr>';
					mobileStr += '<th colspan="2" class="pad-full-10">' + key + '</th>';
					mobileStr += '</tr>';
					mobileStr += '</thead>';

					mobileStr += '<tbody class="visible-sm">';
					$.each(val, function (i, v) {
						var indx = 0;
						for (var prop in v) {
							indx++;
							var borTop = i !== 0 && indx == 1 ? 'borTop' : '';
							if (borTop !== '') mobileStr += '<tr class="borTop"><td colspan="2"></td></tr>';
							mobileStr += '<tr>';
							mobileStr += '<td class="pad-10 mobleftCol">' + prop + '</td>';
							mobileStr += '<td class="pad-10 mobrigCol">' + v[prop] + '</td>';
							mobileStr += '</tr>';
						}
					});
					mobileStr += '</tbody>';
				});
				mobileStr += '</table>';
			});
			return mobileStr;
		},
		init: function init() {
			//this.renderDateData(dateObj);
			this.renderTable();
		}
	};

	$(document).ready(function () {
		if ($('#tanker-fixtures').length > 0) {
			tankerFixtures.init();
		}
	});
})();

},{}],32:[function(require,module,exports){
"use strict";

(function () {
	// body...
	'use strict';

	var tankerPureChem = {
		RenderTable: function RenderTable(data, Parent) {
			var self = this,
			    TableStr = "";
			Parent.empty();

			//Heading
			for (var key in data) {
				TableStr += self.RenderSingleTable(key, data[key]);
			}

			Parent.append(TableStr);
		},
		RenderSingleTable: function RenderSingleTable(heading, Data) {
			console.log(Data);
			var SubHeadingStr = "",
			    SubSubHeadingStr = "",
			    TbodyStr = "",
			    SubHeading = Data[0];

			for (var key in SubHeading) {
				SubHeadingStr += '<th class="pad-10" colspan="2">' + key + '</th>';
				if (Array.isArray(SubHeading[key])) {
					var SubHeadingArray = SubHeading[key][0];
					for (var sub in SubHeadingArray) {
						SubSubHeadingStr += '<th class="pad-10" colspan="1">' + sub + '</th>';
					}
				} else {
					SubSubHeadingStr += '<th class="pad-10" colspan="2"></th>';
				}
			}

			for (var i = 0; i < Data.length; i++) {
				var Body = "",
				    Values = Data[i];

				for (var key in Values) {
					if (Array.isArray(Values[key])) {
						var items = Values[key][0];
						for (var k in items) {
							Body += '<td class="pad-10" align="right">' + items[k] + '</td>';
						}
					} else {
						Body += '<td class="pad-10" align="right" colspan="2">' + Values[key] + '</td>';
					}
				}

				TbodyStr += '<tr>' + Body + '</tr>';
			}

			var Table = '<table class="table">' + '<thead class="table_head">' + '<tr>' + '<th align="left" colspan="14" class="pad-10 main-heading">' + heading + '</th>' + '</tr>' + '<tr class="visible-lg">' + SubHeadingStr + '</tr>' + '<tr class="visible-lg">' + SubSubHeadingStr + '</tr>' + '</thead>' + '<tbody>' + TbodyStr + '</tbody>' + '</table>';

			return Table;
		},
		RenderCarousel: function RenderCarousel(data, Parent) {
			var self = this,
			    CarouselStr = "";
			Parent.empty();

			//Heading
			for (var key in data) {
				CarouselStr += self.RenderSingleCarousel(key, data[key]);
			}

			Parent.append(CarouselStr);
			self.InitCarousel(Parent);
		},
		InitCarousel: function InitCarousel(Parent) {
			Parent.find('.owl-carousel').owlCarousel({
				loop: false,
				margin: 0,
				merge: true,
				nav: false,
				slideBy: 1,
				responsive: {
					0: {
						items: 1
					},
					678: {
						items: 1
					},
					320: {
						items: 1
					},
					480: {
						items: 1
					}
				}
			});
		},
		getData: function getData(key, Data) {
			var ArticleStr = "";
			for (var i = 0; i < Data.length; i++) {
				// ArticleStr += Data[i][]
				var ArticleContent = Data[i][key][0];
				var Str = "";
				for (var k in ArticleContent) {
					Str += '<span class="sub-item">' + ArticleContent[k] + '</span>';
				}
				ArticleStr += '<div class="R16">' + Str + '</div>';
			}
			return ArticleStr;
		},
		getKeyChild: function getKeyChild(key, Object) {
			var ChildStr = "";
			for (var i in Object[0]) {
				ChildStr += "<span class='sub-item'>" + i + "</span>";
			}
			return ChildStr;
		},
		getFixedData: function getFixedData(key, Data) {
			var FixedStr = "";
			for (var i = 0; i < Data.length; i++) {
				FixedStr += '<div class="R16">' + Data[i][key] + '</div>';
			}
			return FixedStr;
		},
		RenderSingleCarousel: function RenderSingleCarousel(Name, Data) {
			var FixedPart = "",
			    CarouselPart = "",
			    Heading = Data[0],
			    self = this;
			for (var key in Heading) {
				if (Array.isArray(Heading[key])) {
					CarouselPart += '<div class="article">' + '<div class="year_heading">' + '<div>' + key + '</div>' + self.getKeyChild(key, Heading[key]) + '</div>' + self.getData(key, Data) + '</div>';
				} else {
					FixedPart += '<div class="year_heading">' + key + '</div>' + self.getFixedData(key, Data);
				}
			}

			var Carousel = '<div class="table_head pad-10 clearfix">' + '<span>' + Name + '</span>' + '</div>' + '<div class="clearfix" style="margin-bottom: 1rem; border:1px solid #d1d3d4">' + '<div class="states_heading">' + FixedPart + '</div>' + '<div class="owl-wrapper">' + '<div class="owl-carousel">' + CarouselPart + '</div>' + '</div>' + '</div>';

			return Carousel;
		},
		init: function init(data, id) {
			var self = this;
			if ($(window).width() > 667) self.RenderTable(data[0], id);else self.RenderCarousel(data[0], id);
		}
	};

	if ($('#tankerPureChemPage').length > 0) {
		tankerPureChem.init(window.tankerPureChemPage, $('#tankerPureChemPage'));
	}
})();

},{}],33:[function(require,module,exports){
'use strict';

var _controllersAnalyticsController = require('../controllers/analytics-controller');

function setClsforFlw(t) {
    for (var i = 0; i < t.length; i++) {
        var tableFlwrow = $(t[i]).find('.followrow.disabled:eq(0)');
        tableFlwrow.addClass('frow');
    }
}

function createJSONData(alltables, UserPreferences, url) {
    for (var i = 0; i < alltables.length; i++) {
        var currenttabtrs = $(alltables[i]).find('tbody tr'),
            getlableStatus = $(alltables[i]).find('thead .lableStatus').val(),
            pubPanPosition = $(alltables[i]).closest('.publicationPan').attr('data-row'),
            tableId = $(alltables[i]).attr('id'),
            publicationName = $(alltables[i]).find('h2').attr('data-publication'),
            subscribeStatus = $(alltables[i]).find('.subscribed').html(),
            channelId = $(alltables[i]).find('h2').attr('data-item-id'),
            channelStatus = $(alltables[i]).find('h2').attr('data-item-status'),
            channellblStatus = getlableStatus == 'followinglbl' ? $('#followingButtonText').val() : $('#followButtonText').val();
        var alltdata = [];
        for (var j = 0; j < currenttabtrs.length; j++) {
            var eachrowAttr = $(currenttabtrs[j]).find('input[type=hidden]').attr('data-row-topic'),
                topicId = $(currenttabtrs[j]).find('input[type=hidden]').attr('data-row-item-id'),
                secondtd = $(currenttabtrs[j]).find('td.wd-25 span').html(),
                datarowNo = $(currenttabtrs[j]).attr('data-row');

            var followStatus = secondtd == $('#followingButtonText').val() ? true : false;
            var subscripStatus = subscribeStatus.toUpperCase() == 'SUBSCRIBED' ? true : false;

            alltdata.push({ 'TopicCode': eachrowAttr, 'TopicOrder': datarowNo, 'IsFollowing': followStatus, 'TopicId': topicId });
        }
        UserPreferences.PreferredChannels.push({ "ChannelCode": publicationName, "ChannelOrder": pubPanPosition, "IsFollowing": channelStatus, "ChannelId": channelId, 'ChannelLblStatus': channellblStatus, Topics: alltdata });
    }
    sendHttpRequest(UserPreferences, null, url);
}

function sendHttpRequest(UserPreferences, setFlag, redirectUrl) {
    $.ajax({
        url: '/Account/api/PersonalizeUserPreferencesApi/Update/',
        data: { 'UserPreferences': JSON.stringify(UserPreferences) },
        dataType: 'json',
        type: 'POST',
        success: function success(data) {
            if (data && data.success) {
                $('.alert-success p').html(data.reason);
                $('.alert-success').show();
                if ($('.alert-success').length > 0) {
                    $(window).scrollTop($('.informa-ribbon').offset().top + $('.informa-ribbon').height());
                }
                if (setFlag == 'register' && redirectUrl == 'href') {
                    window.location.href = $('.registrationBtn').attr('href');
                } else if (setFlag == 'register' && redirectUrl == 'name') {
                    window.location.href = $('.registrationBtn').attr('name');
                }
                if ($('.modal-overlay').hasClass('in')) {
                    if (redirectUrl != 'href' && redirectUrl != 'name') {
                        window.location.href = redirectUrl;
                    }
                }
            } else {
                if (redirectUrl != 'href' && redirectUrl != 'name') {
                    window.location.href = redirectUrl;
                }
                if (setFlag == 'register') {
                    $('.alert-error.register-error p').html(data.reason);
                    $('.alert-error.register-error').show();
                } else {
                    $('.alert-error.myview-error p').html(data.reason);
                    $('.alert-error.myview-error').show();
                }
            }
        },
        error: function error(err) {
            if (err && !err.success) {
                if (setFlag == 'register') {
                    $('.alert-error.register-error p').html(data.reason);
                    $('.alert-error.register-error').show();
                } else {
                    $('.alert-error.myview-error p').html(data.reason);
                    $('.alert-error.myview-error').show();
                }
            }
        }
    });
}

function setDataRow(allpublications) {
    for (var k = 0; k < allpublications.length; k++) {
        var tbody = $(allpublications[k]).find('tbody'),
            newtrs = tbody.find('tr');
        newtrs.removeAttr('data-row');
        for (var v = 0; v < newtrs.length; v++) {
            $(newtrs[v]).attr('data-row', v + 1);
        }
    }
}

function showModal() {
    $('.modal-overlay').addClass('in');
    $('.modal-view').show();
}

function sendRegisterData(alltrs, UserPreferences, redirectUrl) {
    for (var i = 0; i < alltrs.length; i++) {
        var eachrowAttr = $(alltrs[i]).find('input[type=hidden]').attr('data-row-topic'),
            channelId = $(alltrs[i]).find('input[type=hidden]').attr('data-row-item-id'),
            secondtd = $(alltrs[i]).find('td.wd-25 span').html(),
            channelOrder = $(alltrs[i]).attr('data-row'),
            followStatus = secondtd == $('#followingButtonText').val() ? true : false;

        UserPreferences.PreferredChannels.push({ "ChannelCode": eachrowAttr, "ChannelOrder": channelOrder, "IsFollowing": followStatus, "ChannelId": channelId, "Topics": [] });
    }
    sendHttpRequest(UserPreferences, 'register', redirectUrl);
}

function sort_table(tbody, col, asc, sortstatus) {
    var rows = [];
    if (tbody[0] && tbody[0].rows) {
        var allrows = tbody[0].rows;
    } else {
        return;
    }
    if (sortstatus === 'followingBtn') {
        for (var j = 0; j < allrows.length; j++) {
            if (allrows[j].className == 'followrow disabled' || allrows[j].className == 'followrow disabled frow') {
                rows.push(allrows[j]);
            }
        }
    } else if (sortstatus === 'followingrow') {
        for (var j = 0; j < allrows.length; j++) {
            if (allrows[j].className == 'followingrow') {
                rows.push(allrows[j]);
            }
        }
    } else if (sortstatus === 'followrow') {
        for (var j = 0; j < allrows.length; j++) {
            if (allrows[j].className == 'followrow disabled' || allrows[j].className == 'followrow disabled frow') {
                rows.push(allrows[j]);
            }
        }
    }

    var rlen = rows.length,
        arr = new Array(),
        i,
        j,
        cells,
        clen;
    for (i = 0; i < rlen; i++) {
        cells = rows[i].cells;
        clen = cells.length;
        arr[i] = new Array();
        for (j = 0; j < clen; j++) {
            arr[i][j] = cells[j].innerHTML;
        }
    }
    // sort the array by the specified column number (col) and order (asc)
    arr.sort(function (a, b) {
        return a[col] == b[col] ? 0 : a[col] > b[col] ? asc : -1 * asc;
    });
    // replace existing rows with new rows created from the sorted array
    for (i = 0; i < rlen; i++) {
        //rows[i].innerHTML = "<td class='wd-55'>" + arr[i].join("</td><td class='wd-25'>") + "</td>";
        rows[i].innerHTML = "<td class='wd-55'>" + arr[i][0] + "</td><td class='wd-25'>" + arr[i][1] + "</td><td class='wd-15'>" + arr[i][2] + "</td>";
    }
}

$(function () {
    var clickedUrl = '';
    $('a').click(function (e) {
        if ($('#validatePriority') && $('#validatePriority').val() == "true") {
            if (!$(this).hasClass("validationChk")) {
                e.preventDefault();
                showModal();
            }
        }
        if ($('#validateMyViewPriority') && $('#validateMyViewPriority').val() == "true") {
            if (!$(this).hasClass("validationChk")) {
                e.preventDefault();
                showModal();
                if (!$(this).hasClass('myviewLink')) {
                    clickedUrl = $(this).attr('href');
                } else {
                    clickedUrl = $(this).attr('href') + '#' + $(this).attr('name');
                }
            }
        }
    });

    $('form').submit(function () {
        if ($('#validatePriority') && $('#validatePriority').val() == "true") {
            showModal();
            return false;
        }
        if ($('#validateMyViewPriority') && $('#validateMyViewPriority').val() == "true") {
            showModal();
            return false;
        }
    });

    $('#allPublicationsPan').on('click', '.followAllBtn', function () {
        var $this = $(this),
            curpublicPan = $this.closest('.publicationPan'),
            thead = curpublicPan.find('thead.hidden-xs'),
            firstRow = thead.find('tr:first-child'),
            tbody = curpublicPan.find('tbody'),
            div = $this.closest('div'),
            $lgfollow = curpublicPan.find('.followBtn'),
            table = $('.table'),
            lableStatus = '';
        $this.addClass('hideBtn');
        $('#validatePreference').val(1);
        div.find('.unfollowAllBtn').removeClass('hideBtn');
        curpublicPan.find('.firstrow .lableStatus').val('followinglbl');
        curpublicPan.find('.accordionStatus .lableStatus').val('followinglbl');
        $lgfollow.addClass('followingBtn').removeClass('followBtn').html($('#followingButtonText').val());
        $('#validatePriority').val(true);
        $('#validateMyViewPriority').val(true);

        lableStatus = thead.find('.lableStatus').val();
        thead.find('.mtp').addClass('hideBtn');
        thead.find('.mtp.' + lableStatus).removeClass('hideBtn');
        thead.removeClass('followbg').addClass('followingbg');

        for (var i = 0; i < tbody.find('.followingBtn').length; i++) {
            $(tbody.find('.followrow')[i]).attr('draggable', true);
        }
        if ($('.myview-settings-registration').length == 0) {
            curpublicPan.addClass('active');
        }
        curpublicPan.find('.unfollowAllBtn').removeClass('hideBtn');
        for (var i = 0; i < $lgfollow.length; i++) {
            $($lgfollow[i], curpublicPan).closest('tr').removeAttr('class').addClass('followingrow');
        }
        setClsforFlw(table);
        sort_table(tbody, 0, 1, 'followingrow');
    });

    $('#allPublicationsPan').on('click', '.unfollowAllBtn', function () {
        var $this = $(this),
            curpublicPan = $this.closest('.publicationPan'),
            thead = curpublicPan.find('thead.hidden-xs'),
            firstRow = thead.find('tr:first-child'),
            tbody = curpublicPan.find('tbody'),
            div = $this.closest('div'),
            $lgfollowing = curpublicPan.find('.followingBtn'),
            lableStatus = '';
        $this.addClass('hideBtn');
        $this.closest('.smfollowingBtn').find('.followAllBtn').addClass('fr');
        $('#validatePreference').val(1);
        div.find('.followAllBtn').removeClass('hideBtn');
        curpublicPan.find('.firstrow .lableStatus').val('followlbl');
        curpublicPan.find('.accordionStatus .lableStatus').val('followlbl');
        $lgfollowing.addClass('followBtn').removeClass('followingBtn').html($('#followButtonText').val());
        $('#validatePriority').val(false);
        $('#validateMyViewPriority').val(true);

        lableStatus = thead.find('.lableStatus').val();
        thead.find('.mtp').addClass('hideBtn');
        thead.find('.mtp.' + lableStatus).removeClass('hideBtn');
        thead.removeClass('followingbg').addClass('followbg');

        curpublicPan.removeClass('active');
        curpublicPan.find('tbody .frow').removeClass('frow');
        for (var i = 0; i < $lgfollowing.length; i++) {
            $($lgfollowing[i], curpublicPan).closest('tr').removeAttr('class').addClass('followrow disabled');
        }
        sort_table(tbody, 0, 1, 'followrow');
    });

    $('#allPublicationsPan .donesubscribe').on('click', '.followrow .followBtn', function (e) {
        var $this = $(this),
            currenttr = $this.closest('tr'),
            currentTopic = $.trim(currenttr.find('.wd-55').html().split('<input')[0]),
            currentChannel = currenttr.closest('.table').find('thead h2').html(),
            eventDetails;
        if ($('.registrationBtn') && $('.registrationBtn').length) {
            eventDetails = { "event_name": "channel_follow", "page_name": "Registration", "ga_eventCategory": "Channel Follow", "ga_eventAction": analytics_data["publication"], "ga_eventLabel": currentTopic, "follow_publication": analytics_data["publication"], "follow_channel": currentTopic };
        } else {
            eventDetails = { "event_name": "topic_follow", "page_name": "My view settings", "ga_eventCategory": "Topic Follow", "ga_eventAction": analytics_data["publication"] + ':' + currentChannel, "ga_eventLabel": currentTopic, "follow_publication": analytics_data["publication"], "follow_topic": currentTopic, "follow_channel": currentChannel };
        }
        (0, _controllersAnalyticsController.analyticsEvent)(eventDetails);

        eventDetails = {};
    });

    $('#allPublicationsPan .donesubscribe').on('click', '.followingrow .followingBtn', function (e) {
        var $this = $(this),
            currenttr = $this.closest('tr'),
            currentTopic = $.trim(currenttr.find('.wd-55').html().split('<input')[0]),
            currentChannel = currenttr.closest('.table').find('thead h2').html(),
            eventDetails;
        if ($('.registrationBtn') && $('.registrationBtn').length) {
            eventDetails = { "event_name": "channel_unfollow", "page_name": "Registration", "ga_eventCategory": "Channel Unfollow", "ga_eventAction": analytics_data["publication"], "ga_eventLabel": currentTopic, "follow_publication": analytics_data["publication"], "follow_channel": currentTopic };
        } else {
            eventDetails = { "event_name": "topic_unfollow", "page_name": "My view settings", "ga_eventCategory": "Topic Unfollow", "ga_eventAction": analytics_data["publication"] + ':' + currentChannel, "ga_eventLabel": currentTopic, "follow_publication": analytics_data["publication"], "follow_topic": currentTopic, "follow_channel": currentChannel };
        }
        (0, _controllersAnalyticsController.analyticsEvent)(eventDetails);

        eventDetails = {};
    });

    $('#allPublicationsPan .donesubscribe').on('click', '.followrow .followBtn', function (e) {
        var $this = $(this),
            followrow = $this.closest('.followrow'),
            table = $this.closest('.table'),
            curpublicPan = $this.closest('.publicationPan'),
            thead = table.find('thead.hidden-xs'),
            followAllBtn = table.find('.followAllBtn'),
            unfollowAllBtn = table.find('.unfollowAllBtn'),
            trs = $this.closest('tbody').find('tr'),
            trsfollowing = $this.closest('tbody').find('tr.followingrow'),
            lableStatus = '';
        followrow.attr('draggable', true);
        $('#validatePreference').val(1);
        followrow.addClass('followingrow').removeClass('followrow disabled frow');
        $this.addClass('followingBtn').removeClass('followBtn').html($('#followingButtonText').val());
        setClsforFlw(table);
        table.find('.firstrow .lableStatus').val('followinglbl');
        table.find('.accordionStatus .lableStatus').val('followinglbl');
        $('#validateMyViewPriority').val(true);

        lableStatus = thead.find('.lableStatus').val();
        thead.find('.mtp').addClass('hideBtn');
        thead.find('.mtp.' + lableStatus).removeClass('hideBtn');
        thead.removeClass('followbg').addClass('followingbg');
        if ($('.myview-settings-registration').length == 0) {
            curpublicPan.addClass('active');
        }

        if (trs.hasClass('followingrow')) {
            $('#validatePriority').val(true);
            //unfollowAllBtn.addClass('hideBtn');
        }

        if ($('.followrow.disabled.frow', table).length) {
            followrow.appendTo(followrow.clone().insertBefore(table.find('.followrow.disabled.frow')));
        } else {
            followrow.clone().appendTo($this.closest('tbody'));
        }
        followrow.remove();
        if (trs.length === trsfollowing.length + 1) {
            followAllBtn.addClass('hideBtn');
            unfollowAllBtn.removeClass('hideBtn');
        } else {
            followAllBtn.removeClass('hideBtn');
            unfollowAllBtn.removeClass('hideBtn');
        }
    });

    $('#allPublicationsPan .donesubscribe').on('mouseenter', '.followBtn', function (e) {
        $(this).html($('#followText').val());
    }).on('mouseleave', '.followBtn', function () {
        $(this).html($('#followButtonText').val());
    });

    $('#allPublicationsPan .donesubscribe').on('click', '.followingrow .followingBtn', function (e) {
        var $this = $(this),
            table = $this.closest('table'),
            curpublicPan = $this.closest('.publicationPan'),
            followAllBtn = $this.closest('table').find('.followAllBtn'),
            thead = table.find('thead.hidden-xs'),
            unfollowAllBtn = $this.closest('table').find('.unfollowAllBtn'),
            followingrow = $this.closest('.followingrow'),
            tbody = $this.closest('tbody'),
            trs = $this.closest('tbody').find('tr'),
            disabledtrs = $this.closest('tbody').find('.followrow.disabled'),
            trsfollow = $this.closest('tbody').find('tr.followrow'),
            lableStatus = '';
        followingrow.addClass('followrow disabled').removeClass('followingrow');
        $this.addClass('followBtn').removeClass('followingBtn').html($('#followButtonText').val());
        followingrow.clone().appendTo($this.closest('tbody'));
        followingrow.remove();
        $('#validatePreference').val(1);
        sort_table(tbody, 0, 1, 'followingBtn');
        $('#validateMyViewPriority').val(true);

        if (trs.length === disabledtrs.length + 1) {
            table.find('.firstrow .lableStatus').val('followlbl');
            table.find('.accordionStatus .lableStatus').val('followlbl');
            curpublicPan.removeClass('active');
            thead.removeClass('followingbg').addClass('followbg');
        }
        if (trs.length === trsfollow.length + 1) {
            unfollowAllBtn.addClass('hideBtn');
            followAllBtn.removeClass('hideBtn');

            $('#validatePriority').val(false);
        } else {
            followAllBtn.removeClass('hideBtn');
            unfollowAllBtn.removeClass('hideBtn');
        }
        lableStatus = thead.find('.lableStatus').val();
        thead.find('.mtp').addClass('hideBtn');
        thead.find('.mtp.' + lableStatus).removeClass('hideBtn');
    });

    $('#allPublicationsPan .donesubscribe').on('mouseenter', '.followingBtn', function (e) {
        $(this).html($('#unfollowText').val());
    }).on('mouseleave', '.followingBtn', function () {
        $(this).html($('#followingButtonText').val());
    });

    $('.publicationPan').on('click', '.accordionImg .mobileMode', function () {
        var $this = $(this),
            allPublications = $('#allPublicationsPan'),
            pPan = $this.closest('.publicationPan'),
            thead = pPan.find('thead'),
            tbody = pPan.find('tbody'),
            trs = tbody.find('tr'),
            disabledtrs = tbody.find('tr.disabled'),
            followlbl = thead.find('.followlbl'),
            followinglbl = thead.find('.followinglbl'),
            accStatusflwLbl = thead.find('.accordionStatus.flwLbl'),
            accStatusflwBtn = thead.find('.accordionStatus.flwBtn'),
            allpubpans = allPublications.find('.publicationPan'),
            pickTxt = thead.find('.pickTxt'),
            setFlag = true;

        if ($this.hasClass('expanded')) {
            setFlag = false;
            tbody.addClass('tbodyhidden');
            //pPan.find('.smfollowingBtn').hide(); 
            accStatusflwLbl.removeClass('hideRow');
            accStatusflwBtn.addClass('hideRow');
            thead.find('.mtp').addClass('hideBtn');

            for (var i = 0; i < pickTxt.length; i++) {
                $(pickTxt[i]).closest('.accordionStatus').addClass('hideRow');
            }
            if (trs.length === disabledtrs.length) {
                followlbl.removeClass('hideBtn');
            } else {
                followinglbl.removeClass('hideBtn');
            }
            var position = $this.closest('.publicationPan').position();
            $(window).scrollTop(position.top - 40);
        } else {
            allPublications.find('tbody').addClass('tbodyhidden');
            for (var i = 0; i < allpubpans.length; i++) {
                var eachPickTxt = $(allpubpans[i]).find('thead .pickTxt');
                for (var j = 0; j < eachPickTxt.length; j++) {
                    $(eachPickTxt[j]).closest('.accordionStatus').addClass('hideRow');;
                }
            }
            thead.find('tr').removeClass('hidden');
            tbody.removeClass('tbodyhidden');
            pPan.find('.smfollowingBtn').show();
            for (var i = 0; i < pickTxt.length; i++) {
                $(pickTxt[i]).closest('.accordionStatus').removeClass('hideRow');
            }
            if (setFlag) {
                for (var i = 0; i < allpubpans.length; i++) {
                    $(allpubpans[i]).find('.accordionStatus.flwLbl').removeClass('hideRow');
                    $(allpubpans[i]).find('.accordionStatus.flwBtn').addClass('hideRow');
                }
            }
            accStatusflwLbl.addClass('hideRow');
            accStatusflwBtn.removeClass('hideRow');

            var position = $this.closest('.publicationPan').position();
            $(window).scrollTop(position.top - 40);

            for (var i = 0; i < allpubpans.length; i++) {
                var labelVal = $(allpubpans[i]).find('.firstrow .lableStatus').val();
                $('.' + labelVal, allpubpans[i]).removeClass('hideBtn');
            }
            thead.find('.mtp').addClass('hideBtn');
        }
    });

    $('#allPublicationsPan .publicationPan').on('click', 'thead.hidden-xs tr:first-child', function (e) {
        var $this = $(this),
            allPublications = $('#allPublicationsPan'),
            pPan = $this.closest('.publicationPan'),
            accCont = pPan.find('.accCont'),
            thead = pPan.find('thead'),
            tbody = pPan.find('tbody'),
            trs = tbody.find('tr'),
            disabledtrs = tbody.find('tr.disabled'),
            flwlbl = thead.find('.flwLbl'),
            flwBtn = thead.find('.flwBtn'),
            followlbl = thead.find('.followlbl'),
            followinglbl = thead.find('.followinglbl'),
            allpubpans = allPublications.find('.publicationPan'),
            allthead = $this.closest('#allPublicationsPan').find('.publicationPan thead.hidden-xs'),
            lableStatus = allthead.find('.lableStatus').val();

        if ($('.myview-settings-registration').length == 0) {
            allPublications.find('.publicationPan thead.hidden-xs tr:first-child').not($(this)).removeClass('expanded').addClass('collapsed');

            if (e.target.className !== 'subscribed' && e.target.className !== 'rowlines' && e.target.className !== 'pull-left' && e.target.className !== 'mv' && e.target.className !== 'subscr') {
                if ($this.hasClass('expanded')) {
                    $this.removeClass('expanded').addClass('collapsed');
                    tbody.addClass('tbodyhidden');
                    thead.find('.mtp').addClass('hideBtn');
                    accCont.addClass('tbodyhidden');
                    if (trs.length === disabledtrs.length) {
                        followlbl.removeClass('hideBtn');
                        thead.find('.firstrow .lableStatus').val('followlbl');
                    } else {
                        followinglbl.removeClass('hideBtn');
                        thead.find('.firstrow .lableStatus').val('followinglbl');
                    }
                    for (var i = 0; i < allthead.length; i++) {
                        var curthead = $(allthead[i]),
                            getlableStatus = curthead.find('.lableStatus').val();
                        curthead.removeClass('followingbg followbg').addClass(getlableStatus == 'followinglbl' ? 'followingbg' : 'followbg');
                    }
                    allPublications.find('.sorting_arrow--up').removeClass('act').addClass('hide');
                    allPublications.find('.sorting_arrow--down').removeClass('hide');
                    pPan.removeClass('active');
                    var position = $this.closest('.publicationPan').position();
                    $(window).scrollTop(position.top);
                } else {
                    allPublications.find('tbody').addClass('tbodyhidden');
                    allPublications.find('.publicationPan .accordionImg span.accImg .sorting_arrow--up').removeClass('act').addClass('hide');
                    allPublications.find('.publicationPan .accordionImg span.accImg .sorting_arrow--down').removeClass('hide');
                    allPublications.find('.publicationPan thead tr').not(':nth-child(1)').addClass('hidden');
                    allPublications.find('.publicationPan thead tr.showinview').removeClass('hidden');
                    thead.find('tr').removeClass('hidden');
                    $this.addClass('expanded').removeClass('collapsed');
                    accCont.removeClass('tbodyhidden');
                    tbody.removeClass('tbodyhidden');
                    flwBtn.addClass('hideRow');
                    flwlbl.removeClass('hideRow');

                    pPan.find('.sorting_arrow--up').addClass('act').removeClass('hide');
                    pPan.find('.sorting_arrow--down').addClass('hide');
                    pPan.find('.expandTxt').removeAttr('style');
                    pPan.find('.mvTxt').removeAttr('style');

                    allPublications.find('.publicationPan').removeClass('active');
                    if (trs.length == disabledtrs.length) {
                        pPan.removeClass('active');
                    } else {
                        pPan.addClass('active');
                    }
                    for (var i = 0; i < allthead.length; i++) {
                        var curthead = $(allthead[i]),
                            getlableStatus = curthead.find('.lableStatus').val();
                        curthead.removeClass('followingbg followbg').addClass(getlableStatus == 'followinglbl' ? 'followingbg' : 'followbg');
                    }

                    var position = $this.closest('.publicationPan').position();
                    $(window).scrollTop(position.top);
                }
            }
        }
    }).on('mouseenter', 'thead.hidden-xs tr:first-child', function () {
        var $this = $(this),
            firstTrtds = $this.find('th'),
            thead = $this.closest('thead.hidden-xs'),
            lableStatus = $this.find('.lableStatus').val();
        firstTrtds.addClass('active');
        if ($this.hasClass('collapsed') && lableStatus == 'followinglbl') {
            thead.removeClass('followinglbl-txt followlbl-txt').addClass(lableStatus + '-txt');
            thead.find('.mvTxt').css('visibility', 'visible');
            thead.find('.expandTxt').css('visibility', 'visible');
            thead.find('.accImg .sorting_arrow--down').addClass('act');
        } else if ($this.hasClass('collapsed') && lableStatus == 'followlbl') {
            //thead.removeClass('followinglbl-txt followlbl-txt').addClass(lableStatus + '-txt');
            thead.find('.expandTxt').css('visibility', 'visible');
        }
    }).on('mouseleave', 'thead.hidden-xs tr:first-child', function () {
        var $this = $(this),
            firstTrtds = $this.find('th'),
            thead = $this.closest('thead.hidden-xs');
        firstTrtds.removeClass('active');
        if ($this.hasClass('collapsed')) {
            thead.removeClass('followinglbl-txt followlbl-txt');
            thead.find('.mvTxt').removeAttr('style');
            thead.find('.expandTxt').removeAttr('style');
            thead.find('.accImg .sorting_arrow--down').removeClass('act');
        }
    });

    var tables = $('.publicationPan table');
    setClsforFlw(tables);

    $('.saveview').click(function (e) {
        /*if ($('.modal-overlay').hasClass('in')) {
            window.location.href = clickedUrl;
        }*/
        //else {
        var alltables = $('.table'),
            allpublicationsEles = $('.publicationPan'),
            isChannelLevel = $('#isChannelBasedRegistration').val(),
            UserPreferences = { "IsNewUser": false, "IsChannelLevel": isChannelLevel },
            allpublications = $('.publicationPan', '#allPublicationsPan');
        UserPreferences.PreferredChannels = [];
        $('#validateMyViewPriority').val(false);
        e.preventDefault();
        setDataRow(allpublications);
        allpublicationsEles.removeAttr('data-row');
        for (var i = 0; i < allpublicationsEles.length; i++) {
            var j = i + 1;
            $(allpublicationsEles[i]).attr('data-row', j);
        }

        if ($(this).hasClass('validationChk')) {
            createJSONData(alltables, UserPreferences, clickedUrl);
        } else {
            createJSONData(alltables, UserPreferences);
            $('#validatePreference').val(0);
        }
        //}
    });

    $('.registrationBtn').click(function (e) {
        var table = $('.table', '.publicationPan'),
            alltrs = table.find('tbody tr'),
            isChannelLevel = $('#isChannelBasedRegistration').val(),
            UserPreferences = { "IsNewUser": true, "IsChannelLevel": isChannelLevel },
            allpublications = $('.publicationPan', '#allPublicationsPan');
        UserPreferences.PreferredChannels = [];

        e.preventDefault();
        if ($('#validatePriority').val() == "true" && $('#enableSavePreferencesCheck').val() === "false") {
            setDataRow(allpublications);
            sendRegisterData(alltrs, UserPreferences, 'name');
            return false;
        }
        if ($('#validatePriority').val() == "false") {
            if ($('#enableSavePreferencesCheck').val() === "true" && table.find('.followingrow').length == 0) {
                $('.alert-error.register-not-selected').show();
                return false;
            }
            setDataRow(allpublications);

            if ($('#isChannelBasedRegistration').val() == "true") {
                sendRegisterData(alltrs, UserPreferences, 'href');
            } else {
                createJSONData(table, UserPreferences);
            }
        } else {
            setDataRow(allpublications);
            sendRegisterData(alltrs, UserPreferences, 'href');
        }
    });

    $('.gotoview').click(function (e) {
        if ($('#validatePriority') && $('#validatePriority').val() == "true") {
            showModal();
        } else {
            if (+$('#validatePreference').val()) {
                e.preventDefault();
                showModal();
            }
        }
    });

    $('.close-modal').click(function () {
        $('.modal-overlay').removeClass('in');
        $('.modal-view').hide();
    });

    $('.cancel-modal').click(function () {
        window.location.href = clickedUrl;
    });

    if ($('.publicationPan') && $('.publicationPan').length) {
        $('.publicationPan.donesubscribe').dragswap({
            element: '.table tbody tr',
            dropAnimation: true
        });

        $('#allPublicationsPan').dragswap({
            element: '.publicationPan.donesubscribe',
            dropAnimation: true
        });
    }

    $(document).on('click', '.editView', function () {
        var eventDetails = { "event_name": "myview_edit_my_view", "page_name": analytics_data["page_name"], "ga_eventCategory": "My View Page Link", "ga_eventAction": "Link Click", "ga_eventLabel": "EDIT MY VIEW" };
        (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, eventDetails));
        eventDetails = {};
    });

    $('.personalisationPan').on('click', '.loadmore', function () {
        var id = $(this).closest('.eachstoryMpan').find('.eachstory').attr('id'),
            getIdx = 0;
        for (var i = 0; i < loadPreferanceId["Sections"].length; i++) {
            if (loadPreferanceId["Sections"][i]["ChannelId"] == id) {
                getIdx = i;
                break;
            }
        }
        var eventDetails = { "event_name": "myview_load_more", "page_name": analytics_data["page_name"], "ga_eventCategory": "My View Page Publications", "ga_eventAction": analytics_data["publication"], "ga_eventLabel": loadPreferanceId["Sections"][getIdx]["ChannelName"], "publication_click": analytics_data["publication"] };

        (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, eventDetails));
        eventDetails = {};
    });
});

},{"../controllers/analytics-controller":40}],34:[function(require,module,exports){
'use strict';

window.paginationdefaults = {
	totalCategories: 30,
	categoryLimit: 10,
	currentPage: 1,
	paginationEle: 'table'
};

window.setPagination = function (source) {
	$.extend(window.paginationdefaults, source);
};

function paginationCur(fv, tv) {
	if (window.paginationdefaults.paginationEle === 'table') {
		$('tbody.hidden-xs tr', '.page-account__table').hide();
		$('tbody.hidden-lg tr', '.page-account__table').hide();
		for (var i = fv; i < tv; i++) {
			$('tbody.hidden-xs tr', '.page-account__table').eq(i).show();
			$('tbody.hidden-lg tr', '.page-account__table').eq(i).show();
		}
	} else {
		$(window.paginationdefaults.paginationEle).hide();
		for (var i = fv; i < tv; i++) {
			$(window.paginationdefaults.paginationEle).eq(i).show();
		}
	}
}

window.loadPaginationNums = function () {
	var showPageLinks = Math.ceil(window.paginationdefaults.totalCategories / window.paginationdefaults.categoryLimit),
	    linkStr = '';
	for (var i = 1; i <= showPageLinks; i++) {
		linkStr += '<a href="javascript:void(0);">' + i + '</a>';
	}
	if (showPageLinks > 1) {
		$('.pagination span').html(linkStr);
	} else {
		$('.pagination').hide();
	}

	$('.pagination span a:eq(0)').click();
	$('.pagination a:eq(0)').removeAttr('href');
};

$(function () {
	$('.pagination').on('click', 'a', function () {
		var $this = $(this),
		    $val = $this.html();
		if ($val.toLowerCase().indexOf('prev') >= 0) {
			var idx = +$('.pagination span a.active').html() - 1,
			    toVal = idx * window.paginationdefaults.categoryLimit,
			    fromVal = toVal - window.paginationdefaults.categoryLimit;

			if ($('.pagination span a:first').hasClass('active')) return;
			paginationCur(fromVal, toVal);

			var curidx = $('.pagination span a.active').index(),
			    pagesLen = $('.pagination li > span a').length - 1;
			$('.pagination span a').eq(curidx).removeClass('active');
			$('.pagination span a').eq(curidx).prev('a').addClass('active');
			$('.pagination a:last').attr('href', 'javascript:void(0);');
			if (curidx == 0) {
				$('.pagination a:eq(0)').removeAttr('href');
			}
		} else if ($val.toLowerCase().indexOf('next') >= 0) {
			var idx = +$('.pagination span a.active').html() + 1,
			    toVal = idx * window.paginationdefaults.categoryLimit,
			    fromVal = toVal - window.paginationdefaults.categoryLimit;

			if ($('.pagination span a:last').hasClass('active')) return;
			paginationCur(fromVal, toVal);

			var curidx = $('.pagination span a.active').index(),
			    pagesLen = $('.pagination li > span a').length - 1;
			$('.pagination span a').eq(curidx).removeClass('active');
			$('.pagination span a').eq(curidx).next('a').addClass('active');
			$('.pagination a:first').attr('href', 'javascript:void(0);');
			if (curidx == pagesLen) {
				$('.pagination a:last').removeAttr('href');
			}
		} else {
			if (!$this.hasClass('active')) {
				$('.pagination span a').removeClass('active').attr('href', 'javascript:void(0);');
				$this.addClass('active').removeAttr('href');
				toVal = window.paginationdefaults.categoryLimit * $val;
				fromVal = toVal - window.paginationdefaults.categoryLimit;
				paginationCur(fromVal, toVal);
				$('.pagination a:last').attr('href', 'javascript:void(0);');
				$('.pagination a:first').attr('href', 'javascript:void(0);');
				if ($('.pagination span a.active').next('a').length == 0) {
					$('.pagination a:last').removeAttr('href');
				}
				if ($('.pagination span a.active').prev('a').length == 0) {
					$('.pagination a:first').removeAttr('href');
				}
			}
		}
	});

	$('.sortable-table__header').on('click', '.sortable-table__col', function () {
		var $this = $(this),
		    table = $this.closest('.sortable-table'),
		    tbodytrs = table.find('tbody tr');
		setTimeout(function () {
			tbodytrs.removeAttr('style');
			if (!$('.pagination span a:eq(0)').hasClass('active')) {
				$('.pagination span a:eq(0)').click();
			} else {
				paginationCur(0, window.paginationdefaults.categoryLimit);
			}
		}, 1);
	});
});

},{}],35:[function(require,module,exports){
'use strict';

function loadLayoutOneData(data, idx) {
	var editMyView = loadPreferanceId.EditMyViewButtonLableText ? '<a class="editView button--filled button--outline mobview" href="' + loadPreferanceId.MyViewSettingsPageLink + '">' + loadPreferanceId.EditMyViewButtonLableText + '</a>' : '';
	var seeAllTopics = data.loadMore && data.loadMore.seeAllLink ? '<a class="seeAllChannels button--filled button--outline mobview" href="' + data.loadMore.seeAllLink + loadPreferanceId["Sections"][idx]["ChannelName"] + '">' + data.loadMore.seeAllText + ' ' + loadPreferanceId["Sections"][idx]["ChannelName"] + '</a>' : '';

	var loadData = loadPreferanceId["Sections"][idx]["ChannelName"] ? '<div class="latestSubject clearfix" id="' + loadPreferanceId["Sections"][idx].ChannelId + '"><div class="articleloadInfo">' + data.loadMore.currentlyViewingText + '</div><div class="fllatestSub"><span class="sub">' + data.loadMore.latestFromText + ' ' + loadPreferanceId["Sections"][idx]["ChannelName"] + '</span></div><div class="frEditview">' + editMyView + seeAllTopics + '</div></div>' : '',
	    loadmoreLink = data.loadMore && data.loadMore.displayLoadMore ? data.loadMore.loadMoreLinkUrl : '#';
	loadData += '<div class="eachstoryMpan">';
	loadData += loadPreferanceId["Sections"][idx].ChannelId ? '<div class="eachstory layout1">' : '';
	loadData += createLayoutInner1(data);
	loadData += '</div>';
	loadData += data.loadMore && data.loadMore.displayLoadMore ? '<div class="loadmore"><span href="' + loadmoreLink + '">' + data.loadMore.loadMoreLinkText + ' ' + loadPreferanceId["Sections"][idx]["ChannelName"] + '</span></div>' : '';
	loadData += '</div>';

	//loadData += '<div class="googleAdd"><img src="/dist/img/google-add.gif"></div>';

	return loadData;
}

function createLayoutInner1(data) {
	var isArticleBookmarked = data.articles[0].isArticleBookmarked ? data.articles[0].bookmarkedText : data.articles[0].bookmarkText,
	    bookmarkTxt = data.articles[0].bookmarkText || data.articles[0].bookmarkedText ? '<span class="action-flag__label js-bookmark-label">' + isArticleBookmarked + '</span>' : '',
	    linkableUrl0 = data.articles[0].linkableUrl ? data.articles[0].linkableUrl : '#',
	    linkableUrl1 = data.articles[1].linkableUrl ? data.articles[1].linkableUrl : '#',
	    linkableUrl2 = data.articles[2].linkableUrl ? data.articles[2].linkableUrl : '#',
	    linkableUrl3 = data.articles[3].linkableUrl ? data.articles[3].linkableUrl : '#',
	    linkableUrl4 = data.articles[4].linkableUrl ? data.articles[4].linkableUrl : '#',
	    linkableUrl5 = data.articles[5].linkableUrl ? data.articles[5].linkableUrl : '#',
	    linkableUrl6 = data.articles[6].linkableUrl ? data.articles[6].linkableUrl : '#',
	    linkableUrl7 = data.articles[7].linkableUrl ? data.articles[7].linkableUrl : '#',
	    linkableUrl8 = data.articles[8].linkableUrl ? data.articles[8].linkableUrl : '#',
	    bookmarkInfo0 = data.articles[0].isArticleBookmarked ? data.articles[0].bookmarkedText : data.articles[0].bookmarkText,
	    bookmarkInfo1 = data.articles[1].isArticleBookmarked ? data.articles[1].bookmarkedText : data.articles[1].bookmarkText,
	    bookmarkInfo2 = data.articles[2].isArticleBookmarked ? data.articles[2].bookmarkedText : data.articles[2].bookmarkText,
	    bookmarkInfo3 = data.articles[3].isArticleBookmarked ? data.articles[3].bookmarkedText : data.articles[3].bookmarkText,
	    bookmarkInfo4 = data.articles[4].isArticleBookmarked ? data.articles[4].bookmarkedText : data.articles[4].bookmarkText,
	    bookmarkInfo5 = data.articles[5].isArticleBookmarked ? data.articles[5].bookmarkedText : data.articles[5].bookmarkText,
	    bookmarkInfo6 = data.articles[6].isArticleBookmarked ? data.articles[6].bookmarkedText : data.articles[6].bookmarkText,
	    bookmarkInfo7 = data.articles[7].isArticleBookmarked ? data.articles[7].bookmarkedText : data.articles[7].bookmarkText,
	    bookmarkInfo8 = data.articles[8].isArticleBookmarked ? data.articles[8].bookmarkedText : data.articles[8].bookmarkText,
	    fbookmarkIcon0 = data.articles[0].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon0 = data.articles[0].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon1 = data.articles[1].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon1 = data.articles[1].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon2 = data.articles[2].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon2 = data.articles[2].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon3 = data.articles[3].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon3 = data.articles[3].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon4 = data.articles[4].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon4 = data.articles[4].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon5 = data.articles[5].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon5 = data.articles[5].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon6 = data.articles[6].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon6 = data.articles[6].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon7 = data.articles[7].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon7 = data.articles[7].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon8 = data.articles[8].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon8 = data.articles[8].isArticleBookmarked ? '' : 'is-visible';

	var articleData = '';
	articleData += getListViewData(0, data, linkableUrl0, bookmarkInfo0, fbookmarkIcon0, sbookmarkIcon0);
	articleData += getListViewData(1, data, linkableUrl1, bookmarkInfo1, fbookmarkIcon1, sbookmarkIcon1);
	articleData += getListViewData(2, data, linkableUrl2, bookmarkInfo2, fbookmarkIcon2, sbookmarkIcon2);
	articleData += getListViewData(3, data, linkableUrl3, bookmarkInfo3, fbookmarkIcon3, sbookmarkIcon3);
	articleData += getListViewData(4, data, linkableUrl4, bookmarkInfo4, fbookmarkIcon4, sbookmarkIcon4);
	articleData += getListViewData(5, data, linkableUrl5, bookmarkInfo5, fbookmarkIcon5, sbookmarkIcon5);
	articleData += getListViewData(6, data, linkableUrl6, bookmarkInfo6, fbookmarkIcon6, sbookmarkIcon6);
	articleData += getListViewData(7, data, linkableUrl7, bookmarkInfo7, fbookmarkIcon7, sbookmarkIcon7);
	articleData += getListViewData(8, data, linkableUrl8, bookmarkInfo8, fbookmarkIcon8, sbookmarkIcon8);

	articleData += '<section class="article-preview topic-featured-article gridViewCont">';
	articleData += data.articles[0].listableImage ? '<img class="topic-featured-article__image" src="' + data.articles[0].listableImage + '">' : '';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[0].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[0].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[0].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[0].bookmarkText + '" data-label-bookmarked="' + data.articles[0].bookmarkedText + '">' + bookmarkInfo0 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[0].listableDate ? '<li><time class="article-metadata__date">' + data.articles[0].listableDate + '</time></li>' : '';
	articleData += data.articles[0].linkableText ? '<li><h6>' + data.articles[0].linkableText + '</h6></li>' : '';
	articleData += data.articles[0].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type" style="width: 0px; height: 0px;"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="' + data.articles[0].listableType + '"></use></svg><img src="' + data.articles[0].listableType + '" width="25" /></span></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="topic-featured-article__inner-wrapper">';
	articleData += data.articles[0].listableTitle ? '<h3 class="topic-featured-article__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[0].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[0].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[0].listableTitle + '</a></h3>' : '';
	articleData += data.articles[0].listableAuthorByLine ? '<span class="article-preview__byline">' + data.articles[0].listableAuthorByLine + '</span>' : '';
	articleData += '<div class="article-summary">' + data.articles[0].listableSummary ? data.articles[0].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[0].listableTopics) {
		for (var i = 0; i < data.articles[0].listableTopics.length; i++) {
			var getLink8 = data.articles[0].listableTopics[i].linkableUrl ? data.articles[0].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink8 + '">' + data.articles[0].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';

	articleData += '<div class="latest-news__articles">';
	articleData += '<section class="article-preview article-preview--small mobview gridViewCont">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[1].id + '" data-analytics="{"bookmark": "' + bookmarkInfo1 + '", "bookmark_title": "' + data.articles[1].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[1].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[1].bookmarkText + '" data-label-bookmarked="' + data.articles[1].bookmarkedText + '">' + bookmarkInfo1 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon1 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon1 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[1].listableDate ? '<li><time class="article-metadata__date">' + data.articles[1].listableDate + '</time></li>' : '';
	articleData += data.articles[1].linkableText ? '<li><h6>' + data.articles[1].linkableText + '</h6></li>' : '';
	articleData += data.articles[1].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type" style="width: 0px; height: 0px;"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#' + data.articles[1].listableType + '"></use></svg><img src="' + data.articles[1].listableType + '" width="25" /></span></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper">';
	articleData += data.articles[1].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl1 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[1].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[1].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[1].listableTitle + '</a></h1>' : '';
	articleData += data.articles[1].listableAuthorByLine ? '<span class="article-preview__byline">' + data.articles[1].listableAuthorByLine + '</span>' : '';
	articleData += '<div class="article-summary">';
	articleData += data.articles[1].listableSummary ? data.articles[1].listableSummary : '';
	articleData += '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[1].listableTopics) {
		for (var i = 0; i < data.articles[1].listableTopics.length; i++) {
			var getLink1 = data.articles[1].listableTopics[i].linkableUrl ? data.articles[1].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink1 + '">' + data.articles[1].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';

	articleData += '<section class="article-preview article-preview--small mobview gridViewCont">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[2].id + '" data-analytics="{"bookmark": "' + bookmarkInfo2 + '", "bookmark_title": "' + data.articles[2].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[2].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[2].bookmarkText + '" data-label-bookmarked="' + data.articles[2].bookmarkedText + '">' + bookmarkInfo2 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon2 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon2 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[2].listableDate ? '<li><time class="article-metadata__date">' + data.articles[2].listableDate + '</time></li>' : '';
	articleData += data.articles[2].linkableText ? '<li><h6>' + data.articles[2].linkableText + '</h6></li>' : '';
	articleData += data.articles[2].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type" style="width: 0px; height: 0px;"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#' + data.articles[2].listableType + '"></use></svg><img src="' + data.articles[2].listableType + '" width="25" /></span></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper">';
	articleData += data.articles[2].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl2 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[2].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[2].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[2].listableTitle + '</a></h1>' : '';
	articleData += data.articles[2].listableAuthorByLine ? '<span class="article-preview__byline">' + data.articles[2].listableAuthorByLine + '</span>' : '';
	articleData += '<div class="article-summary">' + data.articles[2].listableSummary ? data.articles[2].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[2].listableTopics) {
		for (var i = 0; i < data.articles[2].listableTopics.length; i++) {
			var getLink2 = data.articles[2].listableTopics[i].linkableUrl ? data.articles[2].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink2 + '">' + data.articles[2].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';

	articleData += '<section class="article-preview article-preview--small topics gridViewCont">';
	articleData += data.articles[3].linkableText ? '<h6>&nbsp;</h6>' : '';

	articleData += data.articles[3].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl3 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[3].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[3].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[3].listableTitle + '</a></h1>' : '';

	articleData += data.articles[4].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl4 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[4].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[4].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[4].listableTitle + '</a></h1>' : '';

	articleData += data.articles[5].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl5 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[5].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[5].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[5].listableTitle + '</a></h1>' : '';

	articleData += '</section>';
	articleData += '</div>';

	articleData += '<div class="latest-news__articles">';
	articleData += '<section class="article-preview article-small-preview mobview gridViewCont">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[6].id + '" data-analytics="{"bookmark": "' + bookmarkInfo6 + '", "bookmark_title": "' + data.articles[6].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[6].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[6].bookmarkText + '" data-label-bookmarked="' + data.articles[6].bookmarkedText + '">' + bookmarkInfo6 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked  ' + fbookmarkIcon6 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon6 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[6].listableDate ? '<li><time class="article-metadata__date">' + data.articles[6].listableDate + '</time></li>' : '';
	articleData += data.articles[6].linkableText ? '<li><h6>' + data.articles[6].linkableText + '</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += data.articles[6].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl6 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[6].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[6].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[6].listableTitle + '</a></h1>' : '';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[6].listableTopics) {
		for (var i = 0; i < data.articles[6].listableTopics.length; i++) {
			var getLink6 = data.articles[6].listableTopics[i].linkableUrl ? data.articles[6].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink6 + '">' + data.articles[6].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';

	articleData += '<section class="article-preview article-small-preview mobview gridViewCont">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[7].id + '" data-analytics="{"bookmark": "' + bookmarkInfo7 + '", "bookmark_title": "' + data.articles[7].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[7].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[7].bookmarkText + '" data-label-bookmarked="' + data.articles[7].bookmarkedText + '">' + bookmarkInfo7 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon7 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon7 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[7].listableDate ? '<li><time class="article-metadata__date">' + data.articles[7].listableDate + '</time></li>' : '';
	articleData += data.articles[7].linkableText ? '<li><h6>' + data.articles[7].linkableText + '</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += data.articles[7].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl7 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[7].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[7].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[7].listableTitle + '</a></h1>' : '';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[7].listableTopics) {
		for (var i = 0; i < data.articles[7].listableTopics.length; i++) {
			var getLink7 = data.articles[7].listableTopics[i].linkableUrl ? data.articles[7].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink7 + '">' + data.articles[7].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';

	articleData += '<section class="article-preview article-small-preview mobview gridViewCont">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[8].id + '" data-analytics="{"bookmark": "' + bookmarkInfo8 + '", "bookmark_title": "' + data.articles[8].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[8].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[8].bookmarkText + '" data-label-bookmarked="' + data.articles[8].bookmarkedText + '">' + bookmarkInfo8 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon8 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon8 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[8].listableDate ? '<li><time class="article-metadata__date">' + data.articles[8].listableDate + '</time></li>' : '';
	articleData += data.articles[8].linkableText ? '<li><h6>' + data.articles[8].linkableText + '</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += data.articles[8].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl8 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[8].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[8].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[8].listableTitle + '</a></h1>' : '';
	articleData += data.articles[8].listableAuthorByLine ? '<span class="article-preview__byline">' + data.articles[8].listableAuthorByLine + '</span>' : '';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[8].listableTopics) {
		for (var i = 0; i < data.articles[8].listableTopics.length; i++) {
			var getLink8 = data.articles[8].listableTopics[i].linkableUrl ? data.articles[8].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink8 + '">' + data.articles[8].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	articleData += '</div>';

	articleData += '<input type="hidden" class="getPaginationNum" data-pageSize="' + data.loadMore.pageSize + '" data-pageNo="' + data.loadMore.pageNo + '" data-loadurl="' + data.loadMore.loadMoreLinkUrl + '" data-taxonomyIds="' + data.loadMore.taxonomyIds + '" />';

	return articleData;
}

function getListViewData(idx, data, linkableUrl, bookmarkInfo, fbookmarkIcon, sbookmarkIcon) {
	var sectionData = '';
	sectionData += '<section class="article-preview list-featured-article listViewCont">';
	sectionData += data.articles[idx].listableImage ? '<div class="topic-article-image_pan"><img class="topic-featured-article__image" src="' + data.articles[idx].listableImage + '"></div>' : '';
	sectionData += data.articles[idx].listableTitle ? '<div class="topic-article-rig_pan"><h3 class="topic-featured-article__headline"><a href="' + linkableUrl + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[idx].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[idx].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[idx].listableTitle + '</a></h3>' : '';
	sectionData += '<div class="topic-featured-article__inner-wrapper">';
	sectionData += '<div class="article-metadata">';
	sectionData += '<div class="article-preview__byline">';
	sectionData += data.articles[idx].listableAuthorByLine ? '<div class="authorTitle">' + data.articles[idx].listableAuthorByLine + '</div>' : '';
	sectionData += '<ul>';
	sectionData += data.articles[idx].listableDate ? '<li><time class="article-metadata__date">' + data.articles[idx].listableDate + '</time></li>' : '';
	sectionData += data.articles[idx].linkableText ? '<li><h6>' + data.articles[idx].linkableText + '</h6></li>' : '';
	sectionData += data.articles[idx].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type" style="width: 0px; height: 0px;"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#' + data.articles[idx].listableType + '"></use></svg><img src="' + data.articles[idx].listableType + '" width="25" /></span></li>' : '';
	sectionData += '<li><div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[idx].id + '" data-analytics="{"bookmark": "' + bookmarkInfo + '", "bookmark_title": "' + data.articles[idx].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[idx].isArticleBookmarked + '"><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div></li>';
	sectionData += '</ul>';
	sectionData += '</div>';
	sectionData += '</div>';
	sectionData += '<div class="article-summary">' + data.articles[idx].listableSummary ? data.articles[idx].listableSummary : '' + '</div>';
	sectionData += '</div>';
	sectionData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[idx].listableTopics) {
		for (var i = 0; i < data.articles[idx].listableTopics.length; i++) {
			var getlistLink1 = data.articles[idx].listableTopics[i].linkableUrl ? data.articles[idx].listableTopics[i].linkableUrl : '#';
			sectionData += '<a href="' + getlistLink1 + '">' + data.articles[idx].listableTopics[i].linkableText + '</a>';
		}
	}
	sectionData += '</div>';
	sectionData += '</div>';
	sectionData += '</section>';

	return sectionData;
}

function loadLayoutTwoData(data, idx) {
	var editMyView = loadPreferanceId.EditMyViewButtonLableText ? '<a class="editView button--filled button--outline mobview" href="' + loadPreferanceId.MyViewSettingsPageLink + '">' + loadPreferanceId.EditMyViewButtonLableText + '</a>' : '';
	var seeAllTopics = data.loadMore && data.loadMore.seeAllLink ? '<a class="seeAllChannels button--filled button--outline mobview" href="' + data.loadMore.seeAllLink + loadPreferanceId["Sections"][idx]["ChannelName"] + '">' + data.loadMore.seeAllText + ' ' + loadPreferanceId["Sections"][idx]["ChannelName"] + '</a>' : '';

	var loadData = loadPreferanceId["Sections"][idx]["ChannelName"] ? '<div class="latestSubject clearfix" id="' + loadPreferanceId["Sections"][idx].ChannelId + '"><div class="articleloadInfo">' + data.loadMore.currentlyViewingText + '</div><div class="fllatestSub"><span class="sub">' + data.loadMore.latestFromText + ' ' + loadPreferanceId["Sections"][idx]["ChannelName"] + '</span></div><div class="frEditview">' + editMyView + seeAllTopics + '</div></div>' : '',
	    loadmoreLink = data.loadMore && data.loadMore.displayLoadMore && data.loadMore.displayLoadMore.loadMoreLinkUrl ? data.loadMore.displayLoadMore.loadMoreLinkUrl : '#';
	loadData += '<div class="eachstoryMpan">';
	loadData += loadPreferanceId["Sections"][idx].ChannelId ? '<div class="eachstory layout2">' : '';
	loadData += createLayoutInner2(data);
	loadData += '</div>';

	loadData += data.loadMore && data.loadMore.displayLoadMore ? '<div class="loadmore"><span href="' + loadmoreLink + '">' + data.loadMore.loadMoreLinkText + ' ' + loadPreferanceId["Sections"][idx]["ChannelName"] + '</span></div>' : '';

	loadData += '</div>';

	//loadData += '<div class="googleAdd"><img src="/dist/img/google-add.gif"></div>';

	return loadData;
}

function createLayoutInner2(data) {
	var linkableUrl0 = data.articles[0].linkableUrl ? data.articles[0].linkableUrl : '#',
	    linkableUrl1 = data.articles[1].linkableUrl ? data.articles[1].linkableUrl : '#',
	    linkableUrl2 = data.articles[2].linkableUrl ? data.articles[2].linkableUrl : '#',
	    linkableUrl3 = data.articles[3].linkableUrl ? data.articles[3].linkableUrl : '#',
	    linkableUrl4 = data.articles[4].linkableUrl ? data.articles[4].linkableUrl : '#',
	    linkableUrl5 = data.articles[5].linkableUrl ? data.articles[5].linkableUrl : '#',
	    linkableUrl6 = data.articles[6].linkableUrl ? data.articles[6].linkableUrl : '#',
	    linkableUrl7 = data.articles[7].linkableUrl ? data.articles[7].linkableUrl : '#',
	    linkableUrl8 = data.articles[8].linkableUrl ? data.articles[8].linkableUrl : '#',
	    bookmarkInfo0 = data.articles[0].isArticleBookmarked ? data.articles[0].bookmarkedText : data.articles[0].bookmarkText,
	    bookmarkInfo1 = data.articles[1].isArticleBookmarked ? data.articles[1].bookmarkedText : data.articles[1].bookmarkText,
	    bookmarkInfo2 = data.articles[2].isArticleBookmarked ? data.articles[2].bookmarkedText : data.articles[2].bookmarkText,
	    bookmarkInfo3 = data.articles[3].isArticleBookmarked ? data.articles[3].bookmarkedText : data.articles[3].bookmarkText,
	    bookmarkInfo4 = data.articles[4].isArticleBookmarked ? data.articles[4].bookmarkedText : data.articles[4].bookmarkText,
	    bookmarkInfo5 = data.articles[5].isArticleBookmarked ? data.articles[5].bookmarkedText : data.articles[5].bookmarkText,
	    bookmarkInfo6 = data.articles[6].isArticleBookmarked ? data.articles[6].bookmarkedText : data.articles[6].bookmarkText,
	    bookmarkInfo7 = data.articles[7].isArticleBookmarked ? data.articles[7].bookmarkedText : data.articles[7].bookmarkText,
	    bookmarkInfo8 = data.articles[8].isArticleBookmarked ? data.articles[8].bookmarkedText : data.articles[8].bookmarkText,
	    fbookmarkIcon0 = data.articles[0].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon0 = data.articles[0].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon1 = data.articles[1].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon1 = data.articles[1].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon2 = data.articles[2].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon2 = data.articles[2].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon3 = data.articles[3].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon3 = data.articles[3].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon4 = data.articles[4].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon4 = data.articles[4].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon5 = data.articles[5].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon5 = data.articles[5].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon6 = data.articles[6].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon6 = data.articles[6].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon7 = data.articles[7].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon7 = data.articles[7].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon8 = data.articles[8].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon8 = data.articles[8].isArticleBookmarked ? '' : 'is-visible';

	var articleData = '';

	articleData += getListViewData(0, data, linkableUrl0, bookmarkInfo0, fbookmarkIcon0, sbookmarkIcon0);
	articleData += getListViewData(1, data, linkableUrl1, bookmarkInfo1, fbookmarkIcon1, sbookmarkIcon1);
	articleData += getListViewData(2, data, linkableUrl2, bookmarkInfo2, fbookmarkIcon2, sbookmarkIcon2);
	articleData += getListViewData(3, data, linkableUrl3, bookmarkInfo3, fbookmarkIcon3, sbookmarkIcon3);
	articleData += getListViewData(4, data, linkableUrl4, bookmarkInfo4, fbookmarkIcon4, sbookmarkIcon4);
	articleData += getListViewData(5, data, linkableUrl5, bookmarkInfo5, fbookmarkIcon5, sbookmarkIcon5);
	articleData += getListViewData(6, data, linkableUrl6, bookmarkInfo6, fbookmarkIcon6, sbookmarkIcon6);
	articleData += getListViewData(7, data, linkableUrl7, bookmarkInfo7, fbookmarkIcon7, sbookmarkIcon7);
	articleData += getListViewData(8, data, linkableUrl8, bookmarkInfo8, fbookmarkIcon8, sbookmarkIcon8);

	articleData += '<div class="latest-news__articles">';
	articleData += '<section class="article-preview article-preview--small preview2 gridViewCont">';
	articleData += data.articles[0].listableImage ? '<img class="topic-featured-article__image2 hidden-lg" src="' + data.articles[0].listableImage + '">' : '';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[0].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[0].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[0].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[0].bookmarkText + '" data-label-bookmarked="' + data.articles[0].bookmarkedText + '">' + bookmarkInfo0 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[0].listableDate ? '<li><time class="article-metadata__date">' + data.articles[0].listableDate + '</time></li>' : '';
	articleData += data.articles[0].linkableText ? '<li><h6>' + data.articles[0].linkableText + '</h6></li>' : '';
	articleData += data.articles[0].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type" style="width: 0px; height: 0px;"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#' + data.articles[0].listableType + '"></use></svg><img src="' + data.articles[0].listableType + '" width="25" /></span></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += data.articles[0].listableImage ? '<img class="topic-featured-article__image2 hidden-xs" src="' + data.articles[0].listableImage + '">' : '';
	articleData += '<div class="article-preview__inner-wrapper">';
	articleData += data.articles[0].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[0].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[0].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[0].listableTitle + '</a></h1>' : '';
	articleData += data.articles[0].listableAuthorByLine ? '<span class="article-preview__byline">' + data.articles[0].listableAuthorByLine + '</span>' : '';
	articleData += '<div class="article-summary">' + data.articles[0].listableSummary ? data.articles[0].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[0].listableTopics) {
		for (var i = 0; i < data.articles[0].listableTopics.length; i++) {
			var getLink0 = data.articles[0].listableTopics[i].linkableUrl ? data.articles[0].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink0 + '">' + data.articles[0].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';

	articleData += '<section class="article-preview article-preview--small mobview artheight gridViewCont">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[1].id + '" data-analytics="{"bookmark": "' + bookmarkInfo1 + '", "bookmark_title": "' + data.articles[1].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[1].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[1].bookmarkText + '" data-label-bookmarked="' + data.articles[1].bookmarkedText + '">' + bookmarkInfo1 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon1 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon1 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[1].listableDate ? '<li><time class="article-metadata__date">' + data.articles[1].listableDate + '</time></li>' : '';
	articleData += data.articles[1].linkableText ? '<li><h6>' + data.articles[1].linkableText + '</h6></li>' : '';
	articleData += data.articles[1].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type" style="width: 0px; height: 0px;"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#' + data.articles[1].listableType + '"></use></svg><img src="' + data.articles[1].listableType + '" width="25" /></span></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper">';
	articleData += data.articles[1].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl1 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[1].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[1].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[1].listableTitle + '</a></h1>' : '';
	articleData += data.articles[1].listableAuthorByLine ? '<span class="article-preview__byline">' + data.articles[1].listableAuthorByLine + '</span>' : '';
	articleData += '<div class="article-summary">' + data.articles[1].listableSummary ? data.articles[1].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[1].listableTopics) {
		for (var i = 0; i < data.articles[1].listableTopics.length; i++) {
			var getLink1 = data.articles[1].listableTopics[i].linkableUrl ? data.articles[1].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink1 + '">' + data.articles[1].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';

	articleData += '<section class="article-preview article-preview--small artheight topics gridViewCont">';
	articleData += data.articles[2].linkableText ? '<h6>&nbsp;</h6>' : '';

	articleData += data.articles[2].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl2 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[2].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[2].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[2].listableTitle + '</a></h1>' : '';

	articleData += data.articles[3].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl3 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[3].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[3].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[3].listableTitle + '</a></h1>' : '';

	articleData += data.articles[4].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl4 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[4].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[4].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[4].listableTitle + '</a></h1>' : '';
	articleData += '</section>';
	articleData += '</div>';

	articleData += '<div class="latest-news__articles">';
	articleData += '<section class="article-preview article-small-preview mobview gridViewCont">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[5].id + '" data-analytics="{"bookmark": "' + bookmarkInfo5 + '", "bookmark_title": "' + data.articles[5].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[5].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[5].bookmarkText + '" data-label-bookmarked="' + data.articles[5].bookmarkedText + '">' + bookmarkInfo5 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon5 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon5 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[5].listableDate ? '<li><time class="article-metadata__date">' + data.articles[5].listableDate + '</time></li>' : '';
	articleData += data.articles[5].linkableText ? '<li><h6>' + data.articles[5].linkableText + '</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += data.articles[5].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl5 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[5].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[5].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[5].listableTitle + '</a></h1>' : '';
	articleData += data.articles[1].listableAuthorByLine ? '<span class="article-preview__byline">' + data.articles[1].listableAuthorByLine + '</span>' : '';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[5].listableTopics) {
		for (var i = 0; i < data.articles[5].listableTopics.length; i++) {
			var getLink5 = data.articles[5].listableTopics[i].linkableUrl ? data.articles[5].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink5 + '">' + data.articles[5].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';

	articleData += '<section class="article-preview article-preview--small artheight mobview mtop gridViewCont">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[6].id + '" data-analytics="{"bookmark": "' + bookmarkInfo6 + '", "bookmark_title": "' + data.articles[6].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[6].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[6].bookmarkText + '" data-label-bookmarked="' + data.articles[6].bookmarkedText + '">' + bookmarkInfo6 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon6 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon6 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[6].listableDate ? '<li><time class="article-metadata__date">' + data.articles[6].listableDate + '</time></li>' : '';
	articleData += data.articles[6].linkableText ? '<li><h6>' + data.articles[6].linkableText + '</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper">';
	articleData += data.articles[6].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl6 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[6].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[6].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[6].listableTitle + '</a></h1>' : '';
	articleData += data.articles[6].listableAuthorByLine ? '<span class="article-preview__byline">' + data.articles[6].listableAuthorByLine + '</span>' : '';
	articleData += '<div class="article-summary">' + data.articles[6].listableSummary ? data.articles[6].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[6].listableTopics) {
		for (var i = 0; i < data.articles[6].listableTopics.length; i++) {
			var getLink6 = data.articles[6].listableTopics[i].linkableUrl ? data.articles[6].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink6 + '">' + data.articles[6].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';

	articleData += '<section class="article-preview article-small-preview sm-article sm-articles mtop gridViewCont">';
	articleData += '<section class="sm-article mobview gridViewCont">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[7].id + '" data-analytics="{"bookmark": "' + bookmarkInfo7 + '", "bookmark_title": "' + data.articles[7].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[7].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[7].bookmarkText + '" data-label-bookmarked="' + data.articles[7].bookmarkedText + '">' + bookmarkInfo7 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon7 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon7 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[7].listableDate ? '<li><time class="article-metadata__date">' + data.articles[7].listableDate + '</time></li>' : '';
	articleData += data.articles[7].linkableText ? '<li><h6>' + data.articles[7].linkableText + '</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += data.articles[7].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl7 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[7].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[7].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[7].listableTitle + '</a></h1>' : '';
	articleData += data.articles[1].listableAuthorByLine ? '<span class="article-preview__byline">' + data.articles[1].listableAuthorByLine + '</span>' : '';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[7].listableTopics) {
		for (var i = 0; i < data.articles[7].listableTopics.length; i++) {
			var getLink7 = data.articles[7].listableTopics[i].linkableUrl ? data.articles[7].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink7 + '">' + data.articles[7].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';

	articleData += '<section class="sm-article mobview gridViewCont">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[8].id + '" data-analytics="{"bookmark": "' + bookmarkInfo8 + '", "bookmark_title": "' + data.articles[8].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[8].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[8].bookmarkText + '" data-label-bookmarked="' + data.articles[8].bookmarkedText + '">' + bookmarkInfo8 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon8 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon8 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[8].listableDate ? '<li><time class="article-metadata__date">' + data.articles[8].listableDate + '</time></li>' : '';
	articleData += data.articles[8].linkableText ? '<li><h6>' + data.articles[8].linkableText + '</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += data.articles[8].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl8 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[8].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[8].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[8].listableTitle + '</a></h1>' : '';
	articleData += data.articles[1].listableAuthorByLine ? '<span class="article-preview__byline">' + data.articles[1].listableAuthorByLine + '</span>' : '';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[8].listableTopics) {
		for (var i = 0; i < data.articles[8].listableTopics.length; i++) {
			var getLink8 = data.articles[8].listableTopics[i].linkableUrl ? data.articles[8].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink8 + '">' + data.articles[8].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';

	articleData += '</section>';
	articleData += '</div>';

	articleData += '<input type="hidden" class="getPaginationNum" data-pageSize="' + data.loadMore.pageSize + '" data-pageNo="' + data.loadMore.pageNo + '" data-loadurl="' + data.loadMore.loadMoreLinkUrl + '" data-taxonomyIds="' + data.loadMore.taxonomyIds + '" />';

	return articleData;
}

function getMyviewCookie(cookie) {
	var getAllcookies = cookie.split(';');
	for (var i = 0; i < getAllcookies.length; i++) {
		if ($.trim(getAllcookies[i]).indexOf('myViewCookieName=') == 0) {
			var myViewCookie = getAllcookies[i],
			    selectedCookie = myViewCookie.split('=')[1];
			if (selectedCookie == 'listView') {
				setTimeout(function () {
					$('.view-mode .icon-list-view').trigger('click');
				}, 5);
			} else {
				setTimeout(function () {
					$('.view-mode .icon-tile-view').trigger('click');
				}, 5);
			}
			break;
		}
	}
}
$(function () {
	if ($('.personalisationhome') && $('.personalisationhome').length) {
		getMyviewCookie(document.cookie);
	}
	$('.view-mode').on('click', '.icon-tile-view', function (e) {
		e.preventDefault();
		$('.view-mode li').removeClass('selected');
		$(this).parents('li').addClass('selected');
		if ($('.personalisationhome') && $('.personalisationhome').length) {
			$('.personalisationhome').removeClass('listView').addClass('gridView');
			document.cookie = "myViewCookieName=gridView;";
		}
	});
	$('.view-mode').on('click', '.icon-list-view', function (e) {
		e.preventDefault();
		$('.view-mode li').removeClass('selected');
		$(this).parents('li').addClass('selected');
		if ($('.personalisationhome') && $('.personalisationhome').length) {
			$('.personalisationhome').removeClass('gridView').addClass('listView');
			document.cookie = "myViewCookieName=listView;";
		}
	});
	var getLayoutInfo = $('#getLayoutInfo').val(),
	    layout1 = true,
	    loadLayoutData = '',
	    getLiIdx,
	    getArticleIdx;
	if (typeof loadPreferanceId !== "undefined") {
		var loadDynData = loadPreferanceId["Sections"].length < loadPreferanceId.DefaultSectionLoadCount ? loadPreferanceId["Sections"].length : loadPreferanceId.DefaultSectionLoadCount,
		    getArticalIdx = 0,
		    postedId = window.location.href.split('#')[1];

		if (postedId != '' && postedId != undefined) {
			for (var i = 0; i < loadPreferanceId["Sections"].length; i++) {
				if (loadPreferanceId["Sections"][i]["ChannelId"] == postedId) {
					getArticalIdx = i + 1;
					break;
				}
			}
			loadDynData = getArticalIdx;
		}
		getLiIdx = loadDynData;
		getArticleIdx = loadDynData;
		for (var i = 0; i < loadDynData; i++) {
			var setId = loadPreferanceId["Sections"];
			if (setId.length) {
				(function (idx) {
					if (idx < loadDynData) {
						$.ajax({
							url: '/api/articlesearch',
							data: JSON.stringify({ 'TaxonomyIds': setId[idx]["TaxonomyIds"], 'ChannelId': setId[idx]["ChannelId"], 'PageNo': 1, 'PageSize': 9 }),
							dataType: 'json',
							contentType: "application/json",
							type: 'POST',
							cache: false,
							async: false,
							beforeSend: function beforeSend() {
								$('.spinnerIcon').removeClass('hidespin');
							},
							success: function success(data) {
								if (data.articles && typeof data.articles === "object" && data.articles.length >= 9) {
									if (layout1) {
										layout1 = false;
										loadLayoutData = loadLayoutOneData(data, idx);
										$('.spinnerIcon').addClass('hidespin');
										$('.personalisationPan').append(loadLayoutData);
										window.findTooltips();
									} else {
										layout1 = true;
										loadLayoutData = loadLayoutTwoData(data, idx);
										$('.spinnerIcon').addClass('hidespin');
										$('.personalisationPan').append(loadLayoutData);
										window.findTooltips();
									}
								}
							},
							error: function error(xhr, errorType, _error) {
								console.log('err ' + _error);
							}
						});
					}
				})(i);
			}
		}
	}
	$('.personalisationPan').on('click', '.loadmore', function () {
		var $this = $(this),
		    eachstoryMpan = $this.closest('.eachstoryMpan'),
		    eachstory = eachstoryMpan.find('.eachstory'),
		    eachstoryId = eachstory.attr('id'),
		    layoutCls = eachstory.attr('class'),
		    channelId = $this.closest('.eachstoryMpan').prev('.latestSubject').attr('id'),
		    loadLayoutData;

		var layout = layoutCls.indexOf('layout1') !== -1 ? 'layout1' : 'layout2';
		var setId = loadPreferanceId["Sections"],
		    sendtaxonomyIdsArr = $this.closest('.eachstoryMpan').find('.getPaginationNum').attr('data-taxonomyIds').split(',');

		$.ajax({
			url: $this.closest('.eachstoryMpan').find('.getPaginationNum').attr('data-loadurl'),
			dataType: 'json',
			type: 'POST',
			data: JSON.stringify({ 'TaxonomyIds': sendtaxonomyIdsArr, 'ChannelId': channelId, 'PageNo': $this.closest('.eachstoryMpan').find('.getPaginationNum').attr('data-pageNo'), 'PageSize': $this.closest('.eachstoryMpan').find('.getPaginationNum').attr('data-pageSize') }),
			contentType: "application/json",
			success: function success(data) {
				if (data.articles && typeof data.articles === "object" && data.articles.length >= 9) {
					$this.closest('.eachstoryMpan').find('.getPaginationNum').attr({ 'data-taxonomyIds': data.loadMore.taxonomyIds, 'data-loadurl': data.loadMore.loadMoreLinkUrl, 'data-pageNo': data.loadMore.pageNo, 'data-pageSize': data.loadMore.pageSize });
					if (layout == 'layout1') {
						loadLayoutData = createLayoutInner1(data);
						$(eachstory).append(loadLayoutData);
						window.findTooltips();
						if (data.loadMore && !data.loadMore.displayLoadMore) {
							$this.closest('.eachstoryMpan').find('.loadmore').css('display', 'none');
						}
					} else {
						loadLayoutData = createLayoutInner2(data);
						$(eachstory).append(loadLayoutData);
						window.findTooltips();
						if (data.loadMore && !data.loadMore.displayLoadMore) {
							$this.closest('.eachstoryMpan').find('.loadmore').css('display', 'none');
						}
					}
				}
			},
			error: function error(xhr, errorType, _error2) {
				console.log('err ' + _error2);
			}
		});
	});

	var eachstoryLength = typeof loadPreferanceId !== 'undefined' && loadPreferanceId.DefaultSectionLoadCount ? loadPreferanceId.DefaultSectionLoadCount : 0;
	$(window).scroll(function () {
		var eachstoryMpan = $('.personalisationPan .eachstoryMpan'),
		    eachstoryMpanLast = eachstoryMpan.last(),
		    layoutCls = eachstoryMpan.find('.eachstory').attr('class'),
		    contentHei = $('.personalisationPan').height(),
		    loadsection,
		    getChannelId,
		    texonomyId;

		if ($(window).scrollTop() > contentHei - 400) {
			var getscrollData;

			if (typeof loadPreferanceId !== "undefined") {
				if (getArticleIdx < loadPreferanceId["Sections"].length) {
					getLiIdx = getArticleIdx;
					loadsection = getArticleIdx;
					texonomyId = loadPreferanceId["Sections"][loadsection]["TaxonomyIds"];
					getChannelId = loadPreferanceId["Sections"][loadsection]["ChannelId"];
					getArticleIdx++;
				} else {
					return;
				}
			} else {
				return;
			}

			$.ajax({
				url: '/api/articlesearch',
				data: JSON.stringify({ 'TaxonomyIds': texonomyId, 'ChannelId': getChannelId, 'PageNo': 1, 'PageSize': 9 }),
				type: 'POST',
				contentType: "application/json",
				cache: false,
				async: false,
				dataType: 'json',
				beforeSend: function beforeSend() {
					$('.spinnerIcon').removeClass('hidespin');
				},
				success: function success(data) {
					if (data.articles && typeof data.articles === "object" && data.articles.length >= 9) {
						if ($('.eachstoryMpan', '.personalisationPan').length % 2 == 0) {
							getscrollData = loadLayoutOneData(data, loadsection);
							$('.spinnerIcon').addClass('hidespin');
							$('.personalisationPan').append(getscrollData);
							window.findTooltips();
						} else {
							getscrollData = loadLayoutTwoData(data, loadsection);
							$('.spinnerIcon').addClass('hidespin');
							$('.personalisationPan').append(getscrollData);
							window.findTooltips();
						}
					}
				},
				error: function error(xhr, errorType, _error3) {
					console.log('xhr ' + xhr + ' errorType ' + errorType + ' error ' + _error3);
				}
			});
		}
	});

	$('.main-menu__hoverable a.myviewLink').click(function (e) {
		if ($('#hdnMyViewPage') && $('#hdnMyViewPage').val() == "true") {
			e.preventDefault();
			var $this = $(this),
			    name = $this.attr('name'),
			    getPos = $('#' + name).position(),
			    latestSubject = $('#' + name).closest('.eachstoryMpan').prev('.latestSubject'),
			    subjectHei = latestSubject.height(),
			    allstoriesLen = $('.personalisationPan .eachstoryMpan').length,
			    liIdx = $this.closest('li').index();
			setTimeout(function () {
				if ($('.js-menu-toggle-button, .js-full-menu-toggle').hasClass('is-active')) {
					$('.js-menu-toggle-button, .js-full-menu-toggle').removeClass('is-active');
				}
			}, 5);

			if (typeof loadPreferanceId !== 'undefined' && $('#' + name) && $('#' + name).length) {
				$(window).scrollTop(getPos.top - subjectHei * 3);
			} else {
				if (typeof loadPreferanceId !== "undefined") {
					getLiIdx = getArticleIdx;
					for (var i = getLiIdx; i <= liIdx; i++) {
						var setId = loadPreferanceId["Sections"];
						getArticleIdx++;
						(function (idx) {
							$.ajax({
								url: '/api/articlesearch',
								dataType: 'json',
								contentType: "application/json",
								data: JSON.stringify({ 'TaxonomyIds': loadPreferanceId["Sections"][idx]["TaxonomyIds"], 'ChannelId': loadPreferanceId["Sections"][idx]["ChannelId"], 'PageNo': 1, 'PageSize': 9 }),
								type: 'POST',
								cache: false,
								async: false,
								beforeSend: function beforeSend() {
									$('.spinnerIcon').removeClass('hidespin');
								},
								success: function success(data) {
									if (data.articles && typeof data.articles === "object" && data.articles.length >= 9) {
										if ($('.eachstoryMpan', '.personalisationPan').length % 2 == 0) {
											loadLayoutData = loadLayoutOneData(data, idx);
											$('.personalisationPan').append(loadLayoutData);
											window.findTooltips();
										} else {
											loadLayoutData = loadLayoutTwoData(data, idx);
											$('.personalisationPan').append(loadLayoutData);
											window.findTooltips();
										}
									}
								},
								error: function error(xhr, errorType, _error4) {
									console.log('err ' + _error4);
								},
								complete: function complete(xhr, status) {
									if (status == "success" && $('#' + name).length) {
										setTimeout(function () {
											var getlatestPos = $('#' + name).position();
											if (getlatestPos) {
												$('.spinnerIcon').addClass('hidespin');
												$(window).scrollTop(getlatestPos.top - 120);
											}
										}, 5);
									}
								}
							});
						})(i);
					}
				}
			}
		} else {
			if ($('#validatePreference').val() != 1) {
				e.preventDefault();
				var $this = $(this),
				    href = $this.attr('href'),
				    id = $this.attr('name');
				window.location.href = href + '#' + id;
			}
		}
	});

	var latestSubject = $('.latestSubject');
	if (window.matchMedia("(min-width: 768px)").matches) {
		for (var i = 0; i < latestSubject.length; i++) {
			var getFullwidth = $(latestSubject[i]).width(),
			    frEditviewWid = $(latestSubject[i]).find('.frEditview').width(),
			    setEditViewWidth = Math.ceil(frEditviewWid / getFullwidth * 100),
			    setLatestSubWid = 100 - setEditViewWidth;
			$(latestSubject[i]).find('.frEditview').css('width', setEditViewWidth + '%');
			$(latestSubject[i]).find('.fllatestSub').css('width', setLatestSubWid - 2 + '%');
		}
	}
});

},{}],36:[function(require,module,exports){
/* global analyticsEvent, analytics_data, angular */
'use strict';

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

var _controllersFormController = require('../controllers/form-controller');

var _controllersFormController2 = _interopRequireDefault(_controllersFormController);

var _jscookie = require('../jscookie');

var _jscookie2 = _interopRequireDefault(_jscookie);

var _controllersAnalyticsController = require('../controllers/analytics-controller');

/* * *
SAVE SEARCH
This component handles saving searches from the Search page, as well as setting alerts
for topics from Home/Topic pages. Dispite the naming differences, the back-end functionality
is the same - topic alerts are actually just saved searches for the topic,
plus an email alert for new articles.
* * */

function getParameterByName(name, url) {
	if (!url) {
		url = window.location.href;
	}
	name = name.replace(/[\[\]]/g, "\\$&");
	var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
	    results = regex.exec(url);
	if (!results) return null;
	if (!results[2]) return '';
	return decodeURIComponent(results[2].replace(/\+/g, " "));
}

$(document).ready(function () {

	// When the Save Search pop-out is toggled, need to update some form fields
	// with the most recent data. Used to use Angular for this, but for site-wide
	// reusability we need to do it in Zepto.
	$('.js-save-search').on('click', function (e) {
		$('.js-save-search-url').val(window.location.pathname + window.location.hash);
		$('.js-save-search-title').val($('#js-search-field').val());
	});

	// Populates topic alert data when a user is logging in and saving simultaneously
	$('.js-update-topic-alert').on('click', function (e) {
		$('.js-save-search-url').val($(this).data('topic-alert-url'));
		// Search/Topic title exists as <input> and <span>, needs two techniques to properly
		// update the values.
		$('.js-save-search-title').val($(this).data('topic-alert-title')).html($(this).data('topic-alert-title'));
	});

	$('.js-set-topic-alert').on('click', function (e) {

		var isSettingAlert = !$(this).data('has-topic-alert');
		var topicLabel = $(this).find('.js-set-topic-label');

		$('.js-save-search-url').val($(this).data('topic-alert-url'));
		$('.js-save-search-title').val($(this).data('topic-alert-title'));

		if (isSettingAlert) {
			$('.form-save-search').find('button[type=submit]').click();
			topicLabel.html(topicLabel.data('label-is-set'));
			$(this).data('has-topic-alert', 'true');
			$(this).find('.js-topic-icon-unset').removeClass('is-active');
			$(this).find('.js-topic-icon-set').addClass('is-active');
		} else {
			window.lightboxController.showLightbox($(this));
		}
	});

	var savedSearch = getParameterByName("ss");
	if (savedSearch != null && savedSearch == "true") {
		$('.js-saved-search-success-alert').addClass('is-active').on('animationend', function (e) {
			$(e.target).removeClass('is-active');
		}).addClass('a-fade-alert');
	}

	var removeTopicAlert = new _controllersFormController2['default']({
		observe: '.form-remove-topic-alert',
		successCallback: function successCallback(form, context, event) {
			$(form).find('.js-set-topic-label').html($(form).find('.js-set-topic-label').data('label-not-set'));
			$(form).find('.js-set-topic-alert').data('has-topic-alert', null);
			$(form).find('.js-topic-icon-unset').addClass('is-active');
			$(form).find('.js-topic-icon-set').removeClass('is-active');

			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, $(form).data('analytics')));
		}
	});

	var saveSearchController = new _controllersFormController2['default']({
		observe: '.form-save-search',
		successCallback: function successCallback(form, context, event) {

			// If there's a stashed search, remove it.
			_jscookie2['default'].remove('saveStashedSearch');

			window.controlPopOuts.closePopOut($(form).closest('.pop-out'));
			$('.js-saved-search-success-alert').addClass('is-active').on('animationend', function (e) {
				$(e.target).removeClass('is-active');
			}).addClass('a-fade-alert');

			window.lightboxController.closeLightboxModal();

			if (typeof angular !== 'undefined') {
				angular.element($('.js-saved-search-controller')[0]).controller().searchIsSaved();
			}

			var event_data = {};

			if ($(form).data('is-search') === true) {
				event_data.event_name = "toolbar_use";
				event_data.toolbar_tool = "save_search";
			} else {
				event_data.event_name = "set_alert";
				event_data.alert_topic = $(form).find('.js-save-search-title').val();
			}

			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, event_data));
		},
		beforeRequest: function beforeRequest(form) {
			if (!$(form).find('.js-save-search-title').val().trim()) {
				$('.js-form-error-EmptyTitle').show();
			}
		}
	});

	var saveSearchLoginController = new _controllersFormController2['default']({
		observe: '.form-save-search-login',
		successCallback: function successCallback(form, context, event) {
			_jscookie2['default'].set('saveStashedSearch', {
				'Title': $('.js-save-search-title').val(),
				'Url': $('.js-save-search-url').val(),
				'AlertEnabled': $('#AlertEnabled').prop('checked')
			});

			$.ajax({
				type: "POST",
				url: "/api/SavedSearches",
				data: {
					url: $('.js-save-search-url').val(),
					title: $('.js-save-search-title').val(),
					alertEnabled: $('#AlertEnabled').prop('checked')
				}
			});

			var loginAnalytics = {
				event_name: 'login',
				login_state: 'successful',
				userName: '"' + $(form).find('input[name=username]').val() + '"'
			};
			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, loginAnalytics));

			var ssParam = getParameterByName("ss");
			var searchVal = window.location.search;
			if (ssParam == null) {
				searchVal = searchVal.length < 1 ? "?ss=true" : searchVal + "&ss=true";
			}

			if (ssParam == window.location.search) window.location.reload(true);else window.location = window.location.pathname + searchVal + window.location.hash;
		}
	});

	var toggleSavedSearchAlertController = new _controllersFormController2['default']({
		observe: '.form-toggle-saved-search-alert',
		successCallback: function successCallback(form, context, e) {
			var alertToggle = $(form).find('.js-saved-search-alert-toggle');
			var val = alertToggle.val();
			var event_data = {
				saved_search_alert_title: $(form).data('analytics-title'),
				saved_search_alert_publication: $(form).data('analytics-publication')
			};

			if (val === "on") {
				event_data.event_name = 'saved_search_alert_off';
				alertToggle.val('off');
			} else {
				event_data.event_name = 'saved_search_alert_on';
				alertToggle.val('on');
			}

			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, event_data));
		}
	});

	$('.js-saved-search-alert-toggle').on('click', function (e) {
		$(e.target.form).find('button[type=submit]').click();
	});

	// On page load, check for any stashed searches that need to be saved
	var saveStashedSearch = _jscookie2['default'].getJSON('saveStashedSearch');

	if (saveStashedSearch) {
		// Set `Save Search` values from stashed search data
		$('.js-save-search-title').val(saveStashedSearch['Title']);
		$('.js-save-search-url').val(saveStashedSearch['Url']);
		$('#AlertEnabled').prop('checked', saveStashedSearch['AlertEnabled']);

		// Save the stashed search if Search (Angular) page
		if (typeof angular !== 'undefined') {
			$('.form-save-search').find('button[type=submit]').click();
		} else {
			$('.js-set-topic-alert').each(function (index, item) {
				if ($(item).data('topic-alert-url') === saveStashedSearch['Url']) {
					$(item).click();
					// If there's a stashed search, remove it.
					_jscookie2['default'].remove('saveStashedSearch');
				}
			});
		}
	}

	var removeSavedSearch = new _controllersFormController2['default']({
		observe: '.form-remove-saved-search',
		successCallback: function successCallback(form, context, evt) {
			$(evt.target).closest('tr').remove();

			window.controlPopOuts.closePopOut($(form).closest('.pop-out'));
			$('.js-saved-search-success-alert').addClass('is-active').on('animationend', function (e) {
				console.log("save search component:6");
				$(e.target).removeClass('is-active');
			}).addClass('a-fade-alert');

			window.lightboxController.closeLightboxModal();

			var event_data = {
				event_name: 'saved_search_alert_removal',
				saved_search_alert_title: $(form).data('analytics-title'),
				saved_search_alert_publication: $(form).data('analytics-publication')
			};

			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, event_data));
		}
	});
});

},{"../controllers/analytics-controller":40,"../controllers/form-controller":42,"../jscookie":50}],37:[function(require,module,exports){
'use strict';

if ($('.scrollbar') && $('.scrollbar').length) {
	$('.scrollbar').each(function () {
		var container = $(this).parents('.rolling-stream').find('.scrollbar-container')[0],
		    content = $(this).parents('.rolling-stream').find('.content')[0],
		    scroll = $(this).parents('.rolling-stream').find('.scrollbar')[0];

		scroll.style.height = container.clientHeight * content.clientHeight / content.scrollHeight + "px";
		scroll.style.top = container.clientHeight * content.scrollTop / content.scrollHeight + "px";

		content.addEventListener('scroll', function (e) {
			scroll.style.height = container.clientHeight * content.clientHeight / content.scrollHeight + "px";
			scroll.style.top = container.clientHeight * content.scrollTop / content.scrollHeight + "px";
		});
		// var event = new Event('scroll');

		window.addEventListener('resize', function (e) {
			scroll.style.height = container.clientHeight * content.clientHeight / content.scrollHeight + "px";
			scroll.style.top = container.clientHeight * content.scrollTop / content.scrollHeight + "px";
		});
		// content.dispatchEvent(event);

		scroll.addEventListener('mousedown', function (start) {
			start.preventDefault();
			var y = scroll.offsetTop;
			var onMove = function onMove(end) {
				var delta = end.pageY - start.pageY;
				scroll.style.top = Math.min(container.clientHeight - scroll.clientHeight, Math.max(0, y + delta)) + 'px';
				content.scrollTop = content.scrollHeight * scroll.offsetTop / container.clientHeight;
			};
			document.addEventListener('mousemove', onMove);
			document.addEventListener('mouseup', function () {
				document.removeEventListener('mousemove', onMove);
			});
		});
	});
}

//Horizontal Scroll Bar
if ($('#scrollbar-horizantal')) {
	var container = $('#scrollbar-horizantal')[0],
	    content = $('.wrap-merge')[0],
	    scroll = $('#scrollbar')[0];

	//scroll.style.width = ( $('.merge-acquistion').width() - $('.wrap-merge').width() ) + 'px';

	$(window).on('resize', function () {
		scroll.style.width = $('.merge-acquistion').width() - $('.wrap-merge').width() + 'px';
	});
	$('.wrap-merge').on('scroll', function () {
		scroll.style.left = $(this).find('.table').offset().left * -1 + 14 + 'px';
	});
}

},{}],38:[function(require,module,exports){
'use strict';

function PrintCompanyGraph(chartData, divId, graphType) {
	var chart1 = new AmCharts.AmSerialChart();

	chart1.dataProvider = chartData;
	chart1.categoryField = "year";

	chart1.marginTop = 20;
	chart1.marginBottom = 25;
	chart1.marginLeft = 55;
	//chart1.marginLeft = 45;
	chart1.marginRight = 25;
	chart1.height = '100%';
	chart1.width = '100%';
	//chart1.fontSize = 8;

	chart1.startDuration = 0;
	chart1.startRadius = "100%";

	var graph1 = new AmCharts.AmGraph();
	graph1.valueField = "value";

	graph1.lineColor = "#" + chartData[0].colour;
	graph1.fillColor = "#" + chartData[0].colour;
	graph1.balloonText = "[[category]]: [[value]]";
	graph1.type = graphType;
	graph1.lineAlpha = 0;
	graph1.fillAlphas = 0.8;
	graph1.startDuration = 0;
	graph1.startRadius = "100%";
	//graph1.lineThickness = 2;
	chart1.addGraph(graph1);

	var catAxis = chart1.categoryAxis;

	catAxis.gridPosition = "start";
	catAxis.autoGridCount = true;

	//catAxis.autoGridCount = false;
	//catAxis.gridCount = 5;
	//catAxis.labelFrequency = 1;

	// chart1.addTitle("Millions $", 8);

	chart1.write(divId);
}

$(function () {
	if ($('#graph-carousel') && $('#graph-carousel').length) {
		if (window.matchMedia("(min-width: 670px)").matches) {
			$('.graphsPan').removeClass('owl-carousel');
			$('.loadChart').removeClass('item');
		}

		$('#graph-carousel').find('.owl-carousel').owlCarousel({
			loop: false,
			autoPlay: false,
			nav: true,
			navContainer: '#customNav',
			dotsContainer: '#customDots',
			slideBy: 4,
			responsive: {
				0: {
					items: 4
				},
				678: {
					items: 1
				},
				320: {
					items: 1
				},
				480: {
					items: 1
				},
				1000: {
					items: 8
				}
			}
		});
	}
	setTimeout(function () {
		$("a[title='JavaScript charts']").hide();
	}, 3000);
});

},{}],39:[function(require,module,exports){
'use strict';

var INFORMA = window.INFORMA || {};
INFORMA.videoMini = (function (window, $, namespace) {
    'use strict';
    //variables
    var _videoMiniImgWrapper = $('.video-mini-container .video-img'),
        _videoMiniPlayBtnWrapper = $('.video-mini-container .play-icon'),
        _videoMiniPlayerModal = $('#videoMiniModal'),
        _videoMiniModalClose = $('.video-mini-close'),
        video,

    // methods
    init,
        _playVideoMiniImgWrapper,
        _playVideoMiniBtnWrapper,
        _videoMiniShowPlayIcon;

    _playVideoMiniBtnWrapper = function () {
        _videoMiniPlayBtnWrapper.click(function () {
            var videoImg = $(this).parent().find('img');
            if (videoImg.attr('data-videotype') == "youtube") {
                video = '<iframe width="100%" src="' + videoImg.attr('data-videourl') + '" frameborder="0" allowfullscreen></iframe>';
            } else if (videoImg.attr('data-videotype') == "vimeo") {
                video = '<iframe width="100%" src="' + videoImg.attr('data-videourl') + '" frameborder="0" allowfullscreen></iframe>';
            } else if (videoImg.attr('data-videotype') == "wistia") {
                video = '<iframe width="100%" src="' + videoImg.attr('data-videourl') + '" frameborder="0" allowfullscreen></iframe>';
            }
            _videoMiniPlayerModal.find('.modal-body .video-mini-player').html(video);
            _videoMiniPlayerModal.modal('show');
            // $(this).parents('.video-mini-container').find('.play-icon').hide();
            //  imgContainer.find(_videoMiniPlayBtnWrapper).hide();
        });
    };

    _playVideoMiniImgWrapper = function () {
        _videoMiniImgWrapper.click(function () {
            if ($(this).attr('data-videotype') == "youtube") {
                video = '<iframe width="100%" height="' + $(this).attr('height') + '" src="' + $(this).attr('data-videourl') + '" frameborder="0" allowfullscreen></iframe>';
            } else if ($(this).attr('data-videotype') == "vimeo") {
                video = '<iframe width="100%" height="' + $(this).attr('height') + '" src="' + $(this).attr('data-videourl') + '" frameborder="0" allowfullscreen></iframe>';
            } else if ($(this).attr('data-videotype') == "wistia") {
                video = '<iframe width="100%" height="' + $(this).attr('height') + '" src="' + $(this).attr('data-videourl') + '" frameborder="0" allowfullscreen></iframe>';
            }
            _videoMiniPlayerModal.find('.modal-body .video-mini-player').html(video);
            _videoMiniPlayerModal.modal('show');
            // _videoMiniPlayBtnWrapper.hide();
        });
    };
    _videoMiniShowPlayIcon = function () {
        _videoMiniModalClose.click(function () {
            _videoMiniPlayBtnWrapper.show();
            $(this).parents('.video-mini-modal').find('iframe').remove();
        });
    };

    init = function () {
        _playVideoMiniImgWrapper();
        _playVideoMiniBtnWrapper();
        _videoMiniShowPlayIcon();
    };

    return {
        init: init
    };
})(undefined, Zepto, 'INFORMA');
Zepto(INFORMA.videoMini.init());

},{}],40:[function(require,module,exports){
// * * *
//  ANALYTICS CONTROLLER
//  For ease-of-use, better DRY, better prevention of JS errors when ads are blocked
// * * *

'use strict';

Object.defineProperty(exports, '__esModule', {
    value: true
});
function analyticsEvent(dataObj) {
    if (typeof utag !== 'undefined') {
        utag.link(dataObj);
    }
};

exports.analyticsEvent = analyticsEvent;

},{}],41:[function(require,module,exports){
/* globals analytics_data */
'use strict';

Object.defineProperty(exports, '__esModule', {
    value: true
});

var _analyticsController = require('./analytics-controller');

function bookmarkController() {

    // * * *
    //  Article bookmarking logic goes here
    // * * *
    this.toggle = function (e) {

        var bookmark = {
            elm: $(e)
        };

        // ID of the article we're bookmarking or un-bookmarking
        bookmark.id = bookmark.elm.closest('.js-bookmark-article').data('bookmark-id');

        //passing SalesforceID;
        //bookmark.salesforceId = bookmark.elm.closest('.js-bookmark-article').data('salesforce-id');
        bookmark.salesforceId = bookmark.elm.closest('.js-bookmark-article').attr('data-salesforce-id') !== undefined ? bookmark.elm.closest('.js-bookmark-article').attr('data-salesforce-id') : '';

        // Stash the bookmark label data now, swap label text later
        bookmark.label = {
            elm: bookmark.elm.find('.js-bookmark-label')
        };
        bookmark.label.bookmark = bookmark.label.elm.data('label-bookmark');
        bookmark.label.bookmarked = bookmark.label.elm.data('label-bookmarked');

        // Are we bookmarking an article, or un-bookmarking?
        // Used later to know what API endpoint to hit, and what DOM changes are required
        bookmark.isBookmarking = bookmark.elm.data('is-bookmarked') ? false : true;

        var apiEndpoint = bookmark.isBookmarking ? '/Account/api/SavedDocumentApi/SaveItem/' : '/Account/api/SavedDocumentApi/RemoveItem/';

        if (bookmark.id) {
            $.ajax({
                url: apiEndpoint,
                type: 'POST',
                data: {
                    DocumentID: bookmark.id,
                    SalesforceID: bookmark.salesforceId
                },
                context: this,
                success: function success(response) {
                    if (response.success) {

                        if (bookmark.isBookmarking) {
                            (0, _analyticsController.analyticsEvent)($.extend(analytics_data, $(bookmark.elm).data('analytics')));
                            bookmark.elm.closest('.js-bookmark-article').attr('data-salesforce-id', response.salesforceid);
                        } else {
                            bookmark.elm.closest('.js-bookmark-article').removeAttr('data-salesforce-id');
                        }

                        this.flipIcon(bookmark);
                        return true;
                    } else {}
                },
                error: function error(response) {
                    return false;
                }
            });
        }
    };

    this.flipIcon = function (bookmark) {

        if (!bookmark.elm.hasClass('js-angular-bookmark')) {
            $(bookmark.elm).find('.article-bookmark').removeClass('is-visible');
        }

        if (bookmark.isBookmarking) {
            if (!bookmark.elm.hasClass('js-angular-bookmark')) {
                $(bookmark.elm).find('.article-bookmark__bookmarked').addClass('is-visible');
                bookmark.elm.data('is-bookmarked', true);
            }
            bookmark.label.elm.html(bookmark.label.bookmarked);
        } else {
            if (!bookmark.elm.hasClass('js-angular-bookmark')) {
                $(bookmark.elm).find('.article-bookmark').not('.article-bookmark__bookmarked').addClass('is-visible');
                bookmark.elm.data('is-bookmarked', null);
            }
            bookmark.label.elm.html(bookmark.label.bookmark);
        }
    };
}

exports['default'] = bookmarkController;
module.exports = exports['default'];

},{"./analytics-controller":40}],42:[function(require,module,exports){
/*

opts.observe — Form element(s) to observe
opts.beforeRequest — Function to execute before making Ajax request
opts.successCallback — If Ajax request is successful, callback
opts.failureCallback — If Ajax request fails / returns false, callback

*/

'use strict';

Object.defineProperty(exports, '__esModule', {
	value: true
});
function formController(opts) {

	var showSuccessMessage = function showSuccessMessage(form) {
		$(form).find('.js-form-success').show();
	};

	var showError = function showError(form, error) {
		if ($(form).find(error)) {
			$(form).find(error).show();
		}
	};

	var hideErrors = function hideErrors(form) {
		$(form).find('.js-form-error').hide();
	};

	(function init() {

		var form = opts.observe;

		if (!form) return false;

		var formSubmit = $(form).find('button[type=submit]');

		$(formSubmit).on('click', function (event) {

			// Some forms will require user confirmation before action is taken
			// Default to true (confirmed), set to false later if confirmation is
			// required and user cancels action
			var actionConfirmed = true;

			var currentForm;
			if (event.target.form) {
				currentForm = event.target.form;
			} else {
				currentForm = $(event.target).closest('form');
			}

			if ($(currentForm).data('force-confirm')) {
				actionConfirmed = window.confirm($(currentForm).data('force-confirm'));
			}

			if (actionConfirmed) {

				event.preventDefault(); // Prevent form submitting

				hideErrors(currentForm); // Reset any visible errors

				if (opts.beforeRequest) {
					opts.beforeRequest(currentForm);
				}

				// Prevent user from re-submitting form, unless explicitly allowed
				if (!$(currentForm).data('prevent-disabling')) {
					$(formSubmit).attr('disabled', 'disabled');
				}

				var inputData = {};
				var IsValid = true; //Skip Validation if the form is not Update Contact Informatin Form
				if ($(currentForm).hasClass('form-update-account-contact')) {
					IsValid = ValidateContactInforForm();
				}
				if (IsValid) {
					$(currentForm).find('input, select, textarea').each(function () {

						var value = '';
						var field = $(this);

						if (field.data('checkbox-type') === 'boolean') {
							value = this.checked;

							if (field.data('checkbox-boolean-type') === 'reverse') {
								value = !value;
							}
						} else if (field.data('checkbox-type') === 'value') {
							value = this.checked ? field.val() : undefined;
						} else {
							value = field.val();
						}

						if (value !== undefined) {
							if (inputData[field.attr('name')] === undefined) {
								inputData[field.attr('name')] = value;
							} else if ($.isArray(inputData[field.attr('name')])) {
								inputData[field.attr('name')].push(value);
							} else {
								inputData[field.attr('name')] = [inputData[field.attr('name')]];
								inputData[field.attr('name')].push(value);
							}
						}
					});

					//// 25/10/2016 Commented captcha code to fix the js console error. Raju/Sonia will provide fix of this.
					// add recaptcha if it exists in the form
					var captchaResponse = grecaptcha == null ? undefined : grecaptcha.getResponse();
					if (captchaResponse !== undefined) inputData['RecaptchaResponse'] = captchaResponse;

					if (!$(currentForm).data('on-submit')) {
						console.warn('No submit link for form');
					}
					try {
						for (var index in inputData) {
							if (inputData[index] == "- Select One -") {
								inputData[index] = "";
							}
						}
					} catch (ex) {
						console.log(ex);
					}

					$.ajax({
						url: $(currentForm).data('on-submit'),
						type: $(currentForm).data('submit-type') || 'POST',
						data: inputData,
						context: this,
						success: function success(response) {
							if (response.success) {

								showSuccessMessage(currentForm);

								// Passes the form response through with the "context"
								// successCallback is ripe for refactoring, improving parameters
								this.response = response;

								// //Specific to Sign In Form - Vertical Login
								// if($(currentForm).hasClass('js-sign-in-submit') || $(currentForm).hasClass("form-registration")) {
								// 	$('#hiddenforms_login form').each(function() {
								// 		$(this).find('input[type="text"]').val(inputData["username"]);
								// 		$(this)[0].submit();
								// 	});
								// }
								// //Specific to Sign In Form - Vertical Login
								if (opts.successCallback) {
									opts.successCallback(currentForm, this, event);
								}

								if ($(form).data('on-success')) {
									window.location.href = $(currentForm).data('on-success');
								}
								if (response.redirectRequired !== undefined && response.redirectRequired) {
									window.location.href = response.redirectUrl;
								}
							} else {
								if (response.reasons && response.reasons.length > 0) {
									for (var reason in response.reasons) {
										showError(form, '.js-form-error-' + response.reasons[reason]);
									}
								} else {
									showError(currentForm, '.js-form-error-general');
								}

								if (opts.failureCallback) {
									opts.failureCallback(currentForm, response);
								}
							}
						},
						error: function error(response) {

							showError(currentForm, '.js-form-error-general');

							if (opts.failureCallback) {
								opts.failureCallback(currentForm, response);
							}
						},
						complete: function complete() {
							setTimeout(function () {
								$(formSubmit).removeAttr('disabled');
							}, 250);

							// reset captcha if available
							grecaptcha.reset();
						}

					});
				} // if actionConfirmed
			}
			return false;
		});
	})();
}
function ValidateContactInforForm() {
	var errorHtml = $('#errorMessage').html();
	var errors = 0;
	var result = false;
	var scrollTo = '';
	$('.required').each(function () {
		if ($(this).val() == '' || $(this).text().indexOf("- Select One -") >= 0) {
			$(this).parent().append(errorHtml);
			errors++;
			if (errors == 1) {
				scrollTo = $(this);
			}
		} else {
			$(this).parent().find('.js-form-error').remove();
		}
	});
	if (errors > 0) {
		window.scrollTo(0, scrollTo.offset().top - 30);
		result = false;
	} else {
		result = true;
	}
	return result;
}

exports['default'] = formController;
module.exports = exports['default'];

},{}],43:[function(require,module,exports){
/* global angular */
'use strict';

Object.defineProperty(exports, '__esModule', {
    value: true
});
function lightboxModalController() {

    this.closeLightboxModal = function () {
        $('body').removeClass('lightboxed');
        $('.lightbox-modal__backdrop').remove();
        $('.lightbox-modal').hide();
    };

    var closeLightboxModal = this.closeLightboxModal;

    this.showLightbox = function (lightbox) {
        // Freeze the page and add the dark overlay
        $('body').addClass('lightboxed').append('<div class="lightbox-modal__backdrop"></div>');

        // Find the specific modal for this trigger, and the associated form
        var targetModal = $(lightbox).data('lightbox-modal');
        var successForm = $(lightbox).closest('.' + $(lightbox).data('lightbox-modal-success-target'));

        // Show the modal, add an on-click listener for the "success" button
        $('.' + targetModal).show().find('.js-lightbox-modal-submit')
        // .one, not .on, to prevent stacking event listners
        .one('click', function (e) {
            successForm.find('button[type=submit]').click();
            closeLightboxModal();
        });

        return false;
    };

    var showLightbox = this.showLightbox;

    this.buildLightboxes = function () {
        $('.js-lightbox-modal-trigger').on('click', function (e) {

            if (e.target !== this) {
                this.click();
                return;
            }

            showLightbox(e.target);

            // Don't submit any forms for real.
            return false;
        });
    };

    // When the Dismiss button is clicked...
    $('.js-close-lightbox-modal').on('click', function (e) {
        closeLightboxModal();
    });

    this.buildLightboxes();

    this.clearLightboxes = function () {
        $('.js-lightbox-modal-trigger').off();
    };
}

exports['default'] = lightboxModalController;
module.exports = exports['default'];

},{}],44:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, '__esModule', {
	value: true
});
function popOutController(triggerElm) {
	var _this = this;

	// Toggle pop-out when trigger is clicked
	if (triggerElm) {
		$(triggerElm).off();
		$(triggerElm).on('click', function (event) {
			event.preventDefault();
			_this.togglePopOut($(event.target));
		});
	}

	// Reposition pop-out when browser window resizes
	$(window).on('resize', function (event) {
		_this.updatePopOut();
	});

	// Simulate CSS `rem` (16px)
	// TODO: change this from `rem` to tab padding value for clarity
	var rem = 16;

	// Keep track of the active pop-out element
	// This is an object instead of a var because there might be more "global"
	// state attributes to track in the future.
	var state = {
		activeElm: null,
		customized: {}
	};

	// PUBLIC
	// Get the current pop-out element, if there is one.
	// Lets other JS know what's up with the pop-out.
	this.getPopOutElement = function () {
		return state.activeElm;
	};

	// PUBLIC
	// Closes the pop-out.
	this.closePopOut = function (elm) {
		// Reset all z-indexes so new pop-outs are stacked on top properly
		$('.pop-out').removeClass('is-active').css("z-index", "");
		$('.js-pop-out-trigger').css("z-index", "");
	};

	// PUBLIC
	// Toggles the pop-out
	this.togglePopOut = function (e) {
		// Check if clicked element is the toggle itself
		// Otherwise, climb up DOM tree and find it
		var poParent = e.hasClass('js-pop-out-trigger') ? e : e.closest('.js-pop-out-trigger');

		/*  This is a little hacky, but if a user is trying to bookmark an article
  	but needs to sign in first, we need to capture and pass the article
  	ID as a URL param after a successful sign in attempt. That allows
  	us to automatically bookmark the article on page refresh. */

		if (poParent.data('pop-out-type') === 'sign-in' && poParent.data('bookmark-id')) {
			$('.sign-in__submit').data('pass-article-id', poParent.data('bookmark-id'));
		} else {
			poParent.data('bookmark-id', null);
		}

		// Close all pop-outs
		this.closePopOut();

		if (poParent[0] !== state.activeElm) {
			// Update the controller state and open it
			state.activeElm = poParent[0];
			updatePosition();
		} else {
			state.activeElm = null;
		}
	};

	// PUBLIC
	// Stores pop-out customization details for reference when rendering
	this.customize = function (obj) {
		state.customized[obj.id] = obj;
	};

	// PRIVATE
	// Update the visibility and position of the pop-out box and tab.
	var updatePosition = function updatePosition() {

		var trgr = { // The pop-out trigger
			e: $(state.activeElm)
		};
		// Get trigger height, width, offsetTop, offsetWidth
		trgr.offset = trgr.e.offset();

		trgr.hasStyles = state.customized[trgr.e.data('pop-out-id')];

		// Determine which pop-out template to use
		// TODO: Make this user-configurable
		// Let users assign a name to a template class
		var popOut;
		switch (trgr.e.data('pop-out-type')) {
			// SIGN IN
			// (Global sign-in, bookmarking when not signed in)
			case 'sign-in':
				popOut = $('.js-pop-out__sign-in');
				break;
			// Main Sign In button on top right
			case 'sign-in-header':
				popOut = $('.js-pop-out__sign-in-header');
				break;
			// EMAIL ARTICLE
			case 'email-article':
				popOut = $('.js-pop-out__email-article');
				break;
			// EMAIL ARTICLE
			case 'email-search':
				popOut = $('.js-pop-out__email-search');
				break;
			// EMAIL AUTHOR
			case 'email-author':
				popOut = $('.js-pop-out__email-author');
				break;
			// EMAIL COMPANy
			case 'email-company':
				popOut = $('.js-pop-out__email-company');
				break;
			// EMAIL DEAL
			case 'email-deal':
				popOut = $('.js-pop-out__email-deal');
				break;
			// GLOBAL HEADER REGISTRATION
			case 'register':
				popOut = $('.js-pop-out__register');
				break;
			// GLOBAL HEADER REGISTRATION
			case 'myViewregister':
				popOut = $('.js-pop-out__myViewregister');
				break;
			// SEARCH PAGE - SAVE SEARCH
			case 'save-search':
				popOut = $('.js-pop-out__save-search');
				break;
			default:
				console.warn('Attempting to fire unidentified pop-out.');
				return;
		}

		// Make pop-out visible so we can query for its width
		popOut.addClass('is-active');

		// Check if browser is less than or equal to `small` CSS breakpoint

		var isNarrow = $(window).width() <= 480;
		var isTablet = $(window).width() <= 800;

		// Set separate vertical/horizontal padding on mobile vs. desktop
		var vPad = isNarrow ? 10 : rem;
		var hPad = isNarrow ? 14 : rem;

		// Store output values after calculations, etc.
		var res = {
			offset: {
				box: {},
				tab: {}
			},
			css: {
				box: {},
				tab: {}
			}
		};

		// Box offset top is offsetTop of trigger, plus trigger height,
		// plus padding, minus 1px for border positioning
		res.offset.box.top = Math.floor(trgr.offset.top + trgr.offset.height + (vPad - 1));

		// Check if pop-out will bleed off-screen, causing horizontal scroll bar
		// If it will, force right-align to keep it on-screen
		if (popOut.width() + trgr.offset.left > $(window).width()) {
			trgr.e.data('pop-out-align', 'right');
		}

		// Check for pop-out alignment
		if (trgr.e.data('pop-out-align') === 'right' && !isNarrow) {
			// Pop-out box is flush right with trigger element
			// To flush right, first add trigger offset plus trigger width
			// This positions left edge of pop-out with right edge of trigger
			// Then subtract pop-out width and padding to align both right edges
			// (Flush-left automatically if narrow window)
			res.offset.box.left = isNarrow ? 0 : Math.floor(trgr.offset.left + trgr.offset.width - popOut.offset().width + (hPad - 1));
			// Tab left margin can be ignored, right margin 0 does what we need
			res.offset.tab.left = 'auto';
		} else {
			// Pop-out box is centered with trigger element
			// Box offset left is determined by subtracting the trigger width
			// from the pop-out width, dividing by 2 to find the halfway point,
			// then subtracting that from the trigger left offset.
			// (Flush-left automatically if narrow window)
			res.offset.box.left = isNarrow ? 0 : Math.floor(trgr.offset.left - (popOut.offset().width - trgr.offset.width) / 2);
			// Pop-out tab is aligned with trigger left edge, adjusted for padding
			// Tab width is set to trigger width below, so this centers the tab
			res.offset.tab.left = isNarrow ? Math.floor(trgr.offset.left - hPad) : 0;
		}

		// Blow up z-index to appear above other triggers
		trgr.e.css('z-index', '9999');

		// Box z-index set to 2 lower than trigger element
		// Box should render below trigger, under tab, above everything else
		res.css.box.zIndex = trgr.e.css('z-index') - 2;

		// Tab height equals trigger height plus padding (1rem top and bottom)

		// Check for custom tab styles
		var tS = trgr.hasStyles ? trgr.hasStyles.tabStyles : undefined;

		// If there are custom styles, and browser is desktop-width...
		if (tS && !isNarrow && !isTablet) {

			res.css.tab.height = tS.deskHeight || trgr.offset.height + vPad * 2 + "px";

			tS.deskHeight ? res.offset.box.top += tS.deskHeight - trgr.offset.height - vPad * 2 : null;

			res.css.tab.top = tS.deskHeight ? '-' + (tS.deskHeight - 1) + 'px' : '-' + (trgr.offset.height + vPad * 2 - 1) + 'px';

			// If there are custom styles, and browser is tablet-width...
		} else if (tS && !isNarrow && isTablet) {

				res.css.tab.height = tS.tabletHeight || trgr.offset.height + vPad * 2 + "px";

				tS.tabletHeight ? res.offset.box.top += tS.tabletHeight - trgr.offset.height - vPad * 2 : null;

				res.css.tab.top = tS.tabletHeight ? '-' + (tS.tabletHeight - 1) + 'px' : '-' + (trgr.offset.height + vPad * 2 - 1) + 'px';

				// If there are custom styles, and browser is phone-width...
			} else if (tS && isNarrow) {

					res.css.tab.height = tS.phoneHeight || trgr.offset.height + vPad * 2 + "px";

					tS.phoneHeight ? res.offset.box.top += tS.phoneHeight - trgr.offset.height - vPad * 2 : null;

					res.css.tab.top = tS.phoneHeight ? '-' + (tS.phoneHeight - 1) + 'px' : '-' + (trgr.offset.height + vPad * 2 - 1) + 'px';

					// Default padding/positioning
				} else {

						res.css.tab.height = trgr.offset.height + vPad * 2 + "px";

						// Move the tab upwards, equal to the trigger height plus padding
						// minus 1px to account for border and visually overlapping box
						res.css.tab.top = '-' + (trgr.offset.height + vPad * 2 - 1) + "px";
					}

		// Tab width equals trigger width plus padding (1rem left and right)
		res.css.tab.width = trgr.offset.width + hPad * 2 + "px";

		// Tab z-index is 1 less than trigger; above box, below trigger
		res.css.tab.zIndex = trgr.e.css('z-index') - 1;

		// `transform` to quickly position box, relative to top left corner
		res.css.box.transform = 'translate3d(' + res.offset.box.left + 'px, ' + res.offset.box.top + 'px, 0)';

		// Apply that giant blob of CSS
		popOut.css({
			zIndex: res.css.box.zIndex,
			transform: res.css.box.transform
		}).find('.pop-out__tab').css({ // find this pop-out's child tab
			height: res.css.tab.height,
			width: res.css.tab.width,
			left: res.offset.tab.left,
			right: 0, // This is always 0
			top: res.css.tab.top,
			zIndex: res.css.tab.zIndex
		});
		// Ugly hack for Safari 8, booo
		popOut.css('-webkit-transform', res.css.box.transform);
	};

	// If there is an active pop-out, update its position
	// Mostly useful for when the browser window resizes
	this.updatePopOut = function () {
		if (state.activeElm) {
			updatePosition();
		}
	};
}

exports['default'] = popOutController;
module.exports = exports['default'];

},{}],45:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, '__esModule', {
	value: true
});

var _analyticsController = require('./analytics-controller');

function loginController(requestVerificationToken) {
	this.addRegisterUserControl = function (triggerElement, successCallback, failureCallback) {
		var _this = this;

		if (triggerElement) {
			$(triggerElement).on('click', function (event) {
				_this.hideErrors(triggerElement);
				$(triggerElement).attr('disabled', 'disabled');

				var inputData = {};
				var url = $(triggerElement).data('register-user-url');

				$(triggerElement).parents('.js-register-user-container').find('input').each(function () {
					var value = '';

					if ($(this).data('checkbox-type') === 'boolean') {
						value = this.checked;

						if ($(this).data('checkbox-boolean-type') === 'reverse') {
							value = !value;
						}
					} else {
						value = $(this).val();
					}

					inputData[$(this).attr('name')] = value;
				});

				$.ajax({
					url: url,
					type: 'POST',
					data: inputData,
					context: _this,
					success: function success(response) {
						if (response.success) {

							var registerAnalytics = {
								event_name: 'register-step-1',
								registration_state: 'successful',
								userName: '"' + inputData.username + '"'
							};

							(0, _analyticsController.analyticsEvent)($.extend(analytics_data, registerAnalytics));

							if (successCallback) {
								successCallback(triggerElement);
							}

							var nextStepUrl = $(triggerElement).data('next-step-url');

							if (nextStepUrl) {
								window.location.href = nextStepUrl;
							}

							this.showSuccessMessage(triggerElement);
						} else {
							$(triggerElement).removeAttr('disabled');

							var specificErrorDisplayed = false;

							if (response.reasons && response.reasons.length > 0) {
								for (var reason in response.reasons) {
									this.showError(triggerElement, '.js-register-user-error-' + response.reasons[reason]);
								}

								specificErrorDisplayed = true;
							}

							if (!specificErrorDisplayed) {
								this.showError(triggerElement, '.js-register-user-error-general');
							}

							var registerAnalytics = {
								event_name: "registration failure",
								registration_errors: response.reasons
							};

							(0, _analyticsController.analyticsEvent)($.extend(analytics_data, registerAnalytics));

							if (failureCallback) {
								failureCallback(triggerElement);
							}
						}
					},
					error: function error(response) {
						$(triggerElement).removeAttr('disabled');

						this.showError(triggerElement, '.js-register-user-error-general');

						if (failureCallback) {
							failureCallback(triggerElement);
						}
					}
				});
			});
		}
	};

	this.showSuccessMessage = function (triggerElement) {
		$(triggerElement).parents('.js-register-user-container').find('.js-register-user-success').show();
	};

	this.showError = function (triggerElement, error) {
		$(triggerElement).parents('.js-register-user-container').find('.js-register-user-error-container').show();
		$(triggerElement).parents('.js-register-user-container').find(error).show();
	};

	this.hideErrors = function (triggerElement) {
		$(triggerElement).parents('.js-register-user-container').find('.js-register-user-error-container').hide();
		$(triggerElement).parents('.js-register-user-container').find('.js-register-user-error').hide();
	};
};

exports['default'] = loginController;
module.exports = exports['default'];

},{"./analytics-controller":40}],46:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, '__esModule', {
	value: true
});

var _analyticsController = require('./analytics-controller');

function loginController(requestVerificationToken) {
	this.addRequestControl = function (triggerElement, successCallback, failureCallback) {
		var _this = this;

		if (triggerElement) {
			$(triggerElement).on('click', function (event) {
				_this.hideErrors(triggerElement);
				$(triggerElement).attr('disabled', 'disabled');

				var inputData = {};
				var url = $(triggerElement).data('reset-url');

				$(triggerElement).parents('.js-reset-password-container').find('input').each(function () {
					inputData[$(this).attr('name')] = $(this).val();
				});

				$.ajax({
					url: url,
					type: 'POST',
					data: inputData,
					context: _this,
					success: function success(response) {
						if (response.success) {
							this.showSuccessMessage(triggerElement);

							if (successCallback) {
								successCallback(triggerElement);
							}
						} else {
							$(triggerElement).removeAttr('disabled');
							var resetPasswordAnalytics = {
								event_name: "password reset unsuccessful"
							};

							var specificErrorDisplayed = false;

							if ($.inArray('EmailRequirement', response.reasons) !== -1) {
								this.showError(triggerElement, '.js-reset-password-error-email');
								specificErrorDisplayed = true;
							}

							if (!specificErrorDisplayed) {
								this.showError(triggerElement, '.js-reset-password-error-general');
							}

							(0, _analyticsController.analyticsEvent)($.extend(analytics_data, resetPasswordAnalytics));

							if (failureCallback) {
								failureCallback(triggerElement);
							}
						}
					},
					error: function error(response) {
						$(triggerElement).removeAttr('disabled');

						this.showError(triggerElement, '.js-reset-password-error-general');

						if (failureCallback) {
							failureCallback(triggerElement);
						}
					}
				});
			});
		}
	};

	this.addChangeControl = function (triggerElement, successCallback, failureCallback) {
		var _this2 = this;

		if (triggerElement) {
			$(triggerElement).on('click', function (event) {
				_this2.hideErrors(triggerElement);
				$(triggerElement).attr('disabled', 'disabled');

				var inputData = {};
				var url = $(triggerElement).data('reset-url');

				$(triggerElement).parents('.js-reset-password-container').find('input').each(function () {
					inputData[$(this).attr('name')] = $(this).val();
				});

				$.ajax({
					url: url,
					type: 'POST',
					data: inputData,
					context: _this2,
					success: function success(response) {
						if (response.success) {
							this.showSuccessMessage(triggerElement);

							if (successCallback) {
								successCallback(triggerElement);
							}
						} else {
							$(triggerElement).removeAttr('disabled');

							var specificErrorDisplayed = false;

							if ($.inArray('PasswordMismatch', response.reasons) !== -1) {
								this.showError(triggerElement, '.js-reset-password-error-mismatch');
								specificErrorDisplayed = true;
							}
							if ($.inArray('PasswordRequirements', response.reasons) !== -1) {
								this.showError(triggerElement, '.js-reset-password-error-requirements');
								specificErrorDisplayed = true;
							}

							if (!specificErrorDisplayed || $.inArray('MissingToken', response.reasons) !== -1) {
								this.showError(triggerElement, '.js-reset-password-error-general');
							}

							if (failureCallback) {
								failureCallback(triggerElement);
							}
						}
					},
					error: function error(response) {
						$(triggerElement).removeAttr('disabled');

						this.showError(triggerElement, '.js-reset-password-error-general');

						if (failureCallback) {
							failureCallback(triggerElement);
						}
					}
				});
			});
		}
	};

	this.addRetryControl = function (triggerElement, successCallback, failureCallback) {
		var _this3 = this;

		if (triggerElement) {
			$(triggerElement).on('click', function (event) {
				_this3.hideErrors(triggerElement);
				$(triggerElement).attr('disabled', 'disabled');

				var inputData = {};
				var url = $(triggerElement).data('retry-url');

				$(triggerElement).parents('.js-reset-password-container').find('input').each(function () {
					inputData[$(this).attr('name')] = $(this).val();
				});

				$.ajax({
					url: url,
					type: 'POST',
					data: inputData,
					context: _this3,
					success: function success(response) {
						if (response.success) {
							this.showSuccessMessage(triggerElement);

							if (successCallback) {
								successCallback(triggerElement);
							}
						} else {
							$(triggerElement).removeAttr('disabled');

							this.showError(triggerElement, '.js-reset-password-error-general');

							if (failureCallback) {
								failureCallback(triggerElement);
							}
						}
					},
					error: function error(response) {
						$(triggerElement).removeAttr('disabled');

						this.showError(triggerElement, '.js-reset-password-error-general');

						if (failureCallback) {
							failureCallback(triggerElement);
						}
					}
				});
			});
		}
	};

	this.showSuccessMessage = function (triggerElement) {
		$(triggerElement).parents('.js-reset-password-container').find('.js-reset-password-success').show();
		var resetPasswordAnalytics = {
			event_name: "password reset success"
		};

		(0, _analyticsController.analyticsEvent)($.extend(analytics_data, resetPasswordAnalytics));
	};

	this.showError = function (triggerElement, error) {
		$(triggerElement).parents('.js-reset-password-container').find('.js-reset-password-error-container').show();
		$(triggerElement).parents('.js-reset-password-container').find(error).show();
		var resetPasswordAnalytics = {
			event_name: "password reset unsuccessful"
		};

		(0, _analyticsController.analyticsEvent)($.extend(analytics_data, resetPasswordAnalytics));
	};

	this.hideErrors = function (triggerElement) {
		$(triggerElement).parents('.js-reset-password-container').find('.js-reset-password-error-container').hide();
		$(triggerElement).parents('.js-reset-password-container').find('.js-reset-password-error').hide();
	};
};

exports['default'] = loginController;
module.exports = exports['default'];

},{"./analytics-controller":40}],47:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, '__esModule', {
	value: true
});
function sortableTableController() {

	/*
 Based on SortTable version 2
 7th April 2007
 Stuart Langridge, http://www.kryogenix.org/code/browser/sorttable/
 Licenced as X11: http://www.kryogenix.org/code/browser/licence.html
 */

	var isSortedTable = false;
	var tfo, mtch, sortfn, hasInputs;

	var sorttable = {

		init: function initing() {
			// quit if this function has already been called
			if (isSortedTable) return;
			// flag this function so we don't do the same thing twice
			isSortedTable = true;

			$('.js-sortable-table').each(function (indx, item) {
				sorttable.makeSortable(item);
			});
		},

		sortColumn: function sortColumn(table, col) {

			// build an array to sort. This is a Schwartzian transform thing,
			// i.e., we "decorate" each row with the actual sort key,
			// sort based on the sort keys, and then put the rows back in order
			// which is a lot faster because you only do getInnerText once per row

			var row_array = [];
			var headrow = table.tHead.rows[0].cells;
			var rows = [].slice.call(table.tBodies[0].rows);
			var guesstype = sorttable.guessType(table, col);

			for (var j = 0; j < rows.length; j++) {
				row_array[row_array.length] = [$(rows[j].cells[col]), rows[j]];
			}

			if ($(headrow[col]).data('sortable-type')) {
				row_array.sort(sorttable[$(headrow[col]).data('sortable-type')]);
			} else {
				row_array.sort(guesstype);
			}

			var tb = table.tBodies[0];
			for (var j = 0; j < row_array.length; j++) {
				tb.appendChild(row_array[j][1]);
			}

			row_array = undefined;
		},

		makeSortable: function makeSortable(table) {

			// Sorttable v1 put rows with a class of "sortbottom" at the bottom (as
			// "total" rows, for example). This is B&R, since what you're supposed
			// to do is put them in a tfoot. So, if there are sortbottom rows,
			// for backwards compatibility, move them to tfoot (creating it if needed).
			var sortbottomrows = [];
			for (var i = 0; i < table.rows.length; i++) {
				if ($(table.rows[i]).hasClass('.sortbottom')) {
					sortbottomrows[sortbottomrows.length] = table.rows[i];
				}
			}

			if (sortbottomrows) {
				if (table.tFoot == null) {
					// table doesn't have a tfoot. Create one.
					tfo = document.createElement('tfoot');
					table.appendChild(tfo);
				}
				for (var j = 0; j < sortbottomrows.length; j++) {
					tfo.appendChild(sortbottomrows[j]);
				}
				sortbottomrows = undefined;
			}

			// work through each column and calculate its type
			var headrow = table.tHead.rows[0].cells;
			for (var i = 0; i < headrow.length; i++) {

				// manually override the type with a sorttable_type attribute
				if (!headrow[i].className.match(/\bsorttable_nosort\b/)) {
					// skip this col
					headrow[i].sorttable_sortfunction = sorttable.guessType(table, i);
				}
			};

			$(table).find('.js-sortable-table-sorter').on('click', function (e) {

				// If child element is clicked, redirect the click to the
				// proper element: the parent itself.
				if (e.target !== this) {
					this.click();
					return;
				}

				var colNum = $(e.target).closest('.js-sortable-table-sorter').data('sortable-table-col') - 1;

				if ($(e.target).hasClass('sorttable_sorted')) {
					// This column is sorted top to bottom
					// Re-sort the column to catch any row changes...
					sorttable.sortColumn(table, colNum);
					// ...then reverse the column and update the classes (state).
					sorttable.reverse(table.tBodies[0]);
					$(e.target).removeClass('sorttable_sorted').addClass('sorttable_sorted_reverse');

					return;
				}

				if ($(e.target).hasClass('sorttable_sorted_reverse')) {
					// This column is sorted bottom to top
					// Flip the table back to top-to-bottom (default)...
					sorttable.reverse(table.tBodies[0]);
					// ...then re-sort it to catch any row changes.
					sorttable.sortColumn(table, colNum);
					$(e.target).removeClass('sorttable_sorted_reverse').addClass('sorttable_sorted');

					return;
				}

				// remove sorttable_sorted classes
				var theadrow = e.target.parentNode;
				forEach(theadrow.childNodes, function (cell) {
					if (cell.nodeType == 1) {
						// an element
						$(cell).removeClass('sorttable_sorted_reverse sorttable_sorted');
					}
				});

				if ($('.sorttable_sortfwdind')) {
					$('.sorttable_sortfwdind').remove();
				}

				if ($('.sorttable_sortrevind')) {
					$('.sorttable_sortrevind').remove();
				}

				$(e.target).addClass('sorttable_sorted');

				sorttable.sortColumn(table, colNum);
			});
		},

		guessType: function guessType(table, column) {

			// guess the type of a column based on its first non-blank row
			sortfn = sorttable.sort_alpha;

			for (var i = 0; i < table.tBodies[0].rows.length; i++) {

				var text = $(table.tBodies[0].rows[i].cells[column]).text().trim();
				if (text != '') {
					// If column is numeric or appears to be money, sort numeric
					if (text.match(/^-?[£$¤]?[\d,.]+%?$/)) {
						return sorttable.sort_numeric;
					} else if (Date.parse(text) > 0) {
						// Check for valid date
						// If found, assume column is full of dates, sort by date!
						return sorttable.sort_by_date;
					} else {
						return sorttable.sort_alpha;
					}
				}
			}
			return sortfn;
		},

		reverse: function reverse(tbody) {
			// reverse the rows in a tbody
			var newrows = [];
			for (var i = 0; i < tbody.rows.length; i++) {
				newrows[newrows.length] = tbody.rows[i];
			}
			for (var i = newrows.length - 1; i >= 0; i--) {
				tbody.appendChild(newrows[i]);
			}
			newrows = undefined;
		},

		/* sort functions
  each sort function takes two parameters, a and b
  you are comparing a[0] and b[0] */
		sort_numeric: function sort_numeric(a, b) {
			var aa = parseFloat(a[0].replace(/[^0-9.-]/g, ''));
			if (isNaN(aa)) aa = 0;
			var bb = parseFloat(b[0].replace(/[^0-9.-]/g, ''));
			if (isNaN(bb)) bb = 0;
			return aa - bb;
		},
		sort_alpha: function sort_alpha(a, b) {
			var aClean = a[0].text().trim().toUpperCase();
			var bClean = b[0].text().trim().toUpperCase();
			if (aClean == bClean) return 0;
			if (aClean < bClean) return -1;
			return 1;
		},

		sort_by_date: function sort_by_date(a, b) {
			// http://stackoverflow.com/questions/10123953/sort-javascript-object-array-by-date
			// Turn your strings into dates, and then subtract them
			// to get a value that is either negative, positive, or zero.
			return new Date(b[0].text().trim()) - new Date(a[0].text().trim());
		},

		sort_checkbox: function sort_checkbox(a, b) {
			var aChecked = a[0].find('input[type=checkbox]').prop('checked');
			var bChecked = b[0].find('input[type=checkbox]').prop('checked');
			if (aChecked && !bChecked) return 1;
			if (!aChecked && bChecked) return -1;

			return 0;
		},

		shaker_sort: function shaker_sort(list, comp_func) {
			// A stable sort function to allow multi-level sorting of data
			// see: http://en.wikipedia.org/wiki/Cocktail_sort
			// thanks to Joseph Nahmias
			var b = 0;
			var t = list.length - 1;
			var swap = true;

			while (swap) {
				swap = false;
				for (var i = b; i < t; ++i) {
					if (comp_func(list[i], list[i + 1]) > 0) {
						var q = list[i];list[i] = list[i + 1];list[i + 1] = q;
						swap = true;
					}
				} // for
				t--;

				if (!swap) break;

				for (var i = t; i > b; --i) {
					if (comp_func(list[i], list[i - 1]) < 0) {
						var q = list[i];list[i] = list[i - 1];list[i - 1] = q;
						swap = true;
					}
				} // for
				b++;
			} // while(swap)
		}
	};

	/// HELPER FUNCTIONS

	// Dean's forEach: http://dean.edwards.name/base/forEach.js
	/*
 	forEach, version 1.0
 	Copyright 2006, Dean Edwards
 	License: http://www.opensource.org/licenses/mit-license.php
 */

	// array-like enumeration
	if (!Array.forEach) {
		// mozilla already supports this
		Array.forEach = function (array, block, context) {
			for (var i = 0; i < array.length; i++) {
				block.call(context, array[i], i, array);
			}
		};
	}

	// generic enumeration
	Function.prototype.forEach = function (object, block, context) {
		for (var key in object) {
			if (typeof this.prototype[key] == "undefined") {
				block.call(context, object[key], key, object);
			}
		}
	};

	// character enumeration
	String.forEach = function (string, block, context) {
		Array.forEach(string.split(""), function (chr, index) {
			block.call(context, chr, index, string);
		});
	};

	// globally resolve forEach enumeration
	var forEach = function forEach(object, block, context) {
		if (object) {
			var resolve = Object; // default
			if (object instanceof Function) {
				// functions have a "length" property
				resolve = Function;
			} else if (object.forEach instanceof Function) {
				// the object implements a custom forEach method so use that
				object.forEach(block, context);
				return;
			} else if (typeof object == "string") {
				// the object is a string
				resolve = String;
			} else if (typeof object.length == "number") {
				// the object is array-like
				resolve = Array;
			}
			resolve.forEach(object, block, context);
		}
	};

	sorttable.init();
};

exports['default'] = sortableTableController;
module.exports = exports['default'];

},{}],48:[function(require,module,exports){
/* global tooltipController */

"use strict";

Object.defineProperty(exports, "__esModule", {
    value: true
});
exports["default"] = createPopup;

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { "default": obj }; }

var _calculatePopupOffsetsJs = require("../calculatePopupOffsets.js");

var _calculatePopupOffsetsJs2 = _interopRequireDefault(_calculatePopupOffsetsJs);

/**
 * creates a popup and injects it in to the document.
 * @param  {String} title : title of the popup, optional
 * @param  {String} html : content of the popup as html
 * @param  {Number} top  : top position relative to the document
 * @param  {Number} left : left position relative to the document
 * @param  {Number} offset : a vertical or horizonal distance from the top/left the triangle should point
 *                           depending on if the triangle is top/bottom or left/right
 * @param  {String} container : a selector of the container in which to inject the popup
 * @param  {String} triangle : "top", "right", "bottom", or "left"
 * @param  {Boolean} flipToContain: will flip the popup if it goes outside the parent container
 * @param  {Boolean} closeBtn : if the X should be shown.  If not, the popup with not have pointer events
 *                              this is useful for popups on hover that shouldn't fire mouseenter/leave
 * @param  {Boolean} isHidden: Whether or not the popup is visible.
 * @return {Object} {
 *     {Function} hidePopup   : will set isHidden to true
 *     {Function} removePopup : will remove the popup from the DOM),
 *     {Function} setState : pass an object with any keys from above to update the popup.
 * }
 */

function createPopup(initialState) {

    // defaults, and this object will hold the previous state after setState
    var prevState = {
        title: "",
        html: "",
        top: 0,
        left: 0,
        width: "",
        offset: 0,
        container: "body",
        triangle: "bottom",
        closeBtn: true,
        isHidden: false,
        flipToContain: false
    };

    var state = $.extend({}, prevState, initialState);

    function setState(newState) {

        // copy the old state into prevState
        prevState = $.extend({}, state);

        $.extend(state, newState);

        // console.log(state);

        render();
    }

    // initialize popup
    // always start hidden so it can animate in
    var $popup = $("<div class='popup'>").css({
        "opacity": 0,
        "width": state.width,
        "transform": "scale(0.89)",
        "pointer-events": state.closeBtn ? "auto" : "none"
    });
    var $titleDiv = $("<div>").addClass("popup__title");
    var $triangleDiv = $("<div>").addClass("popup__triangle");
    var $content = $("<div>").addClass("popup__content");

    // attach the close button if we're supposed to
    if (state.closeBtn) {
        var $xDiv = $("<div>").addClass("popup__x-icon").html("<svg class='svg-x'> <use xlink:href='build/img/svg-sprite.svg#x'></use> </svg>").on("click", removePopup);
        $popup.append($xDiv);

        window.addEventListener("click", handleClickAway, true);
        window.addEventListener("resize", handleClickAway, true);
    }

    $popup.append($titleDiv);
    $popup.append($triangleDiv);
    $popup.append($content);

    $(state.container).append($popup);

    // if the user clicked outside of the popup, close it
    function handleClickAway(e) {
        var inPopup = $(e.target).closest(".popup").length;
        if (!inPopup) {
            removePopup();
        }
    }

    function hidePopup() {

        window.removeEventListener("click", handleClickAway, true);
        window.removeEventListener("resize", handleClickAway, true);

        // only re-render if we need to
        if (state.isHidden !== true) {
            // will kick of the transition
            setState({
                isHidden: true
            });
        }
    }

    function removePopup() {

        // first close it
        hidePopup();

        // when the transition finishes, remove the popup from the DOM
        $popup.on("transitionend", function () {
            $popup.remove();
        });
    }

    // render the first time
    render();

    function render() {
        var top = state.top;
        var left = state.left;
        var offset = state.offset;
        var triangle = state.triangle;
        var isHidden = state.isHidden;
        var html = state.html;
        var title = state.title;
        var flipToContain = state.flipToContain;

        // update the content before calculating the offsets
        $content.html(html);
        $titleDiv.html(title);

        var offsets = (0, _calculatePopupOffsetsJs2["default"])({
            popup: $popup.get(0),
            triangleSize: $triangleDiv.height(),
            top: top, left: left, offset: offset, triangle: triangle, flipToContain: flipToContain
        });

        // if the popup was hidden, we want to place it where it needs to be
        // the update will fade it in
        // enter - put it in place before transitioning in
        if (prevState.isHidden && !isHidden) {
            $popup.css({
                "top": offsets.popupTop + "px",
                "left": offsets.popupLeft + "px"
            });
        }

        $popup
        // .stop().animate({
        .css({
            opacity: isHidden ? 0 : 1,
            transform: isHidden ? "scale(0.9)" : "scale(1)",
            top: offsets.popupTop + 'px',
            left: offsets.popupLeft + 'px'
        }, 250).removeClass(function (index, css) {
            return (css.match(/\bis-triangle-\S+/g) || []).join(" ");
        }).addClass("is-triangle-" + offsets.triangleSide).toggleClass("popup--hidden", isHidden);

        // adjust the triangle
        $triangleDiv.css({
            "transform": offsets.triangleSide === "top" || offsets.triangleSide === "bottom" ? "translateX(" + offsets.triangleOffset + "px)" // top/bottom
            : "translateY(" + offsets.triangleOffset + "px)" // left/right
        });

        $popup.toggleClass("no-title", !title);
    }

    // external api
    return {
        setState: setState,
        hidePopup: hidePopup,
        removePopup: removePopup
    };
}

module.exports = exports["default"];

},{"../calculatePopupOffsets.js":2}],49:[function(require,module,exports){
/* global angular, analytics_data */

// THIRD-PARTY / VENDOR
'use strict';

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

var _zeptoMin = require('./zepto.min');

var _zeptoMin2 = _interopRequireDefault(_zeptoMin);

var _svg4everybody = require('./svg4everybody');

var _svg4everybody2 = _interopRequireDefault(_svg4everybody);

var _jscookie = require('./jscookie');

var _jscookie2 = _interopRequireDefault(_jscookie);

var _zeptoDragswap = require('./zepto.dragswap');

var _zeptoDragswap2 = _interopRequireDefault(_zeptoDragswap);

var _DragDropTouch = require('./DragDropTouch');

var _DragDropTouch2 = _interopRequireDefault(_DragDropTouch);

// CAROUSEL
//import highlight from './carousel/highlight.pack';

var _carouselZeptoData = require('./carousel/zepto.data');

var _carouselZeptoData2 = _interopRequireDefault(_carouselZeptoData);

//import app from './carousel/app';

require('./carousel/owl.carousel');

// CONTROLLERS

var _controllersFormController = require('./controllers/form-controller');

var _controllersFormController2 = _interopRequireDefault(_controllersFormController);

var _controllersPopOutController = require('./controllers/pop-out-controller');

var _controllersPopOutController2 = _interopRequireDefault(_controllersPopOutController);

var _controllersBookmarkController = require('./controllers/bookmark-controller');

var _controllersBookmarkController2 = _interopRequireDefault(_controllersBookmarkController);

var _controllersResetPasswordController = require('./controllers/reset-password-controller');

var _controllersResetPasswordController2 = _interopRequireDefault(_controllersResetPasswordController);

var _controllersRegisterController = require('./controllers/register-controller');

var _controllersRegisterController2 = _interopRequireDefault(_controllersRegisterController);

var _controllersSortableTableController = require('./controllers/sortable-table-controller');

var _controllersSortableTableController2 = _interopRequireDefault(_controllersSortableTableController);

var _controllersLightboxModalController = require('./controllers/lightbox-modal-controller');

var _controllersLightboxModalController2 = _interopRequireDefault(_controllersLightboxModalController);

var _controllersAnalyticsController = require('./controllers/analytics-controller');

var _controllersTooltipController = require('./controllers/tooltip-controller');

var _controllersTooltipController2 = _interopRequireDefault(_controllersTooltipController);

// COMPONENTS

require('./components/header-logout');

require('./components/article-sidebar-component');

require('./components/save-search-component');

require('./components/myview-settings');

require('./components/pagination');

require('./components/personalisation');

require('./components/id-comparechart');

require('./components/id-responsive-table');

require('./components/id-financial-responsive-table');

require('./components/id-quarterly-responsive-table');

require('./components/id-comparefinancialresults');

require('./components/latest-casuality');

require('./components/id-merge-acquistion');

require('./components/AMCharts-merges-acquisition');

require('./components/dynamic-content-recomendation');

require('./components/ll-casuality-listing');

require('./components/ll-casuality-detail');

require('./components/ll-market-data');

require('./components/ll-tanker-fixtures');

require('./components/ll-market-data-dryCargo');

require('./components/ll-market-data-dryCargo-icap');

require('./components/ll-market-data-dryCargo-bulkFixture');

require('./components/ll-market-imarex');

require('./components/accordionStockChart');

require('./components/amGraphParam');

require('./components/table_charts');

require('./components/scrollbar.js');

require('./components/ll-ship-coal-export.js');

require('./components/ll-tanker-pure-chem-page.js');

require('./components/ll-marketdata-drycargo-ssyal.js');

require('./components/ll-cockett-bunker.js');

require('./components/ll-ship-vehicle.js');

require('./components/ll-ship-roro.js');

require('./components/ll-ship-container-ship.js');

// OTHER CODE

var _newsletterSignup = require('./newsletter-signup');

var _newsletterSignup2 = _interopRequireDefault(_newsletterSignup);

var _searchPageJs = require('./search-page.js');

var _searchPageJs2 = _interopRequireDefault(_searchPageJs);

var _toggleIcons = require('./toggle-icons');

// Global scope to play nicely with Angular

var _selectivityFull = require('./selectivity-full');

var _selectivityFull2 = _interopRequireDefault(_selectivityFull);

var _componentsVideoMini = require('./components/video-mini');

var _componentsVideoMini2 = _interopRequireDefault(_componentsVideoMini);

var _modal = require('./modal');

var _modal2 = _interopRequireDefault(_modal);

// Make sure proper elm gets the click event
// When a user submits a Forgot Password request, this will display the proper
// success message and hide the form to prevent re-sending.

window.toggleIcons = _toggleIcons.toggleIcons;

/* Polyfill for scripts expecting `jQuery`. Also see: CSS selectors support in zepto.min.js */
window.jQuery = $;
var showForgotPassSuccess = function showForgotPassSuccess() {
    $('.pop-out__sign-in-forgot-password-nested').toggleClass('is-hidden');
    $('.pop-out__sign-in-forgot-password').find('.alert-success').toggleClass('is-active');
};

window.findTooltips = function () {
    $('.js-toggle-tooltip').each(function (index, item) {
        var tooltip;
        $(item).data("ttVisible", false);
        $(item).data("ttTouchTriggered", false);

        $(item).on('mouseenter touchstart', function (e) {
            e.preventDefault();
            e.stopPropagation();
            if (e.type === "touchstart") {
                $(item).data("ttTouchTriggered", true);
            }

            // Actual mouse events thrown can be any number of things...
            if ((e.type === "mouseover" || e.type === "mouseenter") && $(item).data("ttTouchTriggered")) {
                // Do nothing
            } else if ($(item).data("ttVisible") && e.type === "touchstart") {
                    $(item).data("ttVisible", false);
                    $(item).data("ttTouchTriggered", false);
                    tooltip.hidePopup();
                } else {
                    $(item).data("ttVisible", true);
                    var offsets = $(item).offset();
                    tooltip = (0, _controllersTooltipController2['default'])({
                        isHidden: false,
                        html: $(item).data('tooltip-text'),
                        top: offsets.top,
                        left: offsets.left + $(this).width() / 2,
                        triangle: 'bottom'
                    });
                }
        });

        $(item).on('mouseleave', function () {
            $(item).data("ttVisible", false);
            tooltip.hidePopup();
            $('.popup').remove();
        });
    });
};

window.findTooltips();

var renderIframeComponents = function renderIframeComponents() {
    $('.iframe-component').each(function (index, elm) {
        var desktopEmbed = $(elm).find('.iframe-component__desktop');
        var mobileEmbed = $(elm).find('.iframe-component__mobile');

        var isEditMode = $(this).hasClass('is-page-editor');

        var showMobile = $(window).width() <= 480 || isEditMode;
        var showDesktop = !showMobile || isEditMode;

        if (showMobile) {
            mobileEmbed.show();
            if (mobileEmbed.html() == '') mobileEmbed.html(decodeHtml(mobileEmbed.data('embed-link')));
        } else {
            desktopEmbed.hide();
        }

        if (showDesktop) {
            desktopEmbed.show();
            if (desktopEmbed.html() == '') desktopEmbed.html(decodeHtml(desktopEmbed.data('embed-link')));
        } else {
            mobileEmbed.hide();
        }

        var desktopMediaId = $(elm).find('.iframe-component__desktop').data("mediaid");

        var url = window.location.href;
        url.replace("#", "");
        if (url.indexOf("?") < 0) {
            url += "?";
        } else {
            url += "&";
        }

        url += "mobilemedia=true&selectedid=" + desktopMediaId;
        $(elm).find('.iframe-component__mobile a').data('mediaid', url).attr('href', null);
    });
};

var renderTableau = function renderTableau() {

    var desktopEmbed = $('.tableau_component__desktop');
    var mobileEmbed = $('.tableau_component__mobile');

    var mobileHiddenValue = $('#IsMobileDashboardAvailable').val();
    var showMobile = $(window).width() <= 480;
    var showDesktop = !showMobile;

    if (showMobile) {
        if (mobileHiddenValue == "True") {
            mobileEmbed.show();
            desktopEmbed.hide();
        } else {
            desktopEmbed.show();
        }
    }

    if (showDesktop) {
        desktopEmbed.show();
        mobileEmbed.hide();
    }
};

var renderAMchart = function renderAMchart() {
    if ($("#amchartDashboard").hasClass("amchart-dashboard")) {

        var amChartType = $('#ChartType').val().toLowerCase();
        var dataProvider = $('#amChartDataProvider').val().toLowerCase();
        var graphType = $('#GraphType').val().toLowerCase();
        var categoryField = $('#CategoryField').val().toLowerCase();
        var valueField = $('#ValueField').val().toLowerCase();

        AmCharts.makeChart("chartdiv", {
            "type": amChartType,
            "dataProvider": chartData,
            "categoryField": categoryField,
            "graphs": [{
                "valueField": valueField,
                "type": graphType
            }],
            "responsive": {
                "enabled": true
            }
        });
    }
};

var decodeHtml = function decodeHtml(html) {
    var txt = document.createElement("textarea");
    txt.innerHTML = html;
    return txt.value;
};

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

$(document).ready(function () {

    //AM Charts
    if ($('#amchartData') && $('#amchartData').length) {
        var amchartVal = JSON.parse($('#amchartData').val()),
            createNewObj = {},
            chart;

        if (typeof amchartVal.dataProvider !== 'object') {
            for (var prop in amchartVal) {
                if (prop != 'dataProvider') {
                    createNewObj[prop] = amchartVal[prop];
                } else {
                    createNewObj[prop] = chartDataVal;
                }
            }
            chart = AmCharts.makeChart("chartdiv", createNewObj);
        } else {
            chart = AmCharts.makeChart("chartdiv", amchartVal);
        }
    }
    //messaging web users
    window.dismiss = function () {
        $('.dismiss').on('click', function () {
            _jscookie2['default'].set('dismiss_cookie', 'dismiss_cookie_created', '');
            $('.messaging_webUsers').remove();
            $('.messaging_webUsers_white').remove();

            var dismiss_data = {
                event_name: "message_dismissal",
                ga_eventCategory: "Messaging Frame",
                ga_eventAction: "Dismissal",
                ga_eventLabel: "Dismiss",
                page_name: analytics_data["page_name"]
            };

            (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, dismiss_data));
        });
    };
    window.dismiss();

    //Job Listing Pagination
    if ($('#jobTilesCount') && $('#jobTilesCount').length && $('#noofJobsPerPage') && $('#noofJobsPerPage').length) {
        var totalCategories = $("#jobTilesCount").val(),
            categoryLimit = $("#noofJobsPerPage").val();

        window.setPagination({
            totalCategories: parseInt(totalCategories),
            categoryLimit: parseInt(categoryLimit),
            currentPage: 1,
            paginationEle: '.job_list_individual'
        });

        window.loadPaginationNums();
    }

    //Food news Pagination
    if ($('#foodNewsCount') && $('#foodNewsCount').length && $('#noofJobsPerPage') && $('#noofJobsPerPage').length) {
        var totalCategories = $("#foodNewsCount").val(),
            categoryLimit = $("#noofJobsPerPage").val();

        window.setPagination({
            totalCategories: parseInt(totalCategories),
            categoryLimit: parseInt(categoryLimit),
            currentPage: 1
        });

        window.loadPaginationNums();
    }

    window.custom_label = function () {
        $("body").off().on("click", '.label-check', function (e) {
            if ($(this).hasClass("label-check")) {

                var ele = $(this).find('input');
                if (ele.is(':checked')) {
                    ele.prop('checked', false);
                    ele.parent('div').removeClass('wcs-c-on');
                } else {
                    ele.prop('checked', true);
                    ele.parent('div').addClass('wcs-c-on');
                }
            }
        });
    };
    window.custom_label();

    window.personalised_nav = function () {
        //personalise pop up
        var modal = document.getElementById('myModal');

        // Get the button that opens the modal
        var btn = document.getElementById("myBtn");

        // Get the <span> element that closes the modal
        var span = document.getElementsByClassName("personalise_close")[0];

        // When the user clicks the button, open the modal
        $(document).on('click', '#myBtn', function () {
            modal.style.display = "block";
        });

        // When the user clicks on <span> (x), close the modal
        if (span !== undefined) {
            span.onclick = function () {
                modal.style.display = "none";
            };
        }

        // When the user clicks anywhere outside of the modal, close it
        window.onclick = function (event) {
            if (event.target == modal) {
                modal.style.display = "none";
            }
        };
    };
    window.personalised_nav();

    var mediaTable = getParameterByName('mobilemedia');
    if (mediaTable == "true") {
        $("table").each(function () {
            $(this).attr("style", "display:block");
        });
    }

    // Anti Forgery Token
    var requestVerificationToken = $('.main__wrapper').data('request-verification-token');

    var sortTheTables = new _controllersSortableTableController2['default']();

    window.lightboxController = new _controllersLightboxModalController2['default']();

    /* * *
    Traverses the DOM and registers event listeners for any pop-out triggers.
    Bound explicitly to `window` for easier access by Angular.
    * * */
    window.indexPopOuts = function () {

        window.controlPopOuts = new _controllersPopOutController2['default']('.js-pop-out-trigger');

        window.controlPopOuts.customize({
            id: 'header-register',
            tabStyles: {
                deskHeight: 87,
                tabletHeight: 72,
                phoneHeight: '' // Default
            }
        });

        window.controlPopOuts.customize({
            id: 'myView-header-register',
            tabStyles: {
                deskHeight: 87,
                tabletHeight: 72,
                phoneHeight: '' // Default
            }
        });
        window.controlPopOuts.customize({
            id: 'header-register',
            tabStyles: {
                deskHeight: 87,
                tabletHeight: 72,
                phoneHeight: '' // Default
            }
        });

        window.controlPopOuts.customize({
            id: 'header-signin',
            tabStyles: {
                deskHeight: 87,
                tabletHeight: 72,
                phoneHeight: '' // Default
            }
        });
    };

    window.indexPopOuts();

    window.bookmark = new _controllersBookmarkController2['default']();

    /* * *
    Traverses the DOM and registers event listeners for any bookmarkable
    articles. Bound explicitly to `window` for easier access by Angular.
    * * */
    window.indexBookmarks = function () {
        // Toggle bookmark icon
        $(document).off().on('click', '.js-bookmark-article', function bookmarkArticle(e) {

            e.preventDefault();
            window.bookmark.toggle(this);
        });
    };

    window.indexBookmarks();

    //Data tool Landing page

    window.addWidth = function () {
        //landing page
        if ($(".demoText").is(':visible') && !$(".video-demo").is(':hidden')) {
            $('.demoText').addClass('add-width-100');
        }
        if (!$(".demoText").is(':hidden') && $(".video-demo").is(':visible')) {
            $('.video-demo').addClass('add-width-100');
        }
        if ($(".demoText").is(':visible') && $(".video-demo").is(':visible')) {
            $('.demoText').removeClass('add-width-100');
            $('.video-demo').removeClass('add-width-100');
        }
    };
    window.addWidth();

    /* * *
    If a user tries bookmarking an article while logged out, they'll be
    prompted to sign in first. This checks for any articles that have been
    passed along for post-sign-in bookmarking. Bound explicitly to `window`
    for easier access by Angular.
    * * */
    window.autoBookmark = function () {

        var bookmarkTheArticle = function bookmarkTheArticle(article) {
            $('.js-bookmark-article').each(function (indx, item) {
                if ($(item).data('bookmark-id') === article && !$(item).data('is-bookmarked')) {

                    $(item).click();
                } else {
                    // already bookmarked or not a match
                }
            });
        };

        var urlVars = window.location.href.split("?");
        var varsToParse = urlVars[1] ? urlVars[1].split("&") : null;
        if (varsToParse) {
            for (var i = 0; i < varsToParse.length; i++) {
                var pair = varsToParse[i].split("=");
                if (pair[0] === 'immb') {
                    bookmarkTheArticle(pair[1]);
                }
            }
        }
    };

    window.autoBookmark();

    /* * *
    Toggle global header search box
    (toggles at tablet/smartphone sizes, always visible at desktop size)
    * * */
    $('.js-header-search-trigger').on('click', function toggleMenuItems(e) {
        if ($(window).width() <= 800) {
            $('.header-search__wrapper').toggleClass('is-active').focus();
        } else {
            $(e.target).closest('form').submit();
        }
        e.preventDefault();
        return false;
    });

    var newsletterSignup = new _newsletterSignup2['default']();
    newsletterSignup.checkForUserSignedUp();
    newsletterSignup.addControl('.js-newsletter-signup-submit', null, function (triggerElement) {});

    /* * *
    Handle user sign-in attempts.
    * * */
    var userSignIn = new _controllersFormController2['default']({
        observe: '.js-sign-in-submit',
        successCallback: function successCallback(form, context, event) {

            var loginRegisterMethod = "login_register_component";
            if ($(form).parents('.pop-out__sign-in').length > 0) loginRegisterMethod = "global_login";

            var loginAnalytics = {
                event_name: 'login',
                login_state: 'successful',
                userName: '"' + $(form).find('input[name=username]').val() + '"',
                login_register_method: loginRegisterMethod
            };

            (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, loginAnalytics));

            /* * *
            If `pass-article-id` is set, user is probably trying to sign in
            after attempting to bookmark an article. Add the article ID to
            the URL so `autoBookmark()` catches it.
            * * */
            var passArticleId = $(form).find('.sign-in__submit').data('pass-article-id');
            if (passArticleId) {
                var sep = window.location.href.indexOf('?') > -1 ? '&' : '?';

                window.location.href = window.location.href + sep + 'immb=' + passArticleId;

                // If Angular, need location.reload to force page refresh
                if (typeof angular !== 'undefined') {
                    angular.element($('.search-results')[0]).controller().forceRefresh();
                }
            } else {
                window.location.reload(false);
            }
        },
        failureCallback: function failureCallback(form, context, event) {

            var loginAnalytics = {
                event_name: "Login Failure",
                login_state: "unsuccessful",
                userName: '"' + $(form).find('input[name=username]').val() + '"'
            };

            (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, loginAnalytics));
        }
    });

    var resetPassword = new _controllersFormController2['default']({
        observe: '.form-reset-password',
        successCallback: function successCallback() {
            $('.form-reset-password').find('.alert-success').show();
            var isPassword = $('.form-reset-password').data("is-password");
            if (isPassword) {
                (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, { event_name: "password reset success" }));
            }
        },
        failureCallback: function failureCallback() {
            var isPassword = $('.form-reset-password').data("is-password");
            if (isPassword) {
                (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, { event_name: "password reset failure" }));
            }
        }

    });

    var newResetPassToken = new _controllersFormController2['default']({
        observe: '.form-new-reset-pass-token',
        successCallback: function successCallback() {
            $('.form-new-reset-pass-token').find('.alert-success').show();
            (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, { event_name: "password reset success" }));
        },
        failureCallback: function failureCallback() {
            (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, { event_name: "password reset failure" }));
        }
    });

    $('.js-corporate-master-toggle').on('change', function () {
        if ($(this).prop('checked')) {
            $('.js-registration-corporate-wrapper').show();
        } else {
            $('.js-registration-corporate-wrapper').hide();
        }
    });

    var userRegistrationController = new _controllersFormController2['default']({
        observe: '.form-registration',
        successCallback: function successCallback(form, context, event) {

            // Stash registration type so next page can know it without
            // an additional Salesforce call
            _jscookie2['default'].set('registrationType', context.response.registration_type, {});

            (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, {
                registration_type: context.response.registration_type
            }));
        },
        failureCallback: function failureCallback(form, response) {

            var errorMsg = $(".page-registration__error").text();
            if (response.reasons && response.reasons.length > 0) {
                errorMsg = "[";
                for (var reason in response.reasons) {
                    errorMsg += response.reasons[reason] + ",";
                }
                errorMsg = errorMsg.substring(0, errorMsg.length - 1);
                errorMsg += "]";
            }
            (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, { event_name: "registration failure", registraion_errors: errorMsg }));
        }
    });

    var userRegistrationFinalController = new _controllersFormController2['default']({
        observe: '.form-registration-optins',
        successCallback: function successCallback(form, context, event) {

            var registrationType = _jscookie2['default'].get('registrationType');

            (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, {
                registration_type: registrationType
            }));

            _jscookie2['default'].remove('registrationType');
        },
        failureCallback: function failureCallback(form, response) {
            var errorMsg = $(".page-registration__error").text();
            if (response.reasons && response.reasons.length > 0) {
                errorMsg = "[";
                for (var reason in response.reasons) {
                    errorMsg += response.reasons[reason] + ",";
                }
                errorMsg = errorMsg.substring(0, errorMsg.length - 1);
                errorMsg += "]";
            }
            (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, { event_name: "registration failure", registraion_errors: errorMsg }));
        }
    });

    var userPreRegistrationController = new _controllersFormController2['default']({
        observe: '.form-pre-registration',
        successCallback: function successCallback(form) {
            var usernameInput = $(form).find('.js-register-username');

            var forwardingURL = $(form).data('forwarding-url');
            var sep = forwardingURL.indexOf('?') < 0 ? '?' : '&';
            var nextStepUrl = $(form).data('forwarding-url') + sep + usernameInput.attr('name') + '=' + encodeURIComponent(usernameInput.val());

            var loginRegisterMethod = "global_registration";
            if ($(form).hasClass("user-calltoaction")) loginRegisterMethod = "login_register_component";

            (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, { event_name: "registration", login_register_method: loginRegisterMethod }));

            window.location.href = nextStepUrl;
        }
    });

    $('.click-logout').on('click', function (e) {
        (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, { event_name: "logout" }));
    });

    //Specific to Sign Out Vertical Login
    $('.vertical__sign-out').on('click', function (e) {
        e.preventDefault();
        $("#hiddenforms_logout form").each(function () {
            $(this)[0].submit();
        });
        var Url = $(this).attr("href");
        window.location.href = Url;
    });

    var emailArticleController = new _controllersFormController2['default']({
        observe: '.form-email-article',
        successCallback: function successCallback(form) {
            $('.js-email-article-form-wrapper').hide();
            $('.js-email-article-recip-success').html($('.js-email-article-recip-addr').val().split(';').join('; '));
            $('.js-email-article-success').show();

            // Reset the Email Article pop-out to its default state when closed
            $('.js-dismiss-email-article').one('click', function () {
                $('.js-email-article-form-wrapper').show();
                $('.js-email-article-success').hide();
            });
        }
    });

    var emailSearchController = new _controllersFormController2['default']({
        observe: '.form-email-search',
        successCallback: function successCallback(form) {

            $('.js-email-search-form-wrapper').hide();
            $('.js-email-search-recip-success').html($('.js-email-search-recip-addr').val());
            $('.js-email-search-success').show();
            $('.js-email-search-form-wrapper input, .js-email-search-form-wrapper textarea').val('');

            // Reset the Email Article pop-out to its default state when closed
            $('.js-dismiss-email-search').one('click', function () {
                $('.js-email-search-form-wrapper').show();
                $('.js-email-search-success').hide();
            });

            var event_data = {
                event_name: "toolbar_use",
                toolbar_tool: "email"
            };

            (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, event_data));
        },
        beforeRequest: function beforeRequest() {

            var resultIDs = null;

            $('.js-search-results-id').each(function (indx, item) {
                resultIDs = resultIDs ? resultIDs + ',' + $(item).data('bookmark-id') : $(item).data('bookmark-id');
            });

            $('.js-email-search-results-ids').val(resultIDs);
            $('.js-email-search-query').val($('.search-bar__field').val());
            $('.js-email-search-query-url').val(document.location.href);
        }
    });

    var accountEmailPreferencesController = new _controllersFormController2['default']({
        observe: '.form-email-preferences',
        successCallback: function successCallback(form, context, event) {

            var event_data = {};
            var optingIn = null;
            var optingOut = null;

            if ($('#DoNotSendOffersOptIn').prop('checked')) {
                event_data.event_name = 'email_preferences_opt_out';
            } else {

                event_data.event_name = 'email_preferences_update';

                $('.js-account-email-checkbox').each(function (index, item) {
                    if (this.checked) {
                        optingIn = optingIn ? optingIn + '|' + this.value : this.value;
                    } else {
                        optingOut = optingOut ? optingOut + '|' + this.value : this.value;
                    }
                });

                event_data.email_preferences_optin = optingIn;
                event_data.email_preferences_optout = optingOut;
            }

            (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, event_data));
        }
    });

    var accountUpdatePassController = new _controllersFormController2['default']({
        observe: '.form-update-account-pass',
        successCallback: function successCallback(form, context, evt) {
            $(form).find('input, select, textarea').each(function () {
                $(this).val('');
            });
        }
    });

    var accountUpdateContactController = new _controllersFormController2['default']({
        observe: '.form-update-account-contact',
        successCallback: function successCallback(form, context, evt) {
            $(window).scrollTop($(evt.target).closest('form').find('.js-form-error-general').offset().top - 32);
        }
    });

    var savedDocumentsController = new _controllersFormController2['default']({
        observe: '.form-remove-saved-document',
        successCallback: function successCallback(form, context, evt) {
            $(evt.target).closest('tr').remove();
            if ($('.js-sortable-table tbody')[0].rows.length === 0) {
                $('.js-sortable-table').remove();
                $('.js-no-articles').show();
            }

            var event_data = {
                event_name: 'bookmark_removal',
                bookmark_title: $(form).data('analytics-title'),
                bookmark_publication: $(form).data('analytics-publication')
            };

            (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, event_data));
        }
    });

    (0, _svg4everybody2['default'])();

    /* * *
    MAIN SITE MENU
    * * */
    (function MenuController() {

        var getHeaderEdge = function getHeaderEdge() {
            return $('.header__wrapper').offset().top + $('.header__wrapper').height();
        };

        var showMenu = function showMenu() {
            $('.main-menu').addClass('is-active');
            $('.menu-toggler').addClass('is-active');
            $('.header__wrapper .menu-toggler').addClass('is-sticky');
            $('body').addClass('is-frozen');
        };

        var hideMenu = function hideMenu() {
            $('.main-menu').removeClass('is-active');
            $('.menu-toggler').removeClass('is-active');
            $('body').removeClass('is-frozen');
            if ($(window).scrollTop() <= getHeaderEdge()) {
                $('.header__wrapper .menu-toggler').removeClass('is-sticky');
            }
        };

        /* Toggle menu visibility */
        $('.js-menu-toggle-button').on('click', function toggleMenu(e) {
            $('.main-menu').hasClass('is-active') ? hideMenu() : showMenu();
            e.preventDefault();
            e.stopPropagation();
        });

        /*  If the menu is closed, let any clicks on the menu element open
        the menu. This includes the border—visible when the menu is closed—
        so it's easier to open. */
        $('.js-full-menu-toggle').on('click', function toggleMenu() {
            $('.main-menu').hasClass('is-active') ? null : showMenu();
        });

        /* Attach / detach sticky menu */
        $(window).on('scroll', function windowScrolled() {
            // Only stick if the header (including toggler) isn't visible
            if ($(window).scrollTop() > getHeaderEdge() || $('.main-menu').hasClass('is-active')) {
                $('.header__wrapper .menu-toggler').addClass('is-sticky');
            } else {
                $('.header__wrapper .menu-toggler').removeClass('is-sticky');
            }
        });

        /* Toggle menu categories */
        $('.js-toggle-menu-section').on('click', function toggleMenuItems(e) {
            e.target !== this ? this.click() : $(e.target).toggleClass('is-active');
        });

        $('.show-demo').click(function () {

            $(this).closest('.js-toggle-demo').toggleClass('collapsed');
            //IPMP-616	
            if ($(this).parent().hasClass('collapsed')) {
                sessionStorage.setItem("mykey", "false");
                $('.sd').show();
                $('.hd').hide();
                $('.toggle-demo').hide();
            } else {
                sessionStorage.setItem("mykey", "true");
                $('.hd').show();
                $('.sd').hide();
                $('.toggle-demo').show();
            }
        });
        var persistedval = sessionStorage.getItem("mykey");
        if (persistedval == 'false') {
            $('.sd').show();
            $('.hd').hide();
            $('.toggle-demo').hide();
            $('.js-toggle-demo').addClass('collapsed');
        } else {
            $('.hd').show();
            $('.sd').hide();
            $('.toggle-demo').show();
        }
    })();

    /* * *
    When a banner is dismissed, the banner ID is stored in the
    `dismissedBanners` cookie as a JSON object. Banners are invisible by default,
    so on page load, this checks if a banner on the page is dismissed or not,
    then makes the banner visible if not dismissed.
    * * */
    var dismissedBanners = _jscookie2['default'].getJSON('dismissedBanners') || {};
    $('.banner').each(function () {
        if ($(this).data('banner-id') in dismissedBanners === false) {
            $(this).addClass('is-visible');
        }
    });

    /* * *
    Generic banner dismiss
    * * */
    $('.js-dismiss-banner').on('click', function dismissBanner(e) {
        var thisBanner = $(e.target).parents('.banner');
        thisBanner.removeClass('is-visible');

        var dismissedBanners = _jscookie2['default'].getJSON('dismissedBanners') || {};
        dismissedBanners[thisBanner.data('banner-id')] = true;

        // if banner has a 'dismiss-all-subdomains' attribute = true, set the domain of the cookie
        // to the top-level domain.
        var domain = document.location.hostname;
        if (thisBanner.data('dismiss-all-subdomains')) {
            var parts = domain.split('.');
            parts.shift();
            domain = parts.join('.');
        }
        _jscookie2['default'].set('dismissedBanners', dismissedBanners, { expires: 3650, domain: domain });
    });

    // For each article table, clone and append "view full table" markup
    $('.article-body-content table').not('.article-table--mobile-link').forEach(function (e) {
        var mediaId = $(e).data("mediaid");
        var tableLink = $('.js-mobile-table-template .article-table').clone();

        var url = window.location.href;
        url.replace("#", "");
        if (url.indexOf("?") < 0) url += "?";else url += "&";

        url += "mobilemedia=true&selectedid=" + mediaId;

        // $(tableLink).find('a').attr("href", url);
        $(tableLink).find('a').data("table-url", url).attr('href', null);
        $(e).after(tableLink);
    });

    // Find duplicate embeds on article page
    // IITS2-312
    $('[class^=ewf-desktop-iframe] ~ [class^=ewf-mobile-iframe]').each(function (index, item) {
        $(item).remove();
    });

    // When DOM loads, render the appropriate iFrame components
    // Also add a listener for winder resize, render appropriate containers
    renderIframeComponents();
    renderTableau();
    renderAMchart();

    $(window).on('resize', function (event) {
        renderIframeComponents();
        renderTableau();
    });

    // Topic links
    var topicAnchors = $('.js-topic-anchor');

    $('.sub-topic-links').forEach(function (e) {
        var linkList = $(e).find('.bar-separated-link-list');

        topicAnchors.forEach(function (tc) {
            var id = tc.id;
            var text = $(tc).data('topic-link-text');
            var utagInfo = '{"event_name"="topic-jump-to-link-click","topic-name"="' + text + '"}';
            linkList.append('<a href="#' + id + '" class="click-utag" data-info=' + text + '>' + text + '</a>');
        });
    });

    // Display the Forgot Password block when "forgot your password" is clicked
    $('.js-show-forgot-password').on('click', function toggleForgotPass() {
        $('.js-reset-password-container').toggleClass('is-active');
    });

    // Global dismiss button for pop-outs
    $('.dismiss-button').on('click', function (e) {
        if (e.target !== this) {
            this.click();
            return;
        }

        $($(e.target).data('target-element')).removeClass('is-active');
        window.controlPopOuts.closePopOut(e.target);
    });

    // Make sure all external links open in a new window/tab
    $("a[href^=http]").each(function () {
        if (this.href.indexOf(location.hostname) == -1) {
            $(this).attr({
                target: "_blank"
            });
        }
    });

    // Adds analytics for article page clicks
    $('.root').find('a').each(function (index, item) {

        $(this).addClass('click-utag');

        var linkString;

        if (this.href.indexOf(location.hostname) == -1) {
            linkString = 'External:' + this.href;
        } else {
            linkString = this.href;
        }

        if ($(this).data('info') == undefined) {
            $(this).data('info', '{ "event_name": "embeded_link_click_through", "click_through_source": "' + $('h1').text + '", "click_through_destination": "' + linkString + '"}');
        }
    });

    $('.general-header__navigation').each(function () {

        $(this).on('scroll', function () {
            var scrollLeft = $(this).scrollLeft();
            var scrollWidth = $(this)[0].scrollWidth;
            var winWidth = $(window).width();

            if (scrollLeft > 32) {
                $('.general-header__navigation-scroller--left').addClass('is-visible');
            } else {
                $('.general-header__navigation-scroller--left').removeClass('is-visible');
            }

            if (scrollLeft + winWidth < scrollWidth - 32) {
                $('.general-header__navigation-scroller--right').addClass('is-visible');
            } else {
                $('.general-header__navigation-scroller--right').removeClass('is-visible');
            }
        });

        var scrollLeft = $(this).scrollLeft();
        var scrollWidth = $(this)[0].scrollWidth;
        var winWidth = $(window).width();

        if (scrollLeft + winWidth < scrollWidth - 32) {
            $('.general-header__navigation-scroller--right').addClass('is-visible');
        } else {
            $('.general-header__navigation-scroller--right').removeClass('is-visible');
        }
    });

    // Smooth, clickable scrolling for General page headers
    var smoothScrollingNav = function smoothScrollingNav() {

        // Cache for less DOM checking
        var Scrollable = $('.general-header__navigation');
        var Container = $('.general-header');

        // Find current scroll distance is from left and right edges
        var scrollDistance = function scrollDistance() {
            return {
                left: Scrollable.scrollLeft(),
                right: Scrollable[0].scrollWidth - (Container.width() + Scrollable.scrollLeft())
            };
        };

        var init = function init() {

            $('.general-header__navigation-scroller--right').on('click', function () {
                if (scrollDistance().right > 0) {
                    // Not on right edge
                    smoothScroll(200, 'right');
                }
            });

            $('.general-header__navigation-scroller--left').on('click', function () {
                if (scrollDistance().left > 0) {
                    smoothScroll(200, 'left');
                }
            });
        };

        var scrollToTimerCache;
        var totalTravel = null;
        var durationStart = null;

        // Quadratic ease-out algorithm
        var easing = function easing(time, distance) {
            return distance * (time * (2 - time));
        };

        var smoothScroll = function smoothScroll(duration, direction) {
            if (duration <= 0) {
                // Reset everything when duration time finishes
                totalTravel = null;
                durationStart = null;
                return;
            }

            // Store duration as durationStart on first loop
            durationStart = !durationStart ? duration : durationStart;

            // Store travel distance (container width) as totalTravel on first loop
            totalTravel = !totalTravel ? Container.width() : totalTravel;

            // Finds percentage of elapsed time since start
            var travelPcent = 1 - duration / durationStart;

            // Finds travel change on this loop, adjusted for ease-out
            var travel = easing(travelPcent, totalTravel / durationStart * 10);

            scrollToTimerCache = setTimeout((function () {
                if (!isNaN(parseInt(travel, 10))) {
                    if (direction === 'right') {
                        Scrollable.scrollLeft(Scrollable.scrollLeft() + travel);
                        smoothScroll(duration - 10, direction);
                    } else if (direction === 'left') {
                        Scrollable.scrollLeft(Scrollable.scrollLeft() - travel);
                        smoothScroll(duration - 10, direction);
                    }
                }
            }).bind(this), 10);
        };

        // Bind event listeners
        init();
    };

    $('.js-register-final').on('click', function (e) {
        var pub_newsletter = '';
        if (window.matchMedia("(max-width: 600px)").matches) {
            for (var i = 0; i < $('.mobile .newsletter_checkbox.wcs-c-on').length; i++) {
                pub_newsletter += $($('.site_div .newsletter_checkbox.wcs-c-on')[i]).prev().html() + ', ';
            }
        } else {
            for (var i = 0; i < $('.site_div .newsletter_checkbox.wcs-c-on').length; i++) {
                pub_newsletter += $($('.site_div .newsletter_checkbox.wcs-c-on')[i]).prev().html() + ', ';
            }
        }
        var eventDetails = {
            event_name: "newsletter-signup",
            page_name: "Newsletter",
            ga_eventCategory: "Newsletter",
            ga_eventLabel: analytics_data["publication"],
            publication_newsletter: pub_newsletter,
            user_news_email: analytics_data["user_email"]
        };
        var chkDetails = {};
        if ($('#newsletters').is(':checked')) {
            chkDetails.newsletter_optin = "true";

            eventDetails.newsletter_signup_state = "success";
            eventDetails.ga_eventAction = "Sign Up Success";

            $.extend(eventDetails, chkDetails);
            (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, eventDetails));
        } else {
            chkDetails.newsletter_optin = "false";

            eventDetails.newsletter_signup_state = "unsuccessful";
            eventDetails.ga_eventAction = "Sign Up Failure";

            $.extend(eventDetails, chkDetails);
            (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, eventDetails));
        }
    });

    // TODO - Refactor this code, update class name to a `js-` name
    $('.manage-preferences').click(function (e) {
        var preferencesData = {
            event_name: "manage-preferences"
        };
        if ($("#NewsletterOptIn").is(':checked') && $("#DoNotSendOffersOptIn").is(':checked')) {
            preferencesData = {
                newsletter_optin: "true",
                donot_send_offers_optin: "true"
            };
        }
        if (!$("#NewsletterOptIn").is(':checked') && $("#DoNotSendOffersOptIn").is(':checked')) {
            preferencesData = {
                newsletter_optin: "false",
                donot_send_offers_optin: "true"
            };
        }
        if ($("#NewsletterOptIn").is(':checked') && !$("#DoNotSendOffersOptIn").is(':checked')) {
            preferencesData = {
                newsletter_optin: "true",
                donot_send_offers_optin: "false"
            };
        }
        if (!$("#NewsletterOptIn").is(':checked') && !$("#DoNotSendOffersOptIn").is(':checked')) {
            preferencesData = {
                newsletter_optin: "false",
                donot_send_offers_optin: "false"
            };
        }

        (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, preferencesData));
    });

    // Execute!
    smoothScrollingNav();

    // Toggle global Informa bar
    $('.informa-ribbon__title').on('click', function (e) {
        $('.informa-ribbon').toggleClass('show');
    });

    $('.js-toggle-list').on('click', function (e) {
        $(e.target).closest('.js-togglable-list-wrapper').toggleClass('is-active');
    });

    $('.click-utag').click(function (e) {
        (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, $(this).data('info')));
    });

    $('.search-results').on('click', '.click-utag', function (e) {
        (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, $(this).data('info')));
    });

    $('#chkASBilling').click(function (e) {
        if ($(this).is(':checked')) {
            $('#ddlShippingCountry').val($('#ddlBillingCountry').val());
            $('#txtShippingAddress1').val($('#txtBillingAddress1').val());
            $('#txtShippingAddress2').val($('#txtBillingAddress2').val());
            $('#txtShippingCity').val($('#txtBillingCity').val());
            $('#txtShippingState').val($('#txtBillingState').val());
            $('#txtShippingPostalCode').val($('#txtBillingPostalCode').val());
        }
    });

    // Account - Email Preferences toggler
    $('.js-account-email-toggle-all').on('click', function (e) {
        $('.js-update-email-prefs').attr('disabled', null);
    });

    $('.js-account-email-checkbox').on('click', function (e) {
        $('.js-update-email-prefs').attr('disabled', null);
    });

    // Twitter sharing JS
    window.twttr = (function (t, e, r) {
        var n,
            i = t.getElementsByTagName(e)[0],
            w = window.twttr || {};
        return t.getElementById(r) ? w : (n = t.createElement(e), n.id = r, n.src = "https://platform.twitter.com/widgets.js", i.parentNode.insertBefore(n, i), w._e = [], w.ready = function (t) {
            w._e.push(t);
        }, w);
    })(document, "script", "twitter-wjs");

    // Pretty select boxes
    $('select:not(.ng-scope)').selectivity({
        showSearchInputInDropdown: false,
        positionDropdown: function positionDropdown($dropdownEl, $selectEl) {
            $dropdownEl.css("width", $selectEl.width() + "px");
        }
    });

    $(".selectivity-input .selectivity-single-select").each(function () {
        $(this).append('<span class="selectivity-arrow"><svg class="alert__icon"><use xlink:href="/dist/img/svg-sprite.svg#sort-down-arrow"></use></svg></span>');
    });

    //IPMP-1760
    if ($(".article-body-content aside:first").hasClass('article-inline-image')) {
        $('.package-control-articles__quarter').insertAfter('.article-body-content .article-inline-image');
    } else {
        $('.package-control-articles__quarter').insertAfter('.article-body-content .article-executive-summary');
    }

    // //IDE Login to Different Sales Force
    // $(document).ready(function() {
    //      var LoginUrl = window.location.href;
    //      if( (LoginUrl.indexOf('login=success') != -1) && $('.header-account-access__label').hasClass('header_salesforce_sign-in-out') ) {
    //          var UserId = $('.header-account-access__friendly-greeting').text().split('Hi, ')[1];
    //          $('#hiddenforms_login form').each(function() {
    //              $(this).find('input[type="text"]').val(UserId);
    //              $(this)[0].submit();
    //          });
    //      }
    // })
});

},{"./DragDropTouch":1,"./carousel/owl.carousel":3,"./carousel/zepto.data":4,"./components/AMCharts-merges-acquisition":5,"./components/accordionStockChart":6,"./components/amGraphParam":7,"./components/article-sidebar-component":8,"./components/dynamic-content-recomendation":9,"./components/header-logout":10,"./components/id-comparechart":11,"./components/id-comparefinancialresults":12,"./components/id-financial-responsive-table":13,"./components/id-merge-acquistion":14,"./components/id-quarterly-responsive-table":15,"./components/id-responsive-table":16,"./components/latest-casuality":17,"./components/ll-casuality-detail":18,"./components/ll-casuality-listing":19,"./components/ll-cockett-bunker.js":20,"./components/ll-market-data":24,"./components/ll-market-data-dryCargo":23,"./components/ll-market-data-dryCargo-bulkFixture":21,"./components/ll-market-data-dryCargo-icap":22,"./components/ll-market-imarex":25,"./components/ll-marketdata-drycargo-ssyal.js":26,"./components/ll-ship-coal-export.js":27,"./components/ll-ship-container-ship.js":28,"./components/ll-ship-roro.js":29,"./components/ll-ship-vehicle.js":30,"./components/ll-tanker-fixtures":31,"./components/ll-tanker-pure-chem-page.js":32,"./components/myview-settings":33,"./components/pagination":34,"./components/personalisation":35,"./components/save-search-component":36,"./components/scrollbar.js":37,"./components/table_charts":38,"./components/video-mini":39,"./controllers/analytics-controller":40,"./controllers/bookmark-controller":41,"./controllers/form-controller":42,"./controllers/lightbox-modal-controller":43,"./controllers/pop-out-controller":44,"./controllers/register-controller":45,"./controllers/reset-password-controller":46,"./controllers/sortable-table-controller":47,"./controllers/tooltip-controller":48,"./jscookie":50,"./modal":51,"./newsletter-signup":52,"./search-page.js":53,"./selectivity-full":54,"./svg4everybody":55,"./toggle-icons":56,"./zepto.dragswap":57,"./zepto.min":58}],50:[function(require,module,exports){
/*!
 * JavaScript Cookie v2.1.0
 * https://github.com/js-cookie/js-cookie
 *
 * Copyright 2006, 2015 Klaus Hartl & Fagner Brack
 * Released under the MIT license
 */
'use strict';

(function (factory) {
	if (typeof define === 'function' && define.amd) {
		define(factory);
	} else if (typeof exports === 'object') {
		module.exports = factory();
	} else {
		var _OldCookies = window.Cookies;
		var api = window.Cookies = factory();
		api.noConflict = function () {
			window.Cookies = _OldCookies;
			return api;
		};
	}
})(function () {
	function extend() {
		var i = 0;
		var result = {};
		for (; i < arguments.length; i++) {
			var attributes = arguments[i];
			for (var key in attributes) {
				result[key] = attributes[key];
			}
		}
		return result;
	}

	function init(converter) {
		function api(key, value, attributes) {
			var result;

			// Write

			if (arguments.length > 1) {
				attributes = extend({
					path: '/'
				}, api.defaults, attributes);

				if (typeof attributes.expires === 'number') {
					var expires = new Date();
					expires.setMilliseconds(expires.getMilliseconds() + attributes.expires * 864e+5);
					attributes.expires = expires;
				}

				try {
					result = JSON.stringify(value);
					if (/^[\{\[]/.test(result)) {
						value = result;
					}
				} catch (e) {}

				if (!converter.write) {
					value = encodeURIComponent(String(value)).replace(/%(23|24|26|2B|3A|3C|3E|3D|2F|3F|40|5B|5D|5E|60|7B|7D|7C)/g, decodeURIComponent);
				} else {
					value = converter.write(value, key);
				}

				key = encodeURIComponent(String(key));
				key = key.replace(/%(23|24|26|2B|5E|60|7C)/g, decodeURIComponent);
				key = key.replace(/[\(\)]/g, escape);

				return document.cookie = [key, '=', value, attributes.expires && '; expires=' + attributes.expires.toUTCString(), // use expires attribute, max-age is not supported by IE
				attributes.path && '; path=' + attributes.path, attributes.domain && '; domain=' + attributes.domain, attributes.secure ? '; secure' : ''].join('');
			}

			// Read

			if (!key) {
				result = {};
			}

			// To prevent the for loop in the first place assign an empty array
			// in case there are no cookies at all. Also prevents odd result when
			// calling "get()"
			var cookies = document.cookie ? document.cookie.split('; ') : [];
			var rdecode = /(%[0-9A-Z]{2})+/g;
			var i = 0;

			for (; i < cookies.length; i++) {
				var parts = cookies[i].split('=');
				var name = parts[0].replace(rdecode, decodeURIComponent);
				var cookie = parts.slice(1).join('=');

				if (cookie.charAt(0) === '"') {
					cookie = cookie.slice(1, -1);
				}

				try {
					cookie = converter.read ? converter.read(cookie, name) : converter(cookie, name) || cookie.replace(rdecode, decodeURIComponent);

					if (this.json) {
						try {
							cookie = JSON.parse(cookie);
						} catch (e) {}
					}

					if (key === name) {
						result = cookie;
						break;
					}

					if (!key) {
						result[name] = cookie;
					}
				} catch (e) {}
			}

			return result;
		}

		api.get = api.set = api;
		api.getJSON = function () {
			return api.apply({
				json: true
			}, [].slice.call(arguments));
		};
		api.defaults = {};

		api.remove = function (key, attributes) {
			api(key, '', extend(attributes, {
				expires: -1
			}));
		};

		api.withConverter = init;

		return api;
	}

	return init(function () {});
});

},{}],51:[function(require,module,exports){
/* ========================================================================
 * Bootstrap: modal.js v3.3.7
 * http://getbootstrap.com/javascript/#modals
 * ========================================================================
 * Copyright 2011-2016 Twitter, Inc.
 * Licensed under MIT (https://github.com/twbs/bootstrap/blob/master/LICENSE)
 * ======================================================================== */

'use strict';

+(function ($) {
  'use strict';

  // MODAL CLASS DEFINITION
  // ======================

  var Modal = function Modal(element, options) {
    this.options = options;
    this.$body = $(document.body);
    this.$element = $(element);
    this.$dialog = this.$element.find('.modal-dialog');
    this.$backdrop = null;
    this.isShown = null;
    this.originalBodyPad = null;
    this.scrollbarWidth = 0;
    this.ignoreBackdropClick = false;

    if (this.options.remote) {
      this.$element.find('.modal-content').load(this.options.remote, $.proxy(function () {
        this.$element.trigger('loaded.bs.modal');
      }, this));
    }
  };

  Modal.VERSION = '3.3.7';

  Modal.TRANSITION_DURATION = 300;
  Modal.BACKDROP_TRANSITION_DURATION = 150;

  Modal.DEFAULTS = {
    backdrop: true,
    keyboard: true,
    show: true
  };

  Modal.prototype.toggle = function (_relatedTarget) {
    return this.isShown ? this.hide() : this.show(_relatedTarget);
  };

  Modal.prototype.show = function (_relatedTarget) {
    var that = this;
    var e = $.Event('show.bs.modal', { relatedTarget: _relatedTarget });

    this.$element.trigger(e);

    if (this.isShown || e.isDefaultPrevented()) return;

    this.isShown = true;

    this.checkScrollbar();
    this.setScrollbar();
    this.$body.addClass('modal-open');

    this.escape();
    this.resize();

    this.$element.on('click.dismiss.bs.modal', '[data-dismiss="modal"]', $.proxy(this.hide, this));

    this.$dialog.on('mousedown.dismiss.bs.modal', function () {
      that.$element.one('mouseup.dismiss.bs.modal', function (e) {
        if ($(e.target).is(that.$element)) that.ignoreBackdropClick = true;
      });
    });

    this.backdrop(function () {
      var transition = $.support.transition && that.$element.hasClass('fade');

      if (!that.$element.parent().length) {
        that.$element.appendTo(that.$body); // don't move modals dom position
      }

      that.$element.show().scrollTop(0);

      that.adjustDialog();

      if (transition) {
        that.$element[0].offsetWidth; // force reflow
      }

      that.$element.addClass('in');

      that.enforceFocus();

      var e = $.Event('shown.bs.modal', { relatedTarget: _relatedTarget });

      transition ? that.$dialog // wait for modal to slide in
      .one('bsTransitionEnd', function () {
        that.$element.trigger('focus').trigger(e);
      }).emulateTransitionEnd(Modal.TRANSITION_DURATION) : that.$element.trigger('focus').trigger(e);
    });
  };

  Modal.prototype.hide = function (e) {
    if (e) e.preventDefault();

    e = $.Event('hide.bs.modal');

    this.$element.trigger(e);

    if (!this.isShown || e.isDefaultPrevented()) return;

    this.isShown = false;

    this.escape();
    this.resize();

    $(document).off('focusin.bs.modal');

    this.$element.removeClass('in').off('click.dismiss.bs.modal').off('mouseup.dismiss.bs.modal');

    this.$dialog.off('mousedown.dismiss.bs.modal');

    $.support.transition && this.$element.hasClass('fade') ? this.$element.one('bsTransitionEnd', $.proxy(this.hideModal, this)).emulateTransitionEnd(Modal.TRANSITION_DURATION) : this.hideModal();
  };

  Modal.prototype.enforceFocus = function () {
    $(document).off('focusin.bs.modal') // guard against infinite focus loop
    .on('focusin.bs.modal', $.proxy(function (e) {
      if (document !== e.target && this.$element[0] !== e.target && !this.$element.has(e.target).length) {
        this.$element.trigger('focus');
      }
    }, this));
  };

  Modal.prototype.escape = function () {
    if (this.isShown && this.options.keyboard) {
      this.$element.on('keydown.dismiss.bs.modal', $.proxy(function (e) {
        e.which == 27 && this.hide();
      }, this));
    } else if (!this.isShown) {
      this.$element.off('keydown.dismiss.bs.modal');
    }
  };

  Modal.prototype.resize = function () {
    if (this.isShown) {
      $(window).on('resize.bs.modal', $.proxy(this.handleUpdate, this));
    } else {
      $(window).off('resize.bs.modal');
    }
  };

  Modal.prototype.hideModal = function () {
    var that = this;
    this.$element.hide();
    this.backdrop(function () {
      that.$body.removeClass('modal-open');
      that.resetAdjustments();
      that.resetScrollbar();
      that.$element.trigger('hidden.bs.modal');
    });
  };

  Modal.prototype.removeBackdrop = function () {
    this.$backdrop && this.$backdrop.remove();
    this.$backdrop = null;
  };

  Modal.prototype.backdrop = function (callback) {
    var that = this;
    var animate = this.$element.hasClass('fade') ? 'fade' : '';

    if (this.isShown && this.options.backdrop) {
      var doAnimate = $.support.transition && animate;

      this.$backdrop = $(document.createElement('div')).addClass('modal-backdrop ' + animate).appendTo(this.$body);

      this.$element.on('click.dismiss.bs.modal', $.proxy(function (e) {
        if (this.ignoreBackdropClick) {
          this.ignoreBackdropClick = false;
          return;
        }
        if (e.target !== e.currentTarget) return;
        this.options.backdrop == 'static' ? this.$element[0].focus() : this.hide();
      }, this));

      if (doAnimate) this.$backdrop[0].offsetWidth; // force reflow

      this.$backdrop.addClass('in');

      if (!callback) return;

      doAnimate ? this.$backdrop.one('bsTransitionEnd', callback).emulateTransitionEnd(Modal.BACKDROP_TRANSITION_DURATION) : callback();
    } else if (!this.isShown && this.$backdrop) {
      this.$backdrop.removeClass('in');

      var callbackRemove = function callbackRemove() {
        that.removeBackdrop();
        callback && callback();
      };
      $.support.transition && this.$element.hasClass('fade') ? this.$backdrop.one('bsTransitionEnd', callbackRemove).emulateTransitionEnd(Modal.BACKDROP_TRANSITION_DURATION) : callbackRemove();
    } else if (callback) {
      callback();
    }
  };

  // these following methods are used to handle overflowing modals

  Modal.prototype.handleUpdate = function () {
    this.adjustDialog();
  };

  Modal.prototype.adjustDialog = function () {
    var modalIsOverflowing = this.$element[0].scrollHeight > document.documentElement.clientHeight;

    this.$element.css({
      paddingLeft: !this.bodyIsOverflowing && modalIsOverflowing ? this.scrollbarWidth : '',
      paddingRight: this.bodyIsOverflowing && !modalIsOverflowing ? this.scrollbarWidth : ''
    });
  };

  Modal.prototype.resetAdjustments = function () {
    this.$element.css({
      paddingLeft: '',
      paddingRight: ''
    });
  };

  Modal.prototype.checkScrollbar = function () {
    var fullWindowWidth = window.innerWidth;
    if (!fullWindowWidth) {
      // workaround for missing window.innerWidth in IE8
      var documentElementRect = document.documentElement.getBoundingClientRect();
      fullWindowWidth = documentElementRect.right - Math.abs(documentElementRect.left);
    }
    this.bodyIsOverflowing = document.body.clientWidth < fullWindowWidth;
    this.scrollbarWidth = this.measureScrollbar();
  };

  Modal.prototype.setScrollbar = function () {
    var bodyPad = parseInt(this.$body.css('padding-right') || 0, 10);
    this.originalBodyPad = document.body.style.paddingRight || '';
    if (this.bodyIsOverflowing) this.$body.css('padding-right', bodyPad + this.scrollbarWidth);
  };

  Modal.prototype.resetScrollbar = function () {
    this.$body.css('padding-right', this.originalBodyPad);
  };

  Modal.prototype.measureScrollbar = function () {
    // thx walsh
    var scrollDiv = document.createElement('div');
    scrollDiv.className = 'modal-scrollbar-measure';
    this.$body.append(scrollDiv);
    var scrollbarWidth = scrollDiv.offsetWidth - scrollDiv.clientWidth;
    this.$body[0].removeChild(scrollDiv);
    return scrollbarWidth;
  };

  // MODAL PLUGIN DEFINITION
  // =======================

  function Plugin(option, _relatedTarget) {
    return this.each(function () {
      var $this = $(this);
      var data = $this.data('bs.modal');
      var options = $.extend({}, Modal.DEFAULTS, $this.data(), typeof option == 'object' && option);

      if (!data) $this.data('bs.modal', data = new Modal(this, options));
      if (typeof option == 'string') data[option](_relatedTarget);else if (options.show) data.show(_relatedTarget);
    });
  }

  var old = $.fn.modal;

  $.fn.modal = Plugin;
  $.fn.modal.Constructor = Modal;

  // MODAL NO CONFLICT
  // =================

  $.fn.modal.noConflict = function () {
    $.fn.modal = old;
    return this;
  };

  // MODAL DATA-API
  // ==============

  $(document).on('click.bs.modal.data-api', '[data-toggle="modal"]', function (e) {
    var $this = $(this);
    var href = $this.attr('href');
    var $target = $($this.attr('data-target') || href && href.replace(/.*(?=#[^\s]+$)/, '')); // strip for ie7
    var option = $target.data('bs.modal') ? 'toggle' : $.extend({ remote: !/#/.test(href) && href }, $target.data(), $this.data());

    if ($this.is('a')) e.preventDefault();

    $target.one('show.bs.modal', function (showEvent) {
      if (showEvent.isDefaultPrevented()) return; // only register focus restorer if modal will actually get shown
      $target.one('hidden.bs.modal', function () {
        $this.is(':visible') && $this.trigger('focus');
      });
    });
    Plugin.call($target, option, this);
  });
})($);

},{}],52:[function(require,module,exports){
/* global analytics_data */

'use strict';

Object.defineProperty(exports, '__esModule', {
    value: true
});

var _controllersAnalyticsController = require('./controllers/analytics-controller');

function newsletterSignupController() {

    this.checkForUserSignedUp = function () {
        $.get('/Account/api/PreferencesApi/IsUserSignedUp', function (response) {
            var res = response;
            if (response) {
                $(".newsletter-signup").hide();
            }
        });
    };

    this.IsValidEmail = function (email) {
        var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    };
    this.addControl = function (triggerElement, successCallback, failureCallback) {
        var _this = this;

        if (triggerElement) {
            $(triggerElement).on('click', function (event) {

                // Prevent form submit
                event.preventDefault();

                // Hide any errors
                $('.js-newsletter-signup-error').hide();
                $('.newsletter-signup-needs-registration').hide();

                var inputData = $("#newsletterUserName").val();
                var url = $(triggerElement).data('signup-url');

                //$(triggerElement).parents('.newsletter-signup').find('input').each(function() {
                //    inputData = $(this).val();
                //});

                if (inputData !== '' && _this.IsValidEmail(inputData)) {
                    $('.js-newsletter-signup--error-invalidemailformat').hide();
                    url = url + '?userName=' + inputData;

                    $.get(url, function (response) {
                        var newsletterAnalytics;

                        if (response == 'true') {

                            newsletterAnalytics = {
                                event_name: 'newsletter-signup',
                                newsletter_signup_state: 'successful',
                                userName: '"' + inputData + '"'
                            };

                            (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, newsletterAnalytics));

                            $(".newsletter-signup-before-submit").hide();
                            $(".newsletter-signup-after-submit").show();
                        } else if (response == 'mustregister') {

                            newsletterAnalytics = {
                                event_name: 'newsletter-signup',
                                newsletter_signup_state: 'unsuccessful',
                                userName: '"' + inputData + '"'
                            };

                            (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, newsletterAnalytics));

                            $('.newsletter-signup-needs-registration a').attr('href', $('.newsletter-signup-needs-registration a').attr('href') + $('.newsletter-signup-before-submit input').val());

                            $('.newsletter-signup-before-submit').hide();
                            $('.newsletter-signup-needs-registration').show();
                        } else {
                            newsletterAnalytics = {
                                event_name: 'newsletter-signup',
                                newsletter_signup_state: 'unsuccessful',
                                userName: '"' + inputData + '"'
                            };

                            (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, newsletterAnalytics));

                            $('.js-newsletter-signup-error').show();
                        }
                    });
                } else {
                    $('.js-newsletter-signup--error-invalidemailformat').show();
                }
            });
        }
    };
}

exports['default'] = newsletterSignupController;
module.exports = exports['default'];

},{"./controllers/analytics-controller":40}],53:[function(require,module,exports){
'use strict';

var SearchScript = (function () {

	/* Toggle search tips visibility */
	$('.js-toggle-search-tips').on('click', function toggleTips() {
		$('.search-bar__tips').toggleClass('open');
		$('.search-bar').toggleClass('tips-open');
	});
})();

},{}],54:[function(require,module,exports){
(function (global){
/**
 * @license
 * Selectivity.js 2.1.0 <https://arendjr.github.io/selectivity/>
 * Copyright (c) 2014-2016 Arend van Beelen jr.
 *           (c) 2016 Speakap BV
 * Available under MIT license <https://github.com/arendjr/selectivity/blob/master/LICENSE>
 */"use strict";!(function(e){if("object" == typeof exports && "undefined" != typeof module)module.exports = e();else if("function" == typeof define && define.amd)define([],e);else {var f;"undefined" != typeof window?f = window:"undefined" != typeof global?f = global:"undefined" != typeof self && (f = self),f.selectivity = e();}})(function(){var define,module,exports;return (function e(t,n,r){function s(o,u){if(!n[o]){if(!t[o]){var a=typeof require == "function" && require;if(!u && a)return a(o,!0);if(i)return i(o,!0);var f=new Error("Cannot find module '" + o + "'");throw (f.code = "MODULE_NOT_FOUND",f);}var l=n[o] = {exports:{}};t[o][0].call(l.exports,function(e){var n=t[o][1][e];return s(n?n:e);},l,l.exports,e,t,n,r);}return n[o].exports;}var i=typeof require == "function" && require;for(var o=0;o < r.length;o++) s(r[o]);return s;})({1:[function(_dereq_,module,exports){_dereq_(5);_dereq_(6);_dereq_(7);_dereq_(9);_dereq_(10);_dereq_(11);_dereq_(12);_dereq_(13);_dereq_(14);_dereq_(15);_dereq_(16);_dereq_(17);_dereq_(18);_dereq_(19);module.exports = _dereq_(8);},{"10":10,"11":11,"12":12,"13":13,"14":14,"15":15,"16":16,"17":17,"18":18,"19":19,"5":5,"6":6,"7":7,"8":8,"9":9}],2:[function(_dereq_,module,exports){'use strict';var $=window.jQuery || window.Zepto; /**
 * Event Delegator Constructor.
 */function EventDelegator(){this._events = [];this.delegateEvents();} /**
 * Methods.
 */$.extend(EventDelegator.prototype,{ /**
     * Attaches all listeners from the events map to the instance's element.
     *
     * Normally, you should not have to call this method yourself as it's called automatically in
     * the constructor.
     */delegateEvents:function delegateEvents(){this.undelegateEvents();$.each(this.events,(function(event,listener){var selector,index=event.indexOf(' ');if(index > -1){selector = event.slice(index + 1);event = event.slice(0,index);}if($.type(listener) === 'string'){listener = this[listener];}listener = listener.bind(this);if(selector){this.$el.on(event,selector,listener);}else {this.$el.on(event,listener);}this._events.push({event:event,selector:selector,listener:listener});}).bind(this));}, /**
     * Detaches all listeners from the events map from the instance's element.
     */undelegateEvents:function undelegateEvents(){this._events.forEach(function(event){if(event.selector){this.$el.off(event.event,event.selector,event.listener);}else {this.$el.off(event.event,event.listener);}},this);this._events = [];}});module.exports = EventDelegator;},{"jquery":"jquery"}],3:[function(_dereq_,module,exports){'use strict'; /**
 * @license
 * lodash 3.3.1 (Custom Build) <https://lodash.com/>
 * Copyright 2012-2015 The Dojo Foundation <http://dojofoundation.org/>
 * Based on Underscore.js 1.8.2 <http://underscorejs.org/LICENSE>
 * Copyright 2009-2015 Jeremy Ashkenas, DocumentCloud and Investigative Reporters & Editors
 * Available under MIT license <https://lodash.com/license>
 */ /**
 * Gets the number of milliseconds that have elapsed since the Unix epoch
 *  (1 January 1970 00:00:00 UTC).
 *
 * @static
 * @memberOf _
 * @category Date
 * @example
 *
 * _.defer(function(stamp) {
 *   console.log(_.now() - stamp);
 * }, _.now());
 * // => logs the number of milliseconds it took for the deferred function to be invoked
 */var now=Date.now; /**
 * Creates a function that delays invoking `func` until after `wait` milliseconds
 * have elapsed since the last time it was invoked.
 *
 * See [David Corbacho's article]
 *                        (http://drupalmotion.com/article/debounce-and-throttle-visual-explanation)
 * for details over the differences between `_.debounce` and `_.throttle`.
 *
 * @static
 * @memberOf _
 * @category Function
 * @param {Function} func The function to debounce.
 * @param {number} [wait=0] The number of milliseconds to delay.
 * @returns {Function} Returns the new debounced function.
 * @example
 *
 * // avoid costly calculations while the window size is in flux
 * jQuery(window).on('resize', _.debounce(calculateLayout, 150));
 */function debounce(func,wait){var args,result,stamp,timeoutId,trailingCall,lastCalled=0;wait = wait < 0?0:+wait || 0;function delayed(){var remaining=wait - (now() - stamp);if(remaining <= 0 || remaining > wait){var isCalled=trailingCall;timeoutId = trailingCall = undefined;if(isCalled){lastCalled = now();result = func.apply(null,args);if(!timeoutId){args = null;}}}else {timeoutId = setTimeout(delayed,remaining);}}function debounced(){args = arguments;stamp = now();trailingCall = true;if(!timeoutId){timeoutId = setTimeout(delayed,wait);}return result;}return debounced;}module.exports = debounce;},{}],4:[function(_dereq_,module,exports){'use strict'; /**
 * @license
 * Lo-Dash 2.4.1 (Custom Build) <http://lodash.com/>
 * Copyright 2012-2013 The Dojo Foundation <http://dojofoundation.org/>
 * Based on Underscore.js 1.5.2 <http://underscorejs.org/LICENSE>
 * Copyright 2009-2013 Jeremy Ashkenas, DocumentCloud and Investigative Reporters & Editors
 * Available under MIT license <http://lodash.com/license>
 */var htmlEscapes={'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}; /**
 * Used by `escape` to convert characters to HTML entities.
 *
 * @private
 * @param {string} match The matched character to escape.
 * @returns {string} Returns the escaped character.
 */function escapeHtmlChar(match){return htmlEscapes[match];}var reUnescapedHtml=new RegExp('[' + Object.keys(htmlEscapes).join('') + ']','g'); /**
 * Converts the characters `&`, `<`, `>`, `"`, and `'` in `string` to their
 * corresponding HTML entities.
 *
 * @static
 * @memberOf _
 * @category Utilities
 * @param {string} string The string to escape.
 * @returns {string} Returns the escaped string.
 * @example
 *
 * _.escape('Fred, Wilma, & Pebbles');
 * // => 'Fred, Wilma, &amp; Pebbles'
 */function escape(string){return string?String(string).replace(reUnescapedHtml,escapeHtmlChar):'';}module.exports = escape;},{}],5:[function(_dereq_,module,exports){'use strict';var $=window.jQuery || window.Zepto;var debounce=_dereq_(3);var Selectivity=_dereq_(8);_dereq_(13); /**
 * Option listener that implements a convenience query function for performing AJAX requests.
 */Selectivity.OptionListeners.unshift(function(selectivity,options){var ajax=options.ajax;if(ajax && ajax.url){var formatError=ajax.formatError || Selectivity.Locale.ajaxError;var minimumInputLength=ajax.minimumInputLength || 0;var params=ajax.params;var processItem=ajax.processItem || function(item){return item;};var quietMillis=ajax.quietMillis || 0;var resultsCb=ajax.results || function(data){return {results:data,more:false};};var transport=ajax.transport || $.ajax;if(quietMillis){transport = debounce(transport,quietMillis);}options.query = function(queryOptions){var offset=queryOptions.offset;var term=queryOptions.term;if(term.length < minimumInputLength){queryOptions.error(Selectivity.Locale.needMoreCharacters(minimumInputLength - term.length));}else {var url=ajax.url instanceof Function?ajax.url(queryOptions):ajax.url;if(params){url += (url.indexOf('?') > -1?'&':'?') + $.param(params(term,offset));}var _success=ajax.success;var _error=ajax.error;transport($.extend({},ajax,{url:url,success:function success(data,textStatus,jqXHR){if(_success){_success(data,textStatus,jqXHR);}var results=resultsCb(data,offset);results.results = results.results.map(processItem);queryOptions.callback(results);},error:function error(jqXHR,textStatus,errorThrown){if(_error){_error(jqXHR,textStatus,errorThrown);}queryOptions.error(formatError(term,jqXHR,textStatus,errorThrown),{escape:false});}}));}};}});},{"13":13,"3":3,"8":8,"jquery":"jquery"}],6:[function(_dereq_,module,exports){'use strict';var Selectivity=_dereq_(8);var latestQueryNum=0; /**
 * Option listener that will discard any callbacks from the query function if another query has
 * been called afterwards. This prevents responses from remote sources arriving out-of-order.
 */Selectivity.OptionListeners.push(function(selectivity,options){var query=options.query;if(query && !query._async){options.query = function(queryOptions){latestQueryNum++;var queryNum=latestQueryNum;var callback=queryOptions.callback;var error=queryOptions.error;queryOptions.callback = function(){if(queryNum === latestQueryNum){callback.apply(null,arguments);}};queryOptions.error = function(){if(queryNum === latestQueryNum){error.apply(null,arguments);}};query(queryOptions);};options.query._async = true;}});},{"8":8}],7:[function(_dereq_,module,exports){'use strict';var $=window.jQuery || window.Zepto;var SelectivityDropdown=_dereq_(10); /**
 * Methods.
 */$.extend(SelectivityDropdown.prototype,{ /**
     * @inherit
     */removeCloseHandler:function removeCloseHandler(){if(this._$backdrop && !this.parentMenu){this._$backdrop.remove();this._$backdrop = null;}}, /**
     * @inherit
     */setupCloseHandler:function setupCloseHandler(){var $backdrop;if(this.parentMenu){$backdrop = this.parentMenu._$backdrop;}else {$backdrop = $('<div>').addClass('selectivity-backdrop');$('body').append($backdrop);}$backdrop.on('click',this.close.bind(this));this._$backdrop = $backdrop;}});},{"10":10,"jquery":"jquery"}],8:[function(_dereq_,module,exports){'use strict';var $=window.jQuery || window.Zepto;var EventDelegator=_dereq_(2); /**
 * Create a new Selectivity instance or invoke a method on an instance.
 *
 * @param methodName Optional name of a method to call. If omitted, a Selectivity instance is
 *                   created for each element in the set of matched elements. If an element in the
 *                   set already has a Selectivity instance, the result is the same as if the
 *                   setOptions() method is called. If a method name is given, the options
 *                   parameter is ignored and any additional parameters are passed to the given
 *                   method.
 * @param options Options object to pass to the constructor or the setOptions() method. In case
 *                a new instance is being created, the following properties are used:
 *                inputType - The input type to use. Default input types include 'Multiple' and
 *                            'Single', but you can add custom input types to the InputTypes map or
 *                            just specify one here as a function. The default value is 'Single',
 *                            unless multiple is true in which case it is 'Multiple'.
 *                multiple - Boolean determining whether multiple items may be selected
 *                           (default: false). If true, a MultipleSelectivity instance is created,
 *                           otherwise a SingleSelectivity instance is created.
 *
 * @return If the given method returns a value, this method returns the value of that method
 *         executed on the first element in the set of matched elements.
 */function selectivity(methodName,options){ /* jshint validthis: true */var methodArgs=Array.prototype.slice.call(arguments,1);var result;this.each(function(){var instance=this.selectivity;if(instance){if($.type(methodName) !== 'string'){methodArgs = [methodName];methodName = 'setOptions';}if($.type(instance[methodName]) === 'function'){if(result === undefined){result = instance[methodName].apply(instance,methodArgs);}}else {throw new Error('Unknown method: ' + methodName);}}else {if($.type(methodName) === 'string'){if(methodName !== 'destroy'){throw new Error('Cannot call method on element without Selectivity instance');}}else {options = $.extend({},methodName,{element:this}); // this is a one-time hack to facilitate the selectivity-traditional module, because
// the module is not able to hook this early into creation of the instance
var $this=$(this);if($this.is('select') && $this.prop('multiple')){options.multiple = true;}var InputTypes=Selectivity.InputTypes;var InputType=options.inputType || (options.multiple?'Multiple':'Single');if($.type(InputType) !== 'function'){if(InputTypes[InputType]){InputType = InputTypes[InputType];}else {throw new Error('Unknown Selectivity input type: ' + InputType);}}this.selectivity = new InputType(options);}}});return result === undefined?this:result;} /**
 * Selectivity Base Constructor.
 *
 * You will never use this constructor directly. Instead, you use $(selector).selectivity(options)
 * to create an instance of either MultipleSelectivity or SingleSelectivity. This class defines all
 * functionality that is common between both.
 *
 * @param options Options object. Accepts the same options as the setOptions method(), in addition
 *                to the following ones:
 *                data - Initial selection data to set. This should be an array of objects with 'id'
 *                       and 'text' properties. This option is mutually exclusive with 'value'.
 *                element - The DOM element to which to attach the Selectivity instance. This
 *                          property is set automatically by the $.fn.selectivity() function.
 *                value - Initial value to set. This should be an array of IDs. This property is
 *                        mutually exclusive with 'data'.
 */function Selectivity(options){if(!(this instanceof Selectivity)){return selectivity.apply(this,arguments);} /**
     * jQuery container for the element to which this instance is attached.
     */this.$el = $(options.element); /**
     * jQuery container for the search input.
     *
     * May be null as long as there is no visible search input. It is set by initSearchInput().
     */this.$searchInput = null; /**
     * Reference to the currently open dropdown.
     */this.dropdown = null; /**
     * Whether the input is enabled.
     *
     * This is false when the option readOnly is false or the option removeOnly is false.
     */this.enabled = true; /**
     * Boolean whether the browser has touch input.
     */this.hasTouch = typeof window !== 'undefined' && 'ontouchstart' in window; /**
     * Boolean whether the browser has a physical keyboard attached to it.
     *
     * Given that there is no way for JavaScript to reliably detect this yet, we just assume it's
     * the opposite of hasTouch for now.
     */this.hasKeyboard = !this.hasTouch; /**
     * Array of items from which to select. If set, this will be an array of objects with 'id' and
     * 'text' properties.
     *
     * If given, all items are expected to be available locally and all selection operations operate
     * on this local array only. If null, items are not available locally, and a query function
     * should be provided to fetch remote data.
     */this.items = null; /**
     * The function to be used for matching search results.
     */this.matcher = Selectivity.matcher; /**
     * Options passed to the Selectivity instance or set through setOptions().
     */this.options = {}; /**
     * Array of search input listeners.
     *
     * Custom listeners can be specified in the options object.
     */this.searchInputListeners = Selectivity.SearchInputListeners; /**
     * Mapping of templates.
     *
     * Custom templates can be specified in the options object.
     */this.templates = $.extend({},Selectivity.Templates); /**
     * The last used search term.
     */this.term = '';this.setOptions(options);if(options.value){this.value(options.value,{triggerChange:false});}else {this.data(options.data || null,{triggerChange:false});}this.$el.on('mouseover',this._mouseover.bind(this));this.$el.on('mouseleave',this._mouseout.bind(this));this.$el.on('selectivity-close',this._closed.bind(this));EventDelegator.call(this);} /**
 * Methods.
 */$.extend(Selectivity.prototype,EventDelegator.prototype,{ /**
     * Convenience shortcut for this.$el.find(selector).
     */$:function $(selector){return this.$el.find(selector);}, /**
     * Closes the dropdown.
     */close:function close(){if(this.dropdown){this.dropdown.close();}}, /**
     * Sets or gets the selection data.
     *
     * The selection data contains both IDs and text labels. If you only want to set or get the IDs,
     * you should use the value() method.
     *
     * @param newData Optional new data to set. For a MultipleSelectivity instance the data must be
     *                an array of objects with 'id' and 'text' properties, for a SingleSelectivity
     *                instance the data must be a single such object or null to indicate no item is
     *                selected.
     * @param options Optional options object. May contain the following property:
     *                triggerChange - Set to false to suppress the "change" event being triggered.
     *                                Note this will also cause the UI to not update automatically;
     *                                so you may want to call rerenderSelection() manually when
     *                                using this option.
     *
     * @return If newData is omitted, this method returns the current data.
     */data:function data(newData,options){options = options || {};if(newData === undefined){return this._data;}else {newData = this.validateData(newData);this._data = newData;this._value = this.getValueForData(newData);if(options.triggerChange !== false){this.triggerChange();}}}, /**
     * Destroys the Selectivity instance.
     */destroy:function destroy(){this.undelegateEvents();var $el=this.$el;$el.children().remove();$el[0].selectivity = null;$el = null;}, /**
     * Filters the results to be displayed in the dropdown.
     *
     * The default implementation simply returns the results unfiltered, but the MultipleSelectivity
     * class overrides this method to filter out any items that have already been selected.
     *
     * @param results Array of items with 'id' and 'text' properties.
     *
     * @return The filtered array.
     */filterResults:function filterResults(results){return results;}, /**
     * Applies focus to the input.
     */focus:function focus(){if(this.$searchInput){this.$searchInput.focus();}}, /**
     * Returns the correct item for a given ID.
     *
     * @param id The ID to get the item for.
     *
     * @return The corresponding item. Will be an object with 'id' and 'text' properties or null if
     *         the item cannot be found. Note that if no items are defined, this method assumes the
     *         text labels will be equal to the IDs.
     */getItemForId:function getItemForId(id){var items=this.items;if(items){return Selectivity.findNestedById(items,id);}else {return {id:id,text:'' + id};}}, /**
     * Initializes the search input element.
     *
     * Sets the $searchInput property, invokes all search input listeners and attaches the default
     * action of searching when something is typed.
     *
     * @param $input jQuery container for the input element.
     * @param options Optional options object. May contain the following property:
     *                noSearch - If true, no event handlers are setup to initiate searching when
     *                           the user types in the input field. This is useful if you want to
     *                           use the input only to handle keyboard support.
     */initSearchInput:function initSearchInput($input,options){this.$searchInput = $input;this.searchInputListeners.forEach((function(listener){listener(this,$input);}).bind(this));if(!options || !options.noSearch){$input.on('keyup',(function(event){if(!event.isDefaultPrevented()){this.search();}}).bind(this));}}, /**
     * Opens the dropdown.
     *
     * @param options Optional options object. May contain the following property:
     *                search - Boolean whether the dropdown should be initialized by performing a
     *                         search for the empty string (ie. display all results). Default is
     *                         true.
     *                showSearchInput - Boolean whether a search input should be shown in the
     *                                  dropdown. Default is false.
     */open:function open(options){options = options || {};if(!this.dropdown){if(this.triggerEvent('selectivity-opening')){var Dropdown=this.options.dropdown || Selectivity.Dropdown;if(Dropdown){this.dropdown = new Dropdown({items:this.items,position:this.options.positionDropdown,query:this.options.query,selectivity:this,showSearchInput:options.showSearchInput});}if(options.search !== false){this.search('');}}this.$el.children().toggleClass('open',true);}}, /**
     * (Re-)positions the dropdown.
     */positionDropdown:function positionDropdown(){if(this.dropdown){this.dropdown.position();}}, /**
     * Searches for results based on the term given or the term entered in the search input.
     *
     * If an items array has been passed with the options to the Selectivity instance, a local
     * search will be performed among those items. Otherwise, the query function specified in the
     * options will be used to perform the search. If neither is defined, nothing happens.
     *
     * @param term Optional term to search for. If ommitted, the value of the search input element
     *             is used as term.
     */search:function search(term){if(term === undefined){term = this.$searchInput?this.$searchInput.val():'';}this.open({search:false});if(this.dropdown){this.dropdown.search(term);}}, /**
     * Sets one or more options on this Selectivity instance.
     *
     * @param options Options object. May contain one or more of the following properties:
     *                closeOnSelect - Set to false to keep the dropdown open after the user has
     *                                selected an item. This is useful if you want to allow the user
     *                                to quickly select multiple items. The default value is true.
     *                dropdown - Custom dropdown implementation to use for this instance.
     *                initSelection - Function to map values by ID to selection data. This function
     *                                receives two arguments, 'value' and 'callback'. The value is
     *                                the current value of the selection, which is an ID or an array
     *                                of IDs depending on the input type. The callback should be
     *                                invoked with an object or array of objects, respectively,
     *                                containing 'id' and 'text' properties.
     *                items - Array of items from which to select. Should be an array of objects
     *                        with 'id' and 'text' properties. As convenience, you may also pass an
     *                        array of strings, in which case the same string is used for both the
     *                        'id' and 'text' properties. If items are given, all items are expected
     *                        to be available locally and all selection operations operate on this
     *                        local array only. If null, items are not available locally, and a
     *                        query function should be provided to fetch remote data.
     *                matcher - Function to determine whether text matches a given search term. Note
     *                          this function is only used if you have specified an array of items.
     *                          Receives two arguments:
     *                          item - The item that should match the search term.
     *                          term - The search term. Note that for performance reasons, the term
     *                                 has always been already processed using
     *                                 Selectivity.transformText().
     *                          The method should return the item if it matches, and null otherwise.
     *                          If the item has a children array, the matcher is expected to filter
     *                          those itself (be sure to only return the filtered array of children
     *                          in the returned item and not to modify the children of the item
     *                          argument).
     *                placeholder - Placeholder text to display when the element has no focus and
     *                              no selected items.
     *                positionDropdown - Function to position the dropdown. Receives two arguments:
     *                                   $dropdownEl - The element to be positioned.
     *                                   $selectEl - The element of the Selectivity instance, that
     *                                               you can position the dropdown to.
     *                                   The default implementation positions the dropdown element
     *                                   under the Selectivity's element and gives it the same
     *                                   width.
     *                query - Function to use for querying items. Receives a single object as
     *                        argument with the following properties:
     *                        callback - Callback to invoke when the results are available. This
     *                                   callback should be passed a single object as argument with
     *                                   the following properties:
     *                                   more - Boolean that can be set to true to indicate there
     *                                          are more results available. Additional results may
     *                                          be fetched by the user through pagination.
     *                                   results - Array of result items. The format for the result
     *                                             items is the same as for passing local items.
     *                        offset - This property is only used for pagination and indicates how
     *                                 many results should be skipped when returning more results.
     *                        selectivity - The Selectivity instance the query function is used on.
     *                        term - The search term the user is searching for. Unlike with the
     *                               matcher function, the term has not been processed using
     *                               Selectivity.transformText().
     *                readOnly - If true, disables any modification of the input.
     *                removeOnly - If true, disables any modification of the input except removing
     *                             of selected items.
     *                searchInputListeners - Array of search input listeners. By default, the global
     *                                       array Selectivity.SearchInputListeners is used.
     *                showDropdown - Set to false if you don't want to use any dropdown (you can
     *                               still open it programmatically using open()).
     *                templates - Object with instance-specific templates to override the global
     *                            templates assigned to Selectivity.Templates.
     */setOptions:function setOptions(options){options = options || {};Selectivity.OptionListeners.forEach((function(listener){listener(this,options);}).bind(this));$.extend(this.options,options);var allowedTypes=$.extend({closeOnSelect:'boolean',dropdown:'function|null',initSelection:'function|null',matcher:'function|null',placeholder:'string',positionDropdown:'function|null',query:'function|null',readOnly:'boolean',removeOnly:'boolean',searchInputListeners:'array'},options.allowedTypes);$.each(options,(function(key,value){var type=allowedTypes[key];if(type && !type.split('|').some(function(type){return $.type(value) === type;})){throw new Error(key + ' must be of type ' + type);}switch(key){case 'items':this.items = value === null?value:Selectivity.processItems(value);break;case 'matcher':this.matcher = value;break;case 'searchInputListeners':this.searchInputListeners = value;break;case 'templates':$.extend(this.templates,value);break;}}).bind(this));this.enabled = !this.options.readOnly && !this.options.removeOnly;}, /**
     * Returns the result of the given template.
     *
     * @param templateName Name of the template to process.
     * @param options Options to pass to the template.
     *
     * @return String containing HTML.
     */template:function template(templateName,options){var template=this.templates[templateName];if(template){if($.type(template) === 'function'){return template(options);}else if(template.render){return template.render(options);}else {return template.toString();}}else {throw new Error('Unknown template: ' + templateName);}}, /**
     * Triggers the change event.
     *
     * The event object at least contains the following property:
     * value - The new value of the Selectivity instance.
     *
     * @param Optional additional options added to the event object.
     */triggerChange:function triggerChange(options){this.triggerEvent('change',$.extend({value:this._value},options));}, /**
     * Triggers an event on the instance's element.
     *
     * @param Optional event data to be added to the event object.
     *
     * @return Whether the default action of the event may be executed, ie. returns false if
     *         preventDefault() has been called.
     */triggerEvent:function triggerEvent(eventName,data){var event=$.Event(eventName,data || {});this.$el.trigger(event);return !event.isDefaultPrevented();}, /**
     * Shorthand for value().
     */val:function val(newValue){return this.value(newValue);}, /**
     * Validates a single item. Throws an exception if the item is invalid.
     *
     * @param item The item to validate.
     *
     * @return The validated item. May differ from the input item.
     */validateItem:function validateItem(item){if(item && Selectivity.isValidId(item.id) && $.type(item.text) === 'string'){return item;}else {throw new Error('Item should have id (number or string) and text (string) properties');}}, /**
     * Sets or gets the value of the selection.
     *
     * The value of the selection only concerns the IDs of the selection items. If you are
     * interested in the IDs and the text labels, you should use the data() method.
     *
     * Note that if neither the items option nor the initSelection option have been set, Selectivity
     * will have no way to determine what text labels should be used with the given IDs in which
     * case it will assume the text is equal to the ID. This is useful if you're working with tags,
     * or selecting e-mail addresses for instance, but may not always be what you want.
     *
     * @param newValue Optional new value to set. For a MultipleSelectivity instance the value must
     *                 be an array of IDs, for a SingleSelectivity instance the value must be a
     *                 single ID (a string or a number) or null to indicate no item is selected.
     * @param options Optional options object. May contain the following property:
     *                triggerChange - Set to false to suppress the "change" event being triggered.
     *                                Note this will also cause the UI to not update automatically;
     *                                so you may want to call rerenderSelection() manually when
     *                                using this option.
     *
     * @return If newValue is omitted, this method returns the current value.
     */value:function value(newValue,options){options = options || {};if(newValue === undefined){return this._value;}else {newValue = this.validateValue(newValue);this._value = newValue;if(this.options.initSelection){this.options.initSelection(newValue,(function(data){if(this._value === newValue){this._data = this.validateData(data);if(options.triggerChange !== false){this.triggerChange();}}}).bind(this));}else {this._data = this.getDataForValue(newValue);if(options.triggerChange !== false){this.triggerChange();}}}}, /**
     * @private
     */_closed:function _closed(){this.dropdown = null;this.$el.children().toggleClass('open',false);}, /**
     * @private
     */_getItemId:function _getItemId(elementOrEvent){ // returns the item ID related to an element or event target.
// IDs can be either numbers or strings, but attribute values are always strings, so we
// will have to find out whether the item ID ought to be a number or string ourselves.
// $.fn.data() is a bit overzealous for our case, because it returns a number whenever the
// attribute value can be parsed as a number. however, it is possible an item had an ID
// which is a string but which is parseable as number, in which case we verify if the ID
// as number is actually found among the data or results. if it isn't, we assume it was
// supposed to be a string after all...
var $element;if(elementOrEvent.target){$element = $(elementOrEvent.target).closest('[data-item-id]');}else if(elementOrEvent.length){$element = elementOrEvent;}else {$element = $(elementOrEvent);}var id=$element.data('item-id');if($.type(id) === 'string'){return id;}else {if(Selectivity.findById(this._data || [],id)){return id;}else {var dropdown=this.dropdown;while(dropdown) {if(Selectivity.findNestedById(dropdown.results,id)){return id;} // FIXME: reference to submenu doesn't belong in base module
dropdown = dropdown.submenu;}return '' + id;}}}, /**
     * @private
     */_mouseout:function _mouseout(){this.$el.children().toggleClass('hover',false);}, /**
     * @private
     */_mouseover:function _mouseover(){this.$el.children().toggleClass('hover',true);}}); /**
 * Dropdown class to use for displaying dropdowns.
 *
 * The default implementation of a dropdown is defined in the selectivity-dropdown module.
 */Selectivity.Dropdown = null; /**
 * Mapping of input types.
 */Selectivity.InputTypes = {}; /**
 * Array of option listeners.
 *
 * Option listeners are invoked when setOptions() is called. Every listener receives two arguments:
 *
 * selectivity - The Selectivity instance.
 * options - The options that are about to be set. The listener may modify this options object.
 *
 * An example of an option listener is the selectivity-traditional module.
 */Selectivity.OptionListeners = []; /**
 * Array of search input listeners.
 *
 * Search input listeners are invoked when initSearchInput() is called (typically right after the
 * search input is created). Every listener receives two arguments:
 *
 * selectivity - The Selectivity instance.
 * $input - jQuery container with the search input.
 *
 * An example of a search input listener is the selectivity-keyboard module.
 */Selectivity.SearchInputListeners = []; /**
 * Mapping with templates to use for rendering select boxes and dropdowns. See
 * selectivity-templates.js for a useful set of default templates, as well as for documentation of
 * the individual templates.
 */Selectivity.Templates = {}; /**
 * Finds an item in the given array with the specified ID.
 *
 * @param array Array to search in.
 * @param id ID to search for.
 *
 * @return The item in the array with the given ID, or null if the item was not found.
 */Selectivity.findById = function(array,id){var index=Selectivity.findIndexById(array,id);return index > -1?array[index]:null;}; /**
 * Finds the index of an item in the given array with the specified ID.
 *
 * @param array Array to search in.
 * @param id ID to search for.
 *
 * @return The index of the item in the array with the given ID, or -1 if the item was not found.
 */Selectivity.findIndexById = function(array,id){for(var i=0,length=array.length;i < length;i++) {if(array[i].id === id){return i;}}return -1;}; /**
 * Finds an item in the given array with the specified ID. Items in the array may contain 'children'
 * properties which in turn will be searched for the item.
 *
 * @param array Array to search in.
 * @param id ID to search for.
 *
 * @return The item in the array with the given ID, or null if the item was not found.
 */Selectivity.findNestedById = null && function(array,id){for(var i=0,length=array.length;i < length;i++) {var item=array[i];if(item.id === id){return item;}else if(item.children){var result=Selectivity.findNestedById(item.children,id);if(result){return result;}}}return null;}; /**
 * Utility method for inheriting another class.
 *
 * @param SubClass Constructor function of the subclass.
 * @param SuperClass Optional constructor function of the superclass. If omitted, Selectivity is
 *                   used as superclass.
 * @param prototype Object with methods you want to add to the subclass prototype.
 *
 * @return A utility function for calling the methods of the superclass. This function receives two
 *         arguments: The this object on which you want to execute the method and the name of the
 *         method. Any arguments past those are passed to the superclass method.
 */Selectivity.inherits = function(SubClass,SuperClass,prototype){if(arguments.length === 2){prototype = SuperClass;SuperClass = Selectivity;}SubClass.prototype = $.extend(Object.create(SuperClass.prototype),{constructor:SubClass},prototype);return function(self,methodName){SuperClass.prototype[methodName].apply(self,Array.prototype.slice.call(arguments,2));};}; /**
 * Checks whether a value can be used as a valid ID for selection items. Only numbers and strings
 * are accepted to be used as IDs.
 *
 * @param id The value to check whether it is a valid ID.
 *
 * @return true if the value is a valid ID, false otherwise.
 */Selectivity.isValidId = function(id){var type=$.type(id);return type === 'number' || type === 'string';}; /**
 * Decides whether a given item matches a search term. The default implementation simply
 * checks whether the term is contained within the item's text, after transforming them using
 * transformText().
 *
 * @param item The item that should match the search term.
 * @param term The search term. Note that for performance reasons, the term has always been already
 *             processed using transformText().
 *
 * @return true if the text matches the term, false otherwise.
 */Selectivity.matcher = function(item,term){var result=null;if(Selectivity.transformText(item.text).indexOf(term) > -1){result = item;}else if(item.children){var matchingChildren=item.children.map(function(child){return Selectivity.matcher(child,term);}).filter(function(child){return !!child;});if(matchingChildren.length){result = {id:item.id,text:item.text,children:matchingChildren};}}return result;}; /**
 * Helper function for processing items.
 *
 * @param item The item to process, either as object containing 'id' and 'text' properties or just
 *             as ID. The 'id' property of an item is optional if it has a 'children' property
 *             containing an array of items.
 *
 * @return Object containing 'id' and 'text' properties.
 */Selectivity.processItem = function(item){if(Selectivity.isValidId(item)){return {id:item,text:'' + item};}else if(item && (Selectivity.isValidId(item.id) || item.children) && $.type(item.text) === 'string'){if(item.children){item.children = Selectivity.processItems(item.children);}return item;}else {throw new Error('invalid item');}}; /**
 * Helper function for processing an array of items.
 *
 * @param items Array of items to process. See processItem() for details about a single item.
 *
 * @return Array with items.
 */Selectivity.processItems = function(items){if($.type(items) === 'array'){return items.map(Selectivity.processItem);}else {throw new Error('invalid items');}}; /**
 * Quotes a string so it can be used in a CSS attribute selector. It adds double quotes to the
 * string and escapes all occurrences of the quote character inside the string.
 *
 * @param string The string to quote.
 *
 * @return The quoted string.
 */Selectivity.quoteCssAttr = function(string){return '"' + ('' + string).replace(/\\/g,'\\\\').replace(/"/g,'\\"') + '"';}; /**
 * Transforms text in order to find matches. The default implementation casts all strings to
 * lower-case so that any matches found will be case-insensitive.
 *
 * @param string The string to transform.
 *
 * @return The transformed string.
 */Selectivity.transformText = function(string){return string.toLowerCase();};module.exports = $.fn.selectivity = Selectivity;},{"2":2,"jquery":"jquery"}],9:[function(_dereq_,module,exports){'use strict';var DIACRITICS={"Ⓐ":'A',"Ａ":'A',"À":'A',"Á":'A',"Â":'A',"Ầ":'A',"Ấ":'A',"Ẫ":'A',"Ẩ":'A',"Ã":'A',"Ā":'A',"Ă":'A',"Ằ":'A',"Ắ":'A',"Ẵ":'A',"Ẳ":'A',"Ȧ":'A',"Ǡ":'A',"Ä":'A',"Ǟ":'A',"Ả":'A',"Å":'A',"Ǻ":'A',"Ǎ":'A',"Ȁ":'A',"Ȃ":'A',"Ạ":'A',"Ậ":'A',"Ặ":'A',"Ḁ":'A',"Ą":'A',"Ⱥ":'A',"Ɐ":'A',"Ꜳ":'AA',"Æ":'AE',"Ǽ":'AE',"Ǣ":'AE',"Ꜵ":'AO',"Ꜷ":'AU',"Ꜹ":'AV',"Ꜻ":'AV',"Ꜽ":'AY',"Ⓑ":'B',"Ｂ":'B',"Ḃ":'B',"Ḅ":'B',"Ḇ":'B',"Ƀ":'B',"Ƃ":'B',"Ɓ":'B',"Ⓒ":'C',"Ｃ":'C',"Ć":'C',"Ĉ":'C',"Ċ":'C',"Č":'C',"Ç":'C',"Ḉ":'C',"Ƈ":'C',"Ȼ":'C',"Ꜿ":'C',"Ⓓ":'D',"Ｄ":'D',"Ḋ":'D',"Ď":'D',"Ḍ":'D',"Ḑ":'D',"Ḓ":'D',"Ḏ":'D',"Đ":'D',"Ƌ":'D',"Ɗ":'D',"Ɖ":'D',"Ꝺ":'D',"Ǳ":'DZ',"Ǆ":'DZ',"ǲ":'Dz',"ǅ":'Dz',"Ⓔ":'E',"Ｅ":'E',"È":'E',"É":'E',"Ê":'E',"Ề":'E',"Ế":'E',"Ễ":'E',"Ể":'E',"Ẽ":'E',"Ē":'E',"Ḕ":'E',"Ḗ":'E',"Ĕ":'E',"Ė":'E',"Ë":'E',"Ẻ":'E',"Ě":'E',"Ȅ":'E',"Ȇ":'E',"Ẹ":'E',"Ệ":'E',"Ȩ":'E',"Ḝ":'E',"Ę":'E',"Ḙ":'E',"Ḛ":'E',"Ɛ":'E',"Ǝ":'E',"Ⓕ":'F',"Ｆ":'F',"Ḟ":'F',"Ƒ":'F',"Ꝼ":'F',"Ⓖ":'G',"Ｇ":'G',"Ǵ":'G',"Ĝ":'G',"Ḡ":'G',"Ğ":'G',"Ġ":'G',"Ǧ":'G',"Ģ":'G',"Ǥ":'G',"Ɠ":'G',"Ꞡ":'G',"Ᵹ":'G',"Ꝿ":'G',"Ⓗ":'H',"Ｈ":'H',"Ĥ":'H',"Ḣ":'H',"Ḧ":'H',"Ȟ":'H',"Ḥ":'H',"Ḩ":'H',"Ḫ":'H',"Ħ":'H',"Ⱨ":'H',"Ⱶ":'H',"Ɥ":'H',"Ⓘ":'I',"Ｉ":'I',"Ì":'I',"Í":'I',"Î":'I',"Ĩ":'I',"Ī":'I',"Ĭ":'I',"İ":'I',"Ï":'I',"Ḯ":'I',"Ỉ":'I',"Ǐ":'I',"Ȉ":'I',"Ȋ":'I',"Ị":'I',"Į":'I',"Ḭ":'I',"Ɨ":'I',"Ⓙ":'J',"Ｊ":'J',"Ĵ":'J',"Ɉ":'J',"Ⓚ":'K',"Ｋ":'K',"Ḱ":'K',"Ǩ":'K',"Ḳ":'K',"Ķ":'K',"Ḵ":'K',"Ƙ":'K',"Ⱪ":'K',"Ꝁ":'K',"Ꝃ":'K',"Ꝅ":'K',"Ꞣ":'K',"Ⓛ":'L',"Ｌ":'L',"Ŀ":'L',"Ĺ":'L',"Ľ":'L',"Ḷ":'L',"Ḹ":'L',"Ļ":'L',"Ḽ":'L',"Ḻ":'L',"Ł":'L',"Ƚ":'L',"Ɫ":'L',"Ⱡ":'L',"Ꝉ":'L',"Ꝇ":'L',"Ꞁ":'L',"Ǉ":'LJ',"ǈ":'Lj',"Ⓜ":'M',"Ｍ":'M',"Ḿ":'M',"Ṁ":'M',"Ṃ":'M',"Ɱ":'M',"Ɯ":'M',"Ⓝ":'N',"Ｎ":'N',"Ǹ":'N',"Ń":'N',"Ñ":'N',"Ṅ":'N',"Ň":'N',"Ṇ":'N',"Ņ":'N',"Ṋ":'N',"Ṉ":'N',"Ƞ":'N',"Ɲ":'N',"Ꞑ":'N',"Ꞥ":'N',"Ǌ":'NJ',"ǋ":'Nj',"Ⓞ":'O',"Ｏ":'O',"Ò":'O',"Ó":'O',"Ô":'O',"Ồ":'O',"Ố":'O',"Ỗ":'O',"Ổ":'O',"Õ":'O',"Ṍ":'O',"Ȭ":'O',"Ṏ":'O',"Ō":'O',"Ṑ":'O',"Ṓ":'O',"Ŏ":'O',"Ȯ":'O',"Ȱ":'O',"Ö":'O',"Ȫ":'O',"Ỏ":'O',"Ő":'O',"Ǒ":'O',"Ȍ":'O',"Ȏ":'O',"Ơ":'O',"Ờ":'O',"Ớ":'O',"Ỡ":'O',"Ở":'O',"Ợ":'O',"Ọ":'O',"Ộ":'O',"Ǫ":'O',"Ǭ":'O',"Ø":'O',"Ǿ":'O',"Ɔ":'O',"Ɵ":'O',"Ꝋ":'O',"Ꝍ":'O',"Ƣ":'OI',"Ꝏ":'OO',"Ȣ":'OU',"Ⓟ":'P',"Ｐ":'P',"Ṕ":'P',"Ṗ":'P',"Ƥ":'P',"Ᵽ":'P',"Ꝑ":'P',"Ꝓ":'P',"Ꝕ":'P',"Ⓠ":'Q',"Ｑ":'Q',"Ꝗ":'Q',"Ꝙ":'Q',"Ɋ":'Q',"Ⓡ":'R',"Ｒ":'R',"Ŕ":'R',"Ṙ":'R',"Ř":'R',"Ȑ":'R',"Ȓ":'R',"Ṛ":'R',"Ṝ":'R',"Ŗ":'R',"Ṟ":'R',"Ɍ":'R',"Ɽ":'R',"Ꝛ":'R',"Ꞧ":'R',"Ꞃ":'R',"Ⓢ":'S',"Ｓ":'S',"ẞ":'S',"Ś":'S',"Ṥ":'S',"Ŝ":'S',"Ṡ":'S',"Š":'S',"Ṧ":'S',"Ṣ":'S',"Ṩ":'S',"Ș":'S',"Ş":'S',"Ȿ":'S',"Ꞩ":'S',"Ꞅ":'S',"Ⓣ":'T',"Ｔ":'T',"Ṫ":'T',"Ť":'T',"Ṭ":'T',"Ț":'T',"Ţ":'T',"Ṱ":'T',"Ṯ":'T',"Ŧ":'T',"Ƭ":'T',"Ʈ":'T',"Ⱦ":'T',"Ꞇ":'T',"Ꜩ":'TZ',"Ⓤ":'U',"Ｕ":'U',"Ù":'U',"Ú":'U',"Û":'U',"Ũ":'U',"Ṹ":'U',"Ū":'U',"Ṻ":'U',"Ŭ":'U',"Ü":'U',"Ǜ":'U',"Ǘ":'U',"Ǖ":'U',"Ǚ":'U',"Ủ":'U',"Ů":'U',"Ű":'U',"Ǔ":'U',"Ȕ":'U',"Ȗ":'U',"Ư":'U',"Ừ":'U',"Ứ":'U',"Ữ":'U',"Ử":'U',"Ự":'U',"Ụ":'U',"Ṳ":'U',"Ų":'U',"Ṷ":'U',"Ṵ":'U',"Ʉ":'U',"Ⓥ":'V',"Ｖ":'V',"Ṽ":'V',"Ṿ":'V',"Ʋ":'V',"Ꝟ":'V',"Ʌ":'V',"Ꝡ":'VY',"Ⓦ":'W',"Ｗ":'W',"Ẁ":'W',"Ẃ":'W',"Ŵ":'W',"Ẇ":'W',"Ẅ":'W',"Ẉ":'W',"Ⱳ":'W',"Ⓧ":'X',"Ｘ":'X',"Ẋ":'X',"Ẍ":'X',"Ⓨ":'Y',"Ｙ":'Y',"Ỳ":'Y',"Ý":'Y',"Ŷ":'Y',"Ỹ":'Y',"Ȳ":'Y',"Ẏ":'Y',"Ÿ":'Y',"Ỷ":'Y',"Ỵ":'Y',"Ƴ":'Y',"Ɏ":'Y',"Ỿ":'Y',"Ⓩ":'Z',"Ｚ":'Z',"Ź":'Z',"Ẑ":'Z',"Ż":'Z',"Ž":'Z',"Ẓ":'Z',"Ẕ":'Z',"Ƶ":'Z',"Ȥ":'Z',"Ɀ":'Z',"Ⱬ":'Z',"Ꝣ":'Z',"ⓐ":'a',"ａ":'a',"ẚ":'a',"à":'a',"á":'a',"â":'a',"ầ":'a',"ấ":'a',"ẫ":'a',"ẩ":'a',"ã":'a',"ā":'a',"ă":'a',"ằ":'a',"ắ":'a',"ẵ":'a',"ẳ":'a',"ȧ":'a',"ǡ":'a',"ä":'a',"ǟ":'a',"ả":'a',"å":'a',"ǻ":'a',"ǎ":'a',"ȁ":'a',"ȃ":'a',"ạ":'a',"ậ":'a',"ặ":'a',"ḁ":'a',"ą":'a',"ⱥ":'a',"ɐ":'a',"ꜳ":'aa',"æ":'ae',"ǽ":'ae',"ǣ":'ae',"ꜵ":'ao',"ꜷ":'au',"ꜹ":'av',"ꜻ":'av',"ꜽ":'ay',"ⓑ":'b',"ｂ":'b',"ḃ":'b',"ḅ":'b',"ḇ":'b',"ƀ":'b',"ƃ":'b',"ɓ":'b',"ⓒ":'c',"ｃ":'c',"ć":'c',"ĉ":'c',"ċ":'c',"č":'c',"ç":'c',"ḉ":'c',"ƈ":'c',"ȼ":'c',"ꜿ":'c',"ↄ":'c',"ⓓ":'d',"ｄ":'d',"ḋ":'d',"ď":'d',"ḍ":'d',"ḑ":'d',"ḓ":'d',"ḏ":'d',"đ":'d',"ƌ":'d',"ɖ":'d',"ɗ":'d',"ꝺ":'d',"ǳ":'dz',"ǆ":'dz',"ⓔ":'e',"ｅ":'e',"è":'e',"é":'e',"ê":'e',"ề":'e',"ế":'e',"ễ":'e',"ể":'e',"ẽ":'e',"ē":'e',"ḕ":'e',"ḗ":'e',"ĕ":'e',"ė":'e',"ë":'e',"ẻ":'e',"ě":'e',"ȅ":'e',"ȇ":'e',"ẹ":'e',"ệ":'e',"ȩ":'e',"ḝ":'e',"ę":'e',"ḙ":'e',"ḛ":'e',"ɇ":'e',"ɛ":'e',"ǝ":'e',"ⓕ":'f',"ｆ":'f',"ḟ":'f',"ƒ":'f',"ꝼ":'f',"ⓖ":'g',"ｇ":'g',"ǵ":'g',"ĝ":'g',"ḡ":'g',"ğ":'g',"ġ":'g',"ǧ":'g',"ģ":'g',"ǥ":'g',"ɠ":'g',"ꞡ":'g',"ᵹ":'g',"ꝿ":'g',"ⓗ":'h',"ｈ":'h',"ĥ":'h',"ḣ":'h',"ḧ":'h',"ȟ":'h',"ḥ":'h',"ḩ":'h',"ḫ":'h',"ẖ":'h',"ħ":'h',"ⱨ":'h',"ⱶ":'h',"ɥ":'h',"ƕ":'hv',"ⓘ":'i',"ｉ":'i',"ì":'i',"í":'i',"î":'i',"ĩ":'i',"ī":'i',"ĭ":'i',"ï":'i',"ḯ":'i',"ỉ":'i',"ǐ":'i',"ȉ":'i',"ȋ":'i',"ị":'i',"į":'i',"ḭ":'i',"ɨ":'i',"ı":'i',"ⓙ":'j',"ｊ":'j',"ĵ":'j',"ǰ":'j',"ɉ":'j',"ⓚ":'k',"ｋ":'k',"ḱ":'k',"ǩ":'k',"ḳ":'k',"ķ":'k',"ḵ":'k',"ƙ":'k',"ⱪ":'k',"ꝁ":'k',"ꝃ":'k',"ꝅ":'k',"ꞣ":'k',"ⓛ":'l',"ｌ":'l',"ŀ":'l',"ĺ":'l',"ľ":'l',"ḷ":'l',"ḹ":'l',"ļ":'l',"ḽ":'l',"ḻ":'l',"ſ":'l',"ł":'l',"ƚ":'l',"ɫ":'l',"ⱡ":'l',"ꝉ":'l',"ꞁ":'l',"ꝇ":'l',"ǉ":'lj',"ⓜ":'m',"ｍ":'m',"ḿ":'m',"ṁ":'m',"ṃ":'m',"ɱ":'m',"ɯ":'m',"ⓝ":'n',"ｎ":'n',"ǹ":'n',"ń":'n',"ñ":'n',"ṅ":'n',"ň":'n',"ṇ":'n',"ņ":'n',"ṋ":'n',"ṉ":'n',"ƞ":'n',"ɲ":'n',"ŉ":'n',"ꞑ":'n',"ꞥ":'n',"ǌ":'nj',"ⓞ":'o',"ｏ":'o',"ò":'o',"ó":'o',"ô":'o',"ồ":'o',"ố":'o',"ỗ":'o',"ổ":'o',"õ":'o',"ṍ":'o',"ȭ":'o',"ṏ":'o',"ō":'o',"ṑ":'o',"ṓ":'o',"ŏ":'o',"ȯ":'o',"ȱ":'o',"ö":'o',"ȫ":'o',"ỏ":'o',"ő":'o',"ǒ":'o',"ȍ":'o',"ȏ":'o',"ơ":'o',"ờ":'o',"ớ":'o',"ỡ":'o',"ở":'o',"ợ":'o',"ọ":'o',"ộ":'o',"ǫ":'o',"ǭ":'o',"ø":'o',"ǿ":'o',"ɔ":'o',"ꝋ":'o',"ꝍ":'o',"ɵ":'o',"ƣ":'oi',"ȣ":'ou',"ꝏ":'oo',"ⓟ":'p',"ｐ":'p',"ṕ":'p',"ṗ":'p',"ƥ":'p',"ᵽ":'p',"ꝑ":'p',"ꝓ":'p',"ꝕ":'p',"ⓠ":'q',"ｑ":'q',"ɋ":'q',"ꝗ":'q',"ꝙ":'q',"ⓡ":'r',"ｒ":'r',"ŕ":'r',"ṙ":'r',"ř":'r',"ȑ":'r',"ȓ":'r',"ṛ":'r',"ṝ":'r',"ŗ":'r',"ṟ":'r',"ɍ":'r',"ɽ":'r',"ꝛ":'r',"ꞧ":'r',"ꞃ":'r',"ⓢ":'s',"ｓ":'s',"ß":'s',"ś":'s',"ṥ":'s',"ŝ":'s',"ṡ":'s',"š":'s',"ṧ":'s',"ṣ":'s',"ṩ":'s',"ș":'s',"ş":'s',"ȿ":'s',"ꞩ":'s',"ꞅ":'s',"ẛ":'s',"ⓣ":'t',"ｔ":'t',"ṫ":'t',"ẗ":'t',"ť":'t',"ṭ":'t',"ț":'t',"ţ":'t',"ṱ":'t',"ṯ":'t',"ŧ":'t',"ƭ":'t',"ʈ":'t',"ⱦ":'t',"ꞇ":'t',"ꜩ":'tz',"ⓤ":'u',"ｕ":'u',"ù":'u',"ú":'u',"û":'u',"ũ":'u',"ṹ":'u',"ū":'u',"ṻ":'u',"ŭ":'u',"ü":'u',"ǜ":'u',"ǘ":'u',"ǖ":'u',"ǚ":'u',"ủ":'u',"ů":'u',"ű":'u',"ǔ":'u',"ȕ":'u',"ȗ":'u',"ư":'u',"ừ":'u',"ứ":'u',"ữ":'u',"ử":'u',"ự":'u',"ụ":'u',"ṳ":'u',"ų":'u',"ṷ":'u',"ṵ":'u',"ʉ":'u',"ⓥ":'v',"ｖ":'v',"ṽ":'v',"ṿ":'v',"ʋ":'v',"ꝟ":'v',"ʌ":'v',"ꝡ":'vy',"ⓦ":'w',"ｗ":'w',"ẁ":'w',"ẃ":'w',"ŵ":'w',"ẇ":'w',"ẅ":'w',"ẘ":'w',"ẉ":'w',"ⱳ":'w',"ⓧ":'x',"ｘ":'x',"ẋ":'x',"ẍ":'x',"ⓨ":'y',"ｙ":'y',"ỳ":'y',"ý":'y',"ŷ":'y',"ỹ":'y',"ȳ":'y',"ẏ":'y',"ÿ":'y',"ỷ":'y',"ẙ":'y',"ỵ":'y',"ƴ":'y',"ɏ":'y',"ỿ":'y',"ⓩ":'z',"ｚ":'z',"ź":'z',"ẑ":'z',"ż":'z',"ž":'z',"ẓ":'z',"ẕ":'z',"ƶ":'z',"ȥ":'z',"ɀ":'z',"ⱬ":'z',"ꝣ":'z',"Ά":"Α","Έ":"Ε","Ή":"Η","Ί":"Ι","Ϊ":"Ι","Ό":"Ο","Ύ":"Υ","Ϋ":"Υ","Ώ":"Ω","ά":"α","έ":"ε","ή":"η","ί":"ι","ϊ":"ι","ΐ":"ι","ό":"ο","ύ":"υ","ϋ":"υ","ΰ":"υ","ω":"ω","ς":"σ"};var Selectivity=_dereq_(8);var previousTransform=Selectivity.transformText; /**
 * Extended version of the transformText() function that simplifies diacritics to their latin1
 * counterparts.
 *
 * Note that if all query functions fetch their results from a remote server, you may not need this
 * function, because it makes sense to remove diacritics server-side in such cases.
 */Selectivity.transformText = function(string){var result='';for(var i=0,length=string.length;i < length;i++) {var character=string[i];result += DIACRITICS[character] || character;}return previousTransform(result);};},{"8":8}],10:[function(_dereq_,module,exports){'use strict';var $=window.jQuery || window.Zepto;var debounce=_dereq_(3);var EventDelegator=_dereq_(2);var Selectivity=_dereq_(8); /**
 * selectivity Dropdown Constructor.
 *
 * @param options Options object. Should have the following properties:
 *                selectivity - Selectivity instance to show the dropdown for.
 *                showSearchInput - Boolean whether a search input should be shown.
 */function SelectivityDropdown(options){var selectivity=options.selectivity;this.$el = $(selectivity.template('dropdown',{dropdownCssClass:selectivity.options.dropdownCssClass,searchInputPlaceholder:selectivity.options.searchInputPlaceholder,showSearchInput:options.showSearchInput})); /**
     * jQuery container to add the results to.
     */this.$results = this.$('.selectivity-results-container'); /**
     * Boolean indicating whether more results are available than currently displayed in the
     * dropdown.
     */this.hasMore = false; /**
     * The currently highlighted result item.
     */this.highlightedResult = null; /**
     * Boolean whether the load more link is currently highlighted.
     */this.loadMoreHighlighted = false; /**
     * Options passed to the dropdown constructor.
     */this.options = options; /**
     * The results displayed in the dropdown.
     */this.results = []; /**
     * Selectivity instance.
     */this.selectivity = selectivity;this._closed = false;this._closeProxy = this.close.bind(this);if(selectivity.options.closeOnSelect !== false){selectivity.$el.on('selectivity-selecting',this._closeProxy);}this._lastMousePosition = {};this.addToDom();this.position();this.setupCloseHandler();this._suppressMouseWheel();if(options.showSearchInput){selectivity.initSearchInput(this.$('.selectivity-search-input'));selectivity.focus();}EventDelegator.call(this);this.$results.on('scroll touchmove touchend',debounce(this._scrolled.bind(this),50));this.showLoading();setTimeout(this.triggerOpen.bind(this),1);} /**
 * Methods.
 */$.extend(SelectivityDropdown.prototype,EventDelegator.prototype,{ /**
     * Convenience shortcut for this.$el.find(selector).
     */$:function $(selector){return this.$el.find(selector);}, /**
     * Adds the dropdown to the DOM.
     */addToDom:function addToDom(){var $next;var $anchor=this.selectivity.$el;while(($next = $anchor.next('.selectivity-dropdown')).length) {$anchor = $next;} //this.$el.insertAfter($anchor);
$anchor.append(this.$el);}, /**
     * Closes the dropdown.
     */close:function close(){if(!this._closed){this._closed = true;this.$el.remove();this.removeCloseHandler();this.selectivity.$el.off('selectivity-selecting',this._closeProxy);this.triggerClose();}}, /**
     * Events map.
     *
     * Follows the same format as Backbone: http://backbonejs.org/#View-delegateEvents
     */events:{'click .selectivity-load-more':'_loadMoreClicked','click .selectivity-result-item':'_resultClicked','mouseenter .selectivity-load-more':'_loadMoreHovered','mouseenter .selectivity-result-item':'_resultHovered'}, /**
     * Highlights a result item.
     *
     * @param item The item to highlight.
     */highlight:function highlight(item){if(this.loadMoreHighlighted){this.$('.selectivity-load-more').removeClass('highlight');}this.$('.selectivity-result-item').removeClass('highlight').filter('[data-item-id=' + Selectivity.quoteCssAttr(item.id) + ']').addClass('highlight');this.highlightedResult = item;this.loadMoreHighlighted = false;this.selectivity.triggerEvent('selectivity-highlight',{item:item,id:item.id});}, /**
     * Highlights the load more link.
     *
     * @param item The item to highlight.
     */highlightLoadMore:function highlightLoadMore(){this.$('.selectivity-result-item').removeClass('highlight');this.$('.selectivity-load-more').addClass('highlight');this.highlightedResult = null;this.loadMoreHighlighted = true;}, /**
     * Loads a follow-up page with results after a search.
     *
     * This method should only be called after a call to search() when the callback has indicated
     * more results are available.
     */loadMore:function loadMore(){this.options.query({callback:(function(response){if(response && response.results){this._showResults(Selectivity.processItems(response.results),{add:true,hasMore:!!response.more});}else {throw new Error('callback must be passed a response object');}}).bind(this),error:this._showResults.bind(this,[],{add:true}),offset:this.results.length,selectivity:this.selectivity,term:this.term});}, /**
     * Positions the dropdown inside the DOM.
     */position:function position(){var position=this.options.position;if(position){position(this.$el,this.selectivity.$el);}this._scrolled();}, /**
     * Removes the event handler to close the dropdown.
     */removeCloseHandler:function removeCloseHandler(){$('body').off('click',this._closeProxy);}, /**
     * Renders an array of result items.
     *
     * @param items Array of result items.
     *
     * @return HTML-formatted string to display the result items.
     */renderItems:function renderItems(items){var selectivity=this.selectivity;return items.map(function(item){var result=selectivity.template(item.id?'resultItem':'resultLabel',item);if(item.children){result += selectivity.template('resultChildren',{childrenHtml:this.renderItems(item.children)});}return result;},this).join('');}, /**
     * Searches for results based on the term given or the term entered in the search input.
     *
     * If an items array has been passed with the options to the Selectivity instance, a local
     * search will be performed among those items. Otherwise, the query function specified in the
     * options will be used to perform the search. If neither is defined, nothing happens.
     *
     * @param term Term to search for.
     */search:function search(term){var self=this;term = term || '';self.term = term;if(self.options.items){term = Selectivity.transformText(term);var matcher=self.selectivity.matcher;self._showResults(self.options.items.map(function(item){return matcher(item,term);}).filter(function(item){return !!item;}),{term:term});}else if(self.options.query){self.options.query({callback:function callback(response){if(response && response.results){self._showResults(Selectivity.processItems(response.results),{hasMore:!!response.more,term:term});}else {throw new Error('callback must be passed a response object');}},error:self.showError.bind(self),offset:0,selectivity:self.selectivity,term:term});}}, /**
     * Selects the highlighted item.
     */selectHighlight:function selectHighlight(){if(this.highlightedResult){this.selectItem(this.highlightedResult.id);}else if(this.loadMoreHighlighted){this._loadMoreClicked();}}, /**
     * Selects the item with the given ID.
     *
     * @param id ID of the item to select.
     */selectItem:function selectItem(id){var item=Selectivity.findNestedById(this.results,id);if(item && !item.disabled){var options={id:id,item:item};if(this.selectivity.triggerEvent('selectivity-selecting',options)){this.selectivity.triggerEvent('selectivity-selected',options);}}}, /**
     * Sets up an event handler that will close the dropdown when the Selectivity control loses
     * focus.
     */setupCloseHandler:function setupCloseHandler(){$('body').on('click',this._closeProxy);}, /**
     * Shows an error message.
     *
     * @param message Error message to display.
     * @param options Options object. May contain the following property:
     *                escape - Set to false to disable HTML-escaping of the message. Useful if you
     *                         want to set raw HTML as the message, but may open you up to XSS
     *                         attacks if you're not careful with escaping user input.
     */showError:function showError(message,options){options = options || {};this.$results.html(this.selectivity.template('error',{escape:options.escape !== false,message:message}));this.hasMore = false;this.results = [];this.highlightedResult = null;this.loadMoreHighlighted = false;this.position();}, /**
     * Shows a loading indicator in the dropdown.
     */showLoading:function showLoading(){this.$results.html(this.selectivity.template('loading'));this.hasMore = false;this.results = [];this.highlightedResult = null;this.loadMoreHighlighted = false;this.position();}, /**
     * Shows the results from a search query.
     *
     * @param results Array of result items.
     * @param options Options object. May contain the following properties:
     *                add - True if the results should be added to any already shown results.
     *                hasMore - Boolean whether more results can be fetched using the query()
     *                          function.
     *                term - The search term for which the results are displayed.
     */showResults:function showResults(results,options){var resultsHtml=this.renderItems(results);if(options.hasMore){resultsHtml += this.selectivity.template('loadMore');}else {if(!resultsHtml && !options.add){resultsHtml = this.selectivity.template('noResults',{term:options.term});}}if(options.add){this.$('.selectivity-loading').replaceWith(resultsHtml);this.results = this.results.concat(results);}else {this.$results.html(resultsHtml);this.results = results;}this.hasMore = options.hasMore;if(!options.add || this.loadMoreHighlighted){this._highlightFirstItem(results);}this.position();}, /**
     * Triggers the 'selectivity-close' event.
     */triggerClose:function triggerClose(){this.selectivity.$el.trigger('selectivity-close');}, /**
     * Triggers the 'selectivity-open' event.
     */triggerOpen:function triggerOpen(){this.selectivity.$el.trigger('selectivity-open');}, /**
     * @private
     */_highlightFirstItem:function _highlightFirstItem(results){function findFirstItem(results){for(var i=0,length=results.length;i < length;i++) {var result=results[i];if(result.id){return result;}else if(result.children){var item=findFirstItem(result.children);if(item){return item;}}}}var firstItem=findFirstItem(results);if(firstItem){this.highlight(firstItem);}else {this.highlightedResult = null;this.loadMoreHighlighted = false;}}, /**
     * @private
     */_loadMoreClicked:function _loadMoreClicked(){this.$('.selectivity-load-more').replaceWith(this.selectivity.template('loading'));this.loadMore();this.selectivity.focus();return false;}, /**
     * @private
     */_loadMoreHovered:function _loadMoreHovered(event){if(event.screenX === undefined || event.screenX !== this._lastMousePosition.x || event.screenY === undefined || event.screenY !== this._lastMousePosition.y){this.highlightLoadMore();this._recordMousePosition(event);}}, /**
     * @private
     */_recordMousePosition:function _recordMousePosition(event){this._lastMousePosition = {x:event.screenX,y:event.screenY};}, /**
     * @private
     */_resultClicked:function _resultClicked(event){this.selectItem(this.selectivity._getItemId(event));return false;}, /**
     * @private
     */_resultHovered:function _resultHovered(event){if(event.screenX === undefined || event.screenX !== this._lastMousePosition.x || event.screenY === undefined || event.screenY !== this._lastMousePosition.y){var id=this.selectivity._getItemId(event);var item=Selectivity.findNestedById(this.results,id);if(item && !item.disabled){this.highlight(item);}this._recordMousePosition(event);}}, /**
     * @private
     */_scrolled:function _scrolled(){var $loadMore=this.$('.selectivity-load-more');if($loadMore.length){if($loadMore[0].offsetTop - this.$results[0].scrollTop < this.$el.height()){this._loadMoreClicked();}}}, /**
     * @private
     */_showResults:function _showResults(results,options){this.showResults(this.selectivity.filterResults(results),options);}, /**
     * @private
     */_suppressMouseWheel:function _suppressMouseWheel(){var suppressMouseWheelSelector=this.selectivity.options.suppressMouseWheelSelector;if(suppressMouseWheelSelector === null){return;}var selector=suppressMouseWheelSelector || '.selectivity-results-container';this.$el.on('DOMMouseScroll mousewheel',selector,function(event){ // Thanks to Troy Alford:
// http://stackoverflow.com/questions/5802467/prevent-scrolling-of-parent-element
var $el=$(this),scrollTop=this.scrollTop,scrollHeight=this.scrollHeight,height=$el.height(),originalEvent=event.originalEvent,delta=event.type === 'DOMMouseScroll'?originalEvent.detail * -40:originalEvent.wheelDelta,up=delta > 0;function prevent(){event.stopPropagation();event.preventDefault();event.returnValue = false;return false;}if(scrollHeight > height){if(!up && -delta > scrollHeight - height - scrollTop){ // Scrolling down, but this will take us past the bottom.
$el.scrollTop(scrollHeight);return prevent();}else if(up && delta > scrollTop){ // Scrolling up, but this will take us past the top.
$el.scrollTop(0);return prevent();}}});}});module.exports = Selectivity.Dropdown = SelectivityDropdown;},{"2":2,"3":3,"8":8,"jquery":"jquery"}],11:[function(_dereq_,module,exports){'use strict';var $=window.jQuery || window.Zepto;var Selectivity=_dereq_(8);var MultipleSelectivity=_dereq_(14);function isValidEmail(email){var atIndex=email.indexOf('@');var dotIndex=email.lastIndexOf('.');var spaceIndex=email.indexOf(' ');return atIndex > 0 && dotIndex > atIndex + 1 && dotIndex < email.length - 2 && spaceIndex === -1;}function lastWord(token,length){length = length === undefined?token.length:length;for(var i=length - 1;i >= 0;i--) {if(/\s/.test(token[i])){return token.slice(i + 1,length);}}return token.slice(0,length);}function stripEnclosure(token,enclosure){if(token.slice(0,1) === enclosure[0] && token.slice(-1) === enclosure[1]){return token.slice(1,-1).trim();}else {return token.trim();}}function createEmailItem(token){var email=lastWord(token);var name=token.slice(0,-email.length).trim();if(isValidEmail(email)){email = stripEnclosure(stripEnclosure(email,'()'),'<>');name = stripEnclosure(name,'""').trim() || email;return {id:email,text:name};}else {return token.trim()?{id:token,text:token}:null;}}function emailTokenizer(input,selection,createToken){function hasToken(input){if(input){for(var i=0,length=input.length;i < length;i++) {switch(input[i]){case ';':case ',':case '\n':return true;case ' ':case '\t':if(isValidEmail(lastWord(input,i))){return true;}break;case '"':do {i++;}while(i < length && input[i] !== '"');break;default:continue;}}}return false;}function takeToken(input){for(var i=0,length=input.length;i < length;i++) {switch(input[i]){case ';':case ',':case '\n':return {term:input.slice(0,i),input:input.slice(i + 1)};case ' ':case '\t':if(isValidEmail(lastWord(input,i))){return {term:input.slice(0,i),input:input.slice(i + 1)};}break;case '"':do {i++;}while(i < length && input[i] !== '"');break;default:continue;}}return {};}while(hasToken(input)) {var token=takeToken(input);if(token.term){var item=createEmailItem(token.term);if(item && !(item.id && Selectivity.findById(selection,item.id))){createToken(item);}}input = token.input;}return input;} /**
 * Emailselectivity Constructor.
 *
 * @param options Options object. Accepts all options from the MultipleSelectivity Constructor.
 */function Emailselectivity(options){MultipleSelectivity.call(this,options);} /**
 * Methods.
 */var callSuper=Selectivity.inherits(Emailselectivity,MultipleSelectivity,{ /**
     * @inherit
     */initSearchInput:function initSearchInput($input){callSuper(this,'initSearchInput',$input);$input.on('blur',(function(){var term=$input.val();if(isValidEmail(lastWord(term))){this.add(createEmailItem(term));}}).bind(this));}, /**
     * @inherit
     *
     * Note that for the Email input type the option showDropdown is set to false and the tokenizer
     * option is set to a tokenizer specialized for email addresses.
     */setOptions:function setOptions(options){options = $.extend({createTokenItem:createEmailItem,showDropdown:false,tokenizer:emailTokenizer},options);callSuper(this,'setOptions',options);}});module.exports = Selectivity.InputTypes.Email = Emailselectivity;},{"14":14,"8":8,"jquery":"jquery"}],12:[function(_dereq_,module,exports){'use strict';var Selectivity=_dereq_(8);var KEY_BACKSPACE=8;var KEY_DOWN_ARROW=40;var KEY_ENTER=13;var KEY_ESCAPE=27;var KEY_TAB=9;var KEY_UP_ARROW=38; /**
 * Search input listener providing keyboard support for navigating the dropdown.
 */function listener(selectivity,$input){var keydownCanceled=false;var closeSubmenu=null; /**
     * Moves a dropdown's highlight to the next or previous result item.
     *
     * @param delta Either 1 to move to the next item, or -1 to move to the previous item.
     */function moveHighlight(dropdown,delta){function findElementIndex($elements,selector){for(var i=0,length=$elements.length;i < length;i++) {if($elements.eq(i).is(selector)){return i;}}return -1;}function scrollToHighlight(){var $el;if(dropdown.highlightedResult){var quotedId=Selectivity.quoteCssAttr(dropdown.highlightedResult.id);$el = dropdown.$('.selectivity-result-item[data-item-id=' + quotedId + ']');}else if(dropdown.loadMoreHighlighted){$el = dropdown.$('.selectivity-load-more');}else {return; // no highlight to scroll to
}var position=$el.position();if(!position){return;}var top=position.top;var resultsHeight=dropdown.$results.height();var elHeight=$el.outerHeight?$el.outerHeight():$el.height();if(top < 0 || top > resultsHeight - elHeight){top += dropdown.$results.scrollTop();dropdown.$results.scrollTop(delta < 0?top:top - resultsHeight + elHeight);}}if(dropdown.submenu){moveHighlight(dropdown.submenu,delta);return;}var results=dropdown.results;if(results.length){var $results=dropdown.$('.selectivity-result-item');var defaultIndex=delta > 0?0:$results.length - 1;var index=defaultIndex;var highlightedResult=dropdown.highlightedResult;if(highlightedResult){var quotedId=Selectivity.quoteCssAttr(highlightedResult.id);index = findElementIndex($results,'[data-item-id=' + quotedId + ']') + delta;if(delta > 0?index >= $results.length:index < 0){if(dropdown.hasMore){dropdown.highlightLoadMore();scrollToHighlight();return;}else {index = defaultIndex;}}}var result=Selectivity.findNestedById(results,selectivity._getItemId($results[index]));if(result){dropdown.highlight(result,{delay:!!result.submenu});scrollToHighlight();}}}function keyHeld(event){var dropdown=selectivity.dropdown;if(dropdown){if(event.keyCode === KEY_BACKSPACE){if(!$input.val()){if(dropdown.submenu){var submenu=dropdown.submenu;while(submenu.submenu) {submenu = submenu.submenu;}closeSubmenu = submenu;}event.preventDefault();keydownCanceled = true;}}else if(event.keyCode === KEY_DOWN_ARROW){moveHighlight(dropdown,1);}else if(event.keyCode === KEY_UP_ARROW){moveHighlight(dropdown,-1);}else if(event.keyCode === KEY_TAB){setTimeout(function(){selectivity.close({keepFocus:false});},1);}else if(event.keyCode === KEY_ENTER){event.preventDefault(); // don't submit forms on keydown
}}}function keyReleased(event){function open(){if(selectivity.options.showDropdown !== false){selectivity.open();}}var dropdown=selectivity.dropdown;if(keydownCanceled){event.preventDefault();keydownCanceled = false;if(closeSubmenu){closeSubmenu.close();selectivity.focus();closeSubmenu = null;}}else if(event.keyCode === KEY_BACKSPACE){if(!dropdown && selectivity.options.allowClear){selectivity.clear();}}else if(event.keyCode === KEY_ENTER && !event.ctrlKey){if(dropdown){dropdown.selectHighlight();}else if(selectivity.options.showDropdown !== false){open();}event.preventDefault();}else if(event.keyCode === KEY_ESCAPE){selectivity.close();event.preventDefault();}else if(event.keyCode === KEY_DOWN_ARROW || event.keyCode === KEY_UP_ARROW){ // handled in keyHeld() because the response feels faster and it works with repeated
// events if the user holds the key for a longer period
// still, we issue an open() call here in case the dropdown was not yet open...
open();event.preventDefault();}else {open();}}$input.on('keydown',keyHeld).on('keyup',keyReleased);}Selectivity.SearchInputListeners.push(listener);},{"8":8}],13:[function(_dereq_,module,exports){'use strict';var escape=_dereq_(4);var Selectivity=_dereq_(8); /**
 * Localizable elements of the Selectivity Templates.
 *
 * Be aware that these strings are added straight to the HTML output of the templates, so any
 * non-safe strings should be escaped.
 */Selectivity.Locale = {ajaxError:function ajaxError(term){return 'Failed to fetch results for <b>' + escape(term) + '</b>';},loading:'Loading...',loadMore:'Load more...',needMoreCharacters:function needMoreCharacters(numCharacters){return 'Enter ' + numCharacters + ' more characters to search';},noResults:'No results found',noResultsForTerm:function noResultsForTerm(term){return 'No results for <b>' + escape(term) + '</b>';}};},{"4":4,"8":8}],14:[function(_dereq_,module,exports){'use strict';var $=window.jQuery || window.Zepto;var Selectivity=_dereq_(8);var KEY_BACKSPACE=8;var KEY_DELETE=46;var KEY_ENTER=13; /**
 * MultipleSelectivity Constructor.
 *
 * @param options Options object. Accepts all options from the Selectivity Base Constructor in
 *                addition to those accepted by MultipleSelectivity.setOptions().
 */function MultipleSelectivity(options){Selectivity.call(this,options);this.$el.html(this.template('multipleSelectInput',{enabled:this.enabled})).trigger('selectivity-init','multiple');this._highlightedItemId = null;this.initSearchInput(this.$('.selectivity-multiple-input:not(.selectivity-width-detector)'));this.rerenderSelection();if(!options.positionDropdown){ // dropdowns for multiple-value inputs should open below the select box,
// unless there is not enough space below, but there is space enough above, then it should
// open upwards
this.options.positionDropdown = function($el,$selectEl){var position=$selectEl.position(),dropdownHeight=$el.height(),selectHeight=$selectEl.height(),top=$selectEl[0].getBoundingClientRect().top,bottom=top + selectHeight + dropdownHeight,openUpwards=typeof window !== 'undefined' && bottom > $(window).height() && top - dropdownHeight > 0;var width=$selectEl.outerWidth?$selectEl.outerWidth():$selectEl.width();$el.css({left:position.left + 'px',top:position.top + (openUpwards?-dropdownHeight:selectHeight) + 'px'}).width(width);};}} /**
 * Methods.
 */var callSuper=Selectivity.inherits(MultipleSelectivity,{ /**
     * Adds an item to the selection, if it's not selected yet.
     *
     * @param item The item to add. May be an item with 'id' and 'text' properties or just an ID.
     */add:function add(item){var itemIsId=Selectivity.isValidId(item);var id=itemIsId?item:this.validateItem(item) && item.id;if(this._value.indexOf(id) === -1){this._value.push(id);if(itemIsId && this.options.initSelection){this.options.initSelection([id],(function(data){if(this._value.indexOf(id) > -1){item = this.validateItem(data[0]);this._data.push(item);this.triggerChange({added:item});}}).bind(this));}else {if(itemIsId){item = this.getItemForId(id);}this._data.push(item);this.triggerChange({added:item});}}this.$searchInput.val('');}, /**
     * Clears the data and value.
     */clear:function clear(){this.data([]);}, /**
     * Events map.
     *
     * Follows the same format as Backbone: http://backbonejs.org/#View-delegateEvents
     */events:{'change':'rerenderSelection','change .selectivity-multiple-input':function changeSelectivityMultipleInput(){return false;},'click':'_clicked','click .selectivity-multiple-selected-item':'_itemClicked','keydown .selectivity-multiple-input':'_keyHeld','keyup .selectivity-multiple-input':'_keyReleased','paste .selectivity-multiple-input':'_onPaste','selectivity-selected':'_resultSelected'}, /**
     * @inherit
     */filterResults:function filterResults(results){return results.filter(function(item){return !Selectivity.findById(this._data,item.id);},this);}, /**
     * Returns the correct data for a given value.
     *
     * @param value The value to get the data for. Should be an array of IDs.
     *
     * @return The corresponding data. Will be an array of objects with 'id' and 'text' properties.
     *         Note that if no items are defined, this method assumes the text labels will be equal
     *         to the IDs.
     */getDataForValue:function getDataForValue(value){return value.map(this.getItemForId,this).filter(function(item){return !!item;});}, /**
     * Returns the correct value for the given data.
     *
     * @param data The data to get the value for. Should be an array of objects with 'id' and 'text'
     *             properties.
     *
     * @return The corresponding value. Will be an array of IDs.
     */getValueForData:function getValueForData(data){return data.map(function(item){return item.id;});}, /**
     * Removes an item from the selection, if it is selected.
     *
     * @param item The item to remove. May be an item with 'id' and 'text' properties or just an ID.
     */remove:function remove(item){var id=$.type(item) === 'object'?item.id:item;var removedItem;var index=Selectivity.findIndexById(this._data,id);if(index > -1){removedItem = this._data[index];this._data.splice(index,1);}if(this._value[index] !== id){index = this._value.indexOf(id);}if(index > -1){this._value.splice(index,1);}if(removedItem){this.triggerChange({removed:removedItem});}if(id === this._highlightedItemId){this._highlightedItemId = null;}}, /**
     * Re-renders the selection.
     *
     * Normally the UI is automatically updated whenever the selection changes, but you may want to
     * call this method explicitly if you've updated the selection with the triggerChange option set
     * to false.
     */rerenderSelection:function rerenderSelection(event){event = event || {};if(event.added){this._renderSelectedItem(event.added);this._scrollToBottom();}else if(event.removed){var quotedId=Selectivity.quoteCssAttr(event.removed.id);this.$('.selectivity-multiple-selected-item[data-item-id=' + quotedId + ']').remove();}else {this.$('.selectivity-multiple-selected-item').remove();this._data.forEach(this._renderSelectedItem,this);this._updateInputWidth();}if(event.added || event.removed){if(this.dropdown){this.dropdown.showResults(this.filterResults(this.dropdown.results),{hasMore:this.dropdown.hasMore});}if(this.hasKeyboard){this.focus();}}this.positionDropdown();this._updatePlaceholder();}, /**
     * @inherit
     */search:function search(){var term=this.$searchInput.val();if(this.options.tokenizer){term = this.options.tokenizer(term,this._data,this.add.bind(this),this.options);if($.type(term) === 'string' && term !== this.$searchInput.val()){this.$searchInput.val(term);}}if(this.dropdown){callSuper(this,'search');}}, /**
     * @inherit
     *
     * @param options Options object. In addition to the options supported in the base
     *                implementation, this may contain the following properties:
     *                backspaceHighlightsBeforeDelete - If set to true, when the user enters a
     *                                                  backspace while there is no text in the
     *                                                  search field but there are selected items,
     *                                                  the last selected item will be highlighted
     *                                                  and when a second backspace is entered the
     *                                                  item is deleted. If false, the item gets
     *                                                  deleted on the first backspace. The default
     *                                                  value is true on devices that have touch
     *                                                  input and false on devices that don't.
     *                createTokenItem - Function to create a new item from a user's search term.
     *                                  This is used to turn the term into an item when dropdowns
     *                                  are disabled and the user presses Enter. It is also used by
     *                                  the default tokenizer to create items for individual tokens.
     *                                  The function receives a 'token' parameter which is the
     *                                  search term (or part of a search term) to create an item for
     *                                  and must return an item object with 'id' and 'text'
     *                                  properties or null if no token can be created from the term.
     *                                  The default is a function that returns an item where the id
     *                                  and text both match the token for any non-empty string and
     *                                  which returns null otherwise.
     *                tokenizer - Function for tokenizing search terms. Will receive the following
     *                            parameters:
     *                            input - The input string to tokenize.
     *                            selection - The current selection data.
     *                            createToken - Callback to create a token from the search terms.
     *                                          Should be passed an item object with 'id' and 'text'
     *                                          properties.
     *                            options - The options set on the Selectivity instance.
     *                            Any string returned by the tokenizer function is treated as the
     *                            remainder of untokenized input.
     */setOptions:function setOptions(options){options = options || {};var backspaceHighlightsBeforeDelete='backspaceHighlightsBeforeDelete';if(options[backspaceHighlightsBeforeDelete] === undefined){options[backspaceHighlightsBeforeDelete] = this.hasTouch;}options.allowedTypes = options.allowedTypes || {};options.allowedTypes[backspaceHighlightsBeforeDelete] = 'boolean';var wasEnabled=this.enabled;callSuper(this,'setOptions',options);if(wasEnabled !== this.enabled){this.$el.html(this.template('multipleSelectInput',{enabled:this.enabled}));}}, /**
     * Validates data to set. Throws an exception if the data is invalid.
     *
     * @param data The data to validate. Should be an array of objects with 'id' and 'text'
     *             properties.
     *
     * @return The validated data. This may differ from the input data.
     */validateData:function validateData(data){if(data === null){return [];}else if($.type(data) === 'array'){return data.map(this.validateItem,this);}else {throw new Error('Data for MultiSelectivity instance should be array');}}, /**
     * Validates a value to set. Throws an exception if the value is invalid.
     *
     * @param value The value to validate. Should be an array of IDs.
     *
     * @return The validated value. This may differ from the input value.
     */validateValue:function validateValue(value){if(value === null){return [];}else if($.type(value) === 'array'){if(value.every(Selectivity.isValidId)){return value;}else {throw new Error('Value contains invalid IDs');}}else {throw new Error('Value for MultiSelectivity instance should be an array');}}, /**
     * @private
     */_backspacePressed:function _backspacePressed(){if(this.options.backspaceHighlightsBeforeDelete){if(this._highlightedItemId){this._deletePressed();}else if(this._value.length){this._highlightItem(this._value.slice(-1)[0]);}}else if(this._value.length){this.remove(this._value.slice(-1)[0]);}}, /**
     * @private
     */_clicked:function _clicked(){if(this.enabled){this.focus();this._open();return false;}}, /**
     * @private
     */_createToken:function _createToken(){var term=this.$searchInput.val();var createTokenItem=this.options.createTokenItem;if(term && createTokenItem){var item=createTokenItem(term);if(item){this.add(item);}}}, /**
     * @private
     */_deletePressed:function _deletePressed(){if(this._highlightedItemId){this.remove(this._highlightedItemId);}}, /**
     * @private
     */_highlightItem:function _highlightItem(id){this._highlightedItemId = id;this.$('.selectivity-multiple-selected-item').removeClass('highlighted').filter('[data-item-id=' + Selectivity.quoteCssAttr(id) + ']').addClass('highlighted');if(this.hasKeyboard){this.focus();}}, /**
     * @private
     */_itemClicked:function _itemClicked(event){if(this.enabled){this._highlightItem(this._getItemId(event));}}, /**
     * @private
     */_itemRemoveClicked:function _itemRemoveClicked(event){this.remove(this._getItemId(event));this._updateInputWidth();return false;}, /**
     * @private
     */_keyHeld:function _keyHeld(event){this._originalValue = this.$searchInput.val();if(event.keyCode === KEY_ENTER && !event.ctrlKey){event.preventDefault();}}, /**
     * @private
     */_keyReleased:function _keyReleased(event){var inputHadText=!!this._originalValue;if(event.keyCode === KEY_ENTER && !event.ctrlKey){if(this.options.createTokenItem){this._createToken();}}else if(event.keyCode === KEY_BACKSPACE && !inputHadText){this._backspacePressed();}else if(event.keyCode === KEY_DELETE && !inputHadText){this._deletePressed();}this._updateInputWidth();}, /**
     * @private
     */_onPaste:function _onPaste(){setTimeout((function(){this.search();if(this.options.createTokenItem){this._createToken();}}).bind(this),10);}, /**
     * @private
     */_open:function _open(){if(this.options.showDropdown !== false){this.open();}},_renderSelectedItem:function _renderSelectedItem(item){this.$searchInput.before(this.template('multipleSelectedItem',$.extend({highlighted:item.id === this._highlightedItemId,removable:!this.options.readOnly},item)));var quotedId=Selectivity.quoteCssAttr(item.id);this.$('.selectivity-multiple-selected-item[data-item-id=' + quotedId + ']').find('.selectivity-multiple-selected-item-remove').on('click',this._itemRemoveClicked.bind(this));}, /**
     * @private
     */_resultSelected:function _resultSelected(event){if(this._value.indexOf(event.id) === -1){this.add(event.item);}else {this.remove(event.item);}}, /**
     * @private
     */_scrollToBottom:function _scrollToBottom(){var $inputContainer=this.$('.selectivity-multiple-input-container');$inputContainer.scrollTop($inputContainer.height());}, /**
     * @private
     */_updateInputWidth:function _updateInputWidth(){if(this.enabled){var $input=this.$searchInput,$widthDetector=this.$('.selectivity-width-detector');$widthDetector.text($input.val() || !this._data.length && this.options.placeholder || '');$input.width($widthDetector.width() + 20);this.positionDropdown();}}, /**
     * @private
     */_updatePlaceholder:function _updatePlaceholder(){var placeholder=this._data.length?'':this.options.placeholder;if(this.enabled){this.$searchInput.attr('placeholder',placeholder);}else {this.$('.selectivity-placeholder').text(placeholder);}}});module.exports = Selectivity.InputTypes.Multiple = MultipleSelectivity;},{"8":8,"jquery":"jquery"}],15:[function(_dereq_,module,exports){'use strict';var $=window.jQuery || window.Zepto;var Selectivity=_dereq_(8); /**
 * SingleSelectivity Constructor.
 *
 * @param options Options object. Accepts all options from the Selectivity Base Constructor in
 *                addition to those accepted by SingleSelectivity.setOptions().
 */function SingleSelectivity(options){Selectivity.call(this,options);this.$el.html(this.template('singleSelectInput',this.options)).trigger('selectivity-init','single');this.rerenderSelection();if(!options.positionDropdown){ // dropdowns for single-value inputs should open below the select box,
// unless there is not enough space below, in which case the dropdown should be moved up
// just enough so it fits in the window, but never so much that it reaches above the top
this.options.positionDropdown = function($el,$selectEl){var position=$selectEl.position(),dropdownHeight=$el.height(),selectHeight=$selectEl.height(),top=$selectEl[0].getBoundingClientRect().top,bottom=top + selectHeight + dropdownHeight,deltaUp=0;if(typeof window !== 'undefined'){deltaUp = Math.min(Math.max(bottom - $(window).height(),0),top + selectHeight);}var width=$selectEl.outerWidth?$selectEl.outerWidth():$selectEl.width();$el.css({left:position.left + 'px',top:position.top + selectHeight - deltaUp + 'px'}).width(width);};}if(options.showSearchInputInDropdown === false){this.initSearchInput(this.$('.selectivity-single-select-input'),{noSearch:true});}} /**
 * Methods.
 */var callSuper=Selectivity.inherits(SingleSelectivity,{ /**
     * Events map.
     *
     * Follows the same format as Backbone: http://backbonejs.org/#View-delegateEvents
     */events:{'change':'rerenderSelection','click':'_clicked','focus .selectivity-single-select-input':'_focused','selectivity-selected':'_resultSelected'}, /**
     * Clears the data and value.
     */clear:function clear(){this.data(null);}, /**
     * @inherit
     *
     * @param options Optional options object. May contain the following property:
     *                keepFocus - If false, the focus won't remain on the input.
     */close:function close(options){this._closing = true;callSuper(this,'close');if(!options || options.keepFocus !== false){this.$searchInput.focus();}this._closing = false;}, /**
     * Returns the correct data for a given value.
     *
     * @param value The value to get the data for. Should be an ID.
     *
     * @return The corresponding data. Will be an object with 'id' and 'text' properties. Note that
     *         if no items are defined, this method assumes the text label will be equal to the ID.
     */getDataForValue:function getDataForValue(value){return this.getItemForId(value);}, /**
     * Returns the correct value for the given data.
     *
     * @param data The data to get the value for. Should be an object with 'id' and 'text'
     *             properties or null.
     *
     * @return The corresponding value. Will be an ID or null.
     */getValueForData:function getValueForData(data){return data?data.id:null;}, /**
     * @inherit
     */open:function open(options){this._opening = true;var showSearchInput=this.options.showSearchInputInDropdown !== false;callSuper(this,'open',$.extend({showSearchInput:showSearchInput},options));if(!showSearchInput){this.focus();}this._opening = false;}, /**
     * Re-renders the selection.
     *
     * Normally the UI is automatically updated whenever the selection changes, but you may want to
     * call this method explicitly if you've updated the selection with the triggerChange option set
     * to false.
     */rerenderSelection:function rerenderSelection(){var $container=this.$('.selectivity-single-result-container');if(this._data){$container.html(this.template('singleSelectedItem',$.extend({removable:this.options.allowClear && !this.options.readOnly},this._data)));$container.find('.selectivity-single-selected-item-remove').on('click',this._itemRemoveClicked.bind(this));}else {$container.html(this.template('singleSelectPlaceholder',{placeholder:this.options.placeholder}));}}, /**
     * @inherit
     *
     * @param options Options object. In addition to the options supported in the base
     *                implementation, this may contain the following properties:
     *                allowClear - Boolean whether the selected item may be removed.
     *                showSearchInputInDropdown - Set to false to remove the search input used in
     *                                            dropdowns. The default is true.
     */setOptions:function setOptions(options){options = options || {};options.allowedTypes = $.extend(options.allowedTypes || {},{allowClear:'boolean',showSearchInputInDropdown:'boolean'});callSuper(this,'setOptions',options);}, /**
     * Validates data to set. Throws an exception if the data is invalid.
     *
     * @param data The data to validate. Should be an object with 'id' and 'text' properties or null
     *             to indicate no item is selected.
     *
     * @return The validated data. This may differ from the input data.
     */validateData:function validateData(data){return data === null?data:this.validateItem(data);}, /**
     * Validates a value to set. Throws an exception if the value is invalid.
     *
     * @param value The value to validate. Should be null or a valid ID.
     *
     * @return The validated value. This may differ from the input value.
     */validateValue:function validateValue(value){if(value === null || Selectivity.isValidId(value)){return value;}else {throw new Error('Value for SingleSelectivity instance should be a valid ID or null');}}, /**
     * @private
     */_clicked:function _clicked(){if(this.enabled){if(this.dropdown){this.close();}else if(this.options.showDropdown !== false){this.open();}return false;}}, /**
     * @private
     */_focused:function _focused(){if(this.enabled && !this._closing && !this._opening && this.options.showDropdown !== false){this.open();}}, /**
     * @private
     */_itemRemoveClicked:function _itemRemoveClicked(){this.data(null);return false;}, /**
     * @private
     */_resultSelected:function _resultSelected(event){this.data(event.item);this.close();}});module.exports = Selectivity.InputTypes.Single = SingleSelectivity;},{"8":8,"jquery":"jquery"}],16:[function(_dereq_,module,exports){'use strict';var Selectivity=_dereq_(8);var SelectivityDropdown=_dereq_(10); /**
 * Extended dropdown that supports submenus.
 */function SelectivitySubmenu(options){ /**
     * Optional parent dropdown menu from which this dropdown was opened.
     */this.parentMenu = options.parentMenu;SelectivityDropdown.call(this,options);this._closeSubmenuTimeout = 0;this._openSubmenuTimeout = 0;}var callSuper=Selectivity.inherits(SelectivitySubmenu,SelectivityDropdown,{ /**
     * @inherit
     */close:function close(){if(this.submenu){this.submenu.close();}callSuper(this,'close');if(this.parentMenu){this.parentMenu.submenu = null;this.parentMenu = null;}clearTimeout(this._closeSubmenuTimeout);clearTimeout(this._openSubmenuTimeout);}, /**
     * @inherit
     *
     * @param options Optional options object. May contain the following property:
     *                delay - If true, indicates any submenu should not be opened until after some
     *                        delay.
     */highlight:function highlight(item,options){if(options && options.delay){callSuper(this,'highlight',item);clearTimeout(this._openSubmenuTimeout);this._openSubmenuTimeout = setTimeout(this._doHighlight.bind(this,item),300);}else if(this.submenu){if(this.highlightedResult && this.highlightedResult.id === item.id){this._doHighlight(item);}else {clearTimeout(this._closeSubmenuTimeout);this._closeSubmenuTimeout = setTimeout(this._closeSubmenuAndHighlight.bind(this,item),100);}}else {if(this.parentMenu && this.parentMenu._closeSubmenuTimeout){clearTimeout(this.parentMenu._closeSubmenuTimeout);this.parentMenu._closeSubmenuTimeout = 0;}this._doHighlight(item);}}, /**
     * @inherit
     */search:function search(term){if(this.submenu){this.submenu.search(term);}else {callSuper(this,'search',term);}}, /**
     * @inherit
     */selectHighlight:function selectHighlight(){if(this.submenu){this.submenu.selectHighlight();}else {callSuper(this,'selectHighlight');}}, /**
     * @inherit
     */selectItem:function selectItem(id){var item=Selectivity.findNestedById(this.results,id);if(item && !item.disabled && !item.submenu){var options={id:id,item:item};if(this.selectivity.triggerEvent('selectivity-selecting',options)){this.selectivity.triggerEvent('selectivity-selected',options);}}}, /**
     * @inherit
     */showResults:function showResults(results,options){if(this.submenu){this.submenu.showResults(results,options);}else {callSuper(this,'showResults',results,options);}}, /**
     * @inherit
     */triggerClose:function triggerClose(){if(this.parentMenu){this.selectivity.$el.trigger('selectivity-close-submenu');}else {callSuper(this,'triggerClose');}}, /**
     * @inherit
     */triggerOpen:function triggerOpen(){if(this.parentMenu){this.selectivity.$el.trigger('selectivity-open-submenu');}else {callSuper(this,'triggerOpen');}}, /**
     * @private
     */_closeSubmenuAndHighlight:function _closeSubmenuAndHighlight(item){if(this.submenu){this.submenu.close();}this._doHighlight(item);}, /**
     * @private
     */_doHighlight:function _doHighlight(item){callSuper(this,'highlight',item);if(item.submenu && !this.submenu){var selectivity=this.selectivity;var Dropdown=selectivity.options.dropdown || Selectivity.Dropdown;if(Dropdown){var quotedId=Selectivity.quoteCssAttr(item.id);var $item=this.$('.selectivity-result-item[data-item-id=' + quotedId + ']');var $dropdownEl=this.$el;this.submenu = new Dropdown({items:item.submenu.items || null,parentMenu:this,position:item.submenu.positionDropdown || function($el){var dropdownPosition=$dropdownEl.position();var width=$dropdownEl.width();$el.css({left:dropdownPosition.left + width + 'px',top:$item.position().top + dropdownPosition.top + 'px'}).width(width);},query:item.submenu.query || null,selectivity:selectivity,showSearchInput:item.submenu.showSearchInput});this.submenu.search('');}}}});Selectivity.Dropdown = SelectivitySubmenu;Selectivity.findNestedById = function(array,id){for(var i=0,length=array.length;i < length;i++) {var item=array[i],result;if(item.id === id){result = item;}else if(item.children){result = Selectivity.findNestedById(item.children,id);}else if(item.submenu && item.submenu.items){result = Selectivity.findNestedById(item.submenu.items,id);}if(result){return result;}}return null;};module.exports = SelectivitySubmenu;},{"10":10,"8":8}],17:[function(_dereq_,module,exports){'use strict';var escape=_dereq_(4);var Selectivity=_dereq_(8);_dereq_(13); /**
 * Default set of templates to use with Selectivity.js.
 *
 * Note that every template can be defined as either a string, a function returning a string (like
 * Handlebars templates, for instance) or as an object containing a render function (like Hogan.js
 * templates, for instance).
 */Selectivity.Templates = { /**
     * Renders the dropdown.
     *
     * The template is expected to have at least one element with the class
     * 'selectivity-results-container', which is where all results will be added to.
     *
     * @param options Options object containing the following properties:
     *                dropdownCssClass - Optional CSS class to add to the top-level element.
     *                searchInputPlaceholder - Optional placeholder text to display in the search
     *                                         input in the dropdown.
     *                showSearchInput - Boolean whether a search input should be shown. If true,
     *                                  an input element with the 'selectivity-search-input' is
     *                                  expected.
     */dropdown:function dropdown(options){var extraClass=options.dropdownCssClass?' ' + options.dropdownCssClass:'',searchInput='';if(options.showSearchInput){extraClass += ' has-search-input';var placeholder=options.searchInputPlaceholder;searchInput = '<div class="selectivity-search-input-container">' + '<input type="text" class="selectivity-search-input"' + (placeholder?' placeholder="' + escape(placeholder) + '"':'') + '>' + '</div>';}return '<div class="selectivity-dropdown' + extraClass + '">' + searchInput + '<div class="selectivity-results-container"></div>' + '</div>';}, /**
     * Renders an error message in the dropdown.
     *
     * @param options Options object containing the following properties:
     *                escape - Boolean whether the message should be HTML-escaped.
     *                message - The message to display.
     */error:function error(options){return '<div class="selectivity-error">' + (options.escape?escape(options.message):options.message) + '</div>';}, /**
     * Renders a loading indicator in the dropdown.
     *
     * This template is expected to have an element with a 'selectivity-loading' class which may be
     * replaced with actual results.
     */loading:function loading(){return '<div class="selectivity-loading">' + Selectivity.Locale.loading + '</div>';}, /**
     * Load more indicator.
     *
     * This template is expected to have an element with a 'selectivity-load-more' class which, when
     * clicked, will load more results.
     */loadMore:function loadMore(){return '<div class="selectivity-load-more">' + Selectivity.Locale.loadMore + '</div>';}, /**
     * Renders multi-selection input boxes.
     *
     * The template is expected to have at least have elements with the following classes:
     * 'selectivity-multiple-input-container' - The element containing all the selected items and
     *                                          the input for selecting additional items.
     * 'selectivity-multiple-input' - The actual input element that allows the user to type to
     *                                search for more items. When selected items are added, they are
     *                                inserted right before this element.
     * 'selectivity-width-detector' - This element is optional, but important to make sure the
     *                                '.selectivity-multiple-input' element will fit in the
     *                                container. The width detector also has the
     *                                'select2-multiple-input' class on purpose to be able to detect
     *                                the width of text entered in the input element.
     *
     * @param options Options object containing the following property:
     *                enabled - Boolean whether the input is enabled.
     */multipleSelectInput:function multipleSelectInput(options){return '<div class="selectivity-multiple-input-container">' + (options.enabled?'<input type="text" autocomplete="off" autocorrect="off" ' + 'autocapitalize="off" ' + 'class="selectivity-multiple-input">' + '<span class="selectivity-multiple-input ' + 'selectivity-width-detector"></span>':'<div class="selectivity-multiple-input ' + 'selectivity-placeholder"></div>') + '<div class="selectivity-clearfix"></div>' + '</div>';}, /**
     * Renders a selected item in multi-selection input boxes.
     *
     * The template is expected to have a top-level element with the class
     * 'selectivity-multiple-selected-item'. This element is also required to have a 'data-item-id'
     * attribute with the ID set to that passed through the options object.
     *
     * An element with the class 'selectivity-multiple-selected-item-remove' should be present
     * which, when clicked, will cause the element to be removed.
     *
     * @param options Options object containing the following properties:
     *                highlighted - Boolean whether this item is currently highlighted.
     *                id - Identifier for the item.
     *                removable - Boolean whether a remove icon should be displayed.
     *                text - Text label which the user sees.
     */multipleSelectedItem:function multipleSelectedItem(options){var extraClass=options.highlighted?' highlighted':'';return '<span class="selectivity-multiple-selected-item' + extraClass + '" ' + 'data-item-id="' + escape(options.id) + '">' + (options.removable?'<a class="selectivity-multiple-selected-item-remove">' + '<i class="fa fa-remove"></i>' + '</a>':'') + escape(options.text) + '</span>';}, /**
     * Renders a message there are no results for the given query.
     *
     * @param options Options object containing the following property:
     *                term - Search term the user is searching for.
     */noResults:function noResults(options){var Locale=Selectivity.Locale;return '<div class="selectivity-error">' + (options.term?Locale.noResultsForTerm(options.term):Locale.noResults) + '</div>';}, /**
     * Renders a container for item children.
     *
     * The template is expected to have an element with the class 'selectivity-result-children'.
     *
     * @param options Options object containing the following property:
     *                childrenHtml - Rendered HTML for the children.
     */resultChildren:function resultChildren(options){return '<div class="selectivity-result-children">' + options.childrenHtml + '</div>';}, /**
     * Render a result item in the dropdown.
     *
     * The template is expected to have a top-level element with the class
     * 'selectivity-result-item'. This element is also required to have a 'data-item-id' attribute
     * with the ID set to that passed through the options object.
     *
     * @param options Options object containing the following properties:
     *                id - Identifier for the item.
     *                text - Text label which the user sees.
     *                disabled - Truthy if the item should be disabled.
     *                submenu - Truthy if the result item has a menu with subresults.
     */resultItem:function resultItem(options){return '<div class="selectivity-result-item' + (options.disabled?' disabled':'') + '"' + ' data-item-id="' + escape(options.id) + '">' + escape(options.text) + (options.submenu?'<i class="selectivity-submenu-icon fa fa-chevron-right"></i>':'') + '</div>';}, /**
     * Render a result label in the dropdown.
     *
     * The template is expected to have a top-level element with the class
     * 'selectivity-result-label'.
     *
     * @param options Options object containing the following properties:
     *                text - Text label.
     */resultLabel:function resultLabel(options){return '<div class="selectivity-result-label">' + escape(options.text) + '</div>';}, /**
     * Renders single-select input boxes.
     *
     * The template is expected to have at least one element with the class
     * 'selectivity-single-result-container' which is the element containing the selected item or
     * the placeholder.
     */singleSelectInput:'<div class="selectivity-single-select">' + '<input type="text" class="selectivity-single-select-input">' + '<div class="selectivity-single-result-container"></div>' + '<i class="fa fa-sort-desc selectivity-caret"></i>' + '</div>', /**
     * Renders the placeholder for single-select input boxes.
     *
     * The template is expected to have a top-level element with the class
     * 'selectivity-placeholder'.
     *
     * @param options Options object containing the following property:
     *                placeholder - The placeholder text.
     */singleSelectPlaceholder:function singleSelectPlaceholder(options){return '<div class="selectivity-placeholder">' + escape(options.placeholder) + '</div>';}, /**
     * Renders the selected item in single-select input boxes.
     *
     * The template is expected to have a top-level element with the class
     * 'selectivity-single-selected-item'. This element is also required to have a 'data-item-id'
     * attribute with the ID set to that passed through the options object.
     *
     * @param options Options object containing the following properties:
     *                id - Identifier for the item.
     *                removable - Boolean whether a remove icon should be displayed.
     *                text - Text label which the user sees.
     */singleSelectedItem:function singleSelectedItem(options){return '<span class="selectivity-single-selected-item" ' + 'data-item-id="' + escape(options.id) + '">' + (options.removable?'<a class="selectivity-single-selected-item-remove">' + '<i class="fa fa-remove"></i>' + '</a>':'') + escape(options.text) + '</span>';}, /**
     * Renders select-box inside single-select input that was initialized on
     * traditional <select> element.
     *
     * @param options Options object containing the following properties:
     *                name - Name of the <select> element.
     *                mode - Mode in which select exists, single or multiple.
     */selectCompliance:function selectCompliance(options){var mode=options.mode;var name=options.name;if(mode === 'multiple' && name.slice(-2) !== '[]'){name += '[]';}return '<select name="' + name + '"' + (mode === 'multiple'?' multiple':'') + '></select>';}, /**
     * Renders the selected item in compliance <select> element as <option>.
     *
     * @param options Options object containing the following properties
     *                id - Identifier for the item.
     *                text - Text label which the user sees.
     */selectOptionCompliance:function selectOptionCompliance(options){return '<option value="' + escape(options.id) + '" selected>' + escape(options.text) + '</option>';}};},{"13":13,"4":4,"8":8}],18:[function(_dereq_,module,exports){'use strict';var $=window.jQuery || window.Zepto;var Selectivity=_dereq_(8);function defaultTokenizer(input,selection,createToken,options){var createTokenItem=options.createTokenItem || function(token){return token?{id:token,text:token}:null;};var separators=options.tokenSeparators;function hasToken(input){return input?separators.some(function(separator){return input.indexOf(separator) > -1;}):false;}function takeToken(input){for(var i=0,length=input.length;i < length;i++) {if(separators.indexOf(input[i]) > -1){return {term:input.slice(0,i),input:input.slice(i + 1)};}}return {};}while(hasToken(input)) {var token=takeToken(input);if(token.term){var item=createTokenItem(token.term);if(item && !Selectivity.findById(selection,item.id)){createToken(item);}}input = token.input;}return input;} /**
 * Option listener that provides a default tokenizer which is used when the tokenSeparators option
 * is specified.
 *
 * @param options Options object. In addition to the options supported in the multi-input
 *                implementation, this may contain the following property:
 *                tokenSeparators - Array of string separators which are used to separate the search
 *                                  string into tokens. If specified and the tokenizer property is
 *                                  not set, the tokenizer property will be set to a function which
 *                                  splits the search term into tokens separated by any of the given
 *                                  separators. The tokens will be converted into selectable items
 *                                  using the 'createTokenItem' function. The default tokenizer also
 *                                  filters out already selected items.
 */Selectivity.OptionListeners.push(function(selectivity,options){if(options.tokenSeparators){options.allowedTypes = $.extend({tokenSeparators:'array'},options.allowedTypes);options.tokenizer = options.tokenizer || defaultTokenizer;}});},{"8":8,"jquery":"jquery"}],19:[function(_dereq_,module,exports){'use strict';var $=window.jQuery || window.Zepto;var Selectivity=_dereq_(8);function replaceSelectElement($el,options){var data=options.multiple?[]:null;var mapOptions=function mapOptions(){var $this=$(this);if($this.is('option')){var text=$this.text();var id=$this.attr('value') || text;if($this.prop('selected')){var item={id:id,text:text};if(options.multiple){data.push(item);}else {data = item;}}return {id:id,text:$this.attr('label') || text};}else {return {text:$this.attr('label'),children:$this.children('option,optgroup').map(mapOptions).get()};}};options.allowClear = 'allowClear' in options?options.allowClear:!$el.prop('required');var items=$el.children('option,optgroup').map(mapOptions).get();options.items = options.query?null:items;options.placeholder = options.placeholder || $el.data('placeholder') || '';options.data = data;var classes=($el.attr('class') || 'selectivity-input').split(' ');if(classes.indexOf('selectivity-input') === -1){classes.push('selectivity-input');}var $div=$('<div>').attr({'id':$el.attr('id'),'class':classes.join(' '),'style':$el.attr('style'),'data-name':$el.attr('name')});$el.replaceWith($div);return $div;}function bindTraditionalSelectEvents(selectivity){var $el=selectivity.$el;$el.on('selectivity-init',function(event,mode){$el.append(selectivity.template('selectCompliance',{mode:mode,name:$el.attr('data-name')})).removeAttr('data-name');}).on('selectivity-init change',function(){var data=selectivity._data;var $select=$el.find('select');if(data instanceof Array){$select.empty();data.forEach(function(item){$select.append(selectivity.template('selectOptionCompliance',item));});}else {if(data){$select.html(selectivity.template('selectOptionCompliance',data));}else {$select.empty();}}});} /**
 * Option listener providing support for converting traditional <select> boxes into Selectivity
 * instances.
 */Selectivity.OptionListeners.push(function(selectivity,options){var $el=selectivity.$el;if($el.is('select')){if($el.attr('autofocus')){setTimeout(function(){selectivity.focus();},1);}selectivity.$el = replaceSelectElement($el,options);selectivity.$el[0].selectivity = selectivity;bindTraditionalSelectEvents(selectivity);}});},{"8":8,"jquery":"jquery"}]},{},[1])(1);});

}).call(this,typeof global !== "undefined" ? global : typeof self !== "undefined" ? self : typeof window !== "undefined" ? window : {})

},{}],55:[function(require,module,exports){
"use strict";

!(function (root, factory) {
    "function" == typeof define && define.amd ? // AMD. Register as an anonymous module unless amdModuleId is set
    define([], function () {
        return root.svg4everybody = factory();
    }) : "object" == typeof exports ? module.exports = factory() : root.svg4everybody = factory();
})(undefined, function () {
    /*! svg4everybody v2.0.3 | github.com/jonathantneal/svg4everybody */
    function embed(svg, target) {
        // if the target exists
        if (target) {
            // create a document fragment to hold the contents of the target
            var fragment = document.createDocumentFragment(),
                viewBox = !svg.getAttribute("viewBox") && target.getAttribute("viewBox");
            // conditionally set the viewBox on the svg
            viewBox && svg.setAttribute("viewBox", viewBox);
            // copy the contents of the clone into the fragment
            for ( // clone the target
            var clone = target.cloneNode(!0); clone.childNodes.length;) {
                fragment.appendChild(clone.firstChild);
            }
            // append the fragment into the svg
            svg.appendChild(fragment);
        }
    }
    function loadreadystatechange(xhr) {
        // listen to changes in the request
        xhr.onreadystatechange = function () {
            // if the request is ready
            if (4 === xhr.readyState) {
                // get the cached html document
                var cachedDocument = xhr._cachedDocument;
                // ensure the cached html document based on the xhr response
                cachedDocument || (cachedDocument = xhr._cachedDocument = document.implementation.createHTMLDocument(""), cachedDocument.body.innerHTML = xhr.responseText, xhr._cachedTarget = {}), // clear the xhr embeds list and embed each item
                xhr._embeds.splice(0).map(function (item) {
                    // get the cached target
                    var target = xhr._cachedTarget[item.id];
                    // ensure the cached target
                    target || (target = xhr._cachedTarget[item.id] = cachedDocument.getElementById(item.id)),
                    // embed the target into the svg
                    embed(item.svg, target);
                });
            }
        }, // test the ready state change immediately
        xhr.onreadystatechange();
    }
    function svg4everybody(rawopts) {
        function oninterval() {
            // while the index exists in the live <use> collection
            for ( // get the cached <use> index
            var index = 0; index < uses.length;) {
                // get the current <use>
                var use = uses[index],
                    svg = use.parentNode;
                if (svg && /svg/i.test(svg.nodeName)) {
                    var src = use.getAttribute("xlink:href");
                    if (polyfill && (!opts.validate || opts.validate(src, svg, use))) {
                        // remove the <use> element
                        svg.removeChild(use);
                        // parse the src and get the url and id
                        var srcSplit = src.split("#"),
                            url = srcSplit.shift(),
                            id = srcSplit.join("#");
                        // if the link is external
                        if (url.length) {
                            // get the cached xhr request
                            var xhr = requests[url];
                            // ensure the xhr request exists
                            xhr || (xhr = requests[url] = new XMLHttpRequest(), xhr.open("GET", url), xhr.send(), xhr._embeds = []), // add the svg and id as an item to the xhr embeds list
                            xhr._embeds.push({
                                svg: svg,
                                id: id
                            }), // prepare the xhr ready state change event
                            loadreadystatechange(xhr);
                        } else {
                            // embed the local id into the svg
                            embed(svg, document.getElementById(id));
                        }
                    }
                } else {
                    // increase the index when the previous value was not "valid"
                    ++index;
                }
            }
            // continue the interval
            requestAnimationFrame(oninterval, 67);
        }
        var polyfill,
            opts = Object(rawopts),
            newerIEUA = /\bTrident\/[567]\b|\bMSIE (?:9|10)\.0\b/,
            webkitUA = /\bAppleWebKit\/(\d+)\b/,
            olderEdgeUA = /\bEdge\/12\.(\d+)\b/;
        polyfill = "polyfill" in opts ? opts.polyfill : newerIEUA.test(navigator.userAgent) || (navigator.userAgent.match(olderEdgeUA) || [])[1] < 10547 || (navigator.userAgent.match(webkitUA) || [])[1] < 537;
        // create xhr requests object
        var requests = {},
            requestAnimationFrame = window.requestAnimationFrame || setTimeout,
            uses = document.getElementsByTagName("use");
        // conditionally start the interval if the polyfill is active
        polyfill && oninterval();
    }
    return svg4everybody;
});

},{}],56:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, '__esModule', {
    value: true
});
var toggleIcons = function toggleIcons(container) {

    if (!container) console.warn('toggleIcons missing container element');

    $(container).find('.toggleable-icon').each(function (indx, item) {
        $(item).hasClass('is-visible') ? $(item).removeClass('is-visible') : $(item).addClass('is-visible');
    });
};

exports.toggleIcons = toggleIcons;

},{}],57:[function(require,module,exports){
/*!
 * Zepto HTML5 Drag and Drop Sortable
 * Author: James Doyle(@james2doyle) http://ohdoylerules.com
 * Repository: https://github.com/james2doyle/zepto-dragswap
 * Licensed under the MIT license
 */
'use strict';

var _controllersAnalyticsController = require('./controllers/analytics-controller');

;(function ($) {
    $.fn.dragswap = function (options) {
        var dragSrcEl;
        function getPrefix() {
            var el = document.createElement('p'),
                getPre,
                transforms = {
                'webkitAnimation': '-webkit-animation',
                'OAnimation': '-o-animation',
                'msAnimation': '-ms-animation',
                'MozAnimation': '-moz-animation',
                'animation': 'animation'
            };
            document.body.insertBefore(el, null);
            for (var t in transforms) {
                if (el.style[t] !== undefined) {
                    el.style[t] = "translate3d(1px,1px,1px)";
                    getPre = window.getComputedStyle(el).getPropertyValue(transforms[t]);
                    // return the successful prefix
                    return t;
                }
            }
            document.body.removeChild(el);
        }
        this.defaultOptions = {
            element: 'li',
            overClass: 'over',
            moveClass: 'moving',
            dropClass: 'drop',
            dropAnimation: false,
            exclude: '.disabled',
            prefix: getPrefix(),
            dropComplete: function dropComplete() {
                return;
            }
        };

        function excludePattern(elem) {
            return elem.is(settings.excludePatt);
        }

        function onAnimEnd(elem) {
            var $elem = $(elem);
            $elem.addClass(settings.dropClass);
            // add an event for when the animation has finished
            $elem.on(settings.prefix + 'End', function () {
                // remove the class now that the animation is done
                $elem.removeClass(settings.dropClass);
            }, false);
        }

        function handleDragStart(e) {
            if (!excludePattern($(this))) {
                e.preventDefault();
                e.stopPropagation();
                return false;
            }
            $(this).addClass(settings.moveClass);
            // get the dragging element
            dragSrcEl = this;
            // it is moving
            //console.log(e);
            if (e.originalEvent.dataTransfer) {
                var dt = e.originalEvent.dataTransfer;
                dt.effectAllowed = 'move';
                dt.setData('text', this.innerHTML);
            } else if (e.dataTransfer) {
                var dt = e.dataTransfer;
                dt.effectAllowed = 'move';
                dt.setData('text', this.innerHTML);
            }
        }

        function handleDragEnter(e) {
            // this / e.target is the current hover target.
            $(this).addClass(settings.overClass);
        }

        function handleDragLeave(e) {
            $(this).removeClass(settings.overClass); // this / e.target is previous target element.
        }

        function handleDragOver(e) {
            if (e.preventDefault) {
                e.preventDefault(); // Necessary. Allows us to drop.
            }
            if (e.originalEvent.dataTransfer) {
                e.originalEvent.dataTransfer.dropEffect = 'move'; // See the section on the DataTransfer object.
            } else if (e.dataTransfer) {
                    e.dataTransfer.dropEffect = 'move'; // See the section on the DataTransfer object.
                }
            return false;
        }

        function handleDrop(e) {
            // this / e.target is current target element.
            if (e.stopPropagation) {
                e.stopPropagation(); // Stops some browsers from redirecting.
            }
            if (!excludePattern($(this))) {
                console.log('prevent drop');
                return false;
            }

            // Don't do anything if dropping the same column we're draggi.
            if (dragSrcEl != this && dragSrcEl != undefined) {
                // Set the source column's HTML to the HTML of the column dropped on.
                var oldEl = {
                    html: this.innerHTML,
                    id: this.id
                };
                var newEl = {
                    html: dragSrcEl.innerHTML,
                    id: dragSrcEl.id
                };
                // swap all the data
                var that = this;
                // swap all the data
                this.innerHTML = newEl.html;
                this.id = newEl.id;
                $(dragSrcEl).hide();
                dragSrcEl.innerHTML = oldEl.html;
                dragSrcEl.id = oldEl.id;
                if ($(this).index() > $(dragSrcEl).index()) {
                    $(dragSrcEl).insertBefore(that);
                } else {
                    $(dragSrcEl).insertAfter(that);
                }
                $(dragSrcEl).show();
                if (settings.dropAnimation) {
                    onAnimEnd(this);
                    onAnimEnd(dragSrcEl);
                }
                $(this).siblings().removeAttr('draggable');
                $(this).siblings().filter(settings.excludePatt).attr('draggable', true);
                console.log('dropped');
                $('#validatePreference').val(1);
                $('#validateMyViewPriority').val(true);

                var channelTxt = $.trim($(this).find('td:nth-child(1)').html().split('<input')[0]);
                var eventDetails = {
                    event_name: "topic_position_change",
                    "page_name": analytics_data["page_name"],
                    "ga_eventCategory": "My View Settings Link",
                    "ga_eventAction": channelTxt
                };
                (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, eventDetails));
                eventDetails = {};

                settings.dropComplete();
            }
            return false;
        }

        var settings = $.extend({}, this.defaultOptions, options);
        if (settings.exclude) {
            if (typeof settings.exclude != 'string') {
                var excludePatt = '';
                for (var i = 0; i < settings.exclude.length; i++) {
                    excludePatt += ':not(' + settings.exclude[i] + ')';
                }
                settings.excludePatt = excludePatt;
            } else {
                settings.excludePatt = ':not(' + settings.exclude + ')';
            }
        }

        var method = String(options);
        var items = [];
        // check for the methods
        if (/^(toArray|toJSON)$/.test(method)) {
            if (method == 'toArray') {
                $(this).find(settings.element).each(function (index, elem) {
                    items.push(this.id);
                });
                return items;
            } else if (method == 'toJSON') {
                $(this).find(settings.element).each(function (index, elem) {
                    items[index] = {
                        id: this.id
                    };
                });
                return JSON.stringify(items);
            }
            return;
        }

        return this.each(function (index, item) {
            var $this = $(this);
            // select all but the disabled things
            var $elem = $this.find(settings.element);

            var target = this;
            var config = { childList: true };
            var observer = new MutationObserver(function (mutations) {
                console.log(mutations);
                for (var i = 0; i < mutations.length; i++) {
                    if (mutations[i].addedNodes.length != 0) {
                        for (var j = 0; j < mutations[i].addedNodes.length; j++) {
                            $(mutations[i].addedNodes[j]).siblings().removeAttr('draggable');
                            $(mutations[i].addedNodes[j]).siblings().filter(settings.excludePatt).attr('draggable', true);
                        }
                    }
                }
            });

            observer.observe(target, config);

            function handleDragEnd(e) {
                $this.removeClass(settings.moveClass);
                // this/e.target is the source node.
                //console.log('handleDragEnd');
                $elem = $this.find(settings.element);
                $elem.each(function (index, item) {
                    // console.log(item);
                    $(item).removeClass(settings.overClass);
                    $(item).removeClass(settings.moveClass);
                });
            }
            // set the items to draggable
            $elem.filter(settings.excludePatt).attr('draggable', true);

            $this.off('dragstart');
            $this.off('dragenter');
            $this.off('dragover');
            $this.off('dragleave');
            $this.off('drop');
            $this.off('dragend');

            $this.on('dragstart', settings.element, handleDragStart);
            $this.on('dragenter', settings.element, handleDragEnter);
            $this.on('dragover', settings.element, handleDragOver);
            $this.on('dragleave', settings.element, handleDragLeave);
            $this.on('drop', settings.element, handleDrop);
            $this.on('dragend', settings.element, handleDragEnd);
        });
    };
})(Zepto);

},{"./controllers/analytics-controller":40}],58:[function(require,module,exports){
/* Zepto v1.1.6 - zepto event ajax form ie - zeptojs.com/license */
"use strict";

var Zepto = (function () {
  function L(t) {
    return null == t ? String(t) : j[S.call(t)] || "object";
  }function Z(t) {
    return "function" == L(t);
  }function _(t) {
    return null != t && t == t.window;
  }function $(t) {
    return null != t && t.nodeType == t.DOCUMENT_NODE;
  }function D(t) {
    return "object" == L(t);
  }function M(t) {
    return D(t) && !_(t) && Object.getPrototypeOf(t) == Object.prototype;
  }function R(t) {
    return "number" == typeof t.length;
  }function k(t) {
    return s.call(t, function (t) {
      return null != t;
    });
  }function z(t) {
    return t.length > 0 ? n.fn.concat.apply([], t) : t;
  }function F(t) {
    return t.replace(/::/g, "/").replace(/([A-Z]+)([A-Z][a-z])/g, "$1_$2").replace(/([a-z\d])([A-Z])/g, "$1_$2").replace(/_/g, "-").toLowerCase();
  }function q(t) {
    return t in f ? f[t] : f[t] = new RegExp("(^|\\s)" + t + "(\\s|$)");
  }function H(t, e) {
    return "number" != typeof e || c[F(t)] ? e : e + "px";
  }function I(t) {
    var e, n;return u[t] || (e = a.createElement(t), a.body.appendChild(e), n = getComputedStyle(e, "").getPropertyValue("display"), e.parentNode.removeChild(e), "none" == n && (n = "block"), u[t] = n), u[t];
  }function V(t) {
    return "children" in t ? o.call(t.children) : n.map(t.childNodes, function (t) {
      return 1 == t.nodeType ? t : void 0;
    });
  }function B(n, i, r) {
    for (e in i) r && (M(i[e]) || A(i[e])) ? (M(i[e]) && !M(n[e]) && (n[e] = {}), A(i[e]) && !A(n[e]) && (n[e] = []), B(n[e], i[e], r)) : i[e] !== t && (n[e] = i[e]);
  }function U(t, e) {
    return null == e ? n(t) : n(t).filter(e);
  }function J(t, e, n, i) {
    return Z(e) ? e.call(t, n, i) : e;
  }function X(t, e, n) {
    null == n ? t.removeAttribute(e) : t.setAttribute(e, n);
  }function W(e, n) {
    var i = e.className || "",
        r = i && i.baseVal !== t;return n === t ? r ? i.baseVal : i : void (r ? i.baseVal = n : e.className = n);
  }function Y(t) {
    try {
      return t ? "true" == t || ("false" == t ? !1 : "null" == t ? null : +t + "" == t ? +t : /^[\[\{]/.test(t) ? n.parseJSON(t) : t) : t;
    } catch (e) {
      return t;
    }
  }function G(t, e) {
    e(t);for (var n = 0, i = t.childNodes.length; i > n; n++) G(t.childNodes[n], e);
  }var t,
      e,
      n,
      i,
      C,
      N,
      r = [],
      o = r.slice,
      s = r.filter,
      a = window.document,
      u = {},
      f = {},
      c = { "column-count": 1, columns: 1, "font-weight": 1, "line-height": 1, opacity: 1, "z-index": 1, zoom: 1 },
      l = /^\s*<(\w+|!)[^>]*>/,
      h = /^<(\w+)\s*\/?>(?:<\/\1>|)$/,
      p = /<(?!area|br|col|embed|hr|img|input|link|meta|param)(([\w:]+)[^>]*)\/>/gi,
      d = /^(?:body|html)$/i,
      m = /([A-Z])/g,
      g = ["val", "css", "html", "text", "data", "width", "height", "offset"],
      v = ["after", "prepend", "before", "append"],
      y = a.createElement("table"),
      x = a.createElement("tr"),
      b = { tr: a.createElement("tbody"), tbody: y, thead: y, tfoot: y, td: x, th: x, "*": a.createElement("div") },
      w = /complete|loaded|interactive/,
      E = /^[\w-]*$/,
      j = {},
      S = j.toString,
      T = {},
      O = a.createElement("div"),
      P = { tabindex: "tabIndex", readonly: "readOnly", "for": "htmlFor", "class": "className", maxlength: "maxLength", cellspacing: "cellSpacing", cellpadding: "cellPadding", rowspan: "rowSpan", colspan: "colSpan", usemap: "useMap", frameborder: "frameBorder", contenteditable: "contentEditable" },
      A = Array.isArray || function (t) {
    return t instanceof Array;
  };return T.matches = function (t, e) {
    if (!e || !t || 1 !== t.nodeType) return !1;var n = t.webkitMatchesSelector || t.mozMatchesSelector || t.oMatchesSelector || t.matchesSelector;if (n) return n.call(t, e);var i,
        r = t.parentNode,
        o = !r;return o && (r = O).appendChild(t), i = ~T.qsa(r, e).indexOf(t), o && O.removeChild(t), i;
  }, C = function (t) {
    return t.replace(/-+(.)?/g, function (t, e) {
      return e ? e.toUpperCase() : "";
    });
  }, N = function (t) {
    return s.call(t, function (e, n) {
      return t.indexOf(e) == n;
    });
  }, T.fragment = function (e, i, r) {
    var s, u, f;return h.test(e) && (s = n(a.createElement(RegExp.$1))), s || (e.replace && (e = e.replace(p, "<$1></$2>")), i === t && (i = l.test(e) && RegExp.$1), i in b || (i = "*"), f = b[i], f.innerHTML = "" + e, s = n.each(o.call(f.childNodes), function () {
      f.removeChild(this);
    })), M(r) && (u = n(s), n.each(r, function (t, e) {
      g.indexOf(t) > -1 ? u[t](e) : u.attr(t, e);
    })), s;
  }, T.Z = function (t, e) {
    return t = t || [], t.__proto__ = n.fn, t.selector = e || "", t;
  }, T.isZ = function (t) {
    return t instanceof T.Z;
  }, T.init = function (e, i) {
    var r;if (!e) return T.Z();if ("string" == typeof e) if ((e = e.trim(), "<" == e[0] && l.test(e))) r = T.fragment(e, RegExp.$1, i), e = null;else {
      if (i !== t) return n(i).find(e);r = T.qsa(a, e);
    } else {
      if (Z(e)) return n(a).ready(e);if (T.isZ(e)) return e;if (A(e)) r = k(e);else if (D(e)) r = [e], e = null;else if (l.test(e)) r = T.fragment(e.trim(), RegExp.$1, i), e = null;else {
        if (i !== t) return n(i).find(e);r = T.qsa(a, e);
      }
    }return T.Z(r, e);
  }, n = function (t, e) {
    return T.init(t, e);
  }, n.extend = function (t) {
    var e,
        n = o.call(arguments, 1);return "boolean" == typeof t && (e = t, t = n.shift()), n.forEach(function (n) {
      B(t, n, e);
    }), t;
  }, T.qsa = function (t, e) {
    var n,
        i = "#" == e[0],
        r = !i && "." == e[0],
        s = i || r ? e.slice(1) : e,
        a = E.test(s);return $(t) && a && i ? (n = t.getElementById(s)) ? [n] : [] : 1 !== t.nodeType && 9 !== t.nodeType ? [] : o.call(a && !i ? r ? t.getElementsByClassName(s) : t.getElementsByTagName(e) : t.querySelectorAll(e));
  }, n.contains = a.documentElement.contains ? function (t, e) {
    return t !== e && t.contains(e);
  } : function (t, e) {
    for (; e && (e = e.parentNode);) if (e === t) return !0;return !1;
  }, n.type = L, n.isFunction = Z, n.isWindow = _, n.isArray = A, n.isPlainObject = M, n.isEmptyObject = function (t) {
    var e;for (e in t) return !1;return !0;
  }, n.inArray = function (t, e, n) {
    return r.indexOf.call(e, t, n);
  }, n.camelCase = C, n.trim = function (t) {
    return null == t ? "" : String.prototype.trim.call(t);
  }, n.uuid = 0, n.support = {}, n.expr = {}, n.map = function (t, e) {
    var n,
        r,
        o,
        i = [];if (R(t)) for (r = 0; r < t.length; r++) n = e(t[r], r), null != n && i.push(n);else for (o in t) n = e(t[o], o), null != n && i.push(n);return z(i);
  }, n.each = function (t, e) {
    var n, i;if (R(t)) {
      for (n = 0; n < t.length; n++) if (e.call(t[n], n, t[n]) === !1) return t;
    } else for (i in t) if (e.call(t[i], i, t[i]) === !1) return t;return t;
  }, n.grep = function (t, e) {
    return s.call(t, e);
  }, window.JSON && (n.parseJSON = JSON.parse), n.each("Boolean Number String Function Array Date RegExp Object Error".split(" "), function (t, e) {
    j["[object " + e + "]"] = e.toLowerCase();
  }), n.fn = { forEach: r.forEach, reduce: r.reduce, push: r.push, sort: r.sort, indexOf: r.indexOf, concat: r.concat, map: function map(t) {
      return n(n.map(this, function (e, n) {
        return t.call(e, n, e);
      }));
    }, slice: function slice() {
      return n(o.apply(this, arguments));
    }, ready: function ready(t) {
      return w.test(a.readyState) && a.body ? t(n) : a.addEventListener("DOMContentLoaded", function () {
        t(n);
      }, !1), this;
    }, get: function get(e) {
      return e === t ? o.call(this) : this[e >= 0 ? e : e + this.length];
    }, toArray: function toArray() {
      return this.get();
    }, size: function size() {
      return this.length;
    }, remove: function remove() {
      return this.each(function () {
        null != this.parentNode && this.parentNode.removeChild(this);
      });
    }, each: function each(t) {
      return r.every.call(this, function (e, n) {
        return t.call(e, n, e) !== !1;
      }), this;
    }, filter: function filter(t) {
      return Z(t) ? this.not(this.not(t)) : n(s.call(this, function (e) {
        return T.matches(e, t);
      }));
    }, add: function add(t, e) {
      return n(N(this.concat(n(t, e))));
    }, is: function is(t) {
      return this.length > 0 && T.matches(this[0], t);
    }, not: function not(e) {
      var i = [];if (Z(e) && e.call !== t) this.each(function (t) {
        e.call(this, t) || i.push(this);
      });else {
        var r = "string" == typeof e ? this.filter(e) : R(e) && Z(e.item) ? o.call(e) : n(e);this.forEach(function (t) {
          r.indexOf(t) < 0 && i.push(t);
        });
      }return n(i);
    }, has: function has(t) {
      return this.filter(function () {
        return D(t) ? n.contains(this, t) : n(this).find(t).size();
      });
    }, eq: function eq(t) {
      return -1 === t ? this.slice(t) : this.slice(t, +t + 1);
    }, first: function first() {
      var t = this[0];return t && !D(t) ? t : n(t);
    }, last: function last() {
      var t = this[this.length - 1];return t && !D(t) ? t : n(t);
    }, find: function find(t) {
      var e,
          i = this;return e = t ? "object" == typeof t ? n(t).filter(function () {
        var t = this;return r.some.call(i, function (e) {
          return n.contains(e, t);
        });
      }) : 1 == this.length ? n(T.qsa(this[0], t)) : this.map(function () {
        return T.qsa(this, t);
      }) : n();
    }, closest: function closest(t, e) {
      var i = this[0],
          r = !1;for ("object" == typeof t && (r = n(t)); i && !(r ? r.indexOf(i) >= 0 : T.matches(i, t));) i = i !== e && !$(i) && i.parentNode;return n(i);
    }, parents: function parents(t) {
      for (var e = [], i = this; i.length > 0;) i = n.map(i, function (t) {
        return (t = t.parentNode) && !$(t) && e.indexOf(t) < 0 ? (e.push(t), t) : void 0;
      });return U(e, t);
    }, parent: function parent(t) {
      return U(N(this.pluck("parentNode")), t);
    }, children: function children(t) {
      return U(this.map(function () {
        return V(this);
      }), t);
    }, contents: function contents() {
      return this.map(function () {
        return o.call(this.childNodes);
      });
    }, siblings: function siblings(t) {
      return U(this.map(function (t, e) {
        return s.call(V(e.parentNode), function (t) {
          return t !== e;
        });
      }), t);
    }, empty: function empty() {
      return this.each(function () {
        this.innerHTML = "";
      });
    }, pluck: function pluck(t) {
      return n.map(this, function (e) {
        return e[t];
      });
    }, show: function show() {
      return this.each(function () {
        "none" == this.style.display && (this.style.display = ""), "none" == getComputedStyle(this, "").getPropertyValue("display") && (this.style.display = I(this.nodeName));
      });
    }, replaceWith: function replaceWith(t) {
      return this.before(t).remove();
    }, wrap: function wrap(t) {
      var e = Z(t);if (this[0] && !e) var i = n(t).get(0),
          r = i.parentNode || this.length > 1;return this.each(function (o) {
        n(this).wrapAll(e ? t.call(this, o) : r ? i.cloneNode(!0) : i);
      });
    }, wrapAll: function wrapAll(t) {
      if (this[0]) {
        n(this[0]).before(t = n(t));for (var e; (e = t.children()).length;) t = e.first();n(t).append(this);
      }return this;
    }, wrapInner: function wrapInner(t) {
      var e = Z(t);return this.each(function (i) {
        var r = n(this),
            o = r.contents(),
            s = e ? t.call(this, i) : t;o.length ? o.wrapAll(s) : r.append(s);
      });
    }, unwrap: function unwrap() {
      return this.parent().each(function () {
        n(this).replaceWith(n(this).children());
      }), this;
    }, clone: function clone() {
      return this.map(function () {
        return this.cloneNode(!0);
      });
    }, hide: function hide() {
      return this.css("display", "none");
    }, toggle: function toggle(e) {
      return this.each(function () {
        var i = n(this);(e === t ? "none" == i.css("display") : e) ? i.show() : i.hide();
      });
    }, prev: function prev(t) {
      return n(this.pluck("previousElementSibling")).filter(t || "*");
    }, next: function next(t) {
      return n(this.pluck("nextElementSibling")).filter(t || "*");
    }, html: function html(t) {
      return 0 in arguments ? this.each(function (e) {
        var i = this.innerHTML;n(this).empty().append(J(this, t, e, i));
      }) : 0 in this ? this[0].innerHTML : null;
    }, text: function text(t) {
      return 0 in arguments ? this.each(function (e) {
        var n = J(this, t, e, this.textContent);this.textContent = null == n ? "" : "" + n;
      }) : 0 in this ? this[0].textContent : null;
    }, attr: function attr(n, i) {
      var r;return "string" != typeof n || 1 in arguments ? this.each(function (t) {
        if (1 === this.nodeType) if (D(n)) for (e in n) X(this, e, n[e]);else X(this, n, J(this, i, t, this.getAttribute(n)));
      }) : this.length && 1 === this[0].nodeType ? !(r = this[0].getAttribute(n)) && n in this[0] ? this[0][n] : r : t;
    }, removeAttr: function removeAttr(t) {
      return this.each(function () {
        1 === this.nodeType && t.split(" ").forEach(function (t) {
          X(this, t);
        }, this);
      });
    }, prop: function prop(t, e) {
      return t = P[t] || t, 1 in arguments ? this.each(function (n) {
        this[t] = J(this, e, n, this[t]);
      }) : this[0] && this[0][t];
    }, data: function data(e, n) {
      var i = "data-" + e.replace(m, "-$1").toLowerCase(),
          r = 1 in arguments ? this.attr(i, n) : this.attr(i);return null !== r ? Y(r) : t;
    }, val: function val(t) {
      return 0 in arguments ? this.each(function (e) {
        this.value = J(this, t, e, this.value);
      }) : this[0] && (this[0].multiple ? n(this[0]).find("option").filter(function () {
        return this.selected;
      }).pluck("value") : this[0].value);
    }, offset: function offset(t) {
      if (t) return this.each(function (e) {
        var i = n(this),
            r = J(this, t, e, i.offset()),
            o = i.offsetParent().offset(),
            s = { top: r.top - o.top, left: r.left - o.left };"static" == i.css("position") && (s.position = "relative"), i.css(s);
      });if (!this.length) return null;var e = this[0].getBoundingClientRect();return { left: e.left + window.pageXOffset, top: e.top + window.pageYOffset, width: Math.round(e.width), height: Math.round(e.height) };
    }, css: function css(t, i) {
      if (arguments.length < 2) {
        var r,
            o = this[0];if (!o) return;if ((r = getComputedStyle(o, ""), "string" == typeof t)) return o.style[C(t)] || r.getPropertyValue(t);if (A(t)) {
          var s = {};return n.each(t, function (t, e) {
            s[e] = o.style[C(e)] || r.getPropertyValue(e);
          }), s;
        }
      }var a = "";if ("string" == L(t)) i || 0 === i ? a = F(t) + ":" + H(t, i) : this.each(function () {
        this.style.removeProperty(F(t));
      });else for (e in t) t[e] || 0 === t[e] ? a += F(e) + ":" + H(e, t[e]) + ";" : this.each(function () {
        this.style.removeProperty(F(e));
      });return this.each(function () {
        this.style.cssText += ";" + a;
      });
    }, index: function index(t) {
      return t ? this.indexOf(n(t)[0]) : this.parent().children().indexOf(this[0]);
    }, hasClass: function hasClass(t) {
      return t ? r.some.call(this, function (t) {
        return this.test(W(t));
      }, q(t)) : !1;
    }, addClass: function addClass(t) {
      return t ? this.each(function (e) {
        if ("className" in this) {
          i = [];var r = W(this),
              o = J(this, t, e, r);o.split(/\s+/g).forEach(function (t) {
            n(this).hasClass(t) || i.push(t);
          }, this), i.length && W(this, r + (r ? " " : "") + i.join(" "));
        }
      }) : this;
    }, removeClass: function removeClass(e) {
      return this.each(function (n) {
        if ("className" in this) {
          if (e === t) return W(this, "");i = W(this), J(this, e, n, i).split(/\s+/g).forEach(function (t) {
            i = i.replace(q(t), " ");
          }), W(this, i.trim());
        }
      });
    }, toggleClass: function toggleClass(e, i) {
      return e ? this.each(function (r) {
        var o = n(this),
            s = J(this, e, r, W(this));s.split(/\s+/g).forEach(function (e) {
          (i === t ? !o.hasClass(e) : i) ? o.addClass(e) : o.removeClass(e);
        });
      }) : this;
    }, scrollTop: function scrollTop(e) {
      if (this.length) {
        var n = ("scrollTop" in this[0]);return e === t ? n ? this[0].scrollTop : this[0].pageYOffset : this.each(n ? function () {
          this.scrollTop = e;
        } : function () {
          this.scrollTo(this.scrollX, e);
        });
      }
    }, scrollLeft: function scrollLeft(e) {
      if (this.length) {
        var n = ("scrollLeft" in this[0]);return e === t ? n ? this[0].scrollLeft : this[0].pageXOffset : this.each(n ? function () {
          this.scrollLeft = e;
        } : function () {
          this.scrollTo(e, this.scrollY);
        });
      }
    }, position: function position() {
      if (this.length) {
        var t = this[0],
            e = this.offsetParent(),
            i = this.offset(),
            r = d.test(e[0].nodeName) ? { top: 0, left: 0 } : e.offset();return i.top -= parseFloat(n(t).css("margin-top")) || 0, i.left -= parseFloat(n(t).css("margin-left")) || 0, r.top += parseFloat(n(e[0]).css("border-top-width")) || 0, r.left += parseFloat(n(e[0]).css("border-left-width")) || 0, { top: i.top - r.top, left: i.left - r.left };
      }
    }, offsetParent: function offsetParent() {
      return this.map(function () {
        for (var t = this.offsetParent || a.body; t && !d.test(t.nodeName) && "static" == n(t).css("position");) t = t.offsetParent;return t;
      });
    } }, n.fn.detach = n.fn.remove, ["width", "height"].forEach(function (e) {
    var i = e.replace(/./, function (t) {
      return t[0].toUpperCase();
    });n.fn[e] = function (r) {
      var o,
          s = this[0];return r === t ? _(s) ? s["inner" + i] : $(s) ? s.documentElement["scroll" + i] : (o = this.offset()) && o[e] : this.each(function (t) {
        s = n(this), s.css(e, J(this, r, t, s[e]()));
      });
    };
  }), v.forEach(function (t, e) {
    var i = e % 2;n.fn[t] = function () {
      var t,
          o,
          r = n.map(arguments, function (e) {
        return t = L(e), "object" == t || "array" == t || null == e ? e : T.fragment(e);
      }),
          s = this.length > 1;return r.length < 1 ? this : this.each(function (t, u) {
        o = i ? u : u.parentNode, u = 0 == e ? u.nextSibling : 1 == e ? u.firstChild : 2 == e ? u : null;var f = n.contains(a.documentElement, o);r.forEach(function (t) {
          if (s) t = t.cloneNode(!0);else if (!o) return n(t).remove();o.insertBefore(t, u), f && G(t, function (t) {
            null == t.nodeName || "SCRIPT" !== t.nodeName.toUpperCase() || t.type && "text/javascript" !== t.type || t.src || window.eval.call(window, t.innerHTML);
          });
        });
      });
    }, n.fn[i ? t + "To" : "insert" + (e ? "Before" : "After")] = function (e) {
      return n(e)[t](this), this;
    };
  }), T.Z.prototype = n.fn, T.uniq = N, T.deserializeValue = Y, n.zepto = T, n;
})();window.Zepto = Zepto, void 0 === window.$ && (window.$ = Zepto), (function (t) {
  function l(t) {
    return t._zid || (t._zid = e++);
  }function h(t, e, n, i) {
    if ((e = p(e), e.ns)) var r = d(e.ns);return (s[l(t)] || []).filter(function (t) {
      return !(!t || e.e && t.e != e.e || e.ns && !r.test(t.ns) || n && l(t.fn) !== l(n) || i && t.sel != i);
    });
  }function p(t) {
    var e = ("" + t).split(".");return { e: e[0], ns: e.slice(1).sort().join(" ") };
  }function d(t) {
    return new RegExp("(?:^| )" + t.replace(" ", " .* ?") + "(?: |$)");
  }function m(t, e) {
    return t.del && !u && t.e in f || !!e;
  }function g(t) {
    return c[t] || u && f[t] || t;
  }function v(e, i, r, o, a, u, f) {
    var h = l(e),
        d = s[h] || (s[h] = []);i.split(/\s/).forEach(function (i) {
      if ("ready" == i) return t(document).ready(r);var s = p(i);s.fn = r, s.sel = a, s.e in c && (r = function (e) {
        var n = e.relatedTarget;return !n || n !== this && !t.contains(this, n) ? s.fn.apply(this, arguments) : void 0;
      }), s.del = u;var l = u || r;s.proxy = function (t) {
        if ((t = j(t), !t.isImmediatePropagationStopped())) {
          t.data = o;var i = l.apply(e, t._args == n ? [t] : [t].concat(t._args));return i === !1 && (t.preventDefault(), t.stopPropagation()), i;
        }
      }, s.i = d.length, d.push(s), "addEventListener" in e && e.addEventListener(g(s.e), s.proxy, m(s, f));
    });
  }function y(t, e, n, i, r) {
    var o = l(t);(e || "").split(/\s/).forEach(function (e) {
      h(t, e, n, i).forEach(function (e) {
        delete s[o][e.i], "removeEventListener" in t && t.removeEventListener(g(e.e), e.proxy, m(e, r));
      });
    });
  }function j(e, i) {
    return (i || !e.isDefaultPrevented) && (i || (i = e), t.each(E, function (t, n) {
      var r = i[t];e[t] = function () {
        return this[n] = x, r && r.apply(i, arguments);
      }, e[n] = b;
    }), (i.defaultPrevented !== n ? i.defaultPrevented : "returnValue" in i ? i.returnValue === !1 : i.getPreventDefault && i.getPreventDefault()) && (e.isDefaultPrevented = x)), e;
  }function S(t) {
    var e,
        i = { originalEvent: t };for (e in t) w.test(e) || t[e] === n || (i[e] = t[e]);return j(i, t);
  }var n,
      e = 1,
      i = Array.prototype.slice,
      r = t.isFunction,
      o = function o(t) {
    return "string" == typeof t;
  },
      s = {},
      a = {},
      u = ("onfocusin" in window),
      f = { focus: "focusin", blur: "focusout" },
      c = { mouseenter: "mouseover", mouseleave: "mouseout" };a.click = a.mousedown = a.mouseup = a.mousemove = "MouseEvents", t.event = { add: v, remove: y }, t.proxy = function (e, n) {
    var s = 2 in arguments && i.call(arguments, 2);if (r(e)) {
      var a = function a() {
        return e.apply(n, s ? s.concat(i.call(arguments)) : arguments);
      };return a._zid = l(e), a;
    }if (o(n)) return s ? (s.unshift(e[n], e), t.proxy.apply(null, s)) : t.proxy(e[n], e);throw new TypeError("expected function");
  }, t.fn.bind = function (t, e, n) {
    return this.on(t, e, n);
  }, t.fn.unbind = function (t, e) {
    return this.off(t, e);
  }, t.fn.one = function (t, e, n, i) {
    return this.on(t, e, n, i, 1);
  };var x = function x() {
    return !0;
  },
      b = function b() {
    return !1;
  },
      w = /^([A-Z]|returnValue$|layer[XY]$)/,
      E = { preventDefault: "isDefaultPrevented", stopImmediatePropagation: "isImmediatePropagationStopped", stopPropagation: "isPropagationStopped" };t.fn.delegate = function (t, e, n) {
    return this.on(e, t, n);
  }, t.fn.undelegate = function (t, e, n) {
    return this.off(e, t, n);
  }, t.fn.live = function (e, n) {
    return t(document.body).delegate(this.selector, e, n), this;
  }, t.fn.die = function (e, n) {
    return t(document.body).undelegate(this.selector, e, n), this;
  }, t.fn.on = function (e, s, a, u, f) {
    var c,
        l,
        h = this;return e && !o(e) ? (t.each(e, function (t, e) {
      h.on(t, s, a, e, f);
    }), h) : (o(s) || r(u) || u === !1 || (u = a, a = s, s = n), (r(a) || a === !1) && (u = a, a = n), u === !1 && (u = b), h.each(function (n, r) {
      f && (c = function (t) {
        return y(r, t.type, u), u.apply(this, arguments);
      }), s && (l = function (e) {
        var n,
            o = t(e.target).closest(s, r).get(0);return o && o !== r ? (n = t.extend(S(e), { currentTarget: o, liveFired: r }), (c || u).apply(o, [n].concat(i.call(arguments, 1)))) : void 0;
      }), v(r, e, u, a, s, l || c);
    }));
  }, t.fn.off = function (e, i, s) {
    var a = this;return e && !o(e) ? (t.each(e, function (t, e) {
      a.off(t, i, e);
    }), a) : (o(i) || r(s) || s === !1 || (s = i, i = n), s === !1 && (s = b), a.each(function () {
      y(this, e, s, i);
    }));
  }, t.fn.trigger = function (e, n) {
    return e = o(e) || t.isPlainObject(e) ? t.Event(e) : j(e), e._args = n, this.each(function () {
      e.type in f && "function" == typeof this[e.type] ? this[e.type]() : "dispatchEvent" in this ? this.dispatchEvent(e) : t(this).triggerHandler(e, n);
    });
  }, t.fn.triggerHandler = function (e, n) {
    var i, r;return this.each(function (s, a) {
      i = S(o(e) ? t.Event(e) : e), i._args = n, i.target = a, t.each(h(a, e.type || e), function (t, e) {
        return r = e.proxy(i), i.isImmediatePropagationStopped() ? !1 : void 0;
      });
    }), r;
  }, "focusin focusout focus blur load resize scroll unload click dblclick mousedown mouseup mousemove mouseover mouseout mouseenter mouseleave change select keydown keypress keyup error".split(" ").forEach(function (e) {
    t.fn[e] = function (t) {
      return 0 in arguments ? this.bind(e, t) : this.trigger(e);
    };
  }), t.Event = function (t, e) {
    o(t) || (e = t, t = e.type);var n = document.createEvent(a[t] || "Events"),
        i = !0;if (e) for (var r in e) "bubbles" == r ? i = !!e[r] : n[r] = e[r];return n.initEvent(t, i, !0), j(n);
  };
})(Zepto), (function (t) {
  function h(e, n, i) {
    var r = t.Event(n);return t(e).trigger(r, i), !r.isDefaultPrevented();
  }function p(t, e, i, r) {
    return t.global ? h(e || n, i, r) : void 0;
  }function d(e) {
    e.global && 0 === t.active++ && p(e, null, "ajaxStart");
  }function m(e) {
    e.global && ! --t.active && p(e, null, "ajaxStop");
  }function g(t, e) {
    var n = e.context;return e.beforeSend.call(n, t, e) === !1 || p(e, n, "ajaxBeforeSend", [t, e]) === !1 ? !1 : void p(e, n, "ajaxSend", [t, e]);
  }function v(t, e, n, i) {
    var r = n.context,
        o = "success";n.success.call(r, t, o, e), i && i.resolveWith(r, [t, o, e]), p(n, r, "ajaxSuccess", [e, n, t]), x(o, e, n);
  }function y(t, e, n, i, r) {
    var o = i.context;i.error.call(o, n, e, t), r && r.rejectWith(o, [n, e, t]), p(i, o, "ajaxError", [n, i, t || e]), x(e, n, i);
  }function x(t, e, n) {
    var i = n.context;n.complete.call(i, e, t), p(n, i, "ajaxComplete", [e, n]), m(n);
  }function b() {}function w(t) {
    return t && (t = t.split(";", 2)[0]), t && (t == f ? "html" : t == u ? "json" : s.test(t) ? "script" : a.test(t) && "xml") || "text";
  }function E(t, e) {
    return "" == e ? t : (t + "&" + e).replace(/[&?]{1,2}/, "?");
  }function j(e) {
    e.processData && e.data && "string" != t.type(e.data) && (e.data = t.param(e.data, e.traditional)), !e.data || e.type && "GET" != e.type.toUpperCase() || (e.url = E(e.url, e.data), e.data = void 0);
  }function S(e, n, i, r) {
    return t.isFunction(n) && (r = i, i = n, n = void 0), t.isFunction(i) || (r = i, i = void 0), { url: e, data: n, success: i, dataType: r };
  }function C(e, n, i, r) {
    var o,
        s = t.isArray(n),
        a = t.isPlainObject(n);t.each(n, function (n, u) {
      o = t.type(u), r && (n = i ? r : r + "[" + (a || "object" == o || "array" == o ? n : "") + "]"), !r && s ? e.add(u.name, u.value) : "array" == o || !i && "object" == o ? C(e, u, i, n) : e.add(n, u);
    });
  }var i,
      r,
      e = 0,
      n = window.document,
      o = /<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi,
      s = /^(?:text|application)\/javascript/i,
      a = /^(?:text|application)\/xml/i,
      u = "application/json",
      f = "text/html",
      c = /^\s*$/,
      l = n.createElement("a");l.href = window.location.href, t.active = 0, t.ajaxJSONP = function (i, r) {
    if (!("type" in i)) return t.ajax(i);var f,
        h,
        o = i.jsonpCallback,
        s = (t.isFunction(o) ? o() : o) || "jsonp" + ++e,
        a = n.createElement("script"),
        u = window[s],
        c = function c(e) {
      t(a).triggerHandler("error", e || "abort");
    },
        l = { abort: c };return r && r.promise(l), t(a).on("load error", function (e, n) {
      clearTimeout(h), t(a).off().remove(), "error" != e.type && f ? v(f[0], l, i, r) : y(null, n || "error", l, i, r), window[s] = u, f && t.isFunction(u) && u(f[0]), u = f = void 0;
    }), g(l, i) === !1 ? (c("abort"), l) : (window[s] = function () {
      f = arguments;
    }, a.src = i.url.replace(/\?(.+)=\?/, "?$1=" + s), n.head.appendChild(a), i.timeout > 0 && (h = setTimeout(function () {
      c("timeout");
    }, i.timeout)), l);
  }, t.ajaxSettings = { type: "GET", beforeSend: b, success: b, error: b, complete: b, context: null, global: !0, xhr: function xhr() {
      return new window.XMLHttpRequest();
    }, accepts: { script: "text/javascript, application/javascript, application/x-javascript", json: u, xml: "application/xml, text/xml", html: f, text: "text/plain" }, crossDomain: !1, timeout: 0, processData: !0, cache: !0 }, t.ajax = function (e) {
    var a,
        o = t.extend({}, e || {}),
        s = t.Deferred && t.Deferred();for (i in t.ajaxSettings) void 0 === o[i] && (o[i] = t.ajaxSettings[i]);d(o), o.crossDomain || (a = n.createElement("a"), a.href = o.url, a.href = a.href, o.crossDomain = l.protocol + "//" + l.host != a.protocol + "//" + a.host), o.url || (o.url = window.location.toString()), j(o);var u = o.dataType,
        f = /\?.+=\?/.test(o.url);if ((f && (u = "jsonp"), o.cache !== !1 && (e && e.cache === !0 || "script" != u && "jsonp" != u) || (o.url = E(o.url, "_=" + Date.now())), "jsonp" == u)) return f || (o.url = E(o.url, o.jsonp ? o.jsonp + "=?" : o.jsonp === !1 ? "" : "callback=?")), t.ajaxJSONP(o, s);var C,
        h = o.accepts[u],
        p = {},
        m = function m(t, e) {
      p[t.toLowerCase()] = [t, e];
    },
        x = /^([\w-]+:)\/\//.test(o.url) ? RegExp.$1 : window.location.protocol,
        S = o.xhr(),
        T = S.setRequestHeader;if ((s && s.promise(S), o.crossDomain || m("X-Requested-With", "XMLHttpRequest"), m("Accept", h || "*/*"), (h = o.mimeType || h) && (h.indexOf(",") > -1 && (h = h.split(",", 2)[0]), S.overrideMimeType && S.overrideMimeType(h)), (o.contentType || o.contentType !== !1 && o.data && "GET" != o.type.toUpperCase()) && m("Content-Type", o.contentType || "application/x-www-form-urlencoded"), o.headers)) for (r in o.headers) m(r, o.headers[r]);if ((S.setRequestHeader = m, S.onreadystatechange = function () {
      if (4 == S.readyState) {
        S.onreadystatechange = b, clearTimeout(C);var e,
            n = !1;if (S.status >= 200 && S.status < 300 || 304 == S.status || 0 == S.status && "file:" == x) {
          u = u || w(o.mimeType || S.getResponseHeader("content-type")), e = S.responseText;try {
            "script" == u ? (1, eval)(e) : "xml" == u ? e = S.responseXML : "json" == u && (e = c.test(e) ? null : t.parseJSON(e));
          } catch (i) {
            n = i;
          }n ? y(n, "parsererror", S, o, s) : v(e, S, o, s);
        } else y(S.statusText || null, S.status ? "error" : "abort", S, o, s);
      }
    }, g(S, o) === !1)) return S.abort(), y(null, "abort", S, o, s), S;if (o.xhrFields) for (r in o.xhrFields) S[r] = o.xhrFields[r];var N = "async" in o ? o.async : !0;S.open(o.type, o.url, N, o.username, o.password);for (r in p) T.apply(S, p[r]);return o.timeout > 0 && (C = setTimeout(function () {
      S.onreadystatechange = b, S.abort(), y(null, "timeout", S, o, s);
    }, o.timeout)), S.send(o.data ? o.data : null), S;
  }, t.get = function () {
    return t.ajax(S.apply(null, arguments));
  }, t.post = function () {
    var e = S.apply(null, arguments);return e.type = "POST", t.ajax(e);
  }, t.getJSON = function () {
    var e = S.apply(null, arguments);return e.dataType = "json", t.ajax(e);
  }, t.fn.load = function (e, n, i) {
    if (!this.length) return this;var a,
        r = this,
        s = e.split(/\s/),
        u = S(e, n, i),
        f = u.success;return s.length > 1 && (u.url = s[0], a = s[1]), u.success = function (e) {
      r.html(a ? t("<div>").html(e.replace(o, "")).find(a) : e), f && f.apply(r, arguments);
    }, t.ajax(u), this;
  };var T = encodeURIComponent;t.param = function (e, n) {
    var i = [];return i.add = function (e, n) {
      t.isFunction(n) && (n = n()), null == n && (n = ""), this.push(T(e) + "=" + T(n));
    }, C(i, e, n), i.join("&").replace(/%20/g, "+");
  };
})(Zepto), (function (t) {
  t.fn.serializeArray = function () {
    var e,
        n,
        i = [],
        r = function r(t) {
      return t.forEach ? t.forEach(r) : void i.push({ name: e, value: t });
    };return this[0] && t.each(this[0].elements, function (i, o) {
      n = o.type, e = o.name, e && "fieldset" != o.nodeName.toLowerCase() && !o.disabled && "submit" != n && "reset" != n && "button" != n && "file" != n && ("radio" != n && "checkbox" != n || o.checked) && r(t(o).val());
    }), i;
  }, t.fn.serialize = function () {
    var t = [];return this.serializeArray().forEach(function (e) {
      t.push(encodeURIComponent(e.name) + "=" + encodeURIComponent(e.value));
    }), t.join("&");
  }, t.fn.submit = function (e) {
    if (0 in arguments) this.bind("submit", e);else if (this.length) {
      var n = t.Event("submit");this.eq(0).trigger(n), n.isDefaultPrevented() || this.get(0).submit();
    }return this;
  };
})(Zepto), (function (t) {
  "__proto__" in {} || t.extend(t.zepto, { Z: function Z(e, n) {
      return e = e || [], t.extend(e, t.fn), e.selector = n || "", e.__Z = !0, e;
    }, isZ: function isZ(e) {
      return "array" === t.type(e) && "__Z" in e;
    } });try {
    getComputedStyle(void 0);
  } catch (e) {
    var n = getComputedStyle;window.getComputedStyle = function (t) {
      try {
        return n(t);
      } catch (e) {
        return null;
      }
    };
  }
})(Zepto);

//     Zepto.js
//     (c) 2010-2016 Thomas Fuchs
//     Zepto.js may be freely distributed under the MIT license.
//
//     https://github.com/madrobby/zepto/blob/master/src/selector.js

!(function (t) {
  function n(n) {
    return n = t(n), !(!n.width() && !n.height()) && "none" !== n.css("display");
  }function e(t, n) {
    t = t.replace(/=#\]/g, '="#"]');var e,
        i,
        r = u.exec(t);if (r && r[2] in s && (e = s[r[2]], i = r[3], t = r[1], i)) {
      var o = Number(i);i = isNaN(o) ? i.replace(/^["']|["']$/g, "") : o;
    }return n(t, e, i);
  }var i = t.zepto,
      r = i.qsa,
      o = i.matches,
      s = t.expr[":"] = { visible: function visible() {
      return n(this) ? this : void 0;
    }, hidden: function hidden() {
      return n(this) ? void 0 : this;
    }, selected: function selected() {
      return this.selected ? this : void 0;
    }, checked: function checked() {
      return this.checked ? this : void 0;
    }, parent: function parent() {
      return this.parentNode;
    }, first: function first(t) {
      return 0 === t ? this : void 0;
    }, last: function last(t, n) {
      return t === n.length - 1 ? this : void 0;
    }, eq: function eq(t, n, e) {
      return t === e ? this : void 0;
    }, contains: function contains(n, e, i) {
      return t(this).text().indexOf(i) > -1 ? this : void 0;
    }, has: function has(t, n, e) {
      return i.qsa(this, e).length ? this : void 0;
    } },
      u = new RegExp("(.*):(\\w+)(?:\\(([^)]+)\\))?$\\s*"),
      c = /^\s*>/,
      h = "Zepto" + +new Date();i.qsa = function (n, o) {
    return e(o, function (e, s, u) {
      try {
        var a;!e && s ? e = "*" : c.test(e) && (a = t(n).addClass(h), e = "." + h + " " + e);var f = r(n, e);
      } catch (d) {
        throw (console.error("error performing selector: %o", o), d);
      } finally {
        a && a.removeClass(h);
      }return s ? i.uniq(t.map(f, function (t, n) {
        return s.call(t, n, f, u);
      })) : f;
    });
  }, i.matches = function (t, n) {
    return e(n, function (n, e, i) {
      return (!n || o(t, n)) && (!e || e.call(t, null, i) === t);
    });
  };
})(Zepto);

},{}]},{},[49])

//# sourceMappingURL=index.js.map
