using System;
using System.Runtime.Serialization;
using VSNext.Mongo.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace VSNext.Mongo.Entities
{
    [DataContract]
    [Serializable]
    [CollectionName("menu_items")]
    public class MenuItems:Entity
    {
        [DataMember]
        [BsonElement("display_text")]
        public string DisplayText { get; set; }

        [DataMember]
        [BsonElement("page_link")]
        public string PageLink { get; set; }

        [DataMember]
        [BsonElement("image_url")]
        public string ImageUrl { get; set; }

        [DataMember]
        [BsonElement("module")]
        public string Module { get; set; }

        [DataMember]
        [BsonElement("override_sort")]
        [BsonIgnoreIfNull]
        public int? OverrideSort { get; set; }

        [DataMember]
        [BsonElement("parent_id")]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ParentId { get; set; }

        [DataMember]
        [BsonElement("order_id")]
        [BsonIgnoreIfNull]
        public int? OrderId { get; set; }
    }
}
