// Note: Disabling and enabling to make bootstrap field validation work
jQuery(document).ready(function ($) {

	var showpassword = function () {
		$("#login").hide();
		$("#passwordRecovery").show();
		$("#UserNameForgot").removeAttr("disabled");
		$("#UserName").attr({ "disabled": "disabled" });
		$("#Password").attr({ "disabled": "disabled" });
		$("#UserNameForgot").focus();
	};

	$(".show-recovery").click(showpassword);

	$(".hide-recovery").click(function () {
		$("#passwordRecovery").hide();
		$("#login").show();
		$("#UserNameForgot").attr({ "disabled": "disabled" });
		$("#UserName").removeAttr("disabled");
		$("#Password").removeAttr("disabled");
		$("#UserName").focus();
	});

	$("#login input[type='submit']").click(function () {
		if ($("#UserName").val() === "" || $("#Password").val() === "") {
			$("#credentialsError").show();
			return false;
		} else {
			$("#credentialsError").hide();
		}
	});

	$("#licenseOptionsLink").click(function () {
		$("#login").hide();
		$("#licenseOptions").show();
	});

	$("#licenseOptionsBack").click(function () {
		$("#licenseOptions").hide();
		$("#login").show();
	});

	function isPostBack() {
		return document.referrer.indexOf(document.location.href) > -1;
	}

	if (!isPostBack()) {
		var queryStringValue = window.location.href.split('?')[1].split('=')[0];

		if (queryStringValue != null && queryStringValue.indexOf("passwordrecovery") > -1) {
			$(".show-recovery").click();
		}
	}


});