/*
        This a application logic for a standard details view i.e. a details view presents
        one or more reports each of which can have zero or more actions such as edit, 
        remove, new.  
        
        This module defines our standard implementation of that journey. It is
        the sole responsibility of the view to implement the journey application
        all manipulation of view and identification of action requests from the 
        user are delegated to a Page object.
    
    */
require(["logger", "page_events"], function (logger, page_events) {

    page_events.on_edit_entity(function (edit_report_details, page_api) {
        page_api.clear_all_messages();
        page_api.disable_view_actions();
        page_api.replace_report_with_editor(edit_report_details);
    });

    //on submit editor
    page_events.on_save_edits(function (editor_id, page) {
        page.disable_view_actions_for(editor_id);
        page.clear_all_messages();
        page.submit_edits({
            editor_id: editor_id,
            on_success: function (response) {
                // happy case 
                page.display_confirmation_messages(response.messages, editor_id);
                page.replace_editor_with_report(editor_id);
                page.enable_view_actions();
            },
            on_error: function (response) {
                //sad case
                response = JSON.parse(response);
                page.display_error_messages(response.messages, editor_id);
                page.enable_view_actions_for(editor_id);
            }
        });
    });

    //on cancel editor
    page_events.on_cancel_edits(function (editor_id, page) {
        page.clear_all_messages();
        page.replace_editor_with_report(editor_id);
        page.enable_view_actions();
    });

    //When the remove action is clicked on a details list
    page_events.on_remove_entity(function (details, page_api) {
        page_api.clear_all_messages();
        page_api.disable_view_actions();
        page_api.submit_request({
            url: details.action_url,
            data: {},
            on_success: function (response) {
                // happy case 
                page_api.navigate_to_previous_page(editor_id);
                page_api.enable_view_actions();
            },
            on_error: function (response) {
                //sad case
                response = JSON.parse(response);
                page_api.display_error_messages(response.messages);
                page_api.enable_view_actions();
            }
        });
    });

});