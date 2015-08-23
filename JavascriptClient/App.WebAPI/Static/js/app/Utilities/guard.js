/*
    A module that abstracts away null checking
*/
define([], function () {
    //Todo: Create an Exceptions module that will be injected in
    function NullorUndefinedException(message) {
        this.message = message;
        this.name = "NullorUndefinedException";
        this.toString = function () {
            return this.name + ": " + this.message;
        };
    }

    return {
        is_not_null_or_undefined: function (value, message) {
            if (value == undefined) {
                throw new NullorUndefinedException(message);
            }

            if (value == null) {
                throw new NullorUndefinedException(message);
            }

            return value;
        }
    };
});