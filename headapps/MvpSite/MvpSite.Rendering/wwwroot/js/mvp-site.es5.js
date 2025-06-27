"use strict";

function timelineEventsHeightRecalculation($timeline, reset) {
    $timeline.each(function (index, item) {
        var $item = $(item);
        var $prev = undefined;

        $item.find(".t-event").each(function (i, e) {
            var $e = $(e);
            if (reset) {
                $e.css("height", "");
            } else {
                if (i % 3 === 0 && $e.next().length > 0) {
                    $e.height(50);
                } else if (i % 3 === 1) {
                    var prevCard = $prev.children(".card").height();
                    $e.height(prevCard);
                }

                $prev = $e;
            }
        });
    });
}

function directorySearchClearHandler() {
    var searchInput = document.getElementById('directory-search');
    if (!searchInput) return;

    var queryParam = 'q';

    function clearIfNeeded() {
        var urlParams = new URLSearchParams(window.location.search);
        if (!urlParams.has(queryParam)) {
            searchInput.value = '';
        }
    }

    window.addEventListener('pageshow', clearIfNeeded);
}

function setupMentorCheckboxBehavior() {
    var mentorCheckbox = document.getElementById("IsMentor");
    var menteeCheckbox = document.getElementById("IsOpenToNewMentees");

    if (!mentorCheckbox || !menteeCheckbox) {
        return;
    }

    menteeCheckbox.disabled = !mentorCheckbox.checked;

    mentorCheckbox.addEventListener("change", function () {
        menteeCheckbox.disabled = !this.checked;
        if (!this.checked) {
            menteeCheckbox.checked = false;
        }
    });
}

$(document).ready(function () {
    var $window = $(window);

    $("#search").keyup(function (event) {
        if (event.which === 13) {
            event.preventDefault();
            $("#gosearch").click();
        }
    });

    var $addMoreReviewers = $("#addMoreReviewers");
    if ($addMoreReviewers.length > 0) {
        (function () {
            var formGroupHtml = $("#addReviewerForm div.form-group")[0].outerHTML;
            $addMoreReviewers.click(function () {
                $addMoreReviewers.before(formGroupHtml);
            });
        })();
    }

    var $countdowns = $(".countdown");
    if ($countdowns.length > 0) {
        $countdowns.each(function (index, item) {
            var $item = $(item);
            var countDownDate = new Date($item.data("date"));
            var $days = $item.children(".countdown-days");
            var $hours = $item.children(".countdown-hours");
            var $minutes = $item.children(".countdown-minutes");
            var $seconds = $item.children(".countdown-seconds");

            var x = setInterval(function () {
                var now = new Date().getTime();
                var nowUTC = new Date(now + new Date().getTimezoneOffset() * 60000);
                var distance = countDownDate - nowUTC;
                var days = Math.floor(distance / (1000 * 60 * 60 * 24));
                var hours = Math.floor(distance % (1000 * 60 * 60 * 24) / (1000 * 60 * 60));
                var minutes = Math.floor(distance % (1000 * 60 * 60) / (1000 * 60));
                var seconds = Math.floor(distance % (1000 * 60) / 1000);

                $days.text(days);
                $hours.text(hours);
                $minutes.text(minutes);
                $seconds.text(seconds);

                if (distance <= 0) {
                    clearInterval(x);
                    $item.html("<span class='badge badge-danger'>Expired!</span>");
                }
            }, 1000);
        });
    }
    var $directory = $(".mvp-fp-directory");
    if ($directory.length > 0) {
        (function () {
            var $form = $directory.find("form");
            $form.on("change", "input:checkbox", function () {
                $form.submit();
            });

            // Handle search input clearing on back navigation
            directorySearchClearHandler();
        })();
    }

    var $timeline = $(".timeline");
    if ($timeline.length > 0) {
        setTimeout(timelineEventsHeightRecalculation($timeline, $window.width() < 576), 1000);
        setTimeout(timelineEventsHeightRecalculation($timeline, $window.width() < 576), 2000);
        setTimeout(timelineEventsHeightRecalculation($timeline, $window.width() < 576), 3000);
        setTimeout(timelineEventsHeightRecalculation($timeline, $window.width() < 576), 5000);
        setTimeout(timelineEventsHeightRecalculation($timeline, $window.width() < 576), 10000);

        $window.on("resize", function () {
            timelineEventsHeightRecalculation($timeline, $window.width() < 576);
        });
    }

    var $textareacounters = $(".textareacounter");
    if ($textareacounters.length > 0) {
        $textareacounters.each(function (index, item) {
            var $counter = $(item);
            var $textarea = $counter.siblings("textarea");
            var maxLength = $textarea.attr("maxlength");

            $textarea.on("input", function () {
                $counter.text($textarea.val().length + " / " + maxLength);
            });
        });
    }

    // Initialize country facets
    var countryFacets = document.querySelectorAll('[id^="country-search-"]');
    for (var i = 0; i < countryFacets.length; i++) {
        var input = countryFacets[i];
        var identifier = input.id.replace('country-search-', '');
        new CountryFacet(identifier);
    }

    var $mvpmentordata = $(".mvp-fs-mvpmentordata");
    if ($mvpmentordata.length > 0) {
        setupMentorCheckboxBehavior();
    }
});

/**
 * Country Facet Searchable Dropdown Module
 */
function CountryFacet(identifier) {
    this.searchInput = document.getElementById('country-search-' + identifier);
    this.dropdown = document.getElementById('country-dropdown-' + identifier);
    this.hiddenInput = document.getElementById('country-hidden-' + identifier);
    this.clearButton = document.getElementById('country-clear-' + identifier);

    if (!this.searchInput || !this.dropdown || !this.hiddenInput || !this.clearButton) {
        console.warn('CountryFacet: Required elements not found for identifier: ' + identifier);
        return;
    }

    this.isLocked = false;
    this.identifier = identifier;
    this.init();
}

CountryFacet.prototype.init = function () {
    this.setInitialValue();
    this.bindEvents();
};

CountryFacet.prototype.getDisplayValue = function (option) {
    var value = option.getAttribute('data-value');
    if (value === '') return '';

    var displayText = option.getAttribute('data-display');
    var countMatch = option.textContent.trim().match(/\((\d+)\)$/);

    return countMatch ? displayText + ' (' + countMatch[1] + ')' : displayText;
};

CountryFacet.prototype.selectCountry = function (option) {
    var selected = this.dropdown.querySelector('.dropdown-option.selected');
    if (selected) selected.classList.remove('selected');
    option.classList.add('selected');
    var value = option.getAttribute('data-value');
    this.hiddenInput.value = value;
    var label = option.querySelector('.country-label') ? option.querySelector('.country-label').textContent : option.getAttribute('data-display');
    var count = option.querySelector('.badge') ? option.querySelector('.badge').textContent : '';
    if (value !== '') {
        this.showSelectedDisplay(label, count);
        this.lockInput();
    } else {
        this.hideSelectedDisplay();
        this.unlockInput();
    }
    this.hiddenInput.form.submit();
};

CountryFacet.prototype.showDropdown = function () {
    this.dropdown.style.display = 'block';
    this.showAllOptions();
};

CountryFacet.prototype.hideDropdown = function () {
    this.dropdown.style.display = 'none';
};

CountryFacet.prototype.lockInput = function () {
    this.isLocked = true;
    this.searchInput.classList.add('locked');
    this.searchInput.readOnly = true;
    this.clearButton.classList.add('visible');
};

CountryFacet.prototype.unlockInput = function () {
    this.isLocked = false;
    this.searchInput.classList.remove('locked');
    this.searchInput.readOnly = false;
    this.clearButton.classList.remove('visible');
};

CountryFacet.prototype.clearSelection = function () {
    var allCountriesOption = this.dropdown.querySelector('.dropdown-option[data-value=""]');
    if (allCountriesOption) {
        this.selectCountry(allCountriesOption);
    }
};

CountryFacet.prototype.showAllOptions = function () {
    var options = this.dropdown.querySelectorAll('.dropdown-option');
    for (var i = 0; i < options.length; i++) {
        options[i].classList.remove('hidden');
    }
};

CountryFacet.prototype.filterOptions = function (searchTerm) {
    var options = this.dropdown.querySelectorAll('.dropdown-option');
    var lowerSearchTerm = searchTerm.toLowerCase();

    for (var i = 0; i < options.length; i++) {
        var matches = searchTerm === '' || options[i].textContent.toLowerCase().indexOf(lowerSearchTerm) !== -1;
        options[i].classList.toggle('hidden', !matches);
    }
};

CountryFacet.prototype.setInitialValue = function () {
    var selectedOption = this.dropdown.querySelector('.dropdown-option.selected');
    if (selectedOption && selectedOption.getAttribute('data-value') !== '') {
        var label = selectedOption.querySelector('.country-label') ? selectedOption.querySelector('.country-label').textContent : selectedOption.getAttribute('data-display');
        var count = selectedOption.querySelector('.badge') ? selectedOption.querySelector('.badge').textContent : '';
        this.showSelectedDisplay(label, count);
        this.lockInput();
    } else {
        this.hideSelectedDisplay();
        this.unlockInput();
    }
};

CountryFacet.prototype.bindEvents = function () {
    var self = this;

    this.searchInput.addEventListener('focus', function () {
        if (!self.isLocked) {
            self.showDropdown();
        }
    });

    this.searchInput.addEventListener('input', function (e) {
        if (!self.isLocked) {
            self.showDropdown();
            self.filterOptions(e.target.value);
        }
    });

    this.clearButton.addEventListener('click', function () {
        self.clearSelection();
    });

    document.addEventListener('click', function (e) {
        var isClickInside = self.searchInput.contains(e.target) || self.dropdown.contains(e.target) || self.clearButton.contains(e.target);
        if (!isClickInside) {
            self.hideDropdown();
        }
    });

    this.dropdown.addEventListener('click', function (e) {
        if (e.target.classList.contains('dropdown-option')) {
            self.selectCountry(e.target);
        }
    });
};

CountryFacet.prototype.showSelectedDisplay = function (label, count) {
    this.searchInput.style.display = 'none';
    this.dropdown.style.display = 'none';
    var selectedDisplay = document.getElementById('country-selected-' + this.identifier);
    if (selectedDisplay) {
        selectedDisplay.style.display = 'flex';
        selectedDisplay.querySelector('.country-selected-label').textContent = label;
        var countSpan = selectedDisplay.querySelector('.badge');
        if (count) {
            countSpan.textContent = count;
            countSpan.style.display = 'inline-block';
        } else {
            countSpan.textContent = '';
            countSpan.style.display = 'none';
        }
    }
};

CountryFacet.prototype.hideSelectedDisplay = function () {
    var selectedDisplay = document.getElementById('country-selected-' + this.identifier);
    if (selectedDisplay) {
        selectedDisplay.style.display = 'none';
    }
    this.searchInput.style.display = '';
};

