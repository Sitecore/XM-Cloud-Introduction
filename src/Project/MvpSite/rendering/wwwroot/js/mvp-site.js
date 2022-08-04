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

    $("#btnStep1").click(function (event) {
        switchFormTab(event, "#btnStep1", '#form_step1', () => setStep('#step_category'));
    });

    $("#btnStep2").click(function (event) {
        switchFormTab(event, "#btnStep2", '#categoryForm', () => setStep('#step_personal'));
    });

    $("#btnStep3").click(function (event) {
        switchFormTab(event, "#btnStep3", '#personalForm', () => setStep('#step_objectives'));
    });

    $("#btnStep4").click(function (event) {
        switchFormTab(event, "#btnStep4", '#objectivesForm', () => setStep('#step_socials'));
    });

    $("#btnStep5").click(function (event) {
        switchFormTab(event, "#btnStep5", '#socialForm', () => setStep('#step_contributions'));
    });

    $("#btnStep6").click(function (event) {
        switchFormTab(event, "#btnStep6", '#contributionForm', () => setStep('#step_confirmation'));
    });

    $("#btnStep7").click(function (event) {
        switchFormTab(event, "#btnStep7", '#confirmationForm', () => window.location.href = "/thank-you");
    });
});

function switchFormTab(event, buttonSelector, formSelector, successAction) {
    $(buttonSelector).attr("disabled", true);
    event.preventDefault();

    var form = document.querySelectorAll(formSelector)[0];
    if (!form.checkValidity()) {
        event.stopPropagation()
    }
    else {
        successAction();

        setTimeout(function () {
            $("#overlay").fadeOut(300);
        }, 500);
    }

    form.classList.add('was-validated');

    $(buttonSelector).attr("disabled", false);
}

function fillApplicationList() {
    $.ajax({
        type: "GET",
        url: "/Application/GetApplicationLists",
        async: false,
        data: {

        },
        success: function (data) {
            fillDropLists(data.country, 'Country', 'name');
            fillDropLists(data.employmentStatus, 'EmploymentStatus', 'name');
            fillDropLists(data.mvpCategory, 'mvpcategory', 'name');
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
    $(".progress-bar").css("width", percent + "%")
}

function getPrevStep() {
    if (currentStepId > 2) {
        $("#progressbar").find('[data-step="' + currentStepId + '"]').removeClass('active');

        currentStepId--;
        var stepIdid = $("div[data-step='" + currentStepId + "']").attr('id');
        setStep('#' + stepIdid);
    }
}