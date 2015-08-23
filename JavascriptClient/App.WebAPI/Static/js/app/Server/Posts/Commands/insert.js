//Insert Post Command
define(['command', 'guard'], function (command, guard) {

    var _getUrl = function (id) {
        //todo: inject routemanager that can resolve routes from a pattern
        return baseUrl + "api/posts/" + (id || "");
    };


    return {
        execute: function (options) {
            guard.is_not_null_or_undefined(options);

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