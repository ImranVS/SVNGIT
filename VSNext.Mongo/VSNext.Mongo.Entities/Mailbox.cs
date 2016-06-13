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
            DisplayName = null;
            IssueWarningQuota = null;
            ProhibitSendQuota = null;
            ProhibitSendReceiveQuota = null;
            TotalItemSizeMb = null;
            ItemCount = null;
            StorageLimitStatus = null;
            DatabaseName = null;
        }

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

        /*[DataMember]
        [BsonElement("server")]
        public class Server {
            [DataMember]
            [BsonElement("$ref")]
            public string ServerRef { get; set; }

            [DataMember]
            [BsonElement("$id")]
            public string IdRef { get; set; }
        }
         * */
    }

}
