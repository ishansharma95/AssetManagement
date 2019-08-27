using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class SubAsset
    {
        [Key]
        public int SubAssetId { get; set; }

        //Describes the directory according to size e.g 400*400
        public string Version { get; set; }

        //Describes the url of SubAsset
        public string SubAssetUrl { get; set; }

        public int AssetId { get; set; }
        [ForeignKey("AssetId")]
        public virtual Asset asset { get; set; }
    }
}