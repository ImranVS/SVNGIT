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
    [CollectionName("maintain_users")]
    public class MaintainUser : Entity
    {
        [DataMember]
        [BsonElement("login_name")]
        [BsonIgnoreIfNull]
        public string LoginName { get; set; }

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
        public string Status { get; set; }

        [DataMember]
        [BsonElement("super_admin")]
        [BsonIgnoreIfNull]
        public string SuperAdmin { get; set; }

        [DataMember]
        [BsonElement("configurator_access")]
        [BsonIgnoreIfNull]
        public bool ConfiguratorAccess { get; set; }

        [DataMember]
        [BsonElement("console_command_access")]
        [BsonIgnoreIfNull]
        public bool ConsoleCommandAccess { get; set; }

    }
}
