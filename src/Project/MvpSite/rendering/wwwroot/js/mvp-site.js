$(document).ready(function () {
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
});