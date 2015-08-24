define([], function () {

    var iscomplete = false;

    var callbacks = [];

    var args = [];

    var resolveCallbacks = function () {
        callbacks.forEach(function (callback) {
            callback(args);
        });
        callbacks = [];
    };

    return {
        complete: function () {
            args = arguments;
            resolveCallbacks();
            iscomplete = true;
        },
        then: function (callback) {
            if (iscomplete) {
                callback(args);
            } else {
                callbacks.push(callback);
            }
            return this;
        }
    };
});

(function () {

    var promise = (function () {
        var prom = new Promise();
        geolocation.getPosition(function (position) {
            prom.complete(position, time);
        });
        return prom;
    }());

    var promise2;

    promise.then(function () {
        var arguments = arguments[0];
        promise2 = (function () {
            var prom = new Promise();
            ajax.exceute(position, function (data) {
                prom.complete(data);
            })
            return prom;
        }());
    });

    promise2.then(function (data) {
        //do sometyhing with data
    });

    promise.then(function (position) {
        console.log(position);
    });

}());