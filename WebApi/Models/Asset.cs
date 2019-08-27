using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class Asset
    {
        public int AssetId { get; set; }
        public string OriginalName { get; set; }
        public string Type { get; set; }
        public string CustomName { get; set; }
        [Required]
        public string MetaData { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<SubAsset> SubAsset { get; set; }
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}