define(["global", "signalr.hubs", "signalR", "jquery", "guard", "session", "geolocation"], function (window, hubs, signalR, $, guard, session, geolocation) {



    return {
        init: function (handler) {

            var userID = session.getUser().ID;
            //initially run
            handler();
            var stream = $.connection.streamHub;


            stream.client.pollServer = function () {
                handler();
            };

            geolocation.currentPosition(function (position) {
                $.connection.hub.qs = {
                    "userID": userID,
                    "geoLocation": position
                };

                $.connection.hub.start().done(function () {
                    geolocation.watchPosition(function (position) {
                        stream.server.updateLocation(position);
                    });

                });
            });
        }
    };


});