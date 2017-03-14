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
        [BsonElement("version")]
        [BsonIgnoreIfNull]
        public string Version { get; set; }


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
        //[BsonRepresentation(BsonType.String)]
        public List<CollectionReset> CollectionResets { get; set; }

        [DataMember]
        [BsonElement("domino_thread_killed_count")]
        [BsonIgnoreIfNull]
        public int? DominoThreadKilledCount { get; set; }

        [DataMember]
        [BsonElement("service_status")]
        [BsonIgnoreIfNull]
        public List<ServiceStatus> ServiceStatus { get; set; }

        [DataMember]
        [BsonElement("assembly_info")]
        [BsonIgnoreIfNull]
        public List<AssemblyInfo> AssemblyInfo { get; set; }
    }

    public class CollectionReset
    {
        [DataMember]
        [BsonElement("device_type")]
        [BsonIgnoreIfNull]
        public string DeviceType { get; set; }

        [DataMember]
        [BsonElement("date_queued")]
        [BsonIgnoreIfNull]
        public DateTime? DateQueued { get; set; }

        [DataMember]
        [BsonElement("date_cleared")]
        [BsonIgnoreIfNull]
        public DateTime? DateCleared { get; set; }

        [DataMember]
        [BsonElement("reset")]
        [BsonIgnoreIfNull]
        public Boolean? Reset { get; set; }
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

    public class AssemblyInfo
    {
        [DataMember]
        [BsonElement("name")]
        [BsonIgnoreIfNull]
        public string AssemblyName { get; set; }

        [DataMember]
        [BsonElement("name")]
        [BsonIgnoreIfNull]
        public string AssemblyVersion { get; set; }

        [DataMember]
        [BsonElement("name")]
        [BsonIgnoreIfNull]
        public string ProductVersion { get; set; }

        [DataMember]
        [BsonElement("name")]
        [BsonIgnoreIfNull]
        public DateTime? BuildDate { get; set; }

        [DataMember]
        [BsonElement("name")]
        [BsonIgnoreIfNull]
        public string FileArea { get; set; }

    }
}
