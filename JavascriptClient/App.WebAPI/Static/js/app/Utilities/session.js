define(["global", "query"], function (window, query) {

    var localStorage = window.localStorage;

    var user;

    return {
        authenticate: function (callback) {
            user = localStorage["user"] ? JSON.parse(localStorage["user"]) : undefined;

            if (!user) {

                var username = user ? user.Email : undefined;

                while (!username || username == "") {
                    username = prompt("Please, You have to enter your Username to begin using this service");
                }

                //get user by username from server
                //Todo: refactor this query for re-use
                query.execute({
                    action: baseUrl + "api/users/",
                    data: { Email: username },
                    on_success: function (data) {
                        user = data;
                        //Migrate to this at some point
                        localStorage["user"] = JSON.stringify(data);

                        if (callback) {
                            callback(user);
                        }

                    },

                    on_failure: function () {
                        //Think about removing every error handler 
                        //and migrating to a global error mechanism: 
                        //possibly restart the application
                    }
                });

            }

        },
        //Unsafe method, refactor at some point
        getUser: function () {
            return user;
        },
        clear: function () { }
    };

});


