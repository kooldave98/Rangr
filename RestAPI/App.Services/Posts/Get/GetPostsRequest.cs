using App.Services.Connections;
using App.Services.HashTags;

namespace App.Services.Posts.Get
{
    /// <summary>
    /// The request parameters for retrieving a collection of Posts from the data store
    /// </summary>
    public class GetPostsRequest : ConnectionIdentity, ICollectionRequest, IHashTagRequest
    {
        public GetPostsRequest()
        {
            category = GetPostsQueryCategory.ByRadius;
            start_index = 0;
            max_results = 0;
            collection_traversal = CollectionTraversal.Newer;
            first_load = false;
        }

        public GetPostsQueryCategory category { get; set; }

        /// <summary>
        /// The index to start querying for results from
        /// </summary>
        public int start_index { get; set; }

        /// <summary>
        /// The maximum number of items to return
        /// </summary>
        public int max_results { get; set; }

        /// <summary>
        ///newer or older traversal
        /// </summary>
        public CollectionTraversal collection_traversal { get; set; }

        /// <summary>
        /// returns the latest entries on first load ignoring other params
        /// N/B: This overrides other request params
        /// </summary>
        public bool first_load { get; set; }

        /// <summary>
        /// This will only be read if the category has been set to hash_tag
        /// </summary>
        public string hash_tag_name { get; set; }
    }

    /// <summary>
    /// Note: Walkr is location based, and therefore one overarching default context is 
    /// that data is always returned in an order that is nearest to you, except otherwise stated. 
    /// </summary>
    public enum GetPostsQueryCategory
    {
        ByRadius,//Our default category, which is 200 metres
        ByHashTag,
    }
}
