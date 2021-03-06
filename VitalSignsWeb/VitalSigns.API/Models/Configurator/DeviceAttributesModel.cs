﻿using Newtonsoft.Json;
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

        [JsonProperty("cell_name")]
        public string CellName { get; set; }

        [JsonProperty("node_name")]
        public string NodeName { get; set; }

        [JsonProperty("cell_id")]
        public string CellId { get; set; }

        [JsonProperty("node_id")]
        public string NodeId { get; set; }

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

        [JsonProperty("ibm_file_host_name")]
        public string HostName { get; set; }

        [JsonProperty("ibm_file_port_name")]
        public string PortName { get; set; }

        [JsonProperty("device_type")]
        public string Devicetype { get; set; }

        [JsonProperty("credentials_id")]
        public string CredentialsId { get; set; }

        [JsonProperty("require_ssl")]
        public bool? RequireSSL { get; set; }


        [JsonProperty("is_enabled")]
        public bool? IsEnabled { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("device_attributes")]
        public List<DeviceAttributesModel> DeviceAttributes { get; set; }

        [JsonProperty("authentication_type")]
        public string AuthenticationType { get; set; }
    }
    public class DeviceAttributesModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("attribute_name")]
        public string AttributeName { get; set; }

        [JsonProperty("default_value")]
        public string DefaultValue { get; set; }

        [JsonProperty("default_bool_values")]
        public bool DefaultboolValues { get; set; }

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

        [JsonProperty("datatype")]
        public string DataType { get; set; }

        [JsonProperty("is_selected")]
        public bool IsSelected { get; set; }

        [JsonProperty("is_percentage")]
        public bool IsPercentage { get; set; }
    }

    public class DeviceAttributeValue
    {
        [JsonProperty("field_name")]
        public string FieldName { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("datatype")]
        public string DataType { get; set; }

        [JsonProperty("default_bool_values")]
        public bool DefaultboolValues { get; set; }

    }
}
