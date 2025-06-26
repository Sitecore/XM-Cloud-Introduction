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
    
    if (!this.searchInput || !this.dropdown || !this.hiddenInput) {
        console.warn('CountryFacet: Required elements not found for identifier: ' + identifier);
        return;
    }
    
    this.options = this.dropdown.querySelectorAll('.dropdown-option');
    this.cacheLoaded = false;
    this.CACHE_KEY = 'mvp_full_countries_list';
    this.CACHE_PROTECTED_KEY = 'mvp_cache_protected';
    this.MIN_FULL_LIST_SIZE = 10;
    
    this.init();
}

CountryFacet.prototype.init = function() {
    this.setupCache();
    this.loadCachedCountries();
    this.setInitialValue();
    this.bindEvents();
};

CountryFacet.prototype.setupCache = function() {
    if (this.options.length > this.MIN_FULL_LIST_SIZE) {
        this.clearCache();
    }
    this.updateCache();
};

CountryFacet.prototype.updateCache = function() {
    var cacheProtected = localStorage.getItem(this.CACHE_PROTECTED_KEY);
    
    if (this.options.length > this.MIN_FULL_LIST_SIZE && !this.cacheLoaded && !cacheProtected) {
        var countriesData = this.extractCountriesData();
        
        try {
            localStorage.setItem(this.CACHE_KEY, JSON.stringify(countriesData));
            localStorage.setItem(this.CACHE_PROTECTED_KEY, 'true');
        } catch (error) {
            console.warn('CountryFacet: Failed to cache countries data', error);
        }
    }
};

CountryFacet.prototype.extractCountriesData = function() {
    var self = this;
    return Array.from(this.options)
        .map(function(option) {
            var value = option.getAttribute('data-value');
            var display = option.getAttribute('data-display') || option.textContent.trim();
            var cleanDisplay = display.replace(/\s*\(\d+\)\s*$/, '');
            
            return { value: value, display: cleanDisplay, text: cleanDisplay };
        })
        .filter(function(country) { return country.value !== ''; })
        .filter(function(country, index, self) {
            return index === self.findIndex(function(c) { return c.value === country.value; });
        })
        .sort(function(a, b) { return a.display.localeCompare(b.display); });
};

CountryFacet.prototype.getCachedCountries = function() {
    try {
        var cached = localStorage.getItem(this.CACHE_KEY);
        return cached ? JSON.parse(cached) : null;
    } catch (error) {
        console.warn('CountryFacet: Failed to parse cached countries', error);
        return null;
    }
};

CountryFacet.prototype.clearCache = function() {
    localStorage.removeItem(this.CACHE_KEY);
    localStorage.removeItem(this.CACHE_PROTECTED_KEY);
};

CountryFacet.prototype.loadCachedCountries = function() {
    var cachedCountries = this.getCachedCountries();
    
    if (cachedCountries && this.options.length <= this.MIN_FULL_LIST_SIZE) {
        this.cacheLoaded = true;
        this.rebuildDropdown(cachedCountries);
    }
};

CountryFacet.prototype.rebuildDropdown = function(countries) {
    var selectedValue = this.hiddenInput.value;
    this.dropdown.innerHTML = '';
    
    var allCountriesOption = this.createAllCountriesOption(selectedValue);
    this.dropdown.appendChild(allCountriesOption);
    
    var self = this;
    countries.forEach(function(countryData) {
        var option = self.createDropdownOption(countryData, selectedValue);
        self.dropdown.appendChild(option);
    });
    
    this.options = this.dropdown.querySelectorAll('.dropdown-option');
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
    this.searchInput.value = value === '' ? '' : option.getAttribute('data-display');
    
    this.hideDropdown();
    this.hiddenInput.form.submit();
};

CountryFacet.prototype.showDropdown = function() {
    this.dropdown.style.display = 'block';
    this.showAllOptions();
};

CountryFacet.prototype.hideDropdown = function() {
    this.dropdown.style.display = 'none';
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
        this.searchInput.value = selectedOption.getAttribute('data-display') || '';
    } else {
        this.searchInput.value = '';
    }
};

CountryFacet.prototype.bindEvents = function() {
    var self = this;
    
    this.searchInput.addEventListener('focus', function() {
        self.showDropdown();
    });
    
    this.searchInput.addEventListener('input', function(e) {
        self.showDropdown();
        self.filterOptions(e.target.value);
    });
    
    document.addEventListener('click', function(e) {
        if (!self.searchInput.contains(e.target) && !self.dropdown.contains(e.target)) {
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