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

	$('.js-set-topic-alert').on('click', function(e) {

		var isSettingAlert = !$(this).data('has-topic-alert');

		$('.js-save-search-url').val($(this).data('topic-alert-url'));
		$('.js-save-search-title').val($(this).data('topic-alert-title'));

		if(isSettingAlert) {
			$('.form-save-search').find('button[type=submit]').click();
			$('.js-set-topic-label').html($('.js-set-topic-label').data('label-is-set'));
			$(this).data('has-topic-alert', 'true');
			$('.js-topic-icon-unset').removeClass('is-active');
			$('.js-topic-icon-set').addClass('is-active');
		} else {
			window.lightboxController.showLightbox($(this));
		}

	});

	var removeTopicAlert = new FormController({
		observe: '.form-remove-topic-alert',
		successCallback: function(form, context, event) {
			$('.js-set-topic-label').html($('.js-set-topic-label').data('label-not-set'));
			$(form).find('.js-set-topic-alert').data('has-topic-alert', null);
			$('.js-topic-icon-unset').addClass('is-active');
			$('.js-topic-icon-set').removeClass('is-active');
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

			angular.element($('.js-saved-search-controller')[0]).controller().searchIsSaved();
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
			}
		}
	});

	var toggleSavedSearchAlertController = new FormController({
		observe: '.form-toggle-saved-search-alert'
	});

	$('.js-saved-search-alert-toggle').on('click', function(e) {
		$(e.target.form).find('button[type=submit]').click();
		var val = $(e.target).val();
		if (val === "on") {
			$(e.target).val('off');
		} else {
			$(e.target).val('on');
		}
	});

	// On page load, check for any stashed searches that need to be saved
	var saveStashedSearch = Cookies.getJSON('saveStashedSearch');
	if(saveStashedSearch) {

		// Set `Save Search` values from stashed search data
		$('.js-save-search-title').val(saveStashedSearch['Title']);
		$('.js-save-search-url').val(saveStashedSearch['Url']);
		$('#AlertEnabled').prop('checked', saveStashedSearch['AlertEnabled']);

		// Save the stashed search
		$('.form-save-search').find('button[type=submit]').click();
	}
});
