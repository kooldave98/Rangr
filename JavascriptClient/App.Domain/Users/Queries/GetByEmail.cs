using App.Persistence.EF.Infrastructure;
using App.Service.Framework.Queries;
using System;
using System.Linq;

namespace App.Domain.Users.Queries
{
    /// <summary>
    /// We are using the Email as the user's name (JFYI)
    /// </summary>
    public class GetByEmail : IQuery<Details, GetByEmailRequest>
    {
        private readonly RepositorySet repositories;

        public GetByEmail()
        {
            this.repositories = new RepositorySet();
        }

        public Details Execute(GetByEmailRequest request)
        {
            var user = repositories.Users.Entries.FirstOrDefault(d => d.Email == request.Email);
            if (user == null)
            {
                throw new Exception("User with this email doesn't exist");
            }

            return new Details
            {
                Email = user.Email,
                ID = user.ID,
            };
        }
    }


    /// <summary>
    /// The request parameters required to retrieve a User by their Email
    /// </summary>
    public class GetByEmailRequest : Identity{ }


}
