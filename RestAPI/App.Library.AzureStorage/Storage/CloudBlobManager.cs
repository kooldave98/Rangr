using System.Collections.Generic;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace App.Library.Azure.Storage
{
    public class CloudBlobManager : ICloudBlobManager
    {
        private CloudStorageAccount _storageAccount = null;

        public void Initialize(string connectionString)
        {
            _storageAccount = CloudStorageAccount.Parse(connectionString);
        }

        public void StoreFileInAzureStorage(string containerName, string filename, System.IO.Stream stream)
        {
            CloudBlobContainer container = GetBlobContainerCreateIfDoesntExists(containerName);

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
            blockBlob.UploadFromStream(stream);
        }

        public IEnumerable<BlobInfo> GetListOfBlobsFromContainer(string containerName)
        {
            var blobInfos = new List<BlobInfo>();

            CloudBlobContainer container = GetBlobContainerCreateIfDoesntExists(containerName);

            // Only traverse first level no directories, no pages
            foreach (IListBlobItem item in container.ListBlobs(null))
            {
                if (item is CloudBlockBlob)
                {
                    var blob = (CloudBlockBlob)item;

                    blob.FetchAttributes();

                    var blobInfo = new BlobInfo
                    {
                        Uri = blob.Uri,
                        Filename = blob.Name,
                        ParentContainerName = blob.Parent.Container.Name,
                        FileLengthKilobytes = ConvertBytesToKilobytes(blob.Properties.Length)
                    };

                    blobInfos.Add(blobInfo);
                }
            }

            return blobInfos;
        }

        public Stream DownloadBlob(BlobInfo blobInfo)
        {
            var stream = new MemoryStream();
            var container = GetBlobContainerCreateIfDoesntExists(blobInfo.ParentContainerName);
            var blockBlob = container.GetBlockBlobReference(blobInfo.Filename);

            blockBlob.DownloadToStream(stream);

            return stream;
        }

        private CloudBlobContainer GetBlobContainerCreateIfDoesntExists(string containerName)
        {
            CloudBlobContainer container = null;
            var blobClient = _storageAccount.CreateCloudBlobClient();

            container = blobClient.GetContainerReference(containerName);
            container.CreateIfNotExists();

            return container;
        }

        private long ConvertBytesToKilobytes(long byteslength)
        {
            return (byteslength / 1024);
        }
    }
}
