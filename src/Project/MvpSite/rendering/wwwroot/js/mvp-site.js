$(document).ready(function () {
    $("#search").keyup(function (event) {
        if (event.which === 13) {
            event.preventDefault();
            $("#gosearch").click();
        }
    });


    var $addMoreReviewers = $("#addMoreReviewers");
    if ($addMoreReviewers.length > 0) {
        var formGroupHtml = $("#addReviewerForm div.form-group")[0].outerHTML;
        $addMoreReviewers.click(function () {
            $addMoreReviewers.before(formGroupHtml);
        });
    }    
});