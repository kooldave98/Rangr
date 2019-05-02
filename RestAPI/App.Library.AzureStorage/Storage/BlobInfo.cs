using System;
using System.IO;
using System.Net;

namespace App.Library.Azure.Storage
{
    public class BlobInfo
    {
        public string ParentContainerName { get; set; }
        public Uri Uri { get; set; }
        public string Filename { get; set; }
        public long FileLengthKilobytes { get; set; }
    }

    public class BlobData
    {
        public string name { get; set; }
        public long content_length { get; set; }
        public string content_type { get; set; }
        public Stream stream { get; set; }
        public HttpStatusCode status_code { get; set; }
    }
}
