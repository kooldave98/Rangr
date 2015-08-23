using App.Persistence.EF.Infrastructure;
using App.Persistence.Models;
using App.Service.Framework.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using App.GeoNow.Library.Utilities;

namespace App.Domain.Posts.Commands
{
    public class Insert : ICommand<InsertRequest, Identity>
    {
        private InsertRequest request;
        private readonly RepositorySet repositories;
        Post post;

        public Insert()
        {
            this.repositories = new RepositorySet();
        }

        public Identity Execute(InsertRequest create_request)
        {
            this.SetRequest(create_request)
                .ValidateRequest()
                .AddPostToRepository()
                .Commit();

            return new Identity() { ID = post.ID };
        }

        private Insert SetRequest(InsertRequest create_request)
        {
            request = create_request;
            return this;
        }

        private Insert ValidateRequest()
        {
            var user = repositories.Users.Entries.FirstOrDefault(d => d.ID == request.UserID);
            if (user == null)
                throw new Exception(string.Format("No user exists with ID {0}", request.UserID));

            if (string.IsNullOrEmpty(request.Text) )
                throw new Exception("Cannot add an empty post");

            return this;
        }        

        private Insert AddPostToRepository()
        {
            post = new Post
            {
                Date = DateTime.Now,
                Text = request.Text,
                UserID = request.UserID,
                GeoLocation = request.GeoLocation.ToDbGeography()
            };
            this.repositories.Posts.Add(post);
            return this;
        }

        private Insert Commit()
        {
            this.repositories.CommitChanges();
            return this;
        }
    }
}
