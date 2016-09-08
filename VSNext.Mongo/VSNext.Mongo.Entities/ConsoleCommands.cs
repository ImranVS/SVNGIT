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
    [CollectionName("console_commands")]
    public class ConsoleCommands : Entity
    {
        public ConsoleCommands()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach(var property in props)
            {
                property.SetValue(this, null);
            }

        }

        [DataMember]
        [BsonElement("device_name")]
        [BsonIgnoreIfNullAttribute]
        public string DeviceName { get; set; }
        
        [DataMember]
        [BsonElement("device_id")]
        [BsonIgnoreIfNullAttribute]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DeviceId { get; set; }

        [DataMember]
        [BsonElement("command")]
        [BsonIgnoreIfNullAttribute]
        public string Command { get; set; }

        [DataMember]
        [BsonElement("submitter")]
        [BsonIgnoreIfNullAttribute]
        public string Submitter { get; set; }

        [DataMember]
        [BsonElement("date_time_submitted")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? DateTimeSubmitted { get; set; }

        [DataMember]
        [BsonElement("date_time_processed")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? DateTimeProcessed { get; set; }

        [DataMember]
        [BsonElement("result")]
        [BsonIgnoreIfNullAttribute]
        public string Result { get; set; }

        [DataMember]
        [BsonElement("comments")]
        [BsonIgnoreIfNullAttribute]
        public string Comments { get; set; }
    }
}
