﻿using System;
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
        public List<SendList> SendList { get; set; }
    }
    /* Documents created using SendList can be either regular notifications or escalations. The difference between the two is the presence 
     of the property Interval in the list of members. Interval will only pertain to escalations. */
    public class SendList : Entity
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
        public bool PersistentNotification { get; set; }

        [DataMember]
        [BsonElement("interval")]
        [BsonIgnoreIfNullAttribute]
        public int? Interval { get; set; }
    }
}
