define(["global", "guard"], function (window, guard) {

    var repeater = window.setInterval;


    return {
        init: function (handler) {
            handler();

            repeater(function () {
                handler();
            }, 2000);//2 seconds
        }
    };


});