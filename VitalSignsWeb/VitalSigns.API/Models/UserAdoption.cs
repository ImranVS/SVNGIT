using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class UserAdoption
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("server_name")]
        public string ServerName { get; set; }

        [JsonProperty("object_name")]
        public string ObjectName { get; set; }

        [JsonProperty("object_value")]
        public int ObjectValue { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("object_created_date")]
        public DateTime ObjectCreatedDate { get; set; }
    }

    public class UserAdoptionPivot
    {
        [JsonProperty("server_name")]
        public string ServerName { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("object_value")]
        public int ObjectValue { get; set; }

        [JsonProperty("object_type")]
        public string ObjectType { get; set; }

        [JsonProperty("object_values")]
        public List<int> ObjectValues { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("object_created_date")]
        public DateTime ObjectCreatedDate { get; set; }
    }

    public class UserList
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class CommunityUserList
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("community")]
        public string Community { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class UserComparison
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
    }

    public class UserActivityBubble
    {
        [JsonProperty("x")]
        public int X { get; set; }

        [JsonProperty("y")]
        public int Y { get; set; }

        [JsonProperty("z")]
        public int Z { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("activity")]
        public string Activity { get; set; }
    }
}
