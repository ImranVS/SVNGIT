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
    [CollectionName("users_and_groups")]

    public class UsersAndGroups : Entity
    {
        public UsersAndGroups()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }
        }

        [DataMember]
        [BsonElement("device_id")]
        [BsonIgnoreIfNullAttribute]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DeviceId { get; set; }

        [DataMember]
        [BsonElement("identity")]
        [BsonIgnoreIfNullAttribute]
        public string Identity { get; set; }

        [DataMember]
        [BsonElement("distinguished_name")]
        [BsonIgnoreIfNullAttribute]
        public string DistinguishedName { get; set; }

        [DataMember]
        [BsonElement("user_principal_name")]
        [BsonIgnoreIfNullAttribute]
        public string UserPrincipalName { get; set; }

        [DataMember]
        [BsonElement("display_name")]
        [BsonIgnoreIfNullAttribute]
        public string DisplayName { get; set; }

        [DataMember]
        [BsonElement("name")]
        [BsonIgnoreIfNullAttribute]
        public string Name { get; set; }

        [DataMember]
        [BsonElement("is_valid")]
        [BsonIgnoreIfNullAttribute]
        public string IsValid { get; set; }

        [DataMember]
        [BsonElement("sam_account_name")]
        [BsonIgnoreIfNullAttribute]
        public string SamAccountName { get; set; }

        [DataMember]
        [BsonElement("type")]
        [BsonIgnoreIfNullAttribute]
        public string Type { get; set; }

        [DataMember]
        [BsonElement("members")]
        [BsonIgnoreIfNullAttribute]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Members { get; set; }

        [DataMember]
        [BsonElement("mailboxes")]
        [BsonIgnoreIfNullAttribute]
        public List<MailboxInfo> Mailboxes { get; set; }

        public class MailboxInfo
        {
            [DataMember]
            [BsonElement("mailbox_id")]
            [BsonIgnoreIfNullAttribute]
            [BsonRepresentation(BsonType.ObjectId)]
            public string MailboxId { get; set; }

            [DataMember]
            [BsonElement("display_name")]
            [BsonIgnoreIfNullAttribute]
            public string DisplayName { get; set; }

            [DataMember]
            [BsonElement("mailbox_size_mb")]
            [BsonIgnoreIfNullAttribute]
            public double? MailboxSizeMb { get; set; }
        }

    }

}
