
using App.Library.CodeStructures.Behavioral;
using App.Library.Persistence;
using App.Persistence.Main;
using App.Services.Users;
using App.Services.Users.GetById;
using System;
using System.Linq;

namespace App.Domain.Users.GetById
{
    public class GetUserById : IGetUserById
    {

        public GetUserById(IQueryRepository<User> the_user_repository)
        {
            user_repository = Guard.IsNotNull(the_user_repository, "the_user_repository");
        }

        public UserDetails execute(UserIdentity request)
        {
            Guard.IsNotNull(request, "request");
            var user = user_repository.Entities.SingleOrDefault(d => d.ID == request.user_id);

            if (user == null)
            {
                throw new Exception("User with this ID doesn't exist");
            }

            return new UserDetails
            {
                user_display_name = user.DisplayName,
                user_status_message = user.StatusMessage,
                user_id = user.ID
            };
        }

        private readonly IQueryRepository<User> user_repository;
    }
}
