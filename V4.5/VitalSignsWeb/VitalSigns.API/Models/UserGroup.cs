using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class UserGroupModel
    {
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("identity")]
        public string Identity { get; set; }

        [JsonProperty("total_mailboxes_sizes_mb")]
        public double? TotalMailBoxesSizes { get; set; }

        [JsonProperty("mailboxes")]
        public List<MailBoxes> Mailboxes { get; set; }


        public class MailBoxes
        {
            [JsonProperty("display_name")]
            public string DisplayName { get; set; }

            [JsonProperty("mailbox_size_gb")]
            public double? MailboxSizeGb { get; set; }
        }
    }
}
