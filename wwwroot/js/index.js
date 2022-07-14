$(document).ready(function () {
    var $buyForm = $(".buy-form");

    $(".js-buy-button").each(function () {
        $(this).on("click", function () {
            $buyForm.slideToggle(300);
        });
    });
})
