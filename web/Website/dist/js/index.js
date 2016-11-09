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
                        e.preventDefault();
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
                        if (e.target.className == 'pull-left' || e.target.className == 'wd-15') {
                            checkTouchType = true;
                            e.preventDefault();
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

},{}],4:[function(require,module,exports){
'use strict';

var articleSidebarAd, articleSidebarAdParent, lastActionFlagsBar, stickyFloor, sidebarIsTaller;
$(document).ready(function () {
    articleSidebarAdParent = $('.article-right-rail section:last-child');
    articleSidebarAd = articleSidebarAdParent.find('.advertising');
    lastActionFlagsBar = $('.action-flags-bar:last-of-type');
    sidebarIsTaller = $('.article-right-rail').height() > $('.article-left-rail').height();
});
$(window).on('scroll', function () {
    if (articleSidebarAdParent.length && !sidebarIsTaller) {
        // pageYOffset instead of scrollY for IE / pre-Edge compatibility
        stickyFloor = lastActionFlagsBar.offset().top - window.pageYOffset - articleSidebarAd.height();
        if (articleSidebarAdParent.offset().top - window.pageYOffset <= 16) {
            articleSidebarAdParent.addClass('advertising--sticky');
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

},{}],5:[function(require,module,exports){
'use strict';

function setClsforFlw(t) {
	for (var i = 0; i < t.length; i++) {
		var tableFlwrow = $(t[i]).find('.followrow.disabled:eq(0)');
		tableFlwrow.addClass('frow');
	}
}

function createJSONData(alltables, UserPreferences) {
	for (var i = 0; i < alltables.length; i++) {
		var currenttabtrs = $(alltables[i]).find('tbody tr'),
		    pubPanPosition = $(alltables[i]).closest('.publicationPan').attr('data-row'),
		    tableId = $(alltables[i]).attr('id'),
		    publicationName = $(alltables[i]).find('h2').attr('data-publication'),
		    subscribeStatus = $(alltables[i]).find('.subscribed').html(),
		    channelId = $(alltables[i]).find('h2').attr('data-item-id'),
		    channelStatus = $(alltables[i]).find('h2').attr('data-item-status');
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
		UserPreferences.PreferredChannels.push({ "ChannelCode": publicationName, "ChannelOrder": pubPanPosition, "IsFollowing": channelStatus, "ChannelId": channelId, Topics: alltdata });
	}
	sendHttpRequest(UserPreferences);
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
					$(window).scrollTop($('.alert-success').offset().top);
				}
				if (setFlag == 'register' && redirectUrl == 'href') {
					window.location.href = $('.registrationBtn').attr('href');
				} else if (setFlag == 'register' && redirectUrl == 'name') {
					window.location.href = $('.registrationBtn').attr('name');
				}
			} else {
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
		    tbody = curpublicPan.find('tbody'),
		    div = $this.closest('div'),
		    $lgfollow = curpublicPan.find('.followBtn'),
		    table = $('.table');
		$this.addClass('hideBtn');
		$('#validatePreference').val(1);
		div.find('.unfollowAllBtn').removeClass('hideBtn');
		curpublicPan.find('.firstrow .lableStatus').val('followinglbl');
		curpublicPan.find('.accordionStatus .lableStatus').val('followinglbl');
		$lgfollow.addClass('followingBtn').removeClass('followBtn').html($('#followingButtonText').val());
		$('#validatePriority').val(true);
		$('#validateMyViewPriority').val(true);
		for (var i = 0; i < tbody.find('.followingBtn').length; i++) {
			$(tbody.find('.followrow')[i]).attr('draggable', true);
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
		    tbody = curpublicPan.find('tbody'),
		    div = $this.closest('div'),
		    $lgfollowing = curpublicPan.find('.followingBtn');
		$this.addClass('hideBtn');
		$this.closest('.smfollowingBtn').find('.followAllBtn').addClass('fr');
		$('#validatePreference').val(1);
		div.find('.followAllBtn').removeClass('hideBtn');
		curpublicPan.find('.firstrow .lableStatus').val('followlbl');
		curpublicPan.find('.accordionStatus .lableStatus').val('followlbl');
		$lgfollowing.addClass('followBtn').removeClass('followingBtn').html($('#followButtonText').val());
		$('#validatePriority').val(false);
		$('#validateMyViewPriority').val(true);
		for (var i = 0; i < $lgfollowing.length; i++) {
			$($lgfollowing[i], curpublicPan).closest('tr').removeAttr('class').addClass('followrow disabled');
		}
		sort_table(tbody, 0, 1, 'followrow');
	});

	$('#allPublicationsPan .donesubscribe').on('click', '.followrow .followBtn', function () {
		var $this = $(this),
		    followrow = $this.closest('.followrow'),
		    table = $this.closest('.table'),
		    followAllBtnHS = table.find('.hidden-large .followAllBtn'),
		    followAllBtnHL = table.find('.hidden-xs .followAllBtn'),
		    unfollowAllBtnHS = table.find('.hidden-large .unfollowAllBtn'),
		    unfollowAllBtnHL = table.find('.hidden-xs .unfollowAllBtn'),
		    trs = $this.closest('tbody').find('tr'),
		    trsfollowing = $this.closest('tbody').find('tr.followingrow');
		followrow.attr('draggable', true);
		$('#validatePreference').val(1);
		followrow.addClass('followingrow').removeClass('followrow disabled frow');
		$this.addClass('followingBtn').removeClass('followBtn').html($('#followingButtonText').val());
		setClsforFlw(table);
		table.find('.firstrow .lableStatus').val('followinglbl');
		table.find('.accordionStatus .lableStatus').val('followinglbl');
		$('#validateMyViewPriority').val(true);

		if (trs.hasClass('followingrow')) {
			$('#validatePriority').val(true);
			unfollowAllBtnHS.addClass('hideBtn');
		}

		if ($('.followrow.disabled.frow', table).length) {
			followrow.appendTo(followrow.clone().insertBefore(table.find('.followrow.disabled.frow')));
		} else {
			followrow.clone().appendTo($this.closest('tbody'));
		}
		followrow.remove();
		if (trs.length === trsfollowing.length + 1) {
			followAllBtnHL.addClass('hideBtn');
			unfollowAllBtnHL.removeClass('hideBtn');

			followAllBtnHS.addClass('hideBtn');
			unfollowAllBtnHS.removeClass('hideBtn');
		} else {
			followAllBtnHL.removeClass('hideBtn');
			unfollowAllBtnHL.removeClass('hideBtn');

			followAllBtnHS.removeClass('hideBtn');
			unfollowAllBtnHS.addClass('hideBtn');
		}
	});

	$('#allPublicationsPan .donesubscribe').on('click', '.followingrow .followingBtn', function () {
		var $this = $(this),
		    table = $this.closest('table'),
		    followAllBtnHS = table.find('.hidden-large .followAllBtn'),
		    followAllBtnHL = table.find('.hidden-xs .followAllBtn'),
		    unfollowAllBtnHS = table.find('.hidden-large .unfollowAllBtn'),
		    unfollowAllBtnHL = table.find('.hidden-xs .unfollowAllBtn'),
		    followingrow = $this.closest('.followingrow'),
		    tbody = $this.closest('tbody'),
		    trs = $this.closest('tbody').find('tr'),
		    disabledtrs = $this.closest('tbody').find('.followrow.disabled'),
		    trsfollow = $this.closest('tbody').find('tr.followrow');
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
		}
		if (trs.length === trsfollow.length + 1) {
			unfollowAllBtnHL.addClass('hideBtn');
			followAllBtnHL.removeClass('hideBtn');

			unfollowAllBtnHS.removeClass('hideBtn');
			followAllBtnHS.addClass('hideBtn');
			$('#validatePriority').val(false);
		} else {
			followAllBtnHL.removeClass('hideBtn');
			unfollowAllBtnHL.removeClass('hideBtn');

			unfollowAllBtnHS.addClass('hideBtn');
			followAllBtnHS.removeClass('hideBtn');
		}
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
			$(window).scrollTop(position.top - 20);
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
			$(window).scrollTop(position.top - 20);

			for (var i = 0; i < allpubpans.length; i++) {
				var labelVal = $(allpubpans[i]).find('.firstrow .lableStatus').val();
				$('.' + labelVal, allpubpans[i]).removeClass('hideBtn');
			}
			thead.find('.mtp').addClass('hideBtn');
		}
	});

	$('.publicationPan').on('click', '.accordionImg .desktopMode', function () {
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
		    allpubpans = allPublications.find('.publicationPan');

		if ($this.hasClass('expanded')) {
			$this.removeClass('expanded');
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
			var position = $this.closest('.publicationPan').position();
			$(window).scrollTop(position.top);
		} else {
			allPublications.find('tbody').addClass('tbodyhidden');
			allPublications.find('.publicationPan .accordionImg span').removeClass('expanded');
			allPublications.find('.publicationPan thead tr').not(':nth-child(1)').addClass('hidden');
			allPublications.find('.publicationPan thead tr.showinview').removeClass('hidden');
			thead.find('tr').removeClass('hidden');
			$this.addClass('expanded');
			accCont.removeClass('tbodyhidden');
			tbody.removeClass('tbodyhidden');
			flwBtn.addClass('hideRow');
			flwlbl.removeClass('hideRow');

			for (var i = 0; i < allpubpans.length; i++) {
				var labelVal = $(allpubpans[i]).find('.firstrow .lableStatus').val();
				$('.' + labelVal, allpubpans[i]).removeClass('hideBtn');
			}
			thead.find('.mtp').addClass('hideBtn');

			var position = $this.closest('.publicationPan').position();
			$(window).scrollTop(position.top);
		}
	});

	var tables = $('.publicationPan table');
	setClsforFlw(tables);

	$('.saveview').click(function (e) {
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
		createJSONData(alltables, UserPreferences);

		$('#validatePreference').val(0);
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

	$('.close-modal, .cancel-modal').click(function () {
		$('.modal-overlay').removeClass('in');
		$('.modal-view').hide();
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
});

},{}],6:[function(require,module,exports){
'use strict';

var defaults = {
	totalCategories: 30,
	categoryLimit: 10,
	currentPage: 1,
	paginationEle: 'table'
},
    toVal = 1,
    fromVal = 1;

$.fn.setPagination = function (source) {
	$.extend(defaults, source);
};

function paginationCur(fv, tv) {
	if (defaults.paginationEle === 'table') {
		$('tbody.hidden-xs tr', '.page-account__table').hide();
		$('tbody.hidden-lg tr', '.page-account__table').hide();
		for (var i = fv; i < tv; i++) {
			$('tbody.hidden-xs tr', '.page-account__table').eq(i).show();
			$('tbody.hidden-lg tr', '.page-account__table').eq(i).show();
		}
	} else {
		$(defaults.paginationEle).hide();
		for (var i = fv; i < tv; i++) {
			$(defaults.paginationEle).eq(i).show();
		}
	}
}

$(function () {
	var showPageLinks = Math.ceil(defaults.totalCategories / defaults.categoryLimit);
	var linkStr = '';
	for (var i = 1; i <= showPageLinks; i++) {
		linkStr += '<a href="javascript:void(0);">' + i + '</a>';
	}
	if (showPageLinks > 1) {
		$('.pagination span').html(linkStr);
	} else {
		$('.pagination').hide();
	}

	$('.pagination a').click(function () {
		var $this = $(this),
		    $val = $this.html();
		if ($val.toLowerCase().indexOf('prev') >= 0) {
			var idx = +$('.pagination span a.active').html() - 1,
			    toVal = idx * defaults.categoryLimit,
			    fromVal = toVal - defaults.categoryLimit;

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
			    toVal = idx * defaults.categoryLimit,
			    fromVal = toVal - defaults.categoryLimit;

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
				toVal = defaults.categoryLimit * $val;
				fromVal = toVal - defaults.categoryLimit;
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
				paginationCur(0, defaults.categoryLimit);
			}
		}, 1);
	});
});

},{}],7:[function(require,module,exports){
"use strict";

function loadLayoutOneData(data, idx) {
	var loadData = loadPreferanceId["Sections"][idx]["ChannelName"] ? '<div class="latestSubject clearfix"><span class="sub">' + data.loadMore.latestFromText + ' ' + loadPreferanceId["Sections"][idx]["ChannelName"] + '</span><a class="editView mobview click-utag" href="' + loadPreferanceId.MyViewSettingsPageLink + '" data-info="{"event_name":"edit_my_view","page_name":"' + analytics_data["page_name"] + '","ga_eventCategory":"My View Page Link","ga_eventAction":"Link Click","ga_eventLabel":"EDIT MY VIEW"}">EDIT MY VIEW</a></div>' : '',
	    loadmoreLink = data.loadMore && data.loadMore.displayLoadMore ? data.loadMore.loadMoreLinkUrl : '#';
	loadData += '<div class="eachstoryMpan">';
	loadData += loadPreferanceId["Sections"][idx].ChannelId ? '<div class="eachstory layout1" id="' + loadPreferanceId["Sections"][idx].ChannelId + '">' : '';
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
	    bookmarkInfo = data.articles[0].isArticleBookmarked && data.articles[0].isArticleBookmarked ? data.articles[0].bookmarkedText : data.articles[0].bookmarkText;

	var articleData = '';
	articleData = '<section class="article-preview topic-featured-article">';
	articleData += data.articles[0].listableImage ? '<img class="topic-featured-article__image" src="' + data.articles[0].listableImage + '">' : '';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[0].id + '" data-analytics="{"bookmark": "' + bookmarkInfo + '", "bookmark_title": "' + data.articles[0].listableTitle + '", "bookmark_publication": "Scrip"}" data-is-bookmarked="' + data.articles[0].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + bookmarkInfo + '" data-label-bookmarked="' + bookmarkInfo + '">' + bookmarkInfo + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[0].listableDate ? '<li><time class="article-metadata__date">' + data.articles[0].listableDate + '</time></li>' : '';
	articleData += data.articles[0].linkableText ? '<li><h6>' + data.articles[0].linkableText + '</h6></li>' : '';
	articleData += data.articles[0].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="topic-featured-article__inner-wrapper">';
	articleData += data.articles[0].listableTitle ? '<h3 class="topic-featured-article__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info="{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + linkableUrl0 + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[0].listableTitle + '","publication_click":"' + analytics_data["publication"] + '"}">' + data.articles[0].listableTitle + '</a></h3>' : '';
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
	articleData += '<section class="article-preview article-preview--small mobview">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="bec798bb-d9df-40e6-be06-4bedda113621" data-analytics="{"bookmark": "true", "bookmark_title": "PD-1 Deep Dive: Lung Cancer Market Braced For Change", "bookmark_publication": "Scrip"}" data-is-bookmarked="true"><span class="action-flag__label js-bookmark-label" data-label-bookmark="Bookmarked" data-label-bookmarked="Bookmark">Bookmark</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[1].listableDate ? '<li><time class="article-metadata__date">' + data.articles[1].listableDate + '</time></li>' : '';
	articleData += data.articles[1].linkableText ? '<li><h6>' + data.articles[1].linkableText + '</h6></li>' : '';
	articleData += data.articles[1].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper">';
	articleData += data.articles[1].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl1 + '" class="click-utag" data-info="{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + linkableUrl1 + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[1].listableTitle + '","publication_click":"' + analytics_data["publication"] + '"}">' + data.articles[1].listableTitle + '</a></h1>' : '';
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

	articleData += '<section class="article-preview article-preview--small mobview">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="bec798bb-d9df-40e6-be06-4bedda113621" data-analytics="{"bookmark": "true", "bookmark_title": "PD-1 Deep Dive: Lung Cancer Market Braced For Change", "bookmark_publication": "Scrip"}" data-is-bookmarked="true"><span class="action-flag__label js-bookmark-label" data-label-bookmark="Bookmarked" data-label-bookmarked="Bookmark">Bookmark</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[2].listableDate ? '<li><time class="article-metadata__date">' + data.articles[2].listableDate + '</time></li>' : '';
	articleData += data.articles[2].linkableText ? '<li><h6>' + data.articles[2].linkableText + '</h6></li>' : '';
	articleData += data.articles[2].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper">';
	articleData += data.articles[2].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl2 + '" class="click-utag" data-info="{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + linkableUrl2 + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[2].listableTitle + '","publication_click":"' + analytics_data["publication"] + '"}">' + data.articles[2].listableTitle + '</a></h1>' : '';
	articleData += data.articles[2].listableAuthorByLine ? '<span class="article-preview__byline">' + data.articles[2].listableAuthorByLine + '</span>' : '';
	articleData += '<div class="article-summary">' + data.articles[1].listableSummary ? data.articles[1].listableSummary : '' + '</div>';
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

	articleData += '<section class="article-preview article-preview--small topics">';
	articleData += data.articles[3].linkableText ? '<h6>&nbsp;</h6>' : '';

	articleData += data.articles[3].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl3 + '" class="click-utag" data-info="{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + linkableUrl3 + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[3].listableTitle + '","publication_click":"' + analytics_data["publication"] + '"}">' + data.articles[3].listableTitle + '</a></h1>' : '';

	articleData += data.articles[4].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl4 + '" class="click-utag" data-info="{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + linkableUrl4 + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[4].listableTitle + '","publication_click":"' + analytics_data["publication"] + '"}">' + data.articles[4].listableTitle + '</a></h1>' : '';

	articleData += data.articles[5].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl5 + '" class="click-utag" data-info="{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + linkableUrl5 + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[5].listableTitle + '","publication_click":"' + analytics_data["publication"] + '"}">' + data.articles[5].listableTitle + '</a></h1>' : '';

	articleData += '</section>';
	articleData += '</div>';

	articleData += '<div class="latest-news__articles">';
	articleData += '<section class="article-preview article-small-preview mobview">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="bec798bb-d9df-40e6-be06-4bedda113621" data-analytics="{"bookmark": "true", "bookmark_title": "PD-1 Deep Dive: Lung Cancer Market Braced For Change", "bookmark_publication": "Scrip"}" data-is-bookmarked="true"><span class="action-flag__label js-bookmark-label" data-label-bookmark="Bookmarked" data-label-bookmarked="Bookmark">Bookmark</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[6].listableDate ? '<li><time class="article-metadata__date">' + data.articles[6].listableDate + '</time></li>' : '';
	articleData += data.articles[6].linkableText ? '<li><h6>' + data.articles[6].linkableText + '</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += data.articles[6].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl6 + '" class="click-utag" data-info="{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + linkableUrl6 + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[6].listableTitle + '","publication_click":"' + analytics_data["publication"] + '"}">' + data.articles[6].listableTitle + '</a></h1>' : '';
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

	articleData += '<section class="article-preview article-small-preview mobview">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="bec798bb-d9df-40e6-be06-4bedda113621" data-analytics="{"bookmark": "true", "bookmark_title": "PD-1 Deep Dive: Lung Cancer Market Braced For Change", "bookmark_publication": "Scrip"}" data-is-bookmarked="true"><span class="action-flag__label js-bookmark-label" data-label-bookmark="Bookmarked" data-label-bookmarked="Bookmark">Bookmark</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[7].listableDate ? '<li><time class="article-metadata__date">' + data.articles[7].listableDate + '</time></li>' : '';
	articleData += data.articles[7].linkableText ? '<li><h6>' + data.articles[7].linkableText + '</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += data.articles[7].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl7 + '" class="click-utag" data-info="{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + linkableUrl7 + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[7].listableTitle + '","publication_click":"' + analytics_data["publication"] + '"}">' + data.articles[7].listableTitle + '</a></h1>' : '';
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

	articleData += '<section class="article-preview article-small-preview mobview">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="bec798bb-d9df-40e6-be06-4bedda113621" data-analytics="{"bookmark": "true", "bookmark_title": "PD-1 Deep Dive: Lung Cancer Market Braced For Change", "bookmark_publication": "Scrip"}" data-is-bookmarked="true"><span class="action-flag__label js-bookmark-label" data-label-bookmark="Bookmarked" data-label-bookmarked="Bookmark">Bookmark</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[8].listableDate ? '<li><time class="article-metadata__date">' + data.articles[8].listableDate + '</time></li>' : '';
	articleData += data.articles[8].linkableText ? '<li><h6>' + data.articles[8].linkableText + '</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += data.articles[8].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl8 + '" class="click-utag" data-info="{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + linkableUrl8 + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[8].listableTitle + '","publication_click":"' + analytics_data["publication"] + '"}">' + data.articles[8].listableTitle + '</a></h1>' : '';
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

function loadLayoutTwoData(data, idx) {
	var loadData = loadPreferanceId["Sections"][idx]["ChannelName"] ? '<div class="latestSubject clearfix"><span class="sub">' + data.loadMore.latestFromText + ' ' + loadPreferanceId["Sections"][idx]["ChannelName"] + '</span><a class="editView mobview click-utag"  href="' + loadPreferanceId.MyViewSettingsPageLink + '" data-info="{"event_name":"edit_my_view","page_name":"' + analytics_data["page_name"] + '","ga_eventCategory":"My View Page Link","ga_eventAction":"Link Click","ga_eventLabel":"EDIT MY VIEW"}">EDIT MY VIEW</a></div>' : '',
	    loadmoreLink = data.loadMore && data.loadMore.displayLoadMore && data.loadMore.displayLoadMore.loadMoreLinkUrl ? data.loadMore.displayLoadMore.loadMoreLinkUrl : '#';
	loadData += '<div class="eachstoryMpan">';
	loadData += loadPreferanceId["Sections"][idx].ChannelId ? '<div class="eachstory layout2" id="' + loadPreferanceId["Sections"][idx].ChannelId + '">' : '';
	loadData += createLayoutInner2(data);
	loadData += '</div>';

	loadData += data.loadMore && data.loadMore.displayLoadMore ? '<div class="loadmore"><span href="' + loadmoreLink + '" data-info="{"event_name":"load_more","page_name":"' + analytics_data["page_name"] + '","ga_eventCategory":"My View Page Publications","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + [idx]["ChannelName"] + '","publication_click":"' + analytics_data["publication"] + '"}" class="click-utag">' + data.loadMore.loadMoreLinkText + ' ' + loadPreferanceId["Sections"][idx]["ChannelName"] + '</span></div>' : '';

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
	    linkableUrl8 = data.articles[8].linkableUrl ? data.articles[8].linkableUrl : '#';

	var articleData = '<div class="latest-news__articles">';
	articleData += '<section class="article-preview article-preview--small preview2">';
	articleData += data.articles[0].listableImage ? '<img class="topic-featured-article__image2 hidden-xs" src="' + data.articles[0].listableImage + '">' : '';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="bec798bb-d9df-40e6-be06-4bedda113621" data-analytics="{"bookmark": "true", "bookmark_title": "PD-1 Deep Dive: Lung Cancer Market Braced For Change", "bookmark_publication": "Scrip"}" data-is-bookmarked="true"><span class="action-flag__label js-bookmark-label" data-label-bookmark="Bookmarked" data-label-bookmarked="Bookmark">Bookmark</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[0].listableDate ? '<li><time class="article-metadata__date">' + data.articles[0].listableDate + '</time></li>' : '';
	articleData += data.articles[0].linkableText ? '<li><h6>' + data.articles[0].linkableText + '</h6></li>' : '';
	articleData += data.articles[0].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += data.articles[0].listableImage ? '<img class="topic-featured-article__image2 hidden-xs" src="' + data.articles[0].listableImage + '">' : '';
	articleData += '<div class="article-preview__inner-wrapper">';
	articleData += data.articles[0].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info="{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + linkableUrl0 + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[0].listableTitle + '","publication_click":"' + analytics_data["publication"] + '"}">' + data.articles[0].listableTitle + '</a></h1>' : '';
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

	articleData += '<section class="article-preview article-preview--small mobview artheight">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="bec798bb-d9df-40e6-be06-4bedda113621" data-analytics="{"bookmark": "true", "bookmark_title": "PD-1 Deep Dive: Lung Cancer Market Braced For Change", "bookmark_publication": "Scrip"}" data-is-bookmarked="true"><span class="action-flag__label js-bookmark-label" data-label-bookmark="Bookmarked" data-label-bookmarked="Bookmark">Bookmark</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[1].listableDate ? '<li><time class="article-metadata__date">' + data.articles[1].listableDate + '</time></li>' : '';
	articleData += data.articles[1].linkableText ? '<li><h6>' + data.articles[1].linkableText + '</h6></li>' : '';
	articleData += data.articles[1].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper">';
	articleData += data.articles[1].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl1 + '" class="click-utag" data-info="{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + linkableUrl1 + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[1].listableTitle + '","publication_click":"' + analytics_data["publication"] + '"}">' + data.articles[1].listableTitle + '</a></h1>' : '';
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

	articleData += '<section class="article-preview article-preview--small artheight topics">';
	articleData += data.articles[2].linkableText ? '<h6>&nbsp;</h6>' : '';

	articleData += data.articles[2].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl2 + '" class="click-utag" data-info="{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + linkableUrl2 + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[2].listableTitle + '","publication_click":"' + analytics_data["publication"] + '"}">' + data.articles[2].listableTitle + '</a></h1>' : '';

	articleData += data.articles[3].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl3 + '" class="click-utag" data-info="{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + linkableUrl3 + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[3].listableTitle + '","publication_click":"' + analytics_data["publication"] + '"}">' + data.articles[3].listableTitle + '</a></h1>' : '';

	articleData += data.articles[4].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl4 + '" class="click-utag" data-info="{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + linkableUrl4 + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[4].listableTitle + '","publication_click":"' + analytics_data["publication"] + '"}">' + data.articles[4].listableTitle + '</a></h1>' : '';
	articleData += '</section>';
	articleData += '</div>';

	articleData += '<div class="latest-news__articles">';
	articleData += '<section class="article-preview article-small-preview mobview">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="bec798bb-d9df-40e6-be06-4bedda113621" data-analytics="{"bookmark": "true", "bookmark_title": "PD-1 Deep Dive: Lung Cancer Market Braced For Change", "bookmark_publication": "Scrip"}" data-is-bookmarked="true"><span class="action-flag__label js-bookmark-label" data-label-bookmark="Bookmarked" data-label-bookmarked="Bookmark">Bookmark</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[5].listableDate ? '<li><time class="article-metadata__date">' + data.articles[5].listableDate + '</time></li>' : '';
	articleData += data.articles[5].linkableText ? '<li><h6>' + data.articles[5].linkableText + '</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += data.articles[5].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl5 + '" class="click-utag" data-info="{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + linkableUrl5 + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[5].listableTitle + '","publication_click":"' + analytics_data["publication"] + '"}">' + data.articles[5].listableTitle + '</a></h1>' : '';
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

	articleData += '<section class="article-preview article-preview--small artheight mobview mtop">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="bec798bb-d9df-40e6-be06-4bedda113621" data-analytics="{"bookmark": "true", "bookmark_title": "PD-1 Deep Dive: Lung Cancer Market Braced For Change", "bookmark_publication": "Scrip"}" data-is-bookmarked="true"><span class="action-flag__label js-bookmark-label" data-label-bookmark="Bookmarked" data-label-bookmarked="Bookmark">Bookmark</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[6].listableDate ? '<li><time class="article-metadata__date">' + data.articles[6].listableDate + '</time></li>' : '';
	articleData += data.articles[6].linkableText ? '<li><h6>' + data.articles[6].linkableText + '</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper">';
	articleData += data.articles[6].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl6 + '" class="click-utag" data-info="{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + linkableUrl6 + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[6].listableTitle + '","publication_click":"' + analytics_data["publication"] + '"}">' + data.articles[6].listableTitle + '</a></h1>' : '';
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

	articleData += '<section class="article-preview article-small-preview sm-article sm-articles mtop">';
	articleData += '<section class="sm-article mobview">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="bec798bb-d9df-40e6-be06-4bedda113621" data-analytics="{"bookmark": "true", "bookmark_title": "PD-1 Deep Dive: Lung Cancer Market Braced For Change", "bookmark_publication": "Scrip"}" data-is-bookmarked="true"><span class="action-flag__label js-bookmark-label" data-label-bookmark="Bookmarked" data-label-bookmarked="Bookmark">Bookmark</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[7].listableDate ? '<li><time class="article-metadata__date">' + data.articles[7].listableDate + '</time></li>' : '';
	articleData += data.articles[7].linkableText ? '<li><h6>' + data.articles[7].linkableText + '</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += data.articles[7].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl7 + '" class="click-utag" data-info="{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + linkableUrl7 + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[7].listableTitle + '","publication_click":"' + analytics_data["publication"] + '"}">' + data.articles[7].listableTitle + '</a></h1>' : '';
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

	articleData += '<section class="sm-article mobview">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="bec798bb-d9df-40e6-be06-4bedda113621" data-analytics="{"bookmark": "true", "bookmark_title": "PD-1 Deep Dive: Lung Cancer Market Braced For Change", "bookmark_publication": "Scrip"}" data-is-bookmarked="true"><span class="action-flag__label js-bookmark-label" data-label-bookmark="Bookmarked" data-label-bookmarked="Bookmark">Bookmark</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[8].listableDate ? '<li><time class="article-metadata__date">' + data.articles[8].listableDate + '</time></li>' : '';
	articleData += data.articles[8].linkableText ? '<li><h6>' + data.articles[8].linkableText + '</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += data.articles[8].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl8 + '" class="click-utag" data-info="{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + linkableUrl8 + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[8].listableTitle + '","publication_click":"' + analytics_data["publication"] + '"}">' + data.articles[8].listableTitle + '</a></h1>' : '';
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

$(function () {
	var getLayoutInfo = $('#getLayoutInfo').val(),
	    layout1 = true,
	    loadLayoutData = '',
	    getLiIdx;
	if (typeof loadPreferanceId !== "undefined") {
		var loadDynData = loadPreferanceId["Sections"].length < loadPreferanceId.DefaultSectionLoadCount ? loadPreferanceId["Sections"].length : loadPreferanceId.DefaultSectionLoadCount;
		getLiIdx = loadPreferanceId.DefaultSectionLoadCount;
		for (var i = 0; i < loadDynData; i++) {
			var setId = loadPreferanceId["Sections"];
			if (setId.length) {
				(function (idx) {
					if (idx < loadPreferanceId["DefaultSectionLoadCount"]) {
						$.ajax({
							//url: '/api/articlesearch?pId=980D26EA-7B85-482D-8D8C-E7F43D6955B2&pno=1&psize=9',
							//url: '/api/articlesearch?pId='+ setId[idx]["TaxonomyIds"] + '&pno=1&psize=9',
							url: '/api/articlesearch',
							data: JSON.stringify({ 'TaxonomyIds': setId[idx]["TaxonomyIds"], 'PageNo': 1, 'PageSize': 9 }),
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
									} else {
										layout1 = true;
										loadLayoutData = loadLayoutTwoData(data, idx);
										$('.spinnerIcon').addClass('hidespin');
										$('.personalisationPan').append(loadLayoutData);
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
		    loadLayoutData;

		var layout = layoutCls.indexOf('layout1') !== -1 ? 'layout1' : 'layout2';
		var setId = loadPreferanceId["Sections"],
		    sendtaxonomyIdsArr = $this.closest('.eachstoryMpan').find('.getPaginationNum').attr('data-taxonomyIds').split(',');

		$.ajax({
			//url: '/loaddata.json?pId='+ eachstoryId + '&pno='+pageNum+'&psize='+pageSize,
			url: $this.closest('.eachstoryMpan').find('.getPaginationNum').attr('data-loadurl'),
			dataType: 'json',
			type: 'POST',
			data: JSON.stringify({ 'TaxonomyIds': sendtaxonomyIdsArr, 'PageNo': $this.closest('.eachstoryMpan').find('.getPaginationNum').attr('data-pageNo'), 'PageSize': $this.closest('.eachstoryMpan').find('.getPaginationNum').attr('data-pageSize') }),
			contentType: "application/json",
			success: function success(data) {
				if (data.articles && typeof data.articles === "object" && data.articles.length >= 9) {
					$this.closest('.eachstoryMpan').find('.getPaginationNum').attr({ 'data-taxonomyIds': data.loadMore.taxonomyIds, 'data-loadurl': data.loadMore.loadMoreLinkUrl, 'data-pageNo': data.loadMore.pageNo, 'data-pageSize': data.loadMore.pageSize });
					if (layout == 'layout1') {
						loadLayoutData = createLayoutInner1(data);
						$(eachstory).append(loadLayoutData);
					} else {
						loadLayoutData = createLayoutInner2(data);
						$(eachstory).append(loadLayoutData);
					}
				}
			},
			error: function error(xhr, errorType, _error2) {
				console.log('err ' + _error2);
			}
		});
	});

	var layout1Flag = true,
	    indx = 0,
	    eachstoryLength = typeof loadPreferanceId !== 'undefined' && loadPreferanceId.DefaultSectionLoadCount ? loadPreferanceId.DefaultSectionLoadCount : 0;
	$(window).scroll(function () {
		var eachstoryMpan = $('.personalisationPan .eachstoryMpan'),
		    eachstoryMpanLast = eachstoryMpan.last(),
		    layoutCls = eachstoryMpan.find('.eachstory').attr('class'),
		    contentHei = $('.personalisationPan').height(),
		    loadsection,
		    texonomyId;

		if ($(window).scrollTop() > contentHei - 400) {
			var getscrollData;

			if (typeof loadPreferanceId !== "undefined") {
				if (eachstoryLength < loadPreferanceId["Sections"].length) {
					eachstoryLength++;
					getLiIdx = eachstoryLength;
					loadsection = loadPreferanceId.DefaultSectionLoadCount + indx++;
					texonomyId = loadPreferanceId["Sections"][loadsection]["TaxonomyIds"];
				} else {
					return;
				}
			} else {
				return;
			}

			$.ajax({
				url: '/api/articlesearch',
				data: JSON.stringify({ 'TaxonomyIds': texonomyId, 'PageNo': 1, 'PageSize': 9 }),
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
						if ($('.eachstoryMpan', '.personalisationPan').length % 2 == 0 && layout1Flag) {
							layout1Flag = false;
							getscrollData = loadLayoutOneData(data, loadsection);
							$('.spinnerIcon').addClass('hidespin');
							$('.personalisationPan').append(getscrollData);
						} else {
							layout1Flag = true;
							getscrollData = loadLayoutTwoData(data, loadsection);
							$('.spinnerIcon').addClass('hidespin');
							$('.personalisationPan').append(getscrollData);
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
					for (var i = getLiIdx; i <= liIdx + 1; i++) {
						var setId = loadPreferanceId["Sections"];
						(function (idx) {
							$.ajax({
								url: '/api/articlesearch',
								dataType: 'json',
								data: JSON.stringify({ 'TaxonomyIds': loadPreferanceId["Sections"][idx]["TaxonomyIds"], 'PageNo': 1, 'PageSize': 9 }),
								type: 'POST',
								cache: false,
								async: false,
								beforeSend: function beforeSend() {
									$('.spinnerIcon').removeClass('hidespin');
								},
								success: function success(data) {
									if (data.articles && typeof data.articles === "object" && data.articles.length >= 9) {
										if (idx % 2 == 0) {
											loadLayoutData = loadLayoutOneData(data, idx);
											$('.personalisationPan').append(loadLayoutData);
										} else {
											loadLayoutData = loadLayoutTwoData(data, idx);
											$('.personalisationPan').append(loadLayoutData);
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
												$(window).scrollTop(getlatestPos.top - subjectHei);
											}
										}, 5);
									}
								}
							});
						})(i);
					}
				}
			}
		}
	});
});

},{}],8:[function(require,module,exports){
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

			var loginAnalytics = {
				event_name: 'login',
				login_state: 'successful',
				userName: '"' + $(form).find('input[name=username]').val() + '"'
			};
			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, loginAnalytics));

			// If Angular, need location.reload to force page refresh
			if (typeof angular !== 'undefined') {
				angular.element($('.search-results')[0]).controller().forceRefresh();
			} else {
				window.location.reload(false);
			}
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

			var event_data = {
				event_name: 'saved_search_alert_removal',
				saved_search_alert_title: $(form).data('analytics-title'),
				saved_search_alert_publication: $(form).data('analytics-publication')
			};

			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, event_data));
		}
	});
});

},{"../controllers/analytics-controller":10,"../controllers/form-controller":12,"../jscookie":20}],9:[function(require,module,exports){
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
            $(this).parents('.video-mini-container').find('.play-icon').hide();
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
            _videoMiniPlayBtnWrapper.hide();
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

},{}],10:[function(require,module,exports){
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

},{}],11:[function(require,module,exports){
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
                    DocumentID: bookmark.id
                },
                context: this,
                success: function success(response) {
                    if (response.success) {

                        if (bookmark.isBookmarking) {
                            (0, _analyticsController.analyticsEvent)($.extend(analytics_data, $(bookmark.elm).data('analytics')));
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

},{"./analytics-controller":10}],12:[function(require,module,exports){
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
					var captchaResponse = grecaptcha.getResponse();
					if (captchaResponse !== undefined) inputData['RecaptchaResponse'] = captchaResponse;

					if (!$(currentForm).data('on-submit')) {
						console.warn('No submit link for form');
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

},{}],13:[function(require,module,exports){
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

},{}],14:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, '__esModule', {
	value: true
});
function popOutController(triggerElm) {
	var _this = this;

	// Toggle pop-out when trigger is clicked
	if (triggerElm) {
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

},{}],15:[function(require,module,exports){
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

},{"./analytics-controller":10}],16:[function(require,module,exports){
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

},{"./analytics-controller":10}],17:[function(require,module,exports){
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

},{}],18:[function(require,module,exports){
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

},{"../calculatePopupOffsets.js":2}],19:[function(require,module,exports){
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

require('./components/article-sidebar-component');

require('./components/save-search-component');

require('./components/myview-settings');

require('./components/pagination');

require('./components/personalisation');

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

var AMchartUsingBuilder = function AMchartUsingBuilder() {
    if ($("#amchartDashboardBuilder").hasClass("amchart-dashboard-using-builder")) {

        alert(chartPresentation);

        AmCharts.makeChart("chartdiv", {
            "type": "serial",
            "dataProvider": chartData,
            "categoryField": "category",
            "graphs": [{ "balloonText": "[[title]] of [[category]]:[[value]]", "fillAlphas": 1, "id": "AmGraph-1", "title": "graph 1", "type": "column", "valueField": "column-1" }, { "balloonText": "[[title]] of [[category]]:[[value]]", "fillAlphas": 1, "id": "AmGraph-2", "title": "graph 2", "type": "column", "valueField": "column-2" }]
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
            createNewObj = {};
        for (prop in amchartVal) {
            if (prop != 'dataProvider') {
                createNewObj[prop] = amchartVal[prop];
            } else {
                createNewObj[prop] = chartDataVal;
            }
        }

        var chart = AmCharts.makeChart("chartdiv", createNewObj);
    }
    //messaging web users
    window.dismiss = function () {
        $('.dismiss').on('click', function () {
            _jscookie2['default'].set('dismiss_cookie', 'dismiss_cookie_created', '');
            $('.messaging_webUsers').remove();
            $('.messaging_webUsers_white').remove();
        });
    };
    window.dismiss();

    //Job Listing Pagination
    var JobsListingPagination = function JobsListingPagination() {
        var TotalCategories = $("#JobTilesCount").val();
        var CategoryLimit = $("#NoOfJobsPerPage").val();

        $('.pagination').setPagination({
            totalCategories: parseInt(TotalCategories),
            categoryLimit: parseInt(CategoryLimit),
            currentPage: 1,
            paginationEle: '.job_list_individual'
        });

        $('.pagination span a:eq(0)').click();
        $('.pagination a:eq(0)').removeAttr('href');
    };
    JobsListingPagination();

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
        $('.js-bookmark-article').on('click', function bookmarkArticle(e) {

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
    AMchartUsingBuilder();

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

        var eventDetails = {
            // event_name: "newsletter optins"
        };
        var chkDetails = {};
        if ($('#newsletters').is(':checked')) {
            chkDetails.newsletter_optin = "true";
            $.extend(eventDetails, chkDetails);
            (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, eventDetails));
        } else {
            chkDetails.newsletter_optin = "false";
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

    window.findTooltips = function () {
        $('.js-toggle-tooltip').each(function (index, item) {
            var tooltip;
            $(item).data("ttVisible", false);
            $(item).data("ttTouchTriggered", false);

            $(item).on('mouseenter touchstart', function (e) {
                e.preventDefault();

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
                tooltip.remove();
            });
        });
    };

    window.findTooltips();

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
});

},{"./DragDropTouch":1,"./carousel/zepto.data":3,"./components/article-sidebar-component":4,"./components/myview-settings":5,"./components/pagination":6,"./components/personalisation":7,"./components/save-search-component":8,"./components/video-mini":9,"./controllers/analytics-controller":10,"./controllers/bookmark-controller":11,"./controllers/form-controller":12,"./controllers/lightbox-modal-controller":13,"./controllers/pop-out-controller":14,"./controllers/register-controller":15,"./controllers/reset-password-controller":16,"./controllers/sortable-table-controller":17,"./controllers/tooltip-controller":18,"./jscookie":20,"./modal":21,"./newsletter-signup":22,"./search-page.js":23,"./selectivity-full":24,"./svg4everybody":25,"./toggle-icons":26,"./zepto.dragswap":27,"./zepto.min":28}],20:[function(require,module,exports){
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

},{}],21:[function(require,module,exports){
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

},{}],22:[function(require,module,exports){
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

    this.addControl = function (triggerElement, successCallback, failureCallback) {
        if (triggerElement) {
            $(triggerElement).on('click', function (event) {

                // Prevent form submit
                event.preventDefault();

                // Hide any errors
                $('.js-newsletter-signup-error').hide();

                var inputData = "";
                var url = $(triggerElement).data('signup-url');

                $(triggerElement).parents('.newsletter-signup').find('input').each(function () {
                    inputData = $(this).val();
                });

                url = url + '?userName=' + inputData;

                $.get(url, function (response) {
                    var newsletterAnalytics;

                    if (response) {

                        newsletterAnalytics = {
                            event_name: 'newsletter-signup',
                            newsletter_signup_state: 'successful',
                            userName: '"' + inputData + '"'
                        };

                        (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, newsletterAnalytics));

                        $(".newsletter-signup-before-submit").hide();
                        $(".newsletter-signup-after-submit").show();
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
            });
        }
    };
}

exports['default'] = newsletterSignupController;
module.exports = exports['default'];

},{"./controllers/analytics-controller":10}],23:[function(require,module,exports){
'use strict';

var SearchScript = (function () {

	/* Toggle search tips visibility */
	$('.js-toggle-search-tips').on('click', function toggleTips() {
		$('.search-bar__tips').toggleClass('open');
		$('.search-bar').toggleClass('tips-open');
	});
})();

},{}],24:[function(require,module,exports){
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
     */addToDom:function addToDom(){var $next;var $anchor=this.selectivity.$el;while(($next = $anchor.next('.selectivity-dropdown')).length) {$anchor = $next;}this.$el.insertAfter($anchor);}, /**
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

},{}],25:[function(require,module,exports){
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

},{}],26:[function(require,module,exports){
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

},{}],27:[function(require,module,exports){
/*!
 * Zepto HTML5 Drag and Drop Sortable
 * Author: James Doyle(@james2doyle) http://ohdoylerules.com
 * Repository: https://github.com/james2doyle/zepto-dragswap
 * Licensed under the MIT license
 */
'use strict';

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
            if (dragSrcEl != this) {
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

},{}],28:[function(require,module,exports){
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

},{}]},{},[19])


//# sourceMappingURL=index.js.map
