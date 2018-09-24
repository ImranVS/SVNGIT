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
        [BsonElement("folders")]
        [BsonIgnoreIfNullAttribute]
        public List<Folder> Folders { get; set; }

        [DataMember]
        [BsonElement("last_permission_check")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? LastPermissionCheck { get; set; }

        [DataMember]
        [BsonElement("identity")]
        [BsonIgnoreIfNullAttribute]
        public string Identity { get; set; }

        [DataMember]
        [BsonElement("mailbox_forwarding_smtp_address")]
        [BsonIgnoreIfNullAttribute]
        public string MailboxForwardingSMTPAddress { get; set; }

        [DataMember]
        [BsonElement("mailbox_forwarding_address")]
        [BsonIgnoreIfNullAttribute]
        public string MailboxForwardingAddress { get; set; }

        [DataMember]
        [BsonElement("rule_forward_to")]
        [BsonIgnoreIfNullAttribute]
        public List<string> RuleForwardTo { get; set; }

        [DataMember]
        [BsonElement("rule_forward_as_attachment_to")]
        [BsonIgnoreIfNullAttribute]
        public List<string> RuleForwardAsAttachmentTo { get; set; }

        [DataMember]
        [BsonElement("deliver_to_mailbox_and_forward")]
        [BsonIgnoreIfNullAttribute]
        public Boolean DeliverToMailboxAndForward { get; set; }
        

        [DataMember]
        [BsonElement("users_with_permission")]
        [BsonIgnoreIfNullAttribute]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> UsersWithPermission { get; set; }

        [DataMember]
        [BsonElement("retention_policy")]
        [BsonIgnoreIfNullAttribute]
        public string RetentionPolicy { get; set; }

        [DataMember]
        [BsonElement("litigation_hold_enabled")]
        [BsonIgnoreIfNullAttribute]
        public bool? LitigationHoldEnabled { get; set; }

        [DataMember]
        [BsonElement("recipient_type_details")]
        [BsonIgnoreIfNullAttribute]
        public string RecipientTypeDetails { get; set; }

        [DataMember]
        [BsonElement("owa_mailbox_policy")]
        [BsonIgnoreIfNullAttribute]
        public string OWAMailboxPolicy { get; set; }

        [DataMember]
        [BsonElement("last_ews_scan")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? LastEwsScan { get; set; }

        [DataMember]
        [BsonElement("encrypted_mail_count")]
        [BsonIgnoreIfNullAttribute]
        public int? EncryptedMailCount { get; set; }

        [DataMember]
        [BsonElement("attachment_types")]
        [BsonIgnoreIfNullAttribute]
        public List<NameValuePair> AttachmentTypes { get; set; }

        [DataMember]
        [BsonElement("distinguished_name")]
        [BsonIgnoreIfNullAttribute]
        public string DistinguishedName { get; set; }
        


        public class Folder
        {

            [DataMember]
            [BsonElement("name")]
            [BsonIgnoreIfNullAttribute]
            public string Name { get; set; }

            [DataMember]
            [BsonElement("total_item_size_mb")]
            [BsonIgnoreIfNullAttribute]
            public double? TotalItemSizeMb { get; set; }

            [DataMember]
            [BsonElement("item_count")]
            [BsonIgnoreIfNullAttribute]
            public int? ItemCount { get; set; }

            [DataMember]
            [BsonElement("items_and_subfolder_items_count")]
            [BsonIgnoreIfNullAttribute]
            public int? ItemsAndSubfolderItemsCount { get; set; }

            [DataMember]
            [BsonElement("items_and_subfolder_items_size_mb")]
            [BsonIgnoreIfNullAttribute]
            public double? ItemsAndSubfolderItemsSizeMb { get; set; }

            [DataMember]
            [BsonElement("deleted_item_count")]
            [BsonIgnoreIfNullAttribute]
            public int? DeletedItemCount { get; set; }
            

        }
    }

}
