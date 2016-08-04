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
    [CollectionName("name_value")]
    public class NameValue : Entity
    {
        [DataMember]
        [BsonElement("name")]
        [BsonIgnoreIfNullAttribute]
        public string Name { get; set; }

        [DataMember]
        [BsonElement("value")]
        [BsonIgnoreIfNullAttribute]
        public string Value { get; set; }
    }
}
