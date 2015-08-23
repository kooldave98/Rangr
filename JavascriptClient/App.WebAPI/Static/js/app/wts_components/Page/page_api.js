define(["logger", "jquery", "controller", "presenter", "dom", "global", "modal"], function (logger, $, controller, presenter, dom, window, modal) {
    logger.log("page_api");

    var page_api = function () { };

    ///<Summary>
    ///Private methods
    ///</Summary>
    var show_notification_message = function (type, message) {

        var notification_body = "notification-warning";
        var notification_icon = "notification-warning-icon";

        if (type == "confirm") {
            notification_body = "notification-confirm";
            notification_icon = "notification-confirm-icon";
        }

        if (type == "warning") {
            notification_body = "notification-warning";
            notification_icon = "notification-warning-icon";
        }

        //Add the css classes
        $("#notifications").addClass(notification_body);
        $("#notification-icon").addClass(notification_icon);
        $("#notifications").append("<p>" + message + "</p>");
    };

    var navigate_to = function (url) {
        window.location.href = url;
    };

    var previous_page = function () {
        window.history.back();
    };




    // Disables all the views action.
    page_api.prototype.disable_view_actions = function () {
        $(".action").each(function () {
            dom.disableAction(this);
        });
    };

    // Enables all the view actions
    page_api.prototype.enable_view_actions = function () {
        $(".action").each(function () {
            dom.enableAction(this);
        });
    };

    // Disables all the views action.
    page_api.prototype.disable_view_actions_for = function (context_id) {
        $("#" + context_id + " " + +".action").each(function () {
            dom.disableAction(this);
        });
    };

    // Enables all the view actions
    page_api.prototype.enable_view_actions_for = function (context_id) {
        $("#" + context_id + " "+ +".action").each(function () {
            dom.enableAction(this);
        });
    };

    // submits the editor save action via ajax and then calls either the 
    // on_success or on_error handler based on the HttpStauts code
    // of the response.
    page_api.prototype.submit_edits = function (options) {

        var $editor_context = $("#" + options.editor_id);

        // to do: abstract this into a command module
        controller.submit_data({
            submit_url: $editor_context.attr("action"),
            submit_data: $editor_context.serialize(),
            on_success: options.on_success,
            on_error: options.on_error
        });
    };
    
    //Submits arbitrary data to a controller.
    page_api.prototype.submit_request = function (options) {

        // to do: abstract this into a command module
        controller.submit_data({
            submit_url: options.url,
            submit_data: options.data,
            on_success: options.on_success,
            on_error: options.on_error
        });
    };


    // Clears any messages that are currently displayed. 
    page_api.prototype.clear_all_messages = function (context) {
        //Clear previous errors if any
        $("#notifications").removeClass().empty();
        $("#notifications").append('<div id="notification-icon">');
        $("#notification-icon").removeClass();
        context = context || "body";
        var $context = $(context);
        $(".editor-input-error").removeClass("editor-input-error");
        $context.find("label.editor-label-error").empty().closest(".editor-label-errorwrapper").addClass("hidden");
    };

    // to do: (Refactoring) The common parts of the display confirmation and warning message should be extracted into a function
    // to do: (Refactoring) I think the confirm and waring messages should do exactly the same thing except the class that is
    //                      applied to the notification element.
    // to do: With field level warning we need to add add a style to the input control. 

    // Displays the messages as confirmation messages. It puts the messages in 
    // the standard field and page notification elements and sets them to the 
    // confirmation style.
    page_api.prototype.display_confirmation_messages = function (messages) {
        if (messages) {
            //loop and append the messages
            $.each(messages, function (index, object) {
                show_notification_message("confirm", object.validation_message);
            });
        }
    };

    // Displays the messages as confirmation messages. It puts the messages in 
    // the standard field and page notification elements and sets them to the 
    // confirmation style.
    page_api.prototype.display_error_messages = function (messages, editor_id) {
        if (messages) {
            //Loop and append the error messages
            $.each(messages, function (index, object) {
                if (object.field_name !== null && editor_id) {
                    $("#" + object.field_name).addClass("editor-input-error");
                    $("#" + editor_id).find("label[for='" + object.field_name + "'].editor-label-error")
                        .append(object.validation_message + "</br>").closest(".editor-label-errorwrapper").removeClass("hidden");
                } else {
                    show_notification_message("warning", object.validation_message);
                }
            });
        }
    };


    page_api.prototype.show_warning_message = function (message) {
        show_notification_message("warning", message);
    };

    page_api.prototype.show_confirmation_message = function (message) {
        show_notification_message("confirm", message);
    };

    // Remove the specified editor from the page.
    page_api.prototype.remove_editor = function (editor_id) {
        $("#" + editor_id).remove();
    };

    // will perform a full page refresh
    page_api.prototype.navigate_to = function (url) {
        navigate_to(url);
    };

    page_api.prototype.navigate_to_previous_page = function () {
        previous_page();
    };

    page_api.prototype.replace_report_with_editor = function (edit_report_details) {
        var report_context = $("#" + edit_report_details.id);
        var edit_report_action = edit_report_details.editor_url || report_context.find(".action.edit-report").attr("href");

        presenter.get_view({
            url: edit_report_action,
            on_success: function (content) {
                var editor = $(content);
                editor.attr("data-report-presenter", report_context.attr("data-report-presenter"));
                report_context.replaceWith(editor);
            },
            on_error: function () {
                //show_notification_message("warning", "An error occured, please try again later");
                navigate_to("/Error");
            }
        });
    };


    page_api.prototype.replace_editor_with_report = function (editor_id) {

        var editor_element = $("#" + editor_id);
        var report_presenter_url = editor_element.attr("data-report-presenter");

        //if the data-report-presenter attribute is empty
        //check the data-view-presenter
        presenter.get_view({
            url: report_presenter_url,
            on_success: function (content) {
                editor_element.replaceWith(content);
            },
            on_error: function () {
                //show_notification_message("warning", "An error occured, please try again later");
                navigate_to("/Error");
            }
        });
    };

    page_api.prototype.append_editor_to_report = function (new_report_details) {
        var report_context = $("#" + new_report_details.id);
        var new_report_action = new_report_details.editor_url;

        presenter.get_view({
            url: new_report_action,
            on_success: function (content) {
                var editor = $(content);
                report_context.after(editor);
            },
            on_error: function () {
                //show_notification_message("warning", "An error occured, please try again later");
                navigate_to("/Error");
            }
        });
    };


    page_api.prototype.refresh_details_list = function (context_id) {
        var details_list_context = $("#" + context_id).closest("section");
        var details_list_presenter_url = details_list_context.attr("data-report-presenter");

        presenter.get_view({
            url: details_list_presenter_url,
            on_success: function (content) {
                details_list_context.replaceWith(content);
            },
            on_error: function () {
                navigate_to("/Error");
            }
        });
    };


    // displays the content returned from the url in modal 
    page_api.prototype.display_editor_in_modal = function (remove_report_details) {

        var report_context = $("#" + remove_report_details.id);
        var remove_report_action = remove_report_details.editor_url || report_context.find(".action.new-report").attr("href");

        presenter.get_view({
            url: remove_report_action,
            on_success: function (content) {
                var editor = $(content);
                editor.attr("data-report-id", report_context.attr("id"));
                modal.showMessage(content);
            },
            on_error: function () {
                navigate_to("/Error");
            }
        });
    };

    //Closes the modal popup
    page_api.prototype.close_modal = function () {
        modal.close();
    };


    return new page_api();
});
