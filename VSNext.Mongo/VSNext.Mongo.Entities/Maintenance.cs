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
    [CollectionName("maintenance")]
   public  class Maintenance:Entity
    {

        //[DataMember]
        //[BsonElement("id")]
        //[BsonIgnoreIfNullAttribute]
        //public string ID { get; set; }


        [DataMember]
        [BsonElement("name")]
        [BsonIgnoreIfNullAttribute]
        public string Name { get; set; }

        [DataMember]
        [BsonElement("start_date")]
        [BsonIgnoreIfNullAttribute]
        public DateTime StartDate { get; set; }

        [DataMember]
        [BsonElement("start_time")]
        [BsonIgnoreIfNullAttribute]
        public string StartTime { get; set; }

        [DataMember]
        [BsonElement("end_date")]
        [BsonIgnoreIfNullAttribute]
        public DateTime EndDate { get; set; }

        [DataMember]
        [BsonElement("end_time")]
        [BsonIgnoreIfNullAttribute]
        public string EndTime { get; set; }

        [DataMember]
        [BsonElement("duration")]
        [BsonIgnoreIfNullAttribute]
        public int Duration { get; set; }

        [DataMember]
        [BsonElement("maintenance_frequency")]
        [BsonIgnoreIfNullAttribute]
        public int MaintenanceFrequency { get; set; }

        [DataMember]
        [BsonElement("maintenance_days_list")]
        public string MaintenanceDaysList { get; set; }

        [DataMember]
        [BsonElement("continue_forever")]
        public bool ContinueForever { get; set; }

        [DataMember]
        [BsonElement("maintain_type")]
        public int? MaintainType { get; set; }

        [DataMember]
        [BsonElement("duration_type")]
        public int? DurationType { get; set; }


    }
}
