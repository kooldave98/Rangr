using System;

namespace App.Common
{
    public static class UserResources
    {
        public const string restful_resource = "users";

        public static HttpRequest httpRequest {
            get {
                return HttpRequest.Current;
            }
        }

        public static string base_rest_url {
            get {
                return string.Format ("{0}/{1}", Resources.baseUrl, restful_resource);
            }
        }
    }
}

