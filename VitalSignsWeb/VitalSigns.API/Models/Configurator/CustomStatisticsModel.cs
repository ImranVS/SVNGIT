﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models.Configurator
{
    public class CustomStatisticsModel
    {
        [JsonProperty ("id")]
        public string Id { get; set; }

        [JsonProperty("domino_servers")]
        public List<string> DominoServers { get; set; }

        [JsonProperty ("stat_name")]
        public String StatName { get; set; }

        [JsonProperty("yellow_threshold")]
        public String ThresholdValue { get; set; }

        [JsonProperty("greater_than_or_less_than")]
        public String GreaterThanOrLessThan { get; set; }

        [JsonProperty("times_in_a_row")]
        public int? TimesInARow { get; set; }

        [JsonProperty("console_command")]
        public String ConsoleCommand { get; set; }

    }
}