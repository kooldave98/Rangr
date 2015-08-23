//http://stackoverflow.com/questions/17718673/how-is-a-promise-defer-library-implemented
//https://github.com/kriskowal/q/blob/master/design/README.js

var Promise = function () {
    this.okCallbacks = [];
    this.koCallbacks = [];
};

Promise.prototype = {
    okCallbacks: null,
    koCallbacks: null,
    status: 'pending',
    error: null,

    then: function (okCallback, koCallback) {
        var defer = new Defer();

        // Add callbacks to the arrays with the defer binded to these callbacks
        this.okCallbacks.push({
            func: okCallback,
            defer: defer
        });

        if (koCallback) {
            this.koCallbacks.push({
                func: koCallback,
                defer: defer
            });
        }

        // Check if the promise is not pending. If not call the callback
        if (this.status === 'resolved') {
            this.resolveCallback({
                func: okCallback,
                defer: defer
            }, this.data);
        } else if (this.status === 'rejected') {
            this.executeCallback({
                func: koCallback,
                defer: defer
            }, this.error);
        }

        return defer.promise;
    },

    executeCallback: function (callbackData, result) {
        window.setTimeout(function () {
            var res = callbackData.func(result);
            if (res instanceof Promise) {
                callbackData.defer.bind(res);
            } else {
                callbackData.defer.resolve(res);
            }
        }, 0);
    }
};







var Defer = function () {
    this.promise = new Promise();
};

Defer.prototype = {
    promise: null,
    resolve: function (data) {
        var promise = this.promise;
        promise.data = data;
        promise.status = 'resolved';
        promise.okCallbacks.forEach(function (callbackData) {
            promise.executeCallback(callbackData, data);
        });
    },

    reject: function (error) {
        var promise = this.promise;
        promise.error = error;
        promise.status = 'rejected';
        promise.koCallbacks.forEach(function (callbackData) {
            promise.executeCallback(callbackData, error);
        });
    },

    // Make this promise behave like another promise:
    // When the other promise is resoved/rejected this is also resolved/rejected
    // with the same data
    bind: function (promise) {
        var that = this;
        promise.then(function (res) {
            that.resolve(res);
        }, function (err) {
            that.reject(err);
        });
    }
};



function ajax() {
    var deferred = new Defer();
    // an example of an async call
    ajax.execute(function (request) {
        if (request.status === 200) {
            deferred.resolve(request.responseText);
        } else {
            deferred.reject(new Error("Status code was " + request.status));
        }
    });
    return deferred.promise;
}

ajax().then(function (text) {
    alert(text);
}, function (error) {
    alert(error.message);
});


(function () {


    var sendPositionToServer = function(position) {


    };




    var getLocation = function () {
        var prom = new Promise();
        geolocation.getPosition(function (position) {
            prom.complete(position, time);
        });
        return prom;
    };


    getLocation().then();

    promise2.then(function (data) {
        //do sometyhing with data
    });

    promise.then(function (position) {
        console.log(position);
    });

}());