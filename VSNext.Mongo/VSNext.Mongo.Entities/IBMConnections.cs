using System;
using System.Runtime.Serialization;
using VSNext.Mongo.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
namespace VSNext.Mongo.Entities
{
    //[DataContract]
    //[Serializable]
    //[CollectionName("ibm_connections_top_stats")]
    //public class IbmConnectionsTopStats : Entity
    //{
    //    [DataMember]
    //    [BsonElement("device_name")]
    //    public string DeviceName { get; set; }


    //    [DataMember]
    //    [BsonElement("device_id")]
    //    [BsonRepresentation(BsonType.ObjectId)]
    //    public string DeviceId { get; set; }

    //    [DataMember]
    //    [BsonElement("ranking")]
    //    public string Ranking { get; set; }

    //    [DataMember]
    //    [BsonElement("name")]
    //    public string Name { get; set; }

    //    [DataMember]
    //    [BsonElement("usage_count")]
    //    public string UsageCount { get; set; }

    //    [DataMember]
    //    [BsonElement("type")]
    //    public string Type { get; set; }

    //}
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
        [BsonIgnoreIfNull]
        public string Name { get; set; }

        [DataMember]
        [BsonElement("device_name")]
        [BsonIgnoreIfNull]
        public string DeviceName { get; set; }


        [DataMember]
        [BsonElement("device_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfNull]
        public string DeviceId { get; set; }

        [DataMember]
        [BsonElement("ownerid")]
        [BsonIgnoreIfNull]
        public string OwnerId { get; set; }

        [DataMember]
        [BsonElement("guid")]
        [BsonIgnoreIfNull]
        public string GUID { get; set; }


        [DataMember]
        [BsonElement("type")]
        [BsonIgnoreIfNull]
        public string Type { get; set; }

        [DataMember]
        [BsonElement("parent_guid")]
        [BsonIgnoreIfNull]
        public string ParentGUID { get; set; }

        [DataMember]
        [BsonElement("tags")]
        [BsonIgnoreIfNull]
        public List<string> tags { get; set; }

        [DataMember]
        [BsonElement("users")]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfNull]
        public List<string> users { get; set; }

        [DataMember]
        [BsonElement("community_type")]
        [BsonIgnoreIfNull]
        public string CommunityType { get; set; }

        [DataMember]
        [BsonElement("object_created_date")]
        [BsonIgnoreIfNull]
        public DateTime? ObjectCreatedDate { get; set; }

        [DataMember]
        [BsonElement("object_modified_date")]
        [BsonIgnoreIfNull]
        public DateTime? ObjectModifiedDate { get; set; }

        [DataMember]
        [BsonElement("is_active")]
        [BsonIgnoreIfNull]
        public bool? IsActive { get; set; }

        [DataMember]
        [BsonElement("is_internal")]
        [BsonIgnoreIfNull]
        public bool? IsInternal { get; set; }

        [DataMember]
        [BsonElement("num_of_followers")]
        [BsonIgnoreIfNull]
        public int? NumOfFollowers { get; set; }

        [DataMember]
        [BsonElement("num_of_owners")]
        [BsonIgnoreIfNull]
        public int? NumOfOwners { get; set; }

        [DataMember]
        [BsonElement("num_of_members")]
        [BsonIgnoreIfNull]
        public int? NumOfMembers { get; set; }

        [DataMember]
        [BsonElement("object_url")]
        [BsonIgnoreIfNull]
        public string ObjectUrl { get; set; }

        [DataMember]
        [BsonElement("description")]
        [BsonIgnoreIfNull]
        public string Description { get; set; }

        [DataMember]
        [BsonElement("children")]
        [BsonIgnoreIfNull]
        public List<IbmConnectionChildren> Children { get; set; }

        //Users only
        [DataMember]
        [BsonElement("logon_name")]
        [BsonIgnoreIfNull]
        public string LogonName { get; set; }

        //Users only
        [DataMember]
        [BsonElement("last_login_date")]
        [BsonIgnoreIfNull]
        public DateTime? LastLoginDate { get; set; }
    }

    public class IbmConnectionChildren
    {

        public IbmConnectionChildren()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }
        }

        [DataMember]
        [BsonElement("type")]
        [BsonIgnoreIfNullAttribute]
        public string Type { get; set; }

        [DataMember]
        [BsonElement("count")]
        [BsonIgnoreIfNullAttribute]
        public int? Count { get; set; }

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

    //[DataContract]
    //[Serializable]
    //[CollectionName("ibm_connections_objects")]
    //public class IbmConnectionsUsers : Entity
    //{
    //    [DataMember]
    //    [BsonElement("display_name")]
    //    public string DisplayName { get; set; }

    //    [DataMember]
    //    [BsonElement("device_name")]
    //    public string DeviceName { get; set; }

    //    [DataMember]
    //    [BsonElement("device_id")]
    //    [BsonRepresentation(BsonType.ObjectId)]
    //    public string DeviceId { get; set; }

    //    [DataMember]
    //    [BsonElement("guid")]
    //    public string GUID { get; set; }

    //    [DataMember]
    //    [BsonElement("is_active")]
    //    public bool IsActive { get; set; }

    //    [DataMember]
    //    [BsonElement("is_internal")]
    //    public bool IsInternal { get; set; }

    //}

}
