/*
    A module that abstracts away ajax implementation
*/
define(["jquery", "guard"], function ($, guard) {

    var $jqXHR = undefined;

    var ajaxRequest = function (type, url, data, dataType, contentType) { // Ajax helper
        var options = {
            dataType: dataType || "json",
            contentType: contentType || "application/x-www-form-urlencoded; charset=UTF-8",
            cache: false,//explicitly turn off browser caching of requests
            type: type,
            data: data ? data : null
        };
        var antiForgeryToken = $("#antiForgeryToken").val();
        if (antiForgeryToken) {
            options.headers = {
                'RequestVerificationToken': antiForgeryToken
            };
        }
        $jqXHR = $.ajax(url, options);
        return this;
    };

    var success = function (handler) {
        //handler(data)

        guard.is_not_null_or_undefined($jqXHR, "Need to call execute first");

        $jqXHR.done(function (data, textStatus, jqXHR) {
            handler(data);

            //Todo: remove this call in production
            require(["dom"], function (dom) {
                //Check the dom for duplicate ID's
                
                dom.validate_duplicate_ids();
            });
            
        });
        return this;
    };

    var fail = function (handler) {
        guard.is_not_null_or_undefined($jqXHR, "Need to call execute first");

        $jqXHR.fail(function (jqXHR, textStatus, errorThrown) {
            handler(jqXHR.responseText);
        });
        return this;
    };

    return {
        execute: ajaxRequest,
        onsuccess: success,
        onerror: fail
    };
});