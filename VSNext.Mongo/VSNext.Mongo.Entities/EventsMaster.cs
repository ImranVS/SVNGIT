﻿using System;
using System.Runtime.Serialization;
using VSNext.Mongo.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace VSNext.Mongo.Entities
{
    [DataContract]
    [Serializable]
    [CollectionName("events_master")]

    public class EventsMaster : Entity
    {
        [DataMember]
        [BsonElement("event_type")]
        [BsonIgnoreIfNullAttribute]
        public string EventType { get; set; }

        [DataMember]
        [BsonElement("device_type")]
        [BsonIgnoreIfNullAttribute]
        public string DeviceType { get; set; }

        [DataMember]
        [BsonElement("notification_on_repeat")]
        [BsonIgnoreIfNullAttribute]
        public bool NotificationOnRepeat { get; set; }

        [DataMember]
        [BsonElement("notifications")]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfNullAttribute]
        public List<string> NotificationList { get; set; }
    }

}
