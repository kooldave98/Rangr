/*
    A module that abstracts away logging implementation
*/
define(["global"], function (global) {
    var document = global.document;
    var console = global.console;

    var logger = function () {

        // default the console to have an empty logger function
        // if it does not exits.
        if (typeof console == "undefined") {
            console = {};
        }

        if (typeof console.log == "undefined") {

            console.log = function () {
                // Just swallow the message at this point in time 
                // as we do not have anywhere to report it.
            };
        }


        if (typeof console.warn == "undefined") {

            console.warn = function () {
                // Just swallow the message at this point in time 
                // as we do not have anywhere to report it.
            };
        }


        // if there is an element in the dom identified as logger then
        // write the message out to that as-well as the console.
        if ( document.getElementById ) {
            var page_log = document.getElementById( "logger" );

            if ( page_log ) {

                this.message = function ( message ) {
                    var para = document.createElement("p");
                    var node = document.createTextNode( message );
                    para.appendChild( node );
                    page_log.appendChild( para );
                    console.log( message );
                };
            }
        }
    };

    logger.prototype.log = function (message) {
        console.log(message);
    };
    
    logger.prototype.warn = function (message) {
        console.warn(message);
    };

    return new logger();
});