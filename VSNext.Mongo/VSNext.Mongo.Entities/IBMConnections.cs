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
    [CollectionName("ibm_connections_top_stats")]
    public class IbmConnectionsTopStats : Entity
    {
        [DataMember]
        [BsonElement("device_name")]
        public string DeviceName { get; set; }


        [DataMember]
        [BsonElement("device_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DeviceId { get; set; }

        [DataMember]
        [BsonElement("ranking")]
        public string Ranking { get; set; }

        [DataMember]
        [BsonElement("name")]
        public string Name { get; set; }

        [DataMember]
        [BsonElement("usage_count")]
        public string UsageCount { get; set; }

        [DataMember]
        [BsonElement("type")]
        public string Type { get; set; }

    }
    //[DataContract]
    //[Serializable]
    //[CollectionName("ibm_connections_community")]
    //public class IbmConnectionsCommunity : Entity
    //{
    //    [DataMember]
    //    [BsonElement("id")]
    //    public int id { get; set; }

    //    [DataMember]
    //    [BsonElement("community_name")]
    //    public string CommunityName { get; set; }

    //}

    [DataContract]
    [Serializable]
    [CollectionName("ibm_connections_objects")]
    public class IbmConnectionsObjects : Entity
    {
        [DataMember]
        [BsonElement("name")]
        public string Name { get; set; }

        [DataMember]
        [BsonElement("device_name")]
        public string DeviceName { get; set; }


        [DataMember]
        [BsonElement("device_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DeviceId { get; set; }

        [DataMember]
        [BsonElement("ownerid")]
        public string OwnerId { get; set; }

        [DataMember]
        [BsonElement("guid")]
        public string GUID { get; set; }


        [DataMember]
        [BsonElement("type")]
        public string Type { get; set; }

        [DataMember]
        [BsonElement("parent_guid")]
        public string ParentGUID { get; set; }

        [DataMember]
        [BsonElement("tags")]
        public List<string> tags { get; set; }

        [DataMember]
        [BsonElement("users")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> users { get; set; }

        [DataMember]
        [BsonElement("community")]
        public string Community { get; set; }

        [DataMember]
        [BsonElement("object_created_date")]
        public DateTime ObjectCreatedDate { get; set; }

        [DataMember]
        [BsonElement("object_modified_date")]
        public DateTime ObjectModifiedDate { get; set; }
    }

    //[DataContract]
    //[Serializable]
    //[CollectionName("ibm_connections_objects_tags")]
    //public class IbmConnectionsObjectsTags : Entity
    //{
    //    [DataMember]
    //    [BsonElement("device_name")]
    //    public string DeviceName { get; set; }

    //    [DataMember]
    //    [BsonElement("guid")]
    //    public string GUID { get; set; }

       
    //    [DataMember]
    //    [BsonElement("tag_name")]
    //    public string TagName { get; set; }

    //    [DataMember]
    //    [BsonElement("type")]
    //    public string Type { get; set; }

    //    [DataMember]
    //    [BsonElement("tag_id")]
    //    public int TagId { get; set; }

    //}

    [DataContract]
    [Serializable]
    [CollectionName("ibm_connections_users")]
    public class IbmConnectionsUsers : Entity
    {
        [DataMember]
        [BsonElement("display_name")]
        public string DisplayName { get; set; }

        [DataMember]
        [BsonElement("device_name")]
        public string DeviceName { get; set; }

        [DataMember]
        [BsonElement("device_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DeviceId { get; set; }

        [DataMember]
        [BsonElement("guid")]
        public string GUID { get; set; }

        [DataMember]
        [BsonElement("is_active")]
        public bool IsActive { get; set; }

        [DataMember]
        [BsonElement("is_internal")]
        public bool IsInternal { get; set; }

    }

}
