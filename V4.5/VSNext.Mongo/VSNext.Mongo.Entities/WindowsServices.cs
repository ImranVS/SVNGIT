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
    [CollectionName("windows_services")]
    public  class WindowsService:Entity
    {
        [DataMember]
        [BsonElement("display_name")]
        public string DisplayName { get; set; }

        [DataMember]
        [BsonElement("service_name")]
        public string ServiceName { get; set; }

        [DataMember]
        [BsonElement("device_type")]
        public string DeviceType { get; set; }
    }
}
