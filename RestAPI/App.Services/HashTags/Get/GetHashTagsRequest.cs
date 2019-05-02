
namespace App.Services.HashTags.Get
{
    /// <summary>
    /// The request parameters for retrieving a collection of HashTags from the data store
    /// </summary>
    public class GetHashTagsRequest : HashTagIdentity, ICollectionRequest
    {
        public GetHashTagsRequest()
        {
            category = GetHashTagsQueryCategory.ByTrending;
            start_index = 0;
            max_results = 0;
            collection_traversal = CollectionTraversal.Newer;
            first_load = false;
        }

        public GetHashTagsQueryCategory category { get; set; }

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
    }

    public enum GetHashTagsQueryCategory
    {
        ByTrending
    }
}
