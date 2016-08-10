using System;
using System.Runtime.Serialization;
using VSNext.Mongo.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace VSNext.Mongo.Entities
{

    [DataContract]
    [Serializable]
    [CollectionName("cluster_database_details")]

    public class ClusterDatabaseDetails : Entity
    {
        public ClusterDatabaseDetails()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }
        }

        [DataMember]
        [BsonElement("cluster_name")]
        [BsonIgnoreIfNullAttribute]
        public string ClusterName { get; set; }

        [DataMember]
        [BsonElement("database_title")]
        [BsonIgnoreIfNullAttribute]
        public string DatabaseTitle { get; set; }

        [DataMember]
        [BsonElement("database_name")]
        [BsonIgnoreIfNullAttribute]
        public string DatabaseName { get; set; }

        [DataMember]
        [BsonElement("document_count_a")]
        [BsonIgnoreIfNullAttribute]
        public int? DocumentCountA { get; set; }

        [DataMember]
        [BsonElement("document_count_b")]
        [BsonIgnoreIfNullAttribute]
        public int? DocumentCountB { get; set; }

        [DataMember]
        [BsonElement("document_count_c")]
        [BsonIgnoreIfNullAttribute]
        public int? DocumentCountC { get; set; }

        [DataMember]
        [BsonElement("database_size_a")]
        [BsonIgnoreIfNullAttribute]
        public double? DatabaseSizeA { get; set; }

        [DataMember]
        [BsonElement("database_size_b")]
        [BsonIgnoreIfNullAttribute]
        public double? DatabaseSizeB { get; set; }

        [DataMember]
        [BsonElement("database_size_c")]
        [BsonIgnoreIfNullAttribute]
        public double? DatabaseSizeC { get; set; }

        [DataMember]
        [BsonElement("description")]
        [BsonIgnoreIfNullAttribute]
        public string Description { get; set; }

        [DataMember]
        [BsonElement("replica_id")]
        [BsonIgnoreIfNullAttribute]
        public string ReplicaID { get; set; }

    }
}
