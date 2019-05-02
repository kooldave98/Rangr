using App.Library.CodeStructures.Behavioral;
using System.Collections.Generic;

namespace App.Services.HashTags.Get
{
    public interface IGetHashTags : IQuery<GetHashTagsRequest, IEnumerable<HashTagDetails>> { }
}
