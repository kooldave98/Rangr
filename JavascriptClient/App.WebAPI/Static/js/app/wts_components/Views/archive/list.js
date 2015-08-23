require(["logger", "page_events"], function (logger, page_events) {
    logger.log("list");

    page_events.on_drill_into_entry(function (entry, page) {

        page.navigate_to(entry.details_url);
    });

});