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
});

