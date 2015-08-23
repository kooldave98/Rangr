define(["global", "guard", "session", "create_connection", "update_connection", "geolocation"],
function (window, guard, session, create_connection, update_connection, geolocation) {

    var connection_id;

    return {
        getConnectionId: function (handler) {

            if (connection_id) {
                handler(connection_id);
            } else {
                session.getUser(function (user) {

                    geolocation.currentPosition(function (position, accuracy) {

                        create_connection.execute({
                            data: {
                                user_id: user.user_id,
                                long_lat_acc_geo_string: position + "," + accuracy
                            },
                            on_success: function (data) {

                                //update conn_id incase it has been changed on the server
                                connection_id = data.connection_id;
                                handler(connection_id);
                            },
                            on_failure: function () {
                                console.log("failed creating connection");
                                //handler("FakeID>>>>>");
                            }
                        });
                    });
                });
            }

        },
        init_heartbeat: function () {
            guard.is_not_null_or_undefined(connection_id, "connection_id is not defined");

            //movement threshold heartbeat
            geolocation.watchPosition(function (position, accuracy) {
                update_connection.execute({
                    data: {
                        connection_id: connection_id,
                        long_lat_acc_geo_string: position + "," + accuracy
                    },
                    on_success: function (data) {
                        connection_id = data.connection_id;
                    }
                });
            });

        }

    };


});