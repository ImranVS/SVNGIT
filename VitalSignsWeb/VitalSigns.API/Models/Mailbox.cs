using System;
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

        [JsonProperty("database_name")]
        public string DatabaseName { get; set; }

        [JsonProperty("sam_account_name")]
        public string SAMAccountName { get; set; }

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

        [JsonProperty("prohibit_send_percentage")]
        public double? ProhibitedSendPercentage { get; set; }

        [JsonProperty("primary_smtp_address")]
        public string PrimarySmtpAddress { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("max_folder_size_mb")]
        public double? MaxFolderSizeMb { get; set; }

        [JsonProperty("folder_count")]
        public double? FolderCount { get; set; }

        [JsonProperty("mailbox_type", NullValueHandling=NullValueHandling.Ignore)]
        public string MailboxType { get; set; }

        [JsonProperty("days_since_last_logon", NullValueHandling = NullValueHandling.Ignore)]
        public double? DaysSinceLastLogon { get; set; }
        
        [JsonProperty("mailbox_forwarding_smtp_address")]
        public string MailboxForwardingSMTPAddress { get; set; }
        
        [JsonProperty("mailbox_forwarding_address")]
        public string MailboxForwardingAddress { get; set; }
        
        [JsonProperty("rule_forward_to")]
        public List<string> RuleForwardTo { get; set; }
        
        [JsonProperty("rule_forward_as_attachment_to")]
        public List<string> RuleForwardAsAttachmentTo { get; set; }
        
        [JsonProperty("deliver_to_mailbox_and_forward")]
        public Boolean DeliverToMailboxAndForward { get; set; }

        [JsonProperty("retention_policy")]
        public string RetentionPolicy { get; set; }

        [JsonProperty("litigation_hold_enabled")]
        public Boolean? LitigationHoldEnabled { get; set; }

        [JsonProperty("recipient_type_details")]
        public string RecipientTypeDetails { get; set; }

        [JsonProperty("distinguished_name")]
        public string DistinguishedName { get; set; }
        
    }
}
