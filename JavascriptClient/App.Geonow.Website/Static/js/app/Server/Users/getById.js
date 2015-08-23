//Get User by ID Query
define(['query', 'guard'], function (query, guard) {

    var _getUrl = function (id) {
        //todo: inject routemanager that can resolve routes from a pattern
        return baseApiUrl + "api/users/" + (id || "");
    };


    var guard_input = function (options) {
        guard.is_not_null_or_undefined(options, "options is not defined");
        guard.is_not_null_or_undefined(options.on_success, "success callback has not been defined");
        guard.is_not_null_or_undefined(options.data.user_id, "user_id is not defined");
    };


    return {
        execute: function (options) {

            guard_input(options);


            //Map to underlying query module
            query.execute({
                action: _getUrl(),
                data: options.data,
                on_success: options.on_success,
                on_failure: options.on_failure
            });
        }
    };


});