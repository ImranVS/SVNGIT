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
    [CollectionName("mailboxes")]

    public class Mailbox : Entity
    {
        public Mailbox()
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
        [BsonElement("display_name")]
        [BsonIgnoreIfNullAttribute]
        public string DisplayName { get; set; }

        [DataMember]
        [BsonElement("issue_warning_quota")]
        [BsonIgnoreIfNullAttribute]
        public string IssueWarningQuota { get; set; }

        [DataMember]
        [BsonElement("prohibit_send_quota")]
        [BsonIgnoreIfNullAttribute]
        public string ProhibitSendQuota { get; set; }

        [DataMember]
        [BsonElement("prohibit_send_receive_quota")]
        [BsonIgnoreIfNullAttribute]
        public string ProhibitSendReceiveQuota { get; set; }

        [DataMember]
        [BsonElement("total_item_size_mb")]
        [BsonIgnoreIfNullAttribute]
        public double? TotalItemSizeMb { get; set; }

        [DataMember]
        [BsonElement("item_count")]
        [BsonIgnoreIfNullAttribute]
        public int? ItemCount { get; set; }

        [DataMember]
        [BsonElement("storage_limit_status")]
        [BsonIgnoreIfNullAttribute]
        public string StorageLimitStatus { get; set; }

        [DataMember]
        [BsonElement("database_name")]
        [BsonIgnoreIfNullAttribute]
        public string DatabaseName { get; set; }

        [DataMember]
        [BsonElement("device_name")]
        [BsonIgnoreIfNullAttribute]
        public string DeviceName { get; set; }

        [DataMember]
        [BsonElement("last_logon_time")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? LastLogonTime { get; set; }

        [DataMember]
        [BsonElement("last_logoff_time")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? LastLogoffTime { get; set; }

        [DataMember]
        [BsonElement("mailbox_type")]
        [BsonIgnoreIfNullAttribute]
        public string MailboxType { get; set; }

        [DataMember]
        [BsonElement("is_active")]
        [BsonIgnoreIfNullAttribute]
        public Boolean? IsActive { get; set; }

        [DataMember]
        [BsonElement("inactive_days_count")]
        [BsonIgnoreIfNull]
        public int? InactiveDaysCount { get; set; }

        [DataMember]
        [BsonElement("sam_account_name")]
        [BsonIgnoreIfNullAttribute]
        public string SAMAccountName { get; set; }

        [DataMember]
        [BsonElement("primary_smtp_address")]
        [BsonIgnoreIfNullAttribute]
        public string PrimarySmtpAddress { get; set; }

        [DataMember]
        [BsonElement("company")]
        [BsonIgnoreIfNullAttribute]
        public string Company { get; set; }

        [DataMember]
        [BsonElement("department")]
        [BsonIgnoreIfNullAttribute]
        public string Department { get; set; }

        [DataMember]
        [BsonElement("max_folder_count")]
        [BsonIgnoreIfNullAttribute]
        public int? MaxFolderCount { get; set; }

        [DataMember]
        [BsonElement("max_folder_size_mb")]
        [BsonIgnoreIfNullAttribute]
        public double? MaxFolderSizeMb { get; set; }

        [DataMember]
        [BsonElement("folder_count")]
        [BsonIgnoreIfNullAttribute]
        public int? FolderCount { get; set; }
    }

}
