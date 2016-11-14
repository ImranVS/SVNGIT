using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSNext.Mongo.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization;

namespace VSNext.Mongo.Entities
{
    [DataContract]
    [Serializable]
    [CollectionName("notifications")]
    public class Notifications : Entity
    {
        [DataMember]
        [BsonElement("notification_name")]
        [BsonIgnoreIfNullAttribute]
        public string NotificationName { get; set; }

        [DataMember]
        [BsonElement("send")]
        [BsonIgnoreIfNullAttribute]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> SendList { get; set; }
    }
}
