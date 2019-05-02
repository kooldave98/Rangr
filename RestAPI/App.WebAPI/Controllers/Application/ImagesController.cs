using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using App.Library.Azure.Storage;
using App.Library.CodeStructures.Behavioral;
using App.Library.WebAPI.ActionFilters;
using App.WebAPI.Controllers.Infrastructure;

namespace App.WebAPI.Controllers.Application
{
    //See this for how to abstract away the Azure blob storage implemenetation behind an interface
    //http://arcware.net/upload-and-download-files-with-web-api-and-azure-blob-storage/
    public class CreateImageController : BaseController
    {

        [ValidateMimeMultipartContentFilter]
        [Route("api/images")]
        public async Task<HttpResponseMessage> Post()
        {
            try
            {
                var msp = await Request.Content.ReadAsMultipartAsync();
                var parts = msp.Contents;
                var media = await parts.ToArray()[0].ReadAsStreamAsync();

                var url = await storage_service.upload(media);
                // Send OK Response along with saved file names to the client.
                return Request.CreateResponse(HttpStatusCode.OK, url);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        public CreateImageController(AzureStorageService the_storage_service)
        {
            storage_service = Guard.IsNotNull(the_storage_service, "the_storage_service");
        }

        private readonly AzureStorageService storage_service;
    }



    //See this for how to abstract away the Azure blob storage implemenetation behind an interface
    //http://arcware.net/upload-and-download-files-with-web-api-and-azure-blob-storage/
    public class GetImageController : BaseController
    {

        [Route("api/images/{image_id}")]
        public async Task<HttpResponseMessage> Get(string image_id)
        {

            var data = await storage_service.download(image_id);

            if (data.status_code != HttpStatusCode.OK)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "not found");
            }


            var message = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(data.stream)
            };

            message.Content.Headers.ContentLength = data.content_length;
            message.Content.Headers.ContentType = new MediaTypeHeaderValue(data.content_type);
            //message.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            //{
            //    FileName = image_id,
            //    Size = cloudBlob.Properties.Length
            //};

            return message;
        }

        public GetImageController(AzureStorageService the_storage_service)
        {
            storage_service = Guard.IsNotNull(the_storage_service, "the_storage_service");
        }

        private readonly AzureStorageService storage_service;
    }




}