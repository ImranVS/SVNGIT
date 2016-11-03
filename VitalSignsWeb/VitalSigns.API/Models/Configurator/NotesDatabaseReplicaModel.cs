using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class NotesDatabaseReplicaModel
    {
        [JsonProperty("primary_host_name")]
        public string PrimaryHostName { get; set; }

        #region NotesDatabaseReplica

       
        [JsonProperty("difference_threshold")]
        public int? DifferenceThreshold { get; set; }

       
        [JsonProperty("domino_server_a")]
        public string DominoServerA { get; set; }

       
        [JsonProperty("domino_server_a_file_mask")]
        public string DominoServerAFileMask { get; set; }

        [JsonProperty("domino_server_a_exclude_folders")]
        public string DominoServerAExcludeFolders { get; set; }

    
        [JsonProperty("domino_server_b")]
        public string DominoServerB { get; set; }

      
        [JsonProperty("domino_server_b_file_mask")]
        public string DominoServerBFileMask { get; set; }

       
        [JsonProperty("domino_server_b_exclude_folders")]
        public string DominoServerBExcludeFolders { get; set; }

       
        [JsonProperty("domino_server_c")]
        public string DominoServerC { get; set; }

       
        [JsonProperty("domino_server_c_file_mask")]
        public string DominoServerCFileMask { get; set; }

      
        [JsonProperty("domino_server_c_exclude_folders")]
        public string DominoServerCExcludeFolders { get; set; }

        
        [JsonProperty("first_alert_threshold")]
        public int? FirstAlertThreshold { get; set; }

        [JsonProperty("device_name")]
        public string  DeviceName{ get; set; }
        
       [JsonProperty("is_enabled")]
        public bool? IsEnabled { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("scan_interval")]
        public int? ScanInterval { get; set; }

        [JsonProperty("off_hours_scan_interval")]
        public int? OffHoursScanInterval { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("device_type")]
        public string DeviceType { get; set; }
        #endregion

    }
}
