/*
    A module that abstracts away DOM element selection, traversal and manipulation implementation
*/
define(["jquery", "logger"], function ($, logger) {

    function disableAction(elem, name) {
        var $elem = $(elem);
        $elem.attr("disabled", "disabled");
        var action = $elem.attr(name);
        $elem.data(name, action);
        $elem.removeAttr(name);
    }

    function enableAction(elem, name) {
        var $elem = $(elem);
        $elem.removeAttr("disabled");
        var action = $elem.data(name);
        $elem.attr(name, action);
    }

    return {
        disableAction: function (dom_element) {
            var actionAttribute = dom_element.nodeName == "INPUT" ? "formaction" : "href";
            disableAction(dom_element, actionAttribute);
        },
        enableAction: function (dom_element) {
            var actionAttribute = dom_element.nodeName == "INPUT" ? "formaction" : "href";
            enableAction(dom_element, actionAttribute);
        },
        validate_duplicate_ids: function () {
            // Warning Duplicate IDs
            $('[id]').each(function () {
                var ids = $('[id=\'' + this.id + '\']');
                if (ids.length > 1 && ids[0] == this)
                    logger.warn('Multiple IDs #' + this.id);
            });
        }
    };
});