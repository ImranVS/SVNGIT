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
    [CollectionName("events_detected")]
    public class EventsDetected : Entity
    {
        [DataMember]
        [BsonElement("device_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DeviceId { get; set; }

        [DataMember]
        [BsonElement("device")]
        [BsonIgnoreIfNullAttribute]
        public string Device { get; set; }

        [DataMember]
        [BsonElement("device_type")]
        [BsonIgnoreIfNullAttribute]
        public string DeviceType { get; set; }

        [DataMember]
        [BsonElement("event_type")]
        [BsonIgnoreIfNullAttribute]
        public string EventType { get; set; }

        [DataMember]
        [BsonElement("event_repeat_count")]
        [BsonIgnoreIfNullAttribute]
        public int EventRepeatCount { get; set; }

        [DataMember]
        [BsonElement("event_detected")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? EventDetected { get; set; }

        [DataMember]
        [BsonElement("event_dismissed")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? EventDismissed { get; set; }

        [DataMember]
        [BsonElement("details")]
        [BsonIgnoreIfNullAttribute]
        public string Details { get; set; }

        [DataMember]
        [BsonElement("is_system_message")]
        [BsonIgnoreIfNullAttribute]
        public bool IsSystemMessage { get; set; }

        [DataMember]
        [BsonElement("is_system_message_dismissed")]
        [BsonIgnoreIfNullAttribute]
        public bool? IsSystemMessageDismissed { get; set; }

        [DataMember]
        [BsonElement("notifications_sent")]
        [BsonIgnoreIfNullAttribute]
        public List<NotificationsSent> NotificationsSent { get; set; }

        [DataMember]
        [BsonElement("node_name")]
        [BsonIgnoreIfNull]
        public string NodeName { get; set; }

    }

    public class NotificationsSent : Entity
    {
        [DataMember]
        [BsonElement("notification_id")]
        [BsonIgnoreIfNullAttribute]
        [BsonRepresentation(BsonType.ObjectId)]
        public string NotificationId { get; set; }

        [DataMember]
        [BsonElement("event_detected_sent")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? EventDetectedSent { get; set; }

        [DataMember]
        [BsonElement("event_dismissed_sent")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? EventDismissedSent { get; set; }

        [DataMember]
        [BsonElement("notification_sent_to")]
        [BsonIgnoreIfNullAttribute]
        public string NotificationSentTo { get; set; }

        [DataMember]
        [BsonElement("notification_ccd_to")]
        [BsonIgnoreIfNullAttribute]
        public string NotificationCcdTo { get; set; }

        [DataMember]
        [BsonElement("notification_bccd_to")]
        [BsonIgnoreIfNullAttribute]
        public string NotificationBccdTo { get; set; }

        [DataMember]
        [BsonElement("escalation_id")]
        [BsonIgnoreIfNullAttribute]
        [BsonRepresentation(BsonType.ObjectId)]
        public string EscalationId { get; set; }
    }
}
