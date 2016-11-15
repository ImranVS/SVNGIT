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
    [CollectionName("notification_destinations")]
    public class NotificationDestinations : Entity
    {
        [DataMember]
        [BsonElement("business_hours_id")]
        [BsonIgnoreIfNullAttribute]
        [BsonRepresentation(BsonType.ObjectId)]
        public string BusinessHoursId { get; set; }

        [DataMember]
        [BsonElement("b_id")]
        [BsonIgnoreIfNullAttribute]
        public int BId { get; set; }

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

        [DataMember]
        [BsonElement("blind_copy_to")]
        [BsonIgnoreIfNullAttribute]
        public string BlindCopyTo { get; set; }

        [DataMember]
        [BsonElement("script_command")]
        [BsonIgnoreIfNullAttribute]
        public string ScriptCommand { get; set; }

        [DataMember]
        [BsonElement("script_location")]
        [BsonIgnoreIfNullAttribute]
        public string ScriptLocation { get; set; }

        [DataMember]
        [BsonElement("persistent_notification")]
        [BsonIgnoreIfNullAttribute]
        public bool? PersistentNotification { get; set; }

        [DataMember]
        [BsonElement("interval")]
        [BsonIgnoreIfNullAttribute]
        public int? Interval { get; set; }
    }
}
