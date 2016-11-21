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
    [CollectionName("scripts")]
    public class Scripts : Entity
    {
        [DataMember]
        [BsonElement("script_name")]
        [BsonIgnoreIfNullAttribute]
        public string ScriptName { get; set; }

        [DataMember]
        [BsonElement("script_command")]
        [BsonIgnoreIfNullAttribute]
        public string ScriptCommand { get; set; }

        [DataMember]
        [BsonElement("script_location")]
        [BsonIgnoreIfNullAttribute]
        public string ScriptLocation { get; set; }

    }
}
