

using App.Library.CodeStructures.Behavioral;
using App.Library.Persistence;
using App.Persistence.Main;
using App.Services.Users;
using App.Services.Users.Update;
using System;
using System.Linq;

namespace App.Domain.Users.Edit
{
    public class UpdateUser : IUpdateUser
    {
        private UpdateUserRequest request;
        private readonly IEntityRepository<User> user_repository;
        private User user;
        private readonly IUnitOfWork unit_of_work;

        public UpdateUser(IEntityRepository<User> the_user_repository
                            , IUnitOfWork the_unit_of_work)
        {
            user_repository = Guard.IsNotNull(the_user_repository, "the_user_repository");
            unit_of_work = Guard.IsNotNull(the_unit_of_work, "the_unit_of_work");
        }

        public UserIdentity execute(UpdateUserRequest update_request)
        {
            SetRequest(update_request)
                .ValidateRequest()
                .GetUserToUpdate()
                .UpdateUserInRepository()
                .Commit();

            return new UserIdentity { user_id = update_request.user_id };
        }

        private UpdateUser SetRequest(UpdateUserRequest update_request)
        {
            request = Guard.IsNotNull(update_request, "update_request");
            return this;
        }

        private UpdateUser ValidateRequest()
        {
            if (string.IsNullOrWhiteSpace(request.user_display_name))
                throw new Exception("Invalid username");

            if (request.user_display_name.Length > 15)
                throw new Exception("user_name exceeds 15 characters");

            if (request.user_status_message.Length > 160)
                throw new Exception("status message exceeds 160 characters");

            return this;
        }

        private UpdateUser GetUserToUpdate()
        {
            user = user_repository.Entities.Single(u => u.ID == request.user_id);
            return this;
        }

        private UpdateUser UpdateUserInRepository()
        {
            user.DisplayName = request.user_display_name;
            user.StatusMessage = request.user_status_message;
            return this;
        }

        private UpdateUser Commit()
        {
            unit_of_work.Commit();
            return this;
        }

    }
}
