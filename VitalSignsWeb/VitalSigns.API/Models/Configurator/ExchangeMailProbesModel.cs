using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models.Configurator
{
    public class ExchangeMailProbesModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("is_enabled")]
        public bool? IsEnabled { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("scan_interval")]
        public int? ScanInterval { get; set; }

        [JsonProperty("off_hours_scan_interval")]
        public int? OffHoursInterval { get; set; }

        [JsonProperty("mailprobe_red_threshold")]
        public int? RedThreshold { get; set; }

        [JsonProperty("mailprobe_yellow_threshold")]
        public int? YellowThreshold { get; set; }

        [JsonProperty("selected_exchange_servers")]
        public List<string> SelectedExchangeServers { get; set; }
    }
    public class exchangemaillist
    {
        [JsonProperty("exchange_mail_probe")]
        public ExchangeMailProbesModel exchangemailprobe
        { get; set; }

        [JsonProperty("exchange_servers")]
        public List<ServerLocation> exchangeservers { get; set; }

    }
}
