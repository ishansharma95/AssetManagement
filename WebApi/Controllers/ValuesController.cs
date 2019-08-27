using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;
using WebApi.Repositories;
using System.Web;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Drawing;

namespace WebApi.Controllers
{
    
    public class ValuesController : ApiController
    {
        private AssetRepository assetRep;
        private SubAssetRepository subAssetRep;

        public ValuesController(AssetRepository _rep,SubAssetRepository _subAssetRep)
        {
            assetRep = _rep;
            subAssetRep = _subAssetRep;
        }

        /// <summary>
        /// It uploads the image or video content to azure blob and saves data in Asset table.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("Index")]
        public HttpResponseMessage Index()
        {
            Common common = new Common(assetRep);
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            if (System.Web.HttpContext.Current.Request.Files.Count > 0)
            {
                var file = System.Web.HttpContext.Current.Request.Files[0];
                if (MimeMapping.GetMimeMapping(file.FileName).StartsWith("image/") || MimeMapping.GetMimeMapping(file.FileName).StartsWith("video/"))
                {
                    var obj = HttpContext.Current.Request.Form[0];
                    var resultAsset = JsonConvert.DeserializeObject<Asset>(obj);
                    string fileName = Common.AppendTimeStamp(file.FileName);
                    Asset asset = new Asset();
                    asset.CustomName = fileName;
                    asset.OriginalName = file.FileName;
                    asset.UserId = User.Identity.GetUserId();
                    asset.Type = MimeMapping.GetMimeMapping(file.FileName).StartsWith("image/") ? "image" : "video";
                    asset.MetaData = resultAsset.MetaData;
                    var blockBlob = common.UploadFileAsync(file, fileName, resultAsset.MetaData, asset);
                    return new HttpResponseMessage(HttpStatusCode.Accepted);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Adds SubAsset to the database with the corresponding fields
        /// </summary>
        /// <param name="blobName"></param>
        /// <param name="version"></param>
        /// <param name="subAssetUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddSubAsset")]
        public HttpResponseMessage AddSubAsset(string blobName,string version, string subAssetUrl)
        {
            var asset = assetRep.GetAll().Where(d => d.CustomName == blobName).FirstOrDefault();
            if(asset!=null)
            {
                if (asset.Type == "image")
                {
                    SubAsset subasset = new SubAsset();
                    subasset.AssetId = asset.AssetId;
                    subasset.asset = asset;
                    subasset.Version = version;
                    subasset.SubAssetUrl = subAssetUrl;
                    subAssetRep.Create(subasset);
                    return new HttpResponseMessage(HttpStatusCode.Accepted);
                }
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

       
    }
}
