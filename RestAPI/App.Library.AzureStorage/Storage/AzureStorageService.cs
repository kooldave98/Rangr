using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Azure;

namespace App.Library.Azure.Storage
{

    public class AzureStorageService
    {
        public async Task<BlobData> download(string name)
        {
            //http://www.juliencorioland.net/archives/using-aspnet-web-api-to-stream-windows-azure-blobs#.VRBM7JOsUWc
            var cloudBlob = await imagesBlobContainer.GetBlobReferenceFromServerAsync(name);
            var cloudBlobExists = await cloudBlob.ExistsAsync();

            if (!cloudBlobExists)
            {
                return new BlobData()
                {
                    name = name,
                    status_code = HttpStatusCode.NotFound
                };
            }


            var blobStream = await cloudBlob.OpenReadAsync();


            return new BlobData()
            {
                name = name,
                content_type = cloudBlob.Properties.ContentType,
                content_length = cloudBlob.Properties.Length,
                stream = blobStream,
                status_code = HttpStatusCode.OK
            };
        }

        public async Task<string> upload(byte[] data)
        {
            return await upload(new MemoryStream(data));
        }

        public async Task<string> upload(Stream data)
        {
            var blobName = Guid.NewGuid().ToString();

            var imageBlob = imagesBlobContainer.GetBlockBlobReference(blobName);

            await imageBlob.UploadFromStreamAsync(data);

            Trace.TraceInformation("Uploaded image file to {0}", imageBlob.Uri);

            return blobName;
            //return imageBlob.Uri.ToString();
        }

        public AzureStorageService()
        {
            // Open storage account using credentials from .cscfg file.
            var storageAccount = StorageInitializer.CreateStorageAccountFromConnectionString(CloudConfigurationManager.GetSetting("RangrStorageConnection"));

            // Get context object for working with blobs, and 
            // set a default retry policy appropriate for a web user interface.
            var blobClient = storageAccount.CreateCloudBlobClient();
            //blobClient.DefaultRequestOptions.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(3), 3);

            // Get a reference to the blob container.
            imagesBlobContainer = blobClient.GetContainerReference("images");
        }

        private static CloudBlobContainer imagesBlobContainer;
    }

}