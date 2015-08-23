/*
    This is a facade over the HTML that provides the user with access to the actions that
    can be performed and events that the user can hook up to.

    The idea comes from the Humble Dialog design pattern by Michael Feathers.  The Page
    object represents the Humble Dialog aspect of the pattern.  The humble Dialog is 
    a contains no process intelligence it just knows how to perform actions 
    e.g. show some contents in a pop-up dialog.
    
    Notes -

    It may be worth replacing the homebrew event system with a more 
    robust library based implementation such as js-signal 
    (http://millermedeiros.github.io/js-signals/).

*/
define(["logger", "jquery", "underscore", "page_api", "event"], function (logger, $, _, page_api, event) {

    //Todo: remove this call in production
    require(["dom"], function (dom) {
        //Check the dom for duplicate ID's
        dom.validate_duplicate_ids();
    });


    logger.log("page_events");

    //Setup the world
    var events = {
        save_editor: {
            handlers: [],
            bind: function () {
                var handlers = this.handlers;

                event.bind("click", ".save-edits", function (target) {
                    //If it is located within a details list then don't execute
                    //There is a handler for save_details_list_editor
                    //We are only doing this because as at the current state of our architecture, 
                    //we haven't uniquely marked our components in a way that we can target them directly                    
                    if (!$(target).closest(".report-wrapper")[0]) {
                        var this_submit_button = target;
                        var editor_context = $(this_submit_button).closest("form");

                        editor_context.attr("action", $(this_submit_button).attr("formaction"));
                        var editor_id = editor_context.attr("id");

                        _.each(handlers, function (handler) {
                            handler(editor_id, page_api);
                        });
                    }
                });
            }
        },
        cancel_editor: {
            handlers: [],
            bind: function () {
                var handlers = this.handlers;

                event.bind("click", ".cancel-edits", function (target) {
                    //If it is located within a details list then don't execute
                    //There is a handler for save_details_list_editor
                    //We are only doing this because as at the current state of our architecture, 
                    //we haven't uniquely marked our components in a way that we can target them directly
                    if (!$(target).closest(".report-wrapper")[0]) {
                        var this_button = target;
                        var editor_context = $(this_button).closest("form");

                        var editor_id = editor_context.attr("id");

                        _.each(handlers, function (handler) {
                            handler(editor_id, page_api);
                        });
                    }
                });
            }
        },
        drill_into_entry: {
            handlers: [],
            bind: function () {
                var handlers = this.handlers;

                event.bind("click", "tbody > tr", function (target) {
                    var row = $(target);
                    var row_details = {
                        entry_id: row.attr("id"),
                        details_url: row.attr("data-details-url"),
                    };

                    _.each(handlers, function (handler) {
                        handler(row_details, page_api);
                    });
                });
            }
        },
        edit_entity: {
            handlers: [],
            bind: function () {
                var handlers = this.handlers;

                event.bind("click", ".report .edit-report", function (target) {

                    //If it is located within a details list then don't execute
                    //There is a handler for save_details_list_editor
                    //We are only doing this because as at the current state of our architecture, 
                    //we haven't uniquely marked our components in a way that we can target them directly
                    if (!$(target).closest(".report-wrapper")[0]) {
                        var edit_button = $(target);
                        var report = edit_button.closest(".report");

                        var edit_report_details = {
                            id: report.attr("id"),
                            editor_url: edit_button.attr("href"),
                        };

                        _.each(handlers, function (handler) {
                            handler(edit_report_details, page_api);
                        });
                    }
                });
            }
        },
        remove_entity: {
            handlers: [],
            bind: function () {
                var handlers = this.handlers;

                event.bind("click", ".report .remove-entity", function (target) {

                    var remove_button = $(target);
                    var report = remove_button.closest(".report");

                    var details = {
                        report_id: report.attr("id"),
                        action_url: remove_button.attr("href"),
                    };

                    _.each(handlers, function (handler) {
                        handler(details, page_api);
                    });

                });
            }
        },
        confirm_confirmation: {
            handlers: [],
            bind: function () {
                var handlers = this.handlers;

                event.bind("click", ".confirmation .confirm", function (target) {

                    var this_submit_button = target;
                    var editor_context = $(this_submit_button).closest("form");

                    editor_context.attr("action", $(this_submit_button).attr("formaction"));

                    var params = {
                        editor_id: editor_context.attr("id"),
                        report_origin_id: editor_context.attr("data-report-id")
                    };

                    _.each(handlers, function (handler) {
                        handler(params, page_api);
                    });

                });
            }
        },
        cancel_confirmation: {
            handlers: [],
            bind: function () {
                var handlers = this.handlers;

                event.bind("click", ".confirmation .cancel", function (target) {

                    var cancel_button = $(target);
                    var confirmation = cancel_button.closest(".confirmation");

                    var params = {
                        id: confirmation.attr("id"),
                        report_origin_id: confirmation.attr("data-report-id")
                    };

                    _.each(handlers, function (handler) {
                        handler(params, page_api);
                    });
                });
            }
        },
        //Details List Events
        save_details_list_editor: {
            handlers: [],
            bind: function () {
                var handlers = this.handlers;

                event.bind("click", ".report-wrapper .save-edits", function (target) {

                    var this_submit_button = target;
                    var editor_context = $(this_submit_button).closest("form");

                    editor_context.attr("action", $(this_submit_button).attr("formaction"));
                    var editor_id = editor_context.attr("id");

                    _.each(handlers, function (handler) {
                        handler(editor_id, page_api);
                    });
                });
            }
        },
        cancel_details_list_editor: {
            handlers: [],
            bind: function () {
                var handlers = this.handlers;

                event.bind("click", ".report-wrapper .cancel-edits", function (target) {

                    var this_button = target;
                    var editor_context = $(this_button).closest("form");

                    var editor_id = editor_context.attr("id");

                    _.each(handlers, function (handler) {
                        handler(editor_id, page_api);
                    });
                });
            }
        },
        new_details_list_entry: {
            handlers: [],
            bind: function () {
                var handlers = this.handlers;

                event.bind("click", ".details-list-header .new-entry", function (target) {

                    var new_button = $(target);
                    var report = new_button.closest(".details-list-header");

                    var new_report_details = {
                        id: report.attr("id"),
                        editor_url: new_button.attr("href"),
                    };

                    _.each(handlers, function (handler) {
                        handler(new_report_details, page_api);
                    });

                });
            }
        },
        edit_details_list_entry: {
            handlers: [],
            bind: function () {
                var handlers = this.handlers;

                event.bind("click", ".report-wrapper .report .edit-report", function (target) {
                    var edit_button = $(target);
                    var report = edit_button.closest(".report");

                    var edit_report_details = {
                        id: report.attr("id"),
                        editor_url: edit_button.attr("href"),
                    };

                    _.each(handlers, function (handler) {
                        handler(edit_report_details, page_api);
                    });

                });
            }
        },
        remove_details_list_entry: {
            handlers: [],
            bind: function () {
                var handlers = this.handlers;

                event.bind("click", ".report .remove-report", function (target) {

                    var remove_button = $(target);
                    var report = remove_button.closest(".report");

                    var details = {
                        report_id: report.attr("id"),
                        action_url: remove_button.attr("href"),
                    };

                    _.each(handlers, function (handler) {
                        handler(details, page_api);
                    });

                });
            }
        }
    };


    //Initialiser
    _.each(events, function (event_object) {
        event_object.bind();
    });

    //return object
    //Todo: DO - We need an on_leave event handler based on the discussion I had with Paul 
    //about how we want to clear forms that are left regardless of how they are left.
    return {
        on_save_edits: function (handler) {
            events.save_editor.handlers.push(handler);
        },
        on_drill_into_entry: function (handler) {
            events.drill_into_entry.handlers.push(handler);
        },
        on_cancel_edits: function (handler) {
            events.cancel_editor.handlers.push(handler);
        },
        on_edit_entity: function (handler) {
            events.edit_entity.handlers.push(handler);
        },
        on_remove_entity: function (handler) {
            events.remove_entity.handlers.push(handler);
        },
        on_confirm_confirmation: function (handler) {
            events.confirm_confirmation.handlers.push(handler);
        },
        on_cancel_confirmation: function (handler) {
            events.cancel_confirmation.handlers.push(handler);
        },
        //Details List specific events
        on_save_details_list_edits: function (handler) {
            events.save_details_list_editor.handlers.push(handler);
        },
        on_cancel_details_list_edits: function (handler) {
            events.cancel_details_list_editor.handlers.push(handler);
        },
        on_new_details_list_entry: function (handler) {
            events.new_details_list_entry.handlers.push(handler);
        },
        on_edit_details_list_entry: function (handler) {
            events.edit_details_list_entry.handlers.push(handler);
        },
        on_remove_details_list_entry: function (handler) {
            events.remove_details_list_entry.handlers.push(handler);
        },
    };
});