//Insert User command
define(['command', 'guard'], function (command, guard) {

    var _getUrl = function (id) {
        //todo: inject routemanager that can resolve routes from a pattern
        return baseApiUrl + "api/users/" + (id || "");
    };


    return {
        execute: function (options) {

            //Map to underlying query module
            command.execute({
                action: _getUrl(),
                method: "POST",
                data: options.data,
                on_success: options.on_success,
                on_failure: options.on_failure
            });
        }
    };


});