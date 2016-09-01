using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class ServerDatabase
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("server_name")]
        public string ServerName { get; set; }

        [JsonProperty("folder")]
        public string Folder { get; set; }

        [JsonProperty("folder_count")]
        public int? FolderCount { get; set; }

        [JsonProperty("details")]
        public string Details { get; set; }

        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("design_template_name")]
        public string DesignTemplateName { get; set; }

        [JsonProperty("file_size")]
        public int? FileSize { get; set; }

        [JsonProperty("quota")]
        public int? Quota { get; set; }

        [JsonProperty("inbox_doc_count")]
        public int? InboxDocCount { get; set; }

        [JsonProperty("scan_date")]
        public DateTime? ScanDateTime { get; set; }

        [JsonProperty("replica_idss")]
        public string ReplicaId { get; set; }

        [JsonProperty("document_count")]
        public int? DocumentCount { get; set; }

        [JsonProperty("categories")]
        public string Categories { get; set; }
    }
}
