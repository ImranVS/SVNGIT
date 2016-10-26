using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class AdvancedSettingsModel
    {


        [JsonProperty("memory_threshold")]
        public int? MemoryThreshold { get; set; }

        [JsonProperty("cpu_threshold")]
        public int? CpuThreshold { get; set; }

        [JsonProperty("server_days_alert")]
        public int? ServerDaysAlert { get; set; }

        [JsonProperty("cluster_replication_delay_threshold")]
        public int? ClusterReplicationDelayThreshold { get; set; }



        [JsonProperty("cluster_replication_queue_threshold")]
        public int? ClusterReplicationQueueThreshold { get; set; }

        [JsonProperty("proxy_server_type")]
        public string ProxyServerType { get; set; }

        [JsonProperty("proxy_server_protocol")]
        public string ProxyServerprotocol { get; set; }

        [JsonProperty("dbms_host_name")]
        public string DbmsHostName { get; set; }


        [JsonProperty("dbms_name")]
        public string DbmsName { get; set; }

        [JsonProperty("dbms_port")]
        public int? DbmsPort { get; set; }

        [JsonProperty("db2_settings_credentials_id")]
        public string Db2SettingsCredentialsId { get; set; }

        [JsonProperty("collect_extended_statistics")]
        public Boolean? CollectExtendedStatistics { get; set; }

        [JsonProperty("collect_meeting_statistics")]
        public Boolean? CollectMeetingStatistics { get; set; }

        [JsonProperty("collect_conference_statistics")]
        public Boolean? CollectConferenceStatistics { get; set; }

        [JsonProperty("extended_statistics_port")]
        public int? ExtendedStatisticsPort { get; set; }

        [JsonProperty("meeting_host_name")]
        public string MeetingHostName { get; set; }

        [JsonProperty("meeting_port")]
        public int? MeetingPort { get; set; }

        [JsonProperty("meeting_require_ssl")]
        public Boolean? MeetingRequireSSL { get; set; }

        [JsonProperty("conference_host_name")]
        public string ConferenceHostName { get; set; }

        [JsonProperty("conference_port")]
        public int? ConferencePort { get; set; }

        [JsonProperty("conference_require_ssl")]
        public Boolean? ConferenceRequireSSL { get; set; }


        [JsonProperty("database_settings_host_name")]
        public string DatabaseSettingsHostName { get; set; }

        [JsonProperty("database_settings_credentials_id")]
        public string DatabaseSettingsCredentialsId { get; set; }

        [JsonProperty("database_settings_port")]
        public int? DatabaseSettingsPort { get; set; }

        [JsonProperty("device_type")]
        public string DeviceType { get; set; }
    }
}
