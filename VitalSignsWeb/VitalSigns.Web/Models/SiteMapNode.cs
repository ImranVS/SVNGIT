using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.Web.Models
{
    public class SiteMapNode
    {
        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("url")]
        public string Url { get; set; }

        [BsonElement("icon")]
        public string Icon { get; set; }

        [BsonElement("disabled")]
        public bool Disabled { get; set; }

        [BsonElement("nodes")]
        public ICollection<SiteMapNode> Nodes { get; set; }
    }
}
