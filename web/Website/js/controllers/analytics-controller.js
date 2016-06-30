// * * *
//  ANALYTICS CONTROLLER
//  For ease-of-use, better DRY, better prevention of JS errors when ads are blocked
// * * *

function analyticsEvent(dataObj) {
    if(typeof utag !== 'undefined') {
        utag.link(dataObj);
    }
};

export { analyticsEvent };
