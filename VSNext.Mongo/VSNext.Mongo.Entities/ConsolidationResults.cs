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
    [CollectionName("consolidation_results")]
    public class ConsolidationResults : Entity
    {
        [DataMember]
        [BsonElement("scan_date")]
        public DateTime? ScanDate { get; set; }

        [DataMember]
        [BsonElement("result")]
        public string Result { get; set; }

      
    }
}
