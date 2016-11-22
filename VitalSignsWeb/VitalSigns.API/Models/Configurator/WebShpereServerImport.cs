﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace VitalSigns.API.Models
{
    public class WebShpereServerImport
    {
        
        [JsonProperty("cells_data")]
        public List<CellInfo> CellsData { get; set; }

        [JsonProperty("device_attributes")]
        public List<DeviceAttributesModel> DeviceAttributes { get; set; }
        [JsonProperty("credentials_list")]
        public List<ComboBoxListItem> CredentialsData { get; set; }

    }

    public class CellInfo
    {
        [JsonProperty("id")]
        public string DeviceId { get; set; }

        [JsonProperty("cell_id")]
        public string CellId { get; set; }

        [JsonProperty("cell_name")]
        public string CellName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }


        [JsonProperty("host_name")]
        public string HostName { get; set; }


        [JsonProperty("connection_type")]
        public string ConnectionType { get; set; }

        [JsonProperty("port_no")]
        public int? PortNo { get; set; }

        [JsonProperty("global_security")]
        public bool GlobalSecurity { get; set; }

        [JsonProperty("credentials_id")]
        public string CredentialsId { get; set; }

        [JsonProperty("credentials_name")]
        public string CredentialsName { get; set; }

        [JsonProperty("realm")]
        public string Realm { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

    }

    #region Get_Server_List

    [XmlRoot(ElementName = "servers")]
    public class Servers
    {
        [XmlElement(ElementName = "server")]
        public List<string> Server { get; set; }
    }

    [XmlRoot(ElementName = "node")]
    public class Node
    {
        [XmlElement(ElementName = "servers")]
        public Servers Servers { get; set; }
        [XmlAttribute(AttributeName = "hostName")]
        public string HostName { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "nodes")]
    public class NodeList
    {
        [XmlElement(ElementName = "node")]
        public List<Node> Node { get; set; }
    }

    [XmlRoot(ElementName = "cell")]
    public class Cell
    {
        [XmlElement(ElementName = "nodes")]
        public NodeList Nodes { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "cells")]
    public class Cells
    {
        [XmlElement(ElementName = "cell")]
        public List<Cell> Cell { get; set; }
        [XmlElement(ElementName = "timestamp")]
        public string TimeStamp { get; set; }
        [XmlElement(ElementName = "connection-status")]
        public string Connection_Status { get; set; }
    }

    #endregion

}
