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
    [CollectionName("mobile_devices")]

    public class MobileDevices : Entity
    {
        public MobileDevices()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }
        }

        [DataMember]
        [BsonElement("user_name")]
        [BsonIgnoreIfNullAttribute]
        public string UserName { get; set; }

        [DataMember]
        [BsonElement("device_name")]
        [BsonIgnoreIfNullAttribute]
        public string DeviceName { get; set; }

        [DataMember]
        [BsonElement("connection_state")]
        [BsonIgnoreIfNullAttribute]
        public string ConnectionState { get; set; }

        [DataMember]
        [BsonElement("last_sync_time")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? LastSyncTime { get; set; }

        [DataMember]
        [BsonElement("os_type")]
        [BsonIgnoreIfNullAttribute]
        public string OSType { get; set; }

        [DataMember]
        [BsonElement("client_build")]
        [BsonIgnoreIfNullAttribute]
        public string ClientBuild { get; set; }

        [DataMember]
        [BsonElement("notification_type")]
        [BsonIgnoreIfNullAttribute]
        public string NotificationType { get; set; }

        [DataMember]
        [BsonElement("doc_id")]
        [BsonIgnoreIfNullAttribute]
        public int? DocID { get; set; }

        [DataMember]
        [BsonElement("device_type")]
        [BsonIgnoreIfNullAttribute]
        public string DeviceType { get; set; }

        [DataMember]
        [BsonElement("access")]
        [BsonIgnoreIfNullAttribute]
        public string Access { get; set; }

        [DataMember]
        [BsonElement("security_policy")]
        [BsonIgnoreIfNullAttribute]
        public string SecurityPolicy { get; set; }

        [DataMember]
        [BsonElement("wipe_requested")]
        [BsonIgnoreIfNullAttribute]
        public string WipeRequested { get; set; }

        [DataMember]
        [BsonElement("wipe_options")]
        [BsonIgnoreIfNullAttribute]
        public string WipeOptions { get; set; }

        [DataMember]
        [BsonElement("wipe_status")]
        [BsonIgnoreIfNullAttribute]
        public string WipeStatus { get; set; }

        [DataMember]
        [BsonElement("sync_type")]
        [BsonIgnoreIfNullAttribute]
        public string SyncType { get; set; }

        [DataMember]
        [BsonElement("wipe_supported")]
        [BsonIgnoreIfNullAttribute]
        public string WipeSupported { get; set; }

        [DataMember]
        [BsonElement("server_name")]
        [BsonIgnoreIfNullAttribute]
        public string ServerName { get; set; }

        [DataMember]
        [BsonElement("approval")]
        [BsonIgnoreIfNullAttribute]
        public string Approval { get; set; }

        [DataMember]
        [BsonElement("device_id")]
        [BsonIgnoreIfNullAttribute]
        public string DeviceID { get; set; }

        [DataMember]
        [BsonElement("more_details_url")]
        [BsonIgnoreIfNullAttribute]
        public string MoreDetailsUrl { get; set; }

        [DataMember]
        [BsonElement("is_more_details_fetched")]
        [BsonIgnoreIfNullAttribute]
        public Boolean? IsMoreDetailsFetched { get; set; }

        [DataMember]
        [BsonElement("os_type_min")]
        [BsonIgnoreIfNullAttribute]
        public string OSTypeMin { get; set; }

        [DataMember]
        [BsonElement("ha_pool")]
        [BsonIgnoreIfNullAttribute]
        public string HAPool { get; set; }

        [DataMember]
        [BsonElement("is_active")]
        [BsonIgnoreIfNullAttribute]
        public Boolean? IsActive { get; set; }

        [DataMember]
        [BsonElement("href")]
        [BsonIgnoreIfNullAttribute]
        public string Href { get; set; }

        [DataMember]
        [BsonElement("last_updated")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? LastUpdated { get; set; }

    }
}
