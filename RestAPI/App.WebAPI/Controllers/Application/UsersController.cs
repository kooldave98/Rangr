using System.Web.Http;
using App.Library.CodeStructures.Behavioral;
using App.Services.Users;
using App.Services.Users.Create;
using App.Services.Users.GetById;
using App.Services.Users.Update;
using App.WebAPI.Controllers.Infrastructure;

namespace App.WebAPI.Controllers.Application
{
    /// <summary>
    /// Restful controller actions for the 'Users' API
    /// </summary>
    public class UsersController : BaseController
    {
        private readonly IGetUserById get_user_by_id_query;
        private readonly ICreateUser new_user_command;
        private readonly IUpdateUser update_user_command;

        /// <summary>
        /// Constructor to initialise dependencies
        /// </summary>
        public UsersController(IGetUserById the_get_user_by_id_query
                                ,ICreateUser the_new_user_command
                                ,IUpdateUser the_update_user_command)
        {
            get_user_by_id_query = Guard.IsNotNull(the_get_user_by_id_query, "the_get_user_by_id_query");
            new_user_command = Guard.IsNotNull(the_new_user_command, "the_new_user_command");
            update_user_command = Guard.IsNotNull(the_update_user_command, "the_update_user_command");
        }

        /// <summary>
        /// Gets a user based on the specified request params 
        /// e.g GET api/users/?ID=43
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The 'Users' details object</returns>
        public UserDetails Get([FromUri]UserIdentity request)
        {
            //Get User by name
            var user = get_user_by_id_query.execute(request);

            return user;
        }

        /// <summary>
        /// Adds a new User with details based on the request body params.
        /// e.g POST api/users/
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The 'User' as it reflects internally at the current time.</returns>
        public UserIdentity Post([FromBody]CreateUserRequest request)
        {
            var userID = new_user_command.execute(request);
            return userID;
        }

        /// <summary>
        /// Updates a user resource based on the request body params
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The identity of the user resource updated</returns>
        public UserIdentity Put([FromBody]UpdateUserRequest request)
        {
            return update_user_command.execute(request);
        }

    }
}
