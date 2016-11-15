using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace VitalSigns.API.Models.Configurator
{
    public class DominoServerImportModel
    {
        public DominoServerImportModel()
        {
            DeviceAttributes = new List<DeviceAttributesModel>();
            ServerTasks = new List<DominoServerTasksModel>();
            Servers = new List<ServersModel>();
        }


        //Step1
        [JsonProperty("domino_server")]
        public string DominoServer { get; set; }
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

        //Step3
        [JsonProperty("server_tasks")]
        public List<DominoServerTasksModel> ServerTasks { get; set; }
    }
}
