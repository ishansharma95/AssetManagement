using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Web;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi
{
    public class Common
    {
        private AssetRepository assetRep;
        //public Common(AssetRepository rep)
        //{
        //    assetRep = rep;
        //}
        public const string containerName = "assets";
        public static string AppendTimeStamp(string fileName)
        {
            return string.Concat(
                Path.GetFileNameWithoutExtension(fileName),
                DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                Path.GetExtension(fileName)
                );
        }

        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }

        public static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

        /// <summary>
        /// uploaded to azure and then metadata is updated.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        /// <param name="userMetaData"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        public Tuple<CloudBlockBlob, string> UploadFileAsync(HttpPostedFile file, string fileName, string userMetaData,Asset asset)
        {
            string storageConnection = ConfigurationManager.ConnectionStrings["azureStoreCS"].ConnectionString;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnection);
            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = cloudBlobClient.GetContainerReference("assets");
            if (blobContainer.CreateIfNotExists())
            {
                blobContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }

            CloudBlockBlob cloudBlockBlob = blobContainer.GetBlockBlobReference("rawImage/" + fileName);
            cloudBlockBlob.Properties.ContentType = file.ContentType;
            using (var s = file.InputStream)
            {
                cloudBlockBlob.UploadFromStream(s);
            }
            var metaData=SetMetaData(cloudBlockBlob, userMetaData);
            
            return new Tuple<CloudBlockBlob, string>(cloudBlockBlob, metaData);
        }

        /// <summary>
        /// calls azure cognitive service and gets metadata according to the image
        /// </summary>
        /// <param name="blockBlob"></param>
        /// <param name="userMetaData"></param>
        /// <returns></returns>
        public string SetMetaData(CloudBlockBlob blockBlob, string userMetaData)
        {
            string metaData = string.Empty;
            var obj = new { url = blockBlob.Uri.ToString() };
            var client = new RestClient("https://northeurope.api.cognitive.microsoft.com/vision/v1.0/analyze?visualFeatures=Categories&language=en");
            var request = new RestRequest(Method.POST);
            
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Ocp-Apim-Subscription-Key", ConfigurationManager.AppSettings["SubscriptionKey"]);
            var serializedObj=JsonConvert.SerializeObject(obj);
            request.AddParameter("undefined", serializedObj, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                dynamic jsonMetaData=JsonConvert.DeserializeObject(response.Content.ToString());
                jsonMetaData.userData = userMetaData;
                metaData = JsonConvert.SerializeObject(jsonMetaData);
            }
            return metaData;
        }
    }
}