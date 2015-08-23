using App.Domain.Users;
using App.Domain.Users.Commands;
using App.Domain.Users.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace App.WebAPI.Controllers
{
    /// <summary>
    /// Restful controller actions for the 'Users' API
    /// </summary>
    public class UsersController : ApiController
    {
        private readonly GetByEmail GetUserByEmail;
        private readonly Insert InsertUserCommand;

        /// <summary>
        /// Constructor to initialise dependencies
        /// </summary>
        public UsersController()
        {
            GetUserByEmail = new GetByEmail();
            InsertUserCommand = new Insert();
        }

        
        /// <summary>
        /// Gets a user based on the specified request params 
        /// e.g GET api/users/?Email=david
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The 'Users' details object</returns>
        public Details Get([FromUri]GetByEmailRequest request)
        {
            Details user;
            try
            {
                //Get User by name
                user = GetUserByEmail.Execute(request);
            }
            catch (Exception)
            {
                //if the user doesn't exist
                var userID = InsertUserCommand.Execute(new InsertRequest() { Email = request.Email });

                user = GetUserByEmail.Execute(new GetByEmailRequest() { Email = userID.Email, ID = userID.ID });

            }
            return user;
        }

    }
}
