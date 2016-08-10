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
    [CollectionName("database")]
    public class Database : Entity
    {

        public Database()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }
        }

        [DataMember]
        [BsonElement("server_name")]
        [BsonIgnoreIfNull]
        public string ServerName { get; set; }

        [DataMember]
        [BsonElement("server_idss")]
        [BsonIgnoreIfNull]
        public ObjectId? ServerId { get; set; }

        [DataMember]
        [BsonElement("scan_date")]
        [BsonIgnoreIfNull]
        public DateTime? ScanDateTime { get; set; }

        [DataMember]
        [BsonElement("file_name")]
        [BsonIgnoreIfNull]
        public string FileName { get; set; }

        [DataMember]
        [BsonElement("title")]
        [BsonIgnoreIfNull]
        public string Title { get; set; }

        [DataMember]
        [BsonElement("file_size")]
        [BsonIgnoreIfNull]
        public int? FileSize { get; set; }

        [DataMember]
        [BsonElement("design_template_name")]
        [BsonIgnoreIfNull]
        public string DesignTemplateName { get; set; }

        [DataMember]
        [BsonElement("quota")]
        [BsonIgnoreIfNull]
        public int? Quota { get; set; }

        [DataMember]
        [BsonElement("ft_indexed")]
        [BsonIgnoreIfNull]
        public Boolean? FTIndexed { get; set; }

        [DataMember]
        [BsonElement("enabled_for_cluster_replication")]
        [BsonIgnoreIfNull]
        public Boolean? EnabledForClusterReplication { get; set; }

        [DataMember]
        [BsonElement("replica_idss")]
        [BsonIgnoreIfNull]
        public string ReplicaId { get; set; }

        [DataMember]
        [BsonElement("ods")]
        [BsonIgnoreIfNull]
        public int? ODS { get; set; }

        [DataMember]
        [BsonElement("status")]
        [BsonIgnoreIfNull]
        public string Status { get; set; }

        [DataMember]
        [BsonElement("document_count")]
        [BsonIgnoreIfNull]
        public int? DocumentCount { get; set; }

        [DataMember]
        [BsonElement("categories")]
        [BsonIgnoreIfNull]
        public string Categories { get; set; }

        [DataMember]
        [BsonElement("created")]
        [BsonIgnoreIfNull]
        public DateTime? Created { get; set; }

        [DataMember]
        [BsonElement("current_access_level")]
        [BsonIgnoreIfNull]
        public string CurrentAccessLevel { get; set; }

        [DataMember]
        [BsonElement("ft_index_frequency")]
        [BsonIgnoreIfNull]
        public string FTIndexFrequency { get; set; }

        [DataMember]
        [BsonElement("is_in_service")]
        [BsonIgnoreIfNull]
        public Boolean? IsInService { get; set; }

        [DataMember]
        [BsonElement("folder")]
        [BsonIgnoreIfNull]
        public string Folder { get; set; }

        [DataMember]
        [BsonElement("is_private_address_book")]
        [BsonIgnoreIfNull]
        public Boolean? IsPrivateAddressBook { get; set; }

        [DataMember]
        [BsonElement("is_public_address_book")]
        [BsonIgnoreIfNull]
        public Boolean? IsPublicAddressBook { get; set; }

        [DataMember]
        [BsonElement("last_fixup")]
        [BsonIgnoreIfNull]
        public DateTime? LastFixup { get; set; }

        [DataMember]
        [BsonElement("last_ft_indexed")]
        [BsonIgnoreIfNull]
        public DateTime? LastFTIndexed { get; set; }

        [DataMember]
        [BsonElement("percent_used")]
        [BsonIgnoreIfNull]
        public double? PercentUsed { get; set; }

        [DataMember]
        [BsonElement("details")]
        [BsonIgnoreIfNull]
        public string Details { get; set; }

        [DataMember]
        [BsonElement("last_modified")]
        [BsonIgnoreIfNull]
        public DateTime? LastModified { get; set; }

        [DataMember]
        [BsonElement("enabled_for_replication")]
        [BsonIgnoreIfNull]
        public Boolean? EnabledForReplication { get; set; }

        [DataMember]
        [BsonElement("is_mail_file")]
        [BsonIgnoreIfNull]
        public Boolean? IsMailFile { get; set; }

        [DataMember]
        [BsonElement("inbox_doc_count")]
        [BsonIgnoreIfNull]
        public int? InboxDocCount { get; set; }

        [DataMember]
        [BsonElement("q_place_bot_count")]
        [BsonIgnoreIfNull]
        public int? QPlaceBotCount { get; set; }

        [DataMember]
        [BsonElement("q_custom_form_count")]
        [BsonIgnoreIfNull]
        public int? QCustomFormCount { get; set; }

        [DataMember]
        [BsonElement("person_doc_ids")]
        [BsonIgnoreIfNull]
        public string PersonDocId { get; set; }

        [DataMember]
        [BsonElement("file_name_path")]
        [BsonIgnoreIfNull]
        public string FileNamePath { get; set; }

        [DataMember]
        [BsonElement("folder_count")]
        [BsonIgnoreIfNull]
        public int? FolderCount { get; set; }

        [DataMember]
        [BsonElement("temp")]
        [BsonIgnoreIfNull]
        public Boolean? Temp { get; set; }
    }
}




