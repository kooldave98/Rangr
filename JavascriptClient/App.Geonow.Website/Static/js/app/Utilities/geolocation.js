define(["global", "guard", "jquery"], function (window, guard, $) {

    var navigator = window.navigator;

    //Locations are all 180 metres apart on a straight line
    var locations = {
        A: "-2.229798, 53.477738",
        B: "-2.229396, 53.479345",
        C: "-2.227427, 53.480470",
        D: "-2.224769, 53.480107",
    };

    var isLive = true;
    var live_geo_value_copy;
    var geo_value;
    var geo_accuracy = 1;
    var callbacks = [];
    var watchCallbacks = [];
    //inspectors
    var count = 0;

    $("body").prepend('<input id="chkD" type="radio" name="emu" value="D">Point D<br>');
    $("body").prepend('<input id="chkC" type="radio" name="emu" value="C">Point C<br>');
    $("body").prepend('<input id="chkB" type="radio" name="emu" value="B">Point B<br>');
    $("body").prepend('<input id="chkA" type="radio" name="emu" value="A">Point A<br>');

    $("body").prepend('<input type="radio" checked name="emu" value="L">Live: <p id="geolog"></p>');
    //end inspectors


    var func = function (key) {
        if (key == "L") {
            isLive = true;
            localStorage.removeItem("point");
            geo_value = live_geo_value_copy;
            if (geo_value) {
                resolveCallbacks();
            }
        } else {
            isLive = false;
            localStorage["point"] = key;
            geo_value = getLocationAt(key);
            resolveCallbacks();
        }
    };

    $(document).on("click", "input[type=radio]", function () {
        var key = $(this).val();
        func(key);

    });


    var getLocationAt = function (key) {
        return locations[key];
    };


    var resolveCallbacks = function () {
        //currentPosition callbacks that may have been set before watch position fires
        if (callbacks.length > 0) {
            guard.is_not_null_or_undefined(geo_value, "Geolocation is null or undefined");
            callbacks.forEach(function (callback) {
                callback(geo_value, geo_accuracy);
            });

            callbacks = [];
        }

        //execute subscribers
        guard.is_not_null_or_undefined(geo_value, "Geolocation is null or undefined");
        watchCallbacks.forEach(function (callback) {
            callback(geo_value, geo_accuracy);
        });
    };


    if (localStorage["point"]) {
        var key = localStorage["point"];
        $("#chk" + key).prop('checked', true);
        func(key);
    }

    //Initialises the devices geolocation
    navigator.geolocation.watchPosition(
        //On Success handler
        function (position) {
            geo_accuracy = 1;
            live_geo_value_copy = position.coords.longitude + "," + position.coords.latitude;
            $("#geolog").html(live_geo_value_copy + " (" + count++ + ")");

            if (isLive) {
                console.log(position.coords.accuracy);
                geo_accuracy = position.coords.accuracy;
                console.log(geo_accuracy);
                geo_value = live_geo_value_copy;
                resolveCallbacks();
            }

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


    return {
        currentPosition: function (callback) {
            if (!geo_value) {
                callbacks.push(callback);
            } else {
                guard.is_not_null_or_undefined(geo_value, "Geolocation is null or undefined");
                callback(geo_value, geo_accuracy);
            }
        },

        watchPosition: function (callback) {

            watchCallbacks.push(callback);

        }
    };

});


