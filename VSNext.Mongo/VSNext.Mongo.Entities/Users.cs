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
    [CollectionName("users")]
    public class Users : Entity
    {
     
        [DataMember]
        [BsonElement("full_name")]
        [BsonIgnoreIfNull]
        public string FullName { get; set; }

        [DataMember]
        [BsonElement("email")]
        [BsonIgnoreIfNull]
        public string Email { get; set; }

        [DataMember]
        [BsonElement("status")]
        [BsonIgnoreIfNull]
        public bool Status { get; set; }

        [DataMember]
        [BsonElement("roles")]
        [BsonIgnoreIfNull]
        public List<string> Roles { get; set; }

        [DataMember]
        [BsonElement("hash")]
        [BsonIgnoreIfNull]
        public string Hash { get; set; }

        [DataMember]
        [BsonElement("is_password_reset_required")]
        [BsonIgnoreIfNull]
        public bool IsPasswordResetRequired { get; set; }

    }
}
