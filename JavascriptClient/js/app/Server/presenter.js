
define(['query', 'guard', 'knockout', 'jquery'], function (query, guard, ko, $) {

    var _getUrl = function (id) {
        //todo: inject routemanager that can resolve routes from a pattern
        return baseUrl + "Static/html/Pages/" + (id || "Stream.html");
    }


    return {
        show: function (viewname, viewmodel) {
            //Map to underlying query module
            query.execute({
                action: _getUrl(viewname),
                data: null,
                dataType: "html",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                on_success: function (data) {
                    var $page_context = $("#app-content");
                    $page_context.html(data);
                    ko.applyBindings(viewmodel, $page_context[0]);
                },
                on_failure: function () { }
            });
        }
    };


});