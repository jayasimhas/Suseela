var SearchScript = function() {

			/* Toggle search tips visibility */
			$('.js-toggle-search-tips').on('click', function toggleTips() {
				$('.search-bar__tips').toggleClass('open');
				$('.search-bar').toggleClass('tips-open');
			});

			/* Toggle headlines-only results view */
			// $('.js-view-headlines-only').on('click', function toggleHeadlines() {
			// 	$('.search-results').toggleClass('headlines-only');
			// });

			//Toggle Class
			// var classApp = angular.module('classApp', []);
			//
			// classApp.controller('classCtrl', function ($scope) {
			// 	$scope.isActive = false;
			//   $scope.activeButton = function() {
			//     $scope.isActive = !$scope.isActive;
			//   }
			// });

}();
