using App.Domain.Posts.Analyse;
using App.Library.CodeStructures.Behavioral;
using App.Library.Persistence;
using App.Persistence.Main;
using App.Services.Posts;
using App.Services.Posts.Create;
using System;
using System.Linq;
using App.Services.Static;

namespace App.Domain.Posts.Create
{
    public class CreatePost : ICreatePost
    {
        public PostIdentity execute(CreatePostRequest create_request)
        {
            set_request(create_request)                
                .find_connection()
                .validate_request()
                .add_post_to_repository()
                .extract_post_content()
                .commit();

            return new PostIdentity() { post_id = post.ID };
        }

        private CreatePost set_request(CreatePostRequest create_request)
        {
            request = Guard.IsNotNull(create_request, "create_request");

            return this;
        }

        private CreatePost find_connection()
        {
            Guard.IsNotNull(request, "request");

            // Until we get internal error handling sorted out
            // just throw an exception. Really this is a 
            // client error or a resource not found error
            // which the client can decide what to do with

            connection = connected_user_repository.Entities.Single(cu => cu.ID == request.connection_id);

            return this;
        }

        private CreatePost validate_request()
        {
            Guard.IsNotNull(request, "request");

            if (string.IsNullOrEmpty(request.text))
            {
                throw new Exception("Cannot add an empty post");
            }
                

            if (request.text.Length > 500)
            {
                throw new Exception("post exceeds 500 characters");

            }

            if (string.IsNullOrEmpty(request.image_id))
            {
                throw new Exception("Image is empty");
            }
                

            return this;
        }

        private CreatePost add_post_to_repository()
        {
            Guard.IsNotNull(request, "request");
            Guard.IsNotNull(connection.User, "connection.User");
            Guard.IsNotNull(connection.GeoLocation, "connection.GeoLocation");


            post = new Post
            {
                DateTime = Resources.current_date_time,
                Text = request.text,
                GeoLocation = connection.GeoLocation,
                image_id = request.image_id
            };

            //add the post
            connection.User.Posts.Add(post);

            return this;
        }

        private CreatePost extract_post_content()
        {
            Guard.IsNotNull(post, "post");

            analyse_post.execute(post);

            return this;
        }

        private CreatePost commit()
        {
            unit_of_work.Commit();
            return this;
        }

        private CreatePostRequest request;
        private readonly IEntityRepository<Connection> connected_user_repository;
        private readonly IUnitOfWork unit_of_work;
        private Post post;
        private Connection connection;

        public CreatePost(IEntityRepository<Connection> the_connected_user_repository
                            , IUnitOfWork the_unit_of_work
                            , AnalysePost the_analyse_post)
        {
            connected_user_repository = Guard.IsNotNull(the_connected_user_repository, "the_connected_user_repository");
            unit_of_work = Guard.IsNotNull(the_unit_of_work, "the_unit_of_work");

            analyse_post = Guard.IsNotNull(the_analyse_post, "the_analyse_post");
        }

        private readonly AnalysePost analyse_post;
    }

}
