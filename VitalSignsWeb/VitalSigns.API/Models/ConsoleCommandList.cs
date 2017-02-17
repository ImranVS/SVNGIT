using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class ConsoleCommandList
    {
        [JsonProperty("server_name")]
        public string ServerName { get; set; }

        [JsonProperty("command")]
        public string Command { get; set; }

        [JsonProperty("submitter")]
        public string Submitter { get; set; }

        [JsonProperty("result")]
        public string Result { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }


    }

    public class DatabaseInventoryList
    {
        [JsonProperty("device_name")]
        public string Server { get; set; }

        [JsonProperty("folder")]
        public string Folder { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("file_name_path")]
        public string FileNamePath { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("design_template_name")]
        public string DesignTemplateName { get; set; }

        [JsonProperty("is_mail_file")]
        public bool? IsMailFile { get; set; }

    }

    public class LogFileList
    {
        [JsonProperty("key_word")]
        public string Keyword { get; set; }

        [JsonProperty("repeat_once")]
        public Boolean? RepeatOnce { get; set; }

        [JsonProperty("not_required_keyword")]
        public string NotRequiredKeyword { get; set; }

        [JsonProperty("log")]
        public Boolean? Log { get; set; }

        [JsonProperty("agent_log")]
        public Boolean? AgentLog { get; set; }

        [JsonProperty("domino_event_log_id")]
        public int DominoEventLogId { get; set; }


    }

    public class MailThresholdList
    {
        [JsonProperty("device_name")]
        public string ServerName { get; set; }

        [JsonProperty("dead_mail_threshold")]
        public int? DeadMailThreshold { get; set; }

        [JsonProperty("pending_mail_threshold")]
        public int? PendingMailThreshold { get; set; }

        [JsonProperty("held_mail_threshold")]
        public int? HeldMailThreshold { get; set; }


    }

    public class NotesDatabaseList
    {
        [JsonProperty("device_name")]
        public string ServerName { get; set; }

        [JsonProperty("database_file_name")]
        public string DatabaseFileName { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("scan_interval")]
        public int? ScanInterval { get; set; }

        [JsonProperty("off_hours_scan_interval")]
        public int? OffHoursScanInterval { get; set; }

        [JsonProperty("response_time")]
        public int? ResponseTime { get; set; }

        [JsonProperty("retry_interval")]
        public int? RetryInterval { get; set; }

    }


    public class DominoServerTasksList
    {
        [JsonProperty("task_name")]
        public string TaskName { get; set; }

        [JsonProperty("freeze_detect")]
        public bool FreezeDetect { get; set; }

        [JsonProperty("retry_count")]
        public int RetryCount { get; set; }

        [JsonProperty("status_summary")]
        public string StatusSummary { get; set; }

        [JsonProperty("max_busy_time")]
        public int MaxBusyTime { get; set; }

        [JsonProperty("load_string")]
        public string LoadString { get; set; }

        [JsonProperty("enabled")]
        public bool? Enabled { get; set; }

        [JsonProperty("send_load_cmd")]
        public bool? SendLoadCmd { get; set; }

        [JsonProperty("console_string")]
        public string ConsoleString { get; set; }

        [JsonProperty("send_exit_cmd")]
        public string SendExitCmd { get; set; }

        [JsonProperty("send_restart_cmd_offhours")]
        public bool? SendRestartCmdOffHours { get; set; }

        [JsonProperty("idle_string")]
        public string IdleString { get; set; }


    }

    public class IBMConnCommunityUsersList
    {
        [JsonProperty("device_name")]
        public string ServerName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("user")]
        public List<string> users { get; set; }

        [JsonProperty("object_user")]
        public string user { get; set; }

        [JsonProperty("community_type")]
        public string CommunityType { get; set; }

        [JsonProperty("num_of_owners")]
        public int? NumOfOwners { get; set; }

        [JsonProperty("num_of_members")]
        public int NumOfMembers { get; set; }

        [JsonProperty("num_of_followers")]
        public int NumOfFollowers { get; set; }

    }
}
