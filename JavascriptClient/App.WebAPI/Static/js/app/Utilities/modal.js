/*
    A Modal popup module that abstracts away 
    vendor specific modal implementation
    How to use documentation can be found here http://flaviusmatis.github.io/easyModal.js/
*/
define(["jquery", "jqueryeasyModal"], function ($) {
    var execute = function (content) {
        // to do: Introduce Guard condition here i.e check if element exists
        $("body").append('<div class="modal-popup">' + content + "</div>");
        $(".modal-popup").easyModal({
            top: 100,
            autoOpen: true,
            overlayOpacity: 0.3,
            overlayColor: "#333",
            overlayClose: true,
            closeOnEscape: true,
            onOpen: function (myModal) {
                $(myModal).append(content);
            },
            onClose: function (myModal) {
                $(myModal).remove();
                $(".lean-overlay").remove();
            }
        });
    };

    var closeModal = function () {
        $(".modal-popup").trigger("closeModal");
    };

    return {
        showMessage: execute,
        close: closeModal
    };
});