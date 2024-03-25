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
});