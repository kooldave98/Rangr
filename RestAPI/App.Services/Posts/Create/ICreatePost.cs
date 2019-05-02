using App.Library.CodeStructures.Behavioral;

namespace App.Services.Posts.Create
{
    public interface ICreatePost : ICommand<CreatePostRequest, PostIdentity> { }
}
