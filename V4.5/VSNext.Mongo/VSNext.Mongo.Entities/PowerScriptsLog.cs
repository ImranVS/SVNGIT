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
    [CollectionName("powerscripts_log")]
    public class PowerScriptsLog : Entity 
    {
        [DataMember]
        [BsonIgnoreIfNullAttribute]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("target_device_id")]
        public string TargetDeviceId { get; set; }

        [DataMember]
        [BsonIgnoreIfNullAttribute]
        [BsonElement("target_device_name")]
        public string TargetDeviceName { get; set; }

        [DataMember]
        [BsonIgnoreIfNullAttribute]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("user_id")]
        public string UserId { get; set; }

        [DataMember]
        [BsonIgnoreIfNullAttribute]
        [BsonElement("user_name")]
        public string UserName { get; set; }

        [DataMember]
        [BsonIgnoreIfNullAttribute]
        [BsonElement("device_type")]
        public string DeviceType { get; set; }

        [DataMember]
        [BsonIgnoreIfNullAttribute]
        [BsonElement("script_name")]
        public string ScriptName { get; set; }

        [DataMember]
        [BsonIgnoreIfNullAttribute]
        [BsonElement("script_path")]
        public string ScriptPath { get; set; }

        [DataMember]
        [BsonIgnoreIfNullAttribute]
        [BsonElement("response")]
        public string Response { get; set; }

        [DataMember]
        [BsonIgnoreIfNullAttribute]
        [BsonElement("parameters")]
        public List<NameValuePair> Parameters { get; set; }

    }
}
