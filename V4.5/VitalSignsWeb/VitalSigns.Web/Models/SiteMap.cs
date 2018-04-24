using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.Web.Models
{
    public class SiteMap
    {

        [BsonId]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("nodes")]
        public ICollection<SiteMapNode> Nodes { get; set; }
    }
}
