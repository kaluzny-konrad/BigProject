$(document).ready(function () {

    console.log("Hello");

    var $buyForm = $(".buy-form");
    var $buyFormExit = $(".buy-form-exit");
    var $loginToggle = $(".login-toggle");
    var $loginForm = $(".login-form");
    var $loginFormExit = $(".login-form-exit");

    $(".buyButton").each(function () {
        $(this).on("click", function () {
            console.log("Buying Item");
            $loginForm.hide();
            $buyForm.hide();
            $blocker.toggle();
            $buyForm.slideToggle(300);
        });
    });

    var productInfo = $(".mug-info li");
    productInfo.on("click", function () {
        console.log("You clicked on " + $(this).text());
    });

    $loginToggle.on("click", function () {
        $buyForm.hide();
        $loginForm.hide();
        $loginForm.slideToggle(300);
        $blocker.toggle();
    })

    $buyFormExit.on("click", function () {
        $buyForm.slideToggle(300);
        $blocker.hide();
    })

    $loginFormExit.on("click", function () {
        $loginForm.slideToggle(300);
        $blocker.hide();
    })

    var $blocker = $(".blocker");
    $blocker.on("click", function () {
        $buyForm.hide();
        $loginForm.hide();
        $blocker.hide();
    })
})
