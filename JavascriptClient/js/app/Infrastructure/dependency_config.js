//  Holds all configuration for require js dependency management
requirejs.config({
    baseUrl: baseUrl,
    waitSeconds: 0,//default is 7, might wanna increase this shit
    paths: {

        // components
        "session": "Static/js/app/Server/session",
        "connection": "Static/js/app/Server/connection",
        "worker": "Static/js/app/Server/worker",//Mock of signal r                 
        "presenter": "Static/js/app/Server/presenter",

        //viewmodels
        "streamviewmodel": "Static/js/app/ViewModels/StreamViewModel",

        //Connection
        "create_connection": "Static/js/app/Server/Connections/insert",
        "update_connection": "Static/js/app/Server/Connections/update",
        "getconnectionsquery": "Static/js/app/Server/Connections/get",

        //Posts
        "insertpostcommand": "Static/js/app/Server/Posts/insert",
        "getpostsquery": "Static/js/app/Server/Posts/getByParams",


        //Users
        "getUserById": "Static/js/app/Server/Users/getById",
        "insertUser": "Static/js/app/Server/Users/insert",


        
        


        // utilities
        "command": "Static/js/app/Utilities/command",
        "query": "Static/js/app/Utilities/query",
        "geolocation": "Static/js/app/Utilities/geolocation",
        "global": "Static/js/app/Utilities/global",
        "logger": "Static/js/app/Utilities/logger",
        "event": "Static/js/app/Utilities/event",
        "ajax": "Static/js/app/Utilities/ajax",
        "guard": "Static/js/app/Utilities/guard",
        "dom": "Static/js/app/Utilities/dom",
        "modal": "Static/js/app/Utilities/modal",
        


        // libraries
        "jquery": "Static/js/library/jquery/jquery-1.10.2",
        "signalR": "Static/js/library/jquery.signalR/jquery.signalR-2.0.0",
        "knockout": "Static/js/library/knockout-js/knockout-2.3.0",
        "signalr.hubs": baseApiUrl + "signalr/hubs?noext",


        "underscore": "Static/js/library/underscore-js/underscore-min.1.5.1",
        "jqueryeasyModal": "Static/js/library/easyModal/jqueryeasyModal"
    },

    shim: {
        "signalR": ['jquery'],
        "signalr.hubs": ['jquery', 'signalR'],
    }


});
