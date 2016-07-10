/*
 * Digress
 * Version 1.1.0
 *
 * Copyright(c) 2015 Samuel Fine <hi@samuelfine.com>
 * ISC Licensed
 *
 */

var tooltips = function() {

  // Break down `arguments` to our own `opts` object with default values
  // Hurry up, ES6!
  var opts = {
    position: (arguments[0] && arguments[0].position) || 'top'
  };

  // Find all triggers
  var triggers = document.getElementsByClassName('digress-trigger');
  var docElm = document.documentElement; // Prettier/shorter var name

  for(var i = 0; i < triggers.length; i++) {

    var trig = {
      e: triggers[i] // HTML element
    };

    // getBoundingClientRect() for viewport offset
    // scrollTop for document offset
    // parseInt() to remove any CSS values `100px`, `100rem`, etc.
    trig.top = trig.e.getBoundingClientRect().top + docElm.scrollTop;
    trig.left = trig.e.getBoundingClientRect().left + docElm.scrollLeft;
    trig.width = parseInt(window.getComputedStyle(trig.e).width);
    trig.height = parseInt(window.getComputedStyle(trig.e).height);
    trig.position = trig.e.getAttribute('data-digress-position') || opts.position;
    trig.note = trig.e.getAttribute('data-digress-trigger');

    var note = {};
    if(document.getElementById(trig.note)) {
      note.e = document.getElementById(trig.note);
    } else {
      console.warn('DIGRESS: Cannot find note for trigger.', trig.e);
      break;
    }
    note.width = parseInt(window.getComputedStyle(note.e).width);
    note.height = parseInt(window.getComputedStyle(note.e).height);

    if(trig.position === 'right') {
      note.e.style.top = trig.top + (trig.height / 2) - (note.height / 2) + 'px';
      note.e.style.left = trig.left + trig.width + 'px';
    } else if (trig.position === 'bottom') {
      note.e.style.top = trig.top + trig.height + 'px';
      note.e.style.left = trig.left - (note.width / 2) + (trig.width / 2) + 'px';
    } else if (trig.position === 'left') {
      note.e.style.top = trig.top + (trig.height / 2) - (note.height / 2) + 'px';
      note.e.style.left = trig.left - note.width + 'px';
    } else { // Default to top
      note.e.style.top = trig.top - note.height + 'px';
      note.e.style.left = trig.left - (note.width / 2) + (trig.width / 2) + 'px';
    }

    trig.e.addEventListener('click', function() {
      this.classList.toggle('active');
      document.getElementById(this.getAttribute('data-digress-trigger')).classList.toggle('visible');
    }, false);
  }

};

export { tooltips };
