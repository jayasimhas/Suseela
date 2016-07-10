var toggleIcons = function(container) {

    if(!container) console.warn('toggleIcons missing container element');

    $(container).find('.toggleable-icon').each(function(indx, item) {
        $(item).hasClass('is-visible') ? $(item).removeClass('is-visible') : $(item).addClass('is-visible');
    });

};

export { toggleIcons };
