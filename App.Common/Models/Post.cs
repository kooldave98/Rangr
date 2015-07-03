using System;
using System.Threading.Tasks;
using common_lib;

namespace App.Common
{
	public class Post : PostIdentity
	{
        public string text { get; set; }

        public DateTime date { get; set; }

        public string long_lat_acc_geo_string { get; set; }

        public string image_id { get; set; }
	}

    public static class PostExtensions
    {
        public async static Task<string> get_name_for_number(this Post post)
        {
            return await ContactsProvider.Current.get_name_for_number(post.user_id);
        }

        public static string get_time_ago(this Post post)
        {
            return TimeAgoConverter.Current.Convert(post.date);
        }
    }
}

