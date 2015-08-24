//Insert Post Command
define(['command', 'guard'], function (command, guard) {

    var _getUrl = function (id) {
        //todo: inject routemanager that can resolve routes from a pattern
        return baseApiUrl + "api/posts/" + (id || "");
    };


    var guard_input = function (options) {
        guard.is_not_null_or_undefined(options, "options is not defined");
        guard.is_not_null_or_undefined(options.on_success, "success callback has not been defined");
        guard.is_not_null_or_undefined(options.data.connection_id, "connection_id is not defined");
        guard.is_not_null_or_undefined(options.data.text, "text is not defined");
    };



    return {
        execute: function (options) {
            guard_input(options);

            //Map to underlying command module
            command.execute({
                method: "POST",
                action: _getUrl(),
                data: options.data,
                on_success: options.on_success,
                on_failure: options.on_failure
            });
        }
    };


});