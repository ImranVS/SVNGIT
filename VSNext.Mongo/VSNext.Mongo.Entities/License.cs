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
    [CollectionName("License")]
    public class License : Entity
    {
        [DataMember]
        [BsonElement("license_key")]
        [BsonIgnoreIfNull]
        public string LicenseKey { get; set; }

        [DataMember]
        [BsonElement("units")]
        [BsonIgnoreIfNull]
        public int units { get; set; }

        [DataMember]
        [BsonElement("install_type")]
        [BsonIgnoreIfNull]
        public string InstallType { get; set; }

        [DataMember]
        [BsonElement("company_name")]
        [BsonIgnoreIfNull]
        public string CompanyName { get; set; }

        [DataMember]
        [BsonElement("license_type")]
        [BsonIgnoreIfNull]
        public string LicenseType { get; set; }

        [DataMember]
        [BsonElement("expiration_date")]
        [BsonIgnoreIfNull]
        public DateTime  ExpirationDate { get; set; }

        [DataMember]
        [BsonElement("enc_units")]
        [BsonIgnoreIfNull]
        public string EncUnits { get; set; }

    }

    //[DataContract]
    //[Serializable]
    //[CollectionName("device_type_license")]
    //public class DeviceTypeLicense : Entity
    //{
    //    [DataMember]
    //    [BsonElement("device_type")]
    //    [BsonIgnoreIfNull]
    //    public string DeviceType { get; set; }

    //    [DataMember]
    //    [BsonElement("unit_cost")]
    //    [BsonIgnoreIfNull]
    //    public double UnitCost { get; set; }

    //    [DataMember]
    //    [BsonElement("enc_unit_cost")]
    //    [BsonIgnoreIfNull]
    //    public string EncUnitCost { get; set; }

    //}

    [DataContract]
    [Serializable]
    [CollectionName("nodes")]
    public class Nodes : Entity
    {
        [DataMember]
        [BsonElement("name")]
        [BsonIgnoreIfNull]
        public string Name { get; set; }

        [DataMember]
        [BsonElement("host_name")]
        [BsonIgnoreIfNull]
        public string HostName { get; set; }

        [DataMember]
        [BsonElement("alive")]
        [BsonIgnoreIfNull]
        public bool  IsAlive { get; set; }

        [DataMember]
        [BsonElement("node_type")]
        [BsonIgnoreIfNull]
        public string NodeType { get; set; }

        [DataMember]
        [BsonElement("load_factor")]
        [BsonIgnoreIfNull]
        public double LoadFactor { get; set; }


        [DataMember]
        [BsonElement("pulse")]
        [BsonIgnoreIfNull]
        public DateTime Pulse { get; set; }

        [DataMember]
        [BsonElement("is_primary")]
        [BsonIgnoreIfNull]
        public bool IsPrimary { get; set; }

        [DataMember]
        [BsonElement("location")]
        [BsonIgnoreIfNull]
        public string Location { get; set; }

        [DataMember]
        [BsonElement("is_configured_primary")]
        [BsonIgnoreIfNull]
        public bool IsConfiguredPrimary { get; set; }

        [DataMember]
        [BsonElement("is_disabled")]
        [BsonIgnoreIfNull]
        public bool IsDisabled { get; set; }

        [DataMember]
        [BsonElement("collection_resets")]
        [BsonIgnoreIfNull]
        public List<Enums.ServerType> CollectionResets { get; set; }

        [DataMember]
        [BsonElement("domino_thread_killed_count")]
        [BsonIgnoreIfNull]
        public int? DominoThreadKilledCount { get; set; }

        [DataMember]
        [BsonElement("service_status")]
        [BsonIgnoreIfNull]
        public List<ServiceStatus> ServiceStatus { get; set; }
    }

    public class ServiceStatus
    {
        [DataMember]
        [BsonElement("name")]
        [BsonIgnoreIfNull]
        public string Name { get; set; }

        [DataMember]
        [BsonElement("state")]
        [BsonIgnoreIfNull]
        public string State { get; set; }
    }
    [CollectionName("traveler_summarystats")]
    public class TravelerStatusSummary:Entity
    {
        // [JsonProperty("[000-001]")]
        [DataMember]
        [BsonElement("[000-001]")]
        [BsonIgnoreIfNull]
        public int? c_000_001 { get; set; }

        [DataMember]
        [BsonElement("[001-002]")]
        [BsonIgnoreIfNull]
        public int? c_001_002 { get; set; }

        [DataMember]
        [BsonElement("[002-005]")]
        [BsonIgnoreIfNull]
        public int? c_002_005 { get; set; }

        [DataMember]
        [BsonElement("[005-010]")]
        [BsonIgnoreIfNull]
        public int? c_005_010 { get; set; }

        [DataMember]
        [BsonElement("[010-030]")]
        [BsonIgnoreIfNull]
        public int? c_010_030 { get; set; }
        [DataMember]
        [BsonElement("[030-060]")]
        [BsonIgnoreIfNull]
        public int? c_030_060 { get; set; }
        [DataMember]
       [BsonElement("[060-120]")]
        [BsonIgnoreIfNull]
        public int? c_060_120 { get; set; }
        [DataMember]
        [BsonElement("[120-INF]")]
        [BsonIgnoreIfNull]
        public int? c_120_INF { get; set; }


        [DataMember]
        [BsonElement("traveler_server_name")]
        [BsonIgnoreIfNullAttribute]
        public string TravelerServerName { get; set; }

        [DataMember]
        [BsonElement("mail_server_name")]
        [BsonIgnoreIfNullAttribute]
        public string MailServerName { get; set; }

        [DataMember]
        [BsonElement("interval")]
        [BsonIgnoreIfNullAttribute]
        public string Interval { get; set; }

        [DataMember]
        [BsonElement("delta")]
        [BsonIgnoreIfNullAttribute]
        public int? Delta { get; set; }

        [DataMember]
        [BsonElement("open_times")]
        [BsonIgnoreIfNullAttribute]
        public int? OpenTimes { get; set; }

        [DataMember]
        [BsonElement("date_updated")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? DateUpdated { get; set; }


        [DataMember]
        [BsonElement("device_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfNullAttribute]
        public string DeviceId { get; set; }


        [DataMember]
        [BsonElement("stat_name")]
        [BsonIgnoreIfNullAttribute]
        public string StatName { get; set; }

    }
}
