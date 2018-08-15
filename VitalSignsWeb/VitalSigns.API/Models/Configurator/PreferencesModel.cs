using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models.Configurator
{
    public class PreferencesModel
    {
        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("currency_symbol")]
        public string CurrencySymbol { get; set; }

        [JsonProperty("monitoring_delay")]
        public int MonitoringDelay { get; set; }

        [JsonProperty("threshold_show")]
        public int ThresholdShow { get; set; }

        [JsonProperty("dashboardonly_exec_summary_buttons")]
        public bool DashboardonlyExecSummaryButtons { get; set; }

        [JsonProperty("bing_key")]
        public string BingKey { get; set; }

        [JsonProperty("purge_intreval")]
        public string PurgeInterval { get; set; }

        [JsonProperty("ad_enabled")]
        public bool ADEnabled { get; set; }

        [JsonProperty("ad_url")]
        public string ADUrl { get; set; }

        [JsonProperty("ad_login_id")]
        public string ADLoginId { get; set; }

        [JsonProperty("ad_password")]
        public string ADPassword { get; set; }


    }
}
