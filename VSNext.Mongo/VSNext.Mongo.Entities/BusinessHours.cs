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
    [CollectionName("business_hours")]
    public class BusinessHours : Entity
    {
        [DataMember]
        [BsonElement("name")]
        [BsonIgnoreIfNullAttribute]
        public string Name { get; set; }

        //[DataMember]
        //[BsonElement("b_id")]
        //[BsonIgnoreIfNullAttribute]
        //public int? BId { get; set; }

        [DataMember]
        [BsonElement("start_time")]
        [BsonIgnoreIfNullAttribute]
        public string StartTime { get; set; }

        [DataMember]
        [BsonElement("duration")]
        [BsonIgnoreIfNullAttribute]
        public int Duration { get; set; }

        [DataMember]
        [BsonElement("days")]
        [BsonIgnoreIfNullAttribute]
        public string[] Days { get; set; }

        //[DataMember]
        //[BsonElement("device_id")]
        //public string DeviceId { get; set; }

        [DataMember]
        [BsonElement("use_type")]
        public int UseType { get; set; }


    }
}
