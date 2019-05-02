
using System;
using App.Library.CodeStructures.Behavioral;
using App.Library.Persistence;
using App.Persistence.Main;
using App.Services.Users;
using App.Services.Users.Create;

namespace App.Domain.Users.Create
{
    public class CreateUser : ICreateUser
    {
        private CreateUserRequest request;
        private readonly IEntityRepository<User> user_repository;
        private readonly IUnitOfWork unit_of_work;

        private User user;

        public CreateUser(IEntityRepository<User> the_user_repository
                        , IUnitOfWork the_unit_of_work)
        {
            user_repository = Guard.IsNotNull(the_user_repository, "the_user_repository");
            unit_of_work = Guard.IsNotNull(the_unit_of_work, "the_unit_of_work");
        }

        public UserIdentity execute(CreateUserRequest create_request)
        {
            SetRequest(create_request)
                .ValidateRequest()
                .AddUserToRepository()
                .Commit();

            return new UserIdentity() { user_id = user.ID };
        }

        private CreateUser SetRequest(CreateUserRequest create_request)
        {
            request = Guard.IsNotNull(create_request, "create_request");
            return this;
        }

        private CreateUser ValidateRequest()
        {
            if (string.IsNullOrWhiteSpace(request.user_display_name))
            {
                request.user_display_name = "Unnamed User";
            }

            if (request.user_display_name.Length > 15)
                throw new Exception("user_name exceeds 15 characters");

            return this;
        }

        private CreateUser AddUserToRepository()
        {
            user = new User
            {
                DisplayName = request.user_display_name
            };

            user_repository.add(user);
            return this;
        }

        private CreateUser Commit()
        {
            unit_of_work.Commit();
            return this;
        }
    }
}
