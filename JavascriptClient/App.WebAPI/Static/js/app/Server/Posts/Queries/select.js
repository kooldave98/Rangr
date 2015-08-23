//Select Posts Query
define(['query', 'guard'], function (query, guard) {

    var _getUrl = function (id) {
        //todo: inject routemanager that can resolve routes from a pattern
        return baseUrl + "api/posts/" + (id || "");
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