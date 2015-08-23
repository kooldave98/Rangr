define(["global", "signalr.hubs", "signalR", "jquery", "guard", "session", "geolocation"], function (window, hubs, signalR, $, guard, session, geolocation) {

    var connectionReady = false;

    var callbacks = [];

    var stream;

    return {
        init: function (handler) {

            //initially run
            //rather than going back to the server
            //tombstone the posts from the client storage
            //tombstone();
            //handler(0);
            //need to remove above line and replace with resurrect();

            geolocation.currentPosition(function (position) {
                session.getUser(function (user) {
                    $.connection.hub.url = baseApiUrl + 'signalr';
                    $.connection.hub.logging = true;

                    $.connection.hub.qs = {
                        "userID": user.ID,
                        "geoLocation": position
                    };

                    stream = $.connection.streamHub;

                    stream.client.appendPost = function (post) {
                        handler(post);
                    };

                    stream.client.joins = function(userID, userName, dateTime){
                        console.log(userName + "joined at: " + dateTime);
                    };

                    stream.client.leaves = function (userID, userName, dateTime) {
                        console.log(userName + "left at: " + dateTime)
                    };


                    $.connection.hub.start().done(function () {
                        connectionReady = true;

                        geolocation.watchPosition(function (position) {
                            stream.server.updateLocation(position);
                        });


                        if (callbacks.length > 0) {
                            
                            callbacks.forEach(function (callback) {
                                callback(stream);
                            });

                            callbacks = [];
                        }

                    });

                });
            });
        },
        sendPost: function (callback) {
            if (!connectionReady) {
                callbacks.push(callback);
            } else {
                guard.is_not_null_or_undefined(stream, "stream is null or undefined");
                callback(stream);
            }
        }
    };


});