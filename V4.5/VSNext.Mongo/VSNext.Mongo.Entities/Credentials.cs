using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization;
using VSNext.Mongo.Repository;

namespace VSNext.Mongo.Entities
{
    [DataContract]
    [Serializable]
    [CollectionName("credentials")]
    public class Credentials : Entity
    {
        [DataMember]
        [BsonElement("alias")]
        public string Alias { get; set; }

        [DataMember]
        [BsonElement("user_id")]
        public string UserId { get; set; }

        [DataMember]
        [BsonElement("password")]
        public string Password { get; set; }

        [DataMember]
        [BsonElement("device_type")]
        public string DeviceType { get; set; }
    }
}
