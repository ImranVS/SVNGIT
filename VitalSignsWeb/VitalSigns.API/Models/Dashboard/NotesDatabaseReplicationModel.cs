using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class NotesDatabaseReplicationModel
    {
        [JsonProperty("cluster_name")]
        public string ClusterName { get; set; }

        [JsonProperty("database_name")]
        public string DatabaseName { get; set; }

        [JsonProperty("replica_id")]
        public string ReplicaId { get; set; }

        [JsonProperty("database_status")]
        public string DatabaseStatus { get; set; }

        [JsonProperty("domino_server_a")]
        public string DominoServerA { get; set; }

        [JsonProperty("domino_server_b")]
        public string DominoServerB { get; set; }

        [JsonProperty("document_count_a")]
        public int? DocumentCountA { get; set; }

        [JsonProperty("document_count_b")]
        public int? DocumentCountB { get; set; }

        [JsonProperty("database_size_a")]
        public double? DatabaseSizeA { get; set; }

        [JsonProperty("database_size_b")]
        public double? DatabaseSizeB { get; set; }
    }
}
