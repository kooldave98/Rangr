define(['knockout', 'insertpostcommand', 'getpostsquery', 'getconnectionsquery', 'guard', 'global', 'connection'],
    function (ko, insertpostcommand, getpostsquery, getconnectionsquery, guard, window, connection) {

        var posts = ko.observableArray([]);

        var start_index = 0;      



        var addPost = function () {
            console.log("send post called");
            var thisVM = this;
            connection.getConnectionId(function (connection_id) {
                console.log("get connection_id was called");
                console.log(connection_id);
                insertpostcommand.execute({
                    data: {
                        connection_id: connection_id,
                        text : thisVM.newPost()
                    },
                    on_success: function () {
                        thisVM.newPost("");
                    },
                    on_failure: function () {
                        console.log("An error has occured: StreamViewModel.JS line 27");
                    }
                });
            });
            

        };

        var getPosts = function () {
            connection.getConnectionId(function (connection_id) {
                getpostsquery.execute({
                    data: {
                        start_index: start_index,
                        connection_id: connection_id
                    },
                    on_success: function (data) {
                        if (data.length > 0) {
                            console.log(data);
                            var last = data[data.length - 1];
                            console.log(last);
                            start_index = last.post_id + 1;

                            posts.push.apply(posts, data);
                        }
                    },
                    on_failure: function () {
                        console.log("An error has occured: StreamViewModel.JS line 42");
                    }
                });
            });
        };



        var getConnectionsNearby = function () {
            connection.getConnectionId(function (connection_id) {
                getconnectionsquery.execute({
                    data: {                        
                        connection_id: connection_id
                    },
                    on_success: function (data) {
                        alert(data.length + " users nearby, see console for more details");
                        console.log(data);
                    },
                    on_failure: function () {
                        console.log("An error has occured: StreamViewModel.JS line 77");
                    }
                });
            });
        };



        return {
            newPost: ko.observable(""),
            addPost: addPost,
            getPosts: getPosts,
            getConnectionsNearby: getConnectionsNearby,
            posts: posts
        };
    });