﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class MailboxModel
    {
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("issue_warning_quota")]
        public string IssueWarningQuota { get; set; }

        [JsonProperty("prohibit_send_quota")]
        public string ProhibitSendQuota { get; set; }

        [JsonProperty("prohibit_send_receive_quota")]
        public string ProhibitSendReceiveQuota { get; set; }

        [JsonProperty("is_active")]
        public bool? IsActive { get; set; }

        [JsonProperty("total_item_size_mb")]
        public double? TotalItemSizeMb { get; set; }

        [JsonProperty("item_count")]
        public double? ItemCount { get; set; }

        [JsonProperty("last_logon_time")]
        public DateTime? LastLogonTime { get; set; }
    }
}