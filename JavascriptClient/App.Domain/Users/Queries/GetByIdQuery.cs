using App.Persistence.EF.Infrastructure;
using App.Service.Framework.Queries;
using System;
using System.Linq;

namespace App.Domain.Users.Queries
{
    public class GetByIdQuery : IQuery<Details, GetByIdRequest>
    {
        private readonly RepositorySet repositories;

        public GetByIdQuery()
        {
            this.repositories = new RepositorySet();
        }

        public Details Execute(GetByIdRequest request)
        {
            var user = repositories.Users.Entries.FirstOrDefault(d => d.ID == request.ID);
            if (user == null)
            {
                throw new Exception("User with this ID doesn't exist");
            }

            return new Details
            {
                Email = user.Email,
                ID = user.ID
            };
        }
    }

    public class GetByIdRequest: Identity
    {

    }
}
