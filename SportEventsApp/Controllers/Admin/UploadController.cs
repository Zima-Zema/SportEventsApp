using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace SportEventsApp.Controllers.Admin
{
    public class UploadController : Controller
    {
        // GET: Upload
        [HttpPost]
        public JsonResult UploadFile()
        {
            string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=fantasista;AccountKey=pqvFuls2/0QeCWWM0KbfQ2A3FIVy/RsNVdDLF/FyGf1JxqOAmTYO6i9UzrPvZDDBjOEr+ancE7EWr3FwmtGVmw==;EndpointSuffix=core.windows.net";
            CloudStorageAccount account = CloudStorageAccount.Parse(storageConnectionString);
            CloudBlobClient serviceClient = account.CreateCloudBlobClient();
            var container = serviceClient.GetContainerReference("fantasistacontainer");
            container.CreateIfNotExistsAsync().Wait();



            var file = HttpContext.Request.Files[0];
            if (file == null)
                return Json(null);
            


            CloudBlockBlob blob = container.GetBlockBlobReference(file.FileName);
            blob.Properties.ContentType = file.ContentType;
            blob.UploadFromStream(file.InputStream);


            var url = blob.StorageUri.PrimaryUri.AbsoluteUri;

            return Json(url);

        }

        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }
    }
}