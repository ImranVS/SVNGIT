using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models
{
    public class DeviceAttributesDataModel
    {
        public DeviceAttributesDataModel()
        {
            DeviceAttributes = new List<DeviceAttributesModel>();
        }

        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("location_id")]
        public string LocationId { get; set; }

       [JsonProperty("ip_address")]
        public string IPAddress { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("device_type")]
        public string Devicetype { get; set; }


        [JsonProperty("is_enabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("device_attributes")]
        public List<DeviceAttributesModel> DeviceAttributes { get; set; }
    }
    public class DeviceAttributesModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("attribute_name")]       
        public string AttributeName { get; set; }

        [JsonProperty("default_value")]       
        public string DefaultValue { get; set; }

        [JsonProperty("device_type")]
        public string DeviceType { get; set; }


        [JsonProperty("field_name")]       
        public string FieldName { get; set; }

        [JsonProperty("unit_of_measurement")]       
        public string Unitofmeasurement { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("is_selected")]
        public bool IsSelected { get; set; }
    }

    public class DeviceAttributeValue
    {  
        [JsonProperty("field_name")]
        public string FieldName { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
      
    }
}
