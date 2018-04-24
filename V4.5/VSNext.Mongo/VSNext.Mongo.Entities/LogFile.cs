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
    [CollectionName("log_file")]
    public class LogFile : Entity
    {
        public LogFile()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }

        }

        [DataMember]
        [BsonElement("key_word")]
        [BsonIgnoreIfNull]
        public string KeyWord { get; set; }

        [DataMember]
        [BsonElement("repeat_once")]
        [BsonIgnoreIfNull]
        public Boolean? RepeateOnce { get; set; }

        [DataMember]
        [BsonElement("not_required_keyword")]
        [BsonIgnoreIfNull]
        public string NotRequiredKeyword { get; set; }

        [DataMember]
        [BsonElement("log")]
        [BsonIgnoreIfNull]
        public Boolean? Log { get; set; }

        [DataMember]
        [BsonElement("agent_log")]
        [BsonIgnoreIfNull]
        public Boolean? AgentLog { get; set; }
        
        [DataMember]
        [BsonElement("domino_event_log_id")]
        [BsonIgnoreIfNull]
        public int DominoEventLogId { get; set; }

    }
}
