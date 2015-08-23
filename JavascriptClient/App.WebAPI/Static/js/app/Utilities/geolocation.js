define(["global", "guard", "jquery"], function (window, guard, $) {

    var navigator = window.navigator;

    var geo_value;
    var callbacks = [];
    var watchCallbacks = [];
    //inspectors
    var count = 0;
    $("body").prepend('<p id="geolog"></p>');

    //end inspectors

    //Initialises the devices geolocation
    navigator.geolocation.watchPosition(
        //On Success handler
        function (position) {
            geo_value = position.coords.longitude + "," + position.coords.latitude;

            $("#geolog").html(geo_value + " (" + count++ + ")");

            //currentPosition callbacks that may have been set before watch position fires
            if (callbacks.length > 0) {
                callbacks.forEach(function (callback) {
                    callback(geo_value);
                });

                callbacks = [];
            }





            //execute subscribers
            watchCallbacks.forEach(function (callback) {
                callback(geo_value);
            });

        },

        //On error handler
        function (error) {

            //Handle error getting geolocation data
            switch (error.code) {
                case error.PERMISSION_DENIED: alert("user did not share geolocation data");
                    break;
                case error.POSITION_UNAVAILABLE: alert("could not detect current position");
                    break;
                case error.TIMEOUT: alert("retrieving position timed out");
                    break;
                default: alert("unknown error");
                    break;
            }
        }
    );


    var longTask = function () {
        var array = [];
        for (var i = 0; i < 50; i++) {
            array.push("this is some long string");
        }
    };


    return {
        currentPosition: function (callback) {
            if (!geo_value) {
                callbacks.push(callback);
            } else {
                callback(geo_value);
            }
        },

        watchPosition: function (callback) {

            watchCallbacks.push(callback);

        }
    };

});