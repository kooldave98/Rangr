define(["ajax", "guard"], function (ajax, guard) {


    return {
        execute: function (options) {

            //guard condition
            guard.is_not_null_or_undefined(options);

            //execute xhr
            ajax.execute("GET", options.action, options.data, options.dataType, options.contentType, options.cache)
                    .onsuccess(options.on_success)
                    .onerror(options.on_failure);
        }
    };
});