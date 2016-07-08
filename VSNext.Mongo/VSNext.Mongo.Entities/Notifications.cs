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
        public List<SendList> SendList { get; set; }
    }
    public class SendList : Entity
    {
        [DataMember]
        [BsonElement("business_hours_id")]
        [BsonIgnoreIfNullAttribute]
        public ObjectId BusinessHoursId { get; set; }

        [DataMember]
        [BsonElement("send_via")]
        [BsonIgnoreIfNullAttribute]
        public string SendVia { get; set; }

        [DataMember]
        [BsonElement("send_to")]
        [BsonIgnoreIfNullAttribute]
        public string SendTo { get; set; }

        [DataMember]
        [BsonElement("copy_to")]
        [BsonIgnoreIfNullAttribute]
        public string CopyTo { get; set; }
    }
}
