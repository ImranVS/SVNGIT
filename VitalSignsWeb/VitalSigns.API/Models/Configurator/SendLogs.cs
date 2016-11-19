using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models.Configurator
{
    public class SendLogs
    {
        [JsonProperty("file_name")]
        public string FileName { get; set; }

        //[JsonProperty("is_selected")]
        //public bool IsSelected { get; set; }

        //[JsonProperty("log_file_name")]
        //public List<string> LogFileName { get; set; }

        //[JsonProperty("log_folders")]
        //public List<string> LogFolders { get; set; }


    }
    public class LogFolders
    {
        [JsonProperty("log_level")]
        public object LogLevel { get; set; }

        [JsonProperty("emailid")]
        public object Email { get; set; }

       

        [JsonProperty("log_name")]
        public List<object> LogName { get; set; }


    }


}
