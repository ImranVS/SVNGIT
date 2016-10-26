using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models
{
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

        [JsonProperty("catogory")]
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
