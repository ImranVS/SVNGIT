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
    [CollectionName("stats_translation")]
    public class StatsTranslation : Entity
    {
        [DataMember]
        [BsonElement("stat_name")]
        [BsonIgnoreIfNullAttribute]
        public string StatName { get; set; }

        [DataMember]
        [BsonElement("translated_name")]
        [BsonIgnoreIfNullAttribute]
        public string TranslatedName { get; set; }
        
    }
}
