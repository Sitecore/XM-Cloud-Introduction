$(document).ready(function () {

    $("#search").keyup(function (event) {
        if (event.which === 13) {
            event.preventDefault();
            $("#gosearch").click();
        }
    });
    
    if (document.getElementById("application-form") == null) {
        return;
    }

    $(document).ajaxSend(function () {
        $("#overlay").fadeIn(300);
    });

    fillApplicationList();
    getApplicationInfo();
});

function fillApplicationList() {
    $.ajax({
        type: "GET",
        url: "/Application/GetApplicationLists",
        async: false,
        data: {

        },
        success: function (data) {
            fillDropLists(data.Country, 'Country', 'Name');
            fillDropLists(data.EmploymentStatus, 'EmploymentStatus', 'Name');
            fillDropLists(data.MvpCategory, 'mvpcategory', 'Name');
        },
        error: function (result) {
            console.error(result);
        }
    });
}

function fillDropLists(items, dropId, title) {
    var dropLowerCaseId = dropId.toLowerCase();
    $("select[asp-for='" + dropLowerCaseId + "']").append("<option selected disabled value=''>--Select--</option>");

    $.each(items, function (i, item) {
        if (typeof item.Active === 'undefined' || item.Active) {
            $("select[asp-for='" + dropLowerCaseId + "']").append("<option value='" + item['ID'] + "'>" + item[title] + "</option>");
        }
    });
}

function getApplicationInfo() {
	$.ajax({
		type: "GET",
        url: "/Application/GetApplicationInfo",
        async: false,
        success: function (data) {

            if (!data.userIsAuthenticated) {
				window.location = '/Application/Intro';
                return;
            }
				
            setStep('#step_welcome');

            $('.application-visibility').show();
			$("#overlay").fadeOut();
		},
		error: function (result) {

			console.error(result);
			$("#overlay").fadeOut();
		}
    });
}

function setStep(stepId) {

    $('.appStep').attr("hidden", true);
    $(stepId).attr("hidden", false);

    stepCount = $(stepId).attr('data-step');
    for (var i = stepCount; i >= 1; i--) {
        $("#progressbar").find('[data-step="' + i + '"]').addClass('active');
    }

    currentStepId = stepCount;
    setProgressBar(stepCount);
}

function setProgressBar(curStep) {
    var steps = $(".fieldSet").length;
    var percent = parseFloat(100 / steps) * curStep;
    percent = percent.toFixed();
    $(".progress-bar")
        .css("width", percent + "%")
}