/*
	EBI scripts (utilizing jQuery 1.4.4)
*/

/* ---------- Initialize page ---------- */
jQuery.noConflict();
jQuery(function ($) {
    $(document).ready(function () {

        // Stripe table row colors
        $("table.data, table.search-results").find("tr.result").each(function (index) {
            if (index % 2) {
                $(this).addClass("even");
            } else {
                $(this).addClass("odd");
            }
        });

        $("table.data, table.search-results").find("tr.parent").each(function () {
            var previous = $(this).closest('tr').prev();

            if (previous) {
                if (previous.hasClass('even')) {
                    $(this).addClass('even');
                } else if (previous.hasClass('odd')) {
                    $(this).addClass('odd');
                }
            }

        });

        // Insert external and document icons
        $("a.external:not(:has(img))").append('<img class="docicon" src="/images/icon_external.png" width="13" height="13" alt=" (External Site Link)" title="External Site Link" />');
        $("a.pdf:not(:has(img)), a[href$=pdf]:not(:has(img))").append('<img class="docicon" src="/images/icon_pdf.gif" width="12" height="14" alt=" (PDF)" />');
        $("a.word:not(:has(img)), a[href$=doc]:not(:has(img)), a[href$=docx]:not(:has(img))").append('<img class="docicon" src="/images/icon_word.gif" width="12" height="14" alt=" (Word Document)" />');
        $("a.excel:not(:has(img)), a[href$=xls]:not(:has(img)), a[href$=xlsx]:not(:has(img))").append('<img class="docicon" src="/images/icon_excel.gif" width="12" height="14" alt=" (Excel Spreadsheet)" />');
        $("a.powerpoint:not(:has(img)), a[href$=ppt]:not(:has(img)), a[href$=pptx]:not(:has(img))").append('<img class="docicon" src="/images/icon_powerpoint.gif" width="12" height="14" alt=" (Powerpoint Presentation)" />');
        $("a.text:not(:has(img)), a[href$=txt]:not(:has(img)), a[href$=rtf]:not(:has(img))").append('<img class="docicon" src="/images/icon_text.gif" width="12" height="14" alt=" (Text Document)" />');

        // Special case for supporting documents
        $(".supportingdocs li a.externalLink:not(:has(img))").append('<img class="docicon" src="/images/icon_external.png" width="13" height="13" alt=" (External Site Link)" title="External Site Link" />');

        // Column child CSS3 fix
        if ($("div.columns").length > 0) {
            $("div.columns > div.col:first-child").addClass("first-child");
            $("div.columns.three > div.col:nth-child(2)").addClass("middle-child");
            $("div.columns > div.col:last-child:not(:first-child):not('.middle-child')").addClass("last-child");
        }


        // Multiselect form fields (requires jquery.multiSelect)
        if (typeof $("").multiSelect === "function") {
            $("select.form-multiselect").multiSelect({
                selectAll: false
            });

            $("body").append('<div id="multiSelectTooManyDialog" title="Maximum Reached" style="display:none">You may select up to 30 options from this list.</div>');

            $(".multiSelectOptions input").click(function () {
                var size = $(".multiSelectOptions input:checked").length;
                if (size > 30) {
                    $("#multiSelectTooManyDialog").dialog({
                        modal: true,
                        height: 70
                    });
                    $(this).removeAttr("checked");
                    $(this).parent().removeClass("checked");
                    $(this).parents(".multiSelectOptions").siblings(".multiSelect").updateSelected();
                }
            });
        }

        // Select box cropping fix for IE6 through IE8
        if ($.browser.msie && parseInt($.browser.version, 10) >= 6 && parseInt($.browser.version, 10) < 9) {
            $(".search-filters select, .user-shortcuts select").each(function () {
                var $select = $(this);

                // Ignore multi-selects and hidden fields
                if (typeof $select.attr("size") === "undefined" || $select.attr("size") > 1 || $select.is(":hidden")) { return; }

                // Add wrapping container to contain field
                var $wrapper = $('<span class="selectwrapper"></span>')
                $wrapper.width(parseInt($select.outerWidth(), 10));
                $select.wrap($wrapper);

                // Manually redefine select box width to fit in new container
                if (parseInt($.browser.version, 10) >= 8) {
                    // IE8 pixel-pushing
                    var extraspace = ($select.parents(".user-shortcuts").length > 0) ? -6 : 2;
                } else {
                    // IE6-7 pixel-pushing
                    var extraspace = ($select.parents(".user-shortcuts").length > 0) ? -4 : 4;
                }
                $select.css("width", (parseInt($select.width(), 10) + extraspace) + "px");

                // Undo any max-width styles
                $select.css("maxWidth", "none");

                // Set variables
                $select.data("originalwidth", this.offsetWidth);
                $select.data("makewider", null);

                // Open field
                $select.mousedown(function () {
                    var $element = $(this);

                    if ($element.data("makewider") === false) { return; }

                    // Save current width and reset to full size
                    var styledWidth = this.offsetWidth;
                    $element.width("auto");
                    var desiredWidth = this.offsetWidth;

                    // If this control needs less than it was styled for, then don't widen it
                    if ($element.data("makewider") !== true && desiredWidth <= styledWidth) {
                        $element.data("makewider", false);

                        $element.width(parseInt($element.data("originalwidth"), 10));
                        $element.click(); // Simulate another click, since setting styles has already caused the box to close at this point

                    } else {
                        $element.data("makewider", true);
                    }
                });

                // Close field
                $select.bind("blur change", function () {
                    var $element = $(this);
                    $element.width(parseInt($element.data("originalwidth")));
                });
            });
        }
	    
    	//Header Navigation
        $(".headerSearchBoxGo").click(function () {
        	runSearchBox();
        });
	    $(".searchbox input").keypress(function(e) {
			if (e.which == 13) {
				runSearchBox();
				e.preventDefault();
			}
	    });
	    function runSearchBox() {
	    	var input = $(".searchbox input").val();
	    	if (input == "" || input == "Keywords or Article/Deal/Company #")
	    	{ return; }

	    	window.location = "/search?q=" + input;
	    }
	    $(".headerPublicationDropDown").change(function() {
	    	var selection = $(".headerPublicationDropDown").val();
	    	if(selection == "")
	    	{ return; }

		    window.location = selection;
	    });

        // Overlays
        initOverlays();

        // Collapsible blocks
        initCollapsibles();

        // Tabs and link lists
        initTabs();

        // Form filters
        initFilters();

        // Redirect select boxes
        initRedirects();

        // Article toolbar
        initArticleTools();

        // Popup Calendar
        initCalendar();

        // Search results
        initSearchResults();

        // Content Toggle
        initContentToggle();

        // Set CombinedSearchBox state
        setCombinedSearchBox();

        // Clear search field default values
        $("input[type=search]").focus(function () {
            if (this.value == this.defaultValue) {
                $(this).val("");
            }
        }).blur(function () {
            if (this.value == "") {
                $(this).val(this.defaultValue);
            }
        });



        // Delete button confirmations
        $("a[rel=confirmdelete]").click(function () {
            return confirm("Are you sure you want to delete this item?");
        });



        // Mac-only font rendering differences
        if (navigator.platform.indexOf("Mac") == 0) {
            $("html").addClass("os-mac");
        }


        // IE-only bug fixes and helpers
        if ($.browser.msie && parseInt($.browser.version) < 9) {

            /* CSS3 last-child fix for blocks */
            $(".block > *:last-child, .block > ul:last-child > li:last-child, .sidebox > *:last-child, .overlay > *.last-child, .right-col > *:last-child, .coloredbox > *:last-child, #checkout-nav > ul > li:last").addClass("last-child");

            /* <hr> wrapper for background image */
            $("hr").each(function () {
                $(this).wrap('<div class="' + $(this).attr("class") + ' hr"></div>');
            });

            // IE6 PNG fix
            if (parseInt($.browser.version) < 7) {
                iepngfix();
            }
        }

        //Add Red highlight for any control that is not valid
        if (typeof Page_IsValid != 'undefined') {
            if (!Page_IsValid) {
                var i = 0;
                for (; i < Page_Validators.length; i++) {
                    if (!Page_Validators[i].isvalid) {
                        $("#" + Page_Validators[i].controltovalidate).addClass("error");
                    }
                }
            }
        }

        checkShippingBillingMatches();

        $(".address-shipping input:text, .address-shipping select, .address-billing input:text, .address-billing select").change(function () {
            checkShippingBillingMatches();
        });

        $("#chkMatchShippingAndBilling").click(function () {
            var checked = $(this).is(':checked');
            $(".address-shipping").each(function () {
                var shippingBox = $(this).find("input:text, select");
                var fieldClass = $(this).attr('class').split(/\s+/)[1];
                var billingParent = $(".address-billing." + fieldClass);
                var billingBox = billingParent.find("input:text, select");
                if (checked) {
                    shippingBox.val(billingBox.val());
                }
                else {
                    if (shippingBox.val() == billingBox.val()) {
                        shippingBox.val("");
                    }
                }
            });
            var countryParent = $(".address-shipping.field-country");
            var countryDdl = countryParent.find("select");
            if (countryDdl.length > 0)
                FireShippingCountryDropdownEvent(countryDdl[0]);
        });
        
        var images = $('.article-image img');

        images.each(function () {
            if ($(this).width() < 350) {
                var divImage = $(this).closest('section.article-image');
                divImage.removeClass('article-image');
                divImage.addClass('article-image-left').css('width', $(this).width());
            }
            if ($(this).width() < 550) {
                $(this).closest('a.enlarge').removeClass('enlarge');
            }
        });
        
    });

    // Initialize overlay support
    function initOverlays() {

        // Overlays are activated by links with target="overlay" and href="#the-overlay-elements-id"
        // The actual contents should be located at the bottom of the page within the <div id="overlays"> section,
        //   and have the corresponding class attribute to the link (not id for Sitecore compatibility)

        // Use top.window and top.window.document to show overlays on outer windows, not within iframes
        var mainwindow = (top === window) ? window : top.window;


        // Move any stray overlays to within top #overlay wrapper
        $(".overlay").each(function () {

            if (top !== window) {
                // Moved iframed overlays to parent
                $(this).appendTo($("#overlays", mainwindow.document));

            } else if ($(this).parents("#overlays").length == 0) {
                // If not already within the #overlay container
                $(this).appendTo($("#overlays"));
            }
        });


        // Auto-inject the close link
        $("#overlays .overlay").prepend('<a href="#" class="overlay-close" title="Close"></a>');

        // Open overlays using target=overlay attribute
        $("a[target=overlay]").click(function () {

            var overlayname = $(this).attr("href");

            return displayOverlay(mainwindow, overlayname);
        });


        // Re-center overlay if window resizes
        $(mainwindow).resize(function () {
            if ($("#overlays").length == 1 && $("#overlays-cover").is(":visible")) {
                var $targetoverlay = $("#overlays > .overlay:visible");

                if ($targetoverlay.length > 0) {
                    // Same calculations as above
                    var overlaypadding = parseInt($targetoverlay.outerHeight() - $targetoverlay.height());
                    $targetoverlay.css("max-height", parseInt(($(window).height() - overlaypadding) * .85));

                    $targetoverlay.css("left", parseInt($(window).width() - $targetoverlay.outerWidth()) / 2);
                    $targetoverlay.css("top", parseInt(($(window).height() - $targetoverlay.outerHeight()) / 2));
                }
            }
        });


        // Close on cover click
		$("#overlays-cover, #overlays a.overlay-close, #overlays a[rel=close]").click(function (e) {
          closeOverlays($(this));
          e.preventDefault();
		});

        // Login overlay forgot password link
        $loginoverlay = $(".overlay-login");
        if ($loginoverlay.length > 0) {
            $(".forgotpasswordbox", $loginoverlay).hide(); // Hide password box onload

            $("a.forgotpassword", $loginoverlay).click(function () {
                $(".forgotpasswordbox", $loginoverlay).slideDown("fast", function () {
                    // Vertically center again
                    $loginoverlay.animate({ top: parseInt(($(window).height() - $loginoverlay.outerHeight()) / 2) }, "fast");
                });

                $(this).hide(); // Hide toggle link
            });
        }
    }


    // Redirect select boxes (uses class="redirect")
    function initRedirects() {

        $("select.redirect").live("change", function () {
            var url = $(this).val();
            if (url != "" && url != window.location.pathname && url != window.location.href) {
                window.location = url;
            }
            return false;
        });
    }

    // Set CombinedRssSearch box to proper state
    function setCombinedSearchBox() {
        var $radios = $('input:radio[name=stay-up-to-date]');
        if ($radios.length > 0 && $radios.is(':checked') === false) {
            var activePanel = $('.hdnActivePanel').val();
            var $button = $radios.filter('[value=' + activePanel + ']');
            $button.attr('checked', true);
            $($button.data('toggle-target')).toggle();
        }
    }

    // Initialize collapsible blocks
    function initCollapsibles($context) {

        if (!$context) { $context = $("body"); }

        // Use <div class="block collapsible"> for expanded boxes
        //     <div class="block collapsible collapsed"> for collapsed boxes

        // Wrap a div around the collapsible objects
        $(".collapsible", $context).wrapInner('<div class="collapsed-content"></div>')

        // Except for heading and subtitle
        $(".collapsed-content", $context).each(function () {
            $block = $(this).parent();
            $block.prepend($("h2, .subtitle", this));
        });


        // Hide collapsed content
        $(".collapsible.collapsed > .collapsed-content", $context).hide();


        // Auto-inject the collapse link
        $(".collapsible", $context).prepend('<a href="#" class="button-collapse" title="Hide"></a>');

        // Adjust for collapsed lists
        $(".collapsible.collapsed a.button-collapse", $context).attr("title", "Show");

        // Collapse action (heading with href="#" or collapsible button)
        $("a.button-collapse", $context).click(function () {
            $block = $(this).parent(".collapsible");
            $block.toggleClass("collapsed")
			.find(".collapsed-content").slideToggle("normal");

            // Swap button title
            if ($(this).attr("title") == "Show") {
                $(this).attr("title", "Hide");
            } else {
                $(this).attr("title", "Show");
            }

            return false;
        });

        $(".collapsible > h2 > a[href='#'], .collapsible > .subtitle > a[href='#']").click(function () {
            $(this).parent().prev("a.button-collapse").click();
            return false;
        });


        // Hide all / show all links
        $("a.expand-all").live("click", function () {
            $(this).parent().siblings().find("a.button-collapse[title=Show]").click();
            return false;
        });

        $("a.collapse-all").live("click", function () {
            $(this).parent().siblings().find("a.button-collapse[title=Hide]").click();
            return false;
        });
    }




    // Initialize form filters
    function initFilters() {

        /*
        Filter select boxes should be within:
        <div class="filters" data-results="#uniqueid">

        Then the results HTML is located in <div id="uniqueid"> anywhere on the page.

        Any form change will hide and then show the results HTML. All processing is done on the server side.
        */

        // Initialize structure and show results onload
        $("div.filters").each(function () {

            var $form = $(this);

            if ($form.siblings(".collapsed-content").length > 0) {

                // Use existing collapsible strcture if it exists
                $form.closest(".collapsible").addClass("filter-wrapper");
                $form.siblings(".collapsed-content").addClass("filter-contents");

            } else {

                // Create wrapper elements if not part of collapsible list (and don't already exist)
                if (!$form.parent().hasClass("filter-wrapper")) {
                    $form
					.wrap('<div class="filter-wrapper"></div>')
					.after('<div class="filter-contents"></div>'); // Container for results content
                }
            }

            // Find and move form's results <div> within wrapper
            if ($form.data("results") && $($form.data("results")).length > 0) {
                $($form.data("results")).appendTo($form.next(".filter-contents:first"));
            }

            // Hide onload if within collapsed area
            if ($form.closest("filter-wrapper").hasClass("collapsed")) {
                $form.next(".filter-contents").hide();
            }
        })

        // Select box change action
		.find("select").change(function () {

		    var $wrapper = $(this).closest(".filter-wrapper");
		    var $contents = $wrapper.children(".filter-contents");

		    $contents
				.stop(true)
				.slideUp("fast", function () {

				    // Reset height if stopped in middle of animation
				    $(this).css("height", "auto");

				    // Server-side: replace contents with new results
				    // ...
				    // ...

				    // Show new content
				    if ($wrapper.hasClass("collapsed")) {
				        // Click expand button
				        $wrapper.find("a.button-collapse:first").click();
				    } else {
				        // Just show contents if not collapsed
				        $contents.slideDown("normal");
				    }
				});
		});
    }



    // Initialize tabs and link lists
    function initTabs() {

        /*
        Create tab anchor links to elements on page (no AJAX):
        <div class="tabs">
        <ul>
        <li><a href="#anchor1">Link 1</a></li>
        <li><a href="#anchor2">Link 2</a></li>
        </ul>
        </div>

        Add <li class="active"> to any tab that you want to be active on page load.
        The destination contents can be anywhere on the page, and will be dynamically hidden until that tab is clicked.

        Any form filters within the tab will auto-submit upon the tab content loading.
        If the links are to regular pages (not local anchors), they will work like regular pages
        */

        $(".tabs")
		.wrap('<div class="tab-wrapper"></div>')  // Wrap to speed up DOM browsing
		.after('<div class="tab-contents"></div>')  // Group all destination contents here

		.each(function () {
		    // Tabs are coded as lists, but must rewrite into layout tables 
		    // for better spacing until all targeted browsers support display:table

		    var $tabs = $(this);
		    var $table = $('<table cellspacing="0"></table>');

		    $("li", this).each(function () {
		        var $tablecell = $("<td>" + $(this).html() + "</td>");

		        $tablecell.attr("class", $(this).attr("class"));
		        $tablecell.attr("id", $(this).attr("id"));
		        $tablecell.attr("style", $(this).attr("style"));
		        $table.append($tablecell);
		    });

		    // Set first tab to be active if none are
		    if ($("td.active", $table).length == 0) {
		        $("td:first", $table).addClass("active");
		    }

		    // Add row
		    $table = $table.wrapInner('<tbody><tr></tr></tbody>');

		    // Replace list with table
		    $("ul", this).replaceWith($table);


		    // Find and group tabs' destination contents within wrapper and hide
		    $("a", this).each(function () {
		        if (this.hash && $(this.hash).length > 0) {
		            $(this.hash).hide().appendTo($tabs.siblings(".tab-contents:first"));
		        }
		    });
		})

        // Tab click action
		.find("a").click(function () {

		    var link = this;
		    var $link = $(this);

		    // Can't click on current tab or tab without working link
		    if ($link.closest("td").hasClass("active") || $link.attr("href") == "#") { return false; }

		    // If regular link (not anchor), just follow link
		    if (link.hash == "") { return true; }

		    // Highlight new active tab
		    $link.closest("td")
				.addClass("active")
				.siblings(".active")
				.removeClass("active");

		    // Find previous anchor tab contents
		    var $contentsgroup = $link.closest(".tab-wrapper").children(".tab-contents");
		    var $contents = $contentsgroup.children();


		    // Slide up the grouping, not individual content element
		    $contentsgroup
				.stop(true) // Stop if already animating
				.slideUp("fast", function () {

				    // Hide all old tab contents
				    $contentsgroup.children().hide();

				    // Reset height if stopped in middle of animation
				    $(this).css("height", "auto");

				    // Get anchor link
				    var anchor = link.hash;

				    // Double-check that the destination element actually exists
				    if (!anchor || $(anchor).length == 0) { return false; }

				    // Show new contents and group
				    $(anchor).show();
				    $(this).slideDown("normal");
				});

		    return false;
		})


        // Show initial/active tab contents onload
		.end().find("td.active:first a").each(function () {

		    // Get anchor link
		    var anchor = this.hash;

		    // Only continue of the destination element actually exists
		    if (!anchor || $(anchor).length == 0) { return false; }

		    // Show new contents and group
		    $(anchor).show();
		    $(this).show();
		});



        // Horziontal linked list
        // Only add wrappers if not within collapsible blocks (otherwise use those existing elements)
        // Must call initCollapsible() first to setup collapsible structure

        $("#contentwrapper .links")
		.each(function () {

		    var $tabs = $(this);

		    if ($tabs.siblings(".collapsed-content").length > 0) {

		        // Use existing collapsible strcture if it exists
		        $tabs.closest(".collapsible").addClass("tab-wrapper");
		        $tabs.siblings(".collapsed-content").addClass("tab-contents");

		    } else {

		        // Otherwise, create wrappers just like with tabs
		        $tabs
					.wrap('<div class="tab-wrapper"></div>')
					.after('<div class="tab-contents"></div>');
		    }

		    // Set first link to be active if none are
		    if ($("a.highlight", this).length == 0) {
		        $("a:first", this).addClass("highlight");
		    }


		    // Find and group links' destination contents within wrapper and hide
		    $("a", this).each(function () {
		        if (this.hash && $(this.hash).length > 0) {
		            $(this.hash).hide().appendTo($tabs.siblings(".tab-contents:first"));
		        }
		    });

		})

        // Link click action
		.find("a").click(function () {

		    var link = this;
		    var $link = $(this);

		    // Can't click on tab without working link
		    if ($link.attr("href") == "#") { return false; }

		    // Find previous anchor tab contents
		    var $wrapper = $link.closest(".tab-wrapper");
		    var $contentsgroup = $wrapper.children(".tab-contents");
		    var $contents = $contentsgroup.children();

		    // Can't click on current tab, except when the block is collapsed
		    if ($link.hasClass("highlight")) {
		        if ($wrapper.hasClass("collapsed")) {
		            // Expand to show if collapsed
		            $wrapper.find("a.button-collapse:first").click();
		        }
		        return false;
		    }

		    // Highlight new active tab
		    $link
				.addClass("highlight")
				.siblings("a.highlight")
				.removeClass("highlight");


		    // Slide up the grouping, not individual content element
		    $contentsgroup
				.stop(true) // Stop if already animating
				.slideUp("fast", function () {

				    // Hide all old tab contents
				    $contents.hide();

				    // Reset height if stopped in middle of animation
				    $(this).css("height", "auto");

				    // Get anchor link
				    var anchor = link.hash;

				    // Only continue if the destination element actually exists
				    if (!anchor || $(anchor).length == 0) { return false; }

				    // Show new contents and group
				    $(anchor).show();

				    if ($wrapper.hasClass("collapsed")) {
				        // Click expand button if collapsed block
				        $wrapper.find("a.button-collapse:first").click();
				    } else {
				        // Just show contents if not collapsed
				        $contentsgroup.slideDown("normal")
				    }
				});

		    return false;
		})


        // Show initial/active tab contents onload
		.end().find("a.highlight").each(function () {

		    var $wrapper = $(this).closest(".tab-wrapper");
		    var $contentsgroup = $wrapper.children(".tab-contents");

		    // Get anchor link
		    var anchor = this.hash;

		    // Only continue of the destination element actually exists
		    if (!anchor || $(anchor).length == 0) { return false; }

		    // Show new contents and group
		    $(anchor).show();

		    // Show contents only if not collapsed onload
		    if (!$wrapper.hasClass("collapsed")) {
		        $contentsgroup.show();
		    }
		});
    }




    // Article toolbar
    function initArticleTools() {
        if ($("div.article-tools").length == 0) { return; }

        // Set widths of flyouts
        $("li.tool-size > ul").css("min-width",
			$("li.tool-size").width() - parseInt($("li.tool-size > ul").css("padding-left")) - parseInt($("li.tool-size > ul").css("padding-right")));

        $("li.tool-share > ul").css("min=width",
			$("li.tool-share").width() - parseInt($("li.tool-share > ul").css("padding-left")) - parseInt($("li.tool-share > ul").css("padding-right")));


        // Expand flyouts upon focus (for keyboard and tap support)
        $(".article-tools > ul > li.flyout").bind("focusout", function () {
            if ($(this).hasClass("expand")) {
                $(this).removeClass("expand");
            }
            return false;
        })
		.children("a").click(function () {
		    if ($(this).siblings("ul").is(":hidden")) {
		        $(this).parent().addClass("expand");
		    }
		    return false;
		});




        // Determine text size cookie
        var textsize = readCookie("ebi-textsize");

        switch (textsize) {
            case "2":
                $(".tool-text-medium").addClass("active");
                $("body").addClass("text-medium");
                break;

            case "3":
                $(".tool-text-large").addClass("active");
                $("body").addClass("text-large");
                break;

            case "1":
            default:
                $(".tool-text-small").addClass("active");
                $("body").addClass("text-small");
                break;
        }


        // Set text size
        $(".tool-size li > a").click(function () {

            // Check that size isn't already active
            if ($(this).hasClass("active")) { return false; }

            // Remove old classes
            $(".tool-size a.active").removeClass("active");
            $("body").removeClass("text-small text-medium text-large");

            // Set cookie and body classes
            if ($(this).hasClass("tool-text-small")) {
                createCookie("ebi-textsize", "1", 1095);
                $("body").addClass("text-small");

            } else if ($(this).hasClass("tool-text-medium")) {
                createCookie("ebi-textsize", "2", 1095);
                $("body").addClass("text-medium");

            } else if ($(this).hasClass("tool-text-large")) {
                createCookie("ebi-textsize", "3", 1095);
                $("body").addClass("text-large");
            }

            $(this).addClass("active");

            return false;
        });

    }



    // Initialize calendar (requires jQuery UI datepicker)
    function initCalendar() {
        if ($.datepicker) {

            $("a.datepicker")
				.before('<input type="text" name="date" style="display:none" size="10" maxlength="10" />')
				.click(function () { $(this).prev("input").datepicker("show"); return false; });

			$("a.datepicker").prev("input")
				.datepicker({
					changeMonth: true,
					changeYear: true,
					showOtherMonths: true,
					selectOtherMonths: true,
					minDate: (new Date(1991, 1 - 1, 1)),
					maxDate: "+0",
					yearRange: "1991:nn",
					dayNamesMin: ['S', 'M', 'T', 'W', 'T', 'F', 'S'],
					onSelect: function (dateText, inst) {
						// Redirect to location in link with chosen date in querystring
						var d = new Date(dateText);
						var fmt2 = $.datepicker.formatDate("M-dd-yy", d);
						location.href = this.nextSibling.href + "?issue=" + fmt2;
					}
				});
		}
	}

	// Initialize content that's toggled based on clicking form elements
	function initContentToggle() {
		$('[data-toggle-target]').change(function () {
			var targetId = $(this).data('toggle-target');
			var $targets = $("[data-toggle-id=" + targetId + "]");

        	    	// hide ALL toggle targets
            		$("[data-toggle-id]").hide();

		        // show the toggle targets we want
			$targets.show();
		});
	}



    // Initialize search results
    function initSearchResults() {
        if ($("body.search").length == 0) { return; }


        // Check All results column
        if ($("td.search-check, th.search-check").length > 0) {

            // Toggle all boxes of table when header is checked
            $("th.search-check input:checkbox").click(function () {
                if ($(this).is(":checked")) {
                    $(this).closest("table").find("td.search-check input:checkbox").attr("checked", "checked");
                } else {
                    $(this).closest("table").find("td.search-check input:checkbox").removeAttr("checked");
                }
                updateCheckboxCounts();
            });

            $("td.search-check input:checkbox").click(function () {
                if ($(this).is(":checked")) {

                    // Check header if *all* boxes checked
                    var allchecked = true;
                    $(this).closest("table").find("td.search-check input:checkbox").each(function () {
                        allchecked &= $(this).is(":checked");
                    });

                    if (allchecked) {
                        $(this).closest("table").find("th.search-check input:checkbox").attr("checked", "checked");
                    }

                } else {
                    // Uncheck header if any boxes uncheck
                    $(this).closest("table").find("th.search-check input:checkbox").removeAttr("checked");
                }
                updateCheckboxCounts();
            });
        }


        // More/fewer options toggle
        $("a.more-options").click(function () {

            // Toggle options
            $("div.more-options").slideToggle("fast");

            // Swap text
            if ($(this).text().indexOf("More") > 0) {
                $(this).html($(this).html().replace("+ More", "- Fewer"));
            } else {
                $(this).html($(this).html().replace("- Fewer", "+ More"));
            }

            return false;
        });

        // Hide other options onload
        $("div.more-options").not(".expanded").hide();

        //Handle country/state selection
        handleCountryStateEnabling();

        $(".country-ddl-section select").change(function () {
            handleCountryStateEnabling();
        });

        // Date range selection (requires jQuery UI datepicker)
        if ($.datepicker) {

            $.datepicker.setDefaults({
                changeMonth: true,
                changeYear: true,
                showOtherMonths: true,
                selectOtherMonths: true,
                minDate: (new Date(1991, 1 - 1, 1)),
                maxDate: "+0",
                yearRange: "1991:nn",
                dayNamesMin: ['S', 'M', 'T', 'W', 'T', 'F', 'S']
            });

            $("select.date-selection").change(function () {
                handleSearchDateChange($, this);
            }).change();
        }
    }

    function handleCountryStateEnabling() {
        if ($(".country-ddl-section select").val() == "USA") {
            $(".state-ddl-section select").removeAttr("disabled");
        }
        else {
            $(".state-ddl-section select").attr("disabled", "disabled");
        }
    }

    function updateCheckboxCounts() {
        var size = 0;
        var selected = "";
        var i = 0;
        $("td.search-check input").each(function () {
            if ($(this).is(":checked")) {
                size++;
                if (selected.length > 0) {
                    selected += ",";
                }
                selected += i;
            }
            i++;
        });
        $("span.check-count").html(size);
        $("#selected-rows-input").val(selected);
    }


    // IE6 PNG fix
    function iepngfix() {

        // IE 5.5 and 6.0 PNG filter support (derived from youngpup.net)
        $("img[src$=png]").each(function () {
            var src = this.src;
            var div = document.createElement("div");

            // Set replacement div properties
            div.id = this.id;
            div.className = this.className;
            div.title = this.title || this.alt;
            div.style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + this.src + "', sizing='scale')";
            div.style.width = this.width + "px";
            div.style.height = this.height + "px";

            // Replace image with transparent div
            this.replaceNode(div);
        });
    }

    function checkShippingBillingMatches() {
        var discrepancyCount = 0;
        $(".address-shipping").each(function () {
            var shippingBox = $(this).find("input:text, select");
            var fieldClass = $(this).attr('class').split(/\s+/)[1];
            var billingParent = $(".address-billing." + fieldClass);
            var billingBox = billingParent.find("input:text, select");
            if (shippingBox.val() != billingBox.val()) {
                discrepancyCount++;
            }
        });
        $("#chkMatchShippingAndBilling").attr('checked', discrepancyCount == 0);
    }

    /* -------- Helper functions -------- */

    // Cookie utilities
    function createCookie(name, value, days) {
        var expires;
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toGMTString();
        } else {
            expires = "";
        }
        document.cookie = name + "=" + value + expires + "; path=/";
    }

    function readCookie(name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    }
});


// Search date range select box choice toggle
function handleSearchDateChange($, select) {
	var $cell = $(select).parents("td").eq(0);
	var value = $(select).val();

	// Show date fields based on range selection
	switch (value) {
		case 'on':
			$(".date-fields", $cell).removeClass("invisible");
			$(".date-range", $cell).addClass("invisible");

			$("input.date-start", $cell).datepicker("destroy").datepicker();
			break;

		case 'since':
			$(".date-fields", $cell).removeClass("invisible");
			$(".date-range", $cell).addClass("invisible");

			$("input.date-start", $cell).datepicker("destroy").datepicker({ maxDate: "-1" });
			break;

		case 'before':
			$(".date-fields", $cell).removeClass("invisible");
			$(".date-range", $cell).addClass("invisible");

			$("input.date-start", $cell).datepicker("destroy").datepicker({ maxDate: "+1" });
			break;

		case 'between':
			$(".date-fields", $cell).removeClass("invisible");
			$(".date-range", $cell).removeClass("invisible");

			var dates = $("input.date-start, input.date-end", $cell).datepicker("destroy").datepicker({
				onSelect: function (selectedDate) {
					var option = $(this).hasClass("date-start") ? "minDate" : "maxDate",
									instance = $(this).data("datepicker"),
									date = $.datepicker.parseDate(
												instance.settings.dateFormat || $.datepicker._defaults.dateFormat,
												selectedDate,
												instance.settings
											);
					dates.not(this).datepicker("option", option, date);
				}
			});
			break;

		default:
			$(".date-fields", $cell).addClass("invisible");
	}
}

// Publication cart metrics tracking
var uPrice = 0;
var uQuantity = 1;
var uProduct = '';
function setCartMetricsVars(price, quantity, name) {
	uPrice = price;
	uQuantity = quantity;
	uProduct = name;
}

function trackCartAdd() {
	var parameters = 'ev=cartadd&rta=' + uProduct + ';' + uQuantity + ';' + uPrice;
	ntptEventTag(parameters);
}

function trackCartAdd(price, quantity, name) {
	var parameters = 'ev=cartadd&rta=' + name + ';' + quantity + ';' + price;
	ntptEventTag(parameters);
}

var hasShippingZipStateValidation = false;
var hasBillingZipStateValidation = false;
var hasContactZipStateValidation = false;
function checkCountry(ddlID, rfIDsList, starIDsList) {
	//For US enable the controls else for other countries.
    var rfIDs = rfIDsList.split(',');
    var starIDs = starIDsList.split(',');
	if ((document.getElementById(ddlID).value == 'United States') || (document.getElementById(ddlID).value == '')) {
		for (var i = 0; i < rfIDs.length; i++) {
			enable(rfIDs[i], starIDs[i]);
		}
		if (ddlID.indexOf("Billing") >= 0) {
			hasBillingZipStateValidation = true;
		}
		if (ddlID.indexOf("Shipping") >= 0) {
			hasShippingZipStateValidation = true;
		}
		if (ddlID.indexOf("Contact") >= 0) {
		    hasContactZipStateValidation = true;
		}

	}
	else {
		for (var i = 0; i < rfIDs.length; i++) {
			disable(rfIDs[i], starIDs[i]);
		}
		if (ddlID.indexOf("Billing") >= 0) {
			hasBillingZipStateValidation = false;
		}
		if (ddlID.indexOf("Shipping") >= 0) {
			hasShippingZipStateValidation = false;
		}
		if (ddlID.indexOf("Contact") >= 0) {
		    hasContactZipStateValidation = false;
		}
	}
}

function enable(validatorId, starId) {
	var validator = document.getElementById(validatorId);
	if (validator != null) {
		ValidatorEnable(validator, true);
	}
    jQuery('#' + starId).show();
}

function disable(validatorId, starId) {
    var validator = document.getElementById(validatorId);
	if (validator != null) {
		ValidatorEnable(validator, false);
	}
    jQuery('#' + starId).hide();
}

function FireShippingCountryDropdownEvent(countryDdl) {
	countryDdl.ondblclick();
}

function displayOverlay(mainwindow, overlayname) {
	// Find the correct element based on the anchor link
	var overlayname = overlayname.replace("#", ".");

	var $targetoverlay = jQuery(overlayname);

	// If within iframe, check parent window
	if ($targetoverlay.length == 0 && top !== window) {
		$targetoverlay = jQuery(overlayname, mainwindow.document);
	}

	if ($targetoverlay.length > 0) {

   		// Reveal the overlays
   		$targetoverlay.show();

   		jQuery("#overlays-cover", mainwindow.document).fadeTo("normal", .75);
   		jQuery("#overlays", mainwindow.document).fadeIn("normal");

   		// Set height to maximum 85% window, accounting for padding
   		var overlaypadding = parseInt($targetoverlay.outerHeight() - $targetoverlay.height());
   		$targetoverlay.css("max-height", parseInt((jQuery(mainwindow).height() - overlaypadding) * .85));

   		// Horizontally center on screen (must call this after the overlay is visible)
   		$targetoverlay.css("left", parseInt(jQuery(mainwindow).width() - $targetoverlay.outerWidth()) / 2);

   		// Vertically center on screen
   		$targetoverlay.css("top", parseInt((jQuery(mainwindow).height() - $targetoverlay.outerHeight()) / 2));

        // close overlays flagged to autoclose
        var autoclose = $targetoverlay.data("autoclose");
        if (autoclose) {
          delay = $targetoverlay.data('timeout');

          // default to 3 seconds if delay isn't specified
          delay = delay ? delay : 3000;
          var self = this;
          var timer = setTimeout(function() {
            self.closeOverlays($targetoverlay);
          }, delay);
        }

   		return false;
	}
}

function closeOverlays($overlay) {
    jQuery("#overlays-cover, #overlays").fadeOut("normal", function () {
        // Hide contents
        jQuery("#overlays .overlay:visible").hide();
    });
    var parentElement = $overlay.parent(".overlay");
    parentElement.trigger('lightboxClosed');

   }


   function addLoadEvent(func) {
   	var oldonload = window.onload;
   	if (typeof window.onload != 'function') {
   		window.onload = func;
   	} else {
   		window.onload = function () {
   			if (oldonload) {
   				oldonload();
   			}
   			func();
   		}
   	}
   }
   
/** SITECATALYST **/
var siteCatalystCurrentlySelectedPropValue = null;
function siteCatalystTrackEvent(sender, jsEvent, eventName, propName, propValue) {
	var s=s_gi(s_account);
	s.linkTrackVars='events' + (propName ? "," + propName : "");
	s.linkTrackEvents=eventName;
	s.events = eventName;

	if (propName)
		s[propName] = siteCatalystCurrentlySelectedPropValue ? siteCatalystCurrentlySelectedPropValue : propValue;
	
	s.tl(this, 'o', eventName);

	if (jQuery(sender).is('input')) {
		return;
	}

	jsEvent.preventDefault();
	jsEvent.stopPropagation();

	//delay link firing by 1/10th of a second to make sure s.tl has time to fire.
	setTimeout(function () {

		var href = jQuery(sender).attr('href');
		if (href.indexOf('javascript:') == 0) {
			eval(href);
		} else {
			if (jQuery(sender).attr('target') == "_blank") {
				window.open(href);
			} else {
				window.location = href;
			}
		}
	}, 100);

}

function siteCatalystSelectProduct(productName) {
	siteCatalystCurrentlySelectedPropValue = productName;
}

/** 
* Helper method for declaring namespaces 
*/
nspace = function (nspace) {
	var parts = nspace.split(".");
	var obj = window;
	for (var i = 0; i < parts.length; i++) {
		obj[parts[i]] = obj[parts[i]] || {};
		obj = obj[parts[i]];
	}
};



