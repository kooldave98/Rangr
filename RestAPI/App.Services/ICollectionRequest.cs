
namespace App.Services
{
    public interface ICollectionRequest
    {
        int start_index { get; set; }
        int max_results { get; set; }
        CollectionTraversal collection_traversal { get; set; }
        bool first_load { get; set; }
    }

    public enum CollectionTraversal
    {
        Newer,//Our default traversal
        Older
    }
}
