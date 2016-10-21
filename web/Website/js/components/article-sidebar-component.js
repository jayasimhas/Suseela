var articleSidebarAd,
    articleSidebarAdParent,
    lastActionFlagsBar,
    stickyFloor,
    sidebarIsTaller;
$(document).ready(function () {
    articleSidebarAdParent = $('.article-right-rail section:last-child');
    articleSidebarAd = articleSidebarAdParent.find('.advertising');
    lastActionFlagsBar = $('.action-flags-bar:last-of-type');
    sidebarIsTaller = $('.article-right-rail').height() > $('.article-left-rail').height();
});
$(window).on('scroll', function () {
    if (articleSidebarAdParent && articleSidebarAdParent.length && !sidebarIsTaller) {
        // pageYOffset instead of scrollY for IE / pre-Edge compatibility
        stickyFloor = lastActionFlagsBar.offset().top - window.pageYOffset - articleSidebarAd.height();
        if (articleSidebarAdParent.offset().top - window.pageYOffset <= 16) {
            articleSidebarAdParent.addClass('advertising--sticky');
        } else {
            articleSidebarAdParent.removeClass('advertising--sticky');
        }
        if (stickyFloor <= 40) {
            articleSidebarAd.css('top', (stickyFloor - 40) + 'px');
        } else {
            articleSidebarAd.css('top', '');
        }
    }
});