//Select Posts Query
define(['query', 'guard'], function (query, guard) {

    var _getUrl = function (id) {
        //todo: inject routemanager that can resolve routes from a pattern
        return baseApiUrl + "api/posts/" + (id || "");
    };

    var guard_input = function (options) {
        guard.is_not_null_or_undefined(options, "options is not defined");
        guard.is_not_null_or_undefined(options.on_success, "success callback has not been defined");
        guard.is_not_null_or_undefined(options.data.connection_id, "connection_id is not defined");
        guard.is_not_null_or_undefined(options.data.start_index, "start_index is not defined");
    };


    return {
        execute: function (options) {

            //Map to underlying command module
            query.execute({
                action: _getUrl(),
                data: options.data,
                on_success: options.on_success,
                on_failure: options.on_failure
            });
        }
    };


});