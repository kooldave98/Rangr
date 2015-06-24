using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;

namespace App.Common
{
    //GET api/posts?
    //start_index={start_index}
    //&max_results={max_results}
    //&collection_traversal={collection_traversal}
    //&first_load={first_load}
    //&user_id={user_id}
    //&mobile_number_id={mobile_number_id}
    public class GetPosts
    {
        public async Task<List<Post>> Get(GetPostsByMutualRequest request)
        {
            var posts = new List<Post>();

            var url = String.Format("{0}/?start_index={1}&user_id={2}"
                , PostResources.base_rest_url
                , request.start_index
                , request.user_id);

            if (request.collection_traversal == CollectionTraversal.Older)
            {
                url = url + "&collection_traversal=Older";
            }

            if (request.first_load)
            {
                url = url + "&first_load=true";
            }

            try
            {
                string data = await PostResources.httpRequest.Get(url);
                posts = JsonConvert.DeserializeObject<List<Post>>(data);

//                if (first_load)
//                {
//                    //Clear all entries
//                    EntityDatabase.Current.GetItems<Post>().ToList().ForEach(p =>
//                        {
//                            EntityDatabase.Current.DeleteItem<Post>(p.ID);
//                        });
//
//                    //Save a fresh new set
//                    posts.ForEach(p =>
//                        {
//                            EntityDatabase.Current.SaveItem(p);
//                        });
//
//                }

            }
            catch (Exception e)
            {

                AppEvents.Current.TriggerConnectionFailedEvent(e.Message);
                Debug.WriteLine(e.Message);
				
//                if (first_load)
//                {
//                    posts = EntityDatabase.Current.GetItems<Post>().ToList();
//                }
            }

            return posts;
        }
    }

    public enum CollectionTraversal
    {
        Newer,
        //Our default traversal
        Older
    }

    public class GetPostsByMutualRequest : UserIdentity
    {
        public GetPostsByMutualRequest()
        {
            start_index = 0;
            max_results = 0;
            collection_traversal = CollectionTraversal.Newer;
            first_load = false;
        }

        /// <summary>
        /// The cursor to start querying for results from
        /// </summary>
        public long start_index { get; set; }

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
}
