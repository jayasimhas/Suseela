/* global analyticsEvent, analytics_data, angular */
import FormController from '../controllers/form-controller';
import Cookies from '../jscookie';
import { analyticsEvent } from '../controllers/analytics-controller';

/* * *
SAVE SEARCH
This component handles saving searches from the Search page, as well as setting alerts
for topics from Home/Topic pages. Dispite the naming differences, the back-end functionality
is the same - topic alerts are actually just saved searches for the topic,
plus an email alert for new articles.
* * */

$(document).ready(function() {

	// When the Save Search pop-out is toggled, need to update some form fields
	// with the most recent data. Used to use Angular for this, but for site-wide
	// reusability we need to do it in Zepto.
	$('.js-save-search').on('click', function(e) {
		$('.js-save-search-url').val(window.location.pathname + window.location.hash);
		$('.js-save-search-title').val($('#js-search-field').val());
	});

	// Populates topic alert data when a user is logging in and saving simultaneously
	$('.js-update-topic-alert').on('click', function(e) {
		$('.js-save-search-url').val($(this).data('topic-alert-url'));
		// Search/Topic title exists as <input> and <span>, needs two techniques to properly
		// update the values.
		$('.js-save-search-title').val($(this).data('topic-alert-title')).html($(this).data('topic-alert-title'));
	});

	$('.js-set-topic-alert').on('click', function(e) {

		var isSettingAlert = !$(this).data('has-topic-alert');
		var topicLabel = $(this).find('.js-set-topic-label');

		$('.js-save-search-url').val($(this).data('topic-alert-url'));
		$('.js-save-search-title').val($(this).data('topic-alert-title'));

		if(isSettingAlert) {
			$('.form-save-search').find('button[type=submit]').click();
			topicLabel.html(topicLabel.data('label-is-set'));
			$(this).data('has-topic-alert', 'true');
			$(this).find('.js-topic-icon-unset').removeClass('is-active');
			$(this).find('.js-topic-icon-set').addClass('is-active');
		} else {
			window.lightboxController.showLightbox($(this));
		}

	});

	var removeTopicAlert = new FormController({
		observe: '.form-remove-topic-alert',
		successCallback: function(form, context, event) {
			$(form).find('.js-set-topic-label').html($(form).find('.js-set-topic-label').data('label-not-set'));
			$(form).find('.js-set-topic-alert').data('has-topic-alert', null);
			$(form).find('.js-topic-icon-unset').addClass('is-active');
			$(form).find('.js-topic-icon-set').removeClass('is-active');

			analyticsEvent(	$.extend(analytics_data, $(form).data('analytics')) );

		}
	});

	var saveSearchController = new FormController({
		observe: '.form-save-search',
		successCallback: function(form, context, event) {

			// If there's a stashed search, remove it.
			Cookies.remove('saveStashedSearch');

			window.controlPopOuts.closePopOut($(form).closest('.pop-out'));
			$('.js-saved-search-success-alert')
				.addClass('is-active')
				.on('animationend', function(e) {
					$(e.target).removeClass('is-active');
				}).addClass('a-fade-alert');

			window.lightboxController.closeLightboxModal();

			if(typeof angular !== 'undefined') {
				angular.element($('.js-saved-search-controller')[0]).controller().searchIsSaved();
			}

			var event_data = {};

			if($(form).data('is-search') === true) {
				event_data.event_name = "toolbar_use";
				event_data.toolbar_tool = "save_search";
			} else {
				event_data.event_name = "set_alert";
				event_data.alert_topic = $(form).find('.js-save-search-title').val();
			}

			analyticsEvent(	$.extend(analytics_data, event_data) );

		},
		beforeRequest: function(form) {
			if(!$(form).find('.js-save-search-title').val().trim()) {
				$('.js-form-error-EmptyTitle').show();
			}
		}
	});

	var saveSearchLoginController = new FormController({
		observe: '.form-save-search-login',
		successCallback: function(form, context, event) {
			Cookies.set('saveStashedSearch', {
				'Title': $('.js-save-search-title').val(),
				'Url': $('.js-save-search-url').val(),
				'AlertEnabled': $('#AlertEnabled').prop('checked')
			});

			var loginAnalytics =  {
				event_name: 'login',
				login_state: 'successful',
				userName: '"' + $(form).find('input[name=username]').val() + '"'
			};
			analyticsEvent(	$.extend(analytics_data, loginAnalytics) );

			// If Angular, need location.reload to force page refresh
			if(typeof angular !== 'undefined') {
				angular.element($('.search-results')[0]).controller().forceRefresh();
			} else {
				window.location.reload(false);
			}
		}
	});

	var toggleSavedSearchAlertController = new FormController({
		observe: '.form-toggle-saved-search-alert',
		successCallback: function(form, context, e) {
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

			analyticsEvent( $.extend(analytics_data, event_data) );
		}
	});

	$('.js-saved-search-alert-toggle').on('click', function(e) {
		$(e.target.form).find('button[type=submit]').click();
	});

	// On page load, check for any stashed searches that need to be saved
	var saveStashedSearch = Cookies.getJSON('saveStashedSearch');

	if(saveStashedSearch) {
		// Set `Save Search` values from stashed search data
		$('.js-save-search-title').val(saveStashedSearch['Title']);
		$('.js-save-search-url').val(saveStashedSearch['Url']);
		$('#AlertEnabled').prop('checked', saveStashedSearch['AlertEnabled']);

		// Save the stashed search if Search (Angular) page
		if(typeof angular !== 'undefined') {
			$('.form-save-search').find('button[type=submit]').click();
		} else {
			$('.js-set-topic-alert').each(function(index, item) {
				if($(item).data('topic-alert-url') === saveStashedSearch['Url']) {
					$(item).click();
					// If there's a stashed search, remove it.
					Cookies.remove('saveStashedSearch');
				}
			});
		}
	}


	var removeSavedSearch = new FormController({
        observe: '.form-remove-saved-search',
        successCallback: function(form, context, evt) {
            $(evt.target).closest('tr').remove();

			var event_data = {
				event_name: 'saved_search_alert_removal',
				saved_search_alert_title: $(form).data('analytics-title'),
				saved_search_alert_publication: $(form).data('analytics-publication')
			};

			analyticsEvent( $.extend(analytics_data, event_data) );

        }
    });
});
