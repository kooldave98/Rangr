require(["logger", "page_events"], function (logger, page_events) {

    //When the new action is clicked on a details list
    page_events.on_new_details_list_entry(function (details, page_api) {
        page_api.clear_all_messages();
        page_api.disable_view_actions();
        page_api.append_editor_to_report(details);
    });

    //When the remove action is clicked on a details list
    page_events.on_remove_details_list_entry(function (details, page_api) {
        page_api.clear_all_messages();
        page_api.disable_view_actions();
        page_api.submit_request({
            url: details.action_url,
            data: {},
            on_success: function (response) {
                // happy case 
                page_api.display_confirmation_messages(response.messages);
                page_api.refresh_details_list(details.report_id);
                page_api.enable_view_actions();
            },
            on_error: function (response) {
                //sad case
                response = JSON.parse(response);
                page_api.display_error_messages(response.messages);
                page_api.refresh_details_list(details.report_id);
                page_api.enable_view_actions();
            }
        });
    });

    //When the edit action is clicked on a report
    page_events.on_edit_details_list_entry(function (details, page_api) {
        page_api.clear_all_messages();
        page_api.disable_view_actions();
        page_api.replace_report_with_editor(details);
    });

    //When the save action is clicked on an editor in a details list
    page_events.on_save_details_list_edits(function (editor_id, page) {
        page.disable_view_actions_for(editor_id);
        page.clear_all_messages();
        page.submit_edits({
            editor_id: editor_id,
            on_success: function (response) {
                // happy case 
                page.display_confirmation_messages(response.messages, editor_id);
                page.refresh_details_list(editor_id);
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

    //When the cancel action is clicked on an editor in a details list
    page_events.on_cancel_details_list_edits(function (editor_id, page) {
        page.clear_all_messages();
        page.refresh_details_list(editor_id);
        page.enable_view_actions();
    });


    //When the confirm confirmation is clicked on an editor
    page_events.on_confirm_confirmation(function (details, page) {
        page.disable_view_actions_for(details.editor_id);
        page.clear_all_messages();
        page.submit_edits({
            editor_id: details.editor_id,
            on_success: function (response) {
                // happy case 
                page.closeModal();
                page.refresh_details_list(details.report_origin_id);
                page.enable_view_actions();
            },
            on_error: function (response) {
                //sad case
                page.closeModal();
                response = JSON.parse(response);
                page.display_error_messages(response.messages, editor_id);
                page.enable_view_actions();
            }
        });
    });

    //When the cancel confirmation is clicked on an editor
    page_events.on_cancel_confirmation(function (details, page) {
        page.close_modal();
        page.clear_all_messages();
        page.refresh_details_list(details.report_origin_id);
        page.enable_view_actions();
    });

});