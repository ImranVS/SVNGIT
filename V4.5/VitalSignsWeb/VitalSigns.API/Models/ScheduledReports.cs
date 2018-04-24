using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class ScheduledReportsModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("report_name")]
        public string ReportName { get; set; }


        [JsonProperty("report_subject")]
        public string ReportSubject { get; set; }


        [JsonProperty("report_body")]
        public string ReportBody { get; set; }

        [JsonProperty("report_frequency")]
        public string Frequency { get; set; }

        [JsonProperty("send_to")]
        public string SendTo { get; set; }

        [JsonProperty("copy_to")]
        public string CopyTo { get; set; }

        [JsonProperty("blind_copy_to")]
        public string BlindCopyTo { get; set; }

        [JsonProperty("file_format")]
        public string FileFormat { get; set; }

        [JsonProperty("frequency_days_list")]
        public List<string> FrequencyDayList { get; set; }

        //[JsonProperty("report_frequency_value")]
        //public string FrequencyeportValue { get; set; }

        [JsonProperty("repeat")]
        public string Repeat { get; set; }

        [JsonProperty("selected_reports")]
        public List<String> SelectedReports { get; set; }

    }

    public class ReportTitle
    {
        //[JsonProperty("id")]
        //public string Id { get; set; }

        [JsonProperty("is_selected")]
        public bool IsSelected { get; set; }

        [JsonProperty("report_title")]
        public string ReportTitles { get; set; }

        [JsonProperty("report_category")]
        public string ReportCategory { get; set; }

       
    }
}
