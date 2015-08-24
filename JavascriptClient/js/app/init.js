require(['session', 'geolocation', 'connection'], function (session, geolocation, connection) {
    session.getUser(function (user) {
        connection.getConnectionId(function (connection_id) {
            console.log("init: connection ID = " + connection_id);

            connection.init_heartbeat();

            require(['streamviewmodel', 'presenter'], function (streamViewModel, presenter) {
                presenter.show("Stream.html", streamViewModel);
            });

        });                
    });
});