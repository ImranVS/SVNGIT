using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models.Configurator
{
    public class NotesMailProbesModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("is_enabled")]
        public bool? IsEnabled { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("threshold")]
        public int? Threshold { get; set; }

        [JsonProperty("source_server")]
        public string SourceServer { get; set; }

        [JsonProperty("destination_server")]
        public string DestinationServer { get; set; }

        [JsonProperty("send_to")]
        public string SendTo { get; set; }

        [JsonProperty("scan_interval")]
        public int? ScanInterval { get; set; }

        [JsonProperty("off_hours_interval")]
        public int? OffHoursInterval { get; set; }

        [JsonProperty("retry_interval")]
        public int? RetryInterval { get; set; }

        [JsonProperty("echo_service")]
        public bool? EchoService { get; set; }

        [JsonProperty("reply_to")]
        public string ReplyTo { get; set; }

        [JsonProperty("destination_database")]
        public string DestinationDatabase { get; set; }

        [JsonProperty("use_imap")]
        public bool UseImap { get; set; }

        [JsonProperty("imap_host_name")]
        public string ImapHostName { get; set; }

        [JsonProperty("credentials_id")]
        public string CredentialsId { get; set; }
    }
}
