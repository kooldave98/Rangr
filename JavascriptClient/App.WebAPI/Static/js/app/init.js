require(['session', 'geolocation'], function (session, geolocation) {
    session.authenticate();//pass callback to do something on success.
});

//We are only loading geolocation in so that the coordinates can be ready when we need it;
require(['streamviewmodel', 'presenter'], function (streamViewModel, presenter) {
    presenter.show("Stream.html", streamViewModel);
});