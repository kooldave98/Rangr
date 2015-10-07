using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;

namespace rangr.common
{
    //category={category}
    //&start_index={start_index}
    //&max_results={max_results}
    //&collection_traversal={collection_traversal}
    //&first_load={first_load}
    //&hash_tag_name={hash_tag_name}
    //&connection_id={connection_id}
    public class Posts
    {

        public async Task<List<Post>> Get(string connection_id, 
                                          string start_index,		                                   
                                          bool first_load = false, 
                                          CollectionTraversal traversal = CollectionTraversal.Newer,
                                          GetPostsQueryCategory category = GetPostsQueryCategory.ByRadius,
                                          string hash_tag_name = "")
        {
            var posts = new List<Post>();

            var url = String.Format("{0}/?connection_id={1}&start_index={2}", base_rest_url, connection_id, start_index);

            if (traversal == CollectionTraversal.Older)
            {
                url = url + "&collection_traversal=Older";
            }

            if (first_load)
            {
                url = url + "&first_load=true";
            }

            if (category == GetPostsQueryCategory.ByHashTag)
            {
                url = url + string.Format("&category=ByHashTag&hash_tag_name={0}", hash_tag_name);
            }

            try
            {

                string data = await _httpRequest.Get(url);
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

        public async Task<PostIdentity> Create(string text, string connection_id, HttpFile raw_image)
        {
            PostIdentity post_id = null;

            var requestBody = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("connection_id", connection_id),
                new KeyValuePair<string, string>("text", text)
            };

            var binary_request_body = new List<KeyValuePair<string, HttpFile>>()
            {
                new KeyValuePair<string, HttpFile>("image_data", raw_image)
            }; 

            var url = String.Format("{0}/create", base_rest_url);

            try
            {

                var data = await _httpRequest.Post(url, requestBody, binary_request_body);
                post_id = JsonConvert.DeserializeObject<PostIdentity>(data);

            }
            catch (Exception e)
            {
                //hack, the server returns multipart content, need to fix this later
                post_id = new PostIdentity();
                AppEvents.Current.TriggerConnectionFailedEvent(e.Message);
                Debug.WriteLine(e.Message);
            }

            return post_id;
        }


        private const string restful_resource = "posts";

        private HttpRequest _httpRequest
        {
            get
            {
                return HttpRequest.Current;
            }
        }

        private string base_rest_url
        {
            get
            {
                return string.Format("{0}/{1}", Resources.baseUrl, restful_resource);
            }
        }
    }

    public enum GetPostsQueryCategory
    {
        ByRadius,
        //Our default category, which is 200 metres
        ByHashTag,
    }

    public enum CollectionTraversal
    {
        Newer,
        //Our default traversal
        Older
    }
}
