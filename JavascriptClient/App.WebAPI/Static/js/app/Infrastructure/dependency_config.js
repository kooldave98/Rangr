//  Holds all configuration for require js dependency management
requirejs.config({
    baseUrl: baseUrl,

    paths: {

        //viewmodels
        "streamviewmodel": "Static/js/app/ViewModels/StreamViewModel",

        //Commands and Queries
        "insertpostcommand": "Static/js/app/Server/Posts/Commands/insert",
        "getpostsquery": "Static/js/app/Server/Posts/Queries/select",
        "command": "Static/js/app/Server/command",
        "query": "Static/js/app/Server/query",

        "geolocation": "Static/js/app/Utilities/geolocation",


        // components         
        "presenter": "Static/js/app/Utilities/presenter",


        // utilities
        "global": "Static/js/app/Utilities/global",
        "logger": "Static/js/app/Utilities/logger",
        "event": "Static/js/app/Utilities/event",
        "ajax": "Static/js/app/Utilities/ajax",
        "guard": "Static/js/app/Utilities/guard",
        "dom": "Static/js/app/Utilities/dom",
        "modal": "Static/js/app/Utilities/modal",
        "session": "Static/js/app/Utilities/session",

        //Mock of signal r
        "worker": "Static/js/app/Utilities/worker",


        // libraries
        "jquery": "Static/js/library/jquery/jquery-1.10.2",
        "signalR": "Static/js/library/jquery.signalR/jquery.signalR-1.1.3",
        "knockout": "Static/js/library/knockout-js/knockout-2.3.0",
        "signalr.hubs": "signalr/hubs?noext",


        "underscore": "Static/js/library/underscore-js/underscore-min.1.5.1",
        "jqueryeasyModal": "Static/js/library/easyModal/jqueryeasyModal"
    },

    shim: {
        "signalR": ['jquery'],
        "signalr.hubs": ['jquery', 'signalR'],
    }


});
