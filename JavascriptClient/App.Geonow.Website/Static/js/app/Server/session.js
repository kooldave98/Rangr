define(["global", "insertUser", "getUserById", "guard"], function (window, insertUser, getUserById, guard) {

    var localStorage = window.localStorage;

    var getUser = function () {
        return localStorage["user"] ? JSON.parse(localStorage["user"]) : undefined;
    };

    var setUser = function (user) {
        localStorage["user"] = JSON.stringify(user);
    };

    var clearUser = function () {
        localStorage.removeItem("user");
    };

    return {
        getUser: function (callback) {

            if (getUser()) {                

                if (callback) {
                    callback(getUser());

                }
            }
            else {

                var displayname;

                while (!displayname) {
                    displayname = prompt("Please, You have to enter your Display Name to begin using this service");
                }

                //get user by username from server
                //Todo: refactor this query for re-use
                insertUser.execute({
                    data: { user_display_name: displayname },
                    on_success: function (data) {
                        getUserById.execute({
                            data: { user_id: data.user_id },
                            on_success: function (user) {
                                setUser(user);

                                if (callback) {
                                    guard.is_not_null_or_undefined(getUser(), "User is null or undefined");
                                    callback(getUser());
                                }
                            }
                        });


                    },
                    on_failure: function () {
                        //Think about removing every error handler 
                        //and migrating to a global error mechanism: 
                        //possibly restart the application
                    }
                });

            }

        },
        clear: function () { }
    };

});


