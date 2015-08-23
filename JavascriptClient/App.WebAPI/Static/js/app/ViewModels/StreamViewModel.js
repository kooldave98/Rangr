define(['knockout', 'insertpostcommand', 'getpostsquery', 'worker', 'geolocation', 'session'], function (ko, insertpostcommand, getpostsquery, worker, geolocation, session) {

    var posts = ko.observableArray([]);


    //The query to be repeated
    var start = 0;

    //load posts and subscribe for changes
    //search for paged querying
    //Todo: nice to have syntax ==>> worker.for(posts).subscribe(function(){});
    worker.init(function () {
        geolocation.currentPosition(function (position) {
            getpostsquery.execute({
                data: {
                    StartIndex: start,
                    GeoLocationString: position
                },
                on_success: function (data) {
                    if (data.length != 0) {
                        var last = data[data.length - 1];
                        start = last.ID + 1;
                        //posts(data);
                        posts.push.apply(posts, data);//Append to existing array
                    }
                },
                on_failure: function () {
                }
            });
        });

    });


    var addPost = function () {
        var thisVM = this;
        console.log();

        geolocation.currentPosition(function (position) {
            insertpostcommand.execute({
                data: {
                    Text: thisVM.newPost(),
                    UserID: session.getUser().ID,
                    GeoLocation: position
                },
                on_success: function () {
                    //Clear text box on submit
                    thisVM.newPost("");
                },
                on_failure: function () {
                    //Notify the UI somehow
                }
            });
        });

    };



    return {
        newPost: ko.observable(""),
        addPost: addPost,
        posts: posts
    };
});