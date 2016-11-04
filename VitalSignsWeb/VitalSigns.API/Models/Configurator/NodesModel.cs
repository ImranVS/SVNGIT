using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models.Configurator
{
    public class NodesModel
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

         
        [JsonProperty("name")]
        public string Name { get; set; }

         
        [JsonProperty("host_name")]       
        public string HostName { get; set; }

         
        [JsonProperty("alive")]         
        public bool IsAlive { get; set; }

        [JsonProperty("is_alive")]
        public string Alive { get; set; }


        [JsonProperty("node_type")]         
        public string NodeType { get; set; }

         
        [JsonProperty("load_factor")]         
        public double LoadFactor { get; set; }


         
        [JsonProperty("pulse")]        
        public DateTime Pulse { get; set; }

         
        [JsonProperty("is_primary")]        
        public bool IsPrimary { get; set; }

         
        [JsonProperty("location")]         
        public string Location { get; set; }

         
        [JsonProperty("is_configured_primary")]         
        public bool IsConfiguredPrimary { get; set; }

         
        [JsonProperty("is_disabled")]    
        public bool IsDisabled { get; set; }

         
        [JsonProperty("service_status_model")]       
        public List<ServiceStatusModel> ServiceStatusModel { get; set; }
    }

    public class ServiceStatusModel
    {
       
        [JsonProperty("name")]
        public string Name { get; set; }


        [JsonProperty("state")]
        public string State { get; set; }
    }
}
