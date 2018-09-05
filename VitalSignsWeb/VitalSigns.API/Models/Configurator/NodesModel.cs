using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models.Configurator
{
    public class NodesModel
    {
        [JsonProperty("id")]
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

        [JsonProperty("nodes_services")]
        public List<NodesServices> Nodeservices { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }

    public class ServiceStatusModel
    {
       
        [JsonProperty("name")]
        public string Name { get; set; }


        [JsonProperty("state")]
        public string State { get; set; }
    }

    public class NodesServices
    {

        [JsonProperty("VSService_Domino")]
        public string VSServicDomino { get; set; }


        [JsonProperty("VSService_Core")]
        public string VSServiceCore { get; set; }

        [JsonProperty("VSService_Alerting")]
        public string VSServiceAlerting { get; set; }

        [JsonProperty("VSService_Cluster_Health")]
        public string VSServiceCluster { get; set; }

        [JsonProperty("VSService_Daily_Service")]
        public string VSService_Daily { get; set; }

        [JsonProperty("VSService_Master_Service")]
        public string VSService_Master { get; set; }

        [JsonProperty("VSService_DB_Health")]
        public string VSService_DB { get; set; }

        [JsonProperty("VSService_EX_Journal")]
        public string VSService_EX { get; set; }

        [JsonProperty("VSService_Console_Commands")]
        public string VSService_Console { get; set; }

        [JsonProperty("VSService_Microsoft")]
        public string VSService_Microsoft { get; set; }

        [JsonProperty("VSService_Core_64")]
        public string VSService_Core64 { get; set; }

    }
}
