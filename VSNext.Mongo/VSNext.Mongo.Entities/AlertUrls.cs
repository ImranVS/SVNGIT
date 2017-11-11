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
    [CollectionName("alert_urls")]
    public class Alert_URLs : Entity
    {
        [DataMember]
        [BsonElement("name")]
        [BsonIgnoreIfNullAttribute]
        public string Name { get; set; }

        [DataMember]
        [BsonElement("url")]
        [BsonIgnoreIfNullAttribute]
        public string Url { get; set; }
    
       
    }
}
