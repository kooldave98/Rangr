/*
    A module that abstracts view presentation logic
*/
define(["ajax", "guard"], function (ajax, guard) {


    return {
        execute: function (options) {

            //guard condition
            guard.is_not_null_or_undefined(options);

            //execute xhr
            ajax.execute(options.method, options.action, options.data)
                    .onsuccess(options.on_success)
                    .onerror(options.on_failure);
        }
    };
});