require(["logger", "page_events"], function (logger, page_events) {

    var view = function () {
        page_events.on_save_edits(function (editor_id, page) {
            page.disable_view_actions_for(editor_id);
            page.clear_all_messages();
            page.submit_edits({
                editor_id: editor_id,
                on_success: function (response) {
                    // happy case 
                    page.display_confirmation_messages(response.messages, editor_id);
                    page.remove_editor(editor_id);
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

        page_events.on_cancel_edits(function (editor_id, page) {
            page.navigate_to_previous_page(editor_id);
            page.enable_view_actions();
        });
    };

    //No need to return anything here, just execute
    (view());
});