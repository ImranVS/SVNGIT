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
    [CollectionName("domino_server_tasks")]
    public class DominoServerTasks : Entity
    {
        [DataMember]
        [BsonElement("task_name")]
        public string TaskName { get; set; }

        [DataMember]
        [BsonElement("console_string")]
        public string ConsoleString { get; set; }

        [DataMember]
        [BsonElement("retry_count")]
        public int RetryCount { get; set; }

        [DataMember]
        [BsonElement("freeze_detect")]
        public bool FreezeDetect { get; set; }

        [DataMember]
        [BsonElement("max_busy_time")]
        public int MaxBusyTime { get; set; }

        [DataMember]
        [BsonElement("idle_string")]
        public string IdleString { get; set; }

        [DataMember]
        [BsonElement("load_string")]
        public string LoadString { get; set; }
        
            

    }
}
