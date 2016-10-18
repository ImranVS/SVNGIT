using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models
{
    public class SelectedDiksModel
    {
        [JsonProperty("is_selected")]
        public bool IsSelected { get; set; }

        [JsonProperty("disk_name")]
        public string DiskName { get; set; }

        [JsonProperty("freespace_threshold")]
        public string FreespaceThreshold { get; set; }

        [JsonProperty("threshold_type")]       
        public string ThresholdType { get; set; }

        [JsonProperty("is_load")]
        public bool IsLoad { get; set; }

        


    }
    
}
