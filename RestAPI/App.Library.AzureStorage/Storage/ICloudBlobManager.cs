using System.Collections.Generic;
using System.IO;

namespace App.Library.Azure.Storage
{
    public interface ICloudBlobManager
    {
        void Initialize(string connectionstring);
        void StoreFileInAzureStorage(string blobContainer, string filename, Stream stream);
        IEnumerable<BlobInfo> GetListOfBlobsFromContainer(string containerName);
        Stream DownloadBlob(BlobInfo blobInfo);
    }
}