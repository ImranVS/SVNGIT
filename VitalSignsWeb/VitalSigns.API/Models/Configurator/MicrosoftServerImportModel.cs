using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace VitalSigns.API.Models.Configurator
{
    public class MicrosoftServerImportModel
    {
        public MicrosoftServerImportModel()
        {
            DeviceAttributes = new List<DeviceAttributesModel>();
            Servers = new List<ServersModel>();
        }


        //Step1
        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("servers")]
        public List<ServersModel> Servers { get; set; }



        //Step2
        [JsonProperty("device_attributes")]
        public List<DeviceAttributesModel> DeviceAttributes { get; set; }

        [JsonProperty("memory_threshold")]
        public double? MemoryThreshold { get; set; }

        [JsonProperty("cpu_threshold")]
        public double? CpuThreshold { get; set; }


        [JsonProperty("scan_interval")]
        public int? ScanInterval { get; set; }


        [JsonProperty("retry_interval")]
        public int? RetryInterval { get; set; }


        [JsonProperty("off_hours_scan_interval")]
        public int? OffHoursScanInterval { get; set; }


        [JsonProperty("response_time")]
        public int? ResponseTime { get; set; }

        [JsonProperty("authentication_type")]
        public string AuthenticationType { get; set; }

        [JsonProperty("protocol")]
        public string Protocol { get; set; }

        [JsonProperty("device_type")]
        public string DeviceType { get; set; }

        [JsonProperty("ip_address")]
        public string IpAddress { get; set; }

        [JsonProperty("credential_id")]
        public string CredentialId { get; set; }

    }
}
