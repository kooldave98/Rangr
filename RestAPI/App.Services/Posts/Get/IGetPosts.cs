using App.Library.CodeStructures.Behavioral;
using System.Collections.Generic;

namespace App.Services.Posts.Get
{
    public interface IGetPosts : IQuery<GetPostsRequest, IEnumerable<PostDetails>>{ }    
}
