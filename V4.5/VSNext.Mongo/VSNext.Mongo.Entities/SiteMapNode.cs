using System;
using System.Runtime.Serialization;
using VSNext.Mongo.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace VSNext.Mongo.Entities
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

        [BsonElement("server_types")]
        public List<String> ServerTypes { get; set; }

        [BsonElement("nodes")]
        public ICollection<SiteMapNode> Nodes { get; set; }
    }
}
