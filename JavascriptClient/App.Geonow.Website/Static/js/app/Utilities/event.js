/*
    A module that abstracts away event handler binding implementation
*/
define(["global", "jquery"], function (window, $) {
    //Inject the window and extract the document :)
    var document = window.document;
    var bind = function (event, elementSelector, action) {
        //We can always change the way we bind event handlers to elements this way
        //Todo: Introduce guards
        $(document).on(event, elementSelector, function (e) {
            var target = this;

            e.preventDefault();

            if (!$(target).attr("disabled")) {
                action(target);
            }

            return false;
        });
    };

    var bindOnReady = function (event, elementSelector, action) {
        //Not Implemented yet
    };

    var bindOnDelegate = function (event, elementSelector, action) {
        //Not Implemented yet
    };

    return {
        bind: bind,
        bindOnReady: bindOnReady,
        bindOnDelegate: bindOnDelegate
    };
});

