using App.Persistence.EF.Infrastructure;
using App.Persistence.Models;
using App.Service.Framework.Commands;
using System;
using System.Linq;

namespace App.Domain.Users.Commands
{
    public class Insert : ICommand<InsertRequest, Identity>
    {
        private InsertRequest request;
        private readonly RepositorySet repositories;
        private User user;

        public Insert()
        {
            this.repositories = new RepositorySet();
        }

        public Identity Execute(InsertRequest create_request)
        {
            SetRequest(create_request)
                .ValidateRequest()
                .AddUserToRepository()
                .Commit();

            return new Identity() { ID = user.ID, Email = user.Email };
        }

        private Insert SetRequest(InsertRequest create_request)
        {
            request = create_request;
            return this;
        }

        private Insert ValidateRequest()
        {

            if (String.IsNullOrWhiteSpace(request.Email))
                throw new Exception("Invalid username");

            if (UsernameExists())
                throw new Exception("Username exists");

            return this;
        }

        private Insert AddUserToRepository()
        {
            user = new User
            {
                Email = request.Email,
                Password = request.Password
            };
            this.repositories.Users.Add(user);
            return this;
        }

        private bool UsernameExists()
        {
            return repositories.Users.Entries.FirstOrDefault(d => d.Email.ToLower() == request.Email.ToLower()) != null;
        }

        private void Commit()
        {
            this.repositories.CommitChanges();
        }
    }


    public class InsertRequest
    {
        //Todo: to be removed later, this is just here because we are not even authenticating by passwords
        public InsertRequest()
        {
            Password = "Password";
        }

        public string Password { get; set; }
        public string Email { get; set; }
    }

}
