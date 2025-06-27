function timelineEventsHeightRecalculation($timeline, reset) {
    $timeline.each(function (index, item) {
        const $item = $(item);
        let $prev;
            
        $item.find(".t-event").each(function (i, e) {
            let $e = $(e);
            if (reset) {
                $e.css("height", "");
            }
            else {                
                if (i % 3 === 0 && $e.next().length > 0) {
                    $e.height(50);
                }
                else if (i % 3 === 1) {
                    let prevCard = $prev.children(".card").height();
                    $e.height(prevCard);
                }

                $prev = $e;
            }            
        });            
    });
}

function directorySearchClearHandler() {
    const searchInput = document.getElementById('directory-search');
    if (!searchInput) return;
    
    const queryParam = 'q';
    
    function clearIfNeeded() {
        const urlParams = new URLSearchParams(window.location.search);
        if (!urlParams.has(queryParam)) {
            searchInput.value = '';
        }
    }
    
    window.addEventListener('pageshow', clearIfNeeded);

}


function setupMentorCheckboxBehavior() {
    const mentorCheckbox = document.getElementById("IsMentor");
    const menteeCheckbox = document.getElementById("IsOpenToNewMentees");

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
    const $window = $(window);


    $("#search").keyup(function (event) {
        if (event.which === 13) {
            event.preventDefault();
            $("#gosearch").click();
        }
    });


    const $addMoreReviewers = $("#addMoreReviewers");
    if ($addMoreReviewers.length > 0) {
        const formGroupHtml = $("#addReviewerForm div.form-group")[0].outerHTML;
        $addMoreReviewers.click(function () {
            $addMoreReviewers.before(formGroupHtml);
        });
    }


    const $countdowns = $(".countdown");
    if ($countdowns.length > 0) {
        $countdowns.each(function (index, item) {
            const $item = $(item);
            const countDownDate = new Date($item.data("date"));
            const $days = $item.children(".countdown-days");
            const $hours = $item.children(".countdown-hours");
            const $minutes = $item.children(".countdown-minutes");
            const $seconds = $item.children(".countdown-seconds");

            const x = setInterval(function () {
                let now = new Date().getTime();
                let nowUTC = new Date(now + new Date().getTimezoneOffset() * 60000);
                let distance = countDownDate - nowUTC;
                let days = Math.floor(distance / (1000 * 60 * 60 * 24));
                let hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                let minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                let seconds = Math.floor((distance % (1000 * 60)) / 1000);

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
    const $directory = $(".mvp-fp-directory");
    if ($directory.length > 0) {
        const $form = $directory.find("form");
        $form.on("change", "input:checkbox", () => { $form.submit(); });
        
        // Handle search input clearing on back navigation
        directorySearchClearHandler();
    }


    const $timeline = $(".timeline");
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


    const $textareacounters = $(".textareacounter");
    if ($textareacounters.length > 0) {
        $textareacounters.each(function (index, item) {
            const $counter = $(item);
            const $textarea = $counter.siblings("textarea");
            const maxLength = $textarea.attr("maxlength");
            
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

    const $mvpmentordata = $(".mvp-fs-mvpmentordata");
    if ($mvpmentordata.length > 0) {
        setupMentorCheckboxBehavior();
    }
});


/**
 * Country Facet Searchable Dropdown Module
 */
function CountryFacet(identifier) {
    this.identifier = identifier;
    this.searchInput = document.getElementById('country-search-' + identifier);
    this.dropdown = document.getElementById('country-dropdown-' + identifier);
    this.hiddenInput = document.getElementById('country-hidden-' + identifier);
    this.clearButton = document.getElementById('country-clear-' + identifier);
    
    if (!this.searchInput || !this.dropdown || !this.hiddenInput || !this.clearButton) {
        console.warn('CountryFacet: Required elements not found for identifier: ' + identifier);
        return;
    }
    
    this.options = this.dropdown.querySelectorAll('.dropdown-option');
    this.isLocked = false;
    
    this.init();
}

CountryFacet.prototype.init = function() {
    this.setInitialValue();
    this.bindEvents();
};

CountryFacet.prototype.createAllCountriesOption = function(selectedValue) {
    var div = document.createElement('div');
    div.className = 'dropdown-option' + (selectedValue === '' ? ' selected' : '');
    div.setAttribute('data-value', '');
    div.setAttribute('data-display', 'All Countries');
    div.textContent = 'All Countries';
    
    return div;
};

CountryFacet.prototype.createDropdownOption = function(countryData, selectedValue) {
    var div = document.createElement('div');
    div.className = 'dropdown-option';
    div.setAttribute('data-value', countryData.value);
    div.setAttribute('data-display', countryData.display);
    div.textContent = countryData.text;
    
    if (countryData.value === selectedValue) {
        div.classList.add('selected');
    }
    
    return div;
};

CountryFacet.prototype.selectCountry = function(option) {
    var options = this.dropdown.querySelectorAll('.dropdown-option');
    for (var i = 0; i < options.length; i++) {
        options[i].classList.remove('selected');
    }
    option.classList.add('selected');
    
    var value = option.getAttribute('data-value');
    this.hiddenInput.value = value;
    
    // Set display value with count if available
    if (value === '') {
        this.searchInput.value = '';
    } else {
        var displayText = option.getAttribute('data-display');
        var optionText = option.textContent.trim();
        
        // Check if the option text contains count in parentheses
        var countMatch = optionText.match(/\((\d+)\)$/);
        if (countMatch) {
            this.searchInput.value = displayText + ' (' + countMatch[1] + ')';
        } else {
            this.searchInput.value = displayText;
        }
    }
    
    this.hideDropdown();
    
    // Lock or unlock the input based on selection
    if (value !== '') {
        this.lockInput();
    } else {
        this.unlockInput();
    }
    
    this.hiddenInput.form.submit();
};

CountryFacet.prototype.showDropdown = function() {
    this.dropdown.style.display = 'block';
    this.showAllOptions();
};

CountryFacet.prototype.hideDropdown = function() {
    this.dropdown.style.display = 'none';
};

CountryFacet.prototype.lockInput = function() {
    this.isLocked = true;
    this.searchInput.classList.add('locked');
    this.searchInput.readOnly = true;
    this.clearButton.classList.add('visible');
};

CountryFacet.prototype.unlockInput = function() {
    this.isLocked = false;
    this.searchInput.classList.remove('locked');
    this.searchInput.readOnly = false;
    this.clearButton.classList.remove('visible');
};

CountryFacet.prototype.clearSelection = function() {
    // Reset to "All Countries" selection
    var allCountriesOption = this.dropdown.querySelector('.dropdown-option[data-value=""]');
    if (allCountriesOption) {
        this.selectCountry(allCountriesOption);
    }
};

CountryFacet.prototype.showAllOptions = function() {
    var options = this.dropdown.querySelectorAll('.dropdown-option');
    for (var i = 0; i < options.length; i++) {
        options[i].classList.remove('hidden');
    }
};

CountryFacet.prototype.filterOptions = function(searchTerm) {
    if (searchTerm === '') {
        this.showAllOptions();
        return;
    }
    
    var lowerSearchTerm = searchTerm.toLowerCase();
    var options = this.dropdown.querySelectorAll('.dropdown-option');
    for (var i = 0; i < options.length; i++) {
        var matches = options[i].textContent.toLowerCase().indexOf(lowerSearchTerm) !== -1;
        if (matches) {
            options[i].classList.remove('hidden');
        } else {
            options[i].classList.add('hidden');
        }
    }
};

CountryFacet.prototype.setInitialValue = function() {
    var selectedOption = this.dropdown.querySelector('.dropdown-option.selected');
    if (selectedOption && selectedOption.getAttribute('data-value') !== '') {
        var displayText = selectedOption.getAttribute('data-display');
        var optionText = selectedOption.textContent.trim();
        
        // Check if the option text contains count in parentheses
        var countMatch = optionText.match(/\((\d+)\)$/);
        if (countMatch) {
            this.searchInput.value = displayText + ' (' + countMatch[1] + ')';
        } else {
            this.searchInput.value = displayText;
        }
        
        this.lockInput();
    } else {
        this.searchInput.value = '';
        this.unlockInput();
    }
};

CountryFacet.prototype.bindEvents = function() {
    var self = this;
    
    this.searchInput.addEventListener('focus', function() {
        if (!self.isLocked) {
            self.showDropdown();
        }
    });
    
    this.searchInput.addEventListener('input', function(e) {
        if (!self.isLocked) {
            self.showDropdown();
            self.filterOptions(e.target.value);
        }
    });
    
    this.clearButton.addEventListener('click', function() {
        self.clearSelection();
    });
    
    document.addEventListener('click', function(e) {
        if (!self.searchInput.contains(e.target) && !self.dropdown.contains(e.target) && !self.clearButton.contains(e.target)) {
            self.hideDropdown();
        }
    });
    
    // Use event delegation to handle all dropdown options (existing and dynamically created)
    this.dropdown.addEventListener('click', function(e) {
        if (e.target.classList.contains('dropdown-option')) {
            self.selectCountry(e.target);
        }
    });
};